using MeJinkeWebAPI.Models;
using MeSoftOA.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace MeJinkeWebAPI.BLL
{
    public class GoodsRepository
    {
        static string Url_GoodsInfoImages = "/Content/GoodsInfoImages/";


        public static WebReturnInfo QueryGoods(string base_url, HttpServerUtilityBase server)
        {
            WebReturnInfo ret = new WebReturnInfo();
            string msg;
            //

            var allgoods =  SiteProvider.JinkeProvider.GetAllGoods(out msg); //.  new GoodsInfo(); //

            if (null == allgoods || allgoods.Count == 0)
            {
                ret.code = "FAIL";
                ret.msg = msg;// "未找到商品记录";
                ret.data = null;
                return ret; // Json(ret, JsonRequestBehavior.AllowGet);
            }
            int storageID = 0;
            var holdplace = SiteProvider.JinkeProvider.GetStorageName(storageID);  //等于固定值storageID=0的Name值
            List<GoodsInfoBase> returnGoods = new List<GoodsInfoBase>();

            foreach (var goodsinfo in allgoods)
            {
                //goodsinfo.holdPlace = holdplace;
                //StockSub stock = SiteProvider.JinkeProvider.GetStock(storageID, goodsinfo.goodsId);
                //goodsinfo.inventory = stock != null ? stock.Qty : -1;
                
                if (saveGoodsImage(goodsinfo.goodsId, goodsinfo.byteImage, server))
                {
                    goodsinfo.goodsImages = base_url + Url_GoodsInfoImages + goodsinfo.goodsId + ".jpg";
                }

                GoodsInfoBase data = new GoodsInfoBase()
                {
                    goodsDescribe = goodsinfo.goodsDescribe,
                    goodsId = goodsinfo.goodsId,
                    goodsImages = goodsinfo.goodsImages,
                    goodsName = goodsinfo.goodsName,
                    goodsUpdateTime = goodsinfo.goodsUpdateTime,
                    holdPlace = goodsinfo.holdPlace,
                    holdPlaceCode = goodsinfo.holdPlaceCode,
                    inventory = goodsinfo.inventory,
                    marketPrice = goodsinfo.marketPrice,
                    settlePrice = goodsinfo.settlePrice,
                    specifications = goodsinfo.specifications
                };
                returnGoods.Add(data);
            }
            
            ret.data = returnGoods;
            return ret; // Json(ret, JsonRequestBehavior.AllowGet); //// ( "success");
        }
        /// <summary>
        /// 查询商品信息。通过此接口获得商品信息 对应表B_Goods中的商品库与G_StockSub中的StorageID=0的对应GoodsID的Qty
        /// </summary>
        /// <param name="goodsid"></param>
        /// <returns></returns>
        public static WebReturnInfo QueryGood(string goodsid, string base_url, HttpServerUtilityBase server)
        {
            WebReturnInfo ret = new WebReturnInfo();
            string msg;
            var goodsinfo = SiteProvider.JinkeProvider.GetGoodsInfo(goodsid, out msg); //.  new GoodsInfo(); //

            if (null == goodsinfo)
            {
                ret.code = "FAIL";
                ret.msg = msg;// "未找到商品记录";
                ret.data = null;
                return ret; // Json(ret, JsonRequestBehavior.AllowGet);
            }
            int storageID = 0;
            goodsinfo.holdPlace = SiteProvider.JinkeProvider.GetStorageName(storageID);
            StockSub stock = SiteProvider.JinkeProvider.GetStock(storageID, goodsid);
            goodsinfo.inventory = stock != null ? stock.Qty : -1;
            goodsinfo.goodsId = goodsid;
            if (saveGoodsImage(goodsid, goodsinfo.byteImage, server))
            {
                goodsinfo.goodsImages = base_url + Url_GoodsInfoImages + goodsid + ".jpg";
            }

            GoodsInfoBase data = new GoodsInfoBase()
            {
                goodsDescribe = goodsinfo.goodsDescribe,
                goodsId = goodsinfo.goodsId,
                goodsImages = goodsinfo.goodsImages,
                goodsName = goodsinfo.goodsName,
                goodsUpdateTime = goodsinfo.goodsUpdateTime,
                holdPlace = goodsinfo.holdPlace,
                holdPlaceCode = goodsinfo.holdPlaceCode,
                inventory = goodsinfo.inventory,
                marketPrice = goodsinfo.marketPrice,
                settlePrice = goodsinfo.settlePrice,
                specifications = goodsinfo.specifications
            };
            ret.data = data;
            return ret; // Json(ret, JsonRequestBehavior.AllowGet); //// ( "success");
        }
        /// <summary>
        /// 图片数据如果存在且还没有保存至文件，则保存此图片数据为文件
        /// </summary>
        /// <param name="goodsid"></param>
        /// <param name="image"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        private static bool saveGoodsImage(string goodsid, byte[] image, HttpServerUtilityBase server)
        {
            if (null == image || string.IsNullOrEmpty(goodsid))
            {
                return false;
            }
            //var server = HttpContext.Server;
            var Savedir = server.MapPath(Url_GoodsInfoImages); 
            var filename = Savedir + goodsid + ".jpg";
            
            if (!System.IO.File.Exists(filename))
            {
                if (!Directory.Exists(Savedir))
                {
                    Directory.CreateDirectory(Savedir);
                }
                MemoryStream s = new MemoryStream(image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(s);
                img.Save(filename);
                s.Close();
            }else
            {
                FileInfo file =  new FileInfo(filename);
                if (file.CreationTime.AddMonths(1) > DateTime.Now)  //已经超过1个月，可能需要更新
                {

                }
            }
            return true;
        }

        

        /// <summary>
        /// 订单支付通知
        /// </summary>
        /// <param name="pdata"></param>
        /// <returns></returns>
        public static WebReturnInfo PayOrder(payOrderInfo pdata)
        {
            WebReturnInfo ret = new WebReturnInfo();
            string msg;
            //业务逻辑
            bool bpay = SiteProvider.JinkeProvider.PayOrder(pdata, out msg);
            ret.msg = msg;
            if (!bpay)
            {
                ret.code = "FAIL";
            }

            return ret; // Json(ret, JsonRequestBehavior.AllowGet); //// ( "success");
        }
        /// <summary>
        /// 商品物流信息查询
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="expressNo"></param>
        /// <returns></returns>
        internal static WebReturnInfo QueryExpress(string orderNo, string expressNo)
        {
            WebReturnInfo ret = new WebReturnInfo();
            string msg;
            //业务逻辑
            GoodsExpressInfo item = SiteProvider.JinkeProvider.QueryExpress(expressNo, orderNo, out msg);
            ret.msg = msg;
            if (null == item)
            {
                ret.code = "FAIL";
            }
            ret.data = item;
            return ret;
        }

        /// <summary>
        /// 订单退款申请
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="descp"></param>
        /// <returns></returns>
        public static WebReturnInfo Apply4Refund(string orderNo, string descp)
        {
            WebReturnInfo ret = new WebReturnInfo();
            Apply4RefundResult result;
            var bapply = SiteProvider.JinkeProvider.Apply4Refund(orderNo, descp, out result);
            ret.data = result;
            if (!bapply)
            {
                ret.code = "FAIL";
                ret.msg = result.msg;
            }
            return ret;
        }
        internal static WebReturnInfo OrderRefundInform(string orderNo, string status, decimal amount)
        {
            WebReturnInfo ret = new WebReturnInfo();
            string msg;
            bool bapply = SiteProvider.JinkeProvider.OrderRefundInform(orderNo, status, amount, out msg);
            ret.msg = msg;
            if (!bapply)
            {
                ret.code = "FAIL";                
            }
            return ret;
        }
        internal static WebReturnInfo QueryOrder(string orderNo)
        {
            WebReturnInfo ret = new WebReturnInfo();
            return ret;
        }
    }

    

    /// <summary>
    /// 退款申请结果
    /// </summary>
    public class Apply4RefundResult
    {
        public virtual string orderNo { get; set; }
        public virtual string status { get; set; }
        public virtual string amount { get; set; }

        public virtual string msg { get; set; }
    }
}