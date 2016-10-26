using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.Biz
{
    public class RelationPersonDAL : BaseRepository<RelationPerson, HEASContext>
    {
        public override void UpdateRange(IEnumerable<RelationPerson> entityList)
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
        public IEnumerable<RelationPerson> FindByCaseID(string caseid)
        {
            return base.GetAll().Where(o => o.CaseID == caseid);
        }
    }
}
