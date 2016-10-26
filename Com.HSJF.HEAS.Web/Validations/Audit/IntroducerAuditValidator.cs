using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Audit
{
    public class IntroducerAuditValidator : IValidator<IntroducerAuditViewModel>
    {
        public ValidateResult Validate(IntroducerAuditViewModel target)
        {
            var result = new ValidateResult();

            if (target.Name.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("Name", "合作渠道 名称不能为空"));
            }

            if (target.Contract.Trim().IsNullOrWhiteSpace())
            {
                result.Add(new ErrorMessage("Contract", "合作渠道 联系电话不能为空"));
            }

            return result;
        }
    }
}