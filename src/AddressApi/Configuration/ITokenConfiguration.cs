

using System.Threading.Tasks;
using AddressApi.Models;

namespace AddressApi.Configuration
{
    public interface ITokenConfiguration
    {
        User GetPayloadAsync (string accessToken);
    }
}