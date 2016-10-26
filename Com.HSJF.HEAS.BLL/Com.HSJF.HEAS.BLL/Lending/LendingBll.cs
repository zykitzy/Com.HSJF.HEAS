using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.DAL;

namespace Com.HSJF.HEAS.BLL.Lending
{
    public class LendingBll
    {
        private readonly BaseAuditDAL _baseAuditDal;

        public LendingBll()
        {
            _baseAuditDal = new BaseAuditDAL();
        }

        /// <summary>
        /// 提交到放款
        /// </summary>
        /// <param name="lendingCase">放款案件</param>
        /// <param name="creatUser">创建人</param>
        public bool SubmitLending(BaseAudit lendingCase, string creatUser)
        {
            return _baseAuditDal.ReturnAudit(lendingCase, creatUser, CaseStatus.Lending);
        }
    }
}
