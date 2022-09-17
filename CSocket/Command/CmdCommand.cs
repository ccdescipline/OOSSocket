using CSocket.Attr;
using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSocket.Command
{
    public class CmdCommand : IPackageCommand<CommandPackage> 
    {

        public CPackage? ExecuteCmd(CSocketClient socketClient , CommandPackage package )
        {
            
            //Console.WriteLine(this.GetType().Name);

            //判断是否是继承类
            if (this.GetType() == typeof(CmdCommand))
            {
                //否
                return OnExecuteCmd(socketClient, package);
            }
            else
            {
                //是
                return CheckInstruct(socketClient, package);
            }

            

           // return null;
            //socketClient.SendPackage(package);
        }

        /// <summary>
        /// 重写执行的事件
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        protected virtual CPackage? OnExecuteCmd(CSocketClient socketClient, CommandPackage package)
        {
            Console.WriteLine($"指令名： {package.Instruct}");
            foreach (var item in package.Parameters)
            {
                Console.WriteLine($"参数名：[{item.Key}] ，参数值 ：[{item.Value}]");
            }
            return package;
        }

        private CPackage? CheckInstruct(CSocketClient socketClient, CommandPackage package)
        {
            string _instruct;
            Type thistype = this.GetType();

            //获取特性
            CmdAttribute? cmdAttribute = thistype.GetCustomAttribute<CmdAttribute>();
            if (cmdAttribute == null)
            {
                throw new CSocketException($"{this.GetType().Name} 缺少 'CmdAttribute' 特性，{this.GetType().Name} not have 'CmdAttribute' Attribute");
            }

            if (string.IsNullOrEmpty(cmdAttribute._instruct))
            {
                _instruct = thistype.Name;
            }
            else
            {
                _instruct = cmdAttribute._instruct;
            }
            

            //判断命令名是否匹配
            if (package.Instruct == _instruct)
            {
                return OnExecuteCmd(socketClient, package);
            }

            return null;
        }
    }
}
