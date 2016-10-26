using Com.HSJF.Infrastructure.Identity.Model;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Account
{
    public class UserViewModel
    {
        public string ID { get; set; }

        [Display(Name = "登录名")]
        public string LoginName { get; set; }

        [Display(Name = "显示名称")]
        public string DisplayName { get; set; }

        public UserViewModel CastView(User user)
        {
            UserViewModel model = new UserViewModel();
            model.ID = user.Id;
            model.DisplayName = user.DisplayName;
            model.LoginName = user.UserName;

            return model;
        }

        public User CastDB(UserViewModel user)
        {
            User model = new User();
            model.Id = user.ID;
            model.DisplayName = user.DisplayName;
            model.UserName = user.LoginName;

            return model;
        }
    }
}