using Microsoft.AspNetCore.Mvc;
using BookApi.Core.Fillter;
using BookApi.Core;
using BookApi.Services;
using BookApi.Core.Entity;
using System.Threading.Tasks;
using System;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        public BookController (BookService bookService)
        {
            _bookService = bookService;
        } 
        [HttpGet]
        public async Task<IActionResult> GetAll ([FromQuery] Pagination pagination, [FromQuery] FillterBook fillterBook)
        {
            var result = await _bookService.GetBookAsync(pagination,fillterBook);
            return result;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> PostBook ([FromForm] Book book)
        {
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
    }
}