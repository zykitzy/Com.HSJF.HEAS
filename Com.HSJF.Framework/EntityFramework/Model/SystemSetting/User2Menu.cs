using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Infrastructure.DoMain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Com.HSJF.Infrastructure.Identity.Model;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting
{
    public class User2Menu : EntityModel
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public string MenuID { get; set; }

    }
}
