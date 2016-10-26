
namespace Com.HSJF.HEAS.BLL.FinishedCase.Dto
{
    public class GetFinishedCasesInput
    {

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 案件编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 指定页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 排序方式(升序or降序)
        /// </summary>
        public string Sort { get; set; }
    }
}
