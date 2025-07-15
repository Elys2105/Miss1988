using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Miss1988.Models.ViewModel
{
    public class CheckoutVM
    {
        // Thông tin khách hàng
        [Required]
        public string CustomerName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string OrderNote { get; set; }

        public CartVM Cart { get; set; } = new CartVM();
    }

}