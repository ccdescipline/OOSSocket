using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSocket.Command
{
    public class BitCommand : IPackageCommand<Bitpackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, Bitpackage package)
        {
            List<string> vs = new List<string>();
            foreach (var bitpackage in package.Data)
            {

                vs.Add(vs.ToString());
            }

            Console.WriteLine($"byte [] : {String.Join(',', vs.ToArray())}");

            return package;
        }
    }
}
