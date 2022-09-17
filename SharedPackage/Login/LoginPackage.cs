using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPackage.Login
{
    /// <summary>
    /// 登录包
    /// </summary>
    public class LoginPackage:CPackage
    {
        private string account;
        private string password;

        public LoginPackage()
        {
        }
        public LoginPackage(string account, string password)
        {
            this.account = account;
            this.password = password;
        }

        public string Account { get => account; set => account = value; }
        public string Password { get => password; set => password = value; }
    }
}
