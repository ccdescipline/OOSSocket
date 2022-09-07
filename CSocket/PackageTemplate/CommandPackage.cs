using CSocket.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.PackageTemplate
{
    /// <summary>
    /// 命令数据包
    /// </summary>
     //[PackageAttribute("CMD")]
    public class CommandPackage: CPackage
    {

        private string instruct;

        private Dictionary<string, object> parameters;

        /// <summary>
        /// 不要使用
        /// </summary>
        public CommandPackage()
            //: base("CMD")
        {

        }

        /// <summary>
        /// 根据指令名和参数构造
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="instruct"></param>
        public CommandPackage( Dictionary<string, object> Parameters, string instruct)
            //: base("CMD")
        {
            this.parameters = Parameters;
            this.instruct = instruct;
        }

        /// <summary>
        /// 根据字符串构造指令(内部使用) 格式 [instruct] -[ParamName] [ParamValue] ...
        /// </summary>
        /// <param name="CmdStr"></param>
        public CommandPackage(string CmdStr)
           //: base("CMD")
        {
            try
            {
                //根据空格分开
                string[] Params = CmdStr.Split(' ');

                this.instruct = Params[0];

                //构造arraylist并且移除首个 指令
                ArrayList arrayList = new ArrayList(Params);
                arrayList.RemoveAt(0);
                //Params =(string[])arrayList.ToArray();

                //构造参数列表
                this.parameters = new Dictionary<string, object>();
                for (int i = 0; i < arrayList.Count; i += 2)
                {
                    parameters.Add(((string)arrayList[i]).Substring(1), arrayList[i + 1]?.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception($"cmd构造异常:{e}");

            }
            
        }

        /// <summary>
        /// 命令参数
        /// </summary>
        public Dictionary<string, object> Parameters { get => parameters; set => parameters = value; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public string Instruct { get => instruct; set => instruct = value; }

        public override string? ToString()
        {
            foreach (var item in parameters)
            {
                
            }
            
            return $"{instruct} {String.Join(' ', parameters.ToList())}";
        }
    }
}
