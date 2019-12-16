using Galaxy.Libra.DapperExtensions;
using Galaxy.Libra.DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class ColumnMapper
    {
        public static void SetMapper()
        {
            //数据库字段名和c#属性名不一致，手动添加映射关系
            //SqlMapper.SetTypeMap(typeof(Settings), new ColumnAttributeTypeMapper<Settings>());
            //SqlMapper.SetTypeMap(typeof(MiniUser), new ColumnAttributeTypeMapper<MiniUser>());

            //每个需要用到[colmun(Name="")]特性的model，都要在这里添加映射
            DapperExtensions.DefaultMapper = typeof(ColumnAttributeTypeMapper<>);
            DapperExtensions.SqlDialect = new MySqlDialect();
            //DapperExtensions.DapperExtensions.InstanceFactory()
        }
    }
}
