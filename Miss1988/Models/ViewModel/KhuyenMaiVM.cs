using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class KhuyenMaiVM
    {
        public int KhuyenMaiID { get; set; }
        public string MaVoucher { get; set; }
        public string MoTa { get; set; }
        public decimal? GiaTriGiam { get; set; }
        public string KieuGiam { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string ImageUrl { get; set; }
    }
}