using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.Infrastructure.Extensions;
using System;
using System.Linq;

namespace Com.HSJF.HEAS.BLL.Biz
{
    public class BaseCaseBll
    {
        private readonly BaseCaseDAL _baseCaseDal;
        private readonly SalesGroupDAL _salesGroupDal;

        public BaseCaseBll()
        {
            _baseCaseDal = new BaseCaseDAL();
            _salesGroupDal = new SalesGroupDAL();
        }

        /// <summary>
        /// 案件号生成
        /// </summary>
        /// <param name="salesGroupId">销售组Id</param>
        /// <returns>案件号</returns>
        public string GenCaseNumber(string salesGroupId)
        {
            var salesGroup = _salesGroupDal.GetAll().First(p => p.ID == salesGroupId);
            if (salesGroup.IsNull())
            {
                throw new Exception(string.Format("查询不到id为{0}的销售组信息", salesGroupId));
            }

            string maxCaseNumber =
                _baseCaseDal.GetAll().Max(s => s.NewCaseNum.Substring(s.NewCaseNum.Length - 6, s.NewCaseNum.Length));
            if (maxCaseNumber.IsNotNullOrEmpty())
            {
                return "L" + salesGroup.ShortCode + "-" + (Convert.ToInt32(maxCaseNumber) + 1).ToString("d6");
            }

            return string.Format("L{0}-000001", salesGroup.ShortCode);
        }

        /// <summary>
        /// 添加进件
        /// </summary>
        /// <param name="baseCase"></param>
        public void Add(BaseCase baseCase)
        {
            _baseCaseDal.Add(baseCase);
            _baseCaseDal.AcceptAllChange();
        }

        /// <summary>
        /// 删除案件号
        /// </summary>
        /// <param name="baseCase">案件信息</param>
        public void Delete(BaseCase baseCase)
        {
            _baseCaseDal.Delete(baseCase);
            _baseCaseDal.AcceptAllChange();
        }
    }
}