using System;

namespace OrderApi.Filter
{
    public class OrderFilter
    {
        public int Status {get;set;} = 0;
        public DateTime DateFrom {get;set;}
        public DateTime DateTo {get;set;}
    }
}