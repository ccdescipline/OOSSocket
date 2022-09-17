using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Filter
{
    public class TestFillter : ICommandFilter
    {
        public void OnCommandExecuted(CommandExecutingContext commandContext)
        {
            
        }

        public bool OnCommandExecuting(CommandExecutingContext commandContext)
        {
            Console.WriteLine("测试过滤器");
            return true;
        }
    }
}
