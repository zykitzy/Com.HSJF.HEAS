using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Infrastructure.DoMain;
using System;

namespace Com.HSJF.Framework.EntityFramework.Model.Lending
{
    public class Lending : EntityModel
    {

        public string ID { get; set; }

        public DateTime? LendTime { get; set; }

        //2016-09-20 新增收款日期
        public int? PaymentDay { get; set; }
        public string LendFile { get; set; }

        //以下 2-16-06-16 第一次测试之后新增
        public string CustomerName { get; set; }

        public string ContactNumber { get; set; }

        public string Borrower { get; set; }

        public string BorrowNumber { get; set; }

        public string OpeningBank { get; set; }

        public decimal? ContractAmount { get; set; }

        public string CreateUser { get; set; }

        public string CreateTime { get; set; }

        public virtual BaseAudit BaseAudit { get; set; }
    }
}
