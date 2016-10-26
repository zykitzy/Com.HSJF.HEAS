using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Biz;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.Web.Validations.Biz
{
    /// <summary>
    /// 抵押物验证器
    /// </summary>
    public class CollateralValidator : IValidator<CollateralViewModel>
    {
        public ValidateResult Validate(CollateralViewModel target)
        {
            var result = new ValidateResult();

            if (target.CollateralType.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("", "房产信息 类型不能为空"));
            }

            if (target.HouseNumber.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("", "房产信息 产证编号不能为空"));
            }

            if (target.HouseFile.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("", "房产信息 房产证复印件不能为空"));
            }

            if (target.Address.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("", "房产信息 地址不能为空"));
            }

            if (target.RightOwner.IsNullOrEmpty())
            {
                result.Add(new ErrorMessage("", "房产信息 权利人不能为空"));
            }

            target.HouseSize.HasValue.IfFalse(() =>
            {
                result.Add(new ErrorMessage("", "房产信息 面积不能为空"));
            });

            return result;
        }
    }
}