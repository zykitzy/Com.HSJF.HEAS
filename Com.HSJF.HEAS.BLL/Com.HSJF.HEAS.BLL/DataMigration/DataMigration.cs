using System;
using System.Data.Entity;
using System.Linq;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Infrastructure;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using System.Collections.Generic;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Infrastructure.File;

namespace Com.HSJF.HEAS.BLL.DataMigration
{
    public class DataMigration
    {
        private BaseAuditDAL _baseAuditDal;
        private BaseCaseDAL _baseCaseDal;
        private MortgageDAL _mortgageDal;

        public DataMigration()
        {
            _baseCaseDal = new BaseCaseDAL();
            _baseAuditDal = new BaseAuditDAL();
            _mortgageDal = new MortgageDAL();

        }

        /// <summary>
        /// 更新案件号迁移数据
        /// </summary>
        public void UpdateCaseNum()
        {
            this.UpdateAuditCaseNum(new BaseAuditDAL(), new SalesGroupDAL());
            this.UpdateCaseNum(new BaseCaseDAL(), new SalesGroupDAL());
        }

        /// <summary>
        /// 年利率变更迁移
        /// </summary>
        public void TransferAnnualRate()
        {
            // step 1 年利率重新计算
            var baseAuditCases = _baseAuditDal.GetAllBase().Where(p => p.AuditRate != null || p.AnnualRate != null);
            baseAuditCases.ForEach(p =>
            {
                if (p.AuditRate != null && p.AuditRate < 2M)
                {
                    p.AuditRate = p.AuditRate * 12;
                }
                if (p.AnnualRate != null && p.AnnualRate < 2M)
                {
                    p.AnnualRate = p.AnnualRate * 12;
                }

            });
            _baseAuditDal.UpdateRange(baseAuditCases);
            _baseAuditDal.AcceptAllChange();
        }

        /// <summary>
        /// 审核期限变更
        /// 历史数据中审核期限如果没有填写，使用申请期限值
        /// </summary>
        public void TransferAuditTerm()
        {
            var baseAuditCases = _baseAuditDal.GetAllBase().Where(p => p.AuditRate == null);
            baseAuditCases.ForEach(p =>
            {
                if (p.Term != null)
                {
                    p.AuditTerm = p.Term;
                }
            });

            _baseAuditDal.UpdateRange(baseAuditCases);
            _baseAuditDal.AcceptAllChange();
        }

        /// <summary>
        /// 签约-承诺书/联系方式确认书/借条/收据 合并为 四条
        /// jira(HEAS-225)
        /// </summary>
        public void MortgageFileMerge()
        {
            var mortages = _mortgageDal.GetAll().AsNoTracking().ToList();
            mortages.ForEach(m =>
            {
                m.BaseAudit = null;

                m.NoteFile = null;

                if (m.NoteFile.IsNotNullOrEmpty() && m.NoteFile.IsNotNullOrWhiteSpace())
                {
                    m.FourFile = FileJoin(m.FourFile, m.NoteFile);
                }
                if (m.ReceiptFile.IsNotNullOrEmpty() && m.ReceiptFile.IsNotNullOrWhiteSpace())
                {
                    m.FourFile = FileJoin(m.FourFile, m.ContactConfirmFile);
                }
                if (m.UndertakingFile.IsNotNullOrEmpty() && m.UndertakingFile.IsNotNullOrWhiteSpace())
                {
                    m.FourFile = FileJoin(m.FourFile, m.UndertakingFile);
                }
                if (m.ContactConfirmFile.IsNotNullOrEmpty() && m.ContactConfirmFile.IsNotNullOrWhiteSpace())
                {
                    m.FourFile = FileJoin(m.FourFile, m.ReceiptFile);
                }
            });

            _mortgageDal.UpdateRange(mortages);
            _mortgageDal.AcceptAllChange();
        }

