

using System.Threading.Tasks;
using CartApi.Models;

namespace CartApi.Configuration
{
    public interface ITokenConfiguration
    {
        User GetPayloadAsync (string accessToken);
    }
}