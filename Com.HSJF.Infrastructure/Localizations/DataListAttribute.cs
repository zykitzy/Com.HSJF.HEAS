using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataListAttribute : Attribute
    {
        public string DataProperty { get; set; }
    }
}
