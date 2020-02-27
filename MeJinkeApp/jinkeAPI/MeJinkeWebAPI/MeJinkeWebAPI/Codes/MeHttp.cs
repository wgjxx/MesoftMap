using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace MeJinkeWebAPI.Codes
{
    public class MeHttp
    {
        public static string HttpPost(string url, object postData)
        {
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ContentType = "application/json";
            webrequest.Method = "post";
            string posted = Helpers.SerializeObject(postData);
            byte[] postdatabyte = Encoding.UTF8.GetBytes(posted);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();
            using (var httpWebResponse = webrequest.GetResponse())
            using (Stream receiveStream = httpWebResponse.GetResponseStream())
            {
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string msg = readStream.ReadToEnd();
                return msg; // Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}