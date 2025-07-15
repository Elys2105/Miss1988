using Miss1988.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Miss1988.Controllers
{
    public class MenuController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Menu
        public ActionResult Index()
        {
            // Lấy toàn bộ sản phẩm (món)
            var products = db.Mons.ToList();
            // Lấy danh sách nhóm món để hiển thị bên sidebar
            ViewBag.NhomMons = db.NhomMons.ToList();

            return View(products);
        }

        // GET: Menu/Menu?categoryId=1
        public ActionResult Menu(int categoryId)
        {
            // Lấy sản phẩm theo nhóm
            var products = db.Mons.Where(m => m.NhomMonID == categoryId).ToList();
            // Truyền nhóm hiện tại để tô đậm menu
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.NhomMons = db.NhomMons.ToList();

            return View("Index", products); // dùng lại View Index để tránh lặp giao diện
        }
    }
}
