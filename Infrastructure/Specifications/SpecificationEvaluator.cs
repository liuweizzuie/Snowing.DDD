using Galaxy.Libra.DapperExtensions.Predicate;
using Snowing.DDD.Core.Entities;
using Snowing.DDD.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowing.DDD.Infrastructure.Specifications
{
    public class SpecificationEvaluator<T, TKey> where T : BaseEntity<TKey> where TKey: struct
    {
        public static PredicateGroup GetQuery(ISpecification<T> specification)
        {
            //var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            PredicateGroup predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            if (specification.Criteria != null && specification.Criteria.Item1 != null)
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

            specification.Contains.Aggregate(predicateGroup, (current, include) =>
            {
                IPredicate pr = Predicates.Field<T>(include.Item1, Operator.Like, include.Item2);
                predicateGroup.Predicates.Add(pr);
                return predicateGroup;
            });

            specification.Not.Aggregate(predicateGroup, (current, include) =>
            {
                IPredicate pr = Predicates.Field<T>(include.Item1, Operator.Eq, include.Item2, true);
                predicateGroup.Predicates.Add(pr);
                return predicateGroup;
            });

            if(specification.Or.Count > 0)
            {
                PredicateGroup orGroup = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                specification.Or.Aggregate(predicateGroup, (current, include) =>
                {
                    IPredicate pr = Predicates.Field<T>(include.Item1, Operator.Eq, include.Item2);
                    orGroup.Predicates.Add(pr);
                    return orGroup;
                });
                predicateGroup.Predicates.Add(orGroup);
            }

            //// Include any string-based include statements
            //query = specification.IncludeStrings.Aggregate(query,
            //                        (current, include) => current.Include(include));

            // Apply ordering if expressions are set

            //if (specification.OrderBy != null)
            //{ 
            //    PredicateGroup orGroup = new PredicateGroup { , Predicates = new List<IPredicate>() };
            //    predicateGroup.Predicates.Add(orGroup);
            //}
            //else if (specification.OrderByDescending != null)
            //{
            //    PredicateGroup orGroup = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //    //query = query.OrderByDescending(specification.OrderByDescending);
            //    predicateGroup.Predicates.Add(orGroup);
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

        public static ISort GetSort(ISpecification<T> specification)
        {
            if (specification.OrderBy != null)
            {
                return new ExpressionSort<T>(specification.OrderBy) { Ascending = true };
            } else if(specification.OrderByDescending != null)
            {
                return new ExpressionSort<T>(specification.OrderByDescending) { Ascending = false };
            }

            return new Sort() { PropertyName = "ID", Ascending = true };
        }
    }
}
