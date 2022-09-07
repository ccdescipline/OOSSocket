
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Filter
{
    public interface ICommandFilter
    {
        /// <summary>
        /// 命令执行后
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        void OnCommandExecuted(CommandExecutingContext commandContext);

        /// <summary>
        /// 命令执行前
        /// </summary>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        bool OnCommandExecuting(CommandExecutingContext commandContext);
    }
}
