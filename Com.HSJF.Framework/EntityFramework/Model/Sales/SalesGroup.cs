using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales
{
    public partial class SalesGroup : EntityModel
    {
        public SalesGroup()
        {
            this.SalesMen = new List<SalesMan>();
        }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 统一社会信用码/注册号
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 公司简码
        /// </summary>
        public string ShortCode { get; set; }

        /// <summary>
        /// 状态(1:启用)
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public string DistrictID { get; set; }

        /// <summary>
        /// 区域信息
        /// </summary>
        public virtual District District { get; set; }

        /// <summary>
        /// 组内销售员
        /// </summary>
        public virtual ICollection<SalesMan> SalesMen { get; set; }
    }
}
