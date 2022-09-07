using CSocket.Command;
using CSocket.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    public class LoginFilter : ICommandFilter
    {
        public void OnCommandExecuted(CommandExecutingContext commandContext)
        {
        }

        public bool OnCommandExecuting(CommandExecutingContext commandContext)
        {
            Console.WriteLine("请求Cmdcommand 被拦截");

            return false;
        }
    }
}
