namespace OrderApi.Models
{
    public class OrderItem
    {
        public int OrderItemId {get;set;}     
        public int BookId {get;set;}
        public string BookName {get;set;}
        public int BookPrice {get;set;}
        public int Amount {get;set;}
        public string Image {get;set;}

        public int OrderId {get;set;}
        public Order Order {get;set;}
    }
}