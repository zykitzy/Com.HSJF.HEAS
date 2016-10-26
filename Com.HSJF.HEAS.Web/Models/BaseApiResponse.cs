
namespace Com.HSJF.HEAS.Web.Models
{
    /// <summary>
    /// api返回统一格式
    /// </summary>
    /// <typeparam name="T">具体数据格式</typeparam>
    public class BaseApiResponse<T>
    {
        public BaseApiResponse()
        {

        }

        public BaseApiResponse(StatusEnum status, string message)
        {
            this.Status = status.ToString();
            this.Message = message;
        }

        public string Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }

    public enum StatusEnum
    {
        /// <summary>
        /// 请求处理成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 请求处理错误
        /// </summary>
        Error = 1,
        /// <summary>
        /// 请求警告(适用于参数错误)
        /// </summary>
        Warning = 2,

        Failed = 3
    }
}