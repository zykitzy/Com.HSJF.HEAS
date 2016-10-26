using Com.HSJF.Infrastructure.File.FileInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.File
{
    public class FileExtend : IFielExtends
    {
        private FileStream _file;
        private string _filepath;
        public FileExtend()
        {

        }
        public FileExtend(string filepath)
        {
            _file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Filepath = filepath;
        }

        public string Filepath
        {
            get
            {
                return _filepath;
            }

            set
            {
                _filepath = value;
            }
        }

        public byte[] GetFileByte()
        {
            return this.GetFileByte(Filepath);
        }
        /// <summary>
        /// 获取文件以字节返回
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public byte[] GetFileByte(string filepath)
        {

            _file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            return GetFileByte(_file);
        }

        public byte[] GetFileByte(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            BinaryReader _br = new BinaryReader(stream);
            return _br.ReadBytes((int)stream.Length);
        }

        /// <summary>
        /// 获取文件以字符串返回
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public string GetFileString(string filepath)
        {
            StreamReader _sr = new StreamReader(_file);
            return _sr.ReadToEnd();
        }

    }
}
