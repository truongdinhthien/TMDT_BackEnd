using System.ComponentModel.DataAnnotations;

namespace CartApi.Models
{
    public class CartViewModel
    {
        [Required]
        public int Id {get;set;}
        [Required]
        public string UserId {get;set;}
        [Required]
        public string FullName {get;set;}
        [Required]
        public string Name {get;set;}
        [Required]
        public string Slug {get;set;}
        [Required]
        public string ImagePath {get;set;}
        [Required]
        [Range(1,999)]
        public int Amount {get;set;}
        [Required]
        public int Price {get;set;}
    }
}