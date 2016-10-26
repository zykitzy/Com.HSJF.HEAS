using Com.HSJF.Infrastructure.DoMain;
using System;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public partial class Collateral : EntityModel
    {
        public string ID { get; set; }
        public string CaseID { get; set; }
        public string CollateralType { get; set; }
        public string HouseNumber { get; set; }
        public string HouseFile { get; set; }
        public string BuildingName { get; set; }
        public string Address { get; set; }
        public string RightOwner { get; set; }
        public Nullable<decimal> HouseSize { get; set; }
        //以下 2-16-06-16 第一次测试之后新增
        public string HouseReportFile { get; set; }

        //2016-09-08 大改
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string CompletionDate { get; set; }
        /// <summary>
        /// 土地类型
        /// </summary>
        public string LandType { get; set; }
        /// <summary>
        /// 房屋类型 -字典项
        /// </summary>
        public string HouseType { get; set; }

        /// <summary>
        /// 房产明细-总楼层
        /// yanminchun 2016-10-13
        /// </summary>
        public decimal? TotalHeight { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否锁住
        /// yanminchun 2016-10-19
        /// </summary>
        public bool? IsLocked { get; set; }

        public virtual BaseCase BaseCase { get; set; }
    }
}
