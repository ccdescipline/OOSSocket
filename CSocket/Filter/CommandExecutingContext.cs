using CSocket.Command;
using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core.ByteManager;
using TouchSocket.Sockets;

namespace CSocket.Filter
{
    public struct CommandExecutingContext
    {
        public SocketClient CurrentContext;
        public ByteBlock byteBlock;
        public IRequestInfo requestInfo;
        public CPackage package;
        public SocketClient[] Clients;

        public CommandExecutingContext(SocketClient currentContext, ByteBlock byteBlock, IRequestInfo requestInfo, CPackage package, SocketClient[] clients)
        {
            CurrentContext = currentContext;
            this.byteBlock = byteBlock;
            this.requestInfo = requestInfo;
            this.package = package;
            Clients = clients;
        }
    }
}
