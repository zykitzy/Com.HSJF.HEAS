using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.HEAS.BLL.DataStatistics.Dto;
using Com.HSJF.Infrastructure;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.LogExtend;
using Com.HSJF.Infrastructure.Utility;

namespace Com.HSJF.HEAS.BLL.DataStatistics
{

    /// <summary>
    /// 每日数据统计
    /// </summary>
    public class DayStatisticsBll
    {
        #region Fields

        private readonly BaseAuditDAL _baseAuditDal;
        private readonly SalesGroupDAL _salesGroupDal;
        private readonly MortgageDAL _mortgageDal;
        private readonly LogManagerExtend _log;
        private readonly BaseCaseDAL _baseCaseDal;
        private readonly LendingDAL _lendingDal;

        private string receviers = ConfigurationManager.AppSettings["receivers"];
        private string emailToken = ConfigurationManager.AppSettings["emailToken"];
        private string emailTemplateHeader = ConfigurationManager.AppSettings["headerTplPath"];
        private string emailTemplateBody = ConfigurationManager.AppSettings["bodyTplPath"];
        private string subject = ConfigurationManager.AppSettings["subject"];
        private string emailHost = ConfigurationManager.AppSettings["emailHost"];
        private string sender = ConfigurationManager.AppSettings["sender"];
        private string syscode = ConfigurationManager.AppSettings["syscode"];


        #endregion

        #region  Ctor

