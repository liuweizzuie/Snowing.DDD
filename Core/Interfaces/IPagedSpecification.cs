using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Interfaces
{
    public interface IPagedSpecification<T>: ISpecification<T>
    {
        void ApplyOrderBy(string propertyName, bool desc = false);

        void Apply(int skip, int take);
    }
}
