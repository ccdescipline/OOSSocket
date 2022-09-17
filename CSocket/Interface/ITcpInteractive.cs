using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Interface
{
    public interface ITcpInteractive
    {
        /// <summary>
        /// 无返回值发送
        /// </summary>
        /// <param name="package"></param>
        void SendPackage(CPackage package);

        /// <summary>
        /// 指定返回值的发送
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <param name="package"></param>
        /// <returns></returns>
        Task<ResultType?> SendPackage<ResultType>(CPackage package)
             where ResultType : CPackage, new();
    }
}
