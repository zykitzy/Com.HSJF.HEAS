using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Com.HSJF.Infrastructure.File.FileInterface
{
    interface IFileUpload
    {
        /// <summary>
        /// 文件存储方式，0：存储为文件(默认)，1：存储数据库
        /// </summary>
        int FileSaveType { get; set; }
        DbContext FileContext { get; set; }
        HttpPostedFileBase PostFile { get; set; }

        /// <summary>
        /// 根据属性默认保存
        /// </summary>
        string SaveAs(Guid? linkid = null);

        /// <summary>
        /// 保存到文件
        /// </summary>
        string SaveToFile(Guid? linkid = null);
        /// <summary>
        /// 保存到数据库
        /// </summary>
        string SaveToDB(Guid? linkid = null);

        /// <summary>
        /// 获取第一条符合的记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        FileDescription Single(Guid id, bool hasdata = false);
        /// <summary>
        /// 获取第一条符合的记录
        /// </summary>
        /// <param name="LinkID">外键</param>
        /// <param name="LinkKey">文件标志</param>
        /// <returns></returns>
        FileDescription Single(Guid LinkID, string LinkKey, bool hasdata = false);

        /// <summary>
        ///    获取第一条符合的记录
        /// </summary>
        /// <param name="filedesc"></param>
        /// <returns></returns>
        FileDescription Single(FileDescription filedesc, bool hasdata = false);

        /// <summary>
        /// 获取所有符合的文件
        /// </summary>
        /// <param name="LinkID"></param>
        /// <returns></returns>
        IEnumerable<FileDescription> List(Guid LinkID);
        IEnumerable<FileDescription> List(Guid LinkID, string linkKey);
        IEnumerable<FileDescription> List(FileDescription filedesc);
    }
}







