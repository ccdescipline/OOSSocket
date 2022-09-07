using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Attr
{
    internal class CmdAttribute:Attribute
    {
        public string _instruct;

        public CmdAttribute(string instruct = "")
        {
            this._instruct = instruct;
        }
    }
}
