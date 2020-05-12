using System.Collections.Generic;

namespace CartApi.Models
{
    public class Cart
    {
        public Cart() { }

        public Cart(string _buyerId)
        {
            buyerId = _buyerId;
        }

        public string buyerId { get; set; }
        
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}