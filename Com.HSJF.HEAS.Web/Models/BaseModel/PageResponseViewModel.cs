using System.Collections.Generic;

namespace Com.HSJF.HEAS.Web.Models.BaseModel
{
    public class PageResponseViewModel<T>
    {
        public IEnumerable<T> Data { get; set; }
       
        public int Total { get; set; }
       
        public int PageIndex { get; set; }
       
        public int PageSize { get; set; }

        public int TotalPage { get; set; }
    }
}
