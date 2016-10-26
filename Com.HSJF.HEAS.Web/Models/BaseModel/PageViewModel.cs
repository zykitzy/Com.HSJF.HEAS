
namespace Com.HSJF.HEAS.Web.Models.BaseModel
{
    public class PageRequestViewModel
    {
        public int PageSize { get; set; }
        
        public int PageIndex { get; set; }
      
        public int TotalCount { get; set; }
       
        public int TotalPage { get; set; }
       
        public string Order { get; set; }
       
        public string Sort { get; set; }
    }
}
