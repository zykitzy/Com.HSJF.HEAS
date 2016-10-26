using Com.HSJF.Framework.EntityFramework.Model.Audit;
using System;
using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Mortgage.Dto
{
    public class PublicMortgageDto
    {
        public PublicMortgageDto()
        {
            Introducer = new List<IntroducerAudit>();
        }
        public string ID { get; set; }

        /// <summary>
        /// 合同文件
        /// </summary>
        public string ContractFile { get; set; }

        /// <summary>
        /// 借条
        /// </summary>
        public string NoteFile { get; set; }

        /// <summary>
        /// 收据
        /// </summary>
        public string ReceiptFile { get; set; }

        /// <summary>
        /// 他证
        /// </summary>
        public string OtherFile { get; set; }

        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNum { get; set; }


        /// <summary>
        /// 合同号
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        /// 签约信息
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 签约
        /// </summary>
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string ContractPerson { get; set; }
        public string ContractPersonText { get; set; }

        /// <summary>
        /// 承诺书
        /// </summary>
        public string UndertakingFile { get; set; }

        /// <summary>
        /// 还款委托书
        /// </summary>
        public string RepaymentAttorneyFile { get; set; }

        /// <summary>
        /// 联系方式确认书
        /// </summary>
        public string ContactConfirmFile { get; set; }

        /// <summary>
        /// 授权委托书
        /// </summary>
        public string PowerAttorneyFile { get; set; }

        /// <summary>
        /// 收件收据
        /// </summary>
        public string CollectionFile { get; set; }


        /// <summary>
        /// 状态(1:签约成功，2:签约拒绝)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 四条,由借条,收据，承诺书,联系方式确认书合并
        /// </summary>
        public string FourFile { get; set; }

        #region 打款账户

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        public string OpeningSite { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }

        #endregion

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
        #endregion
        /// <summary>
        /// 出借人姓名
        /// </summary>
        public string LenderName { get; set; }


        /// <summary>
        /// 合作渠道
        /// </summary>
        public virtual List<IntroducerAudit> Introducer{ get; set; }
    }
}
