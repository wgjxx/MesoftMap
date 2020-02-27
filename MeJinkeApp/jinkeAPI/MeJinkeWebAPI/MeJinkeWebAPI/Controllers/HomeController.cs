using MeJinkeWebAPI.Codes;
using MeJinkeWebAPI.Models;
using Mesoft.Libraries.DAL;
using Mesoft.Libraries.Factory;
using Mesoft.Libraries.IDAL;
using MeSoftOA.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MeJinkeWebAPI.Controllers
{
    public class RequestState
    {
        // This class stores the State of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;
        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }


    public class HomeController : Controller
    {
        #region temp

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;
        const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

        // Abort the request if the timer fires.
        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.request;
                myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.streamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                return;
            }
            catch (WebException e)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }
            allDone.Set();
        }
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {

                RequestState myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.streamResponse;
                int read = responseStream.EndRead(asyncResult);
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                    return;
                }
                else
                {
                    Console.WriteLine("\nThe contents of the Html page are : ");
                    if (myRequestState.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = myRequestState.requestData.ToString();
                        Console.WriteLine(stringContent);
                    }
                    Console.WriteLine("Press any key to continue..........");
                    Console.ReadLine();



                    {
                        System.Drawing.Image img = System.Drawing.Image.FromStream(responseStream);
                        img.Save(@"E:\Mesoft\jinkeAPI\MeJinkeWebAPI\MeJinkeWebAPI\Content\test.jpg");

                        //String ret = responseStream.ReadToEnd();
                        //return "ok";
                        //string result = XmlDeserialize<string>(ret);

                        //return result;
                    }

                    responseStream.Close();
                }

            }
            catch (WebException e)
            {
                Console.WriteLine("\nReadCallBack Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }
            allDone.Set();

        }
        #endregion
        string testc(string funcn, int goodsid)
        {
            string host = Request.Url.Scheme + "://" + Request.Url.Host
               + (!Request.Url.IsDefaultPort ? (":" + Request.Url.Port) : "");
            string url =host + "/Jinke/get";
           
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.Method = "post";

            WebRequeryInfo postData = new WebRequeryInfo();
            switch (funcn.ToLower())
            {
                case "payorder":
                    postData.method = "payorder";
                    postData.data = "{\"orderNo\":\"1001\", \"orderStatus\":\"已支付\", \"goodsId\":\"1\", \"goodsName\":\"test goodname\"" +
                        ", \"goodsSpecification\":\"test goodsSpecification\", \"name\":\"test name\", \"mobile\":\"1332112\", \"receiveName\":\"test receiveName\"" +
                        ", \"receiveMobile\":\"1772112\", \"receiveAddress\":\"beijing\", \"settlePrice\":\"255\", \"alipayTradeNo\":\"test alipayTradeNo\"" +
                        ", \"orderTradeTime\":\"130000002222\"}";
                    break;
                case "apply4refund":
                    postData.method = "apply4refund";
                    postData.data = "{ orderNo:\"orderNo-1001\", descp:\"这是一个测试退款申请\"}";
                    break;
                case "refundinform":
                    postData.method = "refundinform";
                    postData.data = "{ orderNo:\"1001\", Status:\"OK\", Amount:100}";
                    break;
                case "querygoods":
                    postData.method = "querygoods";
                    postData.data = "{ goodsID:\"1\"}";
                    break;
                case "querygood":
                    postData.method = "querygood";

                    postData.data = "{ goodsID:\"" + goodsid +"\"}";
                    break;
                case "queryexpress":
                    postData.method = "queryexpress";
                    postData.data = "{ orderNo:\"1001\", expressNo:\"1\"}";
                    break;
            }

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
                return msg;
            }
        }
        string testJinkeAPI()
        {
            RSACryption rsaS = new RSACryption();
            string xmlKeys, xmlPublicKey;
            rsaS.RSAKey(out xmlKeys, out xmlPublicKey);
            string host = Request.Url.Scheme + "://" + Request.Url.Host
               + (!Request.Url.IsDefaultPort ? (":" + Request.Url.Port) : "");
            string url = host +"/jinke/GetJinkeApi";
            HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webrequest.ContentType = "application/json";
            webrequest.Method = "post";

            WebRequeryInfo postData = new WebRequeryInfo();
            postData.appId = "HAIER";
            postData.data = "{ \"orderNo\":\"ORD373484292455571475\"}";
            postData.method = "supplier.orderQuery";//"open.api.conn.test";

            postData.timestamp = DateTime.Now.Ticks.ToString();
            postData.seqNo = "HAIERseq" + postData.timestamp; // 368705089608618029";
            postData.v = "1.0";
            string plaintext = string.Format("appId={0}&data={1}&method={2}&seqNo={3}&timestamp={4}&v={5}",
                postData.appId, postData.data, postData.method, postData.seqNo, postData.timestamp, postData.v);
           
            //prikey = prikey.Replace("\n", "");
            //string xmlkey = RSAKeyConvert.RSAPrivateKeyJava2DotNet(prikey);
            //string sig = Helpers.RSASign(plaintext, xmlkey);
           
            //postData.signature = sig;
            string posted = Helpers.SerializeObject(postData);
            byte[] postdatabyte = Encoding.UTF8.GetBytes(posted);
            webrequest.ContentLength = postdatabyte.Length;
            Stream stream;
            stream = webrequest.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            try
            {
                using (var httpWebResponse = webrequest.GetResponse())
                using (Stream receiveStream = httpWebResponse.GetResponseStream())
                {                    
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string msg = readStream.ReadToEnd();
                    return (msg);
                }
            }
            catch (Exception e)
            {
                e = Helpers.GetOriginalException(e);
                return e.Message;
            }

        }
        void testimg()
        {
            var server = HttpContext.Server;
            WebRequest myrequest = WebRequest.Create("http://localhost:56555/content/身份证.jpg");
            WebResponse myresponse = myrequest.GetResponse();
            Stream imgstream = myresponse.GetResponseStream();

            System.Drawing.Image img = System.Drawing.Image.FromStream(imgstream);

            var Savedir = Server.MapPath("/Content/GoodsInfoImages/");
            if (!Directory.Exists(Savedir))
            {
                Directory.CreateDirectory(Savedir);
            }
            var filename = Savedir + "test.jpg";
            if (!System.IO.File.Exists(filename))
            {
                img.Save(filename);
            }
            imgstream.Close();

            return;
                      

        }
        void testInsImg()
        {
           
            var Savedir = Server.MapPath("/Content/");
            var filename = Savedir + "apple.jpg";
            System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
            MemoryStream s = new MemoryStream();
            img.Save(s, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] image = s.ToArray();
            s.Close();
            string msg;
            bool bInsImg = SiteProvider.JinkeProvider.InsertGoodsImage(1, image, out msg);
            s.Close();
            
        }
        void testGetSqlImg()
        {
            byte[] b = SiteProvider.CSProvider.GetUserImage(4);
            MemoryStream s = new MemoryStream(b);
            System.Drawing.Image image = System.Drawing.Image.FromStream(s);
            var server = HttpContext.Server;
            var Savedir = Server.MapPath("/Content/GoodsInfoImages/");
            var filename = Savedir + "test001.jpg";
            image.Save(filename);


        }


       
        public string Test(string funcn, int goodsid=0)
        {
            string msg;
            var items = SiteProvider.CSProvider.GetAllItems<C_Article>(out msg);  //测试泛型，以后取数据都这样写
            IBaseDAL dal = SimpleFactory.CreateInstance(); // new BaseDAL();
            items =dal.FindAll<C_Article>();
            var item = dal.Find<C_Article>(3);
            item.ArticleTitle = "p 这是一个利用泛型新增的记录";
            item.Digest = "王国建测试新" +
                "增泛型添加记录";
            item.TypeID = -1;
            int  newID = dal.Add<C_Article>(item);
            if (string.IsNullOrEmpty(funcn))
            {
                return testJinkeAPI();
            }            
            return testc(funcn, goodsid);
        }
        public ActionResult Index()
        {
            var now = DateTime.Now;
            if (now.AddDays(1) > now)
            {
                Console.Beep();
            }
            string msg = "ok";
            try
            {
                
                //testInsImg();
                //testimg();
                //testGetSqlImg();
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}