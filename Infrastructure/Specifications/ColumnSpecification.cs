using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snowing.DDD.Infrastructure.Specifications
{
    public class ColumnSpecification<T> : BaseSpecification<T>
    {
        public ColumnSpecification(Expression<Func<T, object>> column, object value) : base(column, value) { }
    }
}
