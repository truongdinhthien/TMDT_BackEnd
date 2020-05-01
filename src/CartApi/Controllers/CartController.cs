using System.Threading.Tasks;
using CartApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CartApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private IDatabase database;
        public CartController(IDatabase database)
        {
            this.database = database;
        }
        // [HttpGet]
        // public async Task<ActionResult> GetAsync ()
        // {
        //     return Ok ();
        // }

        [HttpGet("{key}")]
        public async Task<ActionResult> GetByAsync(string key)
        {   

            var dataSource = await database.StringGetAsync(key);

            var data = dataSource.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Cart>(dataSource);

            return Ok(new {success = true, data = data});
        }
        [HttpPut("{key}")]
        public async Task<ActionResult> UpdateAsync(string key,[FromBody] Cart cart)
        {
            
            var data = JsonConvert.SerializeObject(cart);
            var created = await database.StringSetAsync(key, data);
            return !created ? null : await GetByAsync(key);
        }
        [HttpDelete("{key}")]
        public async Task<ActionResult> DeleteAsync(string key)
        {
            
            return Ok( new {success = await database.KeyDeleteAsync(key) });
        }

    }
}