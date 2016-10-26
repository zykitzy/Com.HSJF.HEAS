using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.Framework.Models;

namespace Com.HSJF.Framework.DAL.Biz
{
    public class IntroducerDAL : BaseRepository<Introducer, HEASContext>
    {

        public IEnumerable<Introducer> FindByCaseID(string caseid)
        {
            return base.GetAll().Where(t => t.CaseID == caseid);
        }
    }
}
