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
    public class MonController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admin/Mon
        public ActionResult Index()
        {
            var mon = db.Mons.Include(m => m.NhomMon).ToList();
            return View(mon);
        }

        // GET: Admin/Mon/Create
        public ActionResult Create()
        {
            ViewBag.NhomMonID = new SelectList(db.NhomMons, "NhomMonID", "TenNhom");
            return View();
        }

        // POST: Admin/Mon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mon mon, HttpPostedFileBase ImageFile)
        {
            // Xử lý chuyển đổi nếu GiaBan có dấu chấm ngăn cách
            var giaBanRaw = Request["GiaBan"];
            if (!string.IsNullOrEmpty(giaBanRaw))
            {
                giaBanRaw = giaBanRaw.Replace(".", "").Trim();
                decimal giaBan;
                if (decimal.TryParse(giaBanRaw, out giaBan))
                {
                    mon.GiaBan = giaBan;
                }
            }

            if (ModelState.IsValid)
            {
                db.Mons.Add(mon);
                db.SaveChanges();

                SaveImage(ImageFile, mon);

                return RedirectToAction("Index");
            }

            ViewBag.NhomMonID = new SelectList(db.NhomMons, "NhomMonID", "TenNhom", mon.NhomMonID);
            return View(mon);
        }


        // GET: Admin/Mon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var mon = db.Mons.Find(id);
            if (mon == null) return HttpNotFound();

            ViewBag.NhomMonID = new SelectList(db.NhomMons, "NhomMonID", "TenNhom", mon.NhomMonID);
            return View(mon);
        }

        // POST: Admin/Mon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Mon mon, HttpPostedFileBase ImageFile)
        {
            var giaBanRaw = Request["GiaBan"];
            if (!string.IsNullOrEmpty(giaBanRaw))
            {
                giaBanRaw = giaBanRaw.Replace(".", "").Trim();
                decimal giaBan;
                if (decimal.TryParse(giaBanRaw, out giaBan))
                {
                    mon.GiaBan = giaBan;
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(mon).State = EntityState.Modified;
                db.SaveChanges();

                // Cập nhật ảnh nếu có upload mới
                SaveImage(ImageFile, mon);

                return RedirectToAction("Index");
            }

            ViewBag.NhomMonID = new SelectList(db.NhomMons, "NhomMonID", "TenNhom", mon.NhomMonID);
            return View(mon);
        }

        // GET: Admin/Mon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var mon = db.Mons.Include(m => m.NhomMon).FirstOrDefault(m => m.MonID == id);
            if (mon == null) return HttpNotFound();

            return View(mon);
        }

        // GET: Admin/Mon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var mon = db.Mons.Find(id);
            if (mon == null) return HttpNotFound();

            return View(mon);
        }

        // POST: Admin/Mon/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var mon = db.Mons.Find(id);
            if (mon != null)
            {
                db.Mons.Remove(mon);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Partial View: danh sách món
        public ActionResult PVListMon()
        {
            var mon = db.Mons.Include(m => m.NhomMon).ToList();
            return PartialView(mon);
        }

        // Partial View: chi tiết 1 món
        public ActionResult PVMonInfo(int id)
        {
            var mon = db.Mons.Include(m => m.NhomMon).FirstOrDefault(m => m.MonID == id);
            return PartialView(mon);
        }

        // ==========================
        // Lưu ảnh vào thư mục Content/Images/
        private void SaveImage(HttpPostedFileBase imageFile, Mon mon)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName);
                var fileName = mon.MonID + ext;
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                imageFile.SaveAs(path);

                // Cập nhật đường dẫn ảnh trong DB
                mon.ImageUrl = "/Content/Images/" + fileName;
                db.Entry(mon).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
