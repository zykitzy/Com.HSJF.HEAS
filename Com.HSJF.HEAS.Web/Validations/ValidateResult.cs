using System.Linq;
using System.Collections.Generic;
using Com.HSJF.HEAS.Web.Models;

namespace Com.HSJF.HEAS.Web.Validations
{
    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidateResult
    {
        private IList<ErrorMessage> _errors;

        public ValidateResult()
        {
            _errors = new List<ErrorMessage>();
        }

        /// <summary>
        /// 通过验证
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !_errors.Any();
        }

        /// <summary>
        /// 不通过验证
        /// </summary>
        /// <returns></returns>
        public bool IsNotValid()
        {
            return _errors.Any();
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>错误信息</returns>
        public IEnumerable<ErrorMessage> GetErrors()
        {
            return _errors;
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public void Add(ErrorMessage error)
        {
            _errors.Add(error);
        }

        /// <summary>
        /// 添加多条错误信息
        /// </summary>
        /// <param name="errors">错误信息</param>
        public void Add(IEnumerable<ErrorMessage> errors)
        {
            if (errors != null)
            {
                var iterator = errors.GetEnumerator();
                while (iterator.MoveNext())
                {
                    _errors.Add(iterator.Current);
                }
            }
        }
    }
}