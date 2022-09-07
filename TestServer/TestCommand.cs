using CSocket;
using CSocket.Command;
using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestServer
{
    internal class TestCommand : IPackageCommand<StringPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, StringPackage package)
        {
            return new StringPackage("TestCommand收到");
        }
    }
}
