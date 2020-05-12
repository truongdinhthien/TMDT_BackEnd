using System.ComponentModel.DataAnnotations;

namespace CartApi.Models
{
    public class CartViewModel
    {
        [Required]
        public string buyerId {get;set;}
        [Required]
        public int Id {get;set;}
        [Required]
        public string Name {get;set;}
        [Required]
        public string ImagePath {get;set;}
        [Required]
        [Range(1,999)]
        public int Amount {get;set;}
        [Required]
        public int Price {get;set;}
    }
}