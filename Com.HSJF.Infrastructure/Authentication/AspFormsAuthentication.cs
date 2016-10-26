using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.HSJF.Infrastructure.Authentication
{
    public class AspFormsAuthentication : IFormsAuthentication
    {
        public string GetUserToken()
        {
            return HttpContext.Current.User.Identity.Name.ToLower();
        }
    }
}
