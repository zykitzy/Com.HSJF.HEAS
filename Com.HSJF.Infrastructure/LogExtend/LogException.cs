using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.LogExtend
{
    [Serializable]
    public class LogException : Exception
    {
        public LogException() : base("Log Can not be work!") { }
    }
}
