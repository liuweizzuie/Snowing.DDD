using Snowing.DDD.Core.Entities;
using Snowing.DDD.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Infrastructure.Data
{
    class CachedDapperRepository<T> : DapperRepository<T>, ICachedRepository<T> where T : BaseEntity
    {
        protected ICache<T> cache;
        protected IConnectionProvider provider;
        
        public CachedDapperRepository(ICacheProvider cacheProvider, IConnectionProvider connProvider):base(connProvider)
        {
            this.cache = cacheProvider.GetCache<T>();
            this.provider = connProvider;
        }

        public bool ExistsThroughCache(ulong id)
        {
            bool result = false;
            if (this.cache.KeyExists(id.ToString()))
            {
                T value = this.GetById(id);
                result = value != null;
                if(result)
                {
                    this.cache.Set(id.ToString(), value);
                }
            }
            return result;
        }

        public T GetThrouthCache(ulong id)
        {
            T result = default(T);
            if (!this.KeyExists(id.ToString()))
            {
                result = this.GetById(id);
                if(result != null)
                {
                    this.Set(id.ToString(), result);
                }
            }
            else
            {
                result = this.Get(id.ToString());
            }
            return result;
        }

        #region ICache  
        public T Get(string key)
        {
            return this.cache.Get(key);
        }

        public OtherT Get<OtherT>(string key)
        {
            return this.cache.Get<OtherT>(key);
        }

        public bool KeyExists(string key)
        {
            return this.cache.KeyExists(key);
        }

        public void Set(string key, object value)
        {
            this.cache.Set(key, value);
        }

        public void Unset(string key)
        {
            this.cache.Unset(key);
        }
        #endregion

    }
}
