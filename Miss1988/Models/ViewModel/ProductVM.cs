using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class ProductVM
    {
        public int MonID { get; set; }
        public string TenMon { get; set; }
        public decimal? GiaBan { get; set; }
        public string MoTa { get; set; }
        public string ImageUrl { get; set; }
    }
}