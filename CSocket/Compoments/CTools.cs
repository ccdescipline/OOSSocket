using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Compoments
{
    public class CTools
    {
        public static Type[] GetPersonClassByInterface<T>()
        {
            return typeof(T).Assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Where(s => s.IsGenericType)
                       .Select(s => s.GetGenericTypeDefinition())
                       .Contains(typeof(T))
                   ).ToArray();
        }
    }
}
