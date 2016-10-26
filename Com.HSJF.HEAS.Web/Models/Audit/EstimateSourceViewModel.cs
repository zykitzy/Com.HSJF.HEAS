using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class EstimateSourceViewModel
    {
        public string ID { get; set; }

        [Display(Name = "估价单位名称")]
        public string EstimateInstitutions { get; set; }

        [Display(Name = "急抛/估价")]
        [Range(1000000, 100000000000000000, ErrorMessage = "房屋估价金额不能小于1000000")]
        public decimal? RushEstimate { get; set; }

        [Display(Name = "提供信息人姓名")]
        public string InformationProvider { get; set; }

        [Display(Name = "联系电话")]
        public string ContactNumber { get; set; }

    
        /// <summary>
        /// 房屋详细ID
        /// </summary>
        public string HouseDetailID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        #region 2016-9-8 大改
        /// <summary>
        /// 凭证
        /// </summary>
        public string CertificateFile { get; set; }
        /// <summary>
        /// 凭证集合
        /// </summary>
        public Dictionary<string, string> CertificateFileName { get; set; }
        #endregion
        /// <summary>
        /// 房屋详细
        /// </summary>
        public virtual HouseDetailViewModel HouseDetail { get; set; }
    }
}