using System.Collections.Generic;
namespace AddressApi.DTOs
{
    public class ListAddressDTO
    {
        public string UserId {get;set;}
        public List<AddressDTO> AddressDTOs {get;set;}
    }
}