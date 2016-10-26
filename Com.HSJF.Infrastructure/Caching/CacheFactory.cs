using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure
{
    public static class CacheFactory
    {
        static ICache _cache = null;
        public static ICache CreatInstance()
        {
            if (_cache == null)
            {
                return new CacheImpl();
            }
            return _cache;
        }
    }
}
