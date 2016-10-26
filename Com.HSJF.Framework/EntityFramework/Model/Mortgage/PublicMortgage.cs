using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Infrastructure.DoMain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.Framework.EntityFramework.Model.Mortgage
{
    public class PublicMortgage : EntityModel
    {
        public string ID { get; set; }
       
        [Display(Name = "合同文件")]
        public string ContractFile { get; set; }
       
        [Display(Name = "借条")]
        public string NoteFile { get; set; }
       
        [Display(Name = "收据")]
        public string ReceiptFile { get; set; }
        
        [Display(Name = "它证")]
        public string OtherFile { get; set; }

        /// <summary>
        /// 四条（合并NoteFile,ReceiptFile,UndertakingFile,ContactConfirmFile）
        /// </summary>
        public string FourFile { get; set; }


        //以下 2-16-06-16 第一次测试之后新增
        public string ContractNo { get; set; }
        public decimal? ContractAmount { get; set; }
        public DateTime? ContractDate { get; set; }
        public string ContractPerson { get; set; }           
        public string UndertakingFile { get; set; }
        public string RepaymentAttorneyFile { get; set; }
        public string ContactConfirmFile { get; set; }
        public string PowerAttorneyFile { get; set; }
        public string CollectionFile { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string LenderName { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }
    }
}
