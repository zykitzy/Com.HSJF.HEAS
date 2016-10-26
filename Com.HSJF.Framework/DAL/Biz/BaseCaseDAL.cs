using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.HSJF.Framework.DAL.Biz
{
    public class BaseCaseDAL : BaseDAL<BaseCase>
    {
        public override void Add(BaseCase entity)
        {
            //  entity.Version = 0;
            base.Add(entity);
        }

        public override BaseCase Get(object key)
        {
            var entity = base.Get(key);

            if (entity == null || entity.Version < 0)
            {
                return null;
            }
            return entity;
        }

        public BaseCase GetSelfCase(object key, Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            var entity = base.Get(key);
            if (entity == null)
                return null;
            if (entity.CreateUser == user.UserName)
            {
                return entity;
            }
            else
                return null;
        }

        public BaseCase GetAuthorizeAndSelf(object key, Infrastructure.Identity.Model.User user)
        {
            var entity = base.Get(key);
            if (entity == null)
            {
                return null;
            }

            var pers = GetDataPermission(user);
            if (((pers.Contains(entity.DistrictID) && pers.Contains(entity.SalesGroupID)) || entity.CreateUser == user.UserName))
                return entity;
            else
                return null;
        }

        public bool IsAuthorize(object key, Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            var entity = base.Get(key);
            if (entity == null)
                return false;
            var pers = GetDataPermission(user);
            if (((pers.Contains(entity.DistrictID) && pers.Contains(entity.SalesGroupID)) || entity.CreateUser == user.UserName))
                return true;
            else
                return false;
        }

        public override IQueryable<BaseCase> GetAll()
        {
            return base.GetAll().Where(t => t.Version >= 0);
        }

        public IList<BaseCase> GetAllPage(IQueryable<BaseCase> query, out int totalCount, int pageSize, int pageIndex, string order, string sort)
        {
            totalCount = query.Count();
            return base.ForPage(query, pageSize, pageIndex, order, sort);
        }

        //根据数据权限获取
        public IEnumerable<BaseCase> GetAllAuthorizeAndSelf(Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            var pers = GetDataPermission(user);
            var predicate = PredicateBuilder.True<BaseCase>();
            predicate = predicate.And(testc => pers.Contains(testc.DistrictID));
            predicate = predicate.And(testc => pers.Contains(testc.SalesGroupID));
            predicate = predicate.And(t => t.Version > 0);
            predicate = predicate.Or(t => t.CreateUser == user.UserName);
            return base.GetAll(predicate);
        }

        //查询列表
        public IQueryable<BaseCase> GetAllAuthorizeAndSelfQuery(Infrastructure.Identity.Model.User user, int? version, string borrownname, string caseNum)
        {
            var pers = GetDataPermission(user);
            var predicate = PredicateBuilder.True<BaseCase>();
            predicate = predicate.And(testc => pers.Contains(testc.DistrictID));
            predicate = predicate.And(testc => pers.Contains(testc.SalesGroupID));
            predicate = predicate.And(t => t.Version > 0);
            predicate = predicate.Or(t => t.CreateUser == user.UserName);
            var ulist = base.GetAll(predicate);
            if (version != null)
            {
                ulist = ulist.Where(t => t.Version == version);
            }
            if (!string.IsNullOrEmpty(borrownname) && !string.IsNullOrWhiteSpace(borrownname))
            {
                ulist = ulist.Where(t => t.BorrowerName.Contains(borrownname));
            }
            if (!string.IsNullOrEmpty(caseNum) && !string.IsNullOrWhiteSpace(caseNum))
            {
                ulist = ulist.Where(t => t.NewCaseNum.Contains(caseNum));
            }
            ulist = ulist.OrderByDescending(t => t.CaseNum);

            return ulist;
        }

        #region 进件提交审核

        /// <summary>
        /// 进件提交审核
        /// </summary>
        /// <param name="caseId">进件ID</param>
        /// <param name="creatUser">创建人</param>
        /// <returns></returns>

        public bool SubmitBaseCase(string caseId, string creatUser)
        {
            var baseCase = Get(caseId);//进件信息

            if (baseCase != null)
            {
                BaseAuditDAL baseAuditDal = new BaseAuditDAL();

                #region 审核主表信息

                BaseAudit baseAudit = new BaseAudit();

                baseAudit.ID = Guid.NewGuid().ToString();
                baseAudit.Version = 0;
                baseAudit.BorrowerName = baseCase.BorrowerName;
                baseAudit.CaseNum = baseCase.CaseNum;
                baseAudit.NewCaseNum = baseCase.NewCaseNum;
                baseAudit.AnnualRate = baseCase.AnnualRate;
                baseAudit.CaseType = baseCase.CaseType;
                baseAudit.CreateTime = DateTime.Now;
                baseAudit.CreateUser = creatUser;
                baseAudit.LoanAmount = baseCase.LoanAmount;
                baseAudit.DistrictID = baseCase.DistrictID;
                baseAudit.SalesGroupID = baseCase.SalesGroupID;
                baseAudit.SalesID = baseCase.SalesID;
                baseAudit.CaseStatus = CaseStatus.FirstAudit;
                baseAudit.Term = baseCase.Term;
                baseAudit.OpeningBank = baseCase.OpeningBank;
                baseAudit.OpeningSite = baseCase.OpeningSite;
                baseAudit.BankCard = baseCase.BankCard;
                baseAudit.ServiceCharge = baseCase.ServiceCharge;
                baseAudit.ServiceChargeRate = baseCase.ServiceChargeRate;
                baseAudit.Deposit = baseCase.Deposit;
                baseAudit.DepositDate = baseCase.DepositDate;
                baseAudit.IsActivitieRate = baseCase.IsActivitieRate;
                baseAudit.PaymentFactor = baseCase.PaymentFactor;//还款来源
                baseAudit.Purpose = baseCase.Purpose;//借款用途
                baseAuditDal.Add(baseAudit);

                #endregion 审核主表信息

                #region 保存抵押物信息

                SaveCollaterals<CollateralAudit>(baseCase.Collaterals, baseAudit.ID);

                #endregion 保存抵押物信息

                #region 保存关系人信息集合

                SaveRelationPersons<RelationPersonAudit>(baseCase.RelationPersons, baseAudit.ID);

                #endregion 保存关系人信息集合

                #region 保存介绍人信息集合

                SaveIntroducers<IntroducerAudit>(baseCase.Introducers, baseAudit.ID);

                #endregion 保存介绍人信息集合

                baseAuditDal.AcceptAllChange();

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 保存抵押物信息
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public void SaveCollaterals<T>(ICollection<Collateral> collaterals, string auditId)
            where T : class, new()
        {
            var type = typeof(T);
            if (type.Equals(typeof(CollateralAudit)))
            {
                CollateralAuditDAL caDAL = new CollateralAuditDAL();
                foreach (var model in collaterals)
                {
                    #region 抵押物信息

                    CollateralAudit caModel = new CollateralAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        AuditID = auditId,
                        CollateralType = model.CollateralType,
                        HouseNumber = model.HouseNumber,
                        BuildingName = model.BuildingName,
                        Address = model.Address,
                        RightOwner = model.RightOwner,
                        HouseSize = model.HouseSize,
                        Sequence = model.Sequence,
                        CompletionDate = model.CompletionDate,
                        LandType = model.LandType,
                        HouseType = model.HouseType,
                        TotalHeight = model.TotalHeight
                };

                    #endregion 抵押物信息

                    #region 上传房屋文件保存

                    caModel.HouseFile = SaveFiles(model.HouseFile, caModel.ID, auditId);

                    #endregion 上传房屋文件保存

                    caDAL.Add(caModel); //保存抵押物信息
                }
            }
            else if (type.Equals(typeof(Collateral)))
            {
                CollateralDAL caDAL = new CollateralDAL();
                foreach (var model in collaterals)
                {
                    #region 抵押物信息

                    Collateral caModel = new Collateral()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CollateralType = model.CollateralType,
                        HouseNumber = model.HouseNumber,
                        BuildingName = model.BuildingName,
                        Address = model.Address,
                        RightOwner = model.RightOwner,
                        HouseSize = model.HouseSize,
                        CaseID = auditId,
                        Sequence = model.Sequence,
                        CompletionDate = model.CompletionDate,
                        LandType = model.LandType,
                        HouseType = model.HouseType,
                        TotalHeight = model.TotalHeight
                    };

                    #endregion 抵押物信息

                    #region 上传房屋文件保存

                    caModel.HouseFile = SaveFiles(model.HouseFile, caModel.ID, auditId);

                    #endregion 上传房屋文件保存

                    caDAL.Add(caModel); //保存抵押物信息
                }
            }
        }

        public void SaveCollaterals(ICollection<Collateral> collaterals, string auditId)
        {
            CollateralAuditDAL caDAL = new CollateralAuditDAL();
            foreach (var model in collaterals)
            {
                #region 抵押物信息

                CollateralAudit caModel = new CollateralAudit();
                caModel.ID = Guid.NewGuid().ToString();
                caModel.AuditID = auditId;
                caModel.CollateralType = model.CollateralType;
                caModel.HouseNumber = model.HouseNumber;

                caModel.BuildingName = model.BuildingName;
                caModel.Address = model.Address;
                caModel.RightOwner = model.RightOwner;
                caModel.HouseSize = model.HouseSize;
                caModel.Sequence = model.Sequence;

                #endregion 抵押物信息

                #region 上传房屋文件保存

                caModel.HouseFile = SaveFiles(model.HouseFile, caModel.ID, auditId);

                #endregion 上传房屋文件保存

                caDAL.Add(caModel); //保存抵押物信息
            }
        }

        /// <summary>
        /// 保存介绍人信息
        /// </summary>
        /// <param name="Collaterals"></param>
        /// <param name="auditId"></param>
        public void SaveIntroducers(ICollection<Introducer> collaterals, string auditId)
        {
            IntroducerAuditDAL IntroducerDAL = new IntroducerAuditDAL();
            foreach (var model in collaterals)
            {
                #region 介绍人信息

                IntroducerAudit IntroducerModel = new IntroducerAudit();
                IntroducerModel.ID = Guid.NewGuid().ToString();
                IntroducerModel.AuditID = auditId;
                IntroducerModel.Name = model.Name;
                IntroducerModel.Contract = model.Contract;
                IntroducerModel.RebateAmmount = model.RebateAmmount;
                IntroducerModel.RebateRate = model.RebateRate;
                IntroducerModel.Account = model.Account;
                IntroducerModel.AccountBank = model.AccountBank;
                IntroducerModel.Sequence = model.Sequence;

                #endregion 介绍人信息

                IntroducerDAL.Add(IntroducerModel); //保存介绍人信息
            }
        }

        public void SaveIntroducers<T>(ICollection<Introducer> collaterals, string auditId)
            where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(IntroducerAudit)))
            {
                IntroducerAuditDAL IntroducerDAL = new IntroducerAuditDAL();
                foreach (var model in collaterals)
                {
                    #region 介绍人信息

                    IntroducerAudit IntroducerModel = new IntroducerAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        AuditID = auditId,
                        Name = model.Name,
                        Contract = model.Contract,
                        RebateAmmount = model.RebateAmmount,
                        RebateRate = model.RebateRate,
                        Account = model.Account,
                        AccountBank = model.AccountBank,
                        Sequence = model.Sequence
                    };

                    #endregion 介绍人信息

                    IntroducerDAL.Add(IntroducerModel); //保存介绍人信息
                }
            }
            else if (type.Equals(typeof(Introducer)))
            {
                IntroducerDAL IntroducerDAL = new IntroducerDAL();
                foreach (var model in collaterals)
                {
                    #region 介绍人信息

                    Introducer IntroducerModel = new Introducer()
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Contract = model.Contract,
                        RebateAmmount = model.RebateAmmount,
                        RebateRate = model.RebateRate,
                        Account = model.Account,
                        AccountBank = model.AccountBank,
                        CaseID = auditId,
                        Sequence = model.Sequence
                    };

                    #endregion 介绍人信息

                    IntroducerDAL.Add(IntroducerModel); //保存介绍人信息
                }
            }
        }

        /// <summary>
        /// 保存关系人信息集合
        /// </summary>
        /// <param name="RelationPersons"></param>
        /// <param name="auditId"></param>
        public void SaveRelationPersons(ICollection<RelationPerson> relationPersons, string auditId)
        {
            RelationPersonAuditDAL rpDAL = new RelationPersonAuditDAL();
            foreach (var model in relationPersons)
            {
                RelationPersonAudit rpModel = new RelationPersonAudit();

                #region 关系人信息集合

                rpModel.ID = Guid.NewGuid().ToString();
                rpModel.AuditID = auditId;
                rpModel.RelationType = model.RelationType;
                rpModel.BorrowerRelation = model.BorrowerRelation;
                rpModel.Name = model.Name;
                rpModel.IdentificationType = model.IdentificationType;
                rpModel.IdentificationNumber = model.IdentificationNumber;
                rpModel.ExpiryDate = model.ExpiryDate;
                rpModel.Birthday = model.Birthday;
                rpModel.MaritalStatus = model.MaritalStatus;
                rpModel.SalaryDescription = model.SalaryDescription;
                rpModel.Warranty = model.Warranty;
                rpModel.IsCoBorrower = model.IsCoBorrower;
                rpModel.Sequence = model.Sequence;

                //  上传身份证复印件
                rpModel.IdentificationFile = SaveFiles(model.IdentificationFile, rpModel.ID, auditId);

                // 上传婚姻证明文件保存
                rpModel.MarryFile = SaveFiles(model.MarryFile, rpModel.ID, auditId);

                // 上传单身证明文件保存
                rpModel.SingleFile = SaveFiles(model.SingleFile, rpModel.ID, auditId);

                //  上传出生证明文件保存
                rpModel.BirthFile = SaveFiles(model.BirthFile, rpModel.ID, auditId);

                //  上传户口本复印件文件保存
                rpModel.AccountFile = SaveFiles(model.AccountFile, rpModel.ID, auditId);

                // 上传收入证明（受薪水人士）文件保存
                rpModel.SalaryPersonFile = SaveFiles(model.SalaryPersonFile, rpModel.ID, auditId);

                //  上传收入证明（自雇有执照）文件保存
                rpModel.SelfLicenseFile = SaveFiles(model.SelfLicenseFile, rpModel.ID, auditId);

                // 上传收入证明（自雇无执照）文件保存
                rpModel.SelfNonLicenseFile = SaveFiles(model.SelfNonLicenseFile, rpModel.ID, auditId);
                // 银行流水
                rpModel.BankFlowFile = SaveFiles(model.BankFlowFile, rpModel.ID, auditId);
                // 个人征信
                rpModel.IndividualFile = SaveFiles(model.IndividualFile, rpModel.ID, auditId);
                // 其他证明
                rpModel.OtherFile = SaveFiles(model.OtherFile, rpModel.ID, auditId);
                rpDAL.Add(rpModel);

                #endregion 关系人信息集合

                #region 保存关系人地址信息集合

                SaveAddresses(model.Addresses, rpModel.ID);

                #endregion 保存关系人地址信息集合

                #region 保存关系人紧急联系人信息集合

                SaveEmergencyContacts(model.EmergencyContacts, rpModel.ID);

                #endregion 保存关系人紧急联系人信息集合

                #region 保存关系人联系方式信息集合

                SaveContacts(model.Contacts, rpModel.ID);

                #endregion 保存关系人联系方式信息集合

                #region 关系人相关企业信息集合

                SaveRelationEnterprises<RelationEnterpriseAudit>(model.RelationEnterprises, rpModel.ID, auditId);

                #endregion 关系人相关企业信息集合
            }
        }

        public void SaveRelationPersons<T>(ICollection<RelationPerson> relationPersons, string auditId)
         where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(RelationPersonAudit)))
            {
                RelationPersonAuditDAL rpDAL = new RelationPersonAuditDAL();
                foreach (var model in relationPersons)
                {
                    #region 关系人信息集合

                    RelationPersonAudit rpModel = new RelationPersonAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        AuditID = auditId,
                        RelationType = model.RelationType,
                        BorrowerRelation = model.BorrowerRelation,
                        Name = model.Name,
                        IdentificationType = model.IdentificationType,
                        IdentificationNumber = model.IdentificationNumber,
                        ExpiryDate = model.ExpiryDate,
                        Birthday = model.Birthday,
                        MaritalStatus = model.MaritalStatus,
                        SalaryDescription = model.SalaryDescription,
                        Warranty = model.Warranty,
                        IsCoBorrower = model.IsCoBorrower,
                        Sequence = model.Sequence
                    };
                    //  上传身份证复印件
                    rpModel.IdentificationFile = SaveFiles(model.IdentificationFile, rpModel.ID, auditId);
                    // 上传婚姻证明文件保存
                    rpModel.MarryFile = SaveFiles(model.MarryFile, rpModel.ID, auditId);
                    // 上传单身证明文件保存
                    rpModel.SingleFile = SaveFiles(model.SingleFile, rpModel.ID, auditId);
                    //  上传出生证明文件保存
                    rpModel.BirthFile = SaveFiles(model.BirthFile, rpModel.ID, auditId);
                    //  上传户口本复印件文件保存
                    rpModel.AccountFile = SaveFiles(model.AccountFile, rpModel.ID, auditId);
                    // 上传收入证明（受薪水人士）文件保存
                    rpModel.SalaryPersonFile = SaveFiles(model.SalaryPersonFile, rpModel.ID, auditId);
                    //  上传收入证明（自雇有执照）文件保存
                    rpModel.SelfLicenseFile = SaveFiles(model.SelfLicenseFile, rpModel.ID, auditId);
                    // 上传收入证明（自雇无执照）文件保存
                    rpModel.SelfNonLicenseFile = SaveFiles(model.SelfNonLicenseFile, rpModel.ID, auditId);
                    // 银行流水
                    rpModel.BankFlowFile = SaveFiles(model.BankFlowFile, rpModel.ID, auditId);
                    // 个人征信
                    rpModel.IndividualFile = SaveFiles(model.IndividualFile, rpModel.ID, auditId);
                    // 其他证明
                    rpModel.OtherFile = SaveFiles(model.OtherFile, rpModel.ID, auditId);
                    rpDAL.Add(rpModel);

                    #endregion 关系人信息集合

                    #region 保存关系人地址信息集合

                    SaveAddresses<AddressAudit>(model.Addresses, rpModel.ID);

                    #endregion 保存关系人地址信息集合

                    #region 保存关系人紧急联系人信息集合

                    SaveEmergencyContacts<EmergencyContactAudit>(model.EmergencyContacts, rpModel.ID);

                    #endregion 保存关系人紧急联系人信息集合

                    #region 保存关系人联系方式信息集合

                    SaveContacts<ContactAudit>(model.Contacts, rpModel.ID);

                    #endregion 保存关系人联系方式信息集合

                    #region 关系人相关企业信息集合

                    SaveRelationEnterprises<RelationEnterpriseAudit>(model.RelationEnterprises, rpModel.ID, auditId);

                    #endregion 关系人相关企业信息集合
                }
            }
            else if (type.Equals(typeof(RelationPerson)))
            {
                RelationPersonDAL rpDAL = new RelationPersonDAL();
                foreach (var model in relationPersons)
                {
                    #region 关系人信息集合

                    RelationPerson rpModel = new RelationPerson()
                    {
                        ID = Guid.NewGuid().ToString(),
                        RelationType = model.RelationType,
                        BorrowerRelation = model.BorrowerRelation,
                        Name = model.Name,
                        IdentificationType = model.IdentificationType,
                        IdentificationNumber = model.IdentificationNumber,
                        ExpiryDate = model.ExpiryDate,
                        Birthday = model.Birthday,
                        MaritalStatus = model.MaritalStatus,
                        SalaryDescription = model.SalaryDescription,
                        Warranty = model.Warranty,
                        IsCoBorrower = model.IsCoBorrower,
                        CaseID = auditId,
                        Sequence = model.Sequence
                    };
                    //  上传身份证复印件
                    rpModel.IdentificationFile = SaveFiles(model.IdentificationFile, rpModel.ID, auditId);
                    // 上传婚姻证明文件保存
                    rpModel.MarryFile = SaveFiles(model.MarryFile, rpModel.ID, auditId);
                    // 上传单身证明文件保存
                    rpModel.SingleFile = SaveFiles(model.SingleFile, rpModel.ID, auditId);
                    //  上传出生证明文件保存
                    rpModel.BirthFile = SaveFiles(model.BirthFile, rpModel.ID, auditId);
                    //  上传户口本复印件文件保存
                    rpModel.AccountFile = SaveFiles(model.AccountFile, rpModel.ID, auditId);
                    // 上传收入证明（受薪水人士）文件保存
                    rpModel.SalaryPersonFile = SaveFiles(model.SalaryPersonFile, rpModel.ID, auditId);
                    //  上传收入证明（自雇有执照）文件保存
                    rpModel.SelfLicenseFile = SaveFiles(model.SelfLicenseFile, rpModel.ID, auditId);
                    // 上传收入证明（自雇无执照）文件保存
                    rpModel.SelfNonLicenseFile = SaveFiles(model.SelfNonLicenseFile, rpModel.ID, auditId);
                    // 银行流水
                    rpModel.BankFlowFile = SaveFiles(model.BankFlowFile, rpModel.ID, auditId);
                    // 个人征信
                    rpModel.IndividualFile = SaveFiles(model.IndividualFile, rpModel.ID, auditId);
                    // 其他证明
                    rpModel.OtherFile = SaveFiles(model.OtherFile, rpModel.ID, auditId);
                    rpDAL.Add(rpModel);

                    #endregion 关系人信息集合

                    #region 保存关系人地址信息集合

                    SaveAddresses<Address>(model.Addresses, rpModel.ID);

                    #endregion 保存关系人地址信息集合

                    #region 保存关系人紧急联系人信息集合

                    SaveEmergencyContacts<EmergencyContact>(model.EmergencyContacts, rpModel.ID);

                    #endregion 保存关系人紧急联系人信息集合

                    #region 保存关系人联系方式信息集合

                    SaveContacts<Contact>(model.Contacts, rpModel.ID);

                    #endregion 保存关系人联系方式信息集合

                    #region 关系人相关企业信息集合

                    SaveRelationEnterprises<RelationEnterprise>(model.RelationEnterprises, rpModel.ID, auditId);

                    #endregion 关系人相关企业信息集合
                }
            }
        }

        private string SaveFiles(string filenames, string linkid, string linkkey)
        {
            Infrastructure.File.FileUpload up = new Infrastructure.File.FileUpload();//文件上传
            string files = string.Empty;
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
                    entity.LinkID = Guid.Parse(linkid);
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

        /// <summary>
        /// 保存关系人地址信息集合
        /// </summary>
        /// <param name="address"></param>
        /// <param name="personId"></param>
        public void SaveAddresses(ICollection<Address> address, string personId)
        {
            AddressAuditDAL adal = new AddressAuditDAL();
            foreach (var addressmodel in address)
            {
                AddressAudit aModel = new AddressAudit();
                aModel.ID = Guid.NewGuid().ToString();
                aModel.AddressInfo = addressmodel.AddressInfo;
                aModel.AddressType = addressmodel.AddressType;
                aModel.PersonID = personId;
                aModel.IsDefault = addressmodel.IsDefault;
                aModel.Sequence = addressmodel.Sequence;

                adal.Add(aModel);
            }
        }

        public void SaveAddresses<T>(ICollection<Address> address, string personId)
            where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(AddressAudit)))
            {
                AddressAuditDAL adal = new AddressAuditDAL();
                foreach (var addressmodel in address)
                {
                    AddressAudit aModel = new AddressAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        AddressInfo = addressmodel.AddressInfo,
                        AddressType = addressmodel.AddressType,
                        PersonID = personId,
                        IsDefault = addressmodel.IsDefault,
                        Sequence = addressmodel.Sequence
                    };

                    adal.Add(aModel);
                }
            }
            else if (type.Equals(typeof(Address)))
            {
                AddressDAL adal = new AddressDAL();
                foreach (var addressmodel in address)
                {
                    Address aModel = new Address()
                    {
                        ID = Guid.NewGuid().ToString(),
                        AddressInfo = addressmodel.AddressInfo,
                        AddressType = addressmodel.AddressType,
                        PersonID = personId,
                        IsDefault = addressmodel.IsDefault,
                        Sequence = addressmodel.Sequence
                    };
                    adal.Add(aModel);
                }
            }
        }

        /// <summary>
        /// 保存关系人紧急联系人信息集合
        /// </summary>
        /// <param name="emergencyContacts"></param>
        /// <param name="personId"></param>
        public void SaveEmergencyContacts(ICollection<EmergencyContact> emergencyContacts, string personId)
        {
            EmergencyContactAuditDAL ecdal = new EmergencyContactAuditDAL();
            foreach (var ecmodel in emergencyContacts)
            {
                EmergencyContactAudit ecModel = new EmergencyContactAudit();
                ecModel.ID = Guid.NewGuid().ToString();
                ecModel.ContactType = ecmodel.ContactType;
                ecModel.ContactNumber = ecmodel.ContactNumber;
                ecModel.Name = ecmodel.Name;
                ecModel.PersonID = personId;
                ecmodel.Sequence = ecmodel.Sequence;

                ecdal.Add(ecModel);
            }
        }

        public void SaveEmergencyContacts<T>(ICollection<EmergencyContact> emergencyContacts, string personId)
         where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(EmergencyContactAudit)))
            {
                EmergencyContactAuditDAL ecdal = new EmergencyContactAuditDAL();
                foreach (var ecmodel in emergencyContacts)
                {
                    EmergencyContactAudit ecModel = new EmergencyContactAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ContactType = ecmodel.ContactType,
                        ContactNumber = ecmodel.ContactNumber,
                        Name = ecmodel.Name,
                        PersonID = personId,
                        Sequence = ecmodel.Sequence
                    };
                    ecdal.Add(ecModel);
                }
            }
            else if (type.Equals(typeof(EmergencyContact)))
            {
                EmergencyContactDAL ecdal = new EmergencyContactDAL();
                foreach (var ecmodel in emergencyContacts)
                {
                    EmergencyContact ecModel = new EmergencyContact()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ContactType = ecmodel.ContactType,
                        ContactNumber = ecmodel.ContactNumber,
                        Name = ecmodel.Name,
                        PersonID = personId,
                        Sequence = ecmodel.Sequence
                    };
                    ecdal.Add(ecModel);
                }
            }
        }

        /// <summary>
        /// 保存关系人联系方式信息集合
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="personId"></param>
        public void SaveContacts(ICollection<Contact> contacts, string personId)
        {
            ContactAuditDAL cdal = new ContactAuditDAL();
            foreach (var cmodel in contacts)
            {
                ContactAudit cModel = new ContactAudit();
                cModel.ID = Guid.NewGuid().ToString();
                cModel.ContactType = cmodel.ContactType;
                cModel.ContactNumber = cmodel.ContactNumber;
                cModel.PersonID = personId;
                cModel.IsDefault = cmodel.IsDefault;
                cModel.Sequence = cmodel.Sequence;

                cdal.Add(cModel);
            }
        }

        public void SaveContacts<T>(ICollection<Contact> contacts, string personId)
       where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(ContactAudit)))
            {
                ContactAuditDAL cdal = new ContactAuditDAL();
                foreach (var cmodel in contacts)
                {
                    ContactAudit cModel = new ContactAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ContactType = cmodel.ContactType,
                        ContactNumber = cmodel.ContactNumber,
                        PersonID = personId,
                        IsDefault = cmodel.IsDefault,
                        Sequence = cmodel.Sequence
                    };
                    cdal.Add(cModel);
                }
            }
            else if (type.Equals(typeof(Contact)))
            {
                ContactDAL cdal = new ContactDAL();
                foreach (var cmodel in contacts)
                {
                    Contact cModel = new Contact()
                    {
                        ID = Guid.NewGuid().ToString(),
                        ContactType = cmodel.ContactType,
                        ContactNumber = cmodel.ContactNumber,
                        PersonID = personId,
                        IsDefault = cmodel.IsDefault,
                        Sequence = cmodel.Sequence
                    };
                    cdal.Add(cModel);
                }
            }
        }

        /// <summary>
        /// 关系人相关企业信息集合
        /// </summary>
        /// <param name="relationEnterprises"></param>
        /// <param name="personId"></param>
        public void SaveRelationEnterprises(ICollection<RelationEnterprise> relationEnterprises, string personId, string auditId)
        {
            RelationEnterpriseAuditDAL cdal = new RelationEnterpriseAuditDAL();
            foreach (var remodel in relationEnterprises)
            {
                RelationEnterpriseAudit reModel = new RelationEnterpriseAudit();
                reModel.ID = Guid.NewGuid().ToString();
                reModel.EnterpriseDes = remodel.EnterpriseDes;
                reModel.EnterpriseName = remodel.EnterpriseName;
                reModel.RegisterNumber = remodel.RegisterNumber;
                reModel.LegalPerson = remodel.LegalPerson;
                reModel.ShareholderDetails = remodel.ShareholderDetails;
                reModel.Address = remodel.Address;
                reModel.RegisteredCapital = remodel.RegisteredCapital;
                reModel.MainBusiness = remodel.MainBusiness;
                // 银行流水
                reModel.BankFlowFile = SaveFiles(remodel.BankFlowFile, reModel.ID, auditId);
                // 个人征信
                reModel.IndividualFile = SaveFiles(remodel.IndividualFile, reModel.ID, auditId);
                reModel.PersonID = personId;
                reModel.Sequence = remodel.Sequence;

                cdal.Add(reModel);
            }
        }

        public void SaveRelationEnterprises<T>(ICollection<RelationEnterprise> relationEnterprises, string personId, string auditId)
         where T : class, new()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(RelationEnterpriseAudit)))
            {
                RelationEnterpriseAuditDAL cdal = new RelationEnterpriseAuditDAL();
                foreach (var remodel in relationEnterprises)
                {
                    RelationEnterpriseAudit reModel = new RelationEnterpriseAudit()
                    {
                        ID = Guid.NewGuid().ToString(),
                        EnterpriseDes = remodel.EnterpriseDes,
                        EnterpriseName = remodel.EnterpriseName,
                        RegisterNumber = remodel.RegisterNumber,
                        LegalPerson = remodel.LegalPerson,
                        ShareholderDetails = remodel.ShareholderDetails,
                        Address = remodel.Address,
                        RegisteredCapital = remodel.RegisteredCapital,
                        MainBusiness = remodel.MainBusiness,
                        PersonID = personId,
                        Sequence = remodel.Sequence
                    };
                    // 银行流水
                    reModel.BankFlowFile = SaveFiles(remodel.BankFlowFile, reModel.ID, auditId);
                    // 个人征信
                    reModel.IndividualFile = SaveFiles(remodel.IndividualFile, reModel.ID, auditId);
                    cdal.Add(reModel);
                }
            }
            else if (type.Equals(typeof(RelationEnterprise)))
            {
                RelationEnterpriseDAL cdal = new RelationEnterpriseDAL();
                foreach (var remodel in relationEnterprises)
                {
                    RelationEnterprise reModel = new RelationEnterprise()
                    {
                        ID = Guid.NewGuid().ToString(),
                        EnterpriseDes = remodel.EnterpriseDes,
                        EnterpriseName = remodel.EnterpriseName,
                        RegisterNumber = remodel.RegisterNumber,
                        LegalPerson = remodel.LegalPerson,
                        ShareholderDetails = remodel.ShareholderDetails,
                        Address = remodel.Address,
                        RegisteredCapital = remodel.RegisteredCapital,
                        MainBusiness = remodel.MainBusiness,
                        PersonID = personId,
                        Sequence = remodel.Sequence
                    };
                    // 银行流水
                    reModel.BankFlowFile = SaveFiles(remodel.BankFlowFile, reModel.ID, auditId);
                    // 个人征信
                    reModel.IndividualFile = SaveFiles(remodel.IndividualFile, reModel.ID, auditId);
                    cdal.Add(reModel);
                }
            }
        }

        #endregion 进件提交审核

        #region 拷贝进件

        public BaseCase CopyBaseCase(string ID, Com.HSJF.Infrastructure.Identity.Model.User user)
        {
            var basecase = Get(ID);//进件信息
            if (basecase != null)
            {
                BaseCaseDAL baDal = new BaseCaseDAL();

                #region 主表信息

                BaseCase baModel = new BaseCase()
                {
                    ID = Guid.NewGuid().ToString(),
                    Version = 0,
                    BorrowerName = basecase.BorrowerName,
                    //CaseNum = basecase.CaseNum,
                    CaseType = basecase.CaseType,
                    CreateTime = DateTime.Now,
                    CreateUser = user.UserName,
                    LoanAmount = basecase.LoanAmount,
                    DistrictID = basecase.DistrictID,
                    SalesGroupID = basecase.SalesGroupID,
                    SalesID = basecase.SalesID,
                    Term = basecase.Term,
                    OpeningBank = basecase.OpeningBank,
                    OpeningSite = basecase.OpeningSite,
                    BankCard = basecase.BankCard,
                    ServiceCharge = basecase.ServiceCharge,
                    ServiceChargeRate = basecase.ServiceChargeRate,
                    Deposit = basecase.Deposit,
                    DepositDate = basecase.DepositDate,
                    IsActivitieRate = basecase.IsActivitieRate,
                    //年华利率，原本在审核中，现在移动到进件中
                    PaymentFactor = basecase.PaymentFactor,//还款来源
                    Purpose = basecase.Purpose,//借款用途
                    AnnualRate = basecase.AnnualRate
                };
                baDal.Add(baModel);

                #endregion 主表信息

                #region 保存抵押物信息

                SaveCollaterals<Collateral>(basecase.Collaterals, baModel.ID);

                #endregion 保存抵押物信息

                #region 保存关系人信息集合

                SaveRelationPersons<RelationPerson>(basecase.RelationPersons, baModel.ID);

                #endregion 保存关系人信息集合

                #region 保存介绍人信息集合

                SaveIntroducers<Introducer>(basecase.Introducers, baModel.ID);

                #endregion 保存介绍人信息集合

                baDal.AcceptAllChange();
                return baModel;
            }
            else
            {
                return null;
            }
        }

        #endregion 拷贝进件
    }
}