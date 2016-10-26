using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Authentication
{
    public class IdentityExtension : IIdentity
    {
        public string AreaName { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }

        public string AuthenticationType
        {
            get { return "Custom Authentication"; }
        }

        public bool IsAuthenticated
        {
            get { return string.IsNullOrEmpty(Name) ? false : true; }
        }

    }

    public class PrincipalExtension : IPrincipal
    {
        private IIdentity identity;

        /// <summary>
        /// 初始化一个Principal
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="area">用户所属地区</param>
        /// <param name="depart">用户所属部门</param>
        public PrincipalExtension(string name, string area, string depart)
        {
            identity = new IdentityExtension()
            {
                Name = name,
                AreaName = area,
                Department = depart
            };
        }

        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            return true;
        }

    }
}
