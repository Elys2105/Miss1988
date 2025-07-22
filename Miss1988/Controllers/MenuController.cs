using Miss1988.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Miss1988.Controllers
{
    public class MenuController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Menu
        public ActionResult Index(string tenNhom)
        {
            ViewBag.NhomMons = db.NhomMons.ToList();

            if (string.IsNullOrEmpty(tenNhom))
            {
                var allProducts = db.Mons.Include("NhomMon").ToList();
                ViewBag.CurrentCategoryName = "Tất cả";
                return View(allProducts);
            }

            // So sánh tên nhóm không phân biệt hoa thường và giữ nguyên dấu tiếng Việt
            var nhom = db.NhomMons
                         .ToList()
                         .FirstOrDefault(n => string.Equals(n.TenNhom.Trim(), tenNhom.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (nhom == null)
            {
                ViewBag.CurrentCategoryName = tenNhom; // vẫn hiển thị tên đã chọn
                return View(new List<Mon>()); // không có món nào
            }

            var products = db.Mons
                             .Where(m => m.NhomMonID == nhom.NhomMonID)
                             .Include("NhomMon")
                             .ToList();

            ViewBag.CurrentCategoryName = nhom.TenNhom;
            return View(products);
        }

        public ActionResult Suggest(string term)
        {
            var results = db.Mons
                           .Where(p => p.TenMon.StartsWith(term))
                           .OrderBy(p => p.TenMon)
                           .Select(p => p.TenMon)
                           .Take(10)
                           .ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
