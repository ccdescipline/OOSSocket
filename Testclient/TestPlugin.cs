using CSocket.Interface;
using SharedPackage.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Testclient
{
    internal class TestPlugin : ITcpPlugin
    {

        public int Order { get; set; } = 1;

        public void Dispose()
        {
            
        }

        public void OnConnected(ITcpClientBase client, TouchSocketEventArgs e)
        {
            Console.WriteLine("开始登陆...");
           (client as ITcpInteractive).SendPackage(new LoginPackage("admin","123456"));

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
