using Microsoft.AspNetCore.Mvc;
using AddressApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AddressApi.Models;
using Microsoft.AspNetCore.Authorization;
using AddressApi.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace AddressApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressUser : ControllerBase
    {
        private readonly AddressContext _context;
        private readonly ITokenConfiguration _token;

        public AddressUser(AddressContext context, ITokenConfiguration token)
        {
            _context = context;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressByUser([FromQuery] bool? IsDefault)
        {
            var addressList = await _context.Addresses.ToListAsync();


            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);
            
            if (user != null)
            {
                addressList = addressList.Where(a => a.UserId == user.UserId).ToList();
            }

            
            if(IsDefault != null)
            {
                addressList = addressList.Where(a => a.IsDefault == IsDefault).ToList();
            }

            if (addressList.Count() == 1)
            {
                var address = addressList.SingleOrDefault();
                return Ok(new { success = true, data = address});
            }
            return Ok(new { success = true, data = addressList});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var addressList = await _context.Addresses.ToListAsync();
            var address = addressList.Where(a => a.Id == id).SingleOrDefault();

            return Ok(new { success = true, data = address });
        }

        [HttpPost]
        public async Task<IActionResult> PostAddress([FromBody] Address address)
        {
            var addressList = await _context.Addresses.ToListAsync();

            string access_token = await HttpContext.GetTokenAsync("access_token");

            var user = _token.GetPayloadAsync(access_token);
            address.UserId = user.UserId;
            addressList = addressList.Where(a => a.UserId == address.UserId).ToList();



            if (addressList.Count() > 0)
            {
                address.IsDefault = false;
            }
            else
            {
                address.IsDefault = true;
            }

            await _context.AddAsync(address);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = address });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, [FromBody] Address address)
        {
            var addressItem = await _context.Addresses.Where(a => a.Id == id).SingleOrDefaultAsync();
            if (addressItem != null)
            {
                addressItem.PhoneNumber = address.PhoneNumber;
                addressItem.FullName = address.FullName;
                addressItem.City = address.City;
                addressItem.District = address.District;
                addressItem.Ward = address.Ward;
                addressItem.Street = address.Street;
                await _context.SaveChangesAsync();
            }

            else
            {
                return NotFound(new { success = false, message = "NotFound" });
            }
            return Ok(new { success = true });
        }

        [HttpPut("changedefault/{id}")]
        public async Task<IActionResult> ChangeDefault(int id)
        {
            var addressItem = await _context.Addresses.Where(a => a.Id == id).SingleOrDefaultAsync();

            var userId = addressItem.UserId;
            if (addressItem.IsDefault == true)
            {
                return BadRequest(new { success = false, message = "This address is default" });
            }
            else
            {

                addressItem.IsDefault = true;

                var addressDefault = await _context.Addresses
                  .Where(a => a.UserId == userId && a.IsDefault == true)
                  .SingleOrDefaultAsync();

                addressDefault.IsDefault = false;

                await _context.SaveChangesAsync();
            }
            return Ok(new {success = true});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdress(int id)
        {
            var addressItem = await _context.Addresses.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (addressItem != null)
            {
                if (addressItem.IsDefault == true)
                {
                    return BadRequest(new { success = false, message = "U cant delete the default address" });
                }
                else
                {
                    _context.Remove(addressItem);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return NotFound(new { success = false, message = "NotFound" });
            }
            return Ok(new { sucess = true });
        }
    }
}