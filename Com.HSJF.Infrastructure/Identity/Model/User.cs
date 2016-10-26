using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model
{
    public class User : Microsoft.AspNet.Identity.IUser<string>, IEntityModel
    {
        public User()
        {

        }
        public string Id { get; set; }

        [DisplayName("登录名")]
        public string UserName { get; set; }

        [DisplayName("显示名称")]
        public string DisplayName { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("用户状态")]
        public int UserState { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
