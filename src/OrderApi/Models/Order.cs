using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class Order
    {
        public int OrderId {get;set;}
        public string BuyerId {get;set;}
        public string UserId{get;set;}
        public DateTime Create {get;set;} = DateTime.Now;
        public int Status {get;set;} // 1 : Chưa xử lý , 2 : Đã xử lý , 3 : Đã nhận , 4 : Hủy
        [Required]
        public string Fullname {get;set;}
        [Required]
        public string Address {get;set;}
        [Required]
        public string PhoneNumber {get;set;}
        public int Total {
            get{
                int temp = 0;
                if(OrderItems == null)
                    return temp;
                foreach (var item in OrderItems)
                {
                    temp += (item.Amount * item.BookPrice);
                }
                return temp;
            }
            set {;}
        }

        public int TotalItem {
            get {
                int temp = 0;
                if(OrderItems == null)
                    return temp;
                foreach (var item in OrderItems)
                {
                    temp += item.Amount;
                }
                return temp;
            }
            set{;}
        }
        [Required]
        public ICollection<OrderItem> OrderItems {get;set;}
    }
}