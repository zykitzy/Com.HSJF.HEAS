using System.Linq;
using Com.HSJF.Framework.EntityFramework.Model.Audit;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class QueryByPageInput
    {
        public IQueryable<BaseAudit> Audits { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string Order { get; set; }

        public string Sort { get; set; }
    }
}
