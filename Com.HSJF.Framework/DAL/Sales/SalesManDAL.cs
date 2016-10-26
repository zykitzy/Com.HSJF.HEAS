using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.Sales
{
    public class SalesManDAL : BaseRepository<SalesMan, HEASContext>
    {
        public SalesMan FindBySalesID(string id)
        {
          return base.GetAll().Where(s => s.ID == id).FirstOrDefault();
        }

    }
}
