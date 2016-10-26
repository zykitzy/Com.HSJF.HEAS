using Com.HSJF.HEAS.BLL.DataStatistics;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Com.HSJF.HEAS.BLL.Other.Dto;

namespace Com.HSJF.HEAS.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = new DayStatisticsBll().GetDayStatistics(new DateTime(2016, 7, 13));
        }

        [TestMethod]
        public void TestCopy()
        {
            BizController biz = new BizController();
            biz.CopyBaseCase("06f1437e-0d82-4d35-8134-23224a48d9c4");
        }

        //[TestMethod]
        //public void Testunlock()
        //{
        //    RelationStateBLL Relation = new RelationStateBLL();
        //    RelationStateBLLModel model = new RelationStateBLLModel()
        //    {
        //        Number = "4307031993051539510"
        //    };
        //    Relation.UpdateLockRelationState(model);
        //}
    }
}