using Com.HSJF.Infrastructure.DoMain;
using System;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting
{
    public class Menu2Role : EntityModel
    {
        public Guid ID { get; set; }
        public string RoleID { get; set; }
        public string MenuID { get; set; }
    }
}