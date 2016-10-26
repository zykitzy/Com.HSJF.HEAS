using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Models.Account
{
    public class ChangePasswordViewModel
    {
        public string id { get; set; }


        [Required]
        [Display(Name = "当前密码")]
        public string oldPass { get; set; }
        [Required]
        [Display(Name = "新密码")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}
