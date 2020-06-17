using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Config;
using MessageBus.Message;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Configuration;
using OrderApi.Data;
using OrderApi.Filter;
using OrderApi.Models;
using Stripe;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class OrderController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly OrderContext _context;
        private readonly ITokenConfiguration _token;
        public OrderController(ISendEndpointProvider sendEndpointProvider, OrderContext context, ITokenConfiguration token)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _token = token;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder([FromQuery] OrderFilter filter)
        {
            var orderSource = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

            // var order = orderSource;
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var order = orderSource.Where(o => o.BuyerId == user.UserId).ToList();

            if (filter.Status != 0)
            {
                order = order.Where(o => o.Status == filter.Status).ToList();
            }

            var totalAllPrice = 0;

            foreach (var item in order)
            {
                if (item.Status != 4)
                    totalAllPrice += item.Total;
            }

            return Ok(new { success = true, filter = filter, data = order, totalAllPrice = totalAllPrice });
        }

        [HttpGet("shop/{userId}")]
        public async Task<IActionResult> GetOrderUser(string userId, [FromQuery] OrderFilter filter)
        {
            var orderSource = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

            // var order = orderSource;
            // string access_token = await HttpContext.GetTokenAsync("access_token");

            // var user = _token.GetPayloadAsync(access_token);

            var order = orderSource.Where(o => o.UserId == userId).ToList();

            if (filter.Status != 0)
            {
                order = order.Where(o => o.Status == filter.Status).ToList();
            }

            var totalAllPrice = 0;

            foreach (var item in order)
            {
                if (item.Status != 4)
                    totalAllPrice += item.Total;
            }

            return Ok(new { success = true, filter = filter, data = order, totalAllPrice = totalAllPrice });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderBy(int id)
        {
            var orderItem = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == id).SingleOrDefaultAsync();
            if (orderItem != null)
            {
                return Ok(new { success = true, data = orderItem });
            }
            return NotFound(new { success = false, message = "Order item is not found" });

        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Models.Order order)
        {
            if (order.OrderItems.Count() == 0)
                return BadRequest(new { success = false, message = "OrderItem Is Null" });

            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var temp = order.OrderItems;

            var result = temp.GroupBy(t => t.UserId)
                             .Select(g => new Models.Order()
                             {
                                 Address = order.Address,
                                 Fullname = order.Fullname,
                                 PhoneNumber = order.PhoneNumber,
                                 PaymentIntent = order.PaymentIntent,
                                 BuyerId = user.UserId,
                                 UserId = g.Key,
                                 Status = 1,
                                 OrderItems = g.ToList()
                             });

            await _context.AddRangeAsync(result);
            await _context.SaveChangesAsync();

            var endPoint = await _sendEndpointProvider.
                                    GetSendEndpoint(new Uri("queue:" + BusConstant.AddAmountQueue));

            foreach (var item in temp)
            {
                await endPoint.Send<AddAmountMessage>(new
                {
                    BookId = item.BookId,
                    Amount = item.Amount,
                    isAdd = true
                });
            };

            return Ok(new { success = true, data = result });
        }

        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody][Required] int? price)
        {
            StripeConfiguration.ApiKey = "sk_test_DMFHsAZpG7JjAgLSAaWxCEE800SyfwhfoA";

            var options = new PaymentIntentCreateOptions
            {
                Amount = price,
                Currency = "vnd",
                // Verify your integration in this guide by including this parameter
                Metadata = new Dictionary<string, string>
                {
                    { "integration_check", "accept_a_payment" },
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            return Ok(new { scuccess = true, message = "Success", paymentIntent = paymentIntent });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            StripeConfiguration.ApiKey = "sk_test_DMFHsAZpG7JjAgLSAaWxCEE800SyfwhfoA";

            var order = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == id).SingleOrDefaultAsync();

            if (order != null)
            {
                if (order.Status != 1)
                    return BadRequest(new { success = false, message = "Order item đã xử lý k thể xóa" });
                order.Status = 4;
                await _context.SaveChangesAsync();

                var endPoint = await _sendEndpointProvider.
                                    GetSendEndpoint(new Uri("queue:" + BusConstant.AddAmountQueue));

                foreach (var item in order.OrderItems)
                {
                    await endPoint.Send<AddAmountMessage>(new
                    {
                        BookId = item.BookId,
                        Amount = item.Amount,
                        isAdd = false
                    });
                };

                var refunds = new RefundService();
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntent,
                    Amount = order.Total
                };
                var refund = refunds.Create(refundOptions);
                return Ok(new { success = true, data = order });
            }
            return NotFound(new { success = false, message = "Order item is not found" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody][Range(1, 3)] int status)
        {
            var order = await _context.Orders.Where(o => o.OrderId == id).SingleOrDefaultAsync();
            if (order != null)
            {
                if (status - order.Status == 1)
                {
                    order.Status = status;
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, data = order });
                }
                return BadRequest(new { sucess = false, message = "Error" });
            }
            return NotFound(new { success = false, message = "Order item is not found" });
        }
    }
}