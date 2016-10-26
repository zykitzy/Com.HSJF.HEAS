using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    /// <summary>
    /// 地址
    /// </summary>
    public class AddressViewModel
    {
        [Key]
        public string ID { get; set; }

        [Display(Name = "地址类型")]
        [Required]
        public string AddressType { get; set; }
        
        public string AddressTypeText { get; set; }

        [Display(Name = "地址信息")]
        [Required]
        public string AddressInfo { get; set; }

        public string PersonID { get; set; }

        [Display(Name = "是否为默认地址")]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
