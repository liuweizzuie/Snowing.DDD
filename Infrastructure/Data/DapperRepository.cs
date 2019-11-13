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

namespace Snowing.DDD.Infrastructure.Data
{
    public class DapperRepository<T> : BaseEntityRepository<T>, IRepository<T> where T : BaseEntity
    {
        IConnectionProvider provider;
        public DapperRepository(IConnectionProvider provider)
        {
            this.provider = provider;
        }

        #region Gets
        public T GetBy(ISpecification<T> spec)
        {
            T t = default(T);
            using (IDbConnection cn = this.provider.NewConnection())
            {
                cn.Open();
                PredicateGroup gp = SpecificationEvaluator<T>.GetQuery(spec);
                IList<T> list = cn.GetList<T>(gp).ToList();
                if (list.Count > 0)
                {
                    t = list[0];
                }
                cn.Close();
            }
            return t;
        }

        public T GetById(ulong id)
        {
            T t = default(T);
            using (IDbConnection cn = this.provider.NewConnection())
            {
                cn.Open();
                t = cn.Get<T>(id);
                cn.Close();
            }
            return t;
        }

        public IReadOnlyList<T> GetList(ISpecification<T> spec=null)
        {
            IReadOnlyList<T> list = new List<T>();
            IList<ISort> sorts = new List<ISort>();
            if (spec.OrderBy != null)
            {
                sorts.Add(Predicates.Sort(spec.OrderBy));
            }
            else if (spec.OrderByDescending != null)
            {
                sorts.Add(Predicates.Sort(spec.OrderByDescending, false));
            }
            PredicateGroup gp = SpecificationEvaluator<T>.GetQuery(spec);
            if (spec.IsPagingEnabled)
            {
                list = this.GetPage(spec.Skip / BaseSpecification<T>.PageCount, BaseSpecification<T>.PageCount, sorts, gp);
            }
            else
            {
                list = this.GetList(gp);
            }
            return list;
        }
        #endregion

        public int Count(ISpecification<T> spec)
        {
            int count = 0;
            PredicateGroup gp = SpecificationEvaluator<T>.GetQuery(spec);
            count = base.Count(predicate: gp);
            return count;
        }

        public void DeleteAsync(T entity)
        {
            Task.Run(() => 
            {
                base.Delete(t => t.ID == entity.ID);
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
