using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Biz;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class CollateralViewModel
    {
        public string ID { get; set; }

        public string CaseID { get; set; }

        [Display(Name = "抵押物类型")]
        public string CollateralType { get; set; }

        public string CollateralTypeText { get; set; }

        [Display(Name = "抵押物编号")]
        [Required]
        public string HouseNumber { get; set; }

        [Display(Name = "抵押物复印件")]
        public string HouseFile { get; set; }

        public Dictionary<string, string> HouseFileName { get; set; }

        [Display(Name = "楼盘名称")]
        public string BuildingName { get; set; }

        [Display(Name = "抵押物地址")]
        public string Address { get; set; }

        [Display(Name = "权利人")]
        public string RightOwner { get; set; }

        [Display(Name = "抵押物面积")]
        public Nullable<decimal> HouseSize { get; set; }

        /// <summary>
        /// 2016-9-08 大改
        /// </summary>

        [Display(Name = "竣工日期")]
        public string CompletionDate { get; set; }

        [Display(Name = "房屋类型")]
        public string HouseType { get; set; }

        public string HouseTypeText { get; set; }
        [Display(Name = "土地类型")]
        public string LandType { get; set; }

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

        public CollateralViewModel CastModel(Collateral model)
        {
            var coll = new CollateralViewModel();
            var dicdal = new DictionaryDAL();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, coll);
            coll.CollateralTypeText = dicdal.GetText(model.CollateralType);
            return coll;
        }
    }
}