using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouchSocket.Core.ByteManager;
using TouchSocket.Sockets;

namespace CSocket
{
    
    public class CSocketClient : SocketClient, ITcpInteractive
    {
        /// <summary>
        /// 服务端session
        /// </summary>
        protected ConcurrentDictionary<string, object> session = new ConcurrentDictionary<string, object>();

        public static IPackageTranslate? _TranslateCompoment;

        public static IBodyTranslate? _bodyTranslate;

        public ConcurrentDictionary<string, object> Session { get => session; set => session = value; }

        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //此处逻辑单线程处理。

            //此处处理数据，功能相当于Received事件。
            //string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            //Console.WriteLine($"已接收到信息");
            //原始json
            //string jsonStr = _TranslateCompoment.Decoder(
            //        (requestInfo as MyFixedHeaderRequestInfo).Body
            //        );
            //CPackage? Ipackage = _bodyTranslate.Deserialize<CPackage>(
            //    jsonStr
            //    );
            ////实例化包（根据程序集和类名）

            //ObjectHandle? obj = Activator.CreateInstance(Ipackage.Assembly, Ipackage.FullName);

            //object? v = obj.Unwrap();
            ////调用反序列化方法
            //object? resPackage = _bodyTranslate.GetType().GetMethod("Deserialize")
            //    ?.MakeGenericMethod(new Type[] { obj.Unwrap().GetType() })
            //    .Invoke(_bodyTranslate, new object?[] {
            //                jsonStr
            //    });

            object? resPackage = CPackage.ConvertPackage(_TranslateCompoment, _bodyTranslate, 
                (requestInfo as MyFixedHeaderRequestInfo).Body);

            SendPackageRecvCallback?.Invoke(this, (resPackage as CPackage).Command, resPackage);
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

        private Action<CSocketClient, string, object?>? SendPackageRecvCallback;

        /// <summary>
        /// 带回调的发送数据包
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <param name="package"></param>
        /// <param name="func_callback"></param>
        public void SendPackage<ResultType>(CPackage package, Action<CSocketClient, string, ResultType?> func_callback)
            where ResultType : CPackage
        {
            //注册回调

            //实例化空的回调，再把真正的回调赋值(欺骗编译器)
            Action<CSocketClient, string, object?> func = new Action<CSocketClient, string, object?>(
                 (client, cmd, p) =>
                 { }
                 );

            func = (client, cmd, p) =>
            {
                //过滤回调
                var ResultType_Instance = Activator.CreateInstance(typeof(ResultType));
                string? cmdName = typeof(ResultType).GetProperty("Command")?.GetValue(ResultType_Instance)?.ToString();

                //if (cmdName != cmd)
                //{
                //    return;
                //}

                if (!CPackage.IsEqualpackage((CPackage)p, typeof(ResultType)))
                {
                    return;
                }

                func_callback(client, cmd, p as ResultType);

                //执行完之后事件取消该委托
                SendPackageRecvCallback -= func;

            };


            SendPackageRecvCallback += func;


            this.SendPackage(package);
        }

        public async Task<ResultType?> SendPackage<ResultType>(CPackage package) where ResultType : CPackage, new()
        {

            ResultType? returnValue = await Task.Run(() =>
            {

                ResultType res = null;
                this.SendPackage<ResultType>(package, (client, cmd, p) =>
                {
                    res = p;
                });

                while (res == null)
                {
                    Thread.Sleep(100);
                }

                return res;
            });


            return returnValue;
        }
    }
}
