using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.Infrastructure.Extensions;
using System;
using WebGrease.Css.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Audit
{
    /// <summary>
    /// 审计案件信息验证
    /// </summary>
    public class EditBaseAuditValidator : IValidator<BaseAuditViewModel>
    {
        public ValidateResult Validate(BaseAuditViewModel target)
        {
            var result = new ValidateResult();



            //target.BorrowerPerson.IfNotNull(p =>
            //{
            //    if (p.ExpiryDate.HasValue)
            //    {
            //        if (p.ExpiryDate < DateTime.Now)
            //        {
            //            result.Add(new ErrorMessage("ExpiryDate", p.Name + " 证件有效期不能小于今天"));
            //        }
            //    }

            //    if (p.Birthday.HasValue)
            //    {
            //        if (p.Birthday > DateTime.Now)
            //        {
            //            result.Add(new ErrorMessage("Birthday", p.Name + " 出生日期不能大于今天"));
            //        }
            //    }
            //});            

            //验证抵押物字段
            if (target.CollateralAudits != null)
            {
                foreach (var item in target.CollateralAudits)
                {
                    if (item.CompletionDate.IsNullOrEmpty())
                        result.Add(new ErrorMessage("CompletionDate", "房产信息 - 竣工年份 不能为空"));
                    if (item.HouseType.IsNullOrEmpty())
                        result.Add(new ErrorMessage("HouseType", "房产信息 - 房屋类型 不能为空"));
                    if (item.LandType.IsNullOrEmpty())
                        result.Add(new ErrorMessage("LandType", "房产信息 - 土地类型 不能为空"));
                }
            }

            target.RelationPersonAudits.IfNotNull(persons => persons.ForEach(p =>
            {
                if (p.ExpiryDate.HasValue)
                {
                    if (p.ExpiryDate.HasValue)
                    {
                        if (p.ExpiryDate < DateTime.Now)
                        {
                            result.Add(new ErrorMessage("ExpiryDate", p.Name + " 证件有效期不能小于今天"));
                        }
                    }

                    if (p.Birthday.HasValue)
                    {
                        if (p.Birthday > DateTime.Now)
                        {
                            result.Add(new ErrorMessage("Birthday", p.Name + " 出生日期不能大于今天"));
                        }
                    }
                }
            }));

            return result;
        }
    }
}