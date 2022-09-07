
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Sockets;

namespace CSocket
{
    public class MyFixedHeaderRequestInfo : IFixedHeaderRequestInfo
    {
        private int bodyLength;
        /// <summary>
        /// 接口实现，标识数据长度
        /// </summary>
        public int BodyLength
        {
            get { return bodyLength; }
        }


        private byte[] body;
        /// <summary>
        /// 自定义属性，标识实际数据
        /// </summary>
        public byte[] Body
        {
            get { return body; }
        }


        public bool OnParsingBody(byte[] body)
        {
            if (body.Length == this.bodyLength)
            {
                this.body = body;
                return true;
            }
            return false;
        }


        public unsafe bool OnParsingHeader(byte[] header)
        {


            this.bodyLength = BitConverter.ToInt32(header, 0);



            ////在该示例中，第一个字节表示后续的所有数据长度，但是header设置的是3，所以后续还应当接收length-2个长度。
            //using (MemoryStream memory = new MemoryStream(header))
            //{
            //    //读头
            //    byte [] buffer = new 
            //        byte [sizeof(long)];
            //    this.bodyLength = memory.Read(buffer ,0 , buffer.Length);
            //}
            
            return true;
        }
    }
}
