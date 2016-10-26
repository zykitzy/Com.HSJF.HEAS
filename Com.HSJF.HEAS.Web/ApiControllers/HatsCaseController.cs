using System.Linq;
using System.Web.Http;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.BLL.Audit;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.HatsCase;
using Com.HSJF.HEAS.Web.Validations.HatsCase;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.Utility;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// hats通知签约
    /// </summary>
    public class HatsCaseController : ApiController
    {
        private readonly BaseAuditBll _baseAuditBll;
        private readonly MortgageBll _mortgageBll;

        public HatsCaseController()
        {
            _baseAuditBll = new BaseAuditBll();
            _mortgageBll = new MortgageBll();
        }

        [HttpPost]
        public BaseApiResponse<string> Post(SecurityRequest request)
        {
            var response = new BaseApiResponse<string>();

            try
            {
                HatsCase hatsCase = request.RequestData.FromHatsString<HatsCase>();

                if (hatsCase.IsNotNull())
                {
                    var result = new HatsCaseValidator().Validate(hatsCase);
                    if (result.IsNotValid())
                    {
                        response.Status = StatusEnum.Failed.ToString();
                        response.Message = result.GetErrors().First().Message;
                        return response;
                    }

                    BaseAudit pendingCase = _baseAuditBll.QueryHatsPending(hatsCase.CaseNum);
                    if (pendingCase.IsNotNull())
                    {
                        pendingCase.EarnestMoney = hatsCase.EarnestMoney;
                        pendingCase.CaseMode = hatsCase.TransferType;
                        pendingCase.ThirdParty = hatsCase.ThirdParty;

                        pendingCase.ThirdPartyAuditAmount = hatsCase.TransferAmount;
                        pendingCase.ThirdPartyAuditRate = hatsCase.TransferInterest;
                        pendingCase.ThirdPartyAuditTerm = hatsCase.TransferTerm;

                        var isSuccess = _mortgageBll.IntoMortgage(pendingCase, pendingCase.CreateUser);
                        if (isSuccess)
                        {
                            response.Status = StatusEnum.Success.ToString();
                        }
                        else
                        {
                            response.Status = StatusEnum.Failed.ToString();
                            response.Message = "进入签约失败";
                        }
                    }
                    else
                    {
                        response.Status = StatusEnum.Failed.ToString();
                        response.Message = "数据已更改";
                    }
                }
                else
                {
                    response.Status = StatusEnum.Failed.ToString();
                    response.Message = "案件信息为空";
                }
            }
            catch (System.Exception ex)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
