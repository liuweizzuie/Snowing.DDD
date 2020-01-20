using System;
using System.Collections.Generic;
using System.Text;
using Snowing.DDD.Core.Entities;

namespace Snowing.DDD.Core.Interfaces
{
    public interface ICacheProvider
    {
        ICache<T, TKey> GetCache<T, TKey>() where T : BaseEntity<TKey> where TKey: struct;
    }
}
