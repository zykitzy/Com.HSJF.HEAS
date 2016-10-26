using Com.HSJF.Infrastructure.File.FileInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File
{

    /// <summary>
    /// 表名必须为 FileDescription
    /// </summary>
    public partial class FileDescription
    {
        /// <summary>
        ///   存取于数据库时所需要的主键
        /// </summary>
        public virtual Guid ID { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public virtual Guid? LinkID { get; set; }
        /// <summary>
        ///  标注获取用途
        /// </summary>
        public virtual string LinkKey { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// 存储到服务器上后的文件名
        /// </summary>
        public virtual string FileSaveName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual string FileType { get; set; }

        /// <summary>
        /// 文件创建类型
        /// </summary>
        public virtual DateTime FileCreateTime { get; set; }

        /// <summary>
        ///文件实际数据
        /// </summary>
        public virtual byte[] FileData { get; set; }
        /// <summary>
        /// 文件说明
        /// </summary>
        public virtual string Description { get; set; }

        public virtual Guid FileStorageID { get; set; }

        //public virtual FileStorage FileStorage { get; set; }

    }
}
