using Microsoft.EntityFrameworkCore;
using AddressApi.Models;

namespace AddressApi.Data
{
    public class AddressContext : DbContext
    {
        public AddressContext (DbContextOptions<AddressContext> options) : base (options)
        {

        }
        public DbSet<Address> Addresses {get;set;}
        
    }
}