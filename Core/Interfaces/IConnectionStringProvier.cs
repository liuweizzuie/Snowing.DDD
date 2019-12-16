using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Interfaces
{
    public interface IConnectionStringProvier
    {
        /// <summary>
        /// 如果应用中有多个连接，比如 redis, mysql, mysql2, mysql3 等，以 key 来区分
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConnectionString(string key);
    }
}
