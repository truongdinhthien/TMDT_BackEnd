namespace BookApi.Core.Fillter
{
    public class FillterBook
    {
        public int CategoryId{get;set;} = -1;

        public string UserId {get;set;} = "";
        public string Name { get; set; } = "";
        public int PriceFrom { get; set; } = -1;
        public int PriceTo { get; set; } = int.MaxValue;
        public int Rating {get;set;} = 0;
        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";
    }
}