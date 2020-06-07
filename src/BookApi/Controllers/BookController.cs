using Microsoft.AspNetCore.Mvc;
using BookApi.Core.Fillter;
using BookApi.Core;
using BookApi.Services;
using BookApi.Core.Entity;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using BookApi.Configuration;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly ITokenConfiguration _token;
        public BookController (BookService bookService , ITokenConfiguration token)
        {
            _bookService = bookService;
            _token = token;
        } 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll ([FromQuery] Pagination pagination, [FromQuery] FillterBook fillterBook)
        {
            var result = await _bookService.GetBookAsync(pagination,fillterBook);
            return result;
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> PostBook ([FromForm] Book book)
        {
            string access_token = await HttpContext.GetTokenAsync("access_token");
            var user = _token.GetPayloadAsync(access_token);

            book.UserId = user.UserId;
            book.FullName = user.Fullname;
            var result = await _bookService.PostBookAsync(book);
            
            return result;
        }

        [HttpPut("rate/{id}")]
        public async Task<IActionResult> RateBook (int id,[FromBody] int rate)
        {
            Console.Write($"id : {id} rate : {rate}");
            var result = await _bookService.RateBookAsync(id, rate);
            return result;
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutBook (int id, [FromForm] Book book)
        {
            // string access_token = await HttpContext.GetTokenAsync("access_token");
            // var user = _token.GetPayloadAsync(access_token);

            var result = await _bookService.PutBookAsync(id, book);
            return Ok ();
        }
    }
}