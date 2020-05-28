namespace CartApi.Models
{
    public class CartItem
    {
        public int Id {get;set;}
        public string UserId {get;set;}
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