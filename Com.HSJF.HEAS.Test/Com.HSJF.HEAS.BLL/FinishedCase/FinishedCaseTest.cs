using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.HSJF.HEAS.BLL.FinishedCase;

namespace Com.HSJF.HEAS.Test
{
    [TestClass]
    public class FinishedCaseTest
    {

        [TestMethod]
        public void TestMethod1()
        {
            new FinishedCaseBll().GetFinishedCase("e58749a1-c98c-43a6-801d-646bb6e81b5c");
        }
    }
}
