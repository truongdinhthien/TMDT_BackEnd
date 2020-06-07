using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MessageBus.Config;
using MessageBus.Message;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Configuration;
using OrderApi.Data;
using OrderApi.ViewModels;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly OrderContext _context;
        private readonly ITokenConfiguration _token;
        public RatingController(ISendEndpointProvider sendEndpointProvider, OrderContext context, ITokenConfiguration token)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _context = context;
            _token = token;
        }

        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] RatingViewModel vm)
        {
            var orderItem = _context.OrderItems.Where(o => o.OrderItemId == vm.OrderItemId).SingleOrDefault();

            if (orderItem == null)
            {
                return NotFound(new { success = false, message = "Not Found" });
            }
            else
            {
                if (orderItem.isRate == true)
                {
                    return BadRequest(new { success = false, message = "This item has rated" });
                }
                else
                {
                    var endPoint = await _sendEndpointProvider.
                                    GetSendEndpoint(new Uri("queue:" + BusConstant.CommentQueue));

                    await endPoint.Send<CommentMessage>(new
                    {
                        CommentId = vm.CommentId,
                        BuyerId = vm.BuyerId,
                        UserId = vm.UserId,
                        BookId = vm.BookId,
                        Rating = vm.Rating,
                        Content = vm.Content,
                        CreatedDate = new DateTime()
                    });

                    return Ok();
                }
            }
        }
    }
}