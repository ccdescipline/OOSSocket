
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Interface
{
    /// <summary>
    /// 定义对包进行编码和解码的解析
    /// </summary>
    public interface IPackageTranslate
    {
        /// <summary>
        /// 原始数据进行解码
        /// </summary>
        /// <param name="OriginBytes"></param>
        /// <returns></returns>
        string Decoder(byte[] OriginBytes);

        /// <summary>
        /// 原始数据进行编码
        /// </summary>
        /// <returns></returns>
        byte[] EncoDer(string package);
    }
}
