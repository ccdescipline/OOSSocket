using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.PackageTemplate
{
    /// <summary>
    /// 字符串数据包
    /// </summary>
    //[PackageAttribute("StringPackage")]
    [PackageAttribute]
    public class StringPackage: CPackage
    {
        private string body;

        public StringPackage()
            //: base("StringPackage")
        {
        }

        public StringPackage(string body)
            //:base("StringPackage")
        {
            this.Body = body;
        }

        public string Body { get => body; set => body = value; }
    }
}
