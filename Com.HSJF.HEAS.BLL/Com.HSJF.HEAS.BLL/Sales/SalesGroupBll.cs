using System;
using System.Collections.Generic;
using System.Linq;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Sales;

namespace Com.HSJF.HEAS.BLL.Sales
{
    public class SalesGroupBll
    {
        private readonly SalesGroupDAL _salesGroup;

        public SalesGroupBll()
        {
            _salesGroup = new SalesGroupDAL();
        }

        #region Public Methods

        public IEnumerable<SalesGroup> GetAll()
        {
            return _salesGroup.GetAll().ToList();
        }


        public SalesGroup Query(string id)
        {
            return _salesGroup.Get(Guid.Parse(id));
        }
        #endregion


    }
}
