using Com.HSJF.Framework.DAL.DTODAL;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Audit.DTO;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Lambda;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using static Com.HSJF.Framework.DAL.DictionaryType;

namespace Com.HSJF.Framework.DAL.CaseALL
{
    public class ALLCaseDAL
    {
        public IEnumerable<BaseAuditDTO> GetAll(Infrastructure.Identity.Model.User user)
        {
            var pers = GetDataPermission(user.Id);
            BaseAuditDTODAL bad = new BaseAuditDTODAL();
            var basrauditlist = bad.GetAllBySql("SELECT * FROM baseauditview");
            var ulist = basrauditlist.Where(s => pers.Contains(s.SalesGroupID));
            if (pers.Contains(CaseMode.WeiXuanZe) && pers.Contains(CaseMode.ZiYouZiJin))
            {
                ulist = ulist.Where(t => t.CaseMode == CaseMode.ZiYouZiJin || pers.Contains(t.ThirdParty) || t.ThirdParty == null);
            }
            else if (pers.Contains(CaseMode.ZiYouZiJin))
            {
                ulist = ulist.Where(t => t.CaseMode == CaseMode.ZiYouZiJin || pers.Contains(t.ThirdParty));
            }
            else if (pers.Contains(CaseMode.WeiXuanZe))
            {
                ulist = ulist.Where(t => pers.Contains(t.ThirdParty) || (t.ThirdParty == null && t.CaseMode == null));
            }
            else
            {
                ulist = ulist.Where(t => pers.Contains(t.ThirdParty));
            }
            ulist = ulist.OrderByDescending(t => t.CaseNum);

            return ulist;
        }

        private string[] GetDataPermission(string userid)
        {
            DataPermissionDAL dataPermissionDal = new DataPermissionDAL();
            User2RoleDAL user2RoleDal = new User2RoleDAL();
            IQueryable<UserRole> roles = user2RoleDal.GetUser2RoleByUserId(userid);
            List<DataPermission> permissions = new List<DataPermission>();

            foreach (var role in roles)
            {
                permissions.AddRange(dataPermissionDal.GetAll().Where(t => t.RoleID == role.RoleID));
            }

            var permissionsId = permissions.Distinct().Select(p => p.DataPermissionID);
            if (permissionsId.Any())
            {
                return permissionsId.ToArray();
            }

            return new string[] { };
        }
    }
}