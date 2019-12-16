using System;
using System.Collections.Generic;
using System.Text;
using Snowing.DDD.Core.Interfaces;
using Snowing.DDD.Core.Entities;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Snowing.DDD.Infrastructure.Data
{
    public class RedisCache<T> : ICache<T> where T : BaseEntity
    {
        protected readonly DistributedCacheEntryOptions options;
        protected readonly ConnectionMultiplexer redis;
        protected readonly IDatabase db;
        protected readonly string keyPrefix;

        #region .ctor
        public RedisCache(IConnectionStringProvier con):this(con, null)
        {
            
        }

        public RedisCache(IConnectionStringProvier con, DateTime absoluteExpired):
            this(con, new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpired })
        {
            
        }

        public RedisCache(IConnectionStringProvier con, TimeSpan slidingExpiration):
            this(con, new DistributedCacheEntryOptions() { SlidingExpiration = slidingExpiration })
        {

        }

        protected RedisCache(IConnectionStringProvier con, DistributedCacheEntryOptions options)
        {
            this.options = options ?? new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 20, 0),
                SlidingExpiration = new TimeSpan(0, 20, 0),
            };
            string connectionString = con.GetConnectionString("redis");
            redis = ConnectionMultiplexer.Connect(connectionString);
            db = redis.GetDatabase();
            this.keyPrefix = typeof(T).Name + "_";
        }

        #endregion


        public bool KeyExists(string key)
        {
            string str = this.db.StringGet(this.keyPrefix + key);
            return !string.IsNullOrEmpty(str);

        }

        #region Get
        public T Get(string key)
        {
            return InnerGet<T>(key);
        }

        public OtherT Get<OtherT>(string key)
        {
            return InnerGet<OtherT>(key);
        }

        protected TValue InnerGet<TValue>(string key)
        {
            string str = this.db.StringGet(this.keyPrefix + key);
            TValue result = default(TValue);
            if (!string.IsNullOrEmpty(str))
            {
                Type type = typeof(TValue);
                if (type.IsClass)
                {
                    JsonSerializer serializer = new JsonSerializer();
                    StringReader reader = new StringReader(str);
                    object o = serializer.Deserialize(new JsonTextReader(reader), typeof(TValue));
                    result = (TValue)o;
                }
                else if (type.Name.ToLower() == "string")
                {
                    result =  (TValue)(object)str;
                }
                else if (type.IsValueType)
                {
                    switch (type.FullName)
                    {
                        case "System.UInt64":
                            result = (TValue)(object)  Convert.ToUInt64(str);
                            break;
                        default:
                            throw new NotImplementedException("需要实现");
                    }
                }
            }

            if (this.options.SlidingExpiration.HasValue)
            {
                Task.Run(() => 
                {
                    this.db.StringSet(this.keyPrefix + key, str, this.options.SlidingExpiration);
                });
            }

            return result;
        }
        #endregion 

        public void Set(string key, object value)
        {
            Task.Run(() => 
            {
                string str = string.Empty;
                if (value.GetType().IsClass || value.GetType().Name.ToLower() == "string")
                {
                    str = JsonConvert.SerializeObject(value);
                }
                else if (value.GetType().IsValueType)
                {
                    str = Convert.ToString(value);
                }

                if (this.options.SlidingExpiration.HasValue)
                {
                    this.db.StringSet(this.keyPrefix + key, str, this.options.SlidingExpiration);
                }
                else if (this.options.AbsoluteExpiration.HasValue)
                {
                    this.db.StringSet(this.keyPrefix + key, str, this.options.AbsoluteExpiration.Value.Offset);
                }
                else if (this.options.AbsoluteExpirationRelativeToNow != null)
                {
                    this.db.StringSet(this.keyPrefix + key, str, this.options.AbsoluteExpirationRelativeToNow);
                }
            });
            
        }

        public void Unset(string key)
        {
            Task.Run(() => 
            {
                this.db.KeyDelete(this.keyPrefix + key);
            });
        }
    }
}
