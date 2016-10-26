using System;
using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.EntityFramework.Model.Sales;

namespace Com.HSJF.HEAS.Web.Models.Sales
{
    public class SalesManViewModel
    {
        public string ID { get; set; }

        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "生日")]
        [Required]
        public DateTime? Birthday { get; set; }
        
        [Display(Name = "销售编号")]
        public string SalesID { get; set; }
       
        [Display(Name = "职务")]
        public string Post { get; set; }
        
        public int? State { get; set; }

        [Display(Name = "销售组")]
        public string GroupID { get; set; }

        public SalesMan CastDB(SalesManViewModel model)
        {
            return new SalesMan
            {
                ID = model.ID,
                Birthday = model.Birthday,
                GroupID = model.GroupID,
                Name = model.Name,
                Post = model.Post,
                SalesID = model.SalesID,
                State = model.State
            };
        }
        
        public SalesManViewModel CastModel(SalesMan model)
        {
            return new SalesManViewModel
            {
                ID = model.ID,
                Birthday = model.Birthday,
                GroupID = model.GroupID,
                Name = model.Name,
                Post = model.Post,
                SalesID = model.SalesID,
                State = model.State
            };
        }
    }
}
