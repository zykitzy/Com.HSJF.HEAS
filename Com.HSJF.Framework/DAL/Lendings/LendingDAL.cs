using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.Framework.Models;
using Com.HSJF.Infrastructure.Lambda;
using System;
using System.Linq;

namespace Com.HSJF.Framework.DAL.Lendings
{
    public class LendingDAL : BaseDAL<Lending>
    {
        public override void Update(Lending entity)
        {
            var mor = Get(entity.ID);
            if (mor == null)
            {
                Add(entity);
            }
            else
            {
                base.Update(entity);
            }
        }

        public override Lending Get(object key)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            var curr = bd.Get(key);
            if (curr != null)
            {
                var aud = bd.GetbyCaseSataus(curr.NewCaseNum, CaseStatus.Lending);
                if (aud != null)
                {
                    return base.Get(aud.ID);
                }
            }
            return null;
        }

        /// <summary>
        /// 获取限制数据权限后的数据
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Lending GetAuthorizeAndSelf(object key, Infrastructure.Identity.Model.User user)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            var curr = bd.GetAuthorizeAndSelf(key, user);
            if (curr != null)
            {
                var aud = bd.GetbyCaseSataus(curr.NewCaseNum, CaseStatus.Lending);
                if (aud != null)
                {
                    return base.Get(aud.ID);
                }
            }
            return null;
        }

        public Lending GetHIS(object key)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            var curr = bd.Get(key);
            if (curr != null)
            {
                var aud = bd.GetbyCaseSataus(curr.NewCaseNum, CaseStatus.AfterCase);
                if (aud != null)
                {
                    return base.Get(aud.ID);
                }
            }
            return null;
        }


        public bool SubmitCase(Lending entity, string creatUser, string description)
        {
            this.Update(entity);
            AuditHelp ah = new AuditHelp();
            BaseAuditDAL bad = new BaseAuditDAL();
            var baseaduit = bad.Get(entity.ID);
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.Lending)
            {
                //    baseaduit.CreateTime = DateTime.Now;
                baseaduit.Description = description;
                //   baseaduit.CreateUser = creatUser;
                ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.AfterCase);
                return true;
            }
            return false;
        }

        public bool RejectCase(string id, Infrastructure.Identity.Model.User user)
        {
            AuditHelp ah = new AuditHelp();
            BaseAuditDAL bad = new BaseAuditDAL();
            var baseaduit = bad.GetAuthorizeAndSelf(id, user);
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.Lending)
            {
                //     baseaduit.CreateUser = createUser;
                ah.CopyBaseAudit(baseaduit, user.UserName, CaseStatus.SpecialClose);
                return true;
            }
            return false;
        }

        public IQueryable<BaseAudit> GetAll(DateTime? start, DateTime? end)
        {
            BaseAuditDAL bd = new BaseAuditDAL();

            var modellist = bd.GetHasStatus(CaseStatus.Lending);
            if (start != null)
            {
                var starttime = new DateTime(start.Value.Year, start.Value.Month, start.Value.Day).AddSeconds(-1);
                modellist = modellist.Where(t => t.LendingDate >= starttime);
            }
            if (end != null)
            {
                var endtime = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day).AddDays(1);
                modellist = modellist.Where(t => t.LendingDate < endtime);
            }

            return modellist;
        }

        /// <summary>
        /// 获取限制数据权限后的数据
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IQueryable<BaseAudit> GetAllAuthorizeAndSelf(DateTime? start, DateTime? end, Infrastructure.Identity.Model.User user)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            var pers = GetDataPermission(user);
            var predicate = PredicateBuilder.True<BaseAudit>();
            predicate = predicate.And(testc => pers.Contains(testc.DistrictID));
            predicate = predicate.And(testc => pers.Contains(testc.SalesGroupID));
            predicate = predicate.Or(t => t.CreateUser == user.UserName);

            var modellist = bd.GetHasStatus(CaseStatus.Lending);
            modellist = modellist.Where(predicate);
            if (start != null)
            {
                var starttime = new DateTime(start.Value.Year, start.Value.Month, start.Value.Day).AddSeconds(-1);
                modellist = modellist.Where(t => t.LendingDate >= starttime);
            }
            if (end != null)
            {
                var endtime = new DateTime(end.Value.Year, end.Value.Month, end.Value.Day).AddDays(1);
                modellist = modellist.Where(t => t.LendingDate < endtime);
            }

            return modellist;
        }
    }
}