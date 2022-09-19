using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Command
{
    public class StringCommand : IPackageCommand<StringPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, StringPackage package)
        {
            Console.WriteLine($"包内容是：{package.Body}");
            return package;
        }
    }
}
