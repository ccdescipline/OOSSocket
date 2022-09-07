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
using TouchSocket.Sockets;

namespace TestServer.VoiceProc
{
    /// <summary>
    /// 处理声音的请求
    /// </summary>
    internal class VoiceBitCommand : IPackageCommand<VoiceBitPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, VoiceBitPackage package)
        {
            //写流
            VoiceRoomCenter.Writebyte(package.RoomId, socketClient.ID, package.VoiceData);

            //将声音流发回客户端
            var resVoice = VoiceRoomCenter.GetAnthorVoice(package.RoomId, socketClient.ID);

            return new ResVoicePackage(resVoice);
        }
    }
}
