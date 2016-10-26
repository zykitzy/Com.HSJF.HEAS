using System;
using System.Configuration;
using System.Text;
using Com.HSJF.HEAS.BLL.Mortgage.Dto;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.Utility;

namespace Com.HSJF.HEAS.BLL.Mortgage
{
    public class MortgagePush
    {
        public PushRespose PushToHats(PublicMortgageDto input)
        {
            string hatsHost = ConfigurationManager.AppSettings["hats_host"];

            if (hatsHost.IsNullOrEmpty())
            {
                throw new Exception("缺少hats_host配置节");
            }

            input.Status = 1;// 进入进件推送

            var resquestBody = new PushRequest()
            {
                RequestData = input.ToHatsString<PublicMortgageDto>()
            };

            var request = new HttpItem()
            {
                URL = string.Format("{0}/api/BaseAuditPush/AddBaseAuditByPublic", hatsHost),
                Method = "post",
                ContentType = "application/json;charset=utf-8",
                Postdata = resquestBody.ToJson(),
                Accept = "text/json",
                PostEncoding = Encoding.UTF8
            };

            var httpResult = new HttpHelper().GetHtml(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<PushRespose>(httpResult.Html);

        }

        public PushRespose Reject(string caseNum)
        {
            string hatsHost = ConfigurationManager.AppSettings["hats_host"];

            if (hatsHost.IsNullOrEmpty())
            {
                throw new Exception("缺少hats_host配置节");
            }

            var rejectCase = new PublicMortgageDto()
            {
                CaseNum = caseNum,
                Status = 2
            };

            var resquestBody = new PushRequest()
            {
                RequestData = rejectCase.ToHatsString()
            };


            var request = new HttpItem()
            {
                URL = string.Format("{0}/api/BaseAuditPush/AddBaseAuditByPublic", hatsHost),
                Method = "post",
                ContentType = "application/json;charset=utf-8",
                Postdata = resquestBody.ToJson(),
                Accept = "text/json",
                PostEncoding = Encoding.UTF8
            };

            var httpResult = new HttpHelper().GetHtml(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<PushRespose>(httpResult.Html);

        }
    }
}
