using System.Threading.Tasks;
using IdentityApiOIDC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApiOIDC.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController (UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByAsync (string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null )
                return NotFound (new {success = false, message = "cant not find"});
            user.PasswordHash = null;
            return Ok(new {success = true, data = user});
        }
    }
}