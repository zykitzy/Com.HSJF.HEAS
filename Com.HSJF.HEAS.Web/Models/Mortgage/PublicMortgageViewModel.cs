using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.HEAS.Web.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Com.HSJF.Framework.DAL.Other;

namespace Com.HSJF.HEAS.Web.Models.Mortgage
{
    public class PublicMortgageViewModel
    {
        public string ID { get; set; }

        [Display(Name = "合同文件")]
        public string ContractFile { get; set; }

        public Dictionary<string, string> ContractFileName { get; set; }

        [Obsolete("合并到FourFile")]
        [Display(Name = "借条")]
        public string NoteFile { get; set; }

        [Obsolete("合并到FourFile")]
        public Dictionary<string, string> NoteFileName { get; set; }

        /// <summary>
        /// 借款借据,由借条,收据，承诺书,联系方式确认书合并
        /// </summary>
        public string FourFile { get; set; }

        /// <summary>
        /// 借款借据文件
        /// </summary>
        public Dictionary<string, string> FourFileName { get; set; }

        [Obsolete("合并到FourFile")]
        [Display(Name = "收据")]
        public string ReceiptFile { get; set; }

        [Obsolete("合并到FourFile")]
        public Dictionary<string, string> ReceiptFileName { get; set; }

        [Display(Name = "他证")]
        public string OtherFile { get; set; }

        public Dictionary<string, string> OtherFileName { get; set; }

        [Obsolete("使用新案件号")]
        [Display(Name = "案件号")]
        public string CaseNum { get; set; }
        [Display(Name = "案件号")]
        public string NewCaseNum { get; set; }
        [Display(Name = "案件状态")]
        public string CaseStatus { get; set; }

        [Display(Name = "案件状态")]
        public string CaseStatusText { get; set; }

        [Display(Name = "合同号")]
        [Required]
        public string ContractNo { get; set; }

        [Display(Name = "签约金额")]
        [Range(300000, 100000000000000000, ErrorMessage = "签约金额不能小于300000")]
        [Required]
        public decimal? ContractAmount { get; set; }

        [Display(Name = "签约日期")]
        [Required]
        public DateTime? ContractDate { get; set; }

        [Display(Name = "签约人员")]
        [Required]
        public string ContractPerson { get; set; }

        public string ContractPersonText { get; set; }

        [Obsolete("合并到FourFile")]
        [Display(Name = "承诺书")]
        public string UndertakingFile { get; set; }

        [Obsolete("合并到FourFile")]
        public Dictionary<string, string> UndertakingFileName { get; set; }

        [Display(Name = "还款委托书")]
        public string RepaymentAttorneyFile { get; set; }

        public Dictionary<string, string> RepaymentAttorneyFileName { get; set; }

        [Obsolete("合并到FourFile")]
        [Display(Name = "联系方式确认书")]
        public string ContactConfirmFile { get; set; }

        [Obsolete("合并到FourFile")]
        public Dictionary<string, string> ContactConfirmFileName { get; set; }

        [Display(Name = "授权委托书")]
        public string PowerAttorneyFile { get; set; }

        public Dictionary<string, string> PowerAttorneyFileName { get; set; }

        [Display(Name = "收件收据")]
        public string CollectionFile { get; set; }

        public Dictionary<string, string> CollectionFileName { get; set; }

        [Display(Name = "审批意见")]
        public string Description { get; set; }

        public bool IsCurrent { get; set; }

        [Display(Name = "审批金额")]
        public decimal? AuditAmount { get; set; }

        [Display(Name = "出借人姓名")]
        public string LenderName { get; set; }

        #region 打款账户

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        public string OpeningSite { get; set; }

        public string OpeningSiteText { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }

        #endregion 打款账户

        #region 返利信息

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal? ServiceCharge { get; set; }

        /// <summary>
        /// 服务费点数
        /// </summary>
        public decimal? ServiceChargeRate { get; set; }

        /// <summary>
        /// 客户已支付金额
        /// </summary>
        public decimal? Deposit { get; set; }

        /// <summary>
        /// 客户支付定金日期
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// 是否为活动期间的优惠利率
        /// </summary>
        public int? IsActivitieRate { get; set; }

        public string IsActivitieRateText { get; set; }

        #endregion 返利信息

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

        [Display(Name = "第三方平台")]
        public string ThirdParty { get; set; }

        [Display(Name = "案件模式")]
        public string CaseMode { get; set; }

        #endregion 居间信息

        /// <summary>
        /// 流程历史对象
        /// </summary>
        public IEnumerable<AuditHistory> AuditHistory { get; set; }
        /// <summary>
        ///签约失败理由
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 合作渠道
        /// </summary>
        public virtual IEnumerable<IntroducerAuditViewModel> Introducer { get; set; }

        public PublicMortgageViewModel CastModel(PublicMortgage model)
        {
            BaseAuditDAL bad = new BaseAuditDAL();
            var baseaudit = bad.Get(model.ID);
            var maxaudit = bad.GetMaxAudit(model.ID);
            var dicdal = new DictionaryDAL();
            PublicMortgageViewModel bcvm = new PublicMortgageViewModel();

            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bcvm);

            UserDAL ud = new UserDAL();
            var contr = ud.FindById(model.ContractPerson);
            if (contr != null)
            {
                bcvm.ContractPersonText = contr.Result.DisplayName;
            }
            var baseauditRelaType = "";
            var maxauditRelaType = "";
            if (baseaudit != null)
            {
                if (!string.IsNullOrEmpty(baseaudit.OpeningSite))
                {
                    var baseauditRela = baseaudit.RelationPersonAudits.FirstOrDefault(t => t.IdentificationNumber == baseaudit.OpeningSite);
                    if (baseauditRela != null)
                    {
                        baseauditRelaType = baseauditRela.Name + "(" + dicdal.GetText(baseauditRela.RelationType) + ")";
                    }
                }
            }
            if (maxaudit != null)
            {
                if (!string.IsNullOrEmpty(maxaudit.OpeningSite))
                {
                    var maxauditRela = maxaudit.RelationPersonAudits.FirstOrDefault(t => t.IdentificationNumber == maxaudit.OpeningSite);
                    if (maxauditRela != null)
                    {
                        maxauditRelaType = maxauditRela.Name + "(" + dicdal.GetText(maxauditRela.RelationType) + ")";
                    }
                }
            }
            bcvm.OpeningSiteText = baseaudit == null ? maxaudit == null ? null : maxauditRelaType : baseauditRelaType;
            bcvm.CaseNum = baseaudit == null ? maxaudit == null ? "" : maxaudit.NewCaseNum : baseaudit.NewCaseNum;
            bcvm.CaseStatusText = baseaudit == null ? maxaudit == null ? "" : Com.HSJF.HEAS.Web.Helper.CaseStatusHelper.GetStatsText(maxaudit.CaseStatus) : Com.HSJF.HEAS.Web.Helper.CaseStatusHelper.GetStatsText(baseaudit.CaseStatus);
            bcvm.IsCurrent = baseaudit == null ? false : (baseaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.PublicMortgage) ? true : false;
            bcvm.AuditAmount = baseaudit == null ? maxaudit == null ? 0 : maxaudit.AuditAmount : baseaudit.AuditAmount;
            return bcvm;
        }

        public PublicMortgage CastDB(PublicMortgageViewModel model)
        {
            PublicMortgage bc = new PublicMortgage();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bc);
            return bc;
        }
    }
}