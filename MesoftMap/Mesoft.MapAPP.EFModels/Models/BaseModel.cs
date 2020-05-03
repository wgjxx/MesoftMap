using Mesoft.EF.Model.Extends;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mesoft.EF.Model.Models
{
    public abstract class BaseModel
    {
        public abstract string GetTableName();
        static string _keyName = null;

        public virtual string GetKeyName()
        {
            Type type = this.GetType();
            if (string.IsNullOrEmpty(_keyName))
                foreach (var prop in type.GetProperties())
                {
                    if (prop.IsDefined(typeof(KeyAttribute), true))
                    {
                        _keyName = prop.GetColumn();
                        break;
                    }
                }
            return _keyName;
        }
        /// <summary>
        /// 数据表的主键Key
        /// </summary>
        [Key]
        //[JsonProperty("ID")]
        public virtual int ID { get; set; }
    }
}
