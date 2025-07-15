using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class CartItemVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ExtraInfo { get; set; } // để lưu thông tin size, topping,...

        public decimal TotalPrice => Quantity * UnitPrice;
    }
}