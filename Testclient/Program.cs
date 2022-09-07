// See https://aka.ms/new-console-template for more information
using CSocket;
using CSocket.Interface;
using CSocket.PackageTemplate;
using System.Text;
using System.Text.Json;
using TouchSocket.Core.ByteManager;
using TouchSocket.Core.Config;
using TouchSocket.Sockets;
using Xunit;
using TouchSocket.Core.Plugins;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using NAudio.CoreAudioApi;
using CSocket.TranslateTemplate;
using System.Net.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;
using SharedPackage.Voice;
using System.Collections;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Wave.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using Testclient;
using CSocket.Command;










//while (true)
//{
//    Console.WriteLine("请输入命令：");
//    string cmd = Console.ReadLine();
//    //client.SendPackage<CommandPackage>(new CommandPackage(cmd), (client, cmd, p) =>
//    //{

//    //    Console.WriteLine($"回调函数接受：：指令名： {p.Instruct}");
//    //});

//    //VoiceCMD -GetRoomsInfo 0
//    CommandPackage res = await client.SendPackage<CommandPackage>(new CommandPackage(cmd));
//    Console.WriteLine($"回调函数接受：：指令名： {res.Instruct}");
//}

















public class Programe
{
    static CTcpClient<DefaulPackageTranslate, JsonSerializeTranslate> client = new CTcpClient<DefaulPackageTranslate, JsonSerializeTranslate>();
    static WasapiCapture waveIn;
    public static void Main( string [] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Hello, World!");

        

        client.Received += (client, cmd, p) =>
        {
            //if (cmd != "CommandPackage")
            //{
            //    return;
            //}

            //var package = p as CommandPackage;

            //Console.WriteLine($"服务器返回数据：指令名： {package.Instruct}");
            //foreach (var item in package.Parameters)
            //{
            //    Console.WriteLine($"参数名：[{item.Key}] ，参数值 ：[{item.Value}]");
            //}

            //if (package.Instruct == "GetRoomsInfo")
            //{
            //    List<VoiceRoom> res = client.bodyTranslate.Deserialize<List<VoiceRoom>>(package.Parameters["res"].ToString());
            //    int index = 1;

            //    foreach (VoiceRoom room in res)
            //    {
            //        Console.WriteLine($"{index})房间： {room.RoomName} ");
            //    }
            //}

        };

        //声明配置
        TouchSocketConfig config = new TouchSocketConfig();
        //39.107.96.100:7790
        //39.107.96.100:7791
        //127.0.0.1:7790
        //172.86.125.153:7791

        config.SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
            .UsePlugin()
            .ConfigurePlugins(a =>
            {
                a.UseReconnection(5, true, 1000);
            });


        //载入配置
        client
            .Setup(config)
            .Connect()
            ;

        Console.WriteLine("服务器连接成功！");

        //选择麦克风
        //waveIn = startVoice();
        //QueryVoiceRoom();

        while (true)
        {
            //Console.WriteLine("任意键发送测试数据包（服务器无法识别格式）");
            //Console.ReadLine();
            //client.SendPackage(new TestUnknowPackage("test uknowpackage"));

            Console.WriteLine("输入命令");

            CommandPackage commandPackage = client.SendPackage<CommandPackage>(new CommandPackage(Console.ReadLine()));
            Console.WriteLine(commandPackage);
        }

        Console.WriteLine("结束");
    }

    static ByteBlock translatePackage<T>(T package) where T : CPackage
    {
        List<byte> bufer_List = new List<byte>();

        JsonSerializerOptions option = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        string json = JsonSerializer.Serialize(package, option);
        byte[] buf = Encoding.UTF8.GetBytes(json);

        bufer_List.AddRange(BitConverter.GetBytes(buf.Length));
        bufer_List.AddRange(buf);

        ByteBlock block = new ByteBlock();
        block.Write(bufer_List.ToArray(), 0, bufer_List.Count);

        return block;
    }

    static void StartTcpclient()
    {
        TcpClient tcpClient = new TcpClient();

        tcpClient.Connecting += (client, e) =>
        {
            client.SetDataHandlingAdapter(new MyFixedHeaderCustomDataHandlingAdapter());//直接设置适配器。可以在任何时刻调用。
        };
        tcpClient.Connected += (client, e) => { };//成功连接到服务器
        tcpClient.Disconnected += (client, e) => { };//从服务器断开连接，当连接不成功时不会触发。
        tcpClient.Received += (client, byteBlock, requestInfo) =>
        {
            //从服务器收到信息
            //string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"接收到信息：{Encoding.UTF8.GetString((requestInfo as MyFixedHeaderRequestInfo).Body)}");
        };

        //声明配置
        TouchSocketConfig config = new TouchSocketConfig();
        //39.107.96.100:7790
        //127.0.0.1:7790

        config.SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
            .UsePlugin()
            .ConfigurePlugins(a =>
            {
                a.UseReconnection(5, true, 1000);
                
            });


        //载入配置
        tcpClient
            .Setup(config)
            .Connect()
            ;





        while (true)
        {
            //tcpClient.Send(translatePackage(new CommandPackage( param, "Userinfo")));
            //tcpClient.Send(block);

            //tcpClient.Send(translatePackage(new Bitpackage(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })));

            Console.WriteLine("请输入命令：");
            string cmd = Console.ReadLine();
            tcpClient.Send(translatePackage(new CommandPackage(cmd)));
        }
    }

