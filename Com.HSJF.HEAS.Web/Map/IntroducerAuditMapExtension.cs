using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.Web.Models.Audit;

namespace Com.HSJF.HEAS.Web.Map
{
    public static partial class MapExtension
    {
        public static IntroducerAudit MaptoIntroducerAudit(this IntroducerAuditViewModel model)
        {
            return new IntroducerAudit()
            {
                ID = model.ID,
                Name = model.Name,
                Contract = model.Contract,
                RebateAmmount = model.RebateAmmount,
                RebateRate = model.RebateRate,
                Account = model.Account,
                AccountBank = model.AccountBank,
                AuditID = model.AuditID,
                Sequence = model.Sequence
            };
        }

        public static IntroducerAuditViewModel MaptoIntroduceAuditViewModel(this IntroducerAudit entity)
        {
            return new IntroducerAuditViewModel
            {
                ID = entity.ID,
                Name = entity.Name,
                Contract = entity.Contract,
                RebateAmmount = entity.RebateAmmount,
                RebateRate = entity.RebateRate,
                Account = entity.Account,
                AccountBank = entity.AccountBank,
                AuditID = entity.AuditID,
                Sequence = entity.Sequence
            };
        }
    }
}