using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class IntroducerViewModel
    {
        [Key]
        public string ID { get; set; }
       
        [Display(Name = "介绍人名称")]
        public string Name { get; set; }
       
        [Display(Name = "介绍人联系电话")]
        public string Contract { get; set; }
       
        [Display(Name = "返利金额")]
        public decimal? RebateAmmount { get; set; }
        
        [Display(Name = "返利百分比")]
        public decimal? RebateRate { get; set; }
       
        [Display(Name = "介绍人账户")]
        public string Account { get; set; }
       
        [Display(Name = "介绍人开户行")]
        public string AccountBank { get; set; }
      
        public string CaseID { get; set; }
      
        public BaseCaseViewModel BaseCase { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}