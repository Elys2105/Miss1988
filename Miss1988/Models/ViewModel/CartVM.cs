using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new List<CartItemVM>();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    }
}