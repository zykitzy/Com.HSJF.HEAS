using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Other;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.File.FileInterface;
using static Com.HSJF.Framework.DAL.DictionaryType;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize]
    public class OtherController : BaseController
    {
        #region 字典表
        // GET: Other
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DictionaryIndex(string ParentKey)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            var list = ddal.GetAll().ToList().Select(t => new DictionaryListViewModel().CastModel(t));
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in list)
            {
                if (item.ParentKey == null)
                {
                    dictionary.Add(item.Key, item.Text);
                }
            }
            ViewBag.dictionary = dictionary;
            if (!string.IsNullOrEmpty(ParentKey))
            {
                list = list.Where(s => s.Path.Contains(ParentKey));
            }
            list = list.OrderByDescending(s => s.Desc);
            return View(list);
        }

        public ActionResult CreateDictionary()
        {
            DictionaryViewModel model = new DictionaryViewModel();
            model.State = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDictionary(DictionaryViewModel model)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            ddal.Add(model.CastDB(model));
            ddal.AcceptAllChange();
            return Content("Success");
        }

        public ActionResult EditDictionary(string id)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            var dic = new DictionaryViewModel().CastModel(ddal.Get(id));
            return View(dic);
        }

        [HttpPost]
        public ActionResult EditDictionary(DictionaryViewModel model)
        {
            DictionaryDAL ddal = new DictionaryDAL();

            ddal.Update(model.CastDB(model));
            ddal.AcceptAllChange();
            return Content("Success");
        }

        [HttpPost]
        public ActionResult DeleteDictionary(string id)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            var model = ddal.Get(id);
            ddal.Delete(model);
            ddal.AcceptAllChange();
            return Content("Success");
        }

        #endregion

        public ActionResult FindByType(string dictype)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            var modellist = ddal.FindByParentKey(dictype);
            return Json(modellist.ToString(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult FindByTypeList(IEnumerable<string> dictype)
        {
            DictionaryDAL ddal = new DictionaryDAL();
            var diclist = ddal.FindByParentList(dictype);
            return Json(diclist, JsonRequestBehavior.AllowGet);
        }


        #region 数据权限

        public ActionResult DatapermissionList(string roleid)
        {
            List<CheckBoxListModel> list = new List<CheckBoxListModel>();
            DistrictDAL dtdal = new DistrictDAL();
            SalesGroupDAL sgdal = new SalesGroupDAL();
            DictionaryDAL dicdal = new DictionaryDAL();
            DataPermissionDAL ddal = new DataPermissionDAL();
            var dpuer = ddal.GetAll().Where(t => t.RoleID == roleid).Select(t => t.DataPermissionID);
            var dataplist = dtdal.GetAll().Select(t => new { t.ID, t.Name })
                .Union(dicdal.GetAll(t => t.ParentKey == "-ThirdPlatform").Select(t => new { ID = t.Path, Name = t.Text }))
                .Union(sgdal.GetAll().Select(t => new { t.ID, t.Name }));

            foreach (var t in dataplist)
            {
                list.Add(new CheckBoxListModel(t.ID, t.Name, dpuer.Contains(t.ID)));
            }

            //为数据权限增加 自有资金
            list.Add(new CheckBoxListModel(CaseMode.ZiYouZiJin, "自有资金", dpuer.Contains(CaseMode.ZiYouZiJin)));
            //为数据权限增加 未选择
            list.Add(new CheckBoxListModel(CaseMode.WeiXuanZe, "未选择案件模式", dpuer.Contains(CaseMode.WeiXuanZe)));
            return View(list);

        }

        [HttpPost]
        public ActionResult SaveDataPermission(string roleid, string datapermission)
        {
            DataPermissionDAL ddal = new DataPermissionDAL();
            ddal.SaveDataPermission(roleid, datapermission);
            return ToJsonResult("Success");
        }
        #endregion


        #region 图片处理
        public FileResult FileDisplay(string id)
        {
            FileUpload filedal = new FileUpload();
            var file = filedal.Single(Guid.Parse(id), true);
            if (file != null)
            {
                if (string.IsNullOrEmpty(file.LinkKey))
                {
                    if (file.LinkID == Guid.Empty || file.LinkID == null)
                    {
                        return File(file.FileData, file.FileType);
                    }
                }
                else
                {
                    FileDisplayHelper helper = new FileDisplayHelper();
                    if (helper.CanViewFile(file.LinkKey, CurrentUser))
                    {
                        return File(file.FileData, file.FileType);
                    }
                }
            }
            return null;
        }
        #endregion


        #region 文件
        /// <summary>
        /// 文件模式切换初始化动作
        /// </summary>
        /// <returns>操作结果</returns>
        [HttpPost]
        public ActionResult FileSwitchInit()
        {
            try
            {
                FileUpload fileUpload = new FileUpload();
                using (var fileContext = new FileContext())
                {
                    var fileIds = fileContext.FileDescription.Select(p => p.ID);

                    fileIds.ToList().ForEach(id =>
                    {
                        var fd = fileContext.FileDescription.Single(p => p.ID == id);

                        // step 1 delete data which link id is Guid.Empty
                        if (fd.LinkID == Guid.Empty)
                        {
                            fileContext.Entry(fd).State = EntityState.Deleted;

                            fileContext.SaveChanges();
                        }
                        else
                        {
                            // step 2 switch file store mode

                            if (fd.FileData != null && fd.FileData.Any()) //fd.FileStorageID == Guid.Empty &&
                            {
                                string hashCode = MD5HashCode(fd.FileData);
                                var fileStorage = fileContext.FileStorage.FirstOrDefault(f => f.MD5 == hashCode);

                                if (fileStorage == null)
                                {
                                    fileStorage = new FileStorage
                                    {
                                        ID = Guid.NewGuid(),
                                        FileData = fd.FileData,
                                        StoreType = "1",
                                        FileUrl = "",
                                        MD5 = hashCode
                                    };

                                    fileContext.FileStorage.Add(fileStorage);

                                    fileContext.SaveChanges();
                                }

                                fd.FileStorageID = fileStorage.ID;

                                // 清除filedata
                                fd.FileData = new byte[0];

                                fileContext.FileDescription.Attach(fd);
                                var fileDescriptionEntry = fileContext.Entry(fd);
                                fileDescriptionEntry.State = EntityState.Modified;

                                fileContext.SaveChanges();
                            }
                        }

                    });
                }
                return Json(new
                {
                    Success = true,
                    Message = "成功"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        private string MD5HashCode(byte[] fileBytes)
        {

            HashAlgorithm hashAlgorithm = MD5.Create();

            byte[] hashCode = hashAlgorithm.ComputeHash(fileBytes);

            hashAlgorithm.Dispose();

            return BitConverter.ToString(hashCode).Replace("-", "");
        }
        #endregion
    }
}