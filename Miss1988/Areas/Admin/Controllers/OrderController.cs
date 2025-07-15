using Miss1988.Models;
using Miss1988.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Miss1988.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: Admin/Order
        public ActionResult Index(string status, DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.StatusList = new SelectList(new[] {
                "Tất cả", "Chưa xử lý", "Đang xử lý", "Đang giao", "Hoàn thành", "Hủy"
            }, status ?? "Tất cả");

            var orders = db.ORDERs.Include(o => o.KhachHang).AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
            {
                orders = orders.Where(o => o.TrangThai == status);
            }

            if (fromDate.HasValue)
                orders = orders.Where(o => o.NgayTao >= fromDate.Value);

            if (toDate.HasValue)
                orders = orders.Where(o => o.NgayTao <= toDate.Value);

            var viewModelList = orders.OrderByDescending(o => o.NgayTao)
                .ToList()
                .Select(o => new OrderVM
                {
                    Order = o,
                    PhuongThucThanhToan = "COD" // default, hoặc map từ dữ liệu thực
                }).ToList();

            return View(viewModelList);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.ORDERs
                .Include(o => o.KhachHang)
                .Include(o => o.ChiTietOrders.Select(c => c.Mon))
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null) return HttpNotFound();

            var vm = new OrderVM
            {
                Order = order,
                PhuongThucThanhToan = "COD", // default nếu không có trong DB
                GhiChu = "" // nếu không lưu trong DB
            };

            return View(vm);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.ORDERs.Find(id);
            if (order == null) return HttpNotFound();

            var vm = new OrderVM
            {
                Order = order,
                TrangThai = order.TrangThai
            };

            return View(vm);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderVM model)
        {
            if (ModelState.IsValid)
            {
                var order = db.ORDERs.Find(model.Order.OrderID); // ✅ đúng entity
                if (order == null) return HttpNotFound();

                order.TrangThai = model.TrangThai;
                // Nếu có: order.GhiChu = model.GhiChu;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Order/Cancel/5
        public ActionResult Cancel(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.ORDERs.Include(o => o.KhachHang).FirstOrDefault(o => o.OrderID == id);
            if (order == null) return HttpNotFound();

            return View(order);
        }

        // POST: Admin/Order/CancelConfirmed/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var order = db.ORDERs.Find(id);
            if (order != null)
            {
                order.TrangThai = "Hủy";
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
