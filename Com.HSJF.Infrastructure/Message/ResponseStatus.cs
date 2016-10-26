using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Message
{
    public enum ResponseStatus : int
    {
        Success = 1,
        Warning = 2,
        Error = 4
    }
}
