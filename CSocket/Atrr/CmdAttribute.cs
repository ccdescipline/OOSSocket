using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Atrr
{
    internal class CmdAttribute : Attribute
    {
        public string _instruct;

        public CmdAttribute(string instruct = "")
        {
            _instruct = instruct;
        }
    }
}
