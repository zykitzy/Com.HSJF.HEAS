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
    public class AddressAuditDAL : BaseRepository<AddressAudit, HEASContext>
    {
        public IEnumerable<AddressAudit> GetByPersonID(string personid)
        {
            return base.GetAll().Where(t => t.PersonID == personid);
        }

        public override void UpdateRange(IEnumerable<AddressAudit> entityList)
        {
            foreach (var t in entityList)
            {
                var model = base.Get(t.ID);
                if (model == null)
                {
                    base.Add(t);
                }
                else
                {
                    //    Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(t, model);
                    base.Update(t);
                }

            }
        }
    }
}
