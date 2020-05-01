using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityApi.Models;
using IdentityApi.Data;
using IdentityApi.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using IdentityModel.Client;

namespace IdentityApi.Controller
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController (UserManager<User> userManager, SignInManager<User> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Route("api/account/register")]
        [HttpPost] 
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var user = new User () {PhoneNumber = model.PhoneNumber, UserName = model.PhoneNumber, Fullname = model.Fullname};

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
                return Ok(new {success = true, message = "Register Successfully"});
            else 
            {
                if(result.Errors.Any(x => x.Code == "DuplicateUserName"))
                {
                    return BadRequest(new {success = false, message = "Duplicate user name error"});
                }
            }
                return BadRequest(new {success = false, message = "Register Failed", error = result.Errors});
        }
        [Route("api/account/login")]
        [HttpPost]
        public async Task<IActionResult> Login ([FromBody]LoginViewModel model)
        {
            var client = new HttpClient();
            
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5000");
            if (disco.IsError)
            {
                return BadRequest(new {success = false, message = disco.Error});
            }
            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                GrantType = "password",
                ClientSecret = "secret",
                Scope = "book openid profile",
                UserName = model.PhoneNumber,
                Password = model.Password
            });
            if (tokenResponse.IsError)
            {
                return BadRequest(new {success = false, message = tokenResponse.ErrorDescription});
            }
            // data user
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);
            user.PasswordHash = null;
            return Ok(new {success = true,token = tokenResponse.Json, data = user});
        }
    }
}