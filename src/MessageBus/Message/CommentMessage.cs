using System;

namespace MessageBus.Message
{
    public class CommentMessage
    {
        public int CommentId {get;set;}
        public string BuyerId {get;set;}
        public string FullName {get;set;}
        public string UserId {get;set;}
        public int BookId {get;set;}
        public int Rating {get;set;}
        public string Content {get;set;}
        public DateTime CreatedDate {get;set;}
    }
}