using System;
using TouchSocket.Core.Dependency;

namespace CSocket
{
    [DependencyType(DependencyType.Constructor)]
    public class Class1
    {
        public Class1()
        {

        }

        public void test()
        {
            Console.WriteLine("test ioc");
        }

    }
}