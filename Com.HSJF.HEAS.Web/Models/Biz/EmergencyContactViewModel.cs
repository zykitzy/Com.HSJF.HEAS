using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class EmergencyContactViewModel
    {
        [Key]
        public string ID { get; set; }

        [Display(Name = "联系人类型")]
        [Required]
        public string ContactType { get; set; }
        public string ContactTypeText { get; set; }

        [Display(Name = "联系人姓名")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "联系人号码")]
        [Required]
        public string ContactNumber { get; set; }

        public string PersonID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
