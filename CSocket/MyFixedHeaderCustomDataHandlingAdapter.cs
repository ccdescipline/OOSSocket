
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Sockets;

namespace CSocket
{
    public class MyFixedHeaderCustomDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<MyFixedHeaderRequestInfo>
    {
        public MyFixedHeaderCustomDataHandlingAdapter()
        {
            this.MaxPackageSize = 102400000;
        }

        /// <summary>
        /// 接口实现，指示固定包头长度
        /// </summary>
        public override int HeaderLength => 4;

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        protected override MyFixedHeaderRequestInfo GetInstance()
        {
            return new MyFixedHeaderRequestInfo();
        }
    }
}