        public void InsertHisCase()
        {
            var migDAL = new MigTDAL();
            var personDAL = new RelationPersonAuditDAL();
            var salesGroupDAL = new SalesGroupDAL();
            var miglist = migDAL.GetAll();
            if (miglist.Any())
            {
                var salesGroups = salesGroupDAL.GetAll().ToList();

                int i = 1;
                foreach (var mig in miglist)
                {
                    var audit = new BaseAudit();
                    audit.ID = Guid.NewGuid().ToString();
                    audit.CaseNum = i.ToString("d6");

                    audit.BorrowerName = mig.CO2;
                    audit.BankCard = mig.CO30;
                    // 所属分公司名称 新的案件编号
                    switch (mig.CO4)
                    {
                        case "北京泓申":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "北京泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "北京泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "bff3003c-bfcb-4c34-898d-c9799db56e68";

                            break;
                        case "诚驰":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海诚驰泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海诚驰泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "0d17d335-6c79-4130-82d5-16173c142911";

                            break;
                        case "上海泓申":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "34965801-2ba7-4a80-a40b-7aff4f5c36a7";
                            break;
                        case "铉宇":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海铉宇金融信息服务有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海铉宇金融信息服务有限公司").DistrictID;
                            audit.SalesID = "f68acef9-b478-4ab0-ac43-ccaf6bf77988";
                            break;
                        case "银携":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海银携投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海银携投资管理有限公司").DistrictID;
                            audit.SalesID = "028bcc6f-dc7d-4994-9f2e-196b5cf974ad";

                            break;
                        default:
                            audit.NewCaseNum = "HIS" + "-" + audit.CaseNum;
                            break;
                    }
                    audit.EarnestMoney = string.IsNullOrEmpty(mig.CO7) ? (decimal?)null : Convert.ToDecimal(mig.CO7);
                    audit.LenderName = mig.CO8;
                    audit.NewCaseNum = "HIS" + "-" + audit.CaseNum;
                    // 客户贷款期数
                    switch (mig.CO9)
                    {
                        case "1": audit.Term = "-LoanTerm-1M"; break;
                        case "2": audit.Term = "-LoanTerm-2M"; break;
                        case "3": audit.Term = "-LoanTerm-3M"; break;
                        case "4": audit.Term = "-LoanTerm-4M"; break;
                        case "5": audit.Term = "-LoanTerm-5M"; break;
                        case "6": audit.Term = "-LoanTerm-6M"; break;
                        case "12": audit.Term = "-LoanTerm-12M"; break;
                        case "24": audit.Term = "-LoanTerm-24M"; break;
                        default:
                            audit.Term = "-LoanTerm-2M"; break;
                            break;
                    }
                    audit.AuditTerm = audit.Term;

                    audit.LendingDate = string.IsNullOrEmpty(mig.CO10) ? default(DateTime?) : Convert.ToDateTime(mig.CO10);
                    audit.PaymentDate = string.IsNullOrEmpty(mig.CO11) ? default(DateTime?) : Convert.ToDateTime(mig.CO11);
                    audit.LoanAmount = string.IsNullOrEmpty(mig.CO13) ? default(decimal?) : Convert.ToDecimal(mig.CO13);
                    audit.AuditAmount = audit.LoanAmount;

                    audit.AnnualRate = string.IsNullOrEmpty(mig.CO14) ? null : (decimal?)Convert.ToDecimal(mig.CO14);
                    audit.AuditRate = audit.AnnualRate;

                    // 资金方名称
                    switch (mig.CO18)
                    {
                        case "点融": audit.ThirdParty = "-ThirdPlatform-DianRong"; break;
                        case "聚财猫": audit.ThirdParty = "-ThirdPlatform-JuCaiMao"; break;
                        case "米缸": audit.ThirdParty = "-ThirdPlatform-MiGang"; break;
                        case "诺亚": audit.ThirdParty = "-ThirdPlatform-NuoYa"; break;
                        case "挖财": audit.ThirdParty = "-ThirdPlatform-WaCai"; break;
                        default:
                            break;
                    }
                    audit.ThirdPartyAuditRate = string.IsNullOrEmpty(mig.CO19) ? null : (decimal?)Convert.ToDecimal(mig.CO19);
                    audit.ThirdPartyAuditAmount = string.IsNullOrEmpty(mig.CO28) ? null : (decimal?)Convert.ToDecimal(mig.CO28);
                    switch (mig.CO27)
                    {
                        case "1": audit.ThirdPartyAuditTerm = "-LoanTerm-1M"; break;
                        case "2": audit.ThirdPartyAuditTerm = "-LoanTerm-2M"; break;
                        case "3": audit.ThirdPartyAuditTerm = "-LoanTerm-3M"; break;
                        case "4": audit.ThirdPartyAuditTerm = "-LoanTerm-4M"; break;
                        case "5": audit.ThirdPartyAuditTerm = "-LoanTerm-5M"; break;
                        case "6": audit.ThirdPartyAuditTerm = "-LoanTerm-6M"; break;
                        case "12": audit.ThirdPartyAuditTerm = "-LoanTerm-12M"; break;
                        case "24": audit.ThirdPartyAuditTerm = "-LoanTerm-24M"; break;
                        default:
                            break;
                    }
                    // 贷款模式
                    switch (mig.CO20)
                    {
                        case "居间": audit.CaseMode = "-CaseMode-JuJian"; break;
                        case "债转": audit.CaseMode = "-CaseMode-ZhaiZhuan"; break;
                        default: audit.CaseMode = "-CaseMode-ZiYouZiJin"; break;
                    }

                    audit.OpeningBank = mig.CO23;
                    audit.BankCard = mig.CO26;
                    audit.CaseDetail = mig.CO30;
                    audit.CreateTime = DateTime.Now;
                    audit.CreateUser = "System";
                    audit.CaseStatus = "After";



                    var person = new RelationPersonAudit();
                    person.ID = Guid.NewGuid().ToString();
                    person.AuditID = audit.ID;
                    person.Name = mig.CO2;
                    person.RelationType = "-PersonType-JieKuanRen";
                    switch (mig.CO5)
                    {
                        case "香港居民身份证":
                            person.IdentificationType = "-DocType-HongKong";
                            break;
                        case "身份证":
                            person.IdentificationType = "-DocType-IDCard";
                            break;
                        case "台湾同胞来往内地通行证":
                            person.IdentificationType = "-DocType-Taiwan";
                            break;
                        default:
                            person.IdentificationType = "-DocType-Passport";
                            break;
                    }


                    person.IdentificationNumber = mig.CO6;

                    personDAL.Add(person);

                    var lend = new Com.HSJF.Framework.EntityFramework.Model.Lending.Lending();
                    var lenddal = new LendingDAL();
                    lend.ID = audit.ID;
                    lend.Borrower = audit.BorrowerName;
                    //lend.ContactNumber = mig.CO0;
                    lend.ContractAmount = audit.AuditAmount;
                    lend.CreateTime = DateTime.Now.ToShortDateString();
                    lend.CreateUser = "System";
                    lend.CustomerName = audit.BorrowerName;
                    lend.LendTime = audit.LendingDate;
                    lend.PaymentDay = string.IsNullOrEmpty(mig.CO12) ? default(int?) : Convert.ToInt32(mig.CO12);
                    lenddal.Add(lend);

                    _baseAuditDal.Add(audit);
                    mig.Status = audit.NewCaseNum;
                    migDAL.Update(mig);
                    i++;
                }
            }
            _baseAuditDal.AcceptAllChange();
            migDAL.AcceptAllChange();
        }

