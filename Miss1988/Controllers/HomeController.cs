using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Miss1988.Models;
using Miss1988.Models.ViewModel;

public class HomeController : Controller
{
    private CNPMEntities1 db = new CNPMEntities1();

    public ActionResult Index()
    {
        return View();
    }

    public PartialViewResult PVCategories()
    {
        var categories = db.NhomMons.Select(n => new CategoryVM
        {
            NhomMonID = n.NhomMonID,
            TenNhom = n.TenNhom,
            MoTa = n.MoTa,
           ImageUrl = n.ImageUrl // ✅ Thêm dòng này

        }).ToList();

        return PartialView("_PVCategories", categories);
    }

    public PartialViewResult PVBestSeller()
    {
        var products = db.Mons
                         .OrderByDescending(m => m.SoLuong)
                         .Take(8)
                         .Select(m => new ProductVM
                         {
                             MonID = m.MonID,
                             TenMon = m.TenMon,
                             GiaBan = m.GiaBan ?? 0,
                             MoTa = m.MoTa,
                             ImageUrl = m.ImageUrl // ✅ Thêm dòng này
                         }).ToList();

        return PartialView("_PVBestSeller", products);
    }
    public PartialViewResult PVKhuyenMai()
    {
        var khuyenMaiList = db.KhuyenMais
                              .OrderByDescending(k => k.NgayBatDau)
                              .Take(6)
                              .Select(k => new KhuyenMaiVM
                              {
                                  KhuyenMaiID = k.KhuyenMaiID,
                                  MaVoucher = k.MaVoucher,
                                  MoTa = k.MoTa,
                                  NgayBatDau = k.NgayBatDau,
                                  NgayKetThuc = k.NgayKetThuc,
                                  ImageUrl = k.ImageUrl // nếu có ảnh
                              }).ToList();

        return PartialView("_PVKhuyenMai", khuyenMaiList);
    }
    public ActionResult ProductOptions(int id)
    {
        var product = db.Mons.Find(id);
        if (product == null) return HttpNotFound();

        var vm = new ProductOptionVM
        {
            MonID = product.MonID,
            TenMon = product.TenMon,
            ImageUrl = product.ImageUrl,
            MoTa = product.MoTa,
            GiaBan = product.GiaBan,
            AvailableSizes = new List<SelectListItem>
        {
            new SelectListItem { Text = "🥤 Nhỏ + 0 đ", Value = "Small" },
            new SelectListItem { Text = "🥤 Vừa + 10.000 đ", Value = "Medium" },
            new SelectListItem { Text = "🥤 Lớn + 16.000 đ", Value = "Large" }
        },
            Toppings = new List<SelectListItem>
        {
            new SelectListItem { Text = "Trân châu trắng + 10.000 đ", Value = "Trân châu trắng" },
            new SelectListItem { Text = "Thạch sương sáo + 10.000 đ", Value = "Thạch sương sáo" },
            new SelectListItem { Text = "Đào miếng + 10.000 đ", Value = "Đào miếng" },
            // ... thêm topping tùy theo app
        }
        };

        return View("ProductOptions", vm);
    }
}
