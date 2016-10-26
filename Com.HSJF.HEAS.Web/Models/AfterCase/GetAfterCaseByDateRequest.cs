using System;
using Com.HSJF.Infrastructure;

namespace Com.HSJF.HEAS.Web.Models.AfterCase
{
    /// <summary>
    /// 根据时间请求贷后案件
    /// </summary>
    public class GetAfterCaseByDateRequest
    {
        private string _errorMessgae = string.Empty;

        /// <summary>
        /// 案件开始时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 案件结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return _errorMessgae;
        }

        /// <summary>
        /// 参数是否正确
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            bool isValid = false;
            try
            {
                var beginDateTime = DateTime.ParseExact(this.BeginDate.NullSafe(), "yyyy/MM/dd", null);
                if (!string.IsNullOrEmpty(EndDate.NullSafe()))
                {
                    var endTime = DateTime.ParseExact(this.EndDate.NullSafe(), "yyyy/MM/dd", null);
                }

                isValid = true;

            }
            catch (ArgumentNullException ex)
            {
                _errorMessgae = "参数不可为空";

            }
            catch (FormatException ex)
            {
                _errorMessgae = "参数格式必须为yyyy/MM/dd";
            }
            catch (Exception ex)
            {
                _errorMessgae = ex.Message;
            }

            return isValid;
        }


    }
}