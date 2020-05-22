using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AddressApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressApi.Configuration
{
    public class TokenConfiguration : ControllerBase, ITokenConfiguration
    {
        public async Task<User> GetPayloadAsync(string accessToken)
        {
            var token = new JwtSecurityToken(accessToken);
            var sub = token.Claims.FirstOrDefault(m => m.Type == "sub").Value;
            var Fullname = token.Claims.FirstOrDefault(m => m.Type == "Fullname").Value;
            var Role = token.Claims.FirstOrDefault(m => m.Type == "Role").Value;

            return new User {UserId = sub, Fullname = Fullname, Role = Role};
        }
    }
}