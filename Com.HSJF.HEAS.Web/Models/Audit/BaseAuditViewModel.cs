using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class BaseAuditViewModel
    {
        public string ID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 借款类型
        /// </summary>
        [Display(Name = "借款类型")]
        public string CaseType { get; set; }

        /// <summary>
        /// 借款类型text
        /// </summary>
        public string CaseTypeText { get; set; }

        /// <summary>
        /// 销售人员Id
        /// </summary>
        [Display(Name = "销售人员")]
        [Required]
        public string SalesID { get; set; }

        /// <summary>
        /// 销售组Id
        /// </summary>
        [Display(Name = "销售组")]
        [Required]
        public string SalesGroupID { get; set; }

        /// <summary>
        /// 销售组名称
        /// </summary>
        public string SalesGroupText { get; set; }

        /// <summary>
        /// 地区Id
        /// </summary>
        [Display(Name = "地区")]
        [Required]
        public string DistrictID { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        [Display(Name = "借款人姓名")]
        public string BorrowerName { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        [Display(Name = "借款期限")]
        public string Term { get; set; }

        /// <summary>
        /// 借款期限text
        /// </summary>
        public string TermText { get; set; }

        /// <summary>
        /// 合作???
        /// </summary>
        public string Partner { get; set; }

        [Display(Name = "审批金额")]
        [Range(300000, 100000000000000000, ErrorMessage = "审批金额不能小于300000")]
        public decimal? AuditAmount { get; set; }

        [Obsolete("迁移至进件")]
        [Display(Name = "年化利率")]
        public decimal? AnnualRate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Obsolete("废弃")]
        [Display(Name = "平台费用")]
        public decimal? PlatformCharge { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "综合抵押率")]
        public decimal? ComprehensiveRate { get; set; }

        [Display(Name = "抵押次数")]
        public string MortgageOrder { get; set; }

        public string MortgageOrderText { get; set; }

        [Obsolete("2016-9-8 大改停用")]
        [Display(Name = "案件描述")]
        public string CaseDetail { get; set; }

        [Display(Name = "审核期限")]
        public string AuditTerm { get; set; }

        public string AuditTermText { get; set; }

        [Display(Name = "审核利率")]
        public decimal? AuditRate { get; set; }

        #region 打款账户

        [Display(Name = "开户行")]
        public string OpeningBank { get; set; }

        [Display(Name = "开户名称")]
        public string OpeningSite { get; set; }

        [Display(Name = "银行卡")]
        public string BankCard { get; set; }

        #endregion 打款账户

        [Display(Name = "审批意见")]
        public string AuditComment { get; set; }

        /// <summary>
        /// 退回或者通过需要填注意见时可以使用此字段
        /// </summary>
        [Display(Name = "备注")]
        public string Description { get; set; }

        [Display(Name = "拒绝理由")]
        public string RejectType { get; set; }

        #region 返利信息

        [Display(Name = "服务费")]
        public decimal? ServiceCharge { get; set; }

        [Display(Name = "服务费率")]
        public decimal? ServiceChargeRate { get; set; }

        [Obsolete("移至签约")]
        [Display(Name = "客户已支付金额")]
        public decimal? Deposit { get; set; }

        [Obsolete("移至签约")]
        [Display(Name = "客户支付定金日期")]
        public DateTime? DepositDate { get; set; }

        [Obsolete("移至签约")]
        [Display(Name = "是否为活动期间的优惠利率")]
        public int? IsActivitieRate { get; set; }

        [Obsolete("移至签约")]
        public string IsActivitieRateText { get; set; }

        #endregion 返利信息

        /// <summary>
        /// 数据版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        [Display(Name = "申请金额")]
        [Required]
        [Range(300000, 100000000000000000, ErrorMessage = "申请金额不能小于300000")]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal? LoanAmount { get; set; }

        /// <summary>
        /// 借款人
        /// </summary>
       // public RelationPersonAuditViewModel BorrowerPerson { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "创建人")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 本次业务是否需要出具评估
        /// </summary>
        [Display(Name = "本次业务是否需要出具评估")]
        public string IsNeedReport { get; set; }

        [Display(Name = "案件状态")]
        public string CaseStatus { get; set; }

        public string CaseStatusText { get; set; }

        [Obsolete("2016-9-8 大改停用")]
        [Display(Name = "面谈报告")]
        public string FaceReportFile { get; set; }

        public Dictionary<string, string> FaceReportFileName { get; set; }

        [Display(Name = "现场报告")]
        public string FieldReportFile { get; set; }

        public Dictionary<string, string> FieldReportFileName { get; set; }

        [Obsolete("2016-9-8 大改停用")]
        [Display(Name = "贷前尽调报告")]
        public string LoanDetailReportFile { get; set; }

        public Dictionary<string, string> LoanDetailReportFileName { get; set; }

        [Obsolete]
        [Display(Name = "跟单人")]
        public string Merchandiser { get; set; }

        [Display(Name = "平台保证金")]
        public decimal? EarnestMoney { get; set; }

        [Display(Name = "外访费（下户费）")]
        public decimal? OutboundCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "代收公证费用")]
        public decimal? DebitNotarizationCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "代收评估费")]
        public decimal? DebitEvaluationCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "代收担保费")]
        public decimal? DebitGuaranteeCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "代收保险费")]
        public decimal? DebitInsuranceCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "代收其他")]
        public decimal? DebitOtherCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "公司承担的公证费")]
        public decimal? LevyNotarizationCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "公司承担的产调费")]
        public decimal? LevyAssetsSurveyCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "公司承担的信用报告费")]
        public decimal? LevyCreditReportCost { get; set; }

        [Obsolete("暂时停用")]
        [Display(Name = "公司承担的其他费用")]
        public decimal? LevyOtherCost { get; set; }

        //新增审核字段

        [Display(Name = "案件模式")]
        public string CaseMode { get; set; }

        [Display(Name = "第三方平台")]
        public string ThirdParty { get; set; }

        [Obsolete("2016-9-8大改停用")]
        [Display(Name = "月利息金额")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "放款日期")]
        public DateTime? LendingDate { get; set; }

        [Obsolete]
        [Display(Name = "回款日期")]
        public DateTime? PaymentDate { get; set; }

        [Obsolete]
        [Display(Name = "实收利息（不退客户）")]
        public decimal? ActualInterest { get; set; }

        [Obsolete]
        [Display(Name = "预收利息（可退客户）")]
        public decimal? AdvanceInterest { get; set; }

        #region 居间信息

        /// <summary>
        /// 审批金额
        /// </summary>

        public decimal? ThirdPartyAuditAmount { get; set; }

        /// <summary>
        /// 审批贷款期限
        /// </summary>

        public string ThirdPartyAuditTerm { get; set; }

        /// <summary>
        /// 审批利率
        /// </summary>

        public decimal? ThirdPartyAuditRate { get; set; }

        #endregion 居间信息

        #region 2016-09-09 大改

        /// <summary>
        ///  还款来源
        /// </summary>
        [Required]
        [Display(Name = "还款来源")]
        public string PaymentFactor { get; set; }

        /// <summary>
        /// 借款用途
        /// </summary>
        [Required]
        [Display(Name = "借款用途")]
        public string Purpose { get; set; }

        /// <summary>
        /// 放款条件
        /// </summary>
        public string LendingTerm { get; set; }

        /// <summary>
        /// 签约要件
        /// </summary>
        public string ContractFileInfo { get; set; }

        /// <summary>
        /// 拒绝理由，拒绝批注使用Description
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 借款申请书
        /// </summary>
        [Required]
        [Display(Name = "借款申请书")]
        public string LoanProposedFile { get; set; }

        public Dictionary<string, string> LoanProposedFileName { get; set; }

        /// <summary>
        /// 他证是否上传
        /// </summary>
        public string OtherFileIsupload { get; set; }

        #endregion 2016-09-09 大改

        //2016-10-08
        /// <summary>
        /// 客户保证金
        /// </summary>
        [Display(Name = "客户保证金")]
        
        public decimal? CustEarnestMoney { get; set; }
        public IEnumerable<AuditHistory> AuditHistory { get; set; }

        /// <summary>
        /// 抵押物信息集合
        /// </summary>
        public virtual IEnumerable<CollateralAuditViewModel> CollateralAudits { get; set; }

        /// <summary>
        /// 关系人信息集合
        /// </summary>
        public virtual ICollection<RelationPersonAuditViewModel> RelationPersonAudits { get; set; }

        /// <summary>
        /// 个人资信情况
        /// </summary>
        public virtual IEnumerable<IndividualCreditViewModel> IndividualCredits { get; set; }

        /// <summary>
        /// 企业资信情况
        /// </summary>
        public virtual IEnumerable<EnterpriseCreditViewModel> EnterpriseCredits { get; set; }

        /// <summary>
        /// 被执行人情况
        /// </summary>
        public virtual IEnumerable<EnforcementPersonViewModel> EnforcementPersons { get; set; }

        /// <summary>
        /// 工商税务情况
        /// </summary>
        public virtual IEnumerable<IndustryCommerceTaxViewModel> IndustryCommerceTaxs { get; set; }

        /// <summary>
        /// 房屋明细
        /// </summary>
        public virtual IEnumerable<HouseDetailViewModel> HouseDetails { get; set; }

        /// <summary>
        /// 担保人
        /// </summary>
        public virtual IEnumerable<GuarantorViewModel> Guarantors { get; set; }

        /// <summary>
        /// 合作渠道
        /// </summary>
        [Obsolete("移至签约")]
        public virtual IEnumerable<IntroducerAuditViewModel> Introducer { get; set; }

        public BaseAuditViewModel CastModel(BaseAudit model)
        {
            BaseAuditViewModel bcvm = new BaseAuditViewModel();
            Framework.DAL.Audit.BaseAuditDAL baseAuditDal = new Framework.DAL.Audit.BaseAuditDAL();
            var saleGroups = new SalesGroupBll().GetAll().ToList();

            Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bcvm);
            if (bcvm == null || model == null)
            {
                return null;
            }
            MortgageBll mortgageBll = new MortgageBll();
            var mortgage = mortgageBll.QueryById(model.ID);
            bcvm.OtherFileIsupload = "未上传";
            if (mortgage != null)
            {
                bcvm.OtherFileIsupload = mortgage.OtherFile != "" ? "已上传" : "未上传";
            }
            bcvm.CaseNum = model.NewCaseNum;
            if (model.NewCaseNum != null)
            {
                var caseStatus = baseAuditDal.GetbyCaseNum(model.NewCaseNum).CaseStatus;
                bcvm.CaseStatus = caseStatus ?? "";
                bcvm.CaseStatusText = caseStatus == null ? "" : Helper.CaseStatusHelper.GetStatsText(caseStatus);
            }
            else
            {
                bcvm.CaseStatusText = "";
            }

            if (bcvm.SalesGroupID != null)
            {
                bcvm.SalesGroupText = saleGroups.Single(x => x.ID == bcvm.SalesGroupID).Name;
            }

            return bcvm;
        }

        public BaseAudit CastDB(BaseAuditViewModel model)
        {
            BaseAudit bc = new BaseAudit();

            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bc);

            // bc.NewCaseNum = model.CaseNum;

            return bc;
        }
    }

    public class BaseAuditListPageRequestViewModel : PageRequestViewModel
    {
        public string BorrowerName { get; set; }

        public string CaseNum { get; set; }

        public string CaseStatus { get; set; }

        /// <summary>
        /// 销售团队Id
        /// </summary>
        public string SalesGroupId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class BaseAuditListPageResponseViewModel : PageResponseViewModel<BaseAuditViewModel>
    {
    }
}