using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Models.BaseModel;
using Com.HSJF.HEAS.Web.Models.Lendings;
using Com.HSJF.HEAS.Web.Models.Mortgage;
using System;
using System.Linq;
using System.Threading.Tasks;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.Framework.EntityFramework.Model.Audit.DTO;
using Com.HSJF.HEAS.BLL.Sales;

namespace Com.HSJF.HEAS.Web.Models.CaseAll
{
    /// <summary>
    /// 所有案件列表model
    /// </summary>
    public class CaseAllPageViewModel 
    {

        public string ID { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        public string CaseNum { get; set; }
        /// <summary>
        /// 销售组Id
        /// </summary>
        public string SalesGroupID { get; set; }
        public string SalesGroupText { get; set; }

        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }
        public string CaseModeText { get; set; }

        /// <summary>
        ///  第三方平台
        /// </summary>
        public string ThirdParty { get; set; }
        public string ThirdPartyText { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        ///  审批金额
        /// </summary>
        public decimal? AuditAmount { get; set; }
        /// <summary>
        /// 审批期限
        /// </summary>
        public string AuditTerm { get; set; }
        public string TermText { get; set; }
        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }
        public string CaseStatusText { get; set; }
        /// <summary>
        /// 实际放款日
        /// </summary>
        public DateTime? LendTime { get; set; }
        public string LendTimeText { get; set; }
        /// <summary>
        /// 拷贝BaseAudit信息
        /// </summary>
        /// <param name="baseCase"></param>
        /// <returns></returns>
        public CaseAllPageViewModel CopyModel(BaseAuditDTO baseCase)
        {
            var caseall = new CaseAllPageViewModel();
            var dicdal = new DictionaryDAL();
            var saleGroups = new SalesGroupBll().GetAll().ToList();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(baseCase, caseall);
            caseall.CaseNum = baseCase.NewCaseNum;
            if (caseall.SalesGroupID != null)
            {
                var firstOrDefault = saleGroups.FirstOrDefault(x => x.ID == caseall.SalesGroupID);
                if (firstOrDefault != null)
                    caseall.SalesGroupText = firstOrDefault.Name;
            }
            caseall.CaseStatusText = Helper.CaseStatusHelper.GetStatsText(baseCase.CaseStatus);
            caseall.TermText = dicdal.GetText(caseall.AuditTerm);
            caseall.ThirdPartyText = dicdal.GetText(caseall.ThirdParty);
            caseall.CaseModeText = dicdal.GetText(caseall.CaseMode) == "" ? "未选择" : dicdal.GetText(caseall.CaseMode);
            caseall.LendTimeText = caseall.LendTime == null ? "" : caseall.LendTime.Value.Date.ToLocalTime().ToShortDateString();
            return caseall;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CaseAllPageRequestModel : PageRequestViewModel
    {
        /// <summary>
        /// 案件编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 销售组（分公司）
        /// </summary>
        public string SalesGroupId { get; set; }

        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }

        /// <summary>
        /// 第三方平台
        /// </summary>
        public string ThirdParty { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 实际放款日开始
        /// </summary>
        public DateTime? LendTimeStart { get; set; }

        /// <summary>
        /// 实际放款日结束
        /// </summary>
        public DateTime? LendTimeEnd { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class CaseAllPageResponseModel : PageResponseViewModel<CaseAllPageViewModel>
    {
    }

    /// <summary>
    /// 所有案件详情model
    /// </summary>
    public class CaseAllDetailsViewModel
    {
        /// <summary>
        /// 案件详细信息model
        /// </summary>
        /// <returns></returns>
        public BaseAuditViewModel Baseauditvm { get; set; }

        /// <summary>
        /// 签约信息
        /// </summary>
        public PublicMortgageViewModel Mortgagevm { get; set; }

        /// <summary>
        /// 放款信息
        /// </summary>
        public LendingViewModel Lendingvm { get; set; }

        /// <summary>
        /// 流程历史对象
        /// </summary>
        //public IEnumerable<AuditHistory> AuditHistory { get; set; }
    }
}