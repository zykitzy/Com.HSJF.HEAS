using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.Framework.EntityFramework.Model.Others;
using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.AfterCase;
using Com.HSJF.Infrastructure;
using Com.HSJF.Infrastructure.Crypto;
using Com.HSJF.Infrastructure.ExtendTools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// 贷后案件信息
    /// </summary>
    public class AfterCaseController : ApiController
    {
        #region Fields

        /// <summary>
        /// 合同年利率
        /// </summary>
        private const string CONTRACT_INTEREST_RATE = "24.00";

        /// <summary>
        /// 收款方式
        /// </summary>
        private const string RECEIVE_TYPE = "-ReceiveType-Xiben";

        private readonly BaseAuditDAL _auditDal;

        private readonly SalesGroupDAL _salesGroupDal;

        private readonly RelationPersonAuditDAL _relationPersonAuditDal;

        private readonly LendingDAL _lendingDal;

        private readonly MortgageBll _mortgageBll;

        private readonly DictionaryBLL _dictionaryBll;

        private SymmCrypto symm;

        byte[] _Key = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
        byte[] _IV = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");


        #endregion

        #region Ctor

        public AfterCaseController()
        {
            _auditDal = new BaseAuditDAL();
            _salesGroupDal = new SalesGroupDAL();
            _relationPersonAuditDal = new RelationPersonAuditDAL();
            _lendingDal = new LendingDAL();
            _mortgageBll = new MortgageBll();
            _dictionaryBll = new DictionaryBLL();
            symm = new SymmCrypto(_Key, _IV);
        }

        #endregion

        #region Apis

        /// <summary>
        /// 根据案件日期查询案件信息
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <returns>案件信息</returns>
        [HttpPost]
        public BaseApiResponse<string> Post(GetAfterCaseByDateRequest request)
        {
            var response = new BaseApiResponse<string>();
            try
            {
                if (request.IsValid())
                {
                    DateTime beginData = DateTime.ParseExact(request.BeginDate.NullSafe(), "yyyy/MM/dd", null);
                    DateTime? endDate = null;
                    if (!string.IsNullOrEmpty(request.EndDate.NullSafe()))
                    {
                        endDate = DateTime.ParseExact(request.EndDate.NullSafe(), "yyyy/MM/dd", null);
                    }

                    var afterCases = this.GetAfterCaseByData(beginData, endDate);
                    response.Status = StatusEnum.Success.ToString();
                    response.Message = "Success";
                    response.Data = ConvertToString(symm.EncryptFromString(afterCases.ToJson(), Encoding.UTF8));
                }
                else
                {
                    response.Status = StatusEnum.Failed.ToString();
                    response.Message = request.GetErrorMessage();
                }
            }
            catch (Exception ex)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public string Post(string encryptStr)
        {
            byte[] encrptBytes = Convert.FromBase64String(encryptStr);

            string decryptStr = symm.DecryptToString(encrptBytes, Encoding.UTF8);

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AfterCase>>(decryptStr);
            return decryptStr;
        }

        #endregion

        #region Private Methods
        // todo switch to private method
        public IEnumerable<AfterCase> GetAfterCaseByData(DateTime startTime, DateTime? endTime)
        {
            IQueryable<BaseAudit> baseAudits = GetAllAfterCases().Where(p => p.CreateTime >= startTime);
            if (endTime.HasValue)
            {
                baseAudits = baseAudits.Where(p => p.CreateTime < endTime);
            }
            baseAudits = baseAudits.OrderBy(p => p.CreateTime);

            var afterCases = new List<AfterCase>();
            List<Dictionary> dictionaries = _dictionaryBll.QueryByParentKey("-ThirdPlatform").ToList();

            foreach (var audit in baseAudits)
            {
                // 获取借款人
                RelationPersonAudit customer = _relationPersonAuditDal.GetAll().Single(p => p.AuditID == audit.ID && p.RelationType == "-PersonType-JieKuanRen");

                Lending lendingInfo = audit.NewCaseNum.Contains("HIS") ? _lendingDal.GetHIS(audit.ID) : _lendingDal.Get(audit.ID);

                SalesGroup salesGroup = _salesGroupDal.GetAll().Single(p => p.ID == audit.SalesGroupID);

                PublicMortgage mortgage = _mortgageBll.Query(audit.NewCaseNum);

                var fundDictionary = dictionaries.FirstOrDefault(p => p.Path == audit.ThirdParty);


                // 借款人和共同借款人
                var debtNames =
                    _relationPersonAuditDal.GetAll()
                        .Where(
                            p =>
                                p.AuditID == audit.ID &&
                                (p.RelationType == "-PersonType-JieKuanRen" || p.IsCoBorrower == 1)).Select(p => p.Name);

                var afterase = new AfterCase();

                afterase.CaseNum = audit.NewCaseNum;
                afterase.SalesGrouptId = salesGroup.CompanyCode;
                afterase.SalesCompanyName = salesGroup.Company;
                afterase.CompanyCode = salesGroup.Company.PadRight(4).Substring(0, 4);
                afterase.CustomerId = customer.ID;
                afterase.CustomerName = audit.BorrowerName;
                afterase.CustomerIdentificationType = customer.IdentificationType;
                afterase.CustomerIdentificationNumber = customer.IdentificationNumber;
                afterase.AccountName = audit.OpeningSite ?? string.Empty;
                afterase.EarnestMoney = audit.CustEarnestMoney.ToString();
                afterase.DebtName = string.Join(",", debtNames.ToArray());
                afterase.LenderName = audit.LenderName ?? string.Empty;

                afterase.LoanTerm = audit.AuditTerm ?? string.Empty;
                afterase.LendingDate = lendingInfo.LendTime == null ? "" : lendingInfo.LendTime.ToString();
                afterase.PaymentDay = lendingInfo.PaymentDay == null ? "" : lendingInfo.PaymentDay.ToString();
                afterase.ContractAmount = audit.NewCaseNum.Contains("HIS") ? lendingInfo.ContractAmount.ToString() : mortgage.ContractAmount.ToString();
                afterase.ContractInterestRate = CONTRACT_INTEREST_RATE;
                afterase.RealInterestRate = audit.AuditRate > 2M ? audit.AuditRate.ToString() : (audit.AuditRate * 12).ToString(); // 审计利率*12=实际年利率

                afterase.ReceiveType = RECEIVE_TYPE;
                afterase.FundId = audit.ThirdParty ?? string.Empty;
                afterase.FundInterestRate = GetThirdPartyRate(audit);
                afterase.FundName = fundDictionary == null ? string.Empty : fundDictionary.Text;
                // 债转资金当作自有资金处理
                afterase.LoadType = audit.CaseMode == DictionaryType.CaseMode.ZhaiZhuan ? DictionaryType.CaseMode.ZiYouZiJin : audit.CaseMode;
                afterase.VirtualAccount = "";
                afterase.ContractNum = mortgage.ContractNo;

                afterCases.Add(afterase);

            }

            return afterCases;
        }

        /// <summary>
        /// 获取所有贷后案件
        /// </summary>
        /// <returns></returns>
        private IQueryable<BaseAudit> GetAllAfterCases()
        {
            return _auditDal.GetAll().Where(p => p.CaseStatus == CaseStatus.AfterCase);
        }

        /// <summary>
        /// 获取第三方资金年利率
        /// </summary>
        /// <param name="baseAudit">案件信息</param>
        /// <returns>年利率</returns>
        private string GetThirdPartyRate(BaseAudit baseAudit)
        {
            //string rate;
            if (baseAudit.ThirdParty != null)
            {
                return baseAudit.ThirdPartyAuditRate.ToString();
            }
            else
            {
                return string.Empty;
            }

            //switch (baseAudit.ThirdParty)
            //{
            //    case "-ThirdPlatform-DianRong":
            //        rate = baseAudit.ThirdPartyAuditRate.ToString();
            //        break;
            //    case "-ThirdPlatform-JuCaiMao":
            //        rate = ConfigurationManager.AppSettings["-ThirdPlatform-JuCaiMao"];
            //        break;
            //    case "-ThirdPlatform-MiGang":
            //        rate = ConfigurationManager.AppSettings["-ThirdPlatform-MiGang"];
            //        break;
            //    case "-ThirdPlatform-WaCai":
            //        rate = ConfigurationManager.AppSettings["-ThirdPlatform-WaCai"];
            //        break;
            //    default:
            //        rate = string.Empty;
            //        break;
            //}

            //return rate;
        }

        private string ConvertToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        #endregion
    }
}
