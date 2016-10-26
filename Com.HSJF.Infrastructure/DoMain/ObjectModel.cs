using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.DoMain
{
    public class ObjectModel
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
