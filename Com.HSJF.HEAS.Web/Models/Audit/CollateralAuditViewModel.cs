using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Audit;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class CollateralAuditViewModel
    {
        public string ID { get; set; }
        
        [Display(Name = "抵押物类型")]
        [Required]
        public string CollateralType { get; set; }
       
        public string CollateralTypeText { get; set; }
       
        [Display(Name = "抵押物编号")]
        [Required]
        public string HouseNumber { get; set; }
      
        [Display(Name = "抵押物复印件")]
        [Required]
        public string HouseFile { get; set; }
       
        public Dictionary<string, string> HouseFileName { get; set; }
       
        [Display(Name = "楼盘名称")]
        public string BuildingName { get; set; }
       
        [Display(Name = "抵押物地址")]
        [Required]
        public string Address { get; set; }
       
        [Display(Name = "权利人")]
        [Required]
        public string RightOwner { get; set; }
        
        [Display(Name = "抵押物面积")]
        [Required]
        public decimal? HouseSize { get; set; }

        /// <summary>
        /// 房产明细-总楼层
        /// yanminchun 2016-10-17
        /// </summary>
        public decimal? TotalHeight { get; set; }

        [Display(Name = "房屋产调")]
        public string HouseReportFile { get; set; }
       
        public Dictionary<string, string> HouseReportFileName { get; set; }


        //2016-9-08 大改
        [Display(Name = "竣工日期")]
        public string CompletionDate { get; set; }

        [Display(Name = "房屋类型")]
        public string HouseType { get; set; }

        public string HouseTypeText { get; set; }
        [Display(Name = "土地类型")]
        public string LandType { get; set; }

        /// <summary>
        /// 来自于哪个案件环节
        /// </summary>
        public string IsFrom { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
       
        /// <summary>
        /// 审核信息
        /// </summary>
        public virtual BaseAuditViewModel BaseAudit { get; set; }
        public CollateralAuditViewModel CastModel(CollateralAudit model)
        {
            var coll = new CollateralAuditViewModel();
            var dicdal = new DictionaryDAL();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, coll);
            coll.CollateralTypeText = dicdal.GetText(model.CollateralType);
            return coll;
        }
    }
}