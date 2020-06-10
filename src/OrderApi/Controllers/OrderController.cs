using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderApi.Configuration;
using OrderApi.Data;
using OrderApi.Filter;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class OrderController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly ITokenConfiguration _token;
        public OrderController (OrderContext context, ITokenConfiguration token)
        {
            _context = context;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder ([FromQuery] OrderFilter filter)
        {
            var orderSource = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

            // var order = orderSource;
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var order = orderSource.Where(o => o.BuyerId == user.UserId).ToList();

            if (filter.Status != 0 )
            {
                order = order.Where(o => o.Status == filter.Status).ToList();
            }

            var totalAllPrice = 0;

            foreach (var item in order)
            {
                if(item.Status != 4)
                    totalAllPrice += item.Total;
            }
        
            return Ok (new {success = true,filter = filter, data = order, totalAllPrice = totalAllPrice});
        }

        [HttpGet("shop/{userId}")]
        public async Task<IActionResult> GetOrderUser (string userId,[FromQuery] OrderFilter filter)
        {
            var orderSource = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

            // var order = orderSource;
            // string access_token = await HttpContext.GetTokenAsync("access_token");

            // var user = _token.GetPayloadAsync(access_token);

            var order = orderSource.Where(o => o.UserId == userId).ToList();

            if (filter.Status != 0 )
            {
                order = order.Where(o => o.Status == filter.Status).ToList();
            }

            var totalAllPrice = 0;

            foreach (var item in order)
            {
                if(item.Status != 4)
                    totalAllPrice += item.Total;
            }
        
            return Ok (new {success = true,filter = filter, data = order, totalAllPrice = totalAllPrice});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderBy (int id)
        {
            var orderItem = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == id).SingleOrDefaultAsync();
            if(orderItem != null)
            {
                return Ok (new {success = true, data = orderItem});
            }
                return NotFound (new {success = false, message = "Order item is not found"});
            
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder ([FromBody] Order order )
        {
            if (order.OrderItems.Count() == 0)
                return BadRequest(new {success = false, message = "OrderItem Is Null"});

            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var temp = order.OrderItems;

            var result = temp.GroupBy(t => t.UserId)
                             .Select(g => new Order() {
                                 Address = order.Address,
                                 Fullname = order.Fullname,
                                 PhoneNumber = order.PhoneNumber,
                                 BuyerId = user.UserId,
                                 UserId = g.Key,
                                 Status = 1,
                                 OrderItems = g.ToList()
                             });
            
            await _context.AddRangeAsync(result);
            await _context.SaveChangesAsync();

            return Ok (new {success = true, data = result});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder (int id)
        {
            var order = await _context.Orders.Where(o => o.OrderId == id).SingleOrDefaultAsync();
            if(order != null)
            {
                order.Status = 4;
                await _context.SaveChangesAsync();
                return Ok (new {success = true, data = order});
            }
                return NotFound (new {success = false, message = "Order item is not found"});
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder (int id, int status)
        {
            var order = await _context.Orders.Where(o => o.OrderId == id).SingleOrDefaultAsync();

            if(order != null)
            {
                if(order.Status - status != 1)
                {
                    order.Status = status;
                    await _context.SaveChangesAsync();
                    return Ok (new {success = true, data = order});
                }

                if(status > 3 || status < 1) return BadRequest(new {success = false , message = "Status range is 1 to 3"});
                return BadRequest(new {sucess = false, message = "Error"});
            }
            return NotFound (new {success = false, message = "Order item is not found"});
        }
    }
}