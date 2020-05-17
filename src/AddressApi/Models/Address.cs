using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber {get;set;}
        [Required]
        public string City {get;set;}
        [Required]
        public string District {get;set;}
        [Required]
        public string Ward {get;set;}
        [Required]
        public string Street{get;set;}
        public string FullAddress {
            get {
                return Street + " " + Ward + " " + District + " " + " " + City;
            }
            set {;}
        }
        public bool IsDefault {get;set;}
    }
}