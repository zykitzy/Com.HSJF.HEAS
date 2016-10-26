using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.EntityFramework.Model.Sales;

namespace Com.HSJF.HEAS.Web.Models.Sales
{
    public class DistrictViewModel
    {
        [Key]
        public string ID { get; set; }

        [Display(Name = "地区名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "简号")]
        public string ShortNumber { get; set; }
        /// <summary>
        /// 销售组
        /// </summary>
        public IEnumerable<SalesGroupViewModel> SalesGroup { get; set; }

        public District CastDB(DistrictViewModel model)
        {
            return new District
            {
                ID = model.ID,
                Name = model.Name,
                ShortNumber = model.ShortNumber
            };
        }


        public DistrictViewModel CastModel(District model)
        {
            return new DistrictViewModel
            {
                ID = model.ID,
                Name = model.Name,
                ShortNumber = model.ShortNumber
            };
        }

    }
}