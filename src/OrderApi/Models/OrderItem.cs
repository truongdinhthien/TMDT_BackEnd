using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class OrderItem
    {
        public int OrderItemId {get;set;}
        [Required]   
        [Range(1,int.MaxValue)]  
        public int? BookId {get;set;}
        [Required]
        public string UserId {get;set;}
        [Required]
        public string FullName {get;set;}
        [Required]
        public string BookName {get;set;}
        [Required]
        public string Slug {get;set;}
        [Required]
        [Range(1,int.MaxValue)]
        public int BookPrice {get;set;}
        [Required]
        [Range(1,int.MaxValue)]
        public int Amount {get;set;}
        [Required]
        public string Image {get;set;}

        public int OrderId {get;set;}
        public Order Order {get;set;}
    }
}