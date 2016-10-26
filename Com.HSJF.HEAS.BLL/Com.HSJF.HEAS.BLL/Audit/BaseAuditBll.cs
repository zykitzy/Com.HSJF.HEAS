using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.HEAS.BLL.Audit.Dto;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Lambda;

namespace Com.HSJF.HEAS.BLL.Audit
{
    public class BaseAuditBll
    {
        private readonly DataPermissionDAL _permissionDal;
        private readonly BaseAuditDAL _auditDal;
        private readonly User2RoleDAL _user2RoleDal;

        public BaseAuditBll()
        {
            _permissionDal = new DataPermissionDAL();
            _auditDal = new BaseAuditDAL();
            _user2RoleDal = new User2RoleDAL();
        }

        public IQueryable<BaseAudit> Query(User user, Expression<Func<BaseAudit, bool>> expression)
        {
            IQueryable<UserRole> roles = _user2RoleDal.GetUser2RoleByUserId(user.Id);
            List<DataPermission> permissions = new List<DataPermission>();

            foreach (var role in roles)
            {
                permissions.AddRange(_permissionDal.GetAll().Where(t => t.RoleID == role.RoleID));
            }

            string[] permissionsId = permissions.Distinct().Select(p => p.DataPermissionID).ToArray();

            expression = expression.And(p => permissionsId.Contains(p.DistrictID));
            expression = expression.And(p => permissionsId.Contains(p.SalesGroupID));
            //expression = expression.Or(p => p.CreateUser == user.UserName);

            return _auditDal.GetAll().Where(expression).OrderByDescending(t => t.CreateTime);
        }

        public IEnumerable<BaseAudit> Query(QueryByPageInput input)
        {
            return _auditDal.ForPage(input.Audits, input.PageSize, input.PageIndex, input.Order, input.Sort);
        }

        public IEnumerable<BaseAudit> Query(string caseNum)
        {
            return _auditDal.GetAll().Where(p => p.CaseNum == caseNum);
        }

        public BaseAudit QueryById(string id)
        {
            return _auditDal.GetAllBase().FirstOrDefault(p => p.ID == id);
        }

        public BaseAudit QueryLeatestById(string id)
        {
            return _auditDal.Get(id);
        }

        public BaseAudit QueryHatsPending(string caseNum)
        {
            var pendingCase = _auditDal.GetbyCaseNum(caseNum);
            if (pendingCase != null && pendingCase.CaseStatus == CaseStatus.HatsPending)
            {
                return pendingCase;
            }
            else
            {
                return null;
            }
        }

        public BaseAudit Query(string caseNum, string caseStatus)
        {
            var auditCase = _auditDal.GetbyCaseNum(caseNum);
            if (auditCase != null && auditCase.CaseStatus == caseStatus)
            {
                return auditCase;
            }
            else
            {
                return null;
            }
        }

      

        /// <summary>
        /// 查询最新案件
        /// </summary>
        /// <param name="caseNum">案件号</param>
        /// <returns>案件信息</returns>
        public BaseAudit QueryLeatest(string caseNum)
        {
            return _auditDal.GetbyCaseNum(caseNum);
        }

        public void Update(BaseAudit baseAudit)
        {
            _auditDal.Update(baseAudit);
            _auditDal.SaveChanges();

        }
    }
}
