using System;
using System.Web.Http;
using Com.HSJF.HEAS.BLL.DataStatistics;
using Com.HSJF.HEAS.BLL.DataStatistics.Dto;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// 按日统计个分公司的案件本金
    /// </summary>
    public class DayStatisticsController : ApiController
    {
        private DayStatisticsBll _dayStatistics;

        public DayStatisticsController()
        {
            _dayStatistics = new DayStatisticsBll();
        }

        /// <summary>
        /// 统计指定日期的各分公司案件本金之和
        /// </summary>
        /// <param name="date">指定日期(yyyy/MM/dd)</param>
        /// <param name="isUnitChange">单位是否转换成万元</param>
        /// <returns>本金信息</returns>
        public GetDayStatisticsOutput Post(string date, bool isUnitChange = false)
        {
            if (isUnitChange)
            {
                return _dayStatistics.GetDayStatisticsV2(Convert.ToDateTime(date));
            }
            else
            {
                return _dayStatistics.GetDayStatistics(Convert.ToDateTime(date));
            }
        }
    }
}
