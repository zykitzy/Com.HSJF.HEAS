using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FlatFormatAttribute : Attribute
    {
        public FlatFormatAttribute()
        {
        }

        public string Group1Prop { get; set; }
        public string Group2Prop { get; set; }
        public string Group3Prop { get; set; }

        public int StartOrder { get; set; }
    }
}
