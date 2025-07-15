using System.ComponentModel.DataAnnotations;
using System;
namespace Miss1988.Models.ViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc!")]
        [Display(Name = "Tên đăng nhập")]
        public string HoTen { get; set; }  // Dùng HoTen làm username

        [Required(ErrorMessage = "Mật khẩu là bắt buộc!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm cả chữ và số.")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? NgayTao { get; set; }
    }
}