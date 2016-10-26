using System;

namespace Com.HSJF.Framework.DAL
{
    /// <summary>
    /// 案件状态
    /// </summary>
    public static class CaseStatus
    {
        /// <summary>
        /// 调查，流程1
        /// </summary>
        public const string FirstAudit = "1Audit";
        /// <summary>
        /// 调查或审核拒绝按键后，案件直接结束
        /// </summary>
        public const string CloseCase = "Close";
        /// <summary>
        /// 审核，调查提交后案件状态为审核，审核回退后状态变回调查，流程2
        /// </summary>
        public const string SecondAudit = "2Audit";
        /// <summary>
        /// 贷后挂起(确认案件模式)，流程3
        /// </summary>
        public const string HatsPending = "HatsPending";
        /// <summary>
        /// 公证签约，流程4
        /// </summary>
        public const string PublicMortgage = "Public";
        /// <summary>
        /// 确认签约要件，流程5
        /// </summary>
        public const string ConfrimPublic = "ConfrimPub";
        /// <summary>
        /// 公证签约失败，案件结束
        /// </summary>
        public const string ClosePublic = "ClosePublic";
        [Obsolete("流程节点已取消")]
        /// <summary>
        /// 等待确认放款（等待hats放款通知）
        /// </summary>
        public const string WaitingLending = "WaitingLending";
        /// <summary>
        /// 放款，流程6
        /// </summary>
        public const string Lending = "Lending";
        /// <summary>
        /// 放款提交后，案件进入贷后，流程7
        /// </summary>
        public const string AfterCase = "After";
        /// <summary>
        /// 结清案件
        /// </summary>
        public const string FinishCase = "Finish";
        /// <summary>
        /// 因为特殊原因而意外结束
        /// </summary>
        public const string SpecialClose = "SpecialClose";
    }
}
