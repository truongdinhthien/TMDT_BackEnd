using System.Collections.Generic;

namespace BookApi.Core.Entity
{
    public class Category
    {
        public int CategoryId {get;set;}
        public string Name {get;set;}
        public string Slug {get;set;}
        public ICollection<Book> Books {get;set;}
    }
}