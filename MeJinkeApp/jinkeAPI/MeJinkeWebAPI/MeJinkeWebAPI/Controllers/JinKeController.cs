using MeJinkeWebAPI.BLL;
using MeJinkeWebAPI.Codes;
using MeJinkeWebAPI.Models;
using MeSoftOA.DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace MeJinkeWebAPI.Controllers
{

    public class JinKeController : Controller
    {
        #region "用于调用金科接口，并以JSON形式将值返回给调用者"
        /// <summary>
        /// 金科接口的URL地址
        /// </summary>
        static readonly string url_jinkeAPI = Globals.Settings.JinkeAPIUrl; // "https://share-test1.zhexinit.com/open/gateway";
        /// <summary>
        /// 海尔调用金科接口时的签名私钥
        /// </summary>
        static readonly string prikey = Globals.Settings.JinkHaierPrivkey;
        /// <summary>
        /// 判断调用金科API接口时参数是否合法
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool isJinkeApiParametersValid(WebRequeryInfo postData, out string msg)
        {
            msg = "ok";
            if (string.IsNullOrEmpty(postData.appId))
            {
                msg = "请提供appId参数";
                return false;
            }
            else if (string.IsNullOrEmpty(postData.data))
            {
                msg = "请提供data参数";
                return false;
            }
            else if (string.IsNullOrEmpty(postData.method))
            {
                msg = "请提供method参数";
                return false;
            }
            else if (string.IsNullOrEmpty(postData.seqNo))
            {
                msg = "请提供seqNo参数";
                return false;
            }
            else if (string.IsNullOrEmpty(postData.timestamp))
            {
                msg = "请提供timestamp参数";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 金科提供的接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetJinkeApi(/*[FromBody] WebRequeryInfo postData*/)
        {
            try
            {
                string jsonData = Helpers.GetRequestBody();
                WebRequeryInfo postData = Helpers.DeserializeJsonToObject<WebRequeryInfo>(jsonData);
                string msg = "{  \"seqNo\":\"JKZX22344556566677\",  " +
                        " \"appId\":\"HAIER\", " + " \"method\":\"supplier.orderQuery\", " +
                        "  \"timestamp\": \"130000002222\"," +
                        "  \"data\":\"{\"x\": \"xx\"}\",\"v\":\"1.0\" } ";
                if (null == postData)
                {                    
                    return Json(string.Format("错误请求，请检查请求数据格式（形如：{0}）。", msg), JsonRequestBehavior.AllowGet);
                }else if(!isJinkeApiParametersValid(postData, out msg))
                {
                    return Json(string.Format("错误请求，{0}。", msg), JsonRequestBehavior.AllowGet);
                }

                string plaintext = postData.GetPlaintext();
                
                string xmlkey = RSAKeyConvert.RSAPrivateKeyJava2DotNet(prikey);//需要将私钥形式转换成XML格式，以供C#调用
                string sig = Helpers.RSASign(plaintext, xmlkey);
                postData.signature = sig;

                msg = MeHttp.HttpPost(url_jinkeAPI, postData);
                return Json(msg, JsonRequestBehavior.AllowGet);               
            }
            catch (Exception e)
            {
                e = Helpers.GetOriginalException(e);
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
        #endregion
        /// <summary>
        /// 调用海尔的接口，根据method参数的值调用相应的功能
        /// </summary>
        /// <returns></returns>
        public JsonResult Get()
        {
            
            string jsonData = Helpers.GetRequestBody();
            WebRequeryInfo req = Helpers.DeserializeJsonToObject<WebRequeryInfo>(jsonData);
            if (null == req)
            {
                string msg = "{  \"seqNo\":\"JKZX22344556566677\",  " +
                    " \"appId\":\"JKZX\", " + " \"method\":\"supplier.orderQuery\", " +
                    "  \"timestamp\": \"130000002222\"," +
                    "  \"data\":\"{\"x\": \"xx\"}\",\"v\":\"1.0\" } ";
                return Json(string.Format("错误请求，请检查请求数据格式（形如：{0}）。", msg), JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(req.method))
            {
                return Json("Method参数错误！", JsonRequestBehavior.AllowGet);
            }
            WebReturnInfo ret;
            object data;
            switch (req.method.ToLower())
            {
                case "querygoods":
                    return querygoods();
                    
                case "goodsquery":
                   return querygoods();
                    
                case "querygood":
                     data = Helpers.DeserializeJsonToObject<QueryGoodsData>(req.data);                    
                    if (null != data)
                    {
                        return querygood(((QueryGoodsData)data).goodsID);
                    }
                    else
                    {
                        ret = new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    }                    
                    break;
                case "goodquery":
                    data = Helpers.DeserializeJsonToObject<QueryGoodsData>(req.data);
                    if (null != data)
                    {
                        return querygood(((QueryGoodsData)data).goodsID);
                    }
                    else
                    {
                        ret = new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    }
                    break;
                case "payorder":
                    payOrderInfo pdata = Helpers.DeserializeJsonToObject<payOrderInfo>(req.data);
                    ret = null != pdata ? GoodsRepository.PayOrder(pdata) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    //return PayOrder(pdata);
                    break;
                case "orderpaynotice":
                    pdata = Helpers.DeserializeJsonToObject<payOrderInfo>(req.data);
                    ret = null != pdata ? GoodsRepository.PayOrder(pdata) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    //return PayOrder(pdata);
                    break;
                case "apply4refund":
                    Apply4RefundData adata = Helpers.DeserializeJsonToObject<Apply4RefundData>(req.data);
                    ret = null != adata ? GoodsRepository.Apply4Refund(adata.orderNo, adata.descp) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                case "orderrefundnotice":
                    RefundInformData ridata = Helpers.DeserializeJsonToObject<RefundInformData>(req.data);
                    ret = null != ridata ? GoodsRepository.OrderRefundInform(ridata.OrderNo, ridata.Status, ridata.Amount) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                case "refundinform":
                    ridata = Helpers.DeserializeJsonToObject<RefundInformData>(req.data);
                    ret = null != ridata ? GoodsRepository.OrderRefundInform(ridata.OrderNo, ridata.Status, ridata.Amount) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                case "queryorder":
                    QueryorderData qodata = Helpers.DeserializeJsonToObject<QueryorderData>(req.data);
                    ret = null != qodata ? GoodsRepository.QueryOrder(qodata.orderNo) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                case "queryexpress":
                    data = Helpers.DeserializeJsonToObject<QueryExpressData>(req.data);
                    QueryExpressData qedata = (QueryExpressData)data;
                    ret = null != data ? GoodsRepository.QueryExpress(((QueryExpressData)data).orderNo, qedata.expressNo) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                case "goodsExpressQuery":
                    data = Helpers.DeserializeJsonToObject<QueryExpressData>(req.data);
                    qedata = (QueryExpressData)data;
                    ret = null != data ? GoodsRepository.QueryExpress(((QueryExpressData)data).orderNo, qedata.expressNo) :
                        new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                    break;
                default:
                    ret = new WebReturnInfo() { code = "fail", msg = "error" };
                    break;
            }

            return Json(ret, JsonRequestBehavior.AllowGet); // QueryGoods("1");
        }

        JsonResult querygoods()
        {
            string BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);

            WebReturnInfo ret = GoodsRepository.QueryGoods(BASE_URL, Server);
            //new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult querygood(string goodsid)
        {
            //string BASE_URL = Request.Url.Scheme + "://" + Request.Url.Host 
            //    + (!Request.Url.IsDefaultPort ? (":" + Request.Url.Port): "");
            //BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
            string BASE_URL = Request.Url.GetLeftPart(UriPartial.Authority);
            
            WebReturnInfo ret = GoodsRepository.QueryGood(goodsid, BASE_URL, Server);
                //new WebReturnInfo() { code = "fail", msg = "data数据格式不对！" };
                return Json(ret, JsonRequestBehavior.AllowGet);
        }

       

        class QueryorderData {
            public string orderNo { get; set; }
        }
        class Apply4RefundData
        {
            public virtual string orderNo { get; set; }
            public virtual string descp { get; set; }
        }
        /// <summary>
        /// 订单退款通知
        /// </summary>
        class RefundInformData
        {
            public virtual string OrderNo    { get; set; }
            public virtual string Status { get; set; }
            public virtual decimal Amount { get; set; }
        }
        class QueryExpressData
        {
            public string orderNo { get; set; }
            public string expressNo { get; set; }
        }
    }
    class QueryGoodsData
    {
        public string goodsID { get; set; }
    }
    

}
