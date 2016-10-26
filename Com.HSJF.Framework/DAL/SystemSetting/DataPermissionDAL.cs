using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.SystemSetting
{
    public class DataPermissionDAL : BaseRepository<DataPermission, HEASContext>
    {
        public void SaveDataPermission(string roleid, string datapermission)
        {
            var uplist = base.GetAll().Where(t => t.RoleID == roleid);
            foreach (var t in uplist)
            {
                base.Delete(t);
            }
            var plist = datapermission.Trim(',').Split(',').Select(t => new DataPermission() { ID = Guid.NewGuid().ToString(), DataPermissionID = t, RoleID = roleid });
            base.AddRange(plist);
            AcceptAllChange();
        }
    }
}
