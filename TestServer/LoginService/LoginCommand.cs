using CSocket;
using CSocket.Command;
using CSocket.Interface;
using CSocket.PackageTemplate;
using SharedPackage.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServer.LoginService
{
    /// <summary>
    /// 模拟登录
    /// </summary>
    internal class LoginCommand : IPackageCommand<LoginPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, LoginPackage package)
        {
            socketClient.Session["Userinfo"] = package;

            Console.WriteLine($"userinfo {package.Account} , {package.Password} 登录！");

            return new CommandPackage("Login -status 200");
        }
    }
}
