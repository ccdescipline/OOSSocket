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
        private List<ICommand> _CommandList;

        private List<ICommandFilter> _FilterTypes;

        public CommandOpions(List<ICommand> Ls, List<ICommandFilter> Fs)
        {
            _CommandList = Ls;
            _FilterTypes = Fs;
        }

        public List<ICommand> CommandList { get => _CommandList; }
        public List<ICommandFilter> FilterTypes { get => _FilterTypes; }

        /// <summary>
        /// 添加命令
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        public void AddCommand<T1>() where T1 : ICommand, new()
        {
            CommandList.Add(Activator.CreateInstance(typeof(T1))as ICommand);
        }

        /// <summary>
        /// 添加程序集，自动扫描程序集下所有
        /// </summary>
        /// <param name="Assembly"></param>
        public void AddCommand(Assembly Assembly) {
            List<Type> commandList =  (Assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Where(s => s.IsGenericType)
                       .Select(s => s.GetGenericTypeDefinition())
                       .Contains(typeof(IPackageCommand<>))
                   ).ToList());

            foreach (Type item in commandList)
            {
                CommandList.Add(Activator.CreateInstance(item) as ICommand);
            }
        }

        /// <summary>
        /// 添加全局过滤器
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        public void AddFilter<T2>() where T2 : ICommandFilter, new()
        {
            FilterTypes.Add(Activator.CreateInstance(typeof(T2)) as ICommandFilter);
        }

    }
}
