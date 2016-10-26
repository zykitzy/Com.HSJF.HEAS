using System;
using System.Collections.Generic;
using System.Linq;
using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.BLL.FinishedCase.Dto;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.BLL.Other.Dto;
using Com.HSJF.Infrastructure.Extensions;

namespace Com.HSJF.HEAS.BLL.FinishedCase
{
    public class FinishedCaseBll
    {
        #region Fields

        private readonly BaseAuditDAL _auditDal;
        private readonly CollateralAuditDAL _collateralDal;

        #endregion

        #region Ctors
        public FinishedCaseBll()
        {
            _auditDal = new BaseAuditDAL();
            _collateralDal = new CollateralAuditDAL();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 结清案件
        /// </summary>
        /// <param name="casenum">案件号</param>
        /// <returns>是否成功</returns>
        public Tuple<bool, string> FinishCase(string casenum)
        {
            var baseCase = GetMaxVersionAudit(casenum);
            if (baseCase.IsNull() || baseCase.CaseStatus != CaseStatus.AfterCase)
            {

                return new Tuple<bool, string>(false, "案件号无效");
            }

            // 更新原纪录时间
            baseCase.CreateTime = DateTime.Now;
            _auditDal.Update(baseCase);

            bool isCopied = new AuditHelp().CopyBaseAudit(baseCase, "admin", CaseStatus.FinishCase) != string.Empty;

            if (isCopied)
            {
                //UnlockCollateral(baseCase);
                return new Tuple<bool, string>(true, "成功");
            }
            else
            {
                return new Tuple<bool, string>(false, "未知错误");
            }
        }

        /// <summary>
        /// 查询结清案件
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <param name="total">总条数</param>
        /// <returns>结清案件</returns>
        public IEnumerable<BaseAudit> GetFinishedCases(GetFinishedCasesInput input, out int total)
        {

            var allCases = GetAllFinishedCase();

            if (input.BorrowerName.IsNotNullOrWhiteSpace())
            {
                allCases = allCases.Where(p => p.BorrowerName.Contains(input.BorrowerName));
            }

            if (input.CaseNum.IsNotNullOrWhiteSpace())
            {
                allCases = allCases.Where(p => p.CaseNum.Contains(input.CaseNum));
            }

            var pageResult = _auditDal.GetAllPage(allCases, out total, input.PageSize, input.PageIndex, input.Order,
                input.Sort);

            return pageResult;
        }

        /// <summary>
        /// 查询结清案件
        /// </summary>
        /// <param name="caseId">案件ID</param>
        /// <returns>案件信息或者null</returns>
        public BaseAudit GetFinishedCase(string caseId)
        {
            if (caseId.IsNullOrWhiteSpace())
            {
                return null;
            }

            return GetAllFinishedCase().FirstOrDefault(p => p.ID == caseId);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取所有已结清案件
        /// </summary>
        /// <returns>结清案件</returns>
        private IQueryable<BaseAudit> GetAllFinishedCase()
        {
            return _auditDal.GetAll(p => p.CaseStatus == CaseStatus.FinishCase);
        }

        /// <summary>
        /// 获取最近版本案件
        /// </summary>
        /// <param name="casenum">案件号</param>
        /// <returns>案件信息</returns>
        private BaseAudit GetMaxVersionAudit(string casenum)
        {
            return _auditDal.GetAllBase()
                .Where(p => p.NewCaseNum == casenum)
                .OrderByDescending(p => p.Version)
                .FirstOrDefault();
        }


        /// <summary>
        /// 解锁结清案件抵押物
        /// </summary>
        /// <param name="baseAudit">案件信息</param>
        //private void UnlockCollateral(BaseAudit baseAudit)
        //{
        //   // var relationState = new RelationStateBLL();
        //    var collateras = _collateralDal.GetAll().Where(p => p.AuditID == baseAudit.ID && p.CollateralType == "-FacilityCategary-MainFacility");

        //    collateras.ToList().ForEach(p => relationState.UpdateLockRelationState(new RelationStateBLLModel()
        //    {
        //        Number = p.HouseNumber
        //    }));
        //}

        #endregion
    }
}
