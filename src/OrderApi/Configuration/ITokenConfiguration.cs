

using System.Threading.Tasks;
using OrderApi.Models;

namespace OrderApi.Configuration
{
    public interface ITokenConfiguration
    {
        public Task<User> GetPayloadAsync (string accessToken);
    }
}