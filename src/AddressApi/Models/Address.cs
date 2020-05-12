namespace AddressApi.Models
{
    public class Address
    {
        public int Id {get;set;}
        public string UserId {get;set;}
        public string FullName {get;set;}
        public string PhoneNumber {get;set;}
        public string City {get;set;}
        public string District {get;set;}
        public string Ward {get;set;}
    }
}