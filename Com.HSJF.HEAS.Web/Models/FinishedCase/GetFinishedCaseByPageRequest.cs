using System.Diagnostics;
using Com.HSJF.HEAS.BLL.FinishedCase.Dto;
using Com.HSJF.HEAS.Web.Models.BaseModel;

namespace Com.HSJF.HEAS.Web.Models.FinishedCase
{
    /// <summary>
    /// 分页请求
    /// </summary>
    public class GetFinishedCaseRequest : PageRequestViewModel
    {
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }
        
        /// <summary>
        /// 案件编号
        /// </summary>
        public string CaseNum { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GetFinishedCasesInput Map()
        {
            return new GetFinishedCasesInput()
            {
                BorrowerName = this.BorrowerName,
                CaseNum = this.CaseNum,
                Order = base.Order,
                PageIndex = base.PageIndex,
                PageSize = base.PageSize,
                Sort = base.Sort
            };
        }
    }
}