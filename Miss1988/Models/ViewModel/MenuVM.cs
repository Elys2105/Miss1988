using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class MenuVM
    {
        public List<NhomMon> Categories { get; set; }
        public List<Mon> Products { get; set; }
        public int? SelectedCategoryId { get; set; }
    }

}