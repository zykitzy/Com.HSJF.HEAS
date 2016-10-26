using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Com.HSJF.Framework.DAL.Audit
{
    public class RelationPersonAuditDAL : BaseRepository<RelationPersonAudit, HEASContext>
    {
        public IEnumerable<RelationPersonAudit> FindByCaseID(string auditid)
        {
            return base.GetAll().Where(o => o.AuditID == auditid);
        }
    }
}