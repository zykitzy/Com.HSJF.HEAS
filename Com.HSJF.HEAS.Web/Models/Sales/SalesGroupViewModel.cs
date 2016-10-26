using System.ComponentModel.DataAnnotations;
using Com.HSJF.Framework.EntityFramework.Model.Sales;

namespace Com.HSJF.HEAS.Web.Models.Sales
{
    public class SalesGroupViewModel
    {
        public string ID { get; set; }

        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "所属公司")]
        [Required]
        public string Company { get; set; }

        [Display(Name = "公司注册码")]
        [Required]
        public string CompanyCode { get; set; }

        [Required]
        [Display(Name = "简码")]
        public string ShortCode { get; set; }

        public int? State { get; set; }

        /// <summary>
        ///地区ID 
        /// </summary> 
        [Display(Name = "所属地区")]
        public string DistrictID { get; set; }

        /// <summary>
        /// 地区对象
        /// </summary>
        public virtual DistrictViewModel District { get; set; }

        public SalesGroup CastDB(SalesGroupViewModel model)
        {
            return new SalesGroup
            {
                ID = model.ID,
                Company = model.Company,
                CompanyCode = model.CompanyCode,
                ShortCode = model.ShortCode,
                Name = model.Name,
                State = model.State,
                DistrictID = model.DistrictID
            };
        }


        public SalesGroupViewModel CastModel(SalesGroup model)
        {
            return new SalesGroupViewModel
            {
                ID = model.ID,
                Company = model.Company,
                CompanyCode = model.CompanyCode,
                Name = model.Name,
                ShortCode = model.ShortCode,
                State = model.State,
                DistrictID = model.DistrictID,
                District = new DistrictViewModel().CastModel(model.District)
            };
        }

    }
}
