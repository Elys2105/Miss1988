using Miss1988.Models;
using Miss1988.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Miss1988.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "ShoppingCart";
        private CNPMEntities1 db = new CNPMEntities1();

        // Lấy giỏ hàng từ session
        private CartVM GetCart()
        {
            var cart = Session[CartSessionKey] as CartVM;
            if (cart == null)
            {
                cart = new CartVM();
                Session[CartSessionKey] = cart;
            }
            return cart;
        }

        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, string returnUrl)
        {
            var product = db.Mons.Find(productId);
            if (product == null) return HttpNotFound();

            var cart = GetCart();
            var existing = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItemVM
                {
                    ProductId = product.MonID,
                    ProductName = product.TenMon,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1,
                    UnitPrice = product.GiaBan ?? 0
                });
            }

            // Nếu có returnUrl → chuyển hướng
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult RemoveFromCart(int productId, string extraInfo)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId && x.ExtraInfo == (extraInfo ?? ""));
            if (item != null)
            {
                cart.Items.Remove(item);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity, string extraInfo)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId && x.ExtraInfo == (extraInfo ?? ""));
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Cart = GetCart();
                return View(model);
            }

            // 1. Lưu khách hàng
            var customer = new KhachHang
            {
                HoTen = model.CustomerName,
                Email = model.Email,
                SoDienThoai = model.Phone,
                DiaChi = model.Address,
                NgayDangKy = DateTime.Now,
                DiemTichLuy = 0 // nếu có
            };
            db.KhachHangs.Add(customer);
            db.SaveChanges();

            // 2. Lưu đơn hàng
            var order = new ORDER
            {
                NgayTao = DateTime.Now,
                TrangThai = "Chưa xử lý",
                KhachHangID = customer.KhachHangID,
                KhuyenMaiID = null, // hoặc gán nếu bạn có mã khuyến mãi
                NguoiDungID = null, // nếu không phải admin tạo
                ChiNhanhID = null   // nếu có nhiều chi nhánh
            };
            db.ORDERs.Add(order);
            db.SaveChanges();

            // 3. Lưu chi tiết đơn hàng
            foreach (var item in model.Cart.Items)
            {
                db.ChiTietOrders.Add(new ChiTietOrder
                {
                    OrderID = order.OrderID,
                    MonID = item.ProductId,
                    SoLuong = item.Quantity,
                    TrangThai = "Chờ xử lý",
                    ThoiGianHoanTat = null
                });
            }

            db.SaveChanges();

            // 4. Xóa giỏ hàng
            Session[CartSessionKey] = null;

            return RedirectToAction("Success");
        }


        public ActionResult Success()
        {
            var latestOrder = db.ORDERs.OrderByDescending(o => o.NgayTao).FirstOrDefault();
            if (latestOrder != null)
            {
                TimeSpan elapsed = DateTime.Now - latestOrder.NgayTao;

                if (elapsed.TotalSeconds < 60)
                    latestOrder.TrangThai = "Đang xác nhận";
                else if (elapsed.TotalSeconds < 180)
                    latestOrder.TrangThai = "Đang chuẩn bị hàng";
                else if (elapsed.TotalSeconds < 300)
                    latestOrder.TrangThai = "Đang giao";
                else
                    latestOrder.TrangThai = "Đã hoàn thành";

                db.SaveChanges();
                return View(latestOrder);
            }

            return View(); // fallback nếu không có đơn
        }

        [HttpPost]
        public ActionResult AddToCartWithOptions(int productId, string selectedSize, string[] selectedToppings)
        {
            var product = db.Mons.Find(productId);
            if (product == null) return HttpNotFound();

            var cart = GetCart();
            string extraInfo = $"Size: {selectedSize}";
            if (selectedToppings != null && selectedToppings.Length > 0)
            {
                extraInfo += "; Topping: " + string.Join(", ", selectedToppings);
            }

            var existing = cart.Items.FirstOrDefault(x => x.ProductId == productId && x.ExtraInfo == extraInfo);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItemVM
                {
                    ProductId = product.MonID,
                    ProductName = product.TenMon,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1,
                    UnitPrice = product.GiaBan ?? 0,
                    ExtraInfo = extraInfo
                });
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult CheckoutWithOptions(int? productId, string selectedSize, string[] selectedToppings)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(400, "Thiếu productId");
            }

            var product = db.Mons.Find(productId.Value);
            if (product == null) return HttpNotFound();

            string extraInfo = $"Size: {selectedSize}";
            if (selectedToppings != null && selectedToppings.Length > 0)
            {
                extraInfo += "; Topping: " + string.Join(", ", selectedToppings);
            }

            var cart = new CartVM();
            cart.Items.Add(new CartItemVM
            {
                ProductId = product.MonID,
                ProductName = product.TenMon,
                ImageUrl = product.ImageUrl,
                Quantity = 1,
                UnitPrice = product.GiaBan ?? 0,
                ExtraInfo = extraInfo
            });

            var model = new CheckoutVM
            {
                Cart = cart
            };

            return View("Checkout", model);
        }

    }
}