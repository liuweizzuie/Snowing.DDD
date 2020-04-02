using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Galaxy.Libra.DapperExtensions.PredicateConver;

namespace Snowing.DDD.Infrastructure.Specifications
{
    public class ExpressionSort<T>:ISort
    {
        protected Expression<Func<T, object>> property;
        public string PropertyName 
        {
            get 
            {
                string propertyName = "ID";
                try{
                    propertyName = ExpressionPredicateConvert.PropertyName<T>(property);
                }
                catch (Exception)
                {
                    
                }
                return propertyName;
            }
            set { throw new NotSupportedException(); }
        }
        public bool Ascending { get; set; }

        public ExpressionSort(Expression<Func<T, object>> property)
        {
            this.property = property;
        }
    }
}
