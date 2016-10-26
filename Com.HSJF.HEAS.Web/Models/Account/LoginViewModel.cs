using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "登录名")]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberBrowser { get; set; }
    }
}
