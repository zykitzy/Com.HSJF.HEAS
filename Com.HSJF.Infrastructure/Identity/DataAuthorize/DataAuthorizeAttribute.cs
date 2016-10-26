using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.DataAuthorize
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataAuthorizeAttribute : System.Attribute
    {
        private string authorize;

        /// <summary>
        /// 将需要验证的字段组合
        /// </summary>
        /// <param name="authorizeColumn">需要组合验证的字段</param>
        public DataAuthorizeAttribute(string authorizeColumn)
        {
            this.authorize = authorizeColumn;
        }

        /// <summary>
        /// 数据权限需要结合哪些字段验证
        /// </summary>
        public string AuthorizeColumn
        {
            get
            {
                return authorize;
            }
        }

    }
}
