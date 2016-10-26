namespace Com.HSJF.HEAS.Web.Validations
{
    /// <summary>
    /// 验证器接口
    /// </summary>
    public interface IValidator<in T>
    {
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="target">待验证对象</param>
        /// <returns>验证结果</returns>
        ValidateResult Validate(T target);
    }
}
