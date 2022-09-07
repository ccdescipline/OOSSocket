using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Compoments
{
    
    public class CLog
    {
        public static void Message(string msg)
        {
            Print(msg);
        }

        public static void Warning(string msg)
        {
            Print(msg, ConsoleColor.Yellow);
        }

        public static void Error(string msg)
        {
            Print(msg, ConsoleColor.Red);
        }

        public static void Print(string msg, ConsoleColor fontcolor = ConsoleColor.Green, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = fontcolor;
            Console.Write($"[Csocket] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
