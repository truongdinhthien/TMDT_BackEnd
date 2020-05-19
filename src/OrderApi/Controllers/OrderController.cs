using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _context;
        public OrderController (OrderContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder ()
        {
            return Ok (new {success = true});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderBy ()
        {
            return Ok (new {success = true});
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddOrder ()
        {
            return Ok (new {success = true});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder ()
        {
            return Ok (new {success = true});
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder ()
        {
            return Ok (new {success = true});
        }
        
    }
}