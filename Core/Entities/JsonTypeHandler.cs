using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Dapper;
using Newtonsoft.Json;

namespace Snowing.DDD.Core.Entities
{
    public class JsonTypeHandler : SqlMapper.ITypeHandler
    {
        public object Parse(Type destinationType, object value)
        {
            object o = JsonConvert.DeserializeObject(value as string, destinationType);
            return o;
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.DbType = DbType.String;
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
