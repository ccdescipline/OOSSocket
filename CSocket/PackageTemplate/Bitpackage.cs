using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.PackageTemplate
{
    public class Bitpackage:CPackage
    {
        public byte[] Data { get; set; }

        public Bitpackage()
        //: base("Bitpackage")
            {
            }

        public Bitpackage(byte[] Data)
            //: base("Bitpackage")
        {
            this.Data = Data;
        }
    }
}
