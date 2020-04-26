using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controller
{
    public class TestController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [Route("test")]
        [HttpGet]
        // [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
            return Ok(new { message = "Hello API", claims });
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("test1")]
        [HttpGet]
        
        // [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        public IActionResult Get1()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToArray();
            return Ok(new { message = "Hello API1", claims });
        }
    }
}