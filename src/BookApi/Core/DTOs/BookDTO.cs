using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace BookApi.Core.DTOs
{
    public class BookDTO
    {
        public int BookId {get;set;}
        public string Name {get;set;}
        public int Price {get;set;}
        public int Rate1{get;set;}
        public int Rate2{get;set;}
        public int Rate3{get;set;}
        public int Rate4{get;set;}
        public int Rate5{get;set;}
        public double Rating {get;set;}
        public int RateCount {get;set;}
        public string Slug {get;set;}
        public string Content {get;set;}
        public List<string> ImagePaths {get;set;}

        public string ImagePath {get;set;}
        public string Author {get;set;}
        public string Publisher {get;set;}
        public int? Amount {get;set;}

         public string UserId {get;set;}
        public string FullName {get;set;}
        public CategoryDTO Category {get;set;}
    }
}