using System.Linq;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.Framework.Models;

namespace Com.HSJF.Framework.DAL.Sales
{
    public class SalesGroupDAL : BaseRepository<SalesGroup, HEASContext>
    {
        /// <summary>
        /// 获取所有销售组信息
        /// </summary>
        /// <returns>销售组所有信息</returns>
        public override IQueryable<SalesGroup> GetAll()
        {
            return base.GetAll().Where(p => p.State == 1);
        }
    }
}