        private string FileJoin(string file1, string file2)
        {
            if (file1.IsNullOrWhiteSpace() || file1.IsNullOrEmpty())
            {
                return file2;
            }
            if (file2.IsNullOrEmpty() || file2.IsNullOrWhiteSpace())
            {
                return file1;
            }

            return String.Join(",", file2, file1);
        }

        private void UpdateCaseNum(BaseCaseDAL baseCaseDal, SalesGroupDAL salesGroupDal)
        {
            // step 1 update NewCaseNum

            string[] caseNum = baseCaseDal.GetAll().Where(p => p.CaseNum != null).Select(p => p.CaseNum).Distinct().ToArray();
            caseNum.ForEach(p =>
            {
                var sameCaseNumCases = baseCaseDal.GetAll().Where(c => c.CaseNum == p).ToList();
                var salesGroupId = sameCaseNumCases.First().SalesGroupID;
                var salesGroup = salesGroupDal.GetAll().First(t => t.ID == salesGroupId);
                sameCaseNumCases.ForEach(t =>
                {
                    t.CaseNum = (Convert.ToInt32(t.CaseNum.Substring(4)) + 100000).ToString();
                    t.NewCaseNum = "L" + salesGroup.ShortCode + "-" + t.CaseNum;

                });

                baseCaseDal.UpdateRange(sameCaseNumCases);
                baseCaseDal.AcceptAllChange();
            });


            // step 2 update CaseNum
        }

