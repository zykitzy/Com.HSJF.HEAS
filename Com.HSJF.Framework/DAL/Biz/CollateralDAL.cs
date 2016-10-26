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
    public class CollateralDAL : BaseRepository<Collateral, HEASContext>
    {

        public IEnumerable<Collateral> FindByCaseID(string caseid)
        {
            return base.GetAll().Where(t => t.CaseID == caseid);
        }
    }
}
