using CSocket.Command;
using CSocket.Compoments;
using CSocket.Filter;
using CSocket.Interface;
using CSocket.PackageTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Core.ByteManager;
using TouchSocket.Sockets;
using TouchSocket.Sockets.Plugins;
using static System.Net.Mime.MediaTypeNames;

namespace CSocket
{
    /// <summary>
    /// 服务器
    /// </summary>
    /// <typeparam name="Translate">编解码器</typeparam>
    /// <typeparam name="Serialize">序列化器</typeparam>
    public class CTcpServer<Translate,Serialize> : TcpService<CSocketClient> 
        where Translate : IPackageTranslate, new() 
        where Serialize : IBodyTranslate , new()
    {
        /// <summary>
        /// 定义编解码器
        /// </summary>
        private IPackageTranslate TranslateCompoment;

        private IBodyTranslate bodyTranslate;

        /// <summary>
        /// 命令列表
        /// </summary>
        private List<ICommand> Commandtype;

        /// <summary>
        /// 过滤器列表
        /// </summary>
        private List<ICommandFilter> Filtertypes;

        public CTcpServer()
        {
            //遍历所有的Command
            Commandtype = new List<ICommand>();
            List<Type> commandList = typeof(IPackageCommand<>).Assembly.GetTypes()
                   .Where(s => s.GetInterfaces()
                       .Where(s => s.IsGenericType)
                       .Select(s => s.GetGenericTypeDefinition())
                       .Contains(typeof(IPackageCommand<>))
                   ).ToList();
            foreach (Type type in commandList)
            {
                Commandtype.Add(Activator.CreateInstance(type) as ICommand);
            }

            Filtertypes = new List<ICommandFilter>();

            //实例化编解码器
            this.TranslateCompoment = new Translate();

            //装载编解码器
            CSocketClient._TranslateCompoment = TranslateCompoment;

            //实例化序列化器并且装载
            this.bodyTranslate = new Serialize();

            CSocketClient._bodyTranslate = bodyTranslate;
            

        }

        //public delegate void ReceiveDelegate(CPackage package,CSocketClient socketClient);

        ///// <summary>
        ///// 当数据包（自定义数据格式）接收时
        ///// </summary>
        //public event ReceiveDelegate OnPackageRecvive;

        protected override void OnReceived(CSocketClient socketClient, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //Console.WriteLine("接收数据");
            
            //解析数据,传入事件
            //OnPackageRecvive(Translate.Decoder(byteBlock), socketClient);

            //Console.WriteLine($"包内容： {Encoding.UTF8.GetString((requestInfo as MyFixedHeaderRequestInfo).Body)}");

            //执行过滤器
            ExecuteFilter(socketClient, byteBlock, requestInfo);
            

            base.OnReceived(socketClient, byteBlock, requestInfo);
        }

        protected override void OnConnecting(CSocketClient socketClient, ClientOperationEventArgs e)
        {
            
            
            //此处逻辑会多线程处理。
            socketClient.SetDataHandlingAdapter(new MyFixedHeaderCustomDataHandlingAdapter());//直接设置适配器。可以在任何时刻调用。

            //e.ID:对新连接的客户端进行ID初始化，例如可以设置为其IP地址。
            //e.IsPermitOperation:指示是否允许该客户端链接。

            //对即将连接的客户端做初始化配置
            socketClient.Protocol = new Protocol("MyProtocol");
            base.OnConnecting(socketClient, e);
        }

        protected override void OnConnected(CSocketClient socketClient, TouchSocketEventArgs e)
        {
            Console.WriteLine($"客户端 {socketClient.ID} 连接");
            //socketClient.Session = socketClient.ID;
            base.OnConnected(socketClient, e);
            
        }

        /// <summary>
        /// 断开连接时
        /// </summary>
        public event Action<CSocketClient,ClientDisconnectedEventArgs>? OnDisconnect;

        protected override void OnDisconnected(CSocketClient socketClient, ClientDisconnectedEventArgs e)
        {
            //Console.WriteLine("客户端断开");

            //调用断开事件
            OnDisconnect?.Invoke(socketClient,e);

            base.OnDisconnected(socketClient, e);
        }

        /// <summary>
        /// 添加断开连接时的委托
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public CTcpServer<Translate, Serialize> AddEventDisconnect(Action<CSocketClient, ClientDisconnectedEventArgs> e)
        {
            this.OnDisconnect += e;
            return this;
        }

        /// <summary>
        /// 管理命令和过滤器
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public CTcpServer<Translate, Serialize> UseCommand(Action<CommandOpions> func)
        {
            CommandOpions commandOpions = new CommandOpions(Commandtype, Filtertypes);
            func(commandOpions);

            Commandtype = commandOpions.CommandList;
            Filtertypes = commandOpions.FilterTypes;

            return this;
        }


        /// <summary>
        /// 执行command
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="requestInfo"></param>
        private void ExecuteCommands(CSocketClient socketClient, IRequestInfo requestInfo, CPackage Ipackage)
        {
            //过滤器已经解析了
            ////解析数据
            //CPackage? Ipackage = JsonSerializer.Deserialize<CPackage>(
            //    TranslateCompoment.Decoder(
            //        (requestInfo as MyFixedHeaderRequestInfo).Body
            //        )
            //    );

            //ITcpPlugin
               // TcpPluginBase

            //遍历所有command
            foreach (ICommand item in Commandtype)
            {

                //var cmdNameProperty = type.GetProperty("CmdName");
                //string? cmdName = cmdNameProperty?.GetValue(null, null) as string;

                //根据command 拿 IPackageCommand 的接口泛型
                Type type = item.GetType();

                Type? IPackageCommand_Generic = type.GetInterfaces()
                    
                    .First(x => { return x.Name == typeof(IPackageCommand<>).Name; })
                    .GetGenericArguments()[0];

                //object? IPackageCommand_Generic_Instance =  Activator.CreateInstance(IPackageCommand_Generic);

                //string? cmdName = IPackageCommand_Generic.GetProperty("Command")?.GetValue(IPackageCommand_Generic_Instance)?.ToString();

                //判断命令类型
                //if (cmdName == Ipackage.Command)
                if (CPackage.IsEqualpackage(Ipackage, IPackageCommand_Generic))
                {
                    //实例化
                    object? Instance = item;

                    //调用反序列化方法
                    //MethodInfo? method_Deserialize = type.GetMethod("Deserialize");
                    //object? resPackage = method_Deserialize?.Invoke(Instance, new object[] { (requestInfo as MyFixedHeaderRequestInfo).Body });

                    //获取command 的 package类型
                    //Type Generic = type.GetInterfaces().First(x => x.Name.Contains(typeof(IPackageCommand<>).Name)).GetGenericArguments()[0];

                    ////构造包解析器反射
                    //Type bodyTranslate_Type = bodyTranslate.GetType();

                    ////调用反序列化方法
                    //object? resPackage = bodyTranslate_Type.GetMethod("Deserialize")
                    //    ?.MakeGenericMethod(new Type[] { Generic })
                    //    .Invoke(bodyTranslate,new object?[] {
                    //        TranslateCompoment.Decoder(
                    //            (requestInfo as MyFixedHeaderRequestInfo)?.Body
                    //            )
                    //    });

                    

                    //执行
                    MethodInfo? method_ExecuteCmd = type.GetMethod("ExecuteCmd");
                    object? res = method_ExecuteCmd?.Invoke(Instance, new object[] { socketClient, Ipackage });
                    //判断空
                    if (res!=null)
                    {
                        socketClient.SendPackage((CPackage)res);
                    }
                }


            }

            
        }


        /// <summary>
        /// 执行过滤器 ， 顺便执行完指令
        /// </summary>
        /// <param name="socketClient"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        private void ExecuteFilter(CSocketClient socketClient, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //解析数据
            //CPackage? Ipackage = bodyTranslate.Deserialize<CPackage>(
            //    TranslateCompoment.Decoder(
            //        (requestInfo as MyFixedHeaderRequestInfo).Body
            //        )
            //    );

            CPackage? Ipackage = (CPackage?)CPackage.ConvertPackage(TranslateCompoment, bodyTranslate,
                (requestInfo as MyFixedHeaderRequestInfo).Body);

            //遍历所有过滤器
            foreach (ICommandFilter Filter in Filtertypes)
            {
                //实例化
                //object? Instance = Activator.CreateInstance(type);

                //调用OnCommandExecuting
                //MethodInfo? OnCommandExecuting = type.GetMethod("OnCommandExecuting");
                //bool? res =  OnCommandExecuting?.Invoke(Instance,new object?[] {  }) as bool?;

                bool? res = Filter.OnCommandExecuting(
                    new CommandExecutingContext(socketClient, byteBlock, requestInfo, Ipackage, this.GetClients()));

                if (res == false)
                {
                    return;
                }
            }

            //执行命令
            ExecuteCommands(socketClient, requestInfo, Ipackage);

            //遍历所有过滤器
            foreach (ICommandFilter Filter in Filtertypes)
            {
                ////实例化
                //object? Instance = Activator.CreateInstance(type);

                ////调用OnCommandExecuted
                //MethodInfo? OnCommandExecuting = type.GetMethod("OnCommandExecuted");
                //OnCommandExecuting?.Invoke(Instance, new object?[] { new CommandExecutingContext(socketClient, byteBlock, requestInfo, Ipackage, this.GetClients()) });

                Filter.OnCommandExecuted(
                    new CommandExecutingContext(socketClient, byteBlock, requestInfo, Ipackage, this.GetClients()));
            }
        }
    }
}
