using System;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.ViewModels
{
    public class RatingViewModel
    {
        public int CommentId {get;set;}
        [Required]
        public int? OrderItemId {get;set;}
        [Required]
        public string BuyerId {get;set;}
        [Required]
        public string UserId {get;set;}
        [Required]
        public int? BookId {get;set;}
        [Required]
        [Range(1,5)]
        public int? Rating {get;set;}
        [Required]
        public string Content {get;set;}
    }
}