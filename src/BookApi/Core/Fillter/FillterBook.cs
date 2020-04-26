namespace BookApi.Core.Fillter
{
    public class FillterBook
    {
        public int CategoryId{get;set;} = -1;
        public string Name { get; set; } = "";
        public int PriceFrom { get; set; } = -1;
        public int PriceTo { get; set; } = int.MaxValue;
        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";
    }
}