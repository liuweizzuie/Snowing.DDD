using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.DDD.Core.Entities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableAttribute : Attribute
    {
        /// <summary>Gets the name of the table the class is mapped to.</summary>
        /// <returns>The name of the table the class is mapped to.</returns>
        public string Name { get; protected set; }
        

        /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.DataAnnotations.Schema.TableAttribute" /> class using the specified name of the table.</summary>
        /// <param name="name">The name of the table the class is mapped to.</param>
        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
