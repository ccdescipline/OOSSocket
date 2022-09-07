using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace TestServer.VoiceProc
{
    /// <summary>
    /// 声音插件
    /// </summary>
    internal class VoivcePlugin : ITcpPlugin
    {
        public int Order { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnConnected(ITcpClientBase client, TouchSocketEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnConnectedAsync(ITcpClientBase client, TouchSocketEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnConnecting(ITcpClientBase client, ClientOperationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnConnectingAsync(ITcpClientBase client, ClientOperationEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnDisconnectedAsync(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnIDChanged(ITcpClientBase client, TouchSocketEventArgs e)
        {
           
            throw new NotImplementedException();
        }

        public Task OnIDChangedAsync(ITcpClientBase client, TouchSocketEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnReceivedDataAsync(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnReceivingData(ITcpClientBase client, ByteBlockEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnReceivingDataAsync(ITcpClientBase client, ByteBlockEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnSendingData(ITcpClientBase client, SendingEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnSendingDataAsync(ITcpClientBase client, SendingEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
