using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using MeJinkeWebAPI.Codes;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// DataAccess 的摘要说明
/// </summary>
/// 
namespace MeSoftOA.DAL
{
    public abstract class DataAccess
    {
        private string _connectionString = "";
        protected string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }
        /// <summary>
        /// 办公事务数据库连接字符串
        /// </summary>
        public string OAConnectionString { get; set; }

        private bool _enableCaching = true;
        protected bool EnableCaching
        {
            get { return _enableCaching; }
            set { _enableCaching = value; }
        }

        private int _cacheDuration = 0;

        protected int CacheDuration
        {
            get { return _cacheDuration; }
            set { _cacheDuration = value; }
        }

        protected Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }
        protected IDataReader GetIdataReader(string sql, SqlConnection cn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                IDataReader reader = ExecuteReader(cmd);

                return reader;
            }
            catch (Exception e)
            {
                e = Helpers.GetOriginalException(e);
                throw new Exception(e.Message);
                return null;
            }
        }
        public List<T> GetAllItems<T>(out string msg) where T:class
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                var type = typeof(T);
                object oObject = Activator.CreateInstance(type);
                string sql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} " +
                    $"FROM [{typeof(T).GetField("VisionName").GetValue(oObject)}]";

                try
                {

                    IDataReader reader = GetIdataReader(sql, cn);
                    var ret = new List<T>();

                    while (null != reader && reader.Read())
                    {
                        foreach (var prop in type.GetProperties())
                        {
                            prop.SetValue(oObject, reader[prop.Name]);
                        }
                        ret.Add((T)oObject);
                        oObject = Activator.CreateInstance(type);
                    }
                    msg = "ok";
                    return ret;
                }
                catch (Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                    return null;
                }

            }
        }

        protected int ExecuteNonQuery(DbCommand cmd)
        {
            if (HttpContext.Current.User.Identity.Name.ToLower() == "sampleeditor")
            {
                foreach (DbParameter param in cmd.Parameters)
                {
                    if (param.Direction == ParameterDirection.Output ||
                       param.Direction == ParameterDirection.ReturnValue)
                    {
                        switch (param.DbType)
                        {
                            case DbType.AnsiString:
                            case DbType.AnsiStringFixedLength:
                            case DbType.String:
                            case DbType.StringFixedLength:
                            case DbType.Xml:
                                param.Value = "";
                                break;
                            case DbType.Boolean:
                                param.Value = false;
                                break;
                            case DbType.Byte:
                                param.Value = byte.MinValue;
                                break;
                            case DbType.Date:
                            case DbType.DateTime:
                                param.Value = DateTime.MinValue;
                                break;
                            case DbType.Currency:
                            case DbType.Decimal:
                                param.Value = decimal.MinValue;
                                break;
                            case DbType.Guid:
                                param.Value = Guid.Empty;
                                break;
                            case DbType.Double:
                            case DbType.Int16:
                            case DbType.Int32:
                            case DbType.Int64:
                                param.Value = 0;
                                break;
                            default:
                                param.Value = null;
                                break;
                        }

                    }
                }
                return 1;
            }
            else
                return cmd.ExecuteNonQuery();
        }

        protected IDataReader ExecuteReader(DbCommand cmd)
        {
            return ExecuteReader(cmd, CommandBehavior.Default);
        }

        protected IDataReader ExecuteReader(DbCommand cmd, CommandBehavior behavior)
        {
            return cmd.ExecuteReader(behavior);
        }

        protected object ExecuteScalar(DbCommand cmd)
        {
            return cmd.ExecuteScalar();
        }
    }

}