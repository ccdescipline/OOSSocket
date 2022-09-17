using CSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TouchSocket.Sockets.Plugins;

namespace TestServer.VoiceProc
{
    /// <summary>
    /// 声音插件
    /// </summary>
    internal class VoivcePlugin : ITcpPlugin
    {

        public int Order { get; set; } = 1;

        public void Dispose()
        {
            
        }

        public void OnConnected(ITcpClientBase client, TouchSocketEventArgs e)
        {
            
        }

        public Task OnConnectedAsync(ITcpClientBase client, TouchSocketEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void OnConnecting(ITcpClientBase client, ClientOperationEventArgs e)
        {
            
        }

        public Task OnConnectingAsync(ITcpClientBase client, ClientOperationEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            //判断是否是服务端
            if (client == null)
            {
                return;
            }

            //clientId 离开房间
            VoiceRoomCenter.leaveRoom((client as CSocketClient).ID);
            Console.WriteLine($"{(client as CSocketClient).ID} 断线离开语音服务器");
        }

        public Task OnDisconnectedAsync(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            return Task.CompletedTask;
        }



        public void OnIDChanged(ITcpClientBase client, IDChangedEventArgs e)
        {
             
        }


        public Task OnIDChangedAsync(ITcpClientBase client, IDChangedEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
             
        }

        public Task OnReceivedDataAsync(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void OnReceivingData(ITcpClientBase client, ByteBlockEventArgs e)
        {
            
        }

        public Task OnReceivingDataAsync(ITcpClientBase client, ByteBlockEventArgs e)
        {
            return Task.CompletedTask;
        }

        public void OnSendingData(ITcpClientBase client, SendingEventArgs e)
        {
             
        }

        public Task OnSendingDataAsync(ITcpClientBase client, SendingEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
