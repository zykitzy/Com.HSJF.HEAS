using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.HSJF.Framework.DAL.Audit
{
    public class BaseAuditDAL : BaseDAL<BaseAudit>
    {
        public override void Add(BaseAudit entity)
        {
            var en = GetAll().FirstOrDefault(t => t.NewCaseNum == entity.NewCaseNum);
            if (en == null)
            {
                entity.Version = 0;
            }
            else
            {
                var maxversion = GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).Max(t => t.Version);
                entity.Version = maxversion + 1;
            }
            base.Add(entity);
        }

        /// <summary>
        /// 获取最新版本案件
        /// </summary>
        /// <param name="key">案件Id</param>
        /// <returns>案件信息</returns>
        public override BaseAudit Get(object key)
        {
            var entity = base.Get(key);
            if (entity == null)
            {
                return null;
            }
            var maxentity = this.GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault();
            if (entity.Version < 0 || entity.Version != maxentity.Version)
            {
                return null;
            }
            return entity;
        }

        /// <summary>
        /// 获取限制数据权限后的最新版本案件
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public BaseAudit GetAuthorizeAndSelf(object key, Infrastructure.Identity.Model.User user)
        {
            var entity = base.Get(key);
            if (entity == null)
            {
                return null;
            }
            var maxentity = GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault();
            if (entity.Version < 0 || entity.Version != maxentity.Version)
            {
                return null;
            }
            var pers = GetDataPermission(user);
            if (((pers.Contains(entity.DistrictID) && pers.Contains(entity.SalesGroupID)) || entity.CreateUser == user.UserName))
                return entity;
            else
                return null;
        }

        public BaseAudit GetMaxAudit(string id)
        {
            var entity = base.Get(id);
            if (entity == null)
            {
                return null;
            }
            var maxentity = this.GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault();
            if (maxentity != null)
            {
                return maxentity;
            }
            return null;
        }

        /// <summary>
        /// 获取限制数据权限后的最新版本案件
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public BaseAudit GetMaxAuditAuthorizeAndSelf(string id, Infrastructure.Identity.Model.User user)
        {
            var entity = base.Get(id);
            if (entity == null)
            {
                return null;
            }
            var maxentity = this.GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault();
            if (maxentity != null)
            {
                return maxentity;
            }
            var pers = GetDataPermission(user);
            if (((pers.Contains(entity.DistrictID) && pers.Contains(entity.SalesGroupID)) || entity.CreateUser == user.UserName))
                return entity;
            else
                return null;
        }

        public BaseAudit GetMinAudit(string id)
        {
            var entity = base.Get(id);
            if (entity == null)
            {
                return null;
            }
            var minentity = this.GetAll().Where(t => t.NewCaseNum == entity.NewCaseNum).OrderBy(t => t.Version).FirstOrDefault();
            if (minentity != null)
            {
                return minentity;
            }
            return null;
        }

        public override IQueryable<BaseAudit> GetAll()
        {
            return base.GetAll().Where(t => t.Version >= 0).GroupBy(t => t.CaseNum, (x, xs) => xs.OrderByDescending(a => a.Version).FirstOrDefault());
        }

        /// <summary>
        /// 获取限制数据权限后的数据
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IQueryable<BaseAudit> GetAllAuthorizeAndSelf(Infrastructure.Identity.Model.User user)
        {
            var pers = GetDataPermission(user);
            var list = base.GetAll();
            list = list.Where(t => t.Version >= 0 && ((pers.Contains(t.DistrictID) && pers.Contains(t.SalesGroupID)) || t.CreateUser == user.UserName));
            list = list.GroupBy(t => t.CaseNum, (x, xs) => xs.OrderByDescending(a => a.Version).FirstOrDefault());
            return list;
        }

        public IQueryable<BaseAudit> GetAllBase()
        {
            return base.GetAll();
        }

        public IQueryable<BaseAudit> GetHasStatus(string casestatus)
        {
            var list = base.GetAll().Where(t => t.Version >= 0 && t.CaseStatus == casestatus);
            var statuslist = this.GetAll();
            var returnlist = from i in statuslist
                             join j in list
                             on i.NewCaseNum equals j.NewCaseNum
                             select i;
            return returnlist;
        }

        public IQueryable<BaseAudit> GetHasStatusByPublic(string casestatus)
        {
            var list = base.GetAll().Where(t => t.Version >= 0 && t.CaseStatus == casestatus).GroupBy(t => t.CaseNum, (x, xs) => xs.OrderByDescending(a => a.Version).FirstOrDefault());
            // var list = base.GetAll().Where(t => t.Version >= 0 && t.CaseStatus == casestatus);
            var statuslist = this.GetAll();
            var returnlist = from i in statuslist
                             join j in list
                             on i.NewCaseNum equals j.NewCaseNum
                             select i;
            return returnlist;
        }

        /// <summary>
        /// 获取限制数据权限后的数据
        /// 数据权限限制为分公司或者创建者
        /// yanminchun 2016-10-12 增加数据权限限制
        /// </summary>
        /// <param name="casestatus"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public IQueryable<BaseAudit> GetAllAuthorizeAndSelfByPublic(string casestatus, Infrastructure.Identity.Model.User user)
        {
            var pers = GetDataPermission(user);
            var predicate = PredicateBuilder.True<BaseAudit>();
            predicate = predicate.And(testc => pers.Contains(testc.DistrictID));
            predicate = predicate.And(testc => pers.Contains(testc.SalesGroupID));
            predicate = predicate.Or(t => t.CreateUser == user.UserName);
            var list = base.GetAll(predicate).Where(t => t.Version >= 0 && t.CaseStatus == casestatus).GroupBy(t => t.CaseNum, (x, xs) => xs.OrderByDescending(a => a.Version).FirstOrDefault());
            // var list = base.GetAll().Where(t => t.Version >= 0 && t.CaseStatus == casestatus);
            var statuslist = this.GetAll();
            var returnlist = from i in statuslist
                             join j in list
                             on i.NewCaseNum equals j.NewCaseNum
                             select i;
            return returnlist;
        }

        public BaseAudit GetbyCaseSataus(string casenum, string casestatus)
        {
            return base.GetAll().Where(t => t.NewCaseNum == casenum && t.CaseStatus == casestatus).OrderByDescending(t => t.Version).FirstOrDefault();
        }

        public BaseAudit GetbyCaseSatausList(string casenum, params string[] casestatus)
        {
            return base.GetAll().Where(t => t.NewCaseNum == casenum && casestatus.Contains(t.CaseStatus)).OrderByDescending(t => t.Version).FirstOrDefault();
        }

        /// <summary>
        /// 根据案件号获取最新案件
        /// </summary>
        /// <param name="casenum">新的案件号</param>
        /// <returns>案件信息</returns>
        public BaseAudit GetbyCaseNum(string casenum)
        {
            return base.GetAll().Where(t => t.NewCaseNum == casenum).OrderByDescending(t => t.Version).FirstOrDefault();
        }

        /// <summary>
        /// 根据案件号获取列表
        /// </summary>
        /// <param name="casenum">案件号</param>
        /// <returns>案件信息列表</returns>
        public IQueryable<BaseAudit> GetListByCaseNum(string casenum)
        {
            return base.GetAll().Where(t => t.NewCaseNum == casenum).OrderByDescending(t => t.Version);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="entity">案件信息</param>
        public override void Delete(BaseAudit entity)
        {
            var basecase = base.Get(entity.ID);
            if (basecase != null)
            {
                basecase.Version = -1;
                base.Update(basecase);
            }
        }

        /// <summary>
        /// 通过案件
        /// </summary>
        /// <param name="auditid"></param>
        /// <param name="creatUser"></param>
        /// <returns></returns>
        public bool ApprovalAudit(string auditid, string creatUser)
        {
            //Test(auditid);
            //SaveChanges();

            var baseaduit = this.Get(auditid);
            AuditHelp ah = new AuditHelp();
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.SecondAudit)
            {
                return ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.PublicMortgage) != string.Empty;
            }
            return false;
        }

        /// <summary>
        /// 审批拒绝
        /// </summary>
        /// <param name="auditid">案件ID</param>
        /// <param name="creatUser">人员</param>
        /// <returns></returns>
        public bool RejectAudit(string auditid, string creatUser)
        {
            AuditHelp ah = new AuditHelp();
            var baseaduit = this.Get(auditid);
            if (baseaduit != null)
            {
                return ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.CloseCase) != string.Empty;
            }
            return false;
        }

        /// <summary>
        /// 审核审批退回
        /// </summary>
        /// <param name="auditid">案件ID</param>
        /// <param name="creatUser">人员</param>
        /// <returns></returns>
        public bool ReturnAudit(string auditid, string creatUser)
        {
            AuditHelp ah = new AuditHelp();
            var baseaduit = this.Get(auditid);
            if (baseaduit != null)
            {
                return ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.FirstAudit) != string.Empty;
            }
            return false;
        }

        /// <summary>
        /// 重载审批退回/拒绝 通过，状态修改
        /// </summary>
        /// <param name="baseaduit">案件信息</param>
        /// <param name="creatUser">修改人</param>
        /// <param name="caseStatus">案件状态</param>
        /// <returns></returns>
        public bool ReturnAudit(BaseAudit baseaduit, string creatUser, string caseStatus)
        {
            var auditHelper = new AuditHelp();
            if (baseaduit != null)
            {
                return auditHelper.CopyBaseAudit(baseaduit, creatUser, caseStatus) != string.Empty;
            }
            return false;
        }

        /// <summary>
        /// 公正
        /// </summary>
        /// <param name="auditid"></param>
        /// <param name="creatUser"></param>
        /// <returns></returns>
        public bool PublicMortgageAudit(string auditid, string creatUser)
        {
            AuditHelp ah = new AuditHelp();
            var baseaduit = this.Get(auditid);
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.PublicMortgage)
            {
                return ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.Lending) != string.Empty;
            }
            return false;
        }

        /// <summary>
        /// 放款
        /// </summary>
        /// <param name="auditid"></param>
        /// <param name="creatUser"></param>
        /// <returns></returns>
        public bool LendingAudit(string auditid, string creatUser)
        {
            AuditHelp ah = new AuditHelp();
            var baseaduit = this.Get(auditid);
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.Lending)
            {
                return ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.CloseCase) != string.Empty;
            }
            return false;
        }

        public IList<BaseAudit> GetAllPage(IQueryable<BaseAudit> query, out int totalCount, int pageSize, int pageIndex, string order, string sort)
        {
            totalCount = query.Count();
            return base.ForPage(query, pageSize, pageIndex, order, sort);
        }

        public IList<BaseAudit> GetAllPageOrderByCaseStatus(IQueryable<BaseAudit> query, out int totalCount, int pageSize, int pageIndex, string order, string sort)
        {
            totalCount = query.Count();
            return base.ForPageOrderByCaseStatus(query, pageSize, pageIndex, order, sort);
        }
    }

    public class AuditHelp
    {
        private BaseAuditDAL badal;
        private RelationEnterpriseAuditDAL relationEnterpriseAuditDAL;
        private RelationPersonAuditDAL relationPersonAuditDAL;
        private AddressAuditDAL addressAuditDAL;
        private EmergencyContactAuditDAL emergencyContactAuditDAL;
        private ContactAuditDAL contactAuditDAL;
        private CollateralAuditDAL collateralAuditDAL;
        private IntroducerAuditDAL IntroducerAuditDAL;
        private IndividualCreditDAL individualCreditDAL;
        private EnterpriseCreditDAL enterpriseCreditDAL;
        private EnforcementPersonDAL enforcementPersonDAL;
        private IndustryCommerceTaxDAL industryCommerceTaxDAL;
        private GuarantorDAL guarantorDAL;
        private HouseDetailDAL houseDetailDAL;
        private EstimateSourceDAL estimateSourceDAL;

        public AuditHelp()
        {
            badal = new BaseAuditDAL();
            relationEnterpriseAuditDAL = new RelationEnterpriseAuditDAL();
            relationPersonAuditDAL = new RelationPersonAuditDAL();
            addressAuditDAL = new AddressAuditDAL();
            emergencyContactAuditDAL = new EmergencyContactAuditDAL();
            contactAuditDAL = new ContactAuditDAL();
            collateralAuditDAL = new CollateralAuditDAL();
            individualCreditDAL = new IndividualCreditDAL();
            enterpriseCreditDAL = new EnterpriseCreditDAL();
            enforcementPersonDAL = new EnforcementPersonDAL();
            industryCommerceTaxDAL = new IndustryCommerceTaxDAL();
            guarantorDAL = new GuarantorDAL();
            houseDetailDAL = new HouseDetailDAL();
            estimateSourceDAL = new EstimateSourceDAL();
            IntroducerAuditDAL = new IntroducerAuditDAL();
        }

        /// <summary>
        /// 复制审核信息
        /// </summary>
        /// <param name="baseaduit">案件信息</param>
        /// <param name="creatUser">创建人</param>
        /// <param name="caseStatus">案件状态</param>
        /// <returns>新的案件号</returns>
        public string CopyBaseAudit(BaseAudit baseaduit, string creatUser, string caseStatus, bool isAccept = true)
        {
            if (baseaduit != null)
            {
                var newaudit = new BaseAudit();

                #region 审核主表信息

                newaudit.ID = Guid.NewGuid().ToString();
                newaudit.CreateTime = DateTime.Now;
                newaudit.CreateUser = creatUser;
                newaudit.CaseStatus = caseStatus;
                newaudit.Version = baseaduit.Version + 1;
                newaudit.BorrowerName = baseaduit.BorrowerName;
                newaudit.CaseNum = baseaduit.CaseNum;
                newaudit.NewCaseNum = baseaduit.NewCaseNum;
                newaudit.CaseType = baseaduit.CaseType;
                newaudit.DistrictID = baseaduit.DistrictID;
                newaudit.IsNeedReport = baseaduit.IsNeedReport;
                newaudit.LoanAmount = baseaduit.LoanAmount;
                newaudit.SalesGroupID = baseaduit.SalesGroupID;
                newaudit.Term = baseaduit.Term;
                newaudit.SalesID = baseaduit.SalesID;
                newaudit.Partner = baseaduit.Partner;
                newaudit.AuditAmount = baseaduit.AuditAmount;
                newaudit.AnnualRate = baseaduit.AnnualRate;
                newaudit.PlatformCharge = baseaduit.PlatformCharge;
                newaudit.ComprehensiveRate = baseaduit.ComprehensiveRate;
                newaudit.MortgageOrder = baseaduit.MortgageOrder;
                newaudit.CaseDetail = baseaduit.CaseDetail;
                newaudit.AuditTerm = baseaduit.AuditTerm;
                newaudit.AuditRate = baseaduit.AuditRate;
                newaudit.OpeningBank = baseaduit.OpeningBank;
                newaudit.OpeningSite = baseaduit.OpeningSite;
                newaudit.BankCard = baseaduit.BankCard;
                newaudit.AuditComment = baseaduit.AuditComment;
                newaudit.ServiceCharge = baseaduit.ServiceCharge;
                newaudit.ServiceChargeRate = baseaduit.ServiceChargeRate;
                newaudit.Deposit = baseaduit.Deposit;
                newaudit.DepositDate = baseaduit.DepositDate;
                newaudit.IsActivitieRate = baseaduit.IsActivitieRate;

                newaudit.ThirdPartyAuditAmount = baseaduit.ThirdPartyAuditAmount;
                newaudit.ThirdPartyAuditRate = baseaduit.ThirdPartyAuditRate;
                newaudit.ThirdPartyAuditTerm = baseaduit.ThirdPartyAuditTerm;
                //newaudit.Description = baseaduit.Description;

                #region 2016-06-27 再次新增

                //跟单人
                newaudit.Merchandiser = baseaduit.Merchandiser;
                //出借人姓名
                newaudit.LenderName = baseaduit.LenderName;
                //保证金
                newaudit.EarnestMoney = baseaduit.EarnestMoney;
                //外访费（下户费）
                newaudit.OutboundCost = baseaduit.OutboundCost;
                //代收公证费用
                newaudit.DebitNotarizationCost = baseaduit.DebitNotarizationCost;
                //代收评估费
                newaudit.DebitEvaluationCost = baseaduit.DebitEvaluationCost;
                //代收担保费
                newaudit.DebitGuaranteeCost = baseaduit.DebitGuaranteeCost;
                //代收保险费
                newaudit.DebitInsuranceCost = baseaduit.DebitInsuranceCost;
                //代收其他
                newaudit.DebitOtherCost = baseaduit.DebitOtherCost;
                //公司承担的公证费
                newaudit.LevyNotarizationCost = baseaduit.LevyNotarizationCost;
                //公司承担的产调费
                newaudit.LevyAssetsSurveyCost = baseaduit.LevyAssetsSurveyCost;
                //公司承担的信用报告费
                newaudit.LevyCreditReportCost = baseaduit.LevyCreditReportCost;
                //公司承担的其他费用
                newaudit.LevyOtherCost = baseaduit.LevyOtherCost;
                //新增审核字段
                //案件模式
                newaudit.CaseMode = baseaduit.CaseMode;
                //第三方平台
                newaudit.ThirdParty = baseaduit.ThirdParty;
                //月利息金额
                newaudit.MonthlyInterest = baseaduit.MonthlyInterest;
                //放款日期
                newaudit.LendingDate = baseaduit.LendingDate;
                //回款日期
                newaudit.PaymentDate = baseaduit.PaymentDate;
                //实收利息（不退客户）
                newaudit.ActualInterest = baseaduit.ActualInterest;
                //预收利息（可退客户）
                newaudit.AdvanceInterest = baseaduit.AdvanceInterest;
                //客户保证金
                newaudit.CustEarnestMoney = baseaduit.CustEarnestMoney;
                #endregion 2016-06-27 再次新增

                //2016-09-08 大改

                newaudit.PaymentFactor = baseaduit.PaymentFactor;
                newaudit.Purpose = baseaduit.Purpose;
                newaudit.LendingTerm = baseaduit.LendingTerm;
                newaudit.ContractFileInfo = baseaduit.ContractFileInfo;
                newaudit.RejectReason = baseaduit.RejectReason;
                newaudit.RejectType = baseaduit.RejectType;

                #endregion 审核主表信息

                //借款申请书
                newaudit.LoanProposedFile = SaveFiles(baseaduit.LoanProposedFile, newaudit.ID, newaudit.ID);
                // 面谈报告
                newaudit.FaceReportFile = SaveFiles(baseaduit.FaceReportFile, newaudit.ID, newaudit.ID);
                // 现场报告
                newaudit.FieldReportFile = SaveFiles(baseaduit.FieldReportFile, newaudit.ID, newaudit.ID);
                // 贷前尽调报告
                newaudit.LoanDetailReportFile = SaveFiles(baseaduit.LoanDetailReportFile, newaudit.ID, newaudit.ID);
                // 借款申请书
                //newaudit.LoanProposedFile = SaveFiles(baseaduit.LoanProposedFile, newaudit.ID, newaudit.ID);
                // 保存关系人信息及其自信息
                var psersonlist = SaveRelationPersons(baseaduit, newaudit);

                // 保存抵押物信息
                var collList = SaveCollaterals(baseaduit, newaudit);

                // 保存担保人信息集合
                var guaList = SaveGuarantor(baseaduit, newaudit);
                //介绍人信息
                var Introducer = SaveIntroducers(baseaduit, newaudit);

                //复制签约信息
                // SavePublic(baseaduit.ID, newaudit.ID, creatUser);
                badal.Add(newaudit);
                if (isAccept)
                {
                    badal.AcceptAllChange();
                }
                return newaudit.ID;
            }
            else
            {
                return string.Empty;
            }
        }

        //public void SavePublic(string id, string newid, string creatUser)
        //{
        //    MortgageDAL mort = new MortgageDAL();
        //    var morimodel = mort.GetPublic(id);
        //    if (morimodel == null)
        //    {
        //        return;
        //    }
        //    var pubMort = new PublicMortgage()
        //    {
        //        CreateTime = DateTime.Now,
        //        ID = newid,
        //        CreateUser = creatUser,
        //        ContractFile = morimodel.ContractFile,
        //        NoteFile = morimodel.NoteFile,
        //        ReceiptFile = morimodel.ReceiptFile,
        //        OtherFile = morimodel.OtherFile,
        //        FourFile = morimodel.FourFile,
        //        ContractNo = morimodel.ContractNo,
        //        ContractAmount = morimodel.ContractAmount,
        //        ContractDate = morimodel.ContractDate,
        //        ContractPerson = morimodel.ContractPerson,
        //        UndertakingFile = morimodel.UndertakingFile,
        //        RepaymentAttorneyFile = morimodel.RepaymentAttorneyFile,
        //        ContactConfirmFile = morimodel.ContactConfirmFile,
        //        PowerAttorneyFile = morimodel.PowerAttorneyFile,
        //        CollectionFile = morimodel.CollectionFile,
        //    };
        //    mort.Add(pubMort);
        //}

        /// <summary>
        /// 保存关系人信息集合
        /// </summary>
        /// <param name="RelationPersons"></param>
        /// <param name="auditId"></param>
        public IEnumerable<RelationPersonAudit> SaveRelationPersons(BaseAudit baseaudit, BaseAudit newaudit)
        {
            RelationPersonAuditDAL rpDAL = new RelationPersonAuditDAL();
            var personList = new List<RelationPersonAudit>();
            foreach (var model in baseaudit.RelationPersonAudits)
            {
                RelationPersonAudit entity = new RelationPersonAudit();

                #region 关系人信息集合

                entity.ID = Guid.NewGuid().ToString();
                entity.AuditID = newaudit.ID;
                entity.Birthday = model.Birthday;
                entity.ExpiryDate = model.ExpiryDate;
                entity.IdentificationNumber = model.IdentificationNumber;
                entity.IdentificationType = model.IdentificationType;
                entity.MaritalStatus = model.MaritalStatus;
                entity.Name = model.Name;
                entity.RelationType = model.RelationType;
                entity.SalaryDescription = model.SalaryDescription;
                entity.BorrowerRelation = model.BorrowerRelation;
                entity.IsCoBorrower = model.IsCoBorrower;
                entity.Warranty = model.Warranty;
                entity.Sequence = model.Sequence;
                //  上传身份证复印件
                entity.IdentificationFile = SaveFiles(model.IdentificationFile, entity.ID, newaudit.ID);
                //  上传婚姻证明文件保存
                entity.MarryFile = SaveFiles(model.MarryFile, entity.ID, newaudit.ID);
                //  上传单身证明文件保存
                entity.SingleFile = SaveFiles(model.SingleFile, entity.ID, newaudit.ID);
                //  上传出生证明文件保存
                entity.BirthFile = SaveFiles(model.BirthFile, entity.ID, newaudit.ID);
                //  上传户口本复印件文件保存
                entity.AccountFile = SaveFiles(model.AccountFile, entity.ID, newaudit.ID);
                //  上传收入证明（受薪水人士）文件保存
                entity.SalaryPersonFile = SaveFiles(model.SalaryPersonFile, entity.ID, newaudit.ID);
                //  上传收入证明（自雇有执照）文件保存
                entity.SelfLicenseFile = SaveFiles(model.SelfLicenseFile, entity.ID, newaudit.ID);
                //  上传收入证明（自雇无执照）文件保存
                entity.SelfNonLicenseFile = SaveFiles(model.SelfNonLicenseFile, entity.ID, newaudit.ID);
                //  银行流水
                entity.BankFlowFile = SaveFiles(model.BankFlowFile, entity.ID, newaudit.ID);
                //  个人征信报告
                entity.IndividualFile = SaveFiles(model.IndividualFile, entity.ID, newaudit.ID);
                //  其他证明
                entity.OtherFile = SaveFiles(model.OtherFile, entity.ID, newaudit.ID);
                SaveAddresses(model, entity);
                SaveEmergencyContacts(model, entity);
                SaveContacts(model, entity);
                SaveRelationEnterprises(model, entity, baseaudit, newaudit);

                // 保存个人资信信息集合
                SaveIndividualCredit(baseaudit, newaudit, model, entity);
                // 保存被执行人信息集合
                SaveEnforcementPerson(baseaudit, newaudit, model, entity);

                personList.Add(entity);
            }
            relationPersonAuditDAL.AddRange(personList);
            return personList;
        }

        #region 文件辅助类型

        private string SaveFiles(string filenames, string linkId, string linkkey)
        {
            var up = new FileUpload();//文件上传
            string files = null;
            if (string.IsNullOrEmpty(filenames))
            {
                return files;
            }
            foreach (var file in filenames.Split(','))
            {
                var filemodel = up.Single(new Guid(file));
                if (filemodel != null)
                {
                    var entity = CopyFile(filemodel);
                    entity.LinkID = Guid.Parse(linkId);
                    entity.LinkKey = linkkey;
                    up.SaveFileDescription(entity);
                    files += entity.ID;
                    files += ",";
                }
            }
            return files.Trim(',');
        }

        private FileDescription CopyFile(FileDescription model)
        {
            var entity = new FileDescription();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, entity);
            entity.ID = Guid.NewGuid();
            entity.FileCreateTime = DateTime.Now;
            return entity;
        }

        #endregion 文件辅助类型

        /// <summary>
        /// 保存关系人地址信息集合
        /// </summary>
        /// <param name="address"></param>
        /// <param name="personId"></param>
        public IEnumerable<AddressAudit> SaveAddresses(RelationPersonAudit oldperson, RelationPersonAudit newperson)
        {
            var addressList = new List<AddressAudit>();

            foreach (var model in oldperson.AddressAudits)
            {
                AddressAudit entity = new AddressAudit();

                entity.ID = Guid.NewGuid().ToString();
                entity.PersonID = newperson.ID;
                entity.AddressInfo = model.AddressInfo;
                entity.AddressType = model.AddressType;
                entity.IsDefault = model.IsDefault;
                entity.Sequence = model.Sequence;

                addressList.Add(entity);
            }
            newperson.AddressAudits = addressList;
            addressAuditDAL.AddRange(addressList);
            return addressList;
        }

        /// <summary>
        /// 保存关系人紧急联系人信息集合
        /// </summary>
        /// <param name="emergencyContacts"></param>
        /// <param name="personId"></param>
        public IEnumerable<EmergencyContactAudit> SaveEmergencyContacts(RelationPersonAudit oldperson, RelationPersonAudit newperson)
        {
            var emerList = new List<EmergencyContactAudit>();
            foreach (var model in oldperson.EmergencyContactAudits)
            {
                EmergencyContactAudit entity = new EmergencyContactAudit();
                entity.ID = Guid.NewGuid().ToString();
                entity.PersonID = newperson.ID;
                entity.ContactNumber = model.ContactNumber;
                entity.ContactType = model.ContactType;
                entity.Name = model.Name;
                entity.Sequence = model.Sequence;

                emerList.Add(entity);
            }
            emergencyContactAuditDAL.AddRange(emerList);
            return emerList;
        }

        /// <summary>
        /// 保存关系人联系方式信息集合
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="personId"></param>
        public IEnumerable<ContactAudit> SaveContacts(RelationPersonAudit oldperson, RelationPersonAudit newperson)
        {
            var emerList = new List<ContactAudit>();
            foreach (var model in oldperson.ContactAudits)
            {
                ContactAudit entity = new ContactAudit();

                entity.ID = Guid.NewGuid().ToString();
                entity.PersonID = newperson.ID;
                entity.ContactNumber = model.ContactNumber;
                entity.ContactType = model.ContactType;
                entity.IsDefault = model.IsDefault;
                entity.Sequence = model.Sequence;

                emerList.Add(entity);
            }
            contactAuditDAL.AddRange(emerList);
            return emerList;
        }

        /// <summary>
        /// 关系人相关企业信息集合
        /// </summary>
        /// <param name="relationEnterprises"></param>
        /// <param name="personId"></param>
        public IEnumerable<RelationEnterpriseAudit> SaveRelationEnterprises(RelationPersonAudit oldperson, RelationPersonAudit newperson, BaseAudit baseaudit, BaseAudit newaudit)
        {
            var entList = new List<RelationEnterpriseAudit>();
            foreach (var model in oldperson.RelationEnterpriseAudits)
            {
                RelationEnterpriseAudit entity = new RelationEnterpriseAudit();

                entity.ID = Guid.NewGuid().ToString();
                entity.PersonID = newperson.ID;
                entity.Address = model.Address;
                entity.EnterpriseDes = model.EnterpriseDes;
                entity.EnterpriseName = model.EnterpriseName;
                entity.LegalPerson = model.LegalPerson;
                entity.MainBusiness = model.MainBusiness;
                entity.RegisteredCapital = model.RegisteredCapital;
                entity.RegisterNumber = model.RegisterNumber;
                entity.ShareholderDetails = model.ShareholderDetails;
                entity.BankFlowFile = SaveFiles(model.BankFlowFile, entity.ID, newaudit.ID);
                entity.IndividualFile = SaveFiles(model.IndividualFile, entity.ID, newaudit.ID);
                entity.Sequence = model.Sequence;

                // 保存企业资信信息集合
                SaveEnterpriseCredit(baseaudit, newaudit, model, entity);
                // 保存工商税务信息集合
                SaveIndustryCommerceTax(baseaudit, newaudit, model, entity);

                entList.Add(entity);
            }
            relationEnterpriseAuditDAL.AddRange(entList);
            return entList;
        }

        /// <summary>
        /// 保存抵押物信息
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public IEnumerable<CollateralAudit> SaveCollaterals(BaseAudit baseAudit, BaseAudit newAudit)
        {
            var collList = new List<CollateralAudit>();
            foreach (var model in baseAudit.CollateralAudits)
            {
                CollateralAudit entity = new CollateralAudit();
                // 抵押物信息
                entity.ID = Guid.NewGuid().ToString();
                entity.AuditID = newAudit.ID;
                entity.Address = model.Address;
                entity.BuildingName = model.BuildingName;
                entity.CollateralType = model.CollateralType;
                entity.HouseNumber = model.HouseNumber;
                entity.HouseSize = model.HouseSize;
                entity.RightOwner = model.RightOwner;
                entity.Sequence = model.Sequence;
                entity.CompletionDate = model.CompletionDate;
                entity.LandType = model.LandType;
                entity.HouseType = model.HouseType;
                // 上传房屋文件保存
                entity.HouseFile = SaveFiles(model.HouseFile, entity.ID, newAudit.ID);

                //保存房屋明细信息集合
                SaveHouseDetail(baseAudit, newAudit, model, entity);
                collList.Add(entity); //保存抵押物信息
            }
            collateralAuditDAL.AddRange(collList);
            return collList;
        }

        /// <summary>
        /// 保存介绍人信息
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public IEnumerable<IntroducerAudit> SaveIntroducers(BaseAudit baseAudit, BaseAudit newAudit)
        {
            var IntroducerAuditList = new List<IntroducerAudit>();
            foreach (var model in baseAudit.IntroducerAudits)
            {
                IntroducerAudit Introducer = new IntroducerAudit();
                // 介绍人信息
                Introducer.ID = Guid.NewGuid().ToString();
                Introducer.AuditID = newAudit.ID;
                Introducer.Contract = model.Contract;
                Introducer.Name = model.Name;
                Introducer.RebateAmmount = model.RebateAmmount;
                Introducer.RebateRate = model.RebateRate;
                Introducer.Account = model.Account;
                Introducer.AccountBank = model.AccountBank;
                Introducer.Sequence = model.Sequence;

                IntroducerAuditList.Add(Introducer); //保存抵押物信息
            }
            IntroducerAuditDAL.AddRange(IntroducerAuditList);
            return IntroducerAuditList;
        }

        /// <summary>
        /// 保存个人资信信息集合
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public void SaveIndividualCredit(BaseAudit baseAudit, BaseAudit newAudit, RelationPersonAudit oldperson, RelationPersonAudit newperson)
        {
            var indList = new List<IndividualCredit>();

            var indiv = baseAudit.IndividualCredits.Where(t => t.PersonID == oldperson.ID);
            if (!indiv.Any())
            {
                return;
            }
            foreach (var model in indiv)
            {
                var entity = new IndividualCredit();

                entity.ID = Guid.NewGuid().ToString();
                entity.BaseAuditID = newAudit.ID;
                entity.CreditCard = model.CreditCard;
                entity.CreditInfo = model.CreditInfo;
                entity.OtherCredit = model.OtherCredit;
                entity.OverdueInfo = model.OverdueInfo;
                entity.PersonID = newperson.ID;
                entity.IndividualFile = SaveFiles(model.IndividualFile, entity.ID, newAudit.ID);
                entity.BankFlowFile = SaveFiles(model.BankFlowFile, entity.ID, newAudit.ID);
                entity.Sequence = model.Sequence;

                indList.Add(entity); //保存个人资信信息
            }
            individualCreditDAL.AddRange(indList);
        }

        /// <summary>
        /// 保存企业资信信息集合
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public void SaveEnterpriseCredit(BaseAudit baseAudit, BaseAudit newAudit, RelationEnterpriseAudit oldEnterprise, RelationEnterpriseAudit newEnterprise)
        {
            var entCRList = new List<EnterpriseCredit>();
            var enter = baseAudit.EnterpriseCredits.Where(t => t.EnterpriseID == oldEnterprise.ID);
            if (!enter.Any())
            {
                return;
            }
            foreach (var model in enter)
            {
                var entity = new EnterpriseCredit();
                entity.ID = Guid.NewGuid().ToString();
                entity.BaseAuditID = newAudit.ID;
                entity.CreditCard = model.CreditCard;
                entity.CreditInfo = model.CreditInfo;
                entity.EnterpriseID = newEnterprise.ID;
                entity.ShareholderDetails = model.ShareholderDetails;
                entity.OtherDetailes = model.OtherDetailes;
                entity.Sequence = model.Sequence;

                entCRList.Add(entity); //保存企业资信信息
            }
            enterpriseCreditDAL.AddRange(entCRList);
        }

        /// <summary>
        /// 保存被执行人信息集合
        /// </summary>
        /// <param name="enforcementPerson"></param>
        /// <param name="auditId"></param>
        public void SaveEnforcementPerson(BaseAudit baseAudit, BaseAudit newAudit, RelationPersonAudit oldperson, RelationPersonAudit newperson)
        {
            var entCRList = new List<EnforcementPerson>();
            var enforce = baseAudit.EnforcementPersons.Where(t => t.PersonID == oldperson.ID);
            if (!enforce.Any())
            {
                return;
            }
            foreach (var model in enforce)
            {
                var entity = new EnforcementPerson();
                entity.ID = Guid.NewGuid().ToString();
                entity.BaseAuditID = newAudit.ID;
                entity.BadNews = model.BadNews;
                entity.EnforcementWeb = model.EnforcementWeb;
                entity.LawXP = model.LawXP;
                entity.ShiXin = model.ShiXin;
                entity.TrialRecord = model.TrialRecord;
                entity.PersonID = newperson.ID;
                entity.Sequence = model.Sequence;
                entity.AttachmentFile = model.AttachmentFile;

                entCRList.Add(entity); //保存被执行人信息
            }
            enforcementPersonDAL.AddRange(entCRList);
        }

        /// <summary>
        /// 保存工商税务信息集合
        /// </summary>
        /// <param name="industryCommerceTax"></param>
        /// <param name="auditId"></param>
        public void SaveIndustryCommerceTax(BaseAudit baseAudit, BaseAudit newAudit, RelationEnterpriseAudit oldEnterprise, RelationEnterpriseAudit newEnterprise)
        {
            var indList = new List<IndustryCommerceTax>();
            var indTax = baseAudit.IndustryCommerceTaxs.Where(t => t.EnterpriseID == oldEnterprise.ID);
            if (!indTax.Any())
            {
                return;
            }
            foreach (var model in indTax)
            {
                var entity = new IndustryCommerceTax();
                entity.ID = Guid.NewGuid().ToString();
                entity.BaseAuditID = newAudit.ID;
                entity.ActualManagement = model.ActualManagement;
                entity.AnnualInspection = model.AnnualInspection;
                entity.Description = model.Description;
                entity.EnterpriseID = newEnterprise.ID;
                entity.Sequence = model.Sequence;

                indList.Add(entity); //保存工商税务信息
            }
            industryCommerceTaxDAL.AddRange(indList);
        }

        /// <summary>
        /// 保存担保人信息集合
        /// </summary>
        /// <param name="guarantor"></param>
        /// <param name="auditId"></param>
        public IEnumerable<Guarantor> SaveGuarantor(BaseAudit baseAudit, BaseAudit newAudit)
        {
            var indList = new List<Guarantor>();
            foreach (var model in baseAudit.Guarantors)
            {
                #region 担保人信息

                var entity = new Guarantor();
                entity.ID = Guid.NewGuid().ToString();
                entity.BaseAuditID = newAudit.ID;
                entity.Address = model.Address;
                entity.ContactNumber = model.ContactNumber;
                entity.GuarantType = model.GuarantType;
                entity.IdentityNumber = model.IdentityNumber;
                entity.IdentityType = model.IdentityType;
                entity.MarriedInfo = model.MarriedInfo;
                entity.Name = model.Name;
                entity.RelationType = model.RelationType;
                entity.Sequence = model.Sequence;

                indList.Add(entity); //保存担保人信息

                #endregion 担保人信息
            }
            guarantorDAL.AddRange(indList);
            return indList;
        }

        #region 房屋信息

        /// <summary>
        /// 保存房屋明细信息集合
        /// </summary>
        /// <param name="houseDetail"></param>
        /// <param name="auditId"></param>
        public void SaveHouseDetail(BaseAudit baseAudit, BaseAudit newAudit, CollateralAudit oldColl, CollateralAudit newColl)
        {
            var houseList = new List<HouseDetail>();
            var house = baseAudit.HouseDetails.Where(t => t.CollateralID == oldColl.ID);
            if (!house.Any())
            {
                return;
            }
            foreach (var model in house)
            {
                var entity = new HouseDetail
                {
                    ID = Guid.NewGuid().ToString(),
                    BaseAuditID = newAudit.ID,
                    Accout = model.Accout,
                    AssessedValue = model.AssessedValue,
                    CompletionDate = model.CompletionDate,
                    Description = model.Description,
                    EstimateSources = model.EstimateSources,
                    HouseType = model.HouseType,
                    LimitInfo = model.LimitInfo,
                    LandType = model.LandType,
                    RepairSituation = model.RepairSituation,
                    ServiceCondition = model.ServiceCondition,
                    TotalHeight = model.TotalHeight,
                    Collateral = model.Collateral,
                    CollateralID = newColl.ID,
                    Sequence = model.Sequence,

                    RealHigh = model.RealHigh,
                    IsDamage = model.IsDamage,
                    RealResident = model.RealResident,
                    WaterPaymentCheck = model.WaterPaymentCheck,
                    TaxPaymentCheck = model.TaxPaymentCheck,
                    Man2Wei1 = model.Man2Wei1,
                    SpecialResident = model.SpecialResident,
                    OtherDescription = model.OtherDescription,
                    SchoolInfo = model.SchoolInfo,
                    HospitalInfo = model.HospitalInfo,
                    TrafficInfo = model.TrafficInfo,
                    Supermarket = model.Supermarket,
                    Recreation = model.Recreation,
                    NegativeSite = model.NegativeSite
                };
                // Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, entity);

                entity.VillagePhotoFile = SaveFiles(model.VillagePhotoFile, entity.ID, newAudit.ID);
                entity.MainGatePhotoFile = SaveFiles(model.MainGatePhotoFile, entity.ID, newAudit.ID);
                entity.ParlourPhotoFile = SaveFiles(model.ParlourPhotoFile, entity.ID, newAudit.ID);
                entity.BedroomPhotoFile = SaveFiles(model.BedroomPhotoFile, entity.ID, newAudit.ID);
                entity.KitchenRoomPhotoFile = SaveFiles(model.KitchenRoomPhotoFile, entity.ID, newAudit.ID);
                entity.ToiletPhotoFile = SaveFiles(model.ToiletPhotoFile, entity.ID, newAudit.ID);

                entity.HousePhotoFile = SaveFiles(model.HousePhotoFile, entity.ID, newAudit.ID);
                entity.HouseReportFile = SaveFiles(model.HouseReportFile, entity.ID, newAudit.ID);
                houseList.Add(entity);
                SaveEstimateSource(model, entity);
            }
            houseDetailDAL.AddRange(houseList);
        }

        /// <summary>
        /// 房屋评估来源信息集合
        /// </summary>
        /// <param name="estimateSource"></param>
        /// <param name="houseDetailId"></param>
        public IEnumerable<EstimateSource> SaveEstimateSource(HouseDetail oldmodel, HouseDetail newmodel)
        {
            var estList = new List<EstimateSource>();
            foreach (var model in oldmodel.EstimateSources)
            {
                var entity = new EstimateSource
                {
                    ID = Guid.NewGuid().ToString(),
                    HouseDetailID = newmodel.ID,
                    ContactNumber = model.ContactNumber,
                    EstimateInstitutions = model.EstimateInstitutions,
                    HouseDetail = model.HouseDetail,
                    InformationProvider = model.InformationProvider,
                    RushEstimate = model.RushEstimate,
                    Sequence = model.Sequence,
                    CertificateFile = model.CertificateFile
                };
                entity.CertificateFile = SaveFiles(model.CertificateFile, entity.ID, newmodel.ID);
                estList.Add(entity);
            }
            newmodel.EstimateSources = estList;
            estimateSourceDAL.AddRange(estList);
            return estList;
        }

        #endregion 房屋信息
    }

    #endregion 关系人信息集合
}