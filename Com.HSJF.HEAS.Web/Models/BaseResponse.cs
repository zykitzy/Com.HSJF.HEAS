
namespace Com.HSJF.HEAS.Web.Models
{
    public class BaseResponse<T>
    {
        public string Status { get; set; }

        public ErrorMessage[] Message { get; set; }

        public T Data { get; set; }
    }

    public class ErrorMessage
    {
        public ErrorMessage()
        {

        }

        public ErrorMessage(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }

        public string Message { get; set; }
    }
}
