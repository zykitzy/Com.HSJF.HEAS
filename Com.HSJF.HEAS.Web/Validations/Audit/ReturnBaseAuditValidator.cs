using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;

namespace Com.HSJF.HEAS.Web.Validations.Audit
{
    public class ReturnBaseAuditValidator : IValidator<BaseAuditViewModel>
    {
        public ValidateResult Validate(BaseAuditViewModel target)
        {
            var result = new ValidateResult();
            if (target.AuditAmount != null && target.AuditAmount < 300000)
            {
                result.Add(new ErrorMessage("AuditAmount", "审批金额不能小于300000"));
            }

            return result;
        }
    }
}