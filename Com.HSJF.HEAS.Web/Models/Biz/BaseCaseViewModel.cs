using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models.BaseModel;
using Com.HSJF.HEAS.Web.Models.Biz.AppendClass;
using Com.HSJF.Infrastructure.Identity.Manager;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class BaseCaseViewModel
    {
        #region 原有字段

        public string ID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        [Display(Name = "业务编号")]
        public string CaseNum { get; set; }

        /// <summary>
        /// 借款类型
        /// </summary>
        [Display(Name = "借款类型")]
        public string CaseType { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        [Display(Name = "销售人员")]
        [Required]
        public string SalesID { get; set; }

        /// <summary>
        /// 销售组
        /// </summary>
        [Display(Name = "销售组")]
        [Required]
        public string SalesGroupID { get; set; }

        /// <summary>
        /// 地区
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
        /// 申请金额
        /// </summary>
        [Display(Name = "申请金额")]
        [Required]
        [Range(300000, 100000000000000000, ErrorMessage = "申请金额不能小于300000")]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal? LoanAmount { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        [Display(Name = "借款期限")]
        public string Term { get; set; }

        /// <summary>
        /// 申请利率（年）
        /// </summary>
        public decimal? AnnualRate { get; set; }

        /// <summary>
        /// 借款期限text
        /// </summary>
        public string TermText { get; set; }

        #region 打款账户

        /// <summary>
        /// 开户行
        /// </summary>
        [Display(Name = @"开户行")]
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        [Display(Name = "开户名称")]
        public string OpeningSite { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { get; set; }

        #endregion 打款账户

        #region 返利信息

        /// <summary>
        /// 服务费
        /// </summary>
        [Display(Name = "服务费")]
        public decimal? ServiceCharge { get; set; }

        /// <summary>
        /// 服务费率
        /// </summary>
        [Display(Name = "服务费率")]
        [Required]
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

        [Display(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "创建人")]
        public string CreateUser { get; set; }

        [Display(Name = "版本号")]
        public int Version { get; set; }

        #endregion 返利信息

        //类型为借款人的单独设置，不在relationperson 中设置
        //public RelationPersonViewModel BorrowerPerson { get; set; }

        public ICollection<RelationPersonViewModel> RelationPerson { get; set; }

        public IEnumerable<CollateralViewModel> Collateral { get; set; }

        //流程历史
        public IEnumerable<AuditHistory> AuditHistory { get; set; }

        //介绍人
        [Obsolete("移至签约")]
        public IEnumerable<IntroducerViewModel> Introducer { get; set; }

        #endregion 原有字段

        #region 2016-9-8 新增字段

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
        /// 进件页面显示审批，签约信息对象
        /// </summary>
        public AuditInformationVM AuditInformation { get; set; }

        [Display(Name = "案件状态")]
        public string CaseStatus { get; set; }

        // public string CaseStatusText { get; set; }

        #endregion 2016-9-8 新增字段

        public BaseCaseViewModel CastModel(BaseCase model)
        {
            BaseCaseViewModel bcvm = new BaseCaseViewModel();
            Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bcvm);
            bcvm.CaseNum = model.NewCaseNum;

            #region 2016-9-8新增

            var badal = new BaseAuditDAL();
            var modal = new MortgageDAL();
            var auditvm = new AuditInformationVM();
            var dicdal = new DictionaryDAL();
            var store = new Infrastructure.Identity.Store.UserStore();
            var manager = new UserManager(store);
            if (string.IsNullOrEmpty(model.NewCaseNum))
            {
                bcvm.AuditInformation = auditvm;
                return bcvm;
            }
            var basudit = badal.GetbyCaseNum(model.NewCaseNum);
            if (basudit == null)
            {
                bcvm.AuditInformation = auditvm;
                bcvm.CaseStatus = null;
                return bcvm;
            }
            if (basudit.CaseMode == "-CaseMode-JuJian")
            {
                auditvm.ThirdPartyAuditAmount = basudit.ThirdPartyAuditAmount;
                auditvm.ThirdPartyAuditTerm = basudit.ThirdPartyAuditTerm;
            }
            else
            {
                auditvm.ThirdPartyAuditAmount = basudit.AuditAmount;
                auditvm.ThirdPartyAuditTerm = basudit.AuditTerm;
            }
            var CaseStatus = basudit == null ? "数据不完整" : basudit.CaseStatus;
            bcvm.CaseStatus = CaseStatus == null ? "" : CaseStatus;
            //bcvm.CaseStatusText = CaseStatus == null ? "" : Helper.CaseStatusHelper.GetBigStatusText(CaseStatus);
            string RefuseReason = "";
            if (basudit.RejectType != null)
            {
                string RejectType = "";
                string[] str = basudit.RejectType.Split(',');
                DictionaryDAL dadal = new DictionaryDAL();
                foreach (var item in str)
                {
                    RejectType += dadal.GetText(item) + "，";
                }
                RefuseReason = RejectType.Substring(0, RejectType.Length - 1);
            }

            auditvm.ThirdPartyAuditRate = basudit.AuditRate;
            auditvm.RefuseReason = RefuseReason;
            auditvm.SignRefuseReason = basudit.RejectReason;
            var mortgage = modal.Get(basudit.ID);
            if (mortgage == null)
            {
                bcvm.AuditInformation = auditvm;
                return bcvm;
            }
            var contr = manager.FindById(mortgage.ContractPerson);
            auditvm.ContractPersonText = contr == null ? "" : contr.DisplayName;
            auditvm.ContractAmount = mortgage.ContractAmount;
            auditvm.ContractDate = mortgage.ContractDate;
            bcvm.AuditInformation = auditvm;

            #endregion 2016-9-8新增

            return bcvm;
        }

        public BaseCase CastDB(BaseCaseViewModel model)
        {
            BaseCase bc = new BaseCase();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bc);
            return bc;
        }
    }

    public class BaseCaseListViewModel : BaseCaseViewModel
    {
        [Display(Name = "借款类型")]
        public string CaseDisplayType { get { return "短借"; } }

        [Display(Name = "抵押物名称")]
        public string FirstCollateral { get; set; }

        [Display(Name = "销售人员")]
        public string SalesDisplay { get; set; }

        /// <summary>
        /// 销售组名称
        /// </summary>
        public string SalesGroupText { get; set; }

        [Display(Name = "案件状态")]
        public string CaseStatus { get; set; }

        public string CaseStatusText { get; set; }

        public BaseCaseListViewModel CastModel(BaseCase baseCase)
        {
            BaseCaseListViewModel baseCaseListViewModel = new BaseCaseListViewModel();
            CollateralDAL collateralDal = new CollateralDAL();
            SalesManDAL salesManDal = new SalesManDAL();
            Framework.DAL.Audit.BaseAuditDAL badit = new Framework.DAL.Audit.BaseAuditDAL();
            var saleGroups = new SalesGroupBll().GetAll().ToList();

            Infrastructure.ExtendTools.ObjectExtend.CopyTo(baseCase, baseCaseListViewModel);
            baseCaseListViewModel.CaseNum = baseCase.NewCaseNum;

            var sale = salesManDal.Get(baseCase.SalesID);
            baseCaseListViewModel.SalesDisplay = sale == null ? "" : sale.Name;

            var firstcoll = collateralDal.FindByCaseID(baseCase.ID).FirstOrDefault(t => t.CollateralType == DictionaryString.CollateralType);
            if (firstcoll != null)
            {
                baseCaseListViewModel.FirstCollateral = string.Format("{0}({1})", firstcoll.Address, firstcoll.BuildingName);
            }

            if (baseCase.NewCaseNum != null && baseCase.Version > 0)
            {
                var audit = badit.GetbyCaseNum(baseCase.NewCaseNum);

                var CaseStatus = audit == null ? "数据不完整" : audit.CaseStatus;
                baseCaseListViewModel.CaseStatus = CaseStatus == null ? "" : CaseStatus;
                baseCaseListViewModel.CaseStatusText = CaseStatus == null ? "" : Helper.CaseStatusHelper.GetBigStatusText(CaseStatus);
            }
            else if (baseCase.NewCaseNum != null && baseCase.Version == 0)
            {
                baseCaseListViewModel.CaseStatus = "";
                baseCaseListViewModel.CaseStatusText = "预提交";
            }
            else
            {
                baseCaseListViewModel.CaseStatus = null;
            }

            if (baseCaseListViewModel.SalesGroupID != null)
            {
                baseCaseListViewModel.SalesGroupText = saleGroups.Single(x => x.ID == baseCaseListViewModel.SalesGroupID).Name;
            }

            return baseCaseListViewModel;
        }
    }

    public class BaseCaseListPageRequestViewModel : PageRequestViewModel
    {
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 销售团队Id
        /// </summary>
        public string SalesGroupId { get; set; }
    }

    public class BaseCaseListPageResponseViewModel : PageResponseViewModel<BaseCaseListViewModel>
    {
    }
}