using Com.HSJF.HEAS.Web.Controllers;
using Com.HSJF.HEAS.Web.Models.CaseAll;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Com.HSJF.HEAS.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            CaseAllPageRequestModel request = new CaseAllPageRequestModel()
            {
                PageSize = 100,
            };
            CaseAllController call = new CaseAllController();
            var list = call.GetPageIndex(request);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string id = "a1388869-3cec-4957-b2b9-ebad26373de6";
            CaseAllController call = new CaseAllController();
            var list = call.GetCaseDetails(id);
        }
        [TestMethod]
        public void TestMethod3()
        {
            AccountController acc = new AccountController();
           var menu= acc.GetMenuList("60255446-9996-4aa0-b71f-efd9279ff4d6");
        }
        [TestMethod]
        public void TestMethod4()
        {
            //添加菜单
            AccountController acc = new AccountController();
            var menu = acc.SetUserMenu("60255446-9996-4aa0-b71f-efd9279ff4d6", "f51c4ea3-3177-4e09-ac47-d2de1c436d46");
        }
        [TestMethod]
        public void TestMethod5()
        {
            //删除菜单
            AccountController acc = new AccountController();
            var menu = acc.RemoveUserMenu("60255446-9996-4aa0-b71f-efd9279ff4d6", "f51c4ea3-3177-4e09-ac47-d2de1c436d46");
        }
    }
}