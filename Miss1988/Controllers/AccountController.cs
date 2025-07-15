using Miss1988.Models;
using Miss1988.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Miss1988.Controllers
{
    public class AccountController : Controller
    {
        private CNPMEntities1 db = new CNPMEntities1();

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View(new LoginVM());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.Password);

                var user = db.NguoiDungs
                    .FirstOrDefault(u => u.HoTen == model.HoTen && u.MatKhau == hashedPassword);

                if (user != null && user.TrangThai == true)
                {
                    // Gán session
                    Session["UserID"] = user.NguoiDungID;
                    Session["Username"] = user.HoTen;
                    Session["HoTen"] = user.HoTen;
                    Session["QuyenID"] = user.QuyenID;
                    Session["TenQuyen"] = user.PhanQuyen?.TenQuyen;

                    // Ghi cookie
                    FormsAuthentication.SetAuthCookie(user.HoTen, model.RememberMe);

                    TempData["SuccessMessage"] = "Đăng nhập thành công!";

                    // PHÂN QUYỀN REDIRECT
                    switch (user.PhanQuyen?.TenQuyen)
                    {
                        case "Admin":
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                        case "NhanVien":
                            return RedirectToAction("Index", "Staff", new { area = "Staff" });

                        default:
                            return RedirectToAction("Index", "Home"); // Khách hàng
                    }
                }

                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng hoặc tài khoản đã bị khóa.");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                bool exists = db.NguoiDungs.Any(u => u.HoTen == model.HoTen);
                if (exists)
                {
                    ModelState.AddModelError("HoTen", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                // Gán quyền mặc định: QuyenID = 3 (ví dụ là "Khách hàng")
                int defaultQuyenID = db.PhanQuyens
                    .Where(p => p.TenQuyen == "Khách hàng" || p.TenQuyen == "User")
                    .Select(p => p.QuyenID)
                    .FirstOrDefault();

                var user = new NguoiDung
                {
                    HoTen = model.HoTen,
                    MatKhau = HashPassword(model.MatKhau),
                    Email = model.Email,
                    QuyenID = defaultQuyenID, // Gán mặc định ở đây
                    NgayTao = DateTime.Now,
                    TrangThai = true
                };

                db.NguoiDungs.Add(user);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // Helper: Mã hóa SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