    //将播放设备转换成指定格式的播放设备
    static ISampleProvider ConvertWaveFormat(ISampleProvider wave, WaveFormat format)
    {
        //转换采样率
        var resampler = new WdlResamplingSampleProvider(wave, format.SampleRate);

        if (format.Channels == 1)
        {
            if (resampler.WaveFormat.Channels == 1)
            {
                return resampler;
            }
            else
            {
                return new StereoToMonoSampleProvider(resampler);
            }

        }
        else
        {
            if (resampler.WaveFormat.Channels == 1)
            {
                return new MonoToStereoSampleProvider(resampler);
            }
            return resampler;
        }

    }

    static WasapiCapture startVoice()
    {
        var enumerator = new MMDeviceEnumerator();
        List<MMDevice> devices = new List<MMDevice>();
        int index = 1;
        foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
        {

            devices.Add(wasapi);
            Console.WriteLine($"{index++} {wasapi.DeviceFriendlyName}");//{devices.Count}.  {wasapi.DataFlow} {wasapi.FriendlyName} {wasapi.DeviceFriendlyName} {wasapi.State} {wasapi.ID}
        }
        Console.WriteLine("选择麦克风,输入序号回车：");
        string res = Console.ReadLine();

        //创建采集设备
        return new WasapiCapture(devices[Convert.ToInt32(res) - 1], false, 1000);
    }

    static void QueryVoiceRoom()
    {
       
        //查询房间信息
        CommandPackage? resPackage = client.SendPackage<CommandPackage>(new CommandPackage("VoiceCMD -GetRoomsInfo 0"));
        //Console.WriteLine("resPackage");

        List<VoiceRoom> res = client.bodyTranslate.Deserialize<List<VoiceRoom>>(resPackage.Parameters["res"].ToString());
        int index = 1;
        //遍历房间
        foreach (VoiceRoom room in res)
        {
            Console.WriteLine($"{index})房间： {room.RoomName} ");
        }

        Console.WriteLine("选择一个房间，如想创建房间按0");
        int input = Convert.ToInt32(Console.ReadLine());

        if (input == 0)
        {
            Console.Write("请输入创建的房间名：");
            string? RoomName = Console.ReadLine();
            //发送请求
            client.SendPackage(new CommandPackage($"VoiceCMD -CreateRoom {RoomName}"));
            QueryVoiceRoom();
        }
        else
        {
            string roomid = res[input - 1].RoomId;

            //创建播放设备

            //定义播放格式
            WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(16000, 1);

            //创建混音声道
            var 混音 = new MixingSampleProvider(waveFormat);
            List<BufferedWaveProvider> 混音道 = new List<BufferedWaveProvider>();
            for (int i = 0; i < 10; i++)
            {
                var buffer = new BufferedWaveProvider(waveFormat);
                混音道.Add(buffer);
                混音.AddMixerInput(buffer);
            }

            //注册麦克风接受声音就发送服务器


            //定义一个接受缓冲区设备，并且把他转化成定义的格式
            BufferedWaveProvider bufferedWave = new BufferedWaveProvider(waveIn.WaveFormat);
            waveIn.DataAvailable += (object? sender, WaveInEventArgs waveInEventArgs) => {

                //输入音频重采
                bufferedWave.ClearBuffer();
                bufferedWave.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);

                float ConvertLength = waveInEventArgs.BytesRecorded * ((float)waveFormat.SampleRate / waveIn.WaveFormat.SampleRate) * ((float)waveFormat.Channels / waveIn.WaveFormat.Channels);

                byte[] bytes = new byte[(int)ConvertLength];
                ConvertWaveFormat(bufferedWave.ToSampleProvider(), waveFormat).ToWaveProvider()
                .Read(bytes, 0, (int)ConvertLength);

                client.SendPackage(new VoiceBitPackage(roomid, bytes));
                Console.WriteLine($"发送声音流{(int)ConvertLength}  ,  {waveInEventArgs.BytesRecorded}");
            };

            //定义声音缓冲区
            Dictionary<string, byte[]> voiceBuffer = new Dictionary<string, byte[]>();

            //注册声音回调,和声并播放
            client.Received += (client, cmd, p) => {
                //过滤包
                if (CPackage.IsEqualpackage((CPackage)p, typeof(ResVoicePackage)))
                {
                    ResVoicePackage? resVoicePackage = p as ResVoicePackage;

                    //for (int i = 0; i < resVoicePackage.VoiceBytes.Count; i++)
                    //{
                    //    混音道[i].AddSamples(resVoicePackage.VoiceBytes[i], 0, resVoicePackage.VoiceBytes[i].Length);
                    //}

                    int index = 0;
                    foreach (string key in resVoicePackage.VoiceBytes.Keys)
                    {
                        try
                        {
                            //判断新数据是否跟缓冲区的值一样
                            if (!voiceBuffer[key].SequenceEqual(resVoicePackage.VoiceBytes[key]))
                            {
                                混音道[index].AddSamples(resVoicePackage.VoiceBytes[key], 0, resVoicePackage.VoiceBytes[key].Length);

                            }
                            else
                            {
                                Console.WriteLine("缓冲区重复");
                            }

                        }
                        catch (KeyNotFoundException e)
                        {
                            //说明缓冲过去没有这个clientId的声音，赋值

                        }

                        voiceBuffer[key] = resVoicePackage.VoiceBytes[key];
                        index++;
                    }

                }
            };


            var player = new WaveOutEvent();
            player.Init(混音);

            // begin playback & record
            player.Play();
            waveIn.StartRecording();


            Console.ReadLine();


        }


    }
}