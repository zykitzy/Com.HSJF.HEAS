using Com.HSJF.Framework.DAL;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.HatsCase
{
    public class HatsCaseValidator : IValidator<Models.HatsCase.HatsCase>
    {
        public ValidateResult Validate(Models.HatsCase.HatsCase target)
        {
            var result = new ValidateResult();

            if (target.TransferType.IsNullOrEmpty() || target.TransferType.IsNullOrWhiteSpace())
            {
                result.Add(new ErrorMessage("TransferType", "TransferType参数不能为空"));
            }

            if (target.CaseNum.IsNullOrEmpty() || target.CaseNum.IsNullOrWhiteSpace())
            {
                result.Add(new ErrorMessage("CaseNum", "CaseNum参数不能为空"));
            }

            if (target.TransferType == DictionaryType.CaseMode.JuJian)
            {
                if (target.ThirdParty.IsNullOrEmpty() || target.ThirdParty.IsNullOrWhiteSpace())
                {
                    result.Add(new ErrorMessage("ThirdParty", "ThirdParty参数不能为空"));
                }

                if (!target.EarnestMoney.HasValue)
                {
                    result.Add(new ErrorMessage("EarnestMoney", "EarnestMoney参数不能为空"));
                }

                if (!target.TransferInterest.HasValue)
                {
                    result.Add(new ErrorMessage("TransferInterest", "TransferInterest参数不能为空"));
                }

                if (target.TransferTerm.IsNullOrEmpty() || target.TransferTerm.IsNullOrWhiteSpace())
                {
                    result.Add(new ErrorMessage("TransferTerm", "TransferTerm参数不能为空"));
                }

                if (!target.TransferAmount.HasValue)
                {
                    result.Add(new ErrorMessage("TransferAmount", "TransferAmount参数不能为空"));
                }
            }

            return result;
        }
    }
}