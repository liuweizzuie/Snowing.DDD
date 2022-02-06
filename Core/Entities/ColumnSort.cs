using Galaxy.Libra.DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class ColumnSort : ISort
    {
        public string PropertyName { get; set; }
        public bool Ascending { get; set; }

        public ColumnSort()
        {

        }
    }
}
