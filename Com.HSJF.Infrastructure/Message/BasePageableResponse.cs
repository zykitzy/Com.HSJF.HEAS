using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Message
{
    public abstract class BasePageableResponse : BaseResponse
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNum { get; set; }
        //public int page { get; set; }//page: 1 当前页


        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        //public int total { get; set; }//total: 50  总页数



        /// <summary>
        /// 返回Item的总数量
        /// </summary>
        public int TotalRecords { get; set; }

        //public int records { get; set; }//records:返回总记录数


    }
}
