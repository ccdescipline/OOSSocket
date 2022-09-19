# <center>OOSSocket</center>

## 简 介
   &emsp;&emsp;OOSSocket是基于[TouchSocket](https://github.com/RRQM/TouchSocket)的二创项目，诞生之初希望能帮助开发者快速部署Tcp服务端和客户端，通过插件快速将自己的组件加入服务当中,并且借鉴superSocket设计理念，将包包对象，命令，过滤器等加入其中。

   本项目中TestClient 已经部署好了一个语音流通讯demo，你只需要将
   ```C#
        //选择麦克风
        //waveIn = startVoice();
        //QueryVoiceRoom();
   ```
   取消注释即可，TestServer已经为您装载好了，直接运行即可

## 快速上手

服务端
```C#
CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate> server = 
    new CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate>();

server
    //添加命令
    .UseCommand((options) => {
        //添加filter
        options.AddFilterByAssembly(Assembly.GetExecutingAssembly());
        //添加command
        options.AddCommandByAssembly(Assembly.GetExecutingAssembly());
    })
    .Setup(
        new TouchSocketConfig()//载入配置     
        .SetListenIPHosts(new IPHost[] { new IPHost(7790) })//同时监听两个地址  //new IPHost(7790) ,
        .SetMaxCount(10000)
        .SetThreadCount(100)
        .ConfigurePlugins(a => {
            //添加日志插件
            //a.Add(new LogPlugin());
            //a.Add<VoivcePlugin>();
        })
        .UsePlugin()
    )
    .Start();//启动

Console.ReadLine();
```

客户端
```C#
        CTcpClient<DefaulPackageTranslate, JsonSerializeTranslate> client = new CTcpClient<DefaulPackageTranslate, JsonSerializeTranslate>();
        client.Received += (client, cmd, p) =>
        {
            //数据接收时
        };

        //声明配置
        TouchSocketConfig config = new TouchSocketConfig();
        config.SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
            .UsePlugin()
            .ConfigurePlugins(a =>
            {
                a.Add<TestPlugin>();
                a.UseReconnection(5, true, 1000);
            });


        //载入配置
        client
            .Setup(config)
            .Connect()
            ;

        Console.WriteLine("服务器连接成功！");

        while (true)
        {
            Console.WriteLine("输入命令");
            CommandPackage commandPackage =client.SendPackage1<CommandPackage>(new CommandPackage(Console.ReadLine()));
            Console.WriteLine(commandPackage);
        }

        Console.WriteLine("结束");
```

### 设计理念
#### 包解析
&emsp;&emsp;通讯本质是传输二进制流，OOSSocket将通讯的流封装成了对象，那么将二进制流r如何转化为对象呢？这里引入解码器和序列化器的概念，就是CTcpClient，CTcpServer 构造时传入的两个泛型，服务端和客户端的解码器和序列化器必须相同，不然无法解析包。  

&emsp;&emsp;首先二进制流会首先进入解码器也就是继承了IPackageTranslate的对象并调用Decoder或EncoDer进行编码和解码

```C#
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
```

&emsp;&emsp;其次解码后的原始json字符串会进行json序列化或反序列化 ，从包变成json，或者从json变成包
```C#
    /// <summary>
    /// 序列化类
    /// </summary>
    public interface IBodyTranslate
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        T Deserialize<T>(string str);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        string serialize<T>(T obj);
    }
```

OOSSocket原生自带几个解码器和序列化器，
分别是DefaulPackageTranslate和JsonSerializeTranslate，只需在实例化指定这两个到泛型即可实装（见上例）

那么包是什么？我们来看看包的基类

```C#
    /// <summary>
    /// 数据包基类
    /// </summary>
    public  class CPackage
    {
        /// <summary>
        /// 包类名
        /// </summary>
        protected  string _command;

        /// <summary>
        /// 程序集
        /// </summary>
        protected string _assembly;

        protected string _fullName;

        ....

        /// <summary>
        ///  命令头
        /// </summary>
        public string Command { get => _command; set => _command = value; }
        public string Assembly { get => _assembly; set => _assembly = value; }
        public string FullName { get => _fullName; set => _fullName = value; }
    }
```
为什么这么设计？因为反射要构造一个类，需要提供程序集和完整类名，在基础上我加了一个command字段早期来增加包名（无意义），这里也提供了几个包打开思路
```C#
    /// <summary>
    /// 字符串数据包
    /// </summary>
    //[PackageAttribute("StringPackage")]
    [PackageAttribute]
    public class StringPackage: CPackage
    {
        private string body;

        public StringPackage(string body)
        {
            this.Body = body;
        }

        public string Body { get => body; set => body = value; }
    }
```
该类只有一个字段，传输字符串，PackageAttribute特性是描述该包的_command属性，默认是类名，如想改变可`[PackageAttribute("MyPackage")]`
### 命令（command）
&emsp;&emsp;当服务器成功解析包之后，就应开始使用不同的代码来处理不同的包了，这里引入命令的概念，它可以指定处理指定类型的包，命令必须继承IPackageCommand<T>接口
```C#
    public interface IPackageCommand<T>: ICommand where T : CPackage
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="package"></param>
        public CPackage? ExecuteCmd(CSocketClient socketClient, T package);
    }
```
泛型T指的是要处理的包类型，我们来看个例子
```C#
    public class StringCommand : IPackageCommand<StringPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, StringPackage package)
        {
            Console.WriteLine($"包内容是：{package.Body}");
            return package;
        }
    }
```

这样当客户端发来一个StringPackage的包，服务端立马就可以执行这段代码。当然，前提是得在配置的时候把命令加载到服务中，我们提供两种方法加载：

添加单个命令
```C#
CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate> server = 
    new CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate>();
server
    //添加命令
    .UseCommand((options) => {
        //添加单个命令
        options.AddCommand<StringCommand>();
        
    })
```
和指定程序集
```C#
CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate> server = 
    new CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate>();

server
    //添加命令
    .UseCommand((options) => {

        //添加程序集
        options.AddCommandByAssembly(Assembly.GetExecutingAssembly());
    })
```
Ps:指定程序集之后，OOSSocket会扫描程序集下所有继承了IPackageCommand<>接口，并将它们全部加入到框架之中

### 过滤器（Filler）
&emsp;&emsp;就像Http服务器一样，处理客户端发送的数据并不会全部处理，而是可能会需要类似登录的操作才会进行后续操作，我称之为过滤操作，即在一个特性条件下只有指定包才能通过的操作。过滤器需继承ICommandFilter接口
```C#
    public interface ICommandFilter
    {
        /// <summary>
        /// 命令执行后
        /// </summary>
        /// <param name="commandContext"></param>
        void OnCommandExecuted(CommandExecutingContext commandContext);

        /// <summary>
        /// 命令执行前
        /// </summary>
        /// <param name="commandContext"></param>
        /// <returns></returns>
        bool OnCommandExecuting(CommandExecutingContext commandContext);
    }
```
Ps:OnCommandExecuting的返回值决定是否执行后续命令  

### 例子

我们来看看一个登录例子，假如想实现一个登录操作，即客户端在连接时需要发送一个包，里面包含账号密码等信息，服务端核实之后，在Session加入客户端信息就为已登陆，如果没登录操作就发包，就会被拦截

首先是登录的命令，我们这里使用LoginPackage来传输数据  
包对象：
```C#
    /// <summary>
    /// 登录包
    /// </summary>
    public class LoginPackage:CPackage
    {
        private string account;
        private string password;

        ....
    }
```

登录命令：
```C#
    /// <summary>
    /// 模拟登录
    /// </summary>
    internal class LoginCommand : IPackageCommand<LoginPackage>
    {
        public CPackage? ExecuteCmd(CSocketClient socketClient, LoginPackage package)
        {
            socketClient.Session["Userinfo"] = package;

            Console.WriteLine($"userinfo {package.Account} , {package.Password} 登录！");

            return new CommandPackage("Login -status 200");
        }
    }
```
Ps:CommandPackage是自带的命令行处理包，结构为：包含参数列表和指令类型的数据结构
```C#
    public class CommandPackage: CPackage
    {

        private string instruct;

        private Dictionary<string, object> parameters;
    }
```

登录过滤器：
```C#
    /// <summary>
    /// 登陆过滤器
    /// </summary>
    public class LoginFilter : ICommandFilter
    {
        ...
        public bool OnCommandExecuting(CommandExecutingContext commandContext)
        {
            LoginPackage? userinfo = null;
            try
            {
                userinfo = (commandContext.CurrentContext as CSocketClient).Session["Userinfo"] as LoginPackage;
                //Console.WriteLine($"userinfo {userinfo.Account} , {userinfo.Password} 登录！");
                return true;
            }
            catch (KeyNotFoundException e)
            {
                //判断是否LoginPackage
                if (!CPackage.IsEqualpackage(commandContext.package, typeof(LoginPackage)))
                {
                    Console.WriteLine("用户未登录，流量拦截！");
                    return false;
                }
            }

            return true;
        }
    }
```
将命令和过滤器装在即可（过滤器的装在和命令的装在相同）
```C#
CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate> server = 
    new CTcpServer<DefaulPackageTranslate, JsonSerializeTranslate>();

server
    //添加命令
    .UseCommand((options) => {

        //添加filter
        options.AddFilterByAssembly(Assembly.GetExecutingAssembly());
        //添加command
        options.AddCommandByAssembly(Assembly.GetExecutingAssembly());
    })
```
最后加入插件即可（插件详情请查阅Touchsocket文档插件），说人话就是集每个事件发生的一个类，你可以在“连接时”事件加入登录代码
```C#
internal class TestPlugin : ITcpPlugin
    {

        public void OnConnected(ITcpClientBase client, TouchSocketEventArgs e)
        {
            Console.WriteLine("开始登陆...");
           (client as ITcpInteractive).SendPackage(new LoginPackage("admin","123456"));

        }

        ...
    }
```
Ps:加入插件
```C#
        //声明配置
        TouchSocketConfig config = new TouchSocketConfig();
        config.SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
            .UsePlugin()
            .ConfigurePlugins(a =>
            {
                //添加插件
                a.Add<TestPlugin>();
                //配置断线重连
                a.UseReconnection(5, true, 1000);
            });


        //载入配置
        client
            .Setup(config)
            .Connect()
            ;
```

一切部署完毕，当客户端连接的时候就触发事件登录操作