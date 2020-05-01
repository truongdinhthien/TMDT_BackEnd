namespace CartApi.Models
{
    public class CartItem
    {
        public string Id {get;set;}
        public string Name {get;set;}
        public string ImagePath {get;set;}
        public int Amount {get;set;}
        public int Price {get;set;}
        public int TotalPrice {
            get {return Price * Amount;}
            set {;}
        }
    }
}