using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Xin hay nhap tai khoan")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Xin hay nhap mat khau")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}