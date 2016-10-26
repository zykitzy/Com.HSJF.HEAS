
namespace Com.HSJF.HEAS.Web.Models.AfterCase
{
    /// <summary>
    /// 放款案件信息
    /// </summary>
    public class AfterCase
    {
        /// <summary>
        /// 案件号(合同编码)
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 业务员所属分公司Id
        /// </summary>
        public string SalesGrouptId { get; set; }

        /// <summary>
        /// 业务员所属分公司
        /// </summary>
        public string SalesCompanyName { get; set; }

        /// <summary>
        /// 公司简称
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户证件类型
        /// </summary>
        public string CustomerIdentificationType { get; set; }

        /// <summary>
        /// 客户证件号码
        /// </summary>
        public string CustomerIdentificationNumber { get; set; }

        /// <summary>
        /// 打款账户名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public string EarnestMoney { get; set; }

        /// <summary>
        /// 债务人
        /// </summary>
        public string DebtName { get; set; }

        /// <summary>
        /// 出借人(债权人)
        /// </summary>
        public string LenderName { get; set; }

        /// <summary>
        /// 客户贷款期数
        /// </summary>
        public string LoanTerm { get; set; }

        /// <summary>
        /// 放款日
        /// </summary>
        public string LendingDate { get; set; }
        /// <summary>
        /// 收款日
        /// </summary>
        public string PaymentDay { get; set; }

        /// <summary>
        /// 本金(放款金额)
        /// </summary>
        public string ContractAmount { get; set; }

        /// <summary>
        /// 合同年利率
        /// </summary>
        public string ContractInterestRate { get; set; }

        /// <summary>
        /// 执行年利率
        /// </summary>
        public string RealInterestRate { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public string ReceiveType { get; set; }

        /// <summary>
        /// 资金方ID(放款方ID)
        /// </summary>
        public string FundId { get; set; }

        /// <summary>
        /// 资金方年利率
        /// </summary>
        public string FundInterestRate { get; set; }

        /// <summary>
        /// 资金方名称
        /// </summary>
        public string FundName { get; set; }

        /// <summary>
        /// 贷款模式
        /// </summary>
        public string LoadType { get; set; }

        /// <summary>
        /// 虚拟帐号(贷前不提供)
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 合同号
        /// </summary>
        public string ContractNum { get; set; }
    }
}