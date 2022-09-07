using CSocket.Attr;
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
    /// <summary>
    /// 重连命令
    /// </summary>
    [CmdAttribute]
    internal sealed class ReconnectCommand : CmdCommand
    {
        protected override CPackage? OnExecuteCmd(CSocketClient socketClient, CommandPackage package)
        {
            //获取和设置clientID
            Console.WriteLine("ReconnectCommand!");
            //ReconnectCommand -GetClientID 0
            if (package.Parameters.Keys.Contains("GetClientID"))
            {
                return new CommandPackage($"ReconnectCommand -ClientID {socketClient.ID}");
            }

            if (package.Parameters.Keys.Contains("SetClientID"))
            {
                socketClient.ResetID(package.Parameters["SetClientID"].ToString());
                return new CommandPackage($"ReconnectCommand -Status 200");
            }
            return package;
        }

    }
}
