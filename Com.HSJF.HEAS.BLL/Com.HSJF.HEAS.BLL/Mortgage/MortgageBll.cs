using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.HEAS.BLL.Mortgage.Dto;
using Com.HSJF.Infrastructure.Extensions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.BLL.Mortgage
{
    public class MortgageBll
    {
        private readonly BaseAuditDAL _baseAuditDal;
        private readonly MortgageDAL _mortgageDal;
        private readonly IntroducerAuditDAL _introducerAuditDal;

        public MortgageBll()
        {
            _baseAuditDal = new BaseAuditDAL();
            _mortgageDal = new MortgageDAL();
            _introducerAuditDal = new IntroducerAuditDAL();
        }

        /// <summary>
        /// 根据案件号查询签约信息
        /// </summary>
        /// <param name="caseNum">案件号</param>
        /// <returns>签约信息</returns>
        public PublicMortgage Query(string caseNum)
        {
            var baseaudit = _baseAuditDal.GetbyCaseSataus(caseNum, CaseStatus.PublicMortgage);
            //var baseaudit = _baseAuditDal.GetAllBase().AsNoTracking().OrderByDescending(p => p.Version).FirstOrDefault(p => p.NewCaseNum == caseNum && p.CaseStatus == CaseStatus.PublicMortgage);
            if (baseaudit.IsNotNull())
            {
                return _mortgageDal.GetAll().AsNoTracking().FirstOrDefault(p => p.ID == baseaudit.ID);
            }
            return null;
        }

        public PublicMortgage QueryById(string id)
        {
            var baseaudit = _baseAuditDal.GetAllBase().FirstOrDefault(p => p.ID == id);
            if (baseaudit.IsNotNull())
            {
                var publicCase = _baseAuditDal.GetAllBase().Where(t => t.NewCaseNum == baseaudit.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault(t => t.CaseStatus == CaseStatus.PublicMortgage || t.CaseStatus == CaseStatus.ConfrimPublic);

                if (publicCase.IsNotNull())
                {
                    return _mortgageDal.GetAll().AsNoTracking().FirstOrDefault(p => p.ID == publicCase.ID);
                }
            }
            return null;
        }

        /// <summary>
        /// 修改他证
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool PolishingOtherFile(string id, string fileid)
        {
            var baseaudit = _baseAuditDal.GetAllBase().FirstOrDefault(p => p.ID == id);
            if (!baseaudit.IsNotNull()) return false;

            var publicCase = _baseAuditDal.GetAllBase().Where(t => t.NewCaseNum == baseaudit.NewCaseNum).OrderByDescending(t => t.Version).FirstOrDefault(t => t.CaseStatus == CaseStatus.ConfrimPublic);

            if (!publicCase.IsNotNull()) return false;
            var mort = _mortgageDal.GetAll().AsNoTracking().FirstOrDefault(p => p.ID == publicCase.ID);
            if (!mort.IsNotNull()) return false;
            mort.OtherFile = fileid;
            _mortgageDal.UpdatePublic(mort);
            _mortgageDal.AcceptAllChange();
            return true;
        }

        /// <summary>
        /// 查询等待确认模式案件
        /// </summary>
        /// <param name="caseNum">案件号</param>
        /// <returns>案件信息或者null</returns>
        public BaseAudit QueryWaitingLending(string caseNum)
        {
            var auditCase = _baseAuditDal.GetbyCaseNum(caseNum);
            if (auditCase.IsNotNull() && auditCase.CaseStatus == CaseStatus.WaitingLending)
            {
                return auditCase;
            }
            return null;
        }

        /// <summary>
        /// 进入审核
        /// </summary>
        /// <param name="audit">案件信息</param>
        /// <param name="userName">用户名</param>
        /// <returns>成功与否</returns>
        public bool IntoMortgage(BaseAudit audit, string userName)
        {
            return _baseAuditDal.ReturnAudit(audit, userName, CaseStatus.PublicMortgage);
        }

        /// <summary>
        /// 提交签约信息
        /// </summary>
        /// <param name="entity">签约信息</param>
        /// <param name="auditEntity">案件基础信息</param>
        /// <param name="creatUser">当前用户</param>
        /// <returns>是否签约成功</returns>
        public bool SubmitCase(PublicMortgage entity, BaseAudit auditEntity, string creatUser)
        {
            AuditHelp ah = new AuditHelp();

            //2016-09-09 在确认签约要件之后再解锁
            var introduces = _introducerAuditDal.GetAll().Where(p => p.AuditID == auditEntity.ID);
            _introducerAuditDal.DeleteRange(introduces);

            //  auditEntity.CreateTime = DateTime.Now;
            //  auditEntity.CreateUser = creatUser;

            // 2016-09-09 修改，进入 确认签约要件
            var newid = ah.CopyBaseAudit(auditEntity, creatUser, CaseStatus.ConfrimPublic, false);
            var pub = CopyPublic(entity);
            pub.ID = newid;
            pub.CreateUser = creatUser;
            //entity.ID = id;
            _mortgageDal.Add(pub);
            _mortgageDal.AcceptAllChange();

            return true;
        }

        private PublicMortgage CopyPublic(PublicMortgage entity)
        {
            var pubMort = new PublicMortgage();
            Infrastructure.ExtendTools.ObjectExtend.CopyTo(entity, pubMort);
            pubMort.CreateTime = DateTime.Now;
            return pubMort;
        }

        /// <summary>
        /// 确认签约要件
        /// </summary>
        /// <param name="caseid">案件ID，并非案件号</param>
        /// <returns></returns>
        public async Task<bool> ConfrimPublic(string id, string createUser, string description)
        {
            var audit = _baseAuditDal.Get(id);
            MortgageDAL mort = new MortgageDAL();
            if (audit == null)
            {
                return false;
            }
            var morimodel = mort.GetPublic(id);
            if (morimodel == null)
            {
                return false;
            }
            audit.Description = description;

            AuditHelp ah = new AuditHelp();
            var publicMortgageDto = new PublicMortgageDto();

            Infrastructure.ExtendTools.ObjectExtend.CopyTo(morimodel, publicMortgageDto);
            publicMortgageDto.LenderName = audit.LenderName;
            publicMortgageDto.CaseNum = audit.NewCaseNum;
            publicMortgageDto.OpeningBank = audit.OpeningBank;
            publicMortgageDto.OpeningSite = audit.OpeningSite;
            publicMortgageDto.BankCard = audit.BankCard;
            if (audit.IntroducerAudits != null && audit.IntroducerAudits.Any())
            {
                foreach (var r in audit.IntroducerAudits)
                {
                    var newIntro = new IntroducerAudit();
                    Infrastructure.ExtendTools.ObjectExtend.CopyTo(r, newIntro);
                    publicMortgageDto.Introducer.Add(newIntro);
                }
            }

            UserDAL ud = new UserDAL();
            var contr = await ud.FindById(morimodel.ContractPerson);
            publicMortgageDto.ContractPersonText = contr.DisplayName;
            MortgagePush _mortgagePush = new MortgagePush();
            var pushResult = _mortgagePush.PushToHats(publicMortgageDto);
            if (!pushResult.IsSuccess) return false;
            var newid = ah.CopyBaseAudit(audit, createUser, CaseStatus.Lending, false);

            _mortgageDal.CopyPublic(morimodel, newid, createUser);
            //pub.ID = newid;
            //pub.CreateUser = createUser;
            //_mortgageDal.Add(pub);
            _mortgageDal.AcceptAllChange();
            return true;
        }

        /// <summary>
        /// 编辑签约信息
        /// </summary>
        /// <param name="mortgage">签约信息</param>
        /// <param name="baseAudit">案件基础信息</param>
        /// <param name="createUser">当前用户</param>
        /// <returns>编辑成功与否</returns>
        public bool Edit(PublicMortgage mortgage, BaseAudit baseAudit, string createUser)
        {
            var introduces = _introducerAuditDal.GetAll().Where(p => p.AuditID == baseAudit.ID);
            _introducerAuditDal.DeleteRange(introduces);
            // _baseAuditDal.AcceptAllChange();
            //_mortgageDal.AcceptAllChange();

            return true;
        }
    }
}