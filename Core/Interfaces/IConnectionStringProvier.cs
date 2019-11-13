using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Interfaces
{
    public interface IConnectionStringProvier
    {
        string GetConnectionString();
    }
}
