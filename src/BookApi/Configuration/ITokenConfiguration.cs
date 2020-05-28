

using System.Threading.Tasks;
using BookApi.Models;

namespace BookApi.Configuration
{
    public interface ITokenConfiguration
    {
        User GetPayloadAsync (string accessToken);
    }
}