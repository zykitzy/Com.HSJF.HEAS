using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Com.HSJF.Infrastructure.Crypto;
using Com.HSJF.Framework.DAL.Biz;
using System.Security.Principal;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Infrastructure.Identity.Manager;
using Com.HSJF.Infrastructure.Identity.Store;
using System.Threading.Tasks;
using Com.HSJF.HEAS.Web.Helper;

namespace Com.HSJF.HEAS.Web.HelpPage
{
    public partial class WebBrowser : System.Web.UI.Page
    {

        //   public static Infrastructure.LogExtend.LogManagerExtend logger = new Infrastructure.LogExtend.LogManagerExtend();
        Com.HSJF.Infrastructure.File.FileUpload filedal = new Com.HSJF.Infrastructure.File.FileUpload();
        Com.HSJF.Infrastructure.Identity.Model.User CurrentUser
        {
            get
            {
                return (Com.HSJF.Infrastructure.Identity.Model.User)System.Web.HttpContext.Current.Session["_currentUser"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser == null)
            {
                Response.Redirect("/Home/Failed");
            }
            FileDisplayHelper helper = new FileDisplayHelper();
            string _fileid = Request.QueryString["fileid"];
            string _fileType = Request.QueryString["type"] ?? "down";

            if (!string.IsNullOrEmpty(_fileid))
            {
                var file = filedal.Single(Guid.Parse(_fileid), true);
                if (file != null)
                {
                    if (string.IsNullOrEmpty(file.LinkKey))
                    {
                        if (file.LinkID == Guid.Empty || file.LinkID == null)
                        {
                            ViewFile(_fileType, file);
                        }
                    }
                    else
                    {
                        if (helper.CanViewFile(file.LinkKey, CurrentUser))
                        {
                            ViewFile(_fileType, file);
                        }
                    }
                }
                else
                {
                    Response.Redirect("/Home/Failed");
                }
            }
            else
            {
                Response.Redirect("/Home/Failed");
            }
        }

        private void ViewFile(string _fileType, Infrastructure.File.FileDescription file)
        {
            file.FileType = string.IsNullOrEmpty(file.FileType) ? string.Empty : file.FileType.ToLower().Trim();
            if (_fileType.ToLower() != "down" && (file.FileType == ".jpg" || file.FileType == ".gif" || file.FileType == ".jpeg" || file.FileType == ".png" || file.FileType == ".bmp"))
            {
                this.imgdisplay.Src = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/Other/FileDisplay/" + file.ID;
                Page.Header.Title = file.Description;
            }
            else
            {
                var suffixs = new string[] { ".jpg", ".gif", ".jpeg", ".png", ".bmp", ".pdf" };
                if (_fileType.ToLower() != "down" && !suffixs.Contains(file.FileType))
                {
                    this.imgdisplay.Src = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + "/img/can_not_view.bmp";
                    Page.Header.Title = file.Description;
                }
                else
                {
                    SetBaseReponse(file.FileData, file.FileType, file.FileName, file.FileName, _fileType.ToLower() == "down");
                }
            }
        }

        #region 辅助方法


        //设置并推送Response
        protected void SetBaseReponse(byte[] file, string contenttype, string filename, string title, bool isdownloadn = true)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = contenttype;
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.AddHeader("Content-Disposition", string.Format("{1};FileName={0}", filename, isdownloadn ? "attachment" : "inline"));
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();
        }

        #endregion


    }
}