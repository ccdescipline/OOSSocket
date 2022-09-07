using CSocket.Compoments;
using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSocket.TranslateTemplate
{
    /// <summary>
    /// 默认编解码器（json）
    /// </summary>
    public class DefaulPackageTranslate: IPackageTranslate
    {
        //public BinaryFormatter formater = new BinaryFormatter();



        /// <summary>
        /// 原始数据进行解码
        /// </summary>
        /// <param name="OriginBytes"></param>
        /// <returns></returns>
        public string Decoder(byte[] OriginBytes)
        {
            return Encoding.UTF8.GetString(OriginBytes);
        }

        /// <summary>
        /// 原始数据进行编码
        /// </summary>
        /// <returns></returns>
        public byte[] EncoDer(string package)
        {
            return Encoding.UTF8.GetBytes(package);
        }
    }
}