        private void UpdateAuditCaseNum(BaseAuditDAL baseAuditDal, SalesGroupDAL salesGroupDal)
        {
            string[] caseNum = baseAuditDal.GetAll().Where(p => p.CaseNum != null).Select(p => p.CaseNum).Distinct().ToArray();
            caseNum.ForEach(p =>
            {
                var sameCaseNumCases = baseAuditDal.GetAllBase().Where(c => c.CaseNum == p).ToList();
                var salesGroupId = sameCaseNumCases.First().SalesGroupID;

                var salesGroup = salesGroupDal.GetAll().First(t => t.ID == salesGroupId);
                sameCaseNumCases.ForEach(t =>
                {
                    t.CaseNum = (Convert.ToInt32(t.CaseNum.Substring(4)) + 100000).ToString();
                    t.NewCaseNum = "L" + salesGroup.ShortCode + "-" + t.CaseNum;
                });

                baseAuditDal.UpdateRange(sameCaseNumCases);
                baseAuditDal.AcceptAllChange();
            });
        }

        #region  合并重复
        /*
        public void InsertHisCase()
        {
            var migDAL = new MigTDAL();
            var personDAL = new RelationPersonAuditDAL();
            var salesGroupDAL = new SalesGroupDAL();
            var miglist = migDAL.GetAll();
            if (miglist.Any())
            {
                var salesGroups = salesGroupDAL.GetAll().ToList();

                int i = 1;
                foreach (var mig in miglist)
                {
                    var audit = new BaseAudit();
                    audit.ID = Guid.NewGuid().ToString();
                    audit.CaseNum = i.ToString("d6");

                    audit.BorrowerName = mig.CO2;
                    audit.BankCard = mig.CO30;
                    // 所属分公司名称 新的案件编号
                    switch (mig.CO4)
                    {
                        case "北京泓申":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "北京泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "北京泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "bff3003c-bfcb-4c34-898d-c9799db56e68";

                            break;
                        case "诚驰":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海诚驰泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海诚驰泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "0d17d335-6c79-4130-82d5-16173c142911";

                            break;
                        case "上海泓申":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海泓申投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海泓申投资管理有限公司").DistrictID;
                            audit.SalesID = "34965801-2ba7-4a80-a40b-7aff4f5c36a7";
                            break;
                        case "铉宇":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海铉宇金融信息服务有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海铉宇金融信息服务有限公司").DistrictID;
                            audit.SalesID = "f68acef9-b478-4ab0-ac43-ccaf6bf77988";
                            break;
                        case "银携":
                            audit.SalesGroupID = salesGroups.First(x => x.Name == "上海银携投资管理有限公司").ID;
                            audit.DistrictID = salesGroups.First(x => x.Name == "上海银携投资管理有限公司").DistrictID;
                            audit.SalesID = "028bcc6f-dc7d-4994-9f2e-196b5cf974ad";

                            break;
                        default:
                            audit.NewCaseNum = "HIS" + "-" + audit.CaseNum;
                            break;
                    }
                    audit.EarnestMoney = string.IsNullOrEmpty(mig.CO7) ? (decimal?)null : Convert.ToDecimal(mig.CO7);
                    audit.LenderName = mig.CO8;
                    audit.NewCaseNum = "HIS" + "-" + audit.CaseNum;
                    // 客户贷款期数
                    switch (mig.CO9)
                    {
                        case "1": audit.Term = "-LoanTerm-1M"; break;
                        case "2": audit.Term = "-LoanTerm-2M"; break;
                        case "3": audit.Term = "-LoanTerm-3M"; break;
                        case "4": audit.Term = "-LoanTerm-4M"; break;
                        case "5": audit.Term = "-LoanTerm-5M"; break;
                        case "6": audit.Term = "-LoanTerm-6M"; break;
                        case "12": audit.Term = "-LoanTerm-12M"; break;
                        case "24": audit.Term = "-LoanTerm-24M"; break;
                        default:
                            audit.Term = "-LoanTerm-2M"; break;
                            break;
                    }
                    audit.AuditTerm = audit.Term;

                    audit.LendingDate = string.IsNullOrEmpty(mig.CO10) ? default(DateTime?) : Convert.ToDateTime(mig.CO10);
                    audit.PaymentDate = string.IsNullOrEmpty(mig.CO11) ? default(DateTime?) : Convert.ToDateTime(mig.CO11);
                    audit.LoanAmount = string.IsNullOrEmpty(mig.CO13) ? default(decimal?) : Convert.ToDecimal(mig.CO13);
                    audit.AuditAmount = audit.LoanAmount;

                    audit.AnnualRate = string.IsNullOrEmpty(mig.CO14) ? null : (decimal?)Convert.ToDecimal(mig.CO14);
                    audit.AuditRate = audit.AnnualRate;

                    // 资金方名称
                    switch (mig.CO18)
                    {
                        case "点融": audit.ThirdParty = "-ThirdPlatform-DianRong"; break;
                        case "聚财猫": audit.ThirdParty = "-ThirdPlatform-JuCaiMao"; break;
                        case "米缸": audit.ThirdParty = "-ThirdPlatform-MiGang"; break;
                        case "诺亚": audit.ThirdParty = "-ThirdPlatform-NuoYa"; break;
                        case "挖财": audit.ThirdParty = "-ThirdPlatform-WaCai"; break;
                        default:
                            break;
                    }
                    audit.ThirdPartyAuditRate = string.IsNullOrEmpty(mig.CO19) ? null : (decimal?)Convert.ToDecimal(mig.CO19);
                    audit.ThirdPartyAuditAmount = string.IsNullOrEmpty(mig.CO28) ? null : (decimal?)Convert.ToDecimal(mig.CO28);
                    switch (mig.CO27)
                    {
                        case "1": audit.ThirdPartyAuditTerm = "-LoanTerm-1M"; break;
                        case "2": audit.ThirdPartyAuditTerm = "-LoanTerm-2M"; break;
                        case "3": audit.ThirdPartyAuditTerm = "-LoanTerm-3M"; break;
                        case "4": audit.ThirdPartyAuditTerm = "-LoanTerm-4M"; break;
                        case "5": audit.ThirdPartyAuditTerm = "-LoanTerm-5M"; break;
                        case "6": audit.ThirdPartyAuditTerm = "-LoanTerm-6M"; break;
                        case "12": audit.ThirdPartyAuditTerm = "-LoanTerm-12M"; break;
                        case "24": audit.ThirdPartyAuditTerm = "-LoanTerm-24M"; break;
                        default:
                            break;
                    }
                    // 贷款模式
                    switch (mig.CO20)
                    {
                        case "居间": audit.CaseMode = "-CaseMode-JuJian"; break;
                        case "债转": audit.CaseMode = "-CaseMode-ZhaiZhuan"; break;
                        default: audit.CaseMode = "-CaseMode-ZiYouZiJin"; break;
                    }

                    audit.OpeningBank = mig.CO23;
                    audit.BankCard = mig.CO26;
                    audit.CaseDetail = mig.CO30;
                    audit.CreateTime = DateTime.Now;
                    audit.CreateUser = "System";
                    audit.CaseStatus = "After";



                    var person = new RelationPersonAudit();
                    person.ID = Guid.NewGuid().ToString();
                    person.AuditID = audit.ID;
                    person.Name = mig.CO2;
                    person.RelationType = "-PersonType-JieKuanRen";
                    switch (mig.CO5)
                    {
                        case "香港居民身份证":
                            person.IdentificationType = "-DocType-HongKong";
                            break;
                        case "身份证":
                            person.IdentificationType = "-DocType-IDCard";
                            break;
                        case "台湾同胞来往内地通行证":
                            person.IdentificationType = "-DocType-Taiwan";
                            break;
                        default:
                            person.IdentificationType = "-DocType-Passport";
                            break;
                    }


                    person.IdentificationNumber = mig.CO6;

                    personDAL.Add(person);

                    var lend = new Com.HSJF.Framework.EntityFramework.Model.Lending.Lending();
                    var lenddal = new LendingDAL();
                    lend.ID = audit.ID;
                    lend.Borrower = audit.BorrowerName;
                    //lend.ContactNumber = mig.CO0;
                    lend.ContractAmount = audit.AuditAmount;
                    lend.CreateTime = DateTime.Now.ToShortDateString();
                    lend.CreateUser = "System";
                    lend.CustomerName = audit.BorrowerName;
                    lend.LendTime = audit.LendingDate;
                    lend.PaymentDay = string.IsNullOrEmpty(mig.CO12) ? default(int?) : Convert.ToInt32(mig.CO12);
                    lenddal.Add(lend);

                    _baseAuditDal.Add(audit);
                    mig.Status = audit.NewCaseNum;
                    migDAL.Update(mig);
                    i++;
                }
            }
            _baseAuditDal.AcceptAllChange();
            migDAL.AcceptAllChange();
        }
        */
        #endregion


