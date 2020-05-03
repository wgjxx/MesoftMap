using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesoftMap.Utility.WebHelper
{
    public class CommonHelper
    {
        public static async System.Threading.Tasks.Task<string> GetRequestBodyAsync(HttpRequest req)
        {
            Stream stream = req.Body;
            byte[] buffer = new byte[req.ContentLength.Value];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            string jsonData = Encoding.UTF8.GetString(buffer);
            return jsonData;

        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string jsonData, out string msg) where T : class
        {
            try
            {
                msg = "ok";
                JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                StringReader sr = new StringReader(jsonData);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
                T item = o as T;
                return item;
            }
            catch (Exception e)
            {                
                msg = "failed to DeserializeJsonToObject:" + jsonData + "\n error:" + e.Message;
                return null;
            }

        }


    }
}
