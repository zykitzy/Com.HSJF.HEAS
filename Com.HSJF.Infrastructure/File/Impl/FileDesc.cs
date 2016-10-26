using Com.HSJF.Infrastructure.File.FileInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File
{
    public partial class FileDescription
    {
        public int FileState { get; set; }

        [NotMapped]
        public bool HasID
        {
            get
            {
                return (this.ID == null || this.ID == Guid.Empty) ? false : true;
            }
        }
    }
}
