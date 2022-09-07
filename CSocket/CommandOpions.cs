using CSocket.Command;
using CSocket.Filter;
using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSocket
{
    public class CommandOpions
    {
        private List<Type> _CommandList;

        private List<Type> _FilterTypes;

        public CommandOpions(List<Type> Ls, List<Type> Fs)
        {
            _CommandList = Ls;
            _FilterTypes = Fs;
        }

        public List<Type> CommandList { get => _CommandList; }
        public List<Type> FilterTypes { get => _FilterTypes; }

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        public void AddCommand<T1>() where T1 : ICommand, new()
        {
            CommandList.Add(typeof(T1));
        }

        /// <summary>
        /// 添加程序集，自动扫描程序集下所有
        /// </summary>
        /// <param name="Assembly"></param>
        public void AddCommand(Assembly Assembly) {
            _CommandList.AddRange(Assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Where(s => s.IsGenericType)
                       .Select(s => s.GetGenericTypeDefinition())
                       .Contains(typeof(IPackageCommand<>))
                   ).ToList());
        }

        /// <summary>
        /// 添加全局过滤器
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        public void AddFilter<T2>() where T2 : ICommandFilter, new()
        {
            FilterTypes.Add(typeof(T2));
        }

    }
}
