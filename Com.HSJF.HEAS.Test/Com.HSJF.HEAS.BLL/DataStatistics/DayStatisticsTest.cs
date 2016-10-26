using System;
using System.Diagnostics;
using Com.HSJF.HEAS.BLL.DataStatistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.HSJF.HEAS.Test.Com.HSJF.HEAS.BLL.DataStatistics
{
    [TestClass]
    public class DayStatisticsTest
    {
        [TestMethod]
        public void SendEmialTest()
        {
            new DayStatisticsBll().SendEmail();
        }

        [TestMethod]
        public void TestPathTest()
        {
            //new DayStatistics().TestEncode();
        }
    }
}
