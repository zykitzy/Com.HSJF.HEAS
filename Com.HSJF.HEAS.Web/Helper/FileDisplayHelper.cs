using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Infrastructure.Identity.Manager;
using Com.HSJF.Infrastructure.Identity.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Helper
{
    public class FileDisplayHelper
    {
        /// <summary>
        /// 是否可以下载或者查看文件
        /// </summary>
        /// <param name="linkkey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CanViewFile(string linkkey, Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            bool flag1 = GetBaseCaseFile(linkkey, user);
            bool flag2 = IsInRole(user.Id).Result;
            //GetBaseAuditFile(linkkey, CurrentUser.Id).Result;
            if (flag1 || flag2)
            {
                return true;
            }
            return false;
        }
        public bool GetBaseCaseFile(string linkkey, Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            BaseCaseDAL bcd = new BaseCaseDAL();
            var model = bcd.GetAuthorizeAndSelf(linkkey, user);
            if (model != null)
            {
                return true;
            }
            return false;
        }

        public Task<bool> IsInRole(string userid)
        {
            return Task.Run(async () =>
            {

                UserStore us = new UserStore();
                UserManager um = new UserManager(us);
                bool flag1 = await um.IsInRoleAsync(userid, "1Audit");
                bool flag2 = await um.IsInRoleAsync(userid, "2Audit");
                bool flag3 = await um.IsInRoleAsync(userid, "Public");
                bool flag4 = await um.IsInRoleAsync(userid, "Finance");
                bool flag5 = await um.IsInRoleAsync(userid, "admin");
                if (flag1 || flag2 || flag3 || flag4 || flag5)
                {
                    return true;
                }
                return false;
            });
        }

        //暂时不用
        public Task<bool> GetBaseAuditFile(string linkkey, string userid)
        {
            return Task.Run(async () =>
            {
                BaseAuditDAL bad = new BaseAuditDAL();
                var model = bad.Get(linkkey);
                if (model != null)
                {
                    UserStore us = new UserStore();
                    UserManager um = new UserManager(us);
                    bool flag1 = await um.IsInRoleAsync(userid, "1Audit");
                    bool flag2 = await um.IsInRoleAsync(userid, "2Audit");
                    bool flag3 = await um.IsInRoleAsync(userid, "Public");
                    bool flag4 = await um.IsInRoleAsync(userid, "Finance");
                    bool flag5 = await um.IsInRoleAsync(userid, "admin");
                    if (flag1 || flag2 || flag3 || flag4 || flag5)
                    {
                        return true;
                    }
                }
                return false;
            });
        }


    }
}
