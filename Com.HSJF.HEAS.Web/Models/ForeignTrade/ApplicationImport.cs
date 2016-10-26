using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.Web.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.HSJF.HEAS.Web.Models.ForeignTrade
{
    /// <summary>
    /// 外贸  进件实体类
    /// </summary>
    public class ApplicationImport
    {
        #region Properties

        /// <summary>
        /// 合作机构号  Y  A  10
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(10, ErrorMessage = "MaxLength is 10")]
        public string brNo { get; set; }

        /// <summary>
        /// 批次号码  Y   A   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string batNo { get; set; }

        /// <summary>
        /// 记录数   Y   N   4[1-1000]
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [Range(1, 1000, ErrorMessage = "Range is 1 to 1000")]
        public int dataCnt { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public List<ImportData> importDatas { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseAuditList"></param>
        public ApplicationImport(List<BaseAuditViewModel> baseAuditList)
        {
            this.brNo = "?";
            this.batNo = "?";
            this.dataCnt = baseAuditList.Count;
            this.importDatas = new List<ImportData>();

            foreach (BaseAuditViewModel model in baseAuditList)
            {
                this.importDatas.Add(SetDataValue(model));
            }
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <returns></returns>
        public ImportData SetDataValue(BaseAuditViewModel baseAudit)
        {
            MortgageBll mortage = new MortgageBll();

            ImportData importData = new ImportData();

            RelationPersonAuditViewModel ownPerson = baseAudit.RelationPersonAudits == null ? null : baseAudit.RelationPersonAudits.SingleOrDefault(S => S.Name == baseAudit.BorrowerName);
            PublicMortgage mortgage = mortage.Query(baseAudit.CaseNum);

            if (mortgage != null)
            {
                importData.pactNo = mortgage.ContractNo;
                importData.pactAmt = mortgage.ContractAmount == null ? 0 : Convert.ToDecimal(mortage.Query(baseAudit.CaseNum).ContractAmount);
                importData.endDate = mortgage.ContractDate.ToString();
            }
            else
            {
                importData.pactNo = "";
                importData.pactAmt = 0;
            }

            if (ownPerson != null)
            {
                importData.custName = baseAudit.BorrowerName;
                importData.idType = ownPerson.IdentificationType;
                importData.idNo = ownPerson.IdentificationNumber;
                importData.birth = ownPerson.Birthday.ToString();
                importData.marriage = ownPerson.MaritalStatus;

                if (ownPerson.ContactAudits != null)
                {
                    if (ownPerson.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-HomeNumber") != null)
                    {
                        importData.telNo = ownPerson.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-HomeNumber").ContactNumber;
                        importData.homeTel = ownPerson.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-HomeNumber").ContactNumber;
                    }
                    else
                    {
                        importData.telNo = "";
                    }

                    if (ownPerson.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone") != null)
                    {
                        importData.phoneNo = ownPerson.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone").ContactNumber;
                    }
                    else
                    {
                        importData.phoneNo = "";
                    }
                }

                if (ownPerson.AddressAudits != null && ownPerson.AddressAudits.SingleOrDefault(S => S.AddressType == "-AddressType-HomeAddress") != null)
                {
                    importData.postAddr = ownPerson.AddressAudits.SingleOrDefault(S => S.AddressType == "-AddressType-HomeAddress").AddressInfo;
                    importData.homeAddr = ownPerson.AddressAudits.SingleOrDefault(S => S.AddressType == "-AddressType-HomeAddress").AddressInfo;
                }

                if (baseAudit.RelationPersonAudits!=null && baseAudit.RelationPersonAudits.SingleOrDefault(S => S.RelationType == "-PersonType-JieKuanRenPeiOu")!=null)
                {
                    RelationPersonAuditViewModel peiou = baseAudit.RelationPersonAudits.SingleOrDefault(S => S.RelationType == "-PersonType-JieKuanRenPeiOu");

                    importData.mateName = peiou.Name;
                    importData.mateIdtype = peiou.IdentificationType;
                    importData.mateIdno = peiou.IdentificationNumber;

                    if (peiou.ContactAudits != null && peiou.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone") != null)
                    {
                        importData.mateTel = peiou.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone").ContactNumber;
                    }
                }
            }

            importData.custType = "99";
            importData.sex = "9";
            importData.edu = "99";
            importData.degree = "9";
            importData.homeArea = "000000";
            importData.income = 0;
            importData.projNo = "";
            importData.prdtNo = "";
            importData.feeTotal = 0;
            importData.lnRate = baseAudit.AuditRate == null ? 0 : Convert.ToDecimal(baseAudit.AuditRate);
            importData.appArea = "000000";
            importData.appUse = baseAudit.Purpose;
            importData.termMon = 0;
            importData.termDay = 0;
            importData.vouType = "2";
            importData.payType = "01";
            importData.vonAmt = 0;

            #region 账户

            List<ImportAccount> accountList = new List<ImportAccount>();
            decimal acAmt = mortgage == null ? 0 : mortgage.ContractAmount == null ? 0 : Convert.ToDecimal(mortgage.ContractAmount);

            accountList.Add(new ImportAccount()
            {
                acUse = "1",
                acAmt = acAmt,
                acType = "?",
                acno = baseAudit.BankCard,
                acName = baseAudit.OpeningSite,
                bankCode = "",
                bankSite = baseAudit.OpeningBank
            });
            accountList.Add(new ImportAccount()
            {
                acUse = "2",
                acAmt = acAmt,
                acType = "?",
                acno = baseAudit.BankCard,
                acName = baseAudit.OpeningSite,
                bankCode = "",
                bankSite = baseAudit.OpeningBank
            });

            importData.importAccounts = accountList;

            #endregion

            #region 押品

            List<Gage> gageList = new List<Gage>();

            if (baseAudit.CollateralAudits != null)
            {
                foreach (CollateralAuditViewModel collateral in baseAudit.CollateralAudits)
                {
                    Gage gage = new Gage();

                    HouseDetailViewModel houseDetail = baseAudit.HouseDetails == null ? null : baseAudit.HouseDetails.SingleOrDefault(S => S.CollateralID == collateral.ID);

                    gage.gcustName = collateral.RightOwner;

                    if (baseAudit.RelationPersonAudits != null && baseAudit.RelationPersonAudits.SingleOrDefault(S => S.Name == collateral.RightOwner) != null)
                    {
                        gage.gcustIdtype = baseAudit.RelationPersonAudits.SingleOrDefault(S => S.Name == collateral.RightOwner).IdentificationType;
                        gage.gcustIdno = baseAudit.RelationPersonAudits.SingleOrDefault(S => S.Name == collateral.RightOwner).IdentificationNumber;
                    }

                    gage.gType = "213";
                    gage.gName = collateral.BuildingName;
                    gage.gDesc = "";

                    if (houseDetail != null && houseDetail.EstimateSources != null && houseDetail.EstimateSources.Count() > 0)
                    {
                        gage.gValue = Convert.ToDecimal((houseDetail.EstimateSources.Take(1) as EstimateSourceViewModel).RushEstimate);
                    }

                    gage.gLicno = collateral.HouseNumber;
                    gage.gLicType = "01";

                    gageList.Add(gage);
                }
            }
            
            importData.gages = gageList;

            #endregion

            #region 共同借款人 && 借款关联人

            List<listCom> coms = new List<listCom>();
            List<listRel> rels = new List<listRel>();

            var relationPersons= baseAudit.RelationPersonAudits;

            if(relationPersons!=null)
            {
                foreach (RelationPersonAuditViewModel item in relationPersons)
                {
                    if (item.IsCoBorrower == 1)
                    {
                        listCom com = new listCom();

                        com.comName = item.Name;
                        com.comIdtype = item.IdentificationType;
                        com.comIdno = item.IdentificationNumber;
                        if (item.ContactAudits != null && item.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone") != null)
                        {
                            com.comTel = item.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone").ContactNumber;
                        }
                        else
                        {
                            com.comTel = "";
                        }

                        coms.Add(com);

                        listRel rel = new listRel();

                        rel.relName = item.Name;
                        rel.relIdtype = item.IdentificationType;
                        rel.relIdno = item.IdentificationNumber;
                        if (item.ContactAudits != null && item.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone") != null)
                        {
                            rel.relTel = item.ContactAudits.SingleOrDefault(S => S.ContactType == "-ContactType-MobilePhone").ContactNumber;
                        }
                        else
                        {
                            rel.relTel = "";
                        }

                        rels.Add(rel);
                    }
                }
            }

            importData.listComs = coms;

            #endregion

            importData.ifCar = "2";
            importData.ifCarCred = "2";
            importData.ifRoom = "2";
            importData.ifMort = "2";
            importData.ifCard = "2";
            importData.cardAmt = 0;
            importData.ifApp = "2";
            importData.ifId = "2";
            importData.ifPact = "1";
            importData.prePactNo = "";

            return importData;
        }

        #endregion
    }

    /// <summary>
    /// ImportData
    /// </summary>
    public class ImportData
    {
        /// <summary>
        /// 合同号码  Y   A   40  PK
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(40, ErrorMessage = "MaxLength is 40")]
        public string pactNo { get; set; }

        /// <summary>
        /// 客户名称  Y   C   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string custName { get; set; }

        /// <summary>
        /// 证件类型  Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string idType { get; set; }

        /// <summary>
        /// 证件号码  Y   A   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string idNo { get; set; }

        /// <summary>
        /// 客户类型  Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string custType { get; set; }

        /// <summary>
        /// 性别    Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string sex { get; set; }

        /// <summary>
        /// 出生日期  Y   A   8
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(8, ErrorMessage = "MaxLength is 8")]
        public string birth { get; set; }

        /// <summary>
        /// 婚姻状况  Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string marriage { get; set; }

        /// <summary>
        /// 是否有子女 N   A   2
        /// </summary>
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string children { get; set; }

        /// <summary>
        /// 最高学历  Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string edu { get; set; }

        /// <summary>
        /// 最高学位  Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string degree { get; set; }

        /// <summary>
        /// 联系电话  Y   A   20
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(20, ErrorMessage = "MaxLength is 20")]
        public string telNo { get; set; }

        /// <summary>
        /// 手机号码  Y   A   15
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(15, ErrorMessage = "MaxLength is 15")]
        public string phoneNo { get; set; }

        /// <summary>
        /// 通讯邮编  N   A   6
        /// </summary>
        [MaxLength(6, ErrorMessage = "MaxLength is 6")]
        public string postCode { get; set; }

        /// <summary>
        /// 通讯地址  N   C   60
        /// </summary>
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string postAddr { get; set; }

        /// <summary>
        /// 户籍归属地  Y   A   6  默认000000中国
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(6, ErrorMessage = "MaxLength is 6")]
        public string homeArea { get; set; }

        /// <summary>
        /// 住宅电话  N   A   20
        /// </summary>
        [MaxLength(20, ErrorMessage = "MaxLength is 20")]
        public string homeTel { get; set; }

        /// <summary>
        /// 住宅邮编  N   A   6
        /// </summary>
        [MaxLength(6, ErrorMessage = "MaxLength is 6")]
        public string homeCode { get; set; }

        /// <summary>
        /// 住宅地址  N   C   60
        /// </summary>
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string homeAddr { get; set; }

        /// <summary>
        /// 居住状况  N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string homeSts { get; set; }

        /// <summary>
        /// 月收入（元）  Y   N   10
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(10, ErrorMessage = "MaxLength is 10")]
        public decimal income { get; set; }

        /// <summary>
        /// 配偶名称  N   C   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string mateName { get; set; }

        /// <summary>
        /// 配偶证件类型  N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string mateIdtype { get; set; }

        /// <summary>
        /// 配偶证件号码  N   A   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string mateIdno { get; set; }

        /// <summary>
        /// 配偶工作单位  N   C   60
        /// </summary>
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string mateWork { get; set; }

        /// <summary>
        /// 配偶联系电话  N   A   20
        /// </summary>
        [MaxLength(20, ErrorMessage = "MaxLength is 20")]
        public string mateTel { get; set; }

        /// <summary>
        /// 信托项目编号  Y   A   20
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(20, ErrorMessage = "MaxLength is 20")]
        public string projNo { get; set; }

        /// <summary>
        /// 产品号  Y   A   20
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(20, ErrorMessage = "MaxLength is 20")]
        public string prdtNo { get; set; }

        /// <summary>
        /// 合同金额  Y   N   16，2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public decimal pactAmt { get; set; }

        /// <summary>
        /// 趸交费总额  Y   N   16，2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public decimal feeTotal { get; set; }

        /// <summary>
        /// 利率（月）  Y   N   10，6
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public decimal lnRate { get; set; }

        /// <summary>
        /// 申请地点  Y   A   6   默认000000中国
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(6, ErrorMessage = "MaxLength is 6")]
        public string appArea { get; set; }

        /// <summary>
        /// 申请用途  Y   C   60
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string appUse { get; set; }

        /// <summary>
        /// 合同期限（月）  Y   N   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public int termMon { get; set; }

        /// <summary>
        /// 合同期限（日）  Y   N   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public int termDay { get; set; }

        /// <summary>
        /// 担保方式  Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string vouType { get; set; }

        /// <summary>
        /// 到期日期  N   A   8
        /// </summary>
        [MaxLength(8, ErrorMessage = "MaxLength is 8")]
        public string endDate { get; set; }

        /// <summary>
        /// 扣款日类型  Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string payType { get; set; }

        /// <summary>
        /// 扣款日期  N   N   2
        /// </summary>
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        [Range(1, 28, ErrorMessage = "Range is 1 to 28")]
        public int payDay { get; set; }

        /// <summary>
        /// 期缴（保）费金额  Y   N   16，2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        public decimal vonAmt { get; set; }

        /// <summary>
        /// 职业   N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string workType { get; set; }

        /// <summary>
        /// 工作单位名称   N   A   60
        /// </summary>
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string workName { get; set; }

        /// <summary>
        /// 工作单位所属行业   N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string workWay { get; set; }

        /// <summary>
        /// 工作单位邮编   N   A   6
        /// </summary>
        [MaxLength(6, ErrorMessage = "MaxLength is 6")]
        public string workCode { get; set; }

        /// <summary>
        /// 工作单位地址   N   A   60
        /// </summary>
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string workAddr { get; set; }

        /// <summary>
        /// 职务   N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string workDuty { get; set; }

        /// <summary>
        /// 职称   N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string workTitle { get; set; }

        /// <summary>
        /// 工作起始年份   N   A   4
        /// </summary>
        [MaxLength(4, ErrorMessage = "MaxLength is 4")]
        public string workYear { get; set; }

        /// <summary>
        /// 是否有车   Y   A   
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string ifCar { get; set; }

        /// <summary>
        /// 是否有按揭车贷   Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string ifCarCred { get; set; }

        /// <summary>
        /// 是否有房   Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string ifRoom { get; set; }

        /// <summary>
        /// 是否有按揭房贷   Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string ifMort { get; set; }

        /// <summary>
        /// 是否有贷记卡   Y   A   2
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string ifCard { get; set; }

        /// <summary>
        /// 贷记卡最低额度   Y   N   10
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(10, ErrorMessage = "MaxLength is 10")]
        public long cardAmt { get; set; }

        /// <summary>
        /// 是否填写申请表   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string ifApp { get; set; }

        /// <summary>
        /// 是否有身份证信息   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string ifId { get; set; }

        /// <summary>
        /// 是否可以签订合同   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string ifPact { get; set; }

        /// <summary>
        /// 预审批ID   Y   A   40
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(40, ErrorMessage = "MaxLength is 40")]
        public string prePactNo { get; set; }

        /// <summary>
        /// 查证流水号   N   A   40
        /// </summary>
        [MaxLength(40, ErrorMessage = "MaxLength is 40")]
        public string czPactNo { get; set; }    

        /// <summary>
        /// 工作状态   N   A   2
        /// </summary>
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string workSts { get; set; }     

        /// <summary>
        /// 
        /// </summary>
        public List<ImportAccount> importAccounts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Gage> gages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<listCom> listComs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<listRel> listRels { get; set; }
    }

    /// <summary>
    /// ImportAccount
    /// </summary>
    public class ImportAccount
    {
        /// <summary>
        /// 账户用途   Y   A   4
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(4, ErrorMessage = "MaxLength is 4")]
        public string acUse { get; set; }

        /// <summary>
        /// 放款金额   N   N   16，2
        /// </summary>
        public decimal acAmt { get; set; }

        /// <summary>
        /// 账户类型   Y   A   4
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(4, ErrorMessage = "MaxLength is 4")]
        public string acType { get; set; }

        /// <summary>
        /// 账户号   Y   A   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string acno { get; set; }

        /// <summary>
        /// 账户户名   Y   C   60
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(60, ErrorMessage = "MaxLength is 60")]
        public string acName { get; set; }

        /// <summary>
        /// 银行代码   Y   A   3
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(3, ErrorMessage = "MaxLength is 3")]
        public string bankCode { get; set; }

        /// <summary>
        /// 账户开户省   N   C   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string bankProv { get; set; }

        /// <summary>
        /// 账户开户市   N   C   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string bankCity { get; set; }

        /// <summary>
        /// 开户银行网点   N   C   80
        /// </summary>
        [MaxLength(80, ErrorMessage = "MaxLength is 80")]
        public string bankSite { get; set; }        
    }

    /// <summary>
    /// Gage
    /// </summary>
    public class Gage
    {
        /// <summary>
        /// 押品所有权人名称   N   C   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string gcustName { get; set; }

        /// <summary>
        /// 押品所有权人证件类型   N   A   1
        /// </summary>
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string gcustIdtype { get; set; }

        /// <summary>
        /// 押品所有权人证件号码   N   A   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string gcustIdno { get; set; }

        /// <summary>
        /// 押品类型   N   A   3
        /// </summary>
        [MaxLength(3, ErrorMessage = "MaxLength is 3")]
        public string gType { get; set; }

        /// <summary>
        /// 押品名称   N   C   40
        /// </summary>
        [MaxLength(40, ErrorMessage = "MaxLength is 40")]
        public string gName { get; set; }

        /// <summary>
        /// 押品描述   N   C   100
        /// </summary>
        [MaxLength(100, ErrorMessage = "MaxLength is 100")]
        public string gDesc { get; set; }

        /// <summary>
        /// 押品评估价值   N   N   16，2
        /// </summary>
        public decimal gValue { get; set; }

        /// <summary>
        /// 权证号码   N   C   30
        /// </summary>
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string gLicno { get; set; }

        /// <summary>
        /// 权证类型   N   A   2
        /// </summary>
        [MaxLength(2, ErrorMessage = "MaxLength is 2")]
        public string gLicType { get; set; }            
    }

    /// <summary>
    /// listCom
    /// </summary>
    public class listCom
    {
        /// <summary>
        /// 借款人名称   Y   C   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string comName { get; set; }

        /// <summary>
        /// 证件类型   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string comIdtype { get; set; }

        /// <summary>
        /// 证件号码   Y   A   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string comIdno { get; set; }

        /// <summary>
        /// 证件类型   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(25, ErrorMessage = "MaxLength is 25")]
        public string comTel { get; set; }       
    }

    /// <summary>
    /// listRel
    /// </summary>
    public class listRel
    {
        /// <summary>
        /// 借款人名称   Y   C   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string relName { get; set; }        

        /// <summary>
        /// 证件类型   Y   A   1
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(1, ErrorMessage = "MaxLength is 1")]
        public string relIdtype { get; set; }    

        /// <summary>
        /// 证件号码   Y   A   30
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "MaxLength is 30")]
        public string relIdno { get; set; }       

        /// <summary>
        /// 联系电话   Y   A   20
        /// </summary>
        [Required(ErrorMessage = "Required")]
        [MaxLength(25, ErrorMessage = "MaxLength is 25")]
        public string relTel { get; set; }  
    }
}