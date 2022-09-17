using CSocket.Command;
using CSocket.Compoments;
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
            if (CommandList.Where((x) => { return x.GetType() == typeof(T1); }).Count()>0)
            {
                throw new CSocketException("该command已在其中 the command is already contains");
                return;
            }
            CommandList.Add(Activator.CreateInstance(typeof(T1))as ICommand);
        }

        /// <summary>
        /// 添加程序集，自动扫描程序集下所有
        /// </summary>
        /// <param name="Assembly"></param>
        public void AddCommandByAssembly(Assembly Assembly) {
            //List<Type> commandList =  (Assembly.GetTypes()
            //       .Where(s => s.GetInterfaces()
            //           .Where(s => s.IsGenericType)
            //           .Select(s => s.GetGenericTypeDefinition())
            //           .Contains(typeof(IPackageCommand<>))
            //       ).ToList());

            List<Type> commandList = CTools.GetClassByInterFace(typeof(IPackageCommand<>), Assembly).ToList();

            foreach (Type item in commandList)
            {
                this.GetType().GetMethod("AddCommand")
                    ?.MakeGenericMethod(new Type[] { item })
                    .Invoke(this,null);
                //CommandList.Add(Activator.CreateInstance(item) as ICommand);
            }
        }

        /// <summary>
        /// 添加全局过滤器
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        public void AddFilter<T2>() where T2 : ICommandFilter, new()
        {
            if (CommandList.Where((x) => { return x.GetType() == typeof(T2); }).Count() > 0)
            {
                throw new CSocketException("该Filter已在其中 the Filter is already contains");
                return;
            }
            FilterTypes.Add(Activator.CreateInstance(typeof(T2)) as ICommandFilter);
        }

        public void AddFilterByAssembly(Assembly Assembly)
        {
            //List<Type> FilterList = (Assembly.GetTypes()
            //       .Where(s => s.GetInterfaces()
            //           .Contains(typeof(ICommandFilter))
            //       ).ToList());

            List<Type> FilterList = CTools.GetClassByInterFace(typeof(ICommandFilter), Assembly).ToList();

            foreach (Type item in FilterList)
            {
                this.GetType().GetMethod("AddFilter")
                    ?.MakeGenericMethod(new Type[] { item })
                    .Invoke(this, null);
                //FilterTypes.Add(Activator.CreateInstance(item) as ICommandFilter);
            }
        }

    }
}
