using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.PackageTemplate
{
    /// <summary>
    /// 未知包类型
    /// </summary>
    public class UnknowTypePackage : CPackage
    {
        /// <summary>
        /// 键值对组合
        /// </summary>
        private Dictionary<string, object> keyValueDic;

        public UnknowTypePackage(Dictionary<string, object> keyValueDic)
        {
            keyValueDic = keyValueDic;
        }

        public Dictionary<string, object> KeyValueDic { get => keyValueDic; set => keyValueDic = value; }
    }
}
