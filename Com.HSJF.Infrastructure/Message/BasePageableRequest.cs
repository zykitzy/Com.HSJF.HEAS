using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Com.HSJF.Infrastructure.Message
{
    public abstract class BasePageableRequest : BaseRequest, ILocaleRequest
    {
        //sord: desc排序  sidx:ID排序字段  rows: 10 返回行数及内容  
        //page: 1 当前页  total: 50  总页数  records:返回总记录数
        private int _pageNum = 1;
        private int _pageSize = 25;
        private string _sort = "";
        private string _order = "asc";

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > 0 ? value : 999); //负值或0时显示所有，考虑性能则只显示999行。
            }
        }

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageNum
        {
            get
            {
                return _pageNum;
            }
            set
            {
                _pageNum = (value < 1 ? 1 : value);
            }
        }

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        /// <summary>
        /// 排序的属性名
        /// </summary>
        public string Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = (value ?? "");
            }
        }

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        /// <summary>
        /// 正/倒排序The value should be "asc" or "desc".
        /// </summary>
        public string Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (string.Equals(value, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    _order = "desc";
                }
                else
                {
                    _order = "asc";
                }
            }
        }

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        public bool WithLocale { get; set; }

        [HiddenInput(DisplayValue = false)] //不允许编辑并且不显示
        /// <summary>
        /// Return Sample: "UserName asc,Department desc"
        /// Will be used like, IEnumable.OrderBy("UserName asc,Department desc")
        /// </summary>
        public string Ordering
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Sort) ? "" : this.Sort + " " + this.Order;
            }
        }
    }
}
