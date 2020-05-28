using System.Threading.Tasks;
using CartApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using CartApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class CartController : ControllerBase
    {
        private CartService _cartService;
        private UserService _userService;
        public CartController(CartService cartService, UserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        // [HttpGet]
        // public IActionResult GetAsync ()
        // {
        //     var user = _userService.GetUser(HttpContext.User);
        //     var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
        //     return Ok(new { message = "Hello API", data = user, claims = claims });
        // }


        [HttpGet("{key}")]
        public async Task<ActionResult> GetCart(string key)
        {   

            var data = await _cartService.GetCartAsync(key);

            return Ok(new {success = true, data = data});
        }
        [HttpPost]
        public async Task<ActionResult> AddToCart(CartViewModel cartViewModel)
        {
            var cartItem = new CartItem {
                Id = cartViewModel.Id,
                UserId = cartViewModel.UserId,
                Name = cartViewModel.Name,
                Price = cartViewModel.Price,
                Amount = cartViewModel.Amount,
                ImagePath = cartViewModel.ImagePath
            };
            await _cartService.AddItemToCart(cartViewModel.buyerId, cartItem);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(cartViewModel.buyerId)});
        }
        [HttpPut("{key}")]
        public async Task<ActionResult> PutToCart(string key, [FromBody] CartItem cartItem)
        {
            await _cartService.PutItemToCart(key, cartItem);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(key)});
        }
        [HttpDelete("{key}")]
        public async Task<ActionResult> DeleteAsync(string key,[FromBody] int id)
        {
            await _cartService.DeleteCartItemAsync(key,id);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(key)});
        }

    }
}