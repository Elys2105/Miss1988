using Miss1988.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Data;

namespace Miss1988.Areas.Admin.Controllers
{
    public class KhuyenMaiController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admin/KhuyenMai
        public ActionResult Index()
        {
            var kmList = db.KhuyenMais.ToList();
            return View(kmList);
        }

        // GET: Admin/KhuyenMai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KhuyenMai km, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(km);
                db.SaveChanges();

                SaveImage(ImageFile, km);

                return RedirectToAction("Index");
            }

            return View(km);
        }

        // GET: Admin/KhuyenMai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var km = db.KhuyenMais.Find(id);
            if (km == null) return HttpNotFound();

            return View(km);
        }

        // POST: Admin/KhuyenMai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KhuyenMai km, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(km).State = EntityState.Modified;
                db.SaveChanges();

                SaveImage(ImageFile, km);

                return RedirectToAction("Index");
            }

            return View(km);
        }

        // GET: Admin/KhuyenMai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var km = db.KhuyenMais.Find(id);
            if (km == null) return HttpNotFound();

            return View(km);
        }

        // GET: Admin/KhuyenMai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var km = db.KhuyenMais.Find(id);
            if (km == null) return HttpNotFound();

            return View(km);
        }

        // POST: Admin/KhuyenMai/Delete (xử lý xác nhận xóa)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var km = db.KhuyenMais.Find(id);
            if (km != null)
            {
                db.KhuyenMais.Remove(km);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ========================== SAVE IMAGE ==========================
        private void SaveImage(HttpPostedFileBase imageFile, KhuyenMai km)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = "KM_" + km.KhuyenMaiID + ext;
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                imageFile.SaveAs(path);

                // Cập nhật lại đường dẫn ảnh
                km.ImageUrl = "/Content/Images/" + fileName;
                db.Entry(km).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // ========================== PARTIAL VIEW ==========================

        public ActionResult PVListKhuyenMai()
        {
            var list = db.KhuyenMais.ToList();
            return PartialView(list);
        }

        public ActionResult PVKhuyenMaiInfo(int id)
        {
            var km = db.KhuyenMais.Find(id);
            return PartialView(km);
        }
    }
}
