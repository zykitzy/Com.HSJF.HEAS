using System.Collections.Generic;
using Com.HSJF.Framework.DAL;

namespace Com.HSJF.HEAS.Web.Helper
{
    public static class CaseStatusHelper
    {
        /// <summary>
        /// 获取案件状态含义
        /// </summary>
        /// <param name="status">案件状态</param>
        /// <returns>案件状态含义</returns>
        public static string GetStatsText(string status)
        {
            switch (status)
            {
                case CaseStatus.FirstAudit:
                    {
                        return "初审";
                    }
                case CaseStatus.SecondAudit:
                    {
                        return "复审";
                    }
                case CaseStatus.CloseCase:
                    {
                        return "审核拒绝";
                    }
                case CaseStatus.PublicMortgage:
                    {
                        return "签约";
                    }
                case CaseStatus.ConfrimPublic:
                    {
                        return "确认签约要件";
                    }
                case CaseStatus.ClosePublic:
                    {
                        return "签约失败";
                    }
                //case CaseStatus.WaitingLending:
                //    {
                //        return " 等待确认放款";
                //    }
                case CaseStatus.Lending:
                    {
                        return "放款";
                    }
                case CaseStatus.AfterCase:
                    {
                        return "贷后";
                    }
                case CaseStatus.SpecialClose:
                    {
                        return "未放款结束";
                    }
                case CaseStatus.FinishCase:
                    {
                        return "还清";
                    }
                case CaseStatus.HatsPending:
                    {
                        return "等待确认案件模式";
                    }

                default: return "未知状态";
            }
        }

        /// <summary>
        /// 获取案件状态含义
        /// </summary>
        /// <param name="status">案件状态</param>
        /// <returns>案件状态含义</returns>
        public static string GetBigStatusText(string status)
        {
            switch (status)
            {
                case CaseStatus.FirstAudit:
                    {
                        return "审核";
                    }
                case CaseStatus.SecondAudit:
                    {
                        return "审核";
                    }
                case CaseStatus.CloseCase:
                    {
                        return "审核拒绝";
                    }
                case CaseStatus.PublicMortgage:
                    {
                        return "签约";
                    }
                case CaseStatus.ConfrimPublic:
                    {
                        return "确认签约要件";
                    }
                case CaseStatus.ClosePublic:
                    {
                        return "签约失败";
                    }
                //case CaseStatus.WaitingLending:
                //    {
                //        return " 等待确认放款";
                //    }
                case CaseStatus.Lending:
                    {
                        return "放款";
                    }
                case CaseStatus.AfterCase:
                    {
                        return "贷后";
                    }
                case CaseStatus.SpecialClose:
                    {
                        return "未放款结束";
                    }
                case CaseStatus.FinishCase:
                    {
                        return "还清";
                    }
                case CaseStatus.HatsPending:
                    {
                        return "等待确认案件模式";
                    }

                default: return "未知状态";
            }
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> GetBizStatus()
        {
            List<System.Web.Mvc.SelectListItem> stList = new List<System.Web.Mvc.SelectListItem>();

            stList.Add(new System.Web.Mvc.SelectListItem() { Text = "", Value = "全部", });
            stList.Add(new System.Web.Mvc.SelectListItem() { Text = "0", Value = "未提交", });
            stList.Add(new System.Web.Mvc.SelectListItem() { Text = "1", Value = "已提交", });
            return stList;
        }



    }
}
