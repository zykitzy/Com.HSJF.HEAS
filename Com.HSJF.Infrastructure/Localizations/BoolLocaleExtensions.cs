using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Localizations
{
    public static class BoolLocaleExtensions
    {
        public static string GetLocalString(this bool? val)
        {
            return val.HasValue ? GetLocalString(val.Value) : null;
        }

        public static string GetLocalString(this bool val)
        {
            return val ? Resource.Bool_True : Resource.Bool_False;
        }

        public static bool? ToNullableBool(this string localText)
        {
            return string.IsNullOrWhiteSpace(localText) ? (bool?)null : ToBool(localText);
        }

        public static bool ToBool(this string localText)
        {
            return string.Equals(localText, Resource.Bool_True, StringComparison.OrdinalIgnoreCase);
        }
    }
}
