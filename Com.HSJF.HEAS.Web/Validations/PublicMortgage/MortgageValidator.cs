using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Mortgage;
using Com.HSJF.HEAS.Web.Validations.Audit;
using Com.HSJF.Infrastructure.Extensions;
using WebGrease.Css.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.PublicMortgage
{
    public class MortgageValidator : IValidator<PublicMortgageViewModel>
    {
        public ValidateResult Validate(PublicMortgageViewModel target)
        {
            var result = new ValidateResult();

            if (target.ContractNo.IsNull())
            {
                result.Add(new ErrorMessage("ContractNo", "合同号不能为空"));
            }

            if (target.ContractPerson.IsNull())
            {
                result.Add(new ErrorMessage("ContractPerson", "经办人不能为空"));
            }

            if (!target.ContractAmount.HasValue)
            {
                result.Add(new ErrorMessage("ContractAmount", "签约金额不能为空"));
            }
            if (target.ContractAmount.HasValue)
            {
                if (target.ContractAmount.Value < 300000 || target.ContractAmount.Value > 100000000000000000)
                {
                    result.Add(new ErrorMessage("ContractAmount", "签约金额不能小于300000"));
                }

                if (target.ContractAmount > target.AuditAmount)
                {
                    result.Add(new ErrorMessage("ContactAmount", "签约金额不能大于审批金额"));
                }
            }

            //if (target.OtherFile == null && target.CollectionFile == null)
            //{
            //    result.Add(new ErrorMessage("OtherFile/CollectionFile", "他证和收件收据需要任意上传一项"));
            //}

            if (target.Introducer.IsNotNull())
            {
                var introduceValidator = new IntroducerAuditValidator();
                target.Introducer.ForEach(p =>
                {
                    result.Add(introduceValidator.Validate(p).GetErrors());
                });
            }

            return result;
        }
    }
}