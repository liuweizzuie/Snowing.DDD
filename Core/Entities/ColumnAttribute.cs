using Galaxy.Libra.DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public KeyType KeyType { get; set; }
        public IdentityType IdentityType { get; set; }

        public bool IsAutoIncrement { get; set; }

        public ColumnAttribute(string name)
        {
            this.Name = name;
            this.KeyType = KeyType.NotAKey;
        }
    }

    public class ColumnIgnoreAttribute : Attribute { }
}
