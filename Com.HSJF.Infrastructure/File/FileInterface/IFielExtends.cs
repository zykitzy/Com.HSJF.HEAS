using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File.FileInterface
{
    interface IFielExtends
    {
        string Filepath { get; set; }
        /// <summary>
        /// 获取文件以字节返回
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        byte[] GetFileByte(string filepath);

        /// <summary>
        /// 获取文件以字符串返回
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        string GetFileString(string filepath);
    }
}
