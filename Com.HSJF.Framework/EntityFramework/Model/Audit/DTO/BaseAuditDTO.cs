using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit.DTO
{
    public class BaseAuditDTO : EntityModel
    {
        //DTO
        public string ID { get; set; }
        /// <summary>
        /// 业务编号
        /// </summary>
        public string NewCaseNum { get; set; }
        public string CaseNum { get; set; }
        /// <summary>
        /// 销售组Id
        /// </summary>
        public string SalesGroupID { get; set; }



        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }


        /// <summary>
        ///  第三方平台
        /// </summary>
        public string ThirdParty { get; set; }
      
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
        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
       /// <summary>
       /// 实际放款日
       /// </summary>
        public DateTime? LendTime { get; set; }
    }
}