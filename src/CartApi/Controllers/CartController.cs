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
using CartApi.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class CartController : ControllerBase
    {
        private CartService _cartService;
        private readonly ITokenConfiguration _token;
        public CartController(CartService cartService, ITokenConfiguration token)
        {
            _cartService = cartService;
            _token = token;
        }

        // [HttpGet]
        // public IActionResult GetAsync ()
        // {
        //     var user = _userService.GetUser(HttpContext.User);
        //     var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
        //     return Ok(new { message = "Hello API", data = user, claims = claims });
        // }


        [HttpGet]
        public async Task<ActionResult> GetCart()
        {   
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var data = await _cartService.GetCartAsync(user.UserId);

            return Ok(new {success = true, data = data});
        }
        [HttpPost]
        public async Task<ActionResult> AddToCart(CartViewModel cartViewModel)
        {
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);

            var cartItem = new CartItem {
                Id = cartViewModel.Id,
                UserId = cartViewModel.UserId,
                Name = cartViewModel.Name,
                Slug = cartViewModel.Slug,
                Price = cartViewModel.Price,
                Amount = cartViewModel.Amount,
                ImagePath = cartViewModel.ImagePath
            };
            await _cartService.AddItemToCart(user.UserId, cartItem);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(user.UserId)});
        }
        [HttpPut]
        public async Task<ActionResult> PutToCart([FromBody] CartItem cartItem)
        {
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);
            await _cartService.PutItemToCart(user.UserId, cartItem);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(user.UserId)});
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromBody] int id)
        {
            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);
            await _cartService.DeleteCartItemAsync(user.UserId,id);
            return Ok(new {success = true, data = await _cartService.GetCartAsync(user.UserId)});
        }

    }
}