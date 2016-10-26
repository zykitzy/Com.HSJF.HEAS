
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class EstimateSourceDto
    {
        public string ID { get; set; }

        public string EstimateInstitutions { get; set; }

        public decimal? RushEstimate { get; set; }

        public string InformationProvider { get; set; }

        public string ContactNumber { get; set; }

        /// <summary>
        /// 房屋详细ID
        /// </summary>
        public string HouseDetailID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
