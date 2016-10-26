using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using Com.HSJF.HEAS.Web.Models;

namespace Com.HSJF.HEAS.Web
{
    public static class MvcHtmlHelper
    {
        #region checkboxlist
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">对应 checkbox 的name 属性</param>
        /// <param name="listInfos">需要绑定的数据源</param>
        /// <param name="htmlAttributes"></param>
        /// <param name="number">每行要显示的个数,默认为1个</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<CheckBoxListModel> listInfos, IDictionary<string, object> htmlAttributes, int number = 1)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("此参数不能为空", "name");
            }
            if (listInfos == null || !listInfos.Any())
            {
                return new MvcHtmlString("<br />");
            }
            StringBuilder sb = new StringBuilder();
            int lineNumber = 1;
            foreach (CheckBoxListModel info in listInfos)
            {
                TagBuilder builder = new TagBuilder("input");

                if (info.IsChecked)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("name", name);
                builder.MergeAttribute("value", info.Value);
                builder.InnerHtml = string.Format("<span>{0}</span>", info.DisplayText.PadRight(15, ' '));
                TagBuilder lable = new TagBuilder("label");
                lable.MergeAttributes(htmlAttributes);
                lable.InnerHtml = builder.ToString(TagRenderMode.Normal);
                sb.Append(lable.ToString(TagRenderMode.Normal));
                if (number == 1)
                {
                    sb.Append("<br />");
                }
                else if (lineNumber % number == 0)
                {
                    sb.Append("<br />");
                }
                lineNumber++;
            }
            return new MvcHtmlString(sb.ToString());
        }

        #endregion
    }
}
