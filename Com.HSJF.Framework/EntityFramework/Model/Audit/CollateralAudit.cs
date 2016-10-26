using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class CollateralAudit : EntityModel
    {
        public string ID { get; set; }
        public string AuditID { get; set; }
        public string CollateralType { get; set; }
        public string HouseNumber { get; set; }
        public string HouseFile { get; set; }
        public string BuildingName { get; set; }
        public string Address { get; set; }
        public string RightOwner { get; set; }
        public decimal? HouseSize { get; set; }
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
        /// yanminchun 2016-10-17
        /// </summary>
        public decimal? TotalHeight { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }
    }
}
