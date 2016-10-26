using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.HSJF.Infrastructure.Localizations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FlatDisplayAttribute : Attribute
    {
        public FlatDisplayAttribute()
        {
        }
        public string Name { get; set; }
        public Type ResourceType { get; set; }
        public int Order { get; set; }

        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
    }
}
