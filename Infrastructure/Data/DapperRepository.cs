using Galaxy.Libra.DapperExtensions.EntityRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Snowing.DDD.Core.Entities;
using Snowing.DDD.Core.Interfaces;
using System.Data.Common;
using Galaxy.Libra.DapperExtensions.Predicate;
using System.Linq;
using Galaxy.Libra.DapperExtensions;
using Snowing.DDD.Infrastructure.Specifications;
using System.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Snowing.DDD.Infrastructure.Data
{
    public class DapperRepository<T, TKey> : BaseEntityRepository<T>, IRepository<T,TKey> where T : BaseEntity<TKey> where TKey: struct
    {
        IConnectionProvider provider;
        protected string ConnectionKey = string.Empty;
        public DapperRepository(IConnectionKeyProvider keyProvider, IConnectionProvider provider)
        {
            this.provider = provider;
            if(keyProvider == null)
            {
                this.ConnectionKey = "mysql";
            }
            else
            {
                this.ConnectionKey = keyProvider.Key;
            }
            this.curDbConnection = provider.NewConnection(this.ConnectionKey);
        }


        public override dynamic Add(T entity)
        {
            dynamic result;
            lock (this.curDbConnection)
            {
                result = base.Add(entity);
            }
            return result;
        }


        #region Gets
        public T GetBy(ISpecification<T> spec)
        {
            T t = default(T);
            using (IDbConnection cn = this.provider.NewConnection(this.ConnectionKey))
            {
                cn.Open();
                PredicateGroup gp = SpecificationEvaluator<T, TKey>.GetQuery(spec);
                IList<T> list = cn.GetList<T>(gp).ToList();
                if (list.Count > 0)
                {
                    t = list[0];
                }
                cn.Close();
            }
            return t;
        }

        public T GetById(TKey id)
        {
            T t = default(T);
            using (IDbConnection cn = this.provider.NewConnection(this.ConnectionKey))
            {
                cn.Open();
                t = cn.Get<T>(id);
                cn.Close();
            }
            return t;
        }

        public IList<T> GetList(ISpecification<T> spec=null)
        {
            IList<T> list = new List<T>();
            PredicateGroup gp = SpecificationEvaluator<T, TKey>.GetQuery(spec);
            if (spec.IsPagingEnabled)
            {
                IList<ISort> sorts = new List<ISort>();
                if (spec.OrderBy != null)
                {
                    sorts.Add(Predicates.Sort(spec.OrderBy));
                }
                else if (spec.OrderByDescending != null)
                {
                    sorts.Add(Predicates.Sort(spec.OrderByDescending, false));
                }

                if (sorts.Count == 0)
                {
                    sorts.Add(new Sort() { PropertyName = "ID" });
                }
                list = this.GetPage(spec.Skip / BaseSpecification<T>.PageCount, BaseSpecification<T>.PageCount, sorts, gp);
            }
            else
            {
                list = this.GetList(gp);
            }
            return list;
        }

        public List<T> GetPage(int page, int resultsPerPage)
        {
            return base.GetPage(page, resultsPerPage,
                new List<ISort>() { new Sort() { Ascending = true, PropertyName = "ID" } });
        }

        public List<T> GetPage(int page, int resultsPerPage, string orderBy, Func<string, Expression<Func<T, object>>> map, bool desc = false, ISpecification<T> spec = null)
        {
            PagedSpecification<T> spec1 = new PagedSpecification<T>(map);
            spec1.ApplyOrderBy(orderBy, desc);
            spec1.Apply(page * resultsPerPage, resultsPerPage);
            if(spec == null)
            {
                return base.GetPage(page, resultsPerPage,
                                new List<ISort>() { SpecificationEvaluator<T, TKey>.GetSort(spec1) });
            }
            else
            {
                return base.GetPage(page, resultsPerPage,
                new List<ISort>() { SpecificationEvaluator<T, TKey>.GetSort(spec1) },
                SpecificationEvaluator<T, TKey>.GetQuery(spec));
            }
        }

        #endregion

        public int Count(ISpecification<T> spec)
        {
            int count = 0;
            PredicateGroup gp = SpecificationEvaluator<T, TKey>.GetQuery(spec);
            count = base.Count(predicate: gp);
            return count;
        }

        public dynamic Max(Expression<Func<T, object>> maxExp, ISpecification<T> spec)
        {
            PredicateGroup gp = SpecificationEvaluator<T, TKey>.GetQuery(spec);
            dynamic result = base.Max(maxExp, predicate: gp);
            return result;
        }

        public dynamic Sum(Expression<Func<T, object>> maxExp, ISpecification<T> spec)
        {
            PredicateGroup gp = SpecificationEvaluator<T, TKey>.GetQuery(spec);
            dynamic result = base.Sum(maxExp, predicate: gp);
            return result;
        }

        public void DeleteAsync(T entity)
        {
            Task.Run(() => 
            {
                base.Delete(t => object.Equals(t.ID, entity.ID));
            });
        }

        public void UpdateAsync(T entity)
        {
            Task.Run(() =>
            {
                base.Update(entity);
            });
        }
    }
}
