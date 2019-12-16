using Snowing.DDD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Galaxy.Libra.DapperExtensions.EntityRepository;

namespace Snowing.DDD.Core.Interfaces
{
    public interface ICachedRepository<T> : IRepository<T>, ICache<T> where T : BaseEntity
    {
        bool ExistsThroughCache(ulong id);

        T GetThrouthCache(ulong id);

        void UpdateThroughCache(T entity);
    }

    public interface IRepository<T>: IBaseEntityRepository<T> where T : BaseEntity
    {
        T GetById(ulong id);
        T GetBy(ISpecification<T> spec);
        IReadOnlyList<T> GetList(ISpecification<T> spec);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        int Count(ISpecification<T> spec);
    }

    public interface ICache<T> where T : BaseEntity
    {
        /// <summary>
        /// when the value is Value-Typed, such as int, when the key doesnt exit, the Get method will return a default(T) that is 0, <br/> so we just dont know if the key exists or the value is 0. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool KeyExists(string key);

        T Get(string key);

        OtherT Get<OtherT>(string key);

        void Set(string key, object value);

        void Unset(string key);
    }
}
