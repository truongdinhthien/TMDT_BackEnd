using Microsoft.AspNetCore.Mvc;
using IdentityApiOIDC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityApiOIDC.ViewModels;

namespace IdentityApiOIDC.Controllers
{
    public class AccountController : Controller
    {
         private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }
        [HttpGet]
        public IActionResult Login (string ReturnUrl)
        {
            Console.WriteLine("This is get");
            return View(new LoginViewModel{ReturnUrl = ReturnUrl});
        }        
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            Console.WriteLine("This is post");
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.PhoneNumber, model.Password, false, false);

                if(result.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                Console.WriteLine("This is fail");
                ModelState.AddModelError(string.Empty, "Invalid Login Attemp");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register ()
        {
            return View();
        }
        
    }
}