using CommentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentApi.Data
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base (options)
        {

        }

        public DbSet<Comment> Comments {get;set;}
    }
}