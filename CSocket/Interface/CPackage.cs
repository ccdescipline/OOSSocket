using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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




        //public CPackage(string Command)
        //{
        //    _command = Command;
        //}


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
        ///  命令头
        /// </summary>
        public string Command { get => _command; set => _command = value; }
        public string Assembly { get => _assembly; set => _assembly = value; }
        public string FullName { get => _fullName; set => _fullName = value; }
    }
}
