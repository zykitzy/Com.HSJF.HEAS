using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.Audit
{
    public class AuditBaseAuditDAL : BaseRepository<BaseAudit, HEASContext>
    {
        public override void Add(BaseAudit entity)
        {
            entity.Version = 0;
            base.Add(entity);
        }

        public override BaseAudit Get(object key)
        {
            var entity = base.Get(key);
            var maxentity = this.GetAll().Where(t=>t.ID == key.ToString()).FirstOrDefault();
            if (entity.Version < 0 || entity.Version != maxentity.Version)
            {
                return null;
            }
            return entity;
        }

        public override IQueryable<BaseAudit> GetAll()
        {
            return base.GetAll().Where(t => t.Version >= 0).GroupBy(t=>t.CaseNum,(x,xs)=> xs.OrderByDescending(a=>a.Version).FirstOrDefault());
        }

        public override void Delete(BaseAudit entity)
        {
            var basecase = base.Get(entity.ID);
            if (basecase != null)
            {
                basecase.Version = -1;
                base.Update(basecase);
            }
        }
    }
}
