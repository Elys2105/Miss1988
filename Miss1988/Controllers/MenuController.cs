using Miss1988.Models;
using System;
using System.Collections.Generic;
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
            // Lấy danh sách nhóm món để render sidebar
            ViewBag.NhomMons = db.NhomMons.ToList();

            // Nếu không có nhóm được chọn → hiện toàn bộ món
            if (string.IsNullOrEmpty(tenNhom))
            {
                var allProducts = db.Mons.ToList();
                ViewBag.CurrentCategoryName = "Tất cả";
                return View(allProducts);
            }

            // Tìm nhóm món theo tên KHÔNG cần ToUrlFriendly
            var nhom = db.NhomMons
                .FirstOrDefault(n => n.TenNhom.ToLower() == tenNhom.ToLower());

            if (nhom == null)
                return HttpNotFound("Nhóm món không tồn tại");

            var products = db.Mons
                .Where(m => m.NhomMonID == nhom.NhomMonID)
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
