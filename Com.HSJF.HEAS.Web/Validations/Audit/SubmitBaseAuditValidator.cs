using System.Linq;
using Com.HSJF.Framework.DAL;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Validations.Common;
using Com.HSJF.Infrastructure;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Audit
{
    /// <summary>
    /// 提交审核案件信息验证
    /// </summary>
    public class SubmitBaseAuditValidator : IValidator<BaseAuditViewModel>
    {
        private readonly IValidator<CollateralAuditViewModel> _collateralValidator = null;

        public IValidator<CollateralAuditViewModel> CollateraValidator
        {
            get { return _collateralValidator ?? new CollateraAuditlValidator(); }
        }

        public ValidateResult Validate(BaseAuditViewModel target)
        {
            var result = new ValidateResult();

            // 验证个人信息
            //if (target.BorrowerPerson.IsNull())
            //{
            //    result.Add(new ErrorMessage("RelationPerson", "借款人 不能为空"));
            //}

            if (!target.ComprehensiveRate.HasValue)
            {
                result.Add(new ErrorMessage("ComprehensiveRate", "综合抵押率 不能为空"));
            }

            //if (target.Merchandiser.IsNull())
            //{
            //    result.Add(new ErrorMessage("Merchandiser", "跟单人 不能为空"));
            //}


            //target.BorrowerPerson.IfNotNull(p =>
            //{
            //    if (p.IndividualFile.IsNullOrEmpty())
            //    {
            //        result.Add(new ErrorMessage("BorrowerPerson.IndividualFile",
            //            string.Format("借款人{0}个人征信不能为空", target.BorrowerPerson.Name)));
            //    }

            //    if (p.ContactAudits.IsNull())
            //    {
            //        result.Add(new ErrorMessage("", p.Name + " 联系方式不能为空"));
            //    }

            //    //if (p.AddressAudits.IsNull())
            //    //{
            //    //    result.Add(new ErrorMessage("", p.Name + " 地址不能为空"));
            //    //}
            //});


            // 抵押物
            if (target.CollateralAudits.IsNull() ||
                target.CollateralAudits.All(x => x.CollateralType != DictionaryType.FacilityCategary.MainFacility))
            {
                result.Add(new ErrorMessage("Collateral", "房产信息 - 抵押物 不能为空"));
            }

            if (target.CollateralAudits != null && target.CollateralAudits.Any())
            {
                target.CollateralAudits.ForEach(p => result.Add(CollateraValidator.Validate(p).GetErrors()));
            }

            // 借款人证件号
            //if (target.BorrowerPerson.IdentificationNumber.IsNotNull())
            //{
            //    if (target.BorrowerPerson.IdentificationType == DictionaryType.DocType.IdCard)
            //    {
            //        if (new IdCardValidator().Validate(target.BorrowerPerson.IdentificationNumber).IsNotValid())
            //        {
            //            result.Add(new ErrorMessage("BorrowerPerson.IdentificationNumber",
            //                string.Format("借款人{0}身份证号码不正确", target.BorrowerPerson.Name)));
            //        }
            //    }
            //}

            // 关系人信息验证身份证号码
            if (target.RelationPersonAudits.IsNotNull())
            {
                target.RelationPersonAudits.Where(p => p.IdentificationType == DictionaryType.DocType.IdCard).ForEach(item =>
                {
                    if (new IdCardValidator().Validate(item.IdentificationNumber).IsNotValid())
                    {
                        result.Add(new ErrorMessage("RelationPersonAudits.IdentificationNumber",
                            string.Format("{0}身份证号码不正确", item.Name)));
                    }
                });

                target.RelationPersonAudits.ForEach(p =>
                {
                    if (p.IndividualFile.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("BorrowerPerson.IndividualFile",
                            string.Format("借款人{0}个人征信不能为空", p.Name)));
                    }
                });

                // 验证配偶只能有一位
                if (target.RelationPersonAudits.Count(p => p.RelationType == DictionaryType.PersonType.JieKuanRenPeiOu) > 1)
                {
                    result.Add(new ErrorMessage("RelationPerson", "借款人配偶只能添加一位"));
                }

                // 验证关系人婚姻状态
                target.RelationPersonAudits.ForEach(p =>
                {
                    if (p.MaritalStatus.IsNull())
                    {
                        result.Add(new ErrorMessage("MaritalStatus", p.Name + " 婚姻状态不能为空"));
                    }

                    if (p.MaritalStatus.IsNotNull() && p.MaritalStatus != DictionaryType.MaritalStatus.UnMarried)
                    {
                        if (p.MarryFile.IsNull())
                        {
                            result.Add(new ErrorMessage("MaritalStatus", p.Name + "需要上传婚姻证明"));
                        }
                    }
                });
            }

            //借款人单身需要上传婚姻证明
            //target.BorrowerPerson.IfNotNull(p =>
            //{
            //    if (p.MaritalStatus.IsNull())
            //    {
            //        result.Add(new ErrorMessage("MaritalStatus", p.Name + " 婚姻状态不能为空"));
            //    }

            //    if (p.MaritalStatus.IsNotNull() && p.MaritalStatus != DictionaryType.MaritalStatus.UnMarried)
            //    {
            //        if (p.MarryFile.IsNull())
            //        {
            //            result.Add(new ErrorMessage("MaritalStatus", p.Name + "需要上传婚姻证明"));
            //        }
            //    }
            //});

            target.EnforcementPersons.IfNotNull(persons =>
            {
                persons.ForEach(p =>
                {
                    if (p.PersonID.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("PersonId", "被执行对象 姓名不能为空"));
                    }

                    if (p.EnforcementWeb.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("EnforcementWeb", "被执行对象 全国被执行网情况 不能为空"));
                    }

                    if (p.TrialRecord.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("TrialRecord", "被执行对象 法院开庭记录 不能为空"));
                    }

                    if (p.LawXP.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("TrialRecord", "被执行对象 Lawxin 不能为空"));
                    }

                    if (p.ShiXin.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("TrialRecord", "被执行对象 失信网失信记录 不能为空"));
                    }

                    if (p.BadNews.IsNullOrEmpty())
                    {
                        result.Add(new ErrorMessage("BadNews", "被执行对象 失信网失信记录 不能为空"));
                    }
                });
            });

            return result;
        }
    }
}