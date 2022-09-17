using CSocket;
using CSocket.Command;
using CSocket.Filter;
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
    /// 登陆过滤器
    /// </summary>
    public class LoginFilter : ICommandFilter
    {
        public void OnCommandExecuted(CommandExecutingContext commandContext)
        {
        }

        public bool OnCommandExecuting(CommandExecutingContext commandContext)
        {
            LoginPackage? userinfo = null;
            try
            {
                userinfo = (commandContext.CurrentContext as CSocketClient).Session["Userinfo"] as LoginPackage;
                //Console.WriteLine($"userinfo {userinfo.Account} , {userinfo.Password} 登录！");
                return true;
            }
            catch (KeyNotFoundException e)
            {
                //判断是否LoginPackage
                if (!CPackage.IsEqualpackage(commandContext.package, typeof(LoginPackage)))
                {
                    
                    Console.WriteLine("用户未登录，流量拦截！");
                    return false;
                }
            }

            return true;
        }
    }
}
