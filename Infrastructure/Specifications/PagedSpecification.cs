using Snowing.DDD.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snowing.DDD.Infrastructure.Specifications
{
    public class PagedSpecification<T>: BaseSpecification<T>, IPagedSpecification<T>
    {
        protected Func<string, Expression<Func<T, object>>> map;

        /// <summary>
        /// 为什么要有map？因为从前端接收到的按某列排序，列名(propertyName)可能并非数据库列名，也非实体的属性名(Property)，这就需要使用 map 查找 实体的属性表达式
        /// </summary>
        /// <param name="map"></param>
        public PagedSpecification(Func<string, Expression<Func<T, object>>> map) : base(null, null) 
        {
            this.map = map;
        }

        public void ApplyOrderBy(string propertyName, bool desc = false)
        {
            Expression<Func<T, object>> exp = this.map(propertyName);

            if (desc)
            {
                if(exp == null)
                {
                    exp = t => typeof(T); //随便写个，确保不为空
                }
                this.ApplyOrderByDescending(exp);
            }
            else
            {
                this.ApplyOrderBy(exp);
            }
        }

        public void Apply(int skip, int take)
        {
            base.ApplyPaging(skip, take);
        }
    }
}
