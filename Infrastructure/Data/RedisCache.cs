﻿using System;
using System.Collections.Generic;
using System.Text;
using Snowing.DDD.Core.Interfaces;
using Snowing.DDD.Core.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Snowing.DDD.Infrastructure.Data
{ 
    public class RedisCache<T, TKey> : RedisCacheBase, ICache<T, TKey> where T : BaseEntity<TKey> where TKey : struct
    {
        #region .ctor
        public RedisCache(IRedisConnectionProvider con):this(con, null)
        {
            
        }

        public RedisCache(IRedisConnectionProvider con, DateTime absoluteExpired):
            this(con, new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpired })
        {
            
        }

        public RedisCache(IRedisConnectionProvider con, TimeSpan slidingExpiration):
            this(con, new DistributedCacheEntryOptions() { SlidingExpiration = slidingExpiration })
        {

        }

        protected RedisCache(IRedisConnectionProvider con, DistributedCacheEntryOptions options):base(con, options)
        {
            this.keyPrefix = typeof(T).Name + ":";
        }

        #endregion

        public T Get(string key)
        {
            return InnerGet<T>(key);
        }

    }
}
