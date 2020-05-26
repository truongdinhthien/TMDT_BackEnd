using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace BookApi.Core.Entity
{
    public class Book
    {
        public int BookId {get;set;}
        [Required]
        public string Name {get;set;}
        [Required]
        public int? Price {get;set;}
        [Required]
        public string Slug {get;set;}
        [Required]
        public string Content {get;set;}
        public List<string> ImagePaths {get;set;} = new List<string>();

        public string ImagePath 
        {
            get
            {
                return ImagePaths.FirstOrDefault();
            }
            set{;}
        }

        [Required]
        public string Author {get;set;}
        [Required]
        public string Publisher {get;set;}
        [Required]
        public int? Amount {get;set;}

        public int Rate1{get;set;}
        public int Rate2{get;set;}
        public int Rate3{get;set;}
        public int Rate4{get;set;}
        public int Rate5{get;set;}
        public int RateCount
        {
            get{return (Rate1 + Rate2 + Rate3 + Rate4 + Rate5 );}
            set{;}
        }
        public double Rating 
        {
            get{
                Console.WriteLine("First");
                var rating = (double) (Rate1 * 1 + Rate2 * 2 + Rate3 * 3 + Rate4 * 4 + Rate5 * 5) / (RateCount);
                Console.WriteLine($"Value 1 : {rating}");
                rating = Math.Floor(rating * 10) / 10;
                Console.WriteLine($"Value 2 : {rating}");
                Console.WriteLine($"Value 2 : {Double.NaN}");
                if(Double.IsNaN(rating)) return 0;
                return rating;
            }
            set{;}
        }
        
        [Required]
        public int? CategoryId {get;set;}
        public Category Category {get;set;}
        [NotMapped]
        [Required]
        public List<IFormFile> Images {get;set;}
    }
}