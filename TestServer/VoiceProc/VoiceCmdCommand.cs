using CSocket;
using CSocket.Command;
using CSocket.Interface;
using CSocket.PackageTemplate;
using SharedPackage.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestServer.VoiceProc
{
    internal class VoiceCmdCommand : IPackageCommand<CommandPackage>
    {
        /// <summary>
        /// 处理声音命令
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        public CPackage? ExecuteCmd(CSocketClient socketClient, CommandPackage package)
        {
            if (package.Instruct != "VoiceCMD")
            {
                return null;
            }

            //获取所有房间信息
            if (package.Parameters.Keys.Contains("GetRoomsInfo"))
            {
                var res = VoiceRoomCenter.GetRoomsInfo();
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("res", res);

                //客户端返回
                return new CommandPackage(param, "GetRoomsInfo");
            }

            //创建房间
            if (package.Parameters.Keys.Contains("CreateRoom"))
            {
                //创建房间
                string RoomID = VoiceRoomCenter.CreateRoom(package.Parameters["CreateRoom"].ToString());
            }

            return null;
        }
    }
}
