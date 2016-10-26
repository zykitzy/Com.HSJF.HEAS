using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Authentication
{
    public interface IFormsAuthentication
    {
        string GetUserToken();
    }

}
