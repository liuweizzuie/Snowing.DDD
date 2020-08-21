using Snowing.DDD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Libra.DapperExtensions.EntityRepository;
using System.Linq.Expressions;

namespace Snowing.DDD.Core.Interfaces
{
    public interface ICachedRepository<T, TKey> : IRepository<T, TKey>, ICache<T, TKey> where T : BaseEntity<TKey> where TKey: struct
    {
        bool ExistsThroughCache(TKey id);

        T GetThrouthCache(TKey id);

        void UpdateThroughCache(T entity);
    }

    public interface ICachedRepositoryUInt64<T> : ICachedRepository<T, UInt64> where T : BaseEntityUInt64 { }

    public interface ICachedRepositoryInt64<T>: ICachedRepository<T, Int64> where T : BaseEntityInt64 { }
    public interface ICachedRepositoryInt32<T>: ICachedRepository<T, Int32> where T : BaseEntityInt32 { }
    public interface ICachedRepositoryUInt32<T>: ICachedRepository<T, UInt32> where T : BaseEntityUInt32 { }

    public interface IRepository<T, TKey>: IBaseEntityRepository<T> where T : BaseEntity<TKey> where TKey : struct
    {
        T GetById(TKey id);
        T GetBy(ISpecification<T> spec);
        IList<T> GetList(ISpecification<T> spec);

        List<T> GetPage(int page, int resultsPerPage);

        List<T> GetPage(int page, int resultsPerPage, string orderBy, Func<string, Expression<Func<T, object>>> map, bool desc = false, ISpecification<T> spec = null);

        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        int Count(ISpecification<T> spec);

        dynamic Max(Expression<Func<T, object>> maxExp, ISpecification<T> spec);
    }

    public interface ICache<T, TKey>: ICacheBase where T : BaseEntity<TKey> where TKey : struct
    {
        T Get(string key);
    }


    public interface ICacheBase
    {
        /// <summary>
        /// when the value is Value-Typed, such as int, when the key doesnt exit, the Get method will return a default(T) that is 0, <br/> so we just dont know if the key exists or the value is 0. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool KeyExists(string key);

        OtherT Get<OtherT>(string key);

        void Set(string key, object value);

        void SetAsync(string key, object value);

        void Unset(string key);


    }
}
