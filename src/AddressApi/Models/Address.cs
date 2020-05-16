using System.ComponentModel.DataAnnotations;

namespace AddressApi.Models
{
    public class Address
    {
        public int Id {get;set;}
        [Required]
        public string UserId {get;set;}
        [Required]
        public string FullName {get;set;}
        [Required]
        public string PhoneNumber {get;set;}
        [Required]
        public string City {get;set;}
        [Required]
        public string District {get;set;}
        [Required]
        public string Ward {get;set;}
        [Required]
        public string Street{get;set;}
    }
}