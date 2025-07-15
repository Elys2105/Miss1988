using Miss1988.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Miss1988.Controllers
{
    public class PromotionController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        public ActionResult Index()
        {
            var promotions = db.KhuyenMais.OrderByDescending(km => km.NgayBatDau).ToList();
            return View(promotions);
        }
    }
}