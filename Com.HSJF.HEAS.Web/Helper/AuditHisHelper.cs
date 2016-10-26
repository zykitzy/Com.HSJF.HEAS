using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.HSJF.HEAS.Web.Helper
{
    public class AuditHisHelper
    {
        public IEnumerable<AuditHistory> GetHistory(IQueryable<BaseAudit> list)
        {
            if (list == null || !list.Any())
            {
                return null;
            }
            IList<AuditHistory> hislist = new List<AuditHistory>();
            BaseCaseDAL bcd = new BaseCaseDAL();
            Framework.DAL.UserDAL user = new Framework.DAL.UserDAL();
            var lis = list.OrderByDescending(s => s.Version).First();
            foreach (var model in list)
            {
                var his = new AuditHistory();
                if (model.Equals(lis))
                {
                    his.CreatUser = user.GetDisplayName(model.CreateUser);
                    his.CreateTime = null;
                    his.CaseStatusTest = CaseStatusHelper.GetStatsText(model.CaseStatus);
                    his.Action = ActionTest(model, list);
                    hislist.Add(his);
                }
                else
                {
                    his.CreatUser = user.GetDisplayName(model.CreateUser);
                    his.CreateTime = GetCommitTime(model, list);
                    his.CaseStatusTest = CaseStatusHelper.GetStatsText(model.CaseStatus);
                    his.Description = "";
                    //if (model.RejectType != null)
                    //{
                    //    string RejectType = "";
                    //    string[] str = model.RejectType.Split(',');
                    //    DictionaryDAL dadal = new DictionaryDAL();
                    //    foreach (var item in str)
                    //    {
                    //        RejectType += dadal.GetText(item) + "，";
                    //    }
                    //    RejectType = RejectType.Substring(0, RejectType.Length - 1);
                    //    if (!string.IsNullOrEmpty(model.Description))
                    //    {
                    //        his.Description = RejectType + "<br/>" + model.Description.Replace("\n", "<br/>");
                    //    }
                    //    else
                    //    {
                    //        his.Description = RejectType;
                    //    }

                    //}
                    //else
                    //{
                    //if (!string.IsNullOrEmpty(model.RejectReason))
                    //{
                    //    his.Description += model.RejectReason.Replace("\n", "<br/>");
                    //}
                    if (!string.IsNullOrEmpty(model.Description))
                    {
                        his.Description += model.Description.Replace("\n", "<br/>");
                    }
                    else
                    {
                        his.Description += "";
                    }
                    //}
                    his.Action = ActionTest(model, list);
                    hislist.Add(his);
                }
            }

            var newcasenum = list.FirstOrDefault().NewCaseNum;
            var basecase = bcd.GetAll(t => t.NewCaseNum == newcasenum).FirstOrDefault();

            hislist.Add(new AuditHistory()
            {
                CreatUser = user.GetDisplayName(basecase.CreateUser),
                CreateTime = basecase.CreateTime,
                CaseStatusTest = "进件",
                Description = "",
                Action = "提交",
            });

            return hislist;
        }

        public string ActionTest(BaseAudit curraudit, IEnumerable<BaseAudit> list)
        {
            var nextaudit = list.FirstOrDefault(t => t.Version == curraudit.Version + 1);
            var lastaudit = list.FirstOrDefault(t => t.Version == curraudit.Version - 1);
            if (nextaudit == null)
            {
                return string.Empty;
            }
            if (nextaudit.CaseStatus == CaseStatus.CloseCase)
            {
                return "拒绝";
            }
            else if (nextaudit.CaseStatus == CaseStatus.ClosePublic)
            {
                return "关闭";
            }
            else if (lastaudit != null && (lastaudit.CaseStatus == nextaudit.CaseStatus) && nextaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.FirstAudit && lastaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.FirstAudit)
            {
                return "退回";
            }
            else if (lastaudit != null && lastaudit.CaseStatus == CaseStatus.SecondAudit && nextaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.FirstAudit && curraudit.CaseStatus == CaseStatus.HatsPending)
            {
                return "退回";
            }
            else if (lastaudit != null && (lastaudit.CaseStatus == nextaudit.CaseStatus) && nextaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.SecondAudit && lastaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.SecondAudit)
            {
                return "通过";
            }
            else if (lastaudit != null && (lastaudit.CaseStatus == nextaudit.CaseStatus) && nextaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.PublicMortgage && lastaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.PublicMortgage)
            {
                return "退回";
            }
            else
            {
                return "通过";
            }
        }

        private DateTime? GetCommitTime(BaseAudit curraudit, IEnumerable<BaseAudit> list)
        {
            var nextaudit = list.Where(t => t.Version == curraudit.Version + 1);
            if (nextaudit.Any())
            {
                return nextaudit.FirstOrDefault().CreateTime;
            }
            return null;
        }
    }
}
