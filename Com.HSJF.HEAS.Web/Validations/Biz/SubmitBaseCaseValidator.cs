using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Biz;
using Com.HSJF.HEAS.Web.Validations.Common;
using Com.HSJF.Infrastructure.Extensions;
using System.Linq;
using Com.HSJF.Framework.DAL;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Biz
{
    /// <summary>
    /// 提交进件信息验证
    /// </summary>
    public class SubmitBaseCaseValidator : IValidator<BaseCaseViewModel>
    {
        private readonly IValidator<CollateralViewModel> _collateralValidator = null;

        public IValidator<CollateralViewModel> CollateraValidator
        {
            get { return _collateralValidator ?? new CollateralValidator(); }
        }

        public ValidateResult Validate(BaseCaseViewModel target)
        {
            var result = new ValidateResult();

            // 验证抵押物
            if (target.Collateral == null || target.Collateral.All(x => x.CollateralType != "-FacilityCategary-MainFacility"))
            {
                result.Add(new ErrorMessage()
                {
                    Key = "Collateral",
                    Message = "房产信息 - 抵押物 不能为空"
                });
            }

            if (target.Collateral != null && target.Collateral.Any())
            {
                target.Collateral.ForEach(p => result.Add(CollateraValidator.Validate(p).GetErrors()));

            }

            //验证配偶唯一性
            if (target.RelationPerson.IsNotNull())
            {
                if (target.RelationPerson.Count(p => p.RelationType.Equals(DictionaryType.PersonType.JieKuanRenPeiOu)) > 1)
                {
                    result.Add(new ErrorMessage("RelationPerson", "借款人配偶只能添加一位"));
                }
            }

            // 借款人
            //if (target.BorrowerPerson == null)
            //{
            //    result.Add(new ErrorMessage()
            //    {
            //        Key = "RelationPerson",
            //        Message = "借款人 不能为空"
            //    });
            //}

            //借款人证件号
            //if (target.BorrowerPerson != null && target.BorrowerPerson.IdentificationNumber != null)
            //{
            //    if (target.BorrowerPerson.IdentificationType == DictionaryType.DocType.IdCard)
            //    {
            //        var idCardValidator = new IdCardValidator().Validate(target.BorrowerPerson.IdentificationNumber);
            //        if (idCardValidator.IsNotValid())
            //        {
            //            result.Add(new ErrorMessage("IdentificationNumber", target.BorrowerPerson.Name + " 身份证号码不正确"));
            //        }
            //    }
            //}

            //关系人信息验证身份证号码
            if (target.RelationPerson != null)
            {
                foreach (var item in target.RelationPerson)
                {
                    if (item.IdentificationType == "-DocType-IDCard")
                    {
                        var idCardValidateResult = new IdCardValidator().Validate(item.IdentificationNumber);
                        if (idCardValidateResult.IsNotValid())
                        {
                            result.Add(new ErrorMessage("IdentificationNumber", item.Name + " 身份证号码不正确"));
                        }
                    }
                }
            }
            //单身需要上传婚姻证明
            //Extensions.IfNotNull(target.BorrowerPerson, p =>
            //{
            //    if (p.MaritalStatus != null && p.MaritalStatus != "-MaritalStatus-Unmarried")
            //    {
            //        if (p.MarryFile == null)
            //        {
            //            result.Add(new ErrorMessage("MaritalStatus", p.Name + "需要上传婚姻证明"));
            //        }
            //    }
            //});

            //验证年利率
            if (target.AnnualRate == null)
            {
                result.Add(new ErrorMessage("AnnualRate", "年化利率不能为空"));
            }

            AjaxMinExtensions.IfNotNull(target.AnnualRate, p =>
            {
                if (p < 2 || p > 100)
                {
                    result.Add(new ErrorMessage("AnnualRate", "年化利率必须在2-100之间"));
                }
            });

            return result;
        }
    }
}