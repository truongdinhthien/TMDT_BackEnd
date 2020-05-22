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
        public async Task<IActionResult> GetOrder ()
        {
            var orderSource = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = await _token.GetPayloadAsync(access_token);

            var order = orderSource.Where(o => o.UserId == user.UserId).ToList();

            var totalAllPrice = 0;

            foreach (var item in orderSource)
            {
                if(item.Status != 4)
                    totalAllPrice += item.Total;
            }
        
            return Ok (new {success = true, data = order, totalAllPrice = totalAllPrice});
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
            order.Status = 1;
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = await _token.GetPayloadAsync(access_token);

            order.BuyerId = user.UserId;
            
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            return Ok (new {success = true, data = order});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder (int id)
        {
            var orderItem = await _context.Orders.Include(o => o.OrderItems).Where(o => o.OrderId == id).SingleOrDefaultAsync();
             if(orderItem != null)
            {
                orderItem.Status = 4;
                await _context.SaveChangesAsync();
                return Ok (new {success = true, data = orderItem});
            }
                return NotFound (new {success = false, message = "Order item is not found"});
        }
        
    }
}