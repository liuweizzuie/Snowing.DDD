using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    public class IDSort : ColumnSort
    {
        public IDSort()
        {
            this.PropertyName = "id";
            this.Ascending = true;
        }
    }
}
