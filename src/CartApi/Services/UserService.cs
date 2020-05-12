using System.Security.Claims;
using CartApi.Models;

namespace CartApi.Services
{
    public class UserService
    {
        public User GetUser (ClaimsPrincipal user) 
        {
            return new User {
                UsetId = user.FindFirstValue("sub")
            };
        }
    }
}