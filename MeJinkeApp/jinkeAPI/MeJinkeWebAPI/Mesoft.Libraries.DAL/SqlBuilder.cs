using Mesoft.Extends;
using Mesoft.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Libraries.DAL
{
    /// <summary>
    /// 缓存固定Sql
    /// </summary>
    public class MeSqlBuilder<T> where T:BaseModel
    {
        public static string FindSql = null;
        public static string FindAllSql = null;
        public static string AddSql = null;

        static MeSqlBuilder()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            FindAllSql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{ p.GetColumn()}] AS [{ p.Name}]"))} " +
                $"FROM [{t.GetTableName()}]";
            FindSql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{ p.GetColumn()}] AS [{ p.Name}]"))} " +
                $"FROM [{typeof(T).GetField("VisionName").GetValue(t)}] where ID=@ID";

            string columnString = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly |
                BindingFlags.Instance | BindingFlags.Public).Select(p => $"[{p.GetColumn()}]"));   //不要父类的属性

            string valueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly |
               BindingFlags.Instance | BindingFlags.Public)
               .Select(p => $"@{p.Name}"));   //插入记录时，参数化，这样可以避免注入攻击
            
                

            AddSql = $"Insert [{t.GetTableName()}] ({columnString}) values ({valueColumn}); select  @@Identity";
        }
    }
}
