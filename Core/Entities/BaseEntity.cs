using Galaxy.Libra.DapperExtensions.Mapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Snowing.DDD.Core.Entities
{
    public class BaseEntity<T> where T: struct
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64Unsigned)]
        public T ID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BaseEntityUInt64: BaseEntity<UInt64>
    {
        public BaseEntityUInt64()
        {
            PropertyInfo pi = this.GetType().GetProperty("ID");
            ColumnAttribute ca  = pi.GetCustomAttribute<ColumnAttribute>();
            ca.IdentityType = IdentityType.Int64Unsigned;
        }
    }


    public class BaseEntityInt64 : BaseEntity<Int64>
    {
        public BaseEntityInt64()
        {
            PropertyInfo pi = this.GetType().GetProperty("ID");
            ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            ca.IdentityType = IdentityType.Int64;
        }
    }

    public class BaseEntityUInt32 : BaseEntity<UInt32>
    {
        public BaseEntityUInt32()
        {
            PropertyInfo pi = this.GetType().GetProperty("ID");
            ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            ca.IdentityType = IdentityType.UInt32;
        }
    }

    public class BaseEntityInt32 : BaseEntity<Int32>
    {
        public BaseEntityInt32()
        {
            PropertyInfo pi = this.GetType().GetProperty("ID");
            ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            ca.IdentityType = IdentityType.Int32;
        }
    }


}
