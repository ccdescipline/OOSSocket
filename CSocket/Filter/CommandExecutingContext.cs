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
        public SocketClient commandContext;
        public ByteBlock byteBlock;
        public IRequestInfo requestInfo;
        public CPackage package;

        public CommandExecutingContext(SocketClient commandContext, ByteBlock byteBlock, IRequestInfo requestInfo, CPackage package)
        {
            this.commandContext = commandContext;
            this.byteBlock = byteBlock;
            this.requestInfo = requestInfo;
            this.package = package;
        }
    }
}
