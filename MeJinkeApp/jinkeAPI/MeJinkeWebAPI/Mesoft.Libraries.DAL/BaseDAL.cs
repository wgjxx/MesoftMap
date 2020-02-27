using Mesoft.Extends;
using Mesoft.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Libraries.DAL
{
    public class MeshoppingBaseDAL : IBaseDAL
    {
        readonly static string ConnectionString =
            ConfigurationManager.ConnectionStrings["CSSqlServer"].ConnectionString;
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>返回新增后的自增ID号</returns>
        public int Add<T>(T t) where T : BaseModel
        {
            Type type = t.GetType();
            
            string sql = MeSqlBuilder<T>.AddSql;
            var parameterList = type.GetProperties(BindingFlags.DeclaredOnly |
               BindingFlags.Instance | BindingFlags.Public)
               .Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value));  //注意可空类型  
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                var parameters = parameterList.ToArray();
                command.Parameters.AddRange(parameters);
                conn.Open();
                object obj = command.ExecuteScalar();
                return Convert.ToInt32(obj); // obj as int;
                //在sql 后面增加 Select @@Identity; ExecuteScalar可以返回自增的ID号
                
            }
        }
        #region "private methods"
        private T Trans<T>(Type type, SqlDataReader reader)
        {
            object oObject = Activator.CreateInstance(type);
            foreach (var prop in type.GetProperties())
            {
                prop.SetValue(oObject, reader[prop.Name] is DBNull ? null : reader[prop.Name]);  //如果数据库中字段是Null,则赋值为null
            }
            return (T)oObject;
        }
        #endregion
        public T Find<T>(int id) where T : BaseModel
        {
            var type = typeof(T);
            object oObject = Activator.CreateInstance(type);
            string sql = MeSqlBuilder<T>.FindSql;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("ID", id));
               conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    oObject = this.Trans<T>(type, reader);
                    return (T)oObject;
                } 
                else
                {
                    return null;  //数据库没有记录，则返回null
                }
            }

        }

        public List<T> FindAll<T>() where T : BaseModel
        {
            var type = typeof(T);
            object oObject = Activator.CreateInstance(type);
            string sql = MeSqlBuilder<T>.FindAllSql;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                List<T> tList = new List<T>();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {                    
                    oObject = this.Trans<T>(type, reader);
                    tList.Add((T)oObject);
                }
                return tList;
            }
        }
    }
}
