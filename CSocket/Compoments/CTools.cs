using CSocket.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// 接口遍历出所有继承的子类
        /// </summary>
        /// <typeparam name="Interface">接口</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetClassByInterFace(Type Interface, Assembly? assembly = null)
        {
            //如果为空则是接口的程序集
            if (assembly == null)
            {
                assembly = Interface.Assembly;
            }
            return Interface.IsGenericType ?
                assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Where(s => s.IsGenericType)
                       .Select(s => s.GetGenericTypeDefinition())
                       .Contains(Interface)
                   )
                :
                assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Contains(Interface)
                   );

        }
    }
}
