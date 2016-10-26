using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Audit
{
    public class ApprovalBaseAuditValidator : IValidator<BaseAuditViewModel>
    {
        public ValidateResult Validate(BaseAuditViewModel target)
        {
            var result = new ValidateResult();

            if (!target.AuditAmount.HasValue)
            {
                result.Add(new ErrorMessage("AuditAmount", "审批金额不能为空"));
            }
            if (target.AuditTerm.IsNull())
            {
                result.Add(new ErrorMessage("AuditTerm", "审批期限不能为空"));
            }

            if (!target.AuditRate.HasValue)
            {
                result.Add(new ErrorMessage("AuditRate", "审批利率不能为空"));
            }
            else
            {
                if (!(target.AuditRate.Value < 100 && target.AuditRate.Value >= 2))
                {
                    result.Add(new ErrorMessage("AuditRate", "审批利率必须在2-100之间"));
                }
            }

            return result;
        }
    }
}