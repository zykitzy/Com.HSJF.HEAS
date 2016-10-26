using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Framework.Models;
using Com.HSJF.Infrastructure.Identity.Manager;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Identity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.SystemSetting
{
    public partial class MenuDAL : BaseRepository<Menu, HEASContext>
    {
        /// <summary>
        /// 获取菜单所对应的权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermission(string menuid)
        {
            Menu2PermissionDAL m2p = new Menu2PermissionDAL();
            PermissionStore store = new PermissionStore();
            //中间表
            var listtep = m2p.GetAll().Where(o => o.MenuID == menuid);
            //权限表
            var plisttep = store.GetAll();

            var list = listtep == null ? new List<Menu2Permission>() : listtep.ToList();
            var plist = plisttep == null ? new List<Permission>() : plisttep.ToList();
            //2个结果集连接
            var result = from i in list
                         join j in plist
                         on i.PermissionID equals j.Id
                         orderby j.State
                         select j
                         ;

            return result;
        }

        public IEnumerable<Menu> FindByUser(string userid)
        {
            return null;
        }

        public void SetMenuPermission(string menuid, params string[] permissionid)
        {
            Menu2PermissionDAL m2p = new Menu2PermissionDAL();
            //先删除所有相关权限
            var mlist = m2p.GetAll().Where(o => o.MenuID == menuid);
            if (mlist != null)
            {
                foreach (var p in mlist)
                {
                    m2p.Delete(p);
                }
            }
            //再增加所有权限
            if (permissionid != null && permissionid.Length > 0)
            {

                foreach (var p in permissionid)
                {
                    Menu2Permission mp = new Menu2Permission();
                    mp.ID = Guid.NewGuid().ToString();
                    mp.MenuID = menuid;
                    mp.PermissionID = p;
                    m2p.Add(mp);
                }

            }

            m2p.AcceptAllChange();
        }


        public bool DeleteMenu(string menuid)
        {
            var chidlist = base.GetAll().Where(o => o.ParentID == menuid);
            if (chidlist.Any())
            {
                return false;
            }
            Menu2PermissionDAL m2p = new Menu2PermissionDAL();
            var m2plist = m2p.GetAll().Where(o => o.MenuID == menuid);
            foreach (var m in m2plist)
            {
                m2p.Delete(m);
            }
            var model = base.Get(menuid);
            base.Delete(model);
            AcceptAllChange();
            return true;
        }

    }
}
