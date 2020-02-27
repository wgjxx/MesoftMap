using Mesoft.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace MeJinkeWebAPI.Models
{
    public interface IDataBase
    {
        string GetTableName();

    }
    public class GoodsInfoBase
    {       
        

        public virtual string goodsId { get; set; }
        public virtual string goodsName { get; set; }

        public virtual int settlePrice { get; set; }
        public virtual int marketPrice { get; set; }

        public virtual string specifications { get; set; }
        public virtual int inventory { get; set; }

        public virtual string goodsUpdateTime { get; set; }
        public virtual string holdPlaceCode { get; set; }

        public virtual string holdPlace { get; set; }
        public virtual string goodsImages { get; set; }
        public virtual string goodsDescribe { get; set; }
        
    }
    [Table("B_Goods")]
    public class GoodsInfo : GoodsInfoBase, IDataBase
    {
        public GoodsInfo()
        {
            this.goodsName = "test goodsname";
            this.goodsUpdateTime = DateTime.Now.ToLongDateString();
        }

        public readonly static string TableName = "B_Goods";
        public readonly static string VisionName = "V_GoodsStock";


        public virtual byte[] byteImage { get; set; }

        internal static GoodsInfo GetItemFromReader(IDataReader reader)
        {
            if (null == reader) return null;

            GoodsInfo item = new GoodsInfo();

            item.goodsId = reader["GoodsID"].ToString();
            item.goodsDescribe = reader["goodsDescribe"].ToString();

            //item.goodsImages = reader["Image"].ToString();
            item.goodsName = reader["goodsName"].ToString();
            item.goodsUpdateTime = reader["goodsUpdateTime"].ToString();
            
            item.holdPlaceCode = reader["holdPlaceCode"].ToString();
            item.holdPlace = reader["holdPlace"].ToString();
            item.inventory = System.DBNull.Value != reader["inventory"] ? Convert.ToInt32((decimal)reader["inventory"]):0;
            // item.inventory = 库存 G_StockSub表下的 Qty
            item.marketPrice = System.DBNull.Value != reader["marketPrice"] ?  Convert.ToInt32( (decimal)reader["marketPrice"]):0;
            item.settlePrice = System.DBNull.Value != reader["settlePrice"] ? Convert.ToInt32((decimal)reader["settlePrice"]):0;
            item.specifications = reader["specifications"].ToString();

            item.byteImage =  System.DBNull.Value != reader["Image"] ? (byte[])reader["Image"] : null;
            return item;
        }

        public string GetTableName()
        {
            return VisionName;
        }
    }
    #region 订单信息
    public class OrderBase
    {
        public virtual string orderNo { get; set; }

        public virtual string receiveName { get; set; }

        public virtual string receiveMobile { get; set; }
        public virtual string receiveAddress { get; set; }

        public virtual string goodsName { get; set; }
        public virtual string goodsId { get; set; }

        public virtual string goodsSpecification { get; set; }
    }
    public class orderInfo:OrderBase
    {

        public virtual string name { get; set; }

        public virtual string mobile { get; set; }
        public virtual string orderStatus { get; set; }
        public virtual int settlePrice { get; set; }

        public virtual string expressNo { get; set; }
        public virtual string snNo { get; set; }

        public virtual string expressCompany { get; set; }
        public virtual string deliveryTime { get; set; }
    }
    /// <summary>
    /// 订单支付信息
    /// </summary>
    public class payOrderInfo:OrderBase
    {
        

        public virtual string orderStatus { get; set; }
        public virtual string name { get; set; }

        public virtual string mobile { get; set; }
        public virtual int settlePrice { get; set; }
        public virtual string alipayTradeNo { get; set; }

        public virtual long orderTradeTime { get; set; }
    }
    /// <summary>
    /// 商品物流信息
    /// </summary>
    public class GoodsExpressBase : OrderBase
    {       
        public virtual string snNo { get; set; }

        public virtual string deliveryTime { get; set; }

        public virtual string expressCompany { get; set; }
        public virtual string expressStatus { get; set; }
        public virtual string receiveTime { get; set; }
    }
    public class GoodsExpressInfo : GoodsExpressBase
    {
        static readonly string _tableName = "V_SellMaster";
        public static string TableName { get { return _tableName; } }
        internal static GoodsExpressInfo GetItemFromReader(IDataReader reader)
        {
            if (null == reader) return null;

            GoodsExpressInfo item = new GoodsExpressInfo();

            //这个可能要到另外表里去查询
            item.goodsId = reader["GoodsID"].ToString();
            item.goodsName = reader["FullName"].ToString();
            item.expressCompany = reader["ExpressCompany"].ToString();
            item.expressStatus = reader["ExpressStatus"].ToString();
            item.receiveName = reader["ReceiveName"].ToString();
            item.receiveMobile = reader["ReceiveMobile"].ToString();
            item.receiveAddress = reader["ReceiveAddress"].ToString();
            item.goodsSpecification = reader["GoodsSpecification"].ToString();
            item.snNo = reader["SnNo"].ToString();
            item.deliveryTime = reader["DeliveryTime"].ToString();
            item.receiveTime = reader["ReceiveTime"].ToString();
            item.orderNo = reader["OrderNo"].ToString();


            return item;
        }
    }
    #endregion
    public class StorageInfo
    {
        public readonly static string TableName = "B_Storage";

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }

    }

    public class StockSub
    {
        public readonly static string TableName = "G_StockSub";

        public virtual int StorageID { get; set; }
        public virtual int Qty { get; set; }
    }

    public class SellMaster
    {
        public readonly static string TableName = "G_SellMaster";

        public virtual string VrNo { get; set; }
        public virtual string VrFlag { get; set; }

        public virtual string CustName { get; set; }   //name, receiveName两个合值 
        public virtual string Mobile { get; set; }

        public virtual string Tel { get; set; }
        public virtual string Address { get; set; }

        public virtual string VrMemo { get; set; }
    }
    public class SellDetail
    {
        public readonly static string TableName = "G_SellDetail";

        public virtual string VrID { get; set; }
        public virtual string GoodsID { get; set; }
        public virtual string FullName { get; set; }  
        public virtual string Modal { get; set; }

        public virtual int SingPrice { get; set; }

        public virtual int VrSum { get; set; }
        
    }

    public class C_Article : BaseModel
    {
        public readonly static string VisionName = "C_Articles";
        readonly static string TableName = "C_Articles";
        public override string GetTableName()
        {
            return TableName;
        }
        public C_Article()
        {
            this.RecordTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }
        [Column("TypeID")]
        public int TypeID { get; set; }
        [Column("Title")]
        public string ArticleTitle { get; set; }
        public string Digest { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int WriterID { get; set; }
        public string UploadedBy { get; set; }

        public DateTime RecordTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}