using System;
using System.Web.Http;
using Com.HSJF.HEAS.BLL.Lending;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.LendingConfirmation;
using Com.HSJF.Infrastructure.Utility;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// hats通知案件放款
    /// </summary>
    public class LendingConfirmationController : ApiController
    {
        private readonly MortgageBll _mortgageBll;
        private readonly LendingBll _lendingBll;

        public LendingConfirmationController()
        {
            _mortgageBll = new MortgageBll();
            _lendingBll = new LendingBll();
        }

        [NonAction]
        [Obsolete("流程变更，不需要等待hats确认")]
        [HttpPost]
        public BaseApiResponse<string> Post(SecurityRequest request)
        {
            var response = new BaseApiResponse<string>();
            try
            {
                bool isSuccess = false;

                var caseNum = request.RequestData.FromHatsString<ConfirmLendingRequest>();

                var waitingConfirmCase = _mortgageBll.QueryWaitingLending(caseNum.CaseNum);
                if (waitingConfirmCase != null)
                {
                    isSuccess = _lendingBll.SubmitLending(waitingConfirmCase, waitingConfirmCase.CreateUser);
                }
                if (isSuccess)
                {
                    response.Status = StatusEnum.Success.ToString();
                }
                else
                {
                    response.Status = StatusEnum.Failed.ToString();
                    response.Message = "进入放款失败";
                }
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = ex.Message;
            }

            return response;
        }
    }
}