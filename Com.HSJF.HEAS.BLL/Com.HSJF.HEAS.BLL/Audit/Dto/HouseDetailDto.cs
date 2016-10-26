using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class HouseDetailDto
    {
        public string ID { get; set; }

        public string CollateralID { get; set; }

        public string CollateralIDText { get; set; }

        public decimal? AssessedValue { get; set; }

        public string Accout { get; set; }

        public decimal? TotalHeight { get; set; }

        public string CompletionDate { get; set; }

        public string HouseType { get; set; }

        public string HouseTypeText { get; set; }

        public string ServiceCondition { get; set; }

        public string RepairSituation { get; set; }

        public string Collateral { get; set; }

        public string LimitInfo { get; set; }

        public string Description { get; set; }

        public string HousePhotoFile { get; set; }

        public Dictionary<string, string> HousePhotoFileName { get; set; }

        public string HouseReportFile { get; set; }

        public Dictionary<string, string> HouseReportFileName { get; set; }

        public string LendType { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public IEnumerable<EstimateSourceDto> EstimateSources { get; set; }
    }
}
