using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Audit;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class HouseDetailViewModel
    {
        public string ID { get; set; }

        public string CollateralID { get; set; }
        public string CollateralIDText { get; set; }

        [Display(Name = "评估价")]
        [Range(1000000, 100000000000000000, ErrorMessage = "房屋评估价金额不能小于1000000")]
        public decimal? AssessedValue { get; set; }

        [Display(Name = "户口所在情况核实/抵押物户口")]
        public string Accout { get; set; }

        [Display(Name = "总楼层")]
        public decimal? TotalHeight { get; set; }
        public string HouseTypeText { get; set; }

        [Display(Name = "使用情况")]
        public string ServiceCondition { get; set; }

        [Display(Name = "装修、保养、破损、违章搭建")]
        public string RepairSituation { get; set; }

        [Display(Name = "抵押情况")]
        public string Collateral { get; set; }

        [Display(Name = "限制信息")]
        public string LimitInfo { get; set; }


        [Display(Name = "备注")]
        public string Description { get; set; }

        [Display(Name = "其他照片")]
        public string HousePhotoFile { get; set; }

        public Dictionary<string, string> HousePhotoFileName { get; set; }

        [Display(Name = "房屋产调")]
        public string HouseReportFile { get; set; }

        public Dictionary<string, string> HouseReportFileName { get; set; }

        #region 引用抵押物
        [Display(Name = "楼盘名称")]
        public string BuildingName { get; set; }

        [Display(Name = "房屋地址")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "房屋面积")]
        [Required]
        public decimal? HouseSize { get; set; }

        [Display(Name = "土地类型")]
        public string LandType { get; set; }

        [Display(Name = "竣工日期")]
        public string CompletionDate { get; set; }

        [Display(Name = "房屋类型")]
        public string HouseType { get; set; }

        #endregion

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        #region 20160909新增
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
        /// 小区照片集合
        /// </summary>
        public Dictionary<string, string> VillagePhotoFileName { get; set; }

        /// <summary>
        /// 正门
        /// </summary>
        public string MainGatePhotoFile { get; set; }
        public Dictionary<string, string> MainGatePhotoFileName { get; set; }
        /// <summary>
        /// 大厅
        /// </summary>
        public string ParlourPhotoFile { get; set; }
        public Dictionary<string, string> ParlourPhotoFileName { get; set; }
        /// <summary>
        /// 卧室照片
        /// </summary>
        public string BedroomPhotoFile { get; set; }
        public Dictionary<string, string> BedroomPhotoFileName { get; set; }
        /// <summary>
        /// 厨房照片
        /// </summary>
        public string KitchenRoomPhotoFile { get; set; }
        public Dictionary<string, string> KitchenRoomPhotoFileName { get; set; }
        /// <summary>
        /// 卫生间照片
        /// </summary>
        public string ToiletPhotoFile { get; set; }
        public Dictionary<string, string> ToiletPhotoFileName { get; set; }
        #endregion

        /// <summary>
        /// 审核详细
        /// </summary>
        public BaseAuditViewModel BaseAudit { get; set; }

        /// <summary>
        /// 估价来源
        /// </summary>
        public virtual IEnumerable<EstimateSourceViewModel> EstimateSources { get; set; }
        public HouseDetailViewModel CastModel(HouseDetail model)
        {
            var house = new HouseDetailViewModel();
            var dicdal = new DictionaryDAL();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, house);
            house.HouseTypeText = dicdal.GetText(model.HouseType);
            return house;
        }
    }
}