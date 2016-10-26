using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File.FileInterface
{
    public class FileStorage
    {
        public Guid ID { get; set; }
        public virtual byte[] FileData { get; set; }
        public string FileUrl { get; set; }
        public string StoreType { get; set; }
        public string MD5 { get; set; }

    }
}
