using System.Linq;
using System.Threading.Tasks;
using CommentApi.Data;
using CommentApi.Filter;
using CommentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentApi.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentContext _context;
        public CommentController (CommentContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetComment ([FromQuery] CommentFilter filter, [FromQuery] Pagination pagination)
        {
            var comment = await _context.Comments.ToListAsync();

            if(filter.BookId != 0)
            {
                comment = comment.Where(c => c.BookId == filter.BookId).ToList();
            }

            var result = PaginatedList<Comment>.Create(comment, pagination.current, pagination.pageSize);

            return Ok(new {success = true, data = comment, filter = filter});
        }
    }
}