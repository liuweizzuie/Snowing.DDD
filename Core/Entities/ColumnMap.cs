using Galaxy.Libra.DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Snowing.DDD.Core.Entities
{
    public class ColumnMap : IPropertyMap
    {
        public string Name { get; set; }

        public string ColumnName { get; set; }

        public bool Ignored { get; protected set; }

        public bool IsReadOnly { get; protected set; }

        public KeyType KeyType { get; set; }

        public PropertyInfo PropertyInfo { get; protected set; }

        public int ColumnLength { get; set; }

        public bool IsRequired { get; set; }

        public bool IsAutoIncrement { get; set; }

        public IdentityType IdentityType { get; set; }

        public ColumnMap(PropertyInfo propertyInfo)
        {
            this.Name = propertyInfo.Name;
            this.IsReadOnly = false;
            this.Ignored = false;
            this.PropertyInfo = PropertyInfo;
            this.KeyType = KeyType.NotAKey;
            this.ColumnLength = 64;
            this.IsRequired = false;
            this.IsAutoIncrement = false;
            this.PropertyInfo = propertyInfo;
            this.IdentityType = IdentityType.Int64Unsigned;

            ColumnAttribute ca = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (ca != null)
            {
                this.ColumnName = ca.Name;
                this.KeyType = ca.KeyType;
            }
            else
            {
                this.ColumnName = this.Name;
            }
        }
    }

}
