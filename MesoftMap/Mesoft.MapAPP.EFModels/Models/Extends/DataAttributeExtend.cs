using Mesoft.EF.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Mesoft.EF.Model.Extends
{
    public static class DataAttributeExtend
    {

        /// <summary>
        /// 获取数据模型的key字段名字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetKeyName<T>(this T t) where T : BaseModel
        {
            Type type = t.GetType();
            string keyname = null;
            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(KeyAttribute), true))
                {
                    keyname = prop.GetColumn();
                    break;
                }
            }
            return keyname;
        }
        /// <summary>
        /// 获取类中属性的字段标记的名称，主要用于获取字段在数据表中的字段名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetColumn<T>(this T p) where T : PropertyInfo
        {

            if (p.IsDefined(typeof(ColumnAttribute)))
            {
                ColumnAttribute attribute = (ColumnAttribute)p.GetCustomAttribute(typeof(ColumnAttribute), true);
                return attribute.Name;
            }

            return p.Name;
        }

        public static string GetDisplayName(this PropertyInfo prop)
        {
            Type type = prop.GetType();
            var field = type.GetField(prop.ToString());
            if (field.IsDefined(typeof(DisplayAttribute), true))
            {
                DisplayAttribute attribute = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute), true);
                return attribute.GetName();
            }
            else
            {
                return prop.ToString();
            }
        }
        //public static ValidateErrorModel Validate<T>(this T t)
        //{
        //    Type type = t.GetType();
        //    foreach (var prop in type.GetProperties())
        //    {
        //        object oValue = prop.GetValue(t);
        //        LongAttribute attribute = (LongAttribute)prop.GetCustomAttribute(typeof(LongAttribute));
        //        if (prop.IsDefined(typeof(LongAttribute)))
        //        {
        //            ValidateErrorModel ret = attribute.Validate(oValue);
        //            if (!ret.Result)
        //            {
        //                return ret;
        //            }
        //        }
        //    }
        //    return new ValidateErrorModel() { Result = true, Message = "校验成功" };
        //}
        //public static List<ValidateErrorModel> ValidateAll<T>(this T t)
        //{
        //    List<ValidateErrorModel> validates = new List<ValidateErrorModel>();
        //    Type type = t.GetType();
        //    foreach (var prop in type.GetProperties())
        //    {
        //        object oValue = prop.GetValue(t);
        //        LongAttribute attribute = (LongAttribute)prop.GetCustomAttribute(typeof(LongAttribute));
        //        if (prop.IsDefined(typeof(LongAttribute)))
        //        {
        //            ValidateErrorModel result = attribute.Validate(oValue);
        //            validates.Add(result);
        //        }
        //    }
        //    return validates;
        //}
    }
}
