using System;

namespace OrderApi.ViewModels
{
    public class RatingViewModel
    {
        public int CommentId {get;set;}
        public int OrderItemId {get;set;}
        public string BuyerId {get;set;}
        public string UserId {get;set;}
        public int BookId {get;set;}
        public int Rating {get;set;}
        public string Content {get;set;}
    }
}