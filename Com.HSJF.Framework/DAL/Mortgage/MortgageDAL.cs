using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.HSJF.Framework.EntityFramework.Model.Audit;

namespace Com.HSJF.Framework.DAL.Mortgage
{
    public class MortgageDAL : BaseRepository<PublicMortgage, HEASContext>
    {
        public bool IsCurrentCase(string id)
        {
            BaseAuditDAL bad = new BaseAuditDAL();
            var current = bad.Get(id);
            if (current != null)
            {
                return true;
            }
            return false;
        }

        public override PublicMortgage Get(object key)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            var curr = bd.Get(key);
            if (curr != null)
            {
                var aud = bd.GetbyCaseSataus(curr.NewCaseNum, CaseStatus.PublicMortgage);
                if (aud != null)
                {
                    return base.Get(aud.ID);
                }
            }
            return null;
        }
        public PublicMortgage GetPublic(object key)
        {
            MortgageDAL dal = new MortgageDAL();
            var curr = base.Get(key);
            return curr ?? null;
        }

        public override void Update(PublicMortgage entity)
        {
            var mor = Get(entity.ID);
            if (mor == null)
            {
                Add(entity);
            }
            else
            {
                base.Update(entity);
            }
        }
        public void UpdatePublic(PublicMortgage entity)
        {
            base.Update(entity);
        }
        public override void UpdateRange(IEnumerable<PublicMortgage> entities)
        {
            foreach (var t in entities)
            {
                base.Update(t);
            }
        }

        public bool SubmitCase(PublicMortgage entity, string description, string creatUser)
        {
            this.Update(entity);
            AuditHelp ah = new AuditHelp();
            BaseAuditDAL bad = new BaseAuditDAL();
            var baseaduit = bad.Get(entity.ID);

            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.PublicMortgage)
            {
                //         baseaduit.CreateTime = DateTime.Now;
                //        baseaduit.CreateUser = creatUser;
                baseaduit.Description = description;
                ah.CopyBaseAudit(baseaduit, creatUser, CaseStatus.Lending);
                return true;
            }
            return false;
        }

        public bool RejectCase(string id, string createUser, string Description,string RejectReason)
        {
            AuditHelp ah = new AuditHelp();
            BaseAuditDAL bad = new BaseAuditDAL();
            var baseaduit = bad.Get(id);
            if (baseaduit != null && baseaduit.CaseStatus == CaseStatus.PublicMortgage)
            {
                //     baseaduit.CreateTime = DateTime.Now;
                baseaduit.Description = Description;
                baseaduit.RejectReason = RejectReason;
                //     baseaduit.CreateUser = createUser;
                bad.Update(baseaduit);
                ah.CopyBaseAudit(baseaduit, createUser, CaseStatus.ClosePublic);
                return true;
            }
            return false;
        }
        public bool RejectPublic(string id, string createUser, string Description)
        {
            var ah = new AuditHelp();
            var bad = new BaseAuditDAL();

            var baseaduit = bad.Get(id);
            var morimodel = GetPublic(id);
            if (baseaduit == null || baseaduit.CaseStatus != CaseStatus.ConfrimPublic) return false;
            if(morimodel==null) return false;
            baseaduit.Description = Description;
            var newid = ah.CopyBaseAudit(baseaduit, createUser, CaseStatus.PublicMortgage, false);
            CopyPublic(morimodel, newid, createUser);
            AcceptAllChange();
            return true;
        }
        public  void CopyPublic(PublicMortgage entity, string id, string createUser)
        {
            var pubMort = new PublicMortgage()
            {
                CreateTime = DateTime.Now,
                ID = id,
                CreateUser = createUser,
                ContractFile = entity.ContractFile,
                NoteFile = entity.NoteFile,
                ReceiptFile = entity.ReceiptFile,
                OtherFile = entity.OtherFile,
                FourFile = entity.FourFile,
                ContractNo = entity.ContractNo,
                ContractAmount = entity.ContractAmount,
                ContractDate = entity.ContractDate,
                ContractPerson = entity.ContractPerson,
                UndertakingFile = entity.UndertakingFile,
                RepaymentAttorneyFile = entity.RepaymentAttorneyFile,
                ContactConfirmFile = entity.ContactConfirmFile,
                PowerAttorneyFile = entity.PowerAttorneyFile,
                CollectionFile = entity.CollectionFile,
            };
            MortgageDAL dal = new MortgageDAL();
            //Infrastructure.ExtendTools.ObjectExtend.CopyTo(entity, pubMort);
            //pubMort.CreateTime = DateTime.Now;
            //pubMort.ID = id;
            //pubMort.CreateUser = createUser;
            dal.Add(pubMort);
        }
    }
}