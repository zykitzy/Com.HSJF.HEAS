using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataSourceAttribute : Attribute
    {
        public Type SourceType { get; set; }
        public string SelectMethod { get; set; }
        public Type DataObjectType { get; set; }
        public string BindKey { get; set; }
    }
}
