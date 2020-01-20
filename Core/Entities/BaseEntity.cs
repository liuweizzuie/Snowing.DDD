using Galaxy.Libra.DapperExtensions.Mapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class BaseEntity
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class BaseEntityUInt64: BaseEntity
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64Unsigned)]
        public UInt64 ID { get; set; }
    }


    public class BaseEntityInt64 : BaseEntity
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64)]
        public Int64 ID { get; set; }
    }

    public class BaseEntityUInt32 : BaseEntity
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.UInt32)]
        public UInt32 ID { get; set; }
    }

    public class BaseEntityInt32 : BaseEntity
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int32)]
        public Int32 ID { get; set; }
    }


}
