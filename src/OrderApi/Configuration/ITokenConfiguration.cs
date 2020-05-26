

using System.Threading.Tasks;
using OrderApi.Models;

namespace OrderApi.Configuration
{
    public interface ITokenConfiguration
    {
        User GetPayloadAsync (string accessToken);
    }
}