using System.Collections.Generic;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Others;

namespace Com.HSJF.HEAS.BLL.Other
{
    public class DictionaryBLL 
    {
        private readonly DictionaryDAL _dictionaryDal;

        public DictionaryBLL()
        {
            _dictionaryDal = new DictionaryDAL();
        }

        public IEnumerable<Dictionary> QueryByParentKey(string parentKey)
        {
            return _dictionaryDal.FindByParentKey(parentKey);
        }

    }
}
