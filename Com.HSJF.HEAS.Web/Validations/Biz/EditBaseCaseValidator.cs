using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Biz;
using Com.HSJF.Infrastructure.Extensions;
using System;
using Com.HSJF.Infrastructure;

namespace Com.HSJF.HEAS.Web.Validations.Biz
{
    /// <summary>
    /// 编辑基础案件校验(数据合法性校验)
    /// </summary>
    public class EditBaseCaseValidator : IValidator<BaseCaseViewModel>
    {
        public ValidateResult Validate(BaseCaseViewModel target)
        {
            var result = new ValidateResult();

            // 验证借款人证件有效期
            //target.BorrowerPerson.IfNotNull(p =>
            //{
            //    if (p.ExpiryDate.HasValue)
            //    {
            //        if (p.ExpiryDate.Value < DateTime.Now)
            //        {
            //            result.Add(new ErrorMessage("ExpiryDate", p.Name + " 证件号有效期不能小于今天"));
            //        }
            //    }

            //    if (p.Birthday.HasValue)
            //    {
            //        if (p.Birthday.Value > DateTime.Now)
            //        {
            //            result.Add(new ErrorMessage("Birthday", p.Name + " 出生日期不能大于今天"));
            //        }
            //    }
            //});

            // 验证关系人证件有效期
            target.RelationPerson.IfNotNull(p => p.ForEach(single =>
            {
                if (single.ExpiryDate.HasValue)
                {
                    if (single.ExpiryDate.Value < DateTime.Now)
                    {
                        result.Add(new ErrorMessage("ExpiryDate", single.Name + " 证件号有效期不能小于今天"));
                    }
                }

                if (single.Birthday.HasValue)
                {
                    if (single.Birthday.Value > DateTime.Now)
                    {
                        result.Add(new ErrorMessage("Birthday", single.Name + " 出生日期不能大于今天"));
                    }
                }
            }));
            return result;
        }
    }
}