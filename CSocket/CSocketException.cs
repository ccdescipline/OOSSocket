using CSocket.Compoments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket
{
    internal class CSocketException:Exception
    {
        public CSocketException(string msg):
            base(msg)
        {
            //格式化输出
            CLog.Print(msg);
        }
    }
}
