using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TouchSocket.Core.Extensions;
using CSocket.Compoments;
using TouchSocket.Core.Log;
using TouchSocket.Core.Dependency;
using TouchSocket.Sockets.Plugins;

namespace CSocket.Plugin
{
    public class LogPlugin : ITcpPlugin
    {
        
        private int order = 1;
        public int Order { get => order; set => order = value; }

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
            //client.Logger.Warning($"IP: {client.IP} Port：{client.Port} 连接");
            CLog.Message($"IP: {client.IP} Port：{client.Port} 连接");

        }

        public Task OnConnectingAsync(ITcpClientBase client, ClientOperationEventArgs e)
        {
           return Task.CompletedTask;
        }

        public void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            //client.Logger.Message($"IP: {client.IP} Port：{client.Port} 断开连接");

            
            CLog.Message($"IP: {client.IP} Port：{client.Port} clientId: {(client as CSocketClient).ID} 断开连接");
        }

        public Task OnDisconnectedAsync(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            return Task.CompletedTask;
        }


        public void OnIDChanged(ITcpClientBase client, IDChangedEventArgs e)
        {
            Console.WriteLine($"ClientID被更改,oldValue: {e.OldID}, NewValue: {e.NewID}");
        }

        public Task OnIDChangedAsync(ITcpClientBase client, TouchSocketEventArgs e)
        {
            return Task.CompletedTask;
        }

        public Task OnIDChangedAsync(ITcpClientBase client, IDChangedEventArgs e)
        {
            throw new NotImplementedException();
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
