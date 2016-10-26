using Com.HSJF.Infrastructure.File.FileInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Runtime.Caching;
using System.Configuration;
using System.IO;

namespace Com.HSJF.Infrastructure.File
{
    public class FileUpload : IFileUpload
    {
        #region 构造

        public FileUpload() { }
        public FileUpload(HttpPostedFileBase postfile)
        {
            PostFile = postfile;
        }

        #endregion

        #region 属性
        private int _saveType;
        public int FileSaveType
        {
            get
            {
                return _saveType;
            }

            set
            {
                _saveType = value;
            }
        }

        private DbContext _context;
        public DbContext FileContext
        {
            get
            {
                if (_context == null)
                {

                    _context = new FileContext();
                }
                return _context;
            }
            set
            {
                _context = value;
            }

        }
        HttpPostedFileBase _postFile;
        public HttpPostedFileBase PostFile
        {
            get
            {
                return _postFile;
            }

            set
            {
                _postFile = value;
            }
        }

        #endregion

        public void SaveFileDescription(FileDescription file)
        {
            var entity = FileContext.Set<FileDescription>().FirstOrDefault(t => t.ID == file.ID);

            if (entity != null)
            {
                var entry = FileContext.Entry<FileDescription>(file);
                if (entry.State == EntityState.Detached)
                {
                    FileContext.Set<FileDescription>().Attach(file);
                }
                if (entry.State == EntityState.Unchanged)
                {
                    FileContext.Entry(file).State = EntityState.Modified;
                }
            }
            else
            {
                FileContext.Set<FileDescription>().Add(file);
            }
            FileContext.SaveChanges();
        }

        #region 保存
        public FileDescription Save()
        {
            return Save(PostFile);
        }

        public FileDescription Save(Guid? linkid)
        {
            return Save(PostFile, linkid);
        }
        public FileDescription Save(HttpPostedFileBase upfile, Guid? linkid = null, int filestate = 0)
        {

            FileDescription desc = MapModel(upfile, linkid, filestate);
            if (desc == null) return null;
            desc.FileData = null;
            FileContext.Set<FileDescription>().Add(desc);
            FileContext.SaveChanges();
            return desc;
        }

        public string SaveToDB(Guid? linkid = null)
        {
            FileSaveType = 1;
            var file = Save(linkid);
            if (file != null)
            {
                return file.ID.ToString();
            }
            return string.Empty;

        }

        public string SaveToDB(HttpPostedFileBase upfile, Guid? linkid = null, int filestate = 0)
        {
            FileSaveType = 1;
            var file = Save(upfile, linkid, filestate);
            if (file != null)
            {
                return file.ID.ToString();
            }
            return string.Empty;
        }

        public string SaveToDB(HttpPostedFileBase upfile, DbContext context, Guid? linkid = null)
        {
            FileSaveType = 1;
            FileContext = context;
            var file = Save(upfile, linkid);
            if (file != null)
            {
                return file.ID.ToString();
            }
            return string.Empty;
        }

