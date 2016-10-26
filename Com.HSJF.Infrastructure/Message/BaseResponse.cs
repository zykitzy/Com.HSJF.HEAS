using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Message
{
    public abstract class BaseResponse
    {
        public BaseResponse()
        {
            this.ExtendedProperties = new Dictionary<object, object>();
            this.Data = new object();
        }

        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public IDictionary<object, object> ExtendedProperties { get; set; }

        public object Data { get; set; }
    }
}
