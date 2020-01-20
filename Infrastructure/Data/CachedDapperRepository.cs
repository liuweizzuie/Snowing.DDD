using Snowing.DDD.Core.Entities;
using Snowing.DDD.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Infrastructure.Data
{
    public class CachedDapperRepository<T, TKey> : DapperRepository<T, TKey>, ICachedRepository<T, TKey> where T : BaseEntity<TKey> where TKey: struct
    {
        protected ICache<T, TKey> cache;
        protected IConnectionProvider provider;
        
        public CachedDapperRepository(ICacheProvider cacheProvider,
            IConnectionKeyProvider keyProvider,
            IConnectionProvider connProvider):base(keyProvider, connProvider)
        {
            this.cache = cacheProvider.GetCache<T, TKey>();
            this.provider = connProvider;
        }

        public bool ExistsThroughCache(TKey id)
        {
            bool result = false;
            if (!this.cache.KeyExists(id.ToString()))
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

        public T GetThrouthCache(TKey id)
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

        public void UpdateThroughCache(T entity)
        {
            string key = entity.ID.ToString();
            if (this.KeyExists(key))
            {
                this.cache.Set(key, entity);
            }

            this.UpdateAsync(entity);
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

        public void SetAsync(string key, object value)
        {
            this.cache.SetAsync(key, value);
        }
        #endregion

    }

    public class CachedDapperRepository64<T>: CachedDapperRepository<T, Int64>, ICachedRepositoryInt64<T> where T: BaseEntityInt64
    {
        public CachedDapperRepository64(ICacheProvider cacheProvider,
            IConnectionKeyProvider keyProvider,
            IConnectionProvider connProvider):base(cacheProvider, keyProvider, connProvider)
        {

        }
    }

    public class CachedDapperRepositoryU64<T> : CachedDapperRepository<T, UInt64>, ICachedRepositoryUInt64<T> where T : BaseEntityUInt64
    {
        public CachedDapperRepositoryU64(ICacheProvider cacheProvider,
            IConnectionKeyProvider keyProvider,
            IConnectionProvider connProvider) : base(cacheProvider, keyProvider, connProvider)
        {

        }
    }

    public class CachedDapperRepository32<T> : CachedDapperRepository<T, Int32>, ICachedRepositoryInt32<T> where T : BaseEntityInt32
    {
        public CachedDapperRepository32(ICacheProvider cacheProvider,
            IConnectionKeyProvider keyProvider,
            IConnectionProvider connProvider) : base(cacheProvider, keyProvider, connProvider)
        {

        }
    }

    public class CachedDapperRepositoryU32<T> : CachedDapperRepository<T, UInt32>, ICachedRepositoryUInt32<T> where T : BaseEntityUInt32
    {
        public CachedDapperRepositoryU32(ICacheProvider cacheProvider,
            IConnectionKeyProvider keyProvider,
            IConnectionProvider connProvider) : base(cacheProvider, keyProvider, connProvider)
        {

        }
    }
}
