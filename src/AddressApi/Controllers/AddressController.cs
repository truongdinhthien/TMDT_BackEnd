using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AddressApi.Models;

namespace AddressApi.Controllers
{
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AddressController (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Route("api/city")]
        [HttpGet]
        public async Task<IActionResult> Get ()
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/city");
            // var data = JsonConvert.SerializeObject(result);
            var s = JsonConvert.DeserializeObject<listCity>(result);
            return Ok(new {success = true, data = s});
        }
        [Route("api/city/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Getby (int id)
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/city/" + id.ToString());
            // var data = JsonConvert.SerializeObject(result);
            var s = JsonConvert.DeserializeObject<City>(result);
            return Ok(new {success = true, data = s});
        }
        [Route("api/city/{id}/district")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistrictByCity (int id)
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/city/" + id.ToString() + "/district");
            // var data = JsonConvert.SerializeObject(result);
            var s = JsonConvert.DeserializeObject<IEnumerable<District>>(result);
            return Ok(new {success = true, data = s});
        }
        [Route("api/district/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistrict (int id)
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/district/" + id.ToString());
            // var data = JsonConvert.SerializeObject(result);
            
            var s = JsonConvert.DeserializeObject<District>(result);
            return Ok(new {success = true, data = s});
        }
        [Route("api/district/{id}/ward")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWardByDistrict (int id)
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/district/" + id.ToString() + "/ward");
            // var data = JsonConvert.SerializeObject(result);
            var s = JsonConvert.DeserializeObject<IEnumerable<Ward>>(result);
            return Ok(new {success = true, data = s});
        }
        [Route("api/district/{id}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWard (int id)
        {
            var result = await _httpClient.GetStringAsync("https://thongtindoanhnghiep.co/api/ward/" + id.ToString());
            // var data = JsonConvert.SerializeObject(result);
            
            var s = JsonConvert.DeserializeObject<Ward>(result);
            return Ok(new {success = true, data = s});
        }
    }
}