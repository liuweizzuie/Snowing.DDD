using Snowing.DDD.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Snowing.DDD.Infrastructure.Specifications
{
    /// <summary>
    /// 禁止直接使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public const int PageCount = 20;

        #region static
        /// <summary>
        /// 根据某列查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BaseSpecification<T> ColumnSpecification(Expression<Func<T, object>> criteria, object value)
        {
            return new ColumnSpecification<T>(criteria, value);
        }

        public static BaseSpecification<T> ColumnSpecification()
        {
            return new ColumnSpecification<T>(null, null);
        }

        public static BaseSpecification<T> ColumnPaginatedSpecification(Expression<Func<T, object>> criteria, object value, int pageIndex)
        {
            BaseSpecification<T> spec = new ColumnSpecification<T>(criteria, value);
            spec.ApplyPaging(PageCount * pageIndex, PageCount);
            return spec;
        }
        #endregion


        protected BaseSpecification(Expression<Func<T, object>> criteria, object value)
        {
            if(criteria != null)
            {
                Criteria = new Tuple<Expression<Func<T, object>>, object>(criteria, value);
            }
        }

        public Tuple<Expression<Func<T, object>>, object> Criteria { get; }
        public List<Tuple<Expression<Func<T, object>>, object>> Includes { get; } 
            = new List<Tuple<Expression<Func<T, object>>, object>>();

        public List<Tuple<Expression<Func<T, object>>, object>> Contains { get; }
            = new List<Tuple<Expression<Func<T, object>>, object>>();

        public List<Tuple<Expression<Func<T, object>>, object>> Not { get; }
            = new List<Tuple<Expression<Func<T, object>>, object>>();

        public List<Tuple<Expression<Func<T, object>>, object>> Or { get; }
            = new List<Tuple<Expression<Func<T, object>>, object>>();

        public List<Tuple<Expression<Func<T, object>>, object>> Likes { get; }
            = new List<Tuple<Expression<Func<T, object>>, object>>();


        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        public List<IList<Tuple<Expression<Func<T, object>>, object>>> AndOrs { get; set; } 
            = new List<IList<Tuple<Expression<Func<T, object>>, object>>>();


        public virtual void AddInclude(Expression<Func<T, object>> includeExpression, object value)
        {
            Includes.Add(new Tuple<Expression<Func<T, object>>, object>(includeExpression, value));
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        //Not used anywhere at the moment, but someone requested an example of setting this up.
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }



    }
}
