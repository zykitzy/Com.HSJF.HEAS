using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Validations
{
    /// <summary>
    /// 验证工厂
    /// </summary>
    public class ValidationFactory
    {
        /// <summary>
        /// 创建验证操作
        /// </summary>
        public static IValidation Create()
        {
            return new Validation();
        }
    }
}

