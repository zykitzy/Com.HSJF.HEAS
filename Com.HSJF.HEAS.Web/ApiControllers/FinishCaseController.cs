using Com.HSJF.HEAS.BLL.FinishedCase;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.FinisheCase;
using System;
using System.Web.Http;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// 案件结清
    /// </summary>
    public class FinishCaseController : ApiController
    {
        private FinishedCaseBll _finishedCase;

        public FinishCaseController()
        {
            _finishedCase = new FinishedCaseBll();
        }

        [HttpPost]
        public BaseApiResponse<string> Post(FinishCaseRequest request)
        {
            var response = new BaseApiResponse<string>()
            {
                Data = string.Empty
            };

            try
            {
                Tuple<bool, string> result = _finishedCase.FinishCase(request.CaseNum);

                response.Status = result.Item1 ? StatusEnum.Success.ToString() : StatusEnum.Failed.ToString();
                response.Message = result.Item2;
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
