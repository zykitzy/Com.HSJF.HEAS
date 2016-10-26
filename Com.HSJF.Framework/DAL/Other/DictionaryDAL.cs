using Com.HSJF.Framework.EntityFramework.Base;
using Com.HSJF.Framework.EntityFramework.Model.Others;
using Com.HSJF.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.Other
{
    public class DictionaryDAL : BaseRepository<Dictionary, HEASContext>
    {
        public override void Add(Dictionary entity)
        {
            entity.Path = string.Format(@"{0}-{1}", entity.ParentKey ?? string.Empty, entity.Key);
            base.Add(entity);
        }

        public override void Update(Dictionary entity)
        {
            base.Delete(new Dictionary() { Path = entity.Path });
            entity.Path = string.Format(@"{0}-{1}", entity.ParentKey ?? string.Empty, entity.Key);
            base.Add(entity);
        }
        /// <summary>
        /// 获取同一类型的所有字典
        /// </summary>
        /// <param name="parentkey"></param>
        /// <returns></returns>
        public IEnumerable<Dictionary> FindByParentKey(string parentkey)
        {
            return base.GetAll().Where(o => o.ParentKey == parentkey && o.State).OrderBy(t => t.Desc);
        }

        /// <summary>
        /// 获取同一类型的所有字典，不过滤状态
        /// </summary>
        /// <param name="parentkey"></param>
        /// <returns></returns>
        public IEnumerable<Dictionary> FindByParentKeyAll(string parentkey)
        {
            return base.GetAll().Where(o => o.ParentKey == parentkey).OrderBy(t => t.Desc);
        }

        public IEnumerable<Dictionary> FindByParentList(IEnumerable<string> typelist)
        {
            var Diclist = new List<Dictionary>();
            foreach (var str in typelist)
            {
                var list = FindByParentKey(str);
                if (list.Any())
                {
                    Diclist.AddRange(list);
                }
            }
            return Diclist.OrderBy(t => t.Desc);
        }

        public string GetText(string key)
        {
            var entity = base.Get(key);
            return entity == null ? "" : entity.Text;
        }

    }
}
