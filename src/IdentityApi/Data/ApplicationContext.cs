using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}