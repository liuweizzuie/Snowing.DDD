using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class EnumTypeHandler<T> : SqlMapper.TypeHandler<T> where T : struct, IConvertible
    {
        static EnumTypeHandler()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumeration type");
            }
        }

        public override T Parse(object value)
        {
            return (T)Enum.Parse(typeof(T), Convert.ToString(value));
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value.ToString();
            parameter.DbType = DbType.AnsiString;
        }
    }
}
