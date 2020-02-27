using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MeJinkeWebAPI.Config
{
    public class MeSection : ConfigurationSection
    {
        [ConfigurationProperty("connectionStringName", DefaultValue = "LocalSqlServer")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
        [ConfigurationProperty("sqlConnectionStringName", DefaultValue = "LocalSqlServer")]
        public string SqlConnectionStringName
        {
            get { return (string)base["sqlConnectionStringName"]; }
            set { base["sqlConnectionStringName"] = value; }
        }
        [ConfigurationProperty("csSqlConnectionStringName", DefaultValue = "CSSqlServer")]
        public string csSqlConnectionStringName
        {
            get {
                string s = (string)base["csSqlConnectionStringName"];
                return (string)base["csSqlConnectionStringName"];
            }
            set { base["csSqlConnectionStringName"] = value; }
        }

        [ConfigurationProperty("providerType", DefaultValue = "MeJinkeWebAPI.DAL.SqlClient.SqlSiteProvider")]
        public string ProviderType
        {
            get { return (string)base["providerType"]; }
            set { base["providerType"] = value; }
        }


        [ConfigurationProperty("csProviderType", DefaultValue = "MeJinkeWebAPI.DAL.SqlClient.SqlCsSiteProvider")]
        public string CSProviderType
        {
            get { return (string)base["csProviderType"]; }
            set { base["csProviderType"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = (string.IsNullOrEmpty(this.SqlConnectionStringName) ?
                   Globals.Settings.SqlConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        public string CSConnectionString
        {
            get
            {
                string connStringName = (!string.IsNullOrEmpty(this.csSqlConnectionStringName) ?
                   this.csSqlConnectionStringName : this.ConnectionStringName);
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }


        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)base["enableCaching"]; }
            set { base["enableCaching"] = value; }
        }
        [ConfigurationProperty("defaultCacheDuration", DefaultValue = "600")]
        public int DefaultCacheDuration
        {
            get { return (int)base["defaultCacheDuration"]; }
            set { base["defaultCacheDuration"] = value; }
        }
        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)base["cacheDuration"];
                return (duration > 0 ? duration : this.DefaultCacheDuration);
            }
            set { base["cacheDuration"] = value; }
        }


        #region 'jinke api
        [ConfigurationProperty("UrlOfJinkeAPI", DefaultValue = "https://share-test1.zhexinit.com/open/gateway")]
        public string JinkeAPIUrl
        {
            get { return (string)base["UrlOfJinkeAPI"]; }
            set { base["UrlOfJinkeAPI"] = value; }
        }
        [ConfigurationProperty("RSAPrivatKey", DefaultValue = "111")]
        public string RSAPrivatKey
        {
            get { return (string)base["RSAPrivatKey"]; }
            set { base["RSAPrivatKey"] = value; }
        }
        static string _JinkHaierPrivkey = null;
        public string JinkHaierPrivkey { get {if (string.IsNullOrEmpty(_JinkHaierPrivkey)){
                    _JinkHaierPrivkey = RSAPrivatKey.Replace("\n", "");                    
                }
                return _JinkHaierPrivkey; }
        }

        #endregion
    }
}