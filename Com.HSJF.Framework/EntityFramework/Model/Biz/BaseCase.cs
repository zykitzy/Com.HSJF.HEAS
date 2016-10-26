using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public partial class BaseCase : EntityModel
    {
        public BaseCase()
        {
            this.Collaterals = new List<Collateral>();
            this.RelationPersons = new List<RelationPerson>();
            this.Introducers = new List<Introducer>();
        }

        public string ID { get; set; }

        /// <summary>
        /// 案件号
        /// </summary>
        [Obsolete("请使用NewCaseNum")]
        public string CaseNum { get; set; }

        /// <summary>
        /// 新的案件编号
        /// </summary>
        public string NewCaseNum { get; set; }

        public string CaseType { get; set; }

        public string SalesID { get; set; }

        public string SalesGroupID { get; set; }

        public string DistrictID { get; set; }

        public string BorrowerName { get; set; }

        //以下 2-16-06-16 第一次测试之后新增
        public string Term { get; set; }

        /// <summary>
        /// 年化利率
        /// </summary>
        public decimal? AnnualRate { get; set; }

        public string Partner { get; set; }

        public string OpeningBank { get; set; }

        public string OpeningSite { get; set; }

        public string BankCard { get; set; }

        // 2016-06-27 再次新增
        public decimal? ServiceCharge { get; set; }

        public decimal? ServiceChargeRate { get; set; }

        public decimal? Deposit { get; set; }

        public DateTime? DepositDate { get; set; }

        public int? IsActivitieRate { get; set; }

        public int Version { get; set; }

        public decimal? LoanAmount { get; set; }

        public DateTime? CreateTime { get; set; }

        public string CreateUser { get; set; }

        //2016-09-08 大改
        public string PaymentFactor { get; set; }

        public string Purpose { get; set; }

        public virtual ICollection<Collateral> Collaterals { get; set; }

        public virtual ICollection<RelationPerson> RelationPersons { get; set; }

        public virtual ICollection<Introducer> Introducers { get; set; }
    }
}