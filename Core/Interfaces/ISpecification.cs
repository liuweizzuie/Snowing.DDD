using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snowing.DDD.Core.Interfaces
{
    public interface ISpecification<T>
    {
        Tuple<Expression<Func<T, object>>, object> Criteria { get; }
        List<Tuple<Expression<Func<T, object>>, object>> Includes { get; }

        List<Tuple<Expression<Func<T, object>>, object>> Contains { get; }

        List<Tuple<Expression<Func<T, object>>, object>> Or { get; }

        List<IList<Tuple<Expression<Func<T, object>>, object>>> AndOrs { get; }

        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }

        List<Tuple<Expression<Func<T, object>>, object>> Not { get; }

        int Take { get; }

        int Skip { get; }

        bool IsPagingEnabled { get; }
    }
}
