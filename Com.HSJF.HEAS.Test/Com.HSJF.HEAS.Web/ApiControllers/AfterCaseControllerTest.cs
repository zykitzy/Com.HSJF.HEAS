using System;
using System.Linq;
using Com.HSJF.HEAS.Web.ApiControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.HSJF.HEAS.Test.Com.HSJF.HEAS.Web.ApiControllers
{
    [TestClass]
    public class AfterCaseControllerTest
    {
        [TestMethod]
        public void TestGetAfterCaseByData()
        {
            var result =
                new AfterCaseController().GetAfterCaseByData(new DateTime(2016, 7, 1), new DateTime(2016, 8, 1))
                    .ToList();
        }
    }
}
