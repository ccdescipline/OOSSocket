using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace CSocket.Interface
{
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

        public CPackage()
        {
            //先看看有没有特性
            PackageAttribute? attr =  this.GetType().GetCustomAttribute<PackageAttribute>();

            //判断空
            _command = (attr?.PackageName == null) ? this.GetType().Name : attr.PackageName;
            _assembly = this.GetType().Assembly.GetName().Name;
            _fullName = this.GetType().FullName;
        }

        /// <summary>
        /// 判断一个包和type是否相等
        /// </summary>
        /// <param name="package1"></param>
        /// <param name="package2"></param>
        /// <returns></returns>
        public static bool IsEqualpackage(CPackage package1,Type package2)
        {
            return (package1.Command == package2.Name) 
                && (package1.Assembly == package2.Assembly.GetName().Name) 
                && (package1.FullName == package2.FullName);
        }

        /// <summary>
        /// 根据解码器和序列化器来解析原始数据到指定包对象
        /// </summary>
        /// <typeparam name="ResulType"></typeparam>
        /// <param name="packageTranslate"></param>
        /// <param name="bodyTranslate"></param>
        /// <param name="originData"></param>
        /// <returns></returns>
        public static object? ConvertPackage(IPackageTranslate packageTranslate, IBodyTranslate bodyTranslate,byte[] originData)
        {
            //原始json
            string jsonStr = packageTranslate.Decoder(
                    originData
                    );
            CPackage? Ipackage = bodyTranslate.Deserialize<CPackage>(
                jsonStr
                );
            //实例化包（根据程序集和类名）

            ObjectHandle? obj = Activator.CreateInstance(Ipackage.Assembly, Ipackage.FullName);

            object? v = obj.Unwrap();
            //调用反序列化方法
            object? resPackage = bodyTranslate.GetType().GetMethod("Deserialize")
                ?.MakeGenericMethod(new Type[] { obj.Unwrap().GetType() })
                .Invoke(bodyTranslate, new object?[] {
                            jsonStr
                });

            return resPackage;
        }

        /// <summary>
        ///  命令头
        /// </summary>
        public string Command { get => _command; set => _command = value; }
        public string Assembly { get => _assembly; set => _assembly = value; }
        public string FullName { get => _fullName; set => _fullName = value; }
    }
}
