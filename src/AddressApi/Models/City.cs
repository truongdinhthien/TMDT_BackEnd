using System.Collections.Generic;

namespace AddressApi.Models
{
    public class City
    {
        public int ID{get;set;}
        public string Title {get;set;}

        // public int Type {get;set;}
        // public string SolrID {get;set;}
        // public int STT {get;set;}
        // public string Created {get;set;}

        // public string Updated {get;set;}
        // public int TotalDoanhNghiep {get;set;}
    }

    public class listCity 
    {
        public IList<City> LtsItem {get;set;}
        // public int TotalDoanhNghiep {get;set;}
    }
}