namespace Com.HSJF.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Caching;
    

    public class CacheImpl : ICache
    {
        private readonly ObjectCache _manager;

        public CacheImpl(string cacheManagerName)
            : this(MemoryCache.Default)
        {
        }

        public CacheImpl()
            : this(MemoryCache.Default)
        {
        }


        public CacheImpl(ObjectCache manager)
        {
            Check.Argument.IsNotNull(manager, "manager");

            _manager = manager;
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return (int)_manager.GetCount();
            }
        }

        public void Clear()
        {
            List<string> cacheKeys = new List<string>();
            _manager.ForEach(o => cacheKeys.Add(o.Key));
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
            
        }

        public bool Contains(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _manager.Contains(key);
        }

        public T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return (T) _manager.Get(key);
        }

        public bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            value = default(T);

            if (_manager.Contains(key))
            {
                object existingValue = _manager.Get(key);

                if (existingValue != null)
                {
                    value = (T) existingValue;

                    return true;
                }
            }

            return false;
        }

        public void Set<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            RemoveIfExists(key);

            _manager.Add(key, value, DateTime.Now.AddMinutes(2));
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            CacheItemPolicy p = new CacheItemPolicy();
            p.AbsoluteExpiration = absoluteExpiration;
            p.Priority = CacheItemPriority.Default;


            _manager.Add(key, value, p);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "absoluteExpiration");

            RemoveIfExists(key);
            CacheItemPolicy p = new CacheItemPolicy();
            p.SlidingExpiration = slidingExpiration;
            p.Priority=CacheItemPriority.Default;
             

            _manager.Add(key, value,p);
        }

        public void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _manager.Remove(key);
        }

        internal void RemoveIfExists(string key)
        {
            if (_manager.Contains(key))
            {
                _manager.Remove(key);
            }
        }
    }

    public interface ICacheBuilder
    {
        ObjectCache GetInstance();
        string DefaultRegionName { get; }
   }

    public class MemoryCacheBuilder : ICacheBuilder
    {
        public MemoryCacheBuilder() { }

        public ObjectCache GetInstance()
        {
            return MemoryCache.Default;
        }

        public string DefaultRegionName
        {
            get { return null; }
        }
    }

 //   public class MemcachedCache : ObjectCache, ICacheBuilder
 //   {
 //      private long _lDefaultExpireTime = 3600; // default Expire Time
 //      private MemcachedClient _client = null;
 //      #region ICache Members
   
 //   public MemcachedCache()
 //      {
 //         this._client = MemcachedClientService.Instance.Client;
 //     }
  
 //   public override void Set(string key, object value, System.DateTimeOffset absoluteExpiration, string regionName = null)
 //      {
 //         Enforce.NotNull(key, "key");
 //      CacheItem item = new CacheItem(key, value, regionName);
 //       CacheItemPolicy policy = new CacheItemPolicy();
 //         policy.AbsoluteExpiration = absoluteExpiration;
  
 //        Set(item, policy);
 //      }
 
 //      public override void Set(CacheItem item, CacheItemPolicy policy)
 //    {
 //        if (item == null || item.Value == null)
 //            return;
  
 //       item.Key = item.Key.ToLower();
 
 //         if (policy != null && policy.ChangeMonitors != null && policy.ChangeMonitors.Count > 0)
 //         throw new NotSupportedException("Change monitors are not supported");
 
 //        // max timeout in scaleout = 65535
 //        TimeSpan expire = (policy.AbsoluteExpiration.Equals(null)) ?
 //                             policy.SlidingExpiration :
 //                            (policy.AbsoluteExpiration - DateTimeOffset.Now);
 
 //        double timeout = expire.TotalMinutes;
 //        if (timeout > 65535)
 //            timeout = 65535;
 //       else if (timeout > 0 && timeout < 1)
 //           timeout = 1;
  
 //        this._client.Store(Enyim.Caching.Memcached.StoreMode.Set, item.Key.ToString(), item.Value);
 
 //    }
    
 //    public override object this[string key]
 //    {
 //         get
 //       {
 //           return Get(key);
 //       }
 //      set
 //        {
 //            Set(key, value, null);
 //         }
 //      }
  
 //     public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
 //     {
 //         CacheItem item = GetCacheItem(key, regionName);
 //        if (item == null)
 //      {
 //            Set(new CacheItem(key, value, regionName), policy);
 //           return value;
 //       }
 
 //       return item.Value;
 //     }

 //    public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
 //    {
 //        CacheItem item = GetCacheItem(value.Key, value.RegionName);
 //        if (item == null)
 //        {
 //           Set(value, policy);
 //          return value;
 //        }

 //      return item;
 //   }

 //     public override object AddOrGetExisting(string key, object value, System.DateTimeOffset absoluteExpiration, string regionName = null)
 //    {
 //        CacheItem item = new CacheItem(key, value, regionName);
 //         CacheItemPolicy policy = new CacheItemPolicy();
 //        policy.AbsoluteExpiration = absoluteExpiration;
   
 //          return AddOrGetExisting(item, policy);
 //     }
   
 //    public override bool Contains(string key, string regionName = null)
 //     {
 //        return false;
 //    }
  
 //     public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(System.Collections.Generic.IEnumerable<string> keys, string regionName = null)
 //     {
 //        throw new System.NotImplementedException();
 //    }
  
 //   public override DefaultCacheCapabilities DefaultCacheCapabilities
 //    {
 //       get
 //       {
 //            return
 //                DefaultCacheCapabilities.OutOfProcessProvider |
 //                DefaultCacheCapabilities.AbsoluteExpirations |
 //               DefaultCacheCapabilities.SlidingExpirations |
 //                DefaultCacheCapabilities.CacheRegions;
 //        }
 //    }
  
 //    public override object Get(string key, string regionName = null)
 //    {
 //       key = key.ToLower();
 
 //        return this._client.Get(key);
 //    }
 
 //    public override CacheItem GetCacheItem(string key, string regionName = null)
 //    {
 //       object value = Get(key, regionName);
 //       if (value != null)
 //           return new CacheItem(key, value, regionName);
  
 //       return null;
 //   }
 
 //    public override long GetCount(string regionName = null)
 //   {
 //        return -1;
 //    }
 
 //    protected override System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, object>> GetEnumerator()
 //    {
 //          throw new System.NotImplementedException();
 //    }
  
 //   public override System.Collections.Generic.IDictionary<string, object> GetValues(System.Collections.Generic.IEnumerable<string> keys, string regionName = null)
 //   {
 //         throw new System.NotImplementedException();
 //    }
  
 //    public override string Name
 //    {
 //        get { return "MemcachedProvider"; }
 //   }
  
 //   public override object Remove(string key, string regionName = null)
 //    {
 //        key = key.ToLower();
 //        return this._client.Remove(key);
 //    }
   
 //    public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
 //    {
 //        Set(new CacheItem(key, value, regionName), policy);
 //    }
  
 //    #endregion
   
 //    #region ICacheBuilder Members
  
 //    public ObjectCache GetInstance()
 //     {
 //       return this;
 //    }
  
 //    public string DefaultRegionName
 //    {
 //        get { throw new NotImplementedException(); }
 //    }
 
 //    #endregion
 //}
}