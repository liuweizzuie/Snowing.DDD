using Galaxy.Libra.DapperExtensions.Predicate;
using Snowing.DDD.Core.Entities;
using Snowing.DDD.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowing.DDD.Infrastructure.Specifications
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static PredicateGroup GetQuery(ISpecification<T> specification)
        {
            //var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            //if (specification.Criteria != null)
            //{
            //    query = query.Where(specification.Criteria);
            //}
            PredicateGroup predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            if (specification.Criteria != null)
            {
                IPredicate pr = Predicates.Field<T>(specification.Criteria.Item1, Operator.Eq, specification.Criteria.Item2);
                predicateGroup.Predicates.Add(pr);
            }

            // Includes all expression-based includes
            specification.Includes.Aggregate(predicateGroup, (current, include) =>
            {
                IPredicate pr = Predicates.Field<T>(include.Item1, Operator.Eq, include.Item2);
                predicateGroup.Predicates.Add(pr);
                return predicateGroup;
            });

            //// Include any string-based include statements
            //query = specification.IncludeStrings.Aggregate(query,
            //                        (current, include) => current.Include(include));

            // Apply ordering if expressions are set
            //if (specification.OrderBy != null)
            //{
            //    //predicateGroup.Predicates.Where()
            //    //query = query.OrderBy(specification.OrderBy);
            //}
            //else if (specification.OrderByDescending != null)
            //{
            //    query = query.OrderByDescending(specification.OrderByDescending);
            //}

            //if (specification.GroupBy != null)
            //{
            //    query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            //}

            //// Apply paging if enabled
            //if (specification.IsPagingEnabled)
            //{
            //    query = query.Skip(specification.Skip)
            //                 .Take(specification.Take);
            //}

            if (specification.IsPagingEnabled)
            {

            }
            //return query;
            return predicateGroup;
        }
    }
}
