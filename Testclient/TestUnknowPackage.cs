using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testclient
{
    internal class TestUnknowPackage:CPackage
    {
        string teststr;

        public TestUnknowPackage(string teststr)
        {
            this.teststr = teststr;
        }

        public string Teststr { get => teststr; set => teststr = value; }
    }
}
