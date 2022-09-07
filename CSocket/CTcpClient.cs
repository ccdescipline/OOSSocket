using CSocket.Command;
using CSocket.Filter;
using CSocket.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core.ByteManager;
using TouchSocket.Sockets;

namespace CSocket
{
    /// <summary>
    /// 客户端
    /// </summary>
    /// <typeparam name="Translate">编解码器</typeparam>
    /// <typeparam name="Serialize">序列化器</typeparam>
    public class CTcpClient<Translate, Serialize> : TcpClientBase
        where Translate : IPackageTranslate, new()
        where Serialize : IBodyTranslate, new()
    {
        /// <summary>
        /// 定义编解码器
        /// </summary>
        private IPackageTranslate TranslateCompoment;

        public IBodyTranslate bodyTranslate;

        protected string clientID;

        public CTcpClient()
        {
            //实例化编解码器
            this.TranslateCompoment = new Translate();

            //初始化session
            this.clientID = String.Empty;

            //实例化序列化器并且装载
            this.bodyTranslate = new Serialize();

            this.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new MyFixedHeaderCustomDataHandlingAdapter());//直接设置适配器。可以在任何时刻调用。
            };

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

        private Action<CTcpClient<Translate, Serialize>, string, object?>? SendPackageRecvCallback;

        /// <summary>
        /// 带回调的发送数据包
        /// </summary>
        /// <typeparam name="ResultType"></typeparam>
        /// <param name="package"></param>
        /// <param name="func_callback"></param>
        public void SendPackage<ResultType>(CPackage package, Action<CTcpClient<Translate, Serialize>, string, ResultType?> func_callback)
            where ResultType : CPackage
        {
            //注册回调

            //实例化空的回调，再把真正的回调赋值(欺骗编译器)
            Action<CTcpClient<Translate, Serialize>, string, object?> func = new Action<CTcpClient<Translate, Serialize>, string, object?>(
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


        //public async Task<ResultType?> SendPackage<ResultType>(CPackage package)
        //    where ResultType : CPackage, new()
        //{
        //    //checkType = typeof(ResultType);

        //    //this.SendPackage(package);

        //    //await Task.Run(() => {
        //    //    while (!isRecvPackage)
        //    //    {

        //    //    }

        //    //    isRecvPackage = false;
        //    //});


        //    //return 

        //    ResultType? returnValue = await Task.Run( () => {

        //        ResultType res = null;
        //        this.SendPackage<ResultType>(package, (client, cmd, p) => {
        //            res = p;
        //        });

        //        while (res==null)
        //        {

        //        }

        //        return res;
        //    });


        //    return returnValue;
        //}

        public ResultType SendPackage<ResultType>(CPackage package)
            where ResultType : CPackage, new()
        {
            ResultType res = null;
            this.SendPackage<ResultType>(package, (client, cmd, p) => {
                res = p;
            });

            while (res == null)
            {
                Thread.Sleep(100);
            }

            return res;
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
            string json = bodyTranslate.serialize((object)package);
            byte[] buf = TranslateCompoment.EncoDer(json);

            //根据协议写数据
            bufer_List.AddRange(BitConverter.GetBytes(buf.Length));
            bufer_List.AddRange(buf);

            ByteBlock block = new ByteBlock();
            block.Write(bufer_List.ToArray(), 0, bufer_List.Count);

            return block;
        }

        /// <summary>
        /// 接受数据事件
        /// </summary>
        public event Action<CTcpClient<Translate, Serialize>,string ,object?> Received; 

        /// <summary>
        /// 数据接收时
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //原始json
            string jsonStr = TranslateCompoment.Decoder(
                    (requestInfo as MyFixedHeaderRequestInfo).Body
                    );

            //解析数据
            CPackage? Ipackage = bodyTranslate.Deserialize<CPackage>(
                jsonStr
                );

            //实例化包（根据程序集和类名）
            
            ObjectHandle? obj = Activator.CreateInstance(Ipackage.Assembly, Ipackage.FullName);

            //调用反序列化方法
            object? resPackage = bodyTranslate.GetType().GetMethod("Deserialize")?.MakeGenericMethod(new Type[] { obj.Unwrap().GetType() })
                .Invoke(bodyTranslate, new object?[] {
                            jsonStr
                });

            //调用事件
            Received.Invoke(this,Ipackage.Command, resPackage);

            //sendpackage回调
            SendPackageRecvCallback?.Invoke(this, Ipackage.Command, resPackage);

            //if (CPackage.IsEqualpackage(Ipackage,checkType))
            //{
            //    isRecvPackage = true;
            //}

            base.HandleReceivedData(byteBlock, requestInfo);
        }

        protected override void OnConnected(MsgEventArgs e)
        {
            Console.WriteLine("连接服务器...");

            //判断是否是断线重连
            if (string.IsNullOrEmpty(this.clientID))
            {
                //获取服务器的ClientID

            }
            else
            {
                //提交服务器本次clientID
            }

            base.OnConnected(e);
        }
    }
}
