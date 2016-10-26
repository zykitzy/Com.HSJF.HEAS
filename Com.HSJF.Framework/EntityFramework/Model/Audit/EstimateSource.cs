using Com.HSJF.Infrastructure.DoMain;
using System;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class EstimateSource : EntityModel
    {
        public string ID { get; set; }
        public string EstimateInstitutions { get; set; }
        public Nullable<decimal> RushEstimate { get; set; }
        public string InformationProvider { get; set; }
        public string ContactNumber { get; set; }
        public string HouseDetailID { get; set; }

        //20160909 大改
        /// <summary>
        /// 凭证
        /// </summary>
        public string CertificateFile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual HouseDetail HouseDetail { get; set; }
    }
}
