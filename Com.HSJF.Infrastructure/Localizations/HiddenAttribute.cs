using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HiddenAttribute : Attribute
    {
    }
}
