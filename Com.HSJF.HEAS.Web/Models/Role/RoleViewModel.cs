using Com.HSJF.HEAS.Web.Models.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Models.Role
{
    public class RoleViewModel
    {
        public string ID { get; set; }
        [Display(Name = "角色名称")]
        public string Name { get; set; }
        [Display(Name = "备注信息")]
        public string Description { get; set; }
        //   public IEnumerable<PermissionViewModel> Permissons { get; set; }

        public Com.HSJF.Infrastructure.Identity.Model.Role CastDB(RoleViewModel model)
        {
            return new Infrastructure.Identity.Model.Role()
            {
                ID = model.ID,
                Description = model.Description,
                Name = model.Name,
            };
        }

        public RoleViewModel CastView(Infrastructure.Identity.Model.Role model)
        {
            return new RoleViewModel()
            {
                ID = model.ID,
                Description = model.Description,
                Name = model.Name,
            };
        }
    }
}
