

using System.Threading.Tasks;
using AddressApi.Models;

namespace AddressApi.Configuration
{
    public interface ITokenConfiguration
    {
        public Task<User> GetPayloadAsync (string accessToken);
    }
}