        public DayStatisticsBll()
        {
            _baseAuditDal = new BaseAuditDAL();
            _salesGroupDal = new SalesGroupDAL();
            _mortgageDal = new MortgageDAL();
            _log = new LogManagerExtend();
            _baseCaseDal = new BaseCaseDAL();
            _lendingDal = new LendingDAL();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 发送每日邮件
        /// </summary>
        public void SendEmail()
        {
            try
            {
                // 统计结果
                var result = GetDayStatisticsV2(DateTime.Now);

                string emailBody = string.Empty;
                string emailContent = string.Empty;

                string headerTpl = ReadTemplate(emailTemplateHeader);
                string bodyTpl = ReadTemplate(emailTemplateBody);

                result.DayStatistics.ForEach(p =>
                {
                    emailBody += string.Format(bodyTpl,
                        p.SalesGroupName.PadLeft(4, ' ').Substring(0, 4),
                        p.BaseCaseAmount, p.BaseCaseCount,
                        p.AuditCaseAmount, p.AuditCaseCount,
                        p.PublicCaseAmount, p.PublicCaseCount,
                        p.AfterCaseAmount, p.AfterCaseCount,
                        p.MonthAfterCaseAmount, p.MonthAfterCaseCount);
                });

                emailContent = string.Format(headerTpl,
                    result.StatisticsDate.ToString("yyyy-MM-dd"),
                    emailBody,
                    result.DayStatistics.Sum(p => p.MonthAfterCaseAmount),
                    result.DayStatistics.Sum(p => p.MonthAfterCaseCount));

                // 请求参数
                EmailRequest request = new EmailRequest();
                request.Recipient = receviers;
                request.Sender = sender;
                request.Subject = subject;
                request.Content = emailContent;
                request.IsHtml = 1;
                request.Token = emailToken;
                request.SYSCode = syscode;

                HttpItem httpItem = new HttpItem
                {
                    URL = string.Format("http://{0}/api/MessageService", emailHost),
                    Method = "post",
                    ContentType = "application/json;charset=utf-8",
                    Postdata = request.ToJson(),
                    Accept = "text/json",
                    PostEncoding = Encoding.UTF8

                };
                var httpResult = new HttpHelper().GetHtml(httpItem);

                _log.WriteInfo(emailContent);
                _log.WriteInfo(httpResult.ToJson());

            }
            catch (Exception ex)
            {
                _log.WriteException("发送每日案件报表出错", ex);

            }
        }

        /// <summary>
        /// 按日查询汇总数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>日统计数据</returns>
        public GetDayStatisticsOutput GetDayStatistics(DateTime date)
        {
            GetDayStatisticsOutput output = new GetDayStatisticsOutput();

            output.StatisticsDate = new DateTime(date.Year, date.Month, date.Day);

            var salesGroups = _salesGroupDal.GetAll().Where(p => p.State == 1);

            salesGroups.ForEach(p => output.DayStatistics.Add(GetSingleGroupStatistics(p, output.StatisticsDate)));

            return output;
        }

        public GetDayStatisticsOutput GetDayStatisticsV2(DateTime date)
        {
            var output = GetDayStatistics(date);

            return MonetaryUnitChange(output);

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取指定日期的各个状态案件的统计信息
        /// </summary>
        /// <param name="salesGroup">销售团队信息</param>
        /// <param name="date">日期</param>
        /// <returns>统计信息</returns>
        private DayStatisticsDto GetSingleGroupStatistics(SalesGroup salesGroup, DateTime date)
        {
            var output = new DayStatisticsDto();
            DateTime endDate = date.AddDays(1.0);
            DateTime thisMonth = new DateTime(date.Year, date.Month, 1);
            DateTime nextMonth = thisMonth.AddMonths(1);

            // 审核案件
            var audits = _baseAuditDal.GetAllBase()
                .Where(p => p.SalesGroupID == salesGroup.ID && p.CreateTime >= date && p.CreateTime < endDate)
                .ToList();

            var monthAudits = _baseAuditDal.GetAllBase()
                .Where(p => p.SalesGroupID == salesGroup.ID && p.CreateTime >= thisMonth && p.CreateTime < nextMonth);

            // 进件案件
            var baseCases = _baseCaseDal.GetAll()
                .Where(p => p.SalesGroupID == salesGroup.ID && p.CreateTime > date && p.CreateTime < endDate && p.NewCaseNum != null);

            // 已签约案件
            var mortages = GetMortgages(audits).ToList();

            // 已放款案件
            var afterCases = GetLendings(audits).ToList();

            // 月放款案件
            var monthAfterCases = GetLendings(monthAudits);

            output.SalesGroupId = salesGroup.ID;
            output.SalesGroupName = salesGroup.Company;

            output.BaseCaseCount = baseCases.Count();
            output.BaseCaseAmount = baseCases.Sum(p => p.LoanAmount) ?? 0M;

            output.AuditCaseCount = audits.Count(p => p.CaseStatus == CaseStatus.PublicMortgage);
            output.AuditCaseAmount = audits.Where(p => p.CaseStatus == CaseStatus.PublicMortgage).Sum(p => p.AuditAmount) ?? 0M;

            output.PublicCaseCount = mortages.Count();
            output.PublicCaseAmount = mortages.Sum(p => p.ContractAmount) ?? 0M;

            output.AfterCaseCount = afterCases.Count();
            output.AfterCaseAmount = afterCases.Sum(p => p.ContractAmount) ?? 0M;

            output.MonthAfterCaseAmount = monthAfterCases.Sum(p => p.ContractAmount) ?? 0M;
            output.MonthAfterCaseCount = monthAfterCases.Count();

            return output;
        }

        /// <summary>
        /// 获取签约合同
        /// </summary>
        /// <param name="audits">审计案件</param>
        /// <returns>签约合同</returns>
        private IEnumerable<PublicMortgage> GetMortgages(IEnumerable<BaseAudit> audits)
        {
            var currenStateCase = audits.Where(p => p.CaseStatus == CaseStatus.PublicMortgage);
            string[] currentCaseNums = currenStateCase.Select(p => p.NewCaseNum).ToArray();
            var publicCases =
                audits.Where(p => p.CaseStatus == CaseStatus.PublicMortgage && currentCaseNums.Contains(p.NewCaseNum));
            string[] publicCaseIds = publicCases.Select(p => p.ID).ToArray();

            return _mortgageDal.GetAll().Where(p => publicCaseIds.Contains(p.ID));
        }

        /// <summary>
        /// 获取放款信息
        /// </summary>
        /// <param name="audits">审计案件</param>
        /// <returns>放款信息</returns>
        private IEnumerable<Framework.EntityFramework.Model.Lending.Lending> GetLendings(IEnumerable<BaseAudit> audits)
        {
            var lengdingCases = audits.Where(p => p.CaseStatus == CaseStatus.Lending);
            string[] caseNums = lengdingCases.Select(p => p.NewCaseNum).ToArray();

            var afterCases = audits.Where(p => p.CaseStatus == CaseStatus.Lending && caseNums.Contains(p.NewCaseNum));

            string[] afterCaseIds = afterCases.Select(p => p.ID).ToArray();

            return _lendingDal.GetAll().Where(p => afterCaseIds.Contains(p.ID));
        }

        /// <summary>
        /// 读取文件模板
        /// </summary>
        /// <param name="path">文件模板路径</param>
        /// <returns>模板内容</returns>
        private string ReadTemplate(string path)
        {
            string result = string.Empty;
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
            {

                //string str = reader.ReadLine();
                //while ((str = reader.ReadLine()) != null)
                //{
                //    Debug.WriteLine(str);
                //}

                result = reader.ReadToEnd();
                Debug.WriteLine(result);
            }

            //byte[] fileBytes = ReadBytes(fileStream);

            //string template = Encoding.UTF8.GetString(fileBytes);
            //return template;
            return result;
        }

        /// <summary>
        /// 价格单位从元换成万元
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>单位为万元的结果</returns>
        private GetDayStatisticsOutput MonetaryUnitChange(GetDayStatisticsOutput input)
        {
            input.DayStatistics.ForEach(p =>
            {
                p.AfterCaseAmount = Math.Ceiling(p.AfterCaseAmount / 10000);
                p.AuditCaseAmount = Math.Ceiling(p.AuditCaseAmount / 10000);
                p.BaseCaseAmount = Math.Ceiling(p.BaseCaseAmount / 10000);
                p.PublicCaseAmount = Math.Ceiling(p.PublicCaseAmount / 10000);
                p.MonthAfterCaseAmount = Math.Ceiling(p.MonthAfterCaseAmount / 10000);
            });


            return input;
        }

        #endregion

    }
}
