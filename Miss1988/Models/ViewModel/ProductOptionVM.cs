using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Miss1988.Models.ViewModel
{
    public class ProductOptionVM
    {
        public int MonID { get; set; }
        public string TenMon { get; set; }
        public string ImageUrl { get; set; }
        public string MoTa { get; set; }
        public decimal? GiaBan { get; set; }
        public string TenNhomMon { get; set; }
        public List<SelectListItem> AvailableSizes { get; set; }
        public List<SelectListItem> Toppings { get; set; }
    }
}
