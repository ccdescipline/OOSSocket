using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouchSocket.Core.ByteManager;
using TouchSocket.Sockets;

namespace CSocket
{
    
    public class CSocketClient : SocketClient 
    {
        protected string session;

        public static IPackageTranslate? _TranslateCompoment;

        public static IBodyTranslate? _bodyTranslate;

        public string Session { get => session; set => session = value; }

        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //此处逻辑单线程处理。

            //此处处理数据，功能相当于Received事件。
            //string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            //Console.WriteLine($"已接收到信息");
        }




        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="package"></param>
        public void SendPackage(CPackage package)
        {
            this.Send(translatePackage(package));
        }

        /// <summary>
        /// 异步发送数据包
        /// </summary>
        /// <param name="package"></param>
        public void SendPackageAsync(CPackage package)
        {
            this.SendAsync(translatePackage(package));
        }

        /// <summary>
        /// 将数据包通过编码器编码成流数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="package"></param>
        /// <returns></returns>
        ByteBlock translatePackage(CPackage package)
        {
            List<byte> bufer_List = new List<byte>();

            //将包转成json，再通过编码器转成byte[]
            string json = _bodyTranslate.serialize((object)package);
            byte[] buf = _TranslateCompoment.EncoDer(json);

            //根据协议写数据
            bufer_List.AddRange(BitConverter.GetBytes(buf.Length));
            bufer_List.AddRange(buf);

            ByteBlock block = new ByteBlock();
            block.Write(bufer_List.ToArray(), 0, bufer_List.Count);

            return block;
        }
    }
}
