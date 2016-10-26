using System.Text.RegularExpressions;
using Com.HSJF.HEAS.Web.Models;

namespace Com.HSJF.HEAS.Web.Validations.Common
{
    /// <summary>
    /// 大陆身份证号码验证
    /// </summary>
    public class IdCardValidator : IValidator<string>
    {
        public ValidateResult Validate(string target)
        {
            ValidateResult result = new ValidateResult();

            if (!Regex.IsMatch(target, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
            {
                result.Add(new ErrorMessage()
                {
                    Key = target,
                    Message = "身份证号码格式不正确"
                });
            }

            return result;
        }
    }
}