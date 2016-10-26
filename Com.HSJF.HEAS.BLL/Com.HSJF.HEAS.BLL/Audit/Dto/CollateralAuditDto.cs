using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class CollateralAuditDto
    {
        public string ID { get; set; }

        public string CollateralType { get; set; }

        public string CollateralTypeText { get; set; }

        public string HouseNumber { get; set; }

        public string HouseFile { get; set; }

        public Dictionary<string, string> HouseFileName { get; set; }

        public string BuildingName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 权利人
        /// </summary>

        public string RightOwner { get; set; }

        /// <summary>
        /// 抵押物面积
        /// </summary>
        public decimal? HouseSize { get; set; }

        /// <summary>
        /// 房屋产证
        /// </summary>
        public string HouseReportFile { get; set; }

        /// <summary>
        /// 房屋产证名称
        /// </summary>
        public Dictionary<string, string> HouseReportFileName { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
