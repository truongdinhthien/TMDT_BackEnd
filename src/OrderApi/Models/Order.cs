using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Models
{
    public class Order
    {
        public int OrderId {get;set;}
        [Required]
        public string BuyerId {get;set;}
        public DateTime Create {get;set;} = DateTime.Now;
        public int Status {get;set;} // 1 : Chưa xử lý , 2 : Đã xử lý , 3 : Đã nhận , 4 : Hủy
        public string Fullname {get;set;}
        public string Address {get;set;}

        public int Total {
            get{
                int temp = 0;
                foreach (var item in OrderItems)
                {
                    temp += (item.Amount * item.BookPrice);
                }
                return temp;
            }
            set {;}
        }

        public ICollection<OrderItem> OrderItems {get;set;}
    }
}