        public IQueryable<string> GetNoFileCase()
        {
            var numList = new List<string>();
            var houstList = new HouseDetailDAL().GetAll();
            var file = new FileUpload();
            foreach (var h in houstList)
            {
                if (!string.IsNullOrEmpty(h.VillagePhotoFile))
                {
                    var f = file.Single(new Guid(h.VillagePhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.BedroomPhotoFile))
                {
                    var f = file.Single(new Guid(h.BedroomPhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.HousePhotoFile))
                {
                    var f = file.Single(new Guid(h.HousePhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.KitchenRoomPhotoFile))
                {
                    var f = file.Single(new Guid(h.KitchenRoomPhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.MainGatePhotoFile))
                {
                    var f = file.Single(new Guid(h.MainGatePhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.ParlourPhotoFile))
                {
                    var f = file.Single(new Guid(h.ParlourPhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(h.ToiletPhotoFile))
                {
                    var f = file.Single(new Guid(h.ToiletPhotoFile.Split(",")[0]));
                    if (f == null && !numList.Contains(h.BaseAuditID))
                    {
                        numList.Add(h.BaseAuditID);
                        continue;
                    }
                }


            }

            if (numList.Any())
            {
                var list = new BaseAuditDAL().GetAll().Where(t => numList.Contains(t.ID) && t.CaseStatus != "Close").Select(t => t.NewCaseNum);
                return list;
            }
            return null;
        }
    }
}
