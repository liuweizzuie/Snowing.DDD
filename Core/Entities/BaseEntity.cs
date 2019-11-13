using Galaxy.Libra.DapperExtensions.Mapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class BaseEntity
    {
        [Column("id", KeyType = KeyType.Identity, IsAutoIncrement = true, IdentityType = IdentityType.Int64Unsigned)]
        public ulong ID { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
