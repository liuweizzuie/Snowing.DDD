﻿using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Snowing.DDD.Core.Interfaces;
//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Snowing.DDD.Infrastructure.Data
{
    public class RedisCacheBase : ICacheBase
    {
        protected readonly DistributedCacheEntryOptions options;
        //protected readonly ConnectionMultiplexer redis;
        //protected readonly IDatabase db;
        protected readonly CSRedisClient rclient;
        protected string keyPrefix { get; set; }

        #region .ctor
        public RedisCacheBase(IConnectionStringProvier con) : this(con, null)
        {

        }

        public RedisCacheBase(IConnectionStringProvier con, DateTime absoluteExpired) :
            this(con, new DistributedCacheEntryOptions() { AbsoluteExpiration = absoluteExpired })
        {

        }

        public RedisCacheBase(IConnectionStringProvier con, TimeSpan slidingExpiration) :
            this(con, new DistributedCacheEntryOptions() { SlidingExpiration = slidingExpiration })
        {

        }

        protected RedisCacheBase(IConnectionStringProvier con, DistributedCacheEntryOptions options)
        {
            this.options = options ?? new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 20, 0),
                SlidingExpiration = new TimeSpan(0, 20, 0),
            };
            string connectionString = con.GetConnectionString("redis");
            rclient = new CSRedisClient(connectionString);
            //redis = ConnectionMultiplexer.Connect(connectionString);
            //db = redis.GetDatabase();
            this.keyPrefix = string.Empty;
        }

        #endregion

        public OtherT Get<OtherT>(string key)
        {
            return InnerGet<OtherT>(key);
        }

        public bool KeyExists(string key)
        {
            string str = this.rclient.Get(this.keyPrefix + key);
            return !string.IsNullOrEmpty(str);
        }
        public void Unset(string key)
        {
            this.rclient.Del(key);
        }

        protected TValue InnerGet<TValue>(string key)
        {
            string str = this.rclient.Get(this.keyPrefix + key);
            TValue result = default(TValue);
            if (!string.IsNullOrEmpty(str))
            {
                Type type = typeof(TValue);
                if (type.IsClass || type.IsInterface)
                {
                    JsonSerializer serializer = new JsonSerializer();
                    StringReader reader = new StringReader(str);

                    object o = serializer.Deserialize(new JsonTextReader(reader), typeof(TValue));
                    result = (TValue)o;
                }
                else if (type.Name.ToLower() == "string")
                {
                    result = (TValue)(object)str;
                }
                else if (type.IsValueType)
                {
                    switch (type.FullName)
                    {
                        case "System.UInt64":
                            result = (TValue)(object)Convert.ToUInt64(str);
                            break;
                        case "System.Int64":
                            result = (TValue)(object)Convert.ToInt64(str);
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
                    this.rclient.Set(this.keyPrefix + key, str, (int)this.options.SlidingExpiration.Value.TotalSeconds);
                });
            }

            return result;
        }

        public void SetAsync(string key, object value)
        {
            Task.Run(() =>
            {
                Set(key, value);
            });
        }

        public bool Set(string key, object value)
        {
            bool result = false;
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
                result = this.rclient.Set(this.keyPrefix + key, str, 
                    (int)this.options.SlidingExpiration.Value.TotalSeconds);
            }
            else if (this.options.AbsoluteExpiration.HasValue)
            {
                result = this.rclient.Set(this.keyPrefix + key, str, (int)this.options.AbsoluteExpiration.Value.Offset.TotalSeconds);
            }
            else if (this.options.AbsoluteExpirationRelativeToNow != null)
            {
                result = this.rclient.Set(this.keyPrefix + key, str, (int) this.options.AbsoluteExpirationRelativeToNow.Value.TotalSeconds);
            }
            return result;

        }

        protected void TryOpenDb()
        {

        }
    }
}
