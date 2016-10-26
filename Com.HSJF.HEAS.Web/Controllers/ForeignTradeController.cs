using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Models.ForeignTrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    public class ForeignTradeController : BaseController
    {
        [HttpPost]
        public ActionResult Post2ForeignTrade(List<BaseAuditViewModel> baseAuditList)
        {
            var address = "";
            var transM = new ApplicationImport(baseAuditList);

            //var result = ;
            //var br = new BaseResponse<object>()
            //{
            //    Data = result.Data,
            //    Message = new[] { result.Message },
            //    Status = result.Status
            //};

            return Json(null);
        }

        [HttpPost]
        public ActionResult Post2ForeignTrade13(string caseNum)
        {
            var address = "";

            //BaseAuditDAL dal = new BaseAuditDAL();
            //List<string> nums = new List<string>();
            //foreach (string s in nums)
            //{
            //    //.ToList().Select(t => new BaseAuditViewModel().CastModel(t));
            //    BaseAuditViewModel item = (new BaseAuditViewModel()).CastModel(dal.GetbyCaseNum(s));
            //}

            List<BaseAuditViewModel> baseAuditList = new List<BaseAuditViewModel>();
            BaseAuditDAL dal = new BaseAuditDAL();
            BaseAuditViewModel item = (new BaseAuditViewModel()).CastModel(dal.GetbyCaseNum(caseNum));
            baseAuditList.Add(item);
            var transM = new ApplicationImport(baseAuditList);

            //var result = ;
            //var br = new BaseResponse<object>()
            //{
            //    Data = result.Data,
            //    Message = new[] { result.Message },
            //    Status = result.Status
            //};

            return Json(null);
        }
    }

    public class JinJianResponse
    {
        public string batNo { get; set; }

        public int dataCnt { get; set; }


        public List<JinJianResponseError> errors = new List<JinJianResponseError>();
    }

    public class JinJianResponseError
    {
        public string pactNo { get; set; }

        public string dealDesc { get; set; }
    }
}