        public string SaveAs(Guid? linkid = null)
        {
            var file = Save(linkid);
            if (file != null)
            {
                return file.ID.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置属性之后直接保存
        /// </summary>
        public string SaveToFile(Guid? linkid = null)
        {
            FileSaveType = 0;
            return SaveToFile(PostFile, linkid);
        }

        /// <summary>
        /// 根据配置保存文件
        /// </summary>
        /// <param name="upfile"></param>
        public string SaveToFile(HttpPostedFileBase upfile, Guid? linkid = null)
        {
            FileSaveType = 0;
            string path = ConfigurationManager.AppSettings["FileFolder"];

            return SaveToFile(upfile, path, linkid);
        }

        /// <summary>
        /// 自定义保存文件
        /// </summary>
        /// <param name="upfile"></param>
        /// <param name="savepath"></param>
        public string SaveToFile(HttpPostedFileBase upfile, string savepath, Guid? linkid = null)
        {
            FileSaveType = 0;
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            var file = Save(upfile, linkid);
            if (file != null)
            {
                upfile.SaveAs(Path.Combine(savepath, file.FileSaveName));
                return file.ID.ToString();
            }
            return string.Empty;
        }


        protected FileDescription MapModel(HttpPostedFileBase upfile, Guid? linkid = null, int filestate = 0)
        {
            if (upfile == null)
            {
                return null;
            }
            FileDescription desc = new FileDescription();

            desc.FileName = upfile.FileName.Substring(upfile.FileName.LastIndexOf(@"\") + 1);
            desc.FileType = upfile.FileName.Substring(upfile.FileName.LastIndexOf("."));
            desc.FileSaveName = string.Format("{0}{1}", Guid.NewGuid(), desc.FileType);
            desc.FileCreateTime = DateTime.Now;
            desc.ID = Guid.NewGuid();
            desc.LinkID = linkid;
            desc.FileState = filestate;
            if (FileSaveType == 1)
            {
                var FileStorage = MapFileStorage(upfile, FileSaveType.ToString());

                desc.FileStorageID = FileStorage.ID;
            }


            return desc;
        }

        protected FileStorage MapFileStorage(HttpPostedFileBase file, string fileSaveType)
        {
            string hashCode = MD5HashCode(file);

            FileStorage fileStorage = Single(hashCode);

            if (fileStorage == null)
            {

                fileStorage = new FileStorage()
                {
                    ID = Guid.NewGuid(),
                    StoreType = fileSaveType,
                    MD5 = hashCode,
                    FileData = new FileExtend().GetFileByte(file.InputStream),
                    FileUrl = ""
                };

                FileContext.Set<FileStorage>().Add(fileStorage);

                FileContext.SaveChanges();

            }

            return fileStorage;
        }

        private string MD5HashCode(HttpPostedFileBase file)
        {
            if (file.InputStream.CanSeek)
            {
                file.InputStream.Seek(0, SeekOrigin.Begin);
            }

            System.Security.Cryptography.HashAlgorithm hashAlgorithm = System.Security.Cryptography.MD5.Create();

            byte[] hashCode = hashAlgorithm.ComputeHash(file.InputStream);

            hashAlgorithm.Dispose();

            return BitConverter.ToString(hashCode).Replace("-", "");
        }

        #endregion

        #region 读取
        public FileStorage Single(string md5)
        {
            return FileContext.Set<FileStorage>().FirstOrDefault(f => f.MD5 == md5);
        }

        public FileDescription Single(Guid id, bool hasdata = false)
        {
            return Single(new FileDescription { ID = id }, hasdata);
        }

        public FileDescription Single(Guid LinkID, string LinkKey, bool hasdata = false)
        {
            return Single(new FileDescription { LinkID = LinkID, LinkKey = LinkKey }, hasdata);
        }


        public FileDescription Single(FileDescription filedesc, bool hasdata = false)
        {
            if (filedesc.HasID)
            {
                var fileDescription = FileContext.Set<FileDescription>().FirstOrDefault(o => o.ID == filedesc.ID);
                if (fileDescription != null && fileDescription.FileStorageID != null && fileDescription.FileStorageID != Guid.Empty && hasdata)
                {
                    var filestorage = FileContext.Set<FileStorage>().FirstOrDefault(t => t.ID == fileDescription.FileStorageID);
                    fileDescription.FileData = filestorage.FileData;
                }

                return fileDescription;
            }
            else
            {
                var fileDescription = FileContext.Set<FileDescription>().FirstOrDefault(o => o.LinkID == filedesc.LinkID && o.LinkKey == filedesc.LinkKey);

                if (fileDescription != null && fileDescription.FileStorageID != null && fileDescription.FileStorageID != Guid.Empty && hasdata)
                {
                    var filestorage = FileContext.Set<FileStorage>().FirstOrDefault(t => t.ID == fileDescription.FileStorageID);
                    fileDescription.FileData = filestorage.FileData;
                }

                return fileDescription;
            }
        }

        public string GetFileName(string id, bool flag = false)
        {
            var file = Single(Guid.Parse(id));
            if (file != null)
            {
                if (flag)
                {
                    return file.FileName;
                }
                return file.FileName + "|" + file.FileState;
            }
            return "";
        }

        public IEnumerable<FileDescription> List(Guid LinkID)
        {
            return List(new FileDescription { LinkID = LinkID });
        }

        public IEnumerable<FileDescription> List(Guid LinkID, string linkKey)
        {
            return List(new FileDescription { LinkKey = linkKey, LinkID = LinkID });
        }

        public IEnumerable<FileDescription> List(FileDescription filedesc)
        {
            if (string.IsNullOrEmpty(filedesc.LinkKey))
            {
                return FileContext.Set<FileDescription>().Where(o => o.LinkID == filedesc.ID && o.LinkKey == filedesc.LinkKey);
            }
            else
            {
                return FileContext.Set<FileDescription>().Where(o => o.LinkID == filedesc.ID);
            }
        }

        #endregion
    }
}
