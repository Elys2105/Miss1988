using System.ComponentModel.DataAnnotations;

namespace Miss1988.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc!")]
        [Display(Name = "Tên đăng nhập")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
