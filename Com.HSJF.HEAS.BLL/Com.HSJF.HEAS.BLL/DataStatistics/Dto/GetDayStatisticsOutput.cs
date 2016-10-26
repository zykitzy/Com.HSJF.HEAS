using System;
using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.DataStatistics.Dto
{
    public class GetDayStatisticsOutput
    {
        public GetDayStatisticsOutput()
        {
            DayStatistics = new List<DayStatisticsDto>();
        }

        /// <summary>
        /// 统计的日期
        /// </summary>
        public DateTime StatisticsDate { get; set; }

        /// <summary>
        /// 具体数据 
        /// </summary>
        public List<DayStatisticsDto> DayStatistics { get; set; }

    }
}
