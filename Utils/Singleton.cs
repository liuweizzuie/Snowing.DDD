using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowing.DDD.Utils
{
    public class Singleton<T> where T : class
    {
        protected static T instance;

        protected static object locker = new object();

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            IEnumerable<ConstructorInfo> constructors = typeof(T).GetTypeInfo().DeclaredConstructors;
                            ConstructorInfo ci = constructors.ToList()[0];
                            instance = ci.Invoke(new object[] { }) as T;
                        }
                    }
                }
                return instance;
            }
        }

    }
}
