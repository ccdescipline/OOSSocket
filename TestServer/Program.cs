// See https://aka.ms/new-console-template for more information
using CSocket;
using CSocket.TranslateTemplate;
using TestServer;
using TestServer.VoiceProc;
using TouchSocket.Core.Config;
using TouchSocket.Core.Plugins;
using TouchSocket.Sockets;
using CSocket.Plugin;
using System.Runtime.CompilerServices;
using System.Reflection;
using System;
using TestServer.LoginService;
using CSocket.Command;

Console.WriteLine("Hello, World!");




void startTcpService()
{
    //TcpService
    TcpService service = new TcpService();
    service.Connecting += (client, e) => { };//有客户端正在连接
    service.Connected += (client, e) => { };//有客户端连接
    service.Disconnected += (client, e) => { };//有客户端断开连接
    service.Received += (client, byteBlock, requestInfo) =>
    {
        //从客户端收到信息
        string mes = byteBlock.ToString();
        Console.WriteLine($"已从{client.ID}接收到信息：{mes}");

        client.Send(mes);//将收到的信息直接返回给发送方

        //client.Send("id",mes);//将收到的信息返回给特定ID的客户端

        var clients = service.GetClients();
        foreach (var targetClient in clients)//将收到的信息返回给在线的所有客户端。
        {
            if (targetClient.ID != client.ID)
            {
                targetClient.Send(mes);
            }
        }
    };

    service.Setup(new TouchSocketConfig()//载入配置     
        .SetListenIPHosts(new IPHost[] { new IPHost(7790) })//同时监听两个地址
        .SetMaxCount(10000)
        .SetThreadCount(100))

        .Start();//启动
}

CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate> server = new CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate>();

server
    //添加命令
    .UseCommand((options) => {
        ////添加command
        //options.AddCommand<TestCommand>();

        ////声音command
        //options.AddCommand<VoiceBitCommand>();
        //options.AddCommand<VoiceCmdCommand>();


        //添加filter
        //options.AddFilter<LoginFilter>();
        options.AddFilterByAssembly(Assembly.GetExecutingAssembly());

        //options.AddCommand(typeof(TestCommand).Assembly);
        options.AddCommandByAssembly(Assembly.GetExecutingAssembly());
    })
    //.AddEventDisconnect((client,e) => {
    //    VoiceRoomCenter.leaveRoom(client.ID);
    
    //})
    
    .Setup(
        new TouchSocketConfig()//载入配置     
        .SetListenIPHosts(new IPHost[] { new IPHost(7790) })//同时监听两个地址  //new IPHost(7790) ,
        .SetMaxCount(10000)
        .SetThreadCount(100)
        .ConfigurePlugins(a => {
            //添加日志插件
            a.Add(new LogPlugin());
            a.Add<VoivcePlugin>();
        })
        .UsePlugin()
    )
    .Start();//启动

Console.ReadLine();

