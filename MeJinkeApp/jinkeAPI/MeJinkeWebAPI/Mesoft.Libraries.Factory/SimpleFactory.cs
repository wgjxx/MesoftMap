using Mesoft.Framework;
using Mesoft.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Libraries.Factory
{
    public class SimpleFactory
    {
        /// <summary>
        /// 通过实现不同的IBaseDAL来连接不同的数据库
        /// </summary>
        private static string DLLName = StaticConstraint.IBaseDALConfig.Split(',')[1];
        private static string TypeName = StaticConstraint.IBaseDALConfig.Split(',')[0];
       
        public static IBaseDAL CreateInstance()
        {
            test tt;
           
            
            Assembly assembly = Assembly.Load(DLLName);
            Type type = assembly.GetType(TypeName);
            object oDBHelper = Activator.CreateInstance(type);
            return (IBaseDAL)oDBHelper;
            IBaseDAL iDBHelper = oDBHelper as IBaseDAL;
            return iDBHelper;
        }
    }

    struct test
    {
        public int id;
        public string tests { get; set; }
    }
}
