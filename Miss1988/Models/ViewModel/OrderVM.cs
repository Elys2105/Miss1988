using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class OrderVM
    {
        public ORDER Order { get; set; }

        public string PhuongThucThanhToan { get; set; } // dữ liệu mô phỏng
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }

    }

}