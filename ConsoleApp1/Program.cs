// See https://aka.ms/new-console-template for more information

using MySqlConnector;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text.Json;
using System.Threading;


//Type[] res = typeof(IBase<>).Assembly.GetTypes()

//    .Where(t => t.GetInterfaces()
//        .Select(s => s.Name)
//        .Contains(typeof(IBase<>).Name))
//    .ToArray();


//foreach (var dev in DirectSoundOut.Devices)
//{
//    Console.WriteLine($"{dev.Guid} {dev.ModuleName} {dev.Description}");
//}
//Class1 c =new Class1() { attr1 = "111"};

//Console.WriteLine(typeof(Class1).GetInterfaces().First(x => { return x.Name.Contains("IBase"); }).GetGenericArguments()[0].Name);

var ibase = typeof(IBase<>);
//var res = typeof(Class1).GetInterfaces()
//                        .Where(s => s.IsGenericType)
//                        .Select(s => s.GetGenericTypeDefinition())
//                        .First(x => x == ibase).GetGenericArguments()[0].Name;
//var res = typeof(Class1).GetInterfaces().First(x => { return x.Name == ibase.Name; }).GetGenericArguments()[0].Name;

//.Where(x => x.Equals((Type)ibase));

//Console.WriteLine(
//    res
//       );

//using var connection = new MySqlConnection("Server=39.107.96.100;User ID=BookSystem;Password=661cf54938be92ee;Database=booksystem");
//connection.Open();

//var res =  testAsync();
//Console.WriteLine($"finish res:");

//Activator.CreateInstance(,);
//Assembly



//测试麦克风();

void 测试麦克风()
{
    var enumerator = new MMDeviceEnumerator();



    List<MMDevice> devices = new List<MMDevice>();
    foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
    {
        devices.Add(wasapi);
        Console.WriteLine($"{devices.Count}.  {wasapi.DataFlow} {wasapi.FriendlyName} {wasapi.DeviceFriendlyName} {wasapi.State} {wasapi.ID}");
    }
    Console.WriteLine("选择麦克风,输入序号回车：");
    string res = Console.ReadLine();

    //创建采集设备
    var waveIn = new WasapiCapture(devices[Convert.ToInt32(res) - 1],false,1000);

    //定义声音格式
    WaveFormat waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(16000, 1);

    //缓冲区
    BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(waveFormat);



    waveIn.DataAvailable += (object? sender, WaveInEventArgs waveInEventArgs) => {
        //定义一个接受缓冲区设备，并且把他转化成定义的格式
        BufferedWaveProvider bufferedWave = new BufferedWaveProvider(waveIn.WaveFormat);
        bufferedWave.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);


        float ConvertLength = waveInEventArgs.BytesRecorded * ((float)waveFormat.SampleRate  / waveIn.WaveFormat.SampleRate) * ((float)waveFormat.Channels / waveIn.WaveFormat.Channels);

        byte[] bytes = new byte[(int)ConvertLength];
        ConvertWaveFormat(bufferedWave.ToSampleProvider(), waveFormat).ToWaveProvider()
        .Read(bytes, 0, (int)ConvertLength+1);

        bufferedWaveProvider.AddSamples(bytes, 0, bytes.Length);
        //bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
        Console.WriteLine($"写入缓冲区{(int)ConvertLength}  ,  {waveInEventArgs.BytesRecorded}");
    };

    var enumerator1 =  enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

    
    var audioFile = new AudioFileReader( Environment.CurrentDirectory + @"\周杰伦 - 还在流浪hires.flac");//C:\Users\23505\Music\


    var 混音 =   new MixingSampleProvider(waveFormat);

    //定义输出声音的采样率为16khz 声道1
    //WaveFormat format = new WaveFormat(16000, 1);

    //混音.AddInputStream(bufferedWaveProvider);
    //混音.AddInputStream(audioFile);

    混音.AddMixerInput(ConvertWaveFormat(bufferedWaveProvider.ToSampleProvider(), waveFormat)); 
    混音.AddMixerInput(ConvertWaveFormat(audioFile.ToSampleProvider(), waveFormat));
    //混音.

    // set up playback
    var player = new WaveOutEvent();
    player.Init(混音);

    // begin playback & record
    player.Play();
    waveIn.StartRecording();


    Console.ReadLine();
    //测试Naudio();

}

ISampleProvider ConvertWaveFormat(ISampleProvider wave,WaveFormat format)
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
        if (resampler.WaveFormat.Channels== 1)
        {
            return new MonoToStereoSampleProvider(resampler);
        }
        return resampler;
    }
    
}


void 测试Naudio()
{
    using (var audioFile = new AudioFileReader(@"C:\Users\23505\Music\周杰伦 - 还在流浪hires.flac"))
    using (var outputDevice = new WasapiOut())
    {
        outputDevice.Init(audioFile);
        outputDevice.Play();
        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(1000);
        }
    }
}


void 测试泛型接口遍历子类()
{
    Type[] res = typeof(IBase<>).Assembly.GetTypes()
        .Where(s => s.GetInterfaces()
            .Where(s => s.IsGenericType)
            .Select(s => s.GetGenericTypeDefinition())
            .Contains(typeof(IBase<>))
        ).ToArray();

    foreach (var item in res)
    {
        object obj = Activator.CreateInstance(item);

        MethodInfo method = item.GetMethod("Say");
        var Generics = item.GetGenericArguments();

        //Console.WriteLine($"当前class 泛型 : {}");
        method.Invoke(obj, null);
    }
}

public interface IBase<T> where T : struct
{
    public void Say();
}

public class Class1 : IBase<int>
{

    public Class1()
    {
        Console.WriteLine($"{this.GetType().GetCustomAttribute<testAttr>().className}");
        //Console.WriteLine($"{this.GetType().Name}");
    }

    public string attr1 { get; set; }

    public void Say()
    {
        Console.WriteLine("say Class1");
    }
}

public class Class2 : IBase<float>
{
    public void Say()
    {
        Console.WriteLine("say Class2");
    }
}

[testAttr("Class3")]
public class Class3: Class1
{


    public void Say()
    {
        Console.WriteLine("say Class3");
    }
}


class testAttr : Attribute
{
    public string className;

    public testAttr(string className)
    {
        this.className = className;
    }
}

