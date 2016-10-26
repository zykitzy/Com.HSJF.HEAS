
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.Utility;
using Newtonsoft.Json;

namespace Com.HSJF.HEAS.BLL.Audit
{
    public class AuditCasePush
    {
        private readonly BaseAuditDAL _baseAuditDal;

        public AuditCasePush()
        {
            _baseAuditDal = new BaseAuditDAL();
        }


        /// <summary>
        /// 二次退回拒绝推送
        /// </summary>
        /// <param name="caseNum">案件号</param>
        public PushResponse RejectPush(string caseNum)
        {
            string hatsHost = ConfigurationManager.AppSettings["hats_host"];

            if (hatsHost.IsNullOrEmpty())
            {
                throw new Exception("缺少hats_host配置节");
            }


            var auditCases = _baseAuditDal.GetListByCaseNum(caseNum);
            if (auditCases.Any(p => p.CaseStatus == CaseStatus.HatsPending))
            {
                var pushRequest = new PushRequest()
                {
                    RequestData = caseNum.ToHatsString()
                };

                var request = new HttpItem()
                {
                    URL = string.Format("{0}/api/BaseAuditPush/BaseAuditRejects", hatsHost),
                    Method = "post",
                    ContentType = "application/json;charset=utf-8",
                    Postdata = pushRequest.ToJson(),
                    Accept = "text/json",
                    PostEncoding = Encoding.UTF8
                };

                var httpResult = new HttpHelper().GetHtml(request);
                if (httpResult.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<PushResponse>(httpResult.Html);
                }
            }
            return new PushResponse()
            {
                IsSuccess = true
            };

        }
    }
}
