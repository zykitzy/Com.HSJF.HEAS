using Com.HSJF.Infrastructure.DoMain;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class HouseDetail : EntityModel
    {
        public HouseDetail()
        {
            this.EstimateSources = new List<EstimateSource>();
        }
        public string ID { get; set; }
        public decimal? AssessedValue { get; set; }
        public string Accout { get; set; }
        public decimal? TotalHeight { get; set; }
        public string CompletionDate { get; set; }
        public string HouseType { get; set; }
        public string LandType { get; set; }
        public string ServiceCondition { get; set; }
        public string RepairSituation { get; set; }
        public string Collateral { get; set; }
        public string LimitInfo { get; set; }
        public string Description { get; set; }
        //以下 2-16-06-16 第一次测试之后新增
        //20160909 房屋照片修改为其他照片
        public string HousePhotoFile { get; set; }
        //以下 2-16-06-16 第一次测试之后新增
        public string HouseReportFile { get; set; }
        public string BaseAuditID { get; set; }
        public string CollateralID { get; set; }

        //20160908 大改
        /// <summary>
        /// 实际所处层高
        /// </summary>
        public decimal? RealHigh { get; set; }
        /// <summary>
        /// 内部结构是否被破坏
        /// </summary>
        public string IsDamage { get; set; }
        /// <summary>
        /// 实际居住人员
        /// </summary>
        public string RealResident { get; set; }
        /// <summary>
        /// 水电煤账单核实
        /// </summary>
        public string WaterPaymentCheck { get; set; }
        /// <summary>
        /// 税单账单核实
        /// </summary>
        public string TaxPaymentCheck { get; set; }
        /// <summary>
        ///是否满二唯一
        /// </summary>
        public string Man2Wei1 { get; set; }
        /// <summary>
        /// 是否有18以下70以上人员居住
        /// </summary>
        public string SpecialResident { get; set; }
        /// <summary>
        /// 其他描述情况
        /// </summary>
        public string OtherDescription { get; set; }
        /// <summary>
        /// 对口学校情况
        /// </summary>
        public string SchoolInfo { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalInfo { get; set; }
        /// <summary>
        /// 交通情况
        /// </summary>
        public string TrafficInfo { get; set; }
        /// <summary>
        /// 超市/商场
        /// </summary>
        public string Supermarket { get; set; }
        /// <summary>
        /// 休闲娱乐
        /// </summary>
        public string Recreation { get; set; }
        /// <summary>
        /// 不利场所
        /// </summary>
        public string NegativeSite { get; set; }
        /// <summary>
        /// 小区照片
        /// </summary>

        public string VillagePhotoFile { get; set; }
        /// <summary>
        /// 正门
        /// </summary>
        public string MainGatePhotoFile { get; set; }
        /// <summary>
        /// 大厅
        /// </summary>

        public string ParlourPhotoFile { get; set; }
        /// <summary>
        /// 卧室照片
        /// </summary>
        public string BedroomPhotoFile{ get; set; }

        /// <summary>
        /// 厨房照片
        /// </summary>

        public string KitchenRoomPhotoFile { get; set; }
        /// <summary>
        /// 卫生间照片
        /// </summary>

        public string ToiletPhotoFile { get; set; }




        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public BaseAudit BaseAudit { get; set; }

        public virtual ICollection<EstimateSource> EstimateSources { get; set; }
    }
}
