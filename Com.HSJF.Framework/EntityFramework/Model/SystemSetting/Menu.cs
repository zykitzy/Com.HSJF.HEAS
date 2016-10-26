using Com.HSJF.Infrastructure.DoMain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting
{
    public class Menu : EntityModel
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }
        public string Description { get; set; }

        public int State { get; set; }

        public string ParentID { get; set; }

        public virtual Menu ParentMenu { get; set; }

        public virtual ICollection<Menu> ChildMenu { get; set; }
    }
}
