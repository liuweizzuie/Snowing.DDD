﻿using Dapper;
using Galaxy.Libra.DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    /// <summary>
    /// Uses the Name value of the <see cref="ColumnAttribute"/> specified to determine
    /// the association between the name of the column in the query results and the member to
    /// which it will be extracted. If no column mapping is present all members are mapped as
    /// usual.
    /// </summary>
    /// <typeparam name="T">The type of the object that this association between the mapper applies to.</typeparam>
    public class ColumnAttributeTypeMapper<T> : FallbackTypeMapper<T> where T : class
    {
        public ColumnAttributeTypeMapper()
            : base(new SqlMapper.ITypeMap[]
                {
                    new CustomPropertyTypeMap(
                       typeof(T),
                       (type, columnName) =>
                           type.GetProperties().FirstOrDefault(prop =>
                               prop.GetCustomAttributes(true)
                                   .OfType<ColumnAttribute>()
                                   .Any(attr => attr.Name == columnName)
                               )
                       ),
                    //new DefaultTypeMap(typeof(T))
                })
        {
        }
    }

    public class FallbackTypeMapper<T> : AutoClassMapper<T>, SqlMapper.ITypeMap where T : class
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;
        private static readonly IDictionary<string, string> _tableMapper;


        static FallbackTypeMapper()
        {
            _tableMapper = new Dictionary<string, string>();
        }

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;

            this.Properties.Clear();
            Type t = typeof(T);
            PropertyInfo[] pinfos = t.GetProperties();
            foreach (PropertyInfo item in pinfos)
            {
                this.Properties.Add(new ColumnMap(item));
            }
        }


        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }

        public override void Table(string tableName)
        {
            if (!_tableMapper.ContainsKey(tableName))
            {
                Type t = typeof(T);
                TableAttribute ta = t.GetCustomAttribute<TableAttribute>();
                if (ta != null)
                {
                    _tableMapper.Add(tableName, ta.Name);
                }
                else
                {
                    _tableMapper.Add(tableName, tableName);
                }
            }
            base.Table(_tableMapper[tableName]);
        }

        protected override void AutoMap(Func<Type, PropertyInfo, bool> canMap)
        {
            //canMap()
            base.AutoMap(canMap);
        }

        protected override void AutoMap()
        {
            base.AutoMap();
        }

    }
}
