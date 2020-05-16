using Microsoft.AspNetCore.Mvc;
using AddressApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AddressApi.Models;

namespace AddressApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressUser : ControllerBase
    {
        private readonly AddressContext _context;

        public AddressUser (AddressContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAddressByUser ([FromQuery]string userId)
        {
            var addressList = await _context.Addresses.ToListAsync(); 
            if (userId != null)
            {
                addressList = addressList.Where(a => a.UserId == userId).ToList();
            }
            return Ok(new {success = true, data = addressList});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddressById (int id)
        {
            var addressList = await _context.Addresses.ToListAsync(); 
            var address = addressList.Where(a => a.Id == id).SingleOrDefault();

            return Ok(new {success = true, data = address});
        }

        [HttpPost]
        public async Task<IActionResult> PostAddress ([FromBody]Address address)
        {
            await _context.AddAsync(address);
            await _context.SaveChangesAsync();

            return Ok(new {success = true, data = address});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, [FromBody]Address address)
        {
            var addressItem = await _context.Addresses.Where(a => a.Id == id).SingleOrDefaultAsync(); 
            if(addressItem != null)
            {
                addressItem.PhoneNumber = address.PhoneNumber;
                addressItem.FullName = address.FullName;
                addressItem.City = address.City;
                addressItem.District = address.District;
                addressItem.Ward= address.Ward;
                addressItem.Street = address.Street;
                await _context.SaveChangesAsync();
            }

            else {
                return NotFound(new {success = false, message = "NotFound"});
            }
            return Ok(new {success = true});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdress(int id)
        {
            var addressItem = await _context.Addresses.Where(a => a.Id == id).FirstOrDefaultAsync(); 
            if(addressItem != null)
            {
                _context.Remove(addressItem);
                await _context.SaveChangesAsync();
            }
            else {
                return NotFound(new {success = false, message = "NotFound"});
            }
            return Ok (new {sucess = true});
        }
    }
}