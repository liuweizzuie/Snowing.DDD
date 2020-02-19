using Galaxy.Libra.DapperExtensions.Mapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Snowing.DDD.Core.Entities
{
    public abstract class BaseEntity<T> where T: struct
    {
        //[Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64Unsigned)]
        /// <summary>
        /// 需要重写此属性，将其标记为 ColumnAttribute
        /// </summary>
        public abstract T ID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BaseEntityUInt64: BaseEntity<UInt64>
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64Unsigned)]
        public override UInt64 ID { get; set; }

        public BaseEntityUInt64()
        {
            /* this doesn't work
            PropertyInfo pi = this.GetType().GetProperty("ID");
            ColumnAttribute ca  = pi.GetCustomAttribute<ColumnAttribute>();
            ca.IdentityType = IdentityType.Int64Unsigned; 
            */
            
        }
    }


    public class BaseEntityInt64 : BaseEntity<Int64>
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64)]
        public override Int64 ID { get; set; }

        public BaseEntityInt64()
        {
            //PropertyInfo pi = this.GetType().GetProperty("ID");
            //ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            //ca.IdentityType = IdentityType.Int64;
        }
    }

    public class BaseEntityUInt32 : BaseEntity<UInt32>
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.UInt32)]
        public override UInt32 ID { get; set; }
        public BaseEntityUInt32()
        {
            //PropertyInfo pi = this.GetType().GetProperty("ID");
            //ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            //ca.IdentityType = IdentityType.UInt32;
        }
    }

    public class BaseEntityInt32 : BaseEntity<Int32>
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int32)]
        public override Int32 ID { get; set; }

        public BaseEntityInt32()
        {
            //PropertyInfo pi = this.GetType().GetProperty("ID");
            //ColumnAttribute ca = pi.GetCustomAttribute<ColumnAttribute>();
            //ca.IdentityType = IdentityType.Int32;
        }
    }


}
