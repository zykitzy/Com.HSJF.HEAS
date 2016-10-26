using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class DisplayingAttribute : Attribute
    {
        public string Name { get; set; }
        public Type ResourceType { get; set; }
        public object Tag { get; set; }
    }
}
