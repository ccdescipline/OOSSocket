using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace CSocket.Plugin
{
    internal class CTcpPluginbase : ITcpPlugin
    {
        private int order;

        public int Order { get => order; set => order = value; }

        public CTcpPluginbase(int order)
        {
            this.order = order;
        }

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

        public void OnIDChanged(ITcpClientBase client, IDChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task OnIDChangedAsync(ITcpClientBase client, IDChangedEventArgs e)
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
