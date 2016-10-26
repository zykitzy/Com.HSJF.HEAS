using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.Models;
using Com.HSJF.Infrastructure.DoMain;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;

namespace Com.HSJF.Framework.DAL
{
    public class BaseDAL<T> : BaseRepository<T, HEASContext>
        where T : EntityModel
    {
        public IList<T> ForPage(IQueryable<T> query, int pageSize, int pageIndex, string order, string sort = "desc")
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (string.IsNullOrEmpty(order))
            {
                order = "CaseNum";
            }
            if (sort == null)
            {
                sort = "desc";
            }
            string ordomg = order + " " + sort;
            query = query.OrderBy(ordomg).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return query.ToList();
        }

        public IList<T> ForPageOrderByCaseStatus(IQueryable<T> query, int pageSize, int pageIndex, string order, string sort = "desc")
        {
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (string.IsNullOrEmpty(order))
            {
                order = "CaseNum";
            }
            if (sort == null)
            {
                sort = "desc";
            }
            string ordomg = order + " " + sort;

            query = query.OrderBy("CaseStatus DESC,CaseNum DESC").Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return query.ToList();
        }

        /// <summary>
        /// 获取所有数据权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string[] GetDataPermission(User user)
        {
            var dataPermissionDal = new DataPermissionDAL();
            var user2RoleDal = new User2RoleDAL();
            var roles = user2RoleDal.GetUser2RoleByUserId(user.Id);
            var permissions = new List<DataPermission>();

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