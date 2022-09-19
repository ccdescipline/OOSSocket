using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Command
{
    public interface IPackageCommand<T>: ICommand where T : CPackage
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="package"></param>
        public CPackage? ExecuteCmd(CSocketClient socketClient, T package);
    }
}
