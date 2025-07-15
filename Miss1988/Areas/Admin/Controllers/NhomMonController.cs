using Miss1988.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Data;

namespace Miss1988.Areas.Admin.Controllers
{
    public class NhomMonController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admin/NhomMon
        public ActionResult Index()
        {
            var nhomMons = db.NhomMons.ToList();
            return View(nhomMons);
        }

        // GET: Admin/NhomMon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhomMon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhomMon nhomMon, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                db.NhomMons.Add(nhomMon);
                db.SaveChanges();

                // Lưu ảnh sau khi có ID
                SaveImage(ImageFile, nhomMon);

                return RedirectToAction("Index");
            }
            return View(nhomMon);
        }

        // GET: Admin/NhomMon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nhomMon = db.NhomMons.Find(id);
            if (nhomMon == null) return HttpNotFound();

            return View(nhomMon);
        }

        // POST: Admin/NhomMon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhomMon nhomMon, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existing = db.NhomMons.Find(nhomMon.NhomMonID);
                if (existing == null)
                {
                    return HttpNotFound("Nhóm món không tồn tại.");
                }

                existing.TenNhom = nhomMon.TenNhom;
                existing.ImageUrl = nhomMon.ImageUrl;

                db.SaveChanges();

                // Nếu có ảnh mới thì lưu lại
                SaveImage(ImageFile, existing);

                return RedirectToAction("Index");
            }

            return View(nhomMon);
        }

        // GET: Admin/NhomMon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nhomMon = db.NhomMons.Find(id);
            if (nhomMon == null) return HttpNotFound();

            return View(nhomMon);
        }

        // GET: Admin/NhomMon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nhom = db.NhomMons.Find(id);
            if (nhom == null) return HttpNotFound();

            return View(nhom);
        }

        // POST: Admin/NhomMon/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nhom = db.NhomMons.Find(id);
            if (nhom != null)
            {
                db.NhomMons.Remove(nhom);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // ==========================
        // Lưu ảnh cho nhóm món
        private void SaveImage(HttpPostedFileBase imageFile, NhomMon nhomMon)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = "nhommon_" + nhomMon.NhomMonID + ext;
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                imageFile.SaveAs(path);

                nhomMon.ImageUrl = "/Content/Images/" + fileName;
                db.Entry(nhomMon).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
