using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Snowing.DDD.Core.Interfaces
{
    public interface IConnectionProvider
    {
        public IDbConnection NewConnection(string key);
    }

    public interface IConnectionKeyProvider
    {
        string Key { get; }
    }
}
