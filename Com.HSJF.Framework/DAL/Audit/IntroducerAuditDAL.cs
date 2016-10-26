using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.Models;

namespace Com.HSJF.Framework.DAL.Audit
{
    public class IntroducerAuditDAL : BaseRepository<IntroducerAudit, HEASContext>
    {
        public IEnumerable<IntroducerAudit> FindByAuditID(string auditid)
        {
            return base.GetAll().Where(t => t.AuditID == auditid);
        }
    }
}
