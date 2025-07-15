using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Miss1988.Models; // namespace chứa DbContext và các model

namespace Miss1988.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admin/NguoiDungs
        public ActionResult Index()
        {
            var users = db.NguoiDungs.Include(u => u.PhanQuyen);
            return View(users.ToList());
        }

        // GET: Admin/Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nd = db.NguoiDungs
                       .Include(u => u.PhanQuyen)
                       .FirstOrDefault(u => u.NguoiDungID == id);

            if (nd == null)
                return HttpNotFound();

            return View(nd);
        }

        // GET: Admin/NguoiDungs/Create
        public ActionResult Create()
        {
            ViewBag.QuyenID = new SelectList(db.PhanQuyens, "QuyenID", "TenQuyen");
            return View();
        }

        // POST: Admin/NguoiDungs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                nd.NgayTao = DateTime.Now;
                nd.TrangThai = true; // Mặc định đang hoạt động
                db.NguoiDungs.Add(nd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuyenID = new SelectList(db.PhanQuyens, "QuyenID", "TenQuyen", nd.QuyenID);
            return View(nd);
        }

        // GET: Admin/NguoiDungs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NguoiDung nd = db.NguoiDungs.Find(id);
            if (nd == null)
                return HttpNotFound();

            ViewBag.QuyenID = new SelectList(db.PhanQuyens, "QuyenID", "TenQuyen", nd.QuyenID);
            return View(nd);
        }

        // POST: Admin/NguoiDungs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NguoiDung nd)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuyenID = new SelectList(db.PhanQuyens, "QuyenID", "TenQuyen", nd.QuyenID);
            return View(nd);
        }

        // KHÓA tài khoản
        public ActionResult Lock(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.NguoiDungs.Find(id);
            if (user == null)
                return HttpNotFound();

            user.TrangThai = false;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Tài khoản đã bị khóa.";
            return RedirectToAction("Index");
        }

        // MỞ KHÓA tài khoản
        public ActionResult Unlock(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.NguoiDungs.Find(id);
            if (user == null)
                return HttpNotFound();

            user.TrangThai = true;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Tài khoản đã được mở khóa.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
