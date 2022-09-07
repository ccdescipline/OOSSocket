using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core.Extensions;

namespace CSocket.Interface
{
    /// <summary>
    /// 包信息注入
    /// </summary>
    public class PackageAttribute : Attribute
    {
        /// <summary>
        /// 包名
        /// </summary>
        private string packageName;

        /// <summary>
        /// 程序集名
        /// </summary>
        private string assembly;

        /// <summary>
        /// 包信息注入
        /// </summary>
        /// <param name="packageName"></param>
        public PackageAttribute(string packageName, string assembly) 
        {
            this.packageName = packageName;
            this.assembly = assembly;
        }

        public PackageAttribute()
        {
            
        }

        public string PackageName { get => packageName;  }
        public string Assembly { get => assembly; }
    }
}
