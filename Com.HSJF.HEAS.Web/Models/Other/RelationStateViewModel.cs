using System;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Other
{
    public class RelationStateViewModel
    {
        [Key]
        [Display(Name = "主键")]
        public Guid ID { get; set; }

        [Display(Name = "案件编号")]
        public string CaseNum { get; set; }

        [Display(Name = "销售ID")]
        public string SalesID { get; set; }

        [Display(Name = "关系类型")]
        public int RelationType { get; set; }

        [Display(Name = "关系号码")]
        public string RelationNumber { get; set; }

        [Display(Name = "是否锁定")]
        public int IsLock { get; set; }

        [Display(Name = "是否绑定")]
        public int IsBinding { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "备注")]
        public string Desc { get; set; }

        [Display(Name = "是否启用")]
        public int State { get; set; }
    }
}