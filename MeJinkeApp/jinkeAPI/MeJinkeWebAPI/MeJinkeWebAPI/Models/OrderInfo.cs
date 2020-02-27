using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MeJinkeWebAPI.Models
{
    public class OrderInfo
    {

        public readonly static string TableName = "B_Goods";

        public virtual string orderNo { get; set; }
        public virtual string orderStatus { get; set; }

        public virtual string receiveName { get; set; }
        public virtual string receiveMobile { get; set; }

        public virtual string receiveAddress { get; set; }
        public virtual string name { get; set; }

        public virtual string mobile { get; set; }
        public virtual string goodsName { get; set; }

        public virtual string goodsId { get; set; }
        public virtual string goodsSpecification { get; set; }
        public virtual string settlePrice { get; set; }


        public virtual string expressNo { get; set; }
        public virtual string snNo { get; set; }
        public virtual string expressCompany { get; set; }

        public virtual string deliveryTime { get; set; }

        internal static OrderInfo GetItemFromReader(IDataReader reader)
        {
            if (null == reader) return null;

            OrderInfo item = new OrderInfo();

            item.orderNo = reader["orderNo"].ToString();
            item.settlePrice = reader["Memo"].ToString();

            item.goodsSpecification = reader["Image"].ToString();
            item.orderStatus = reader["FullName"].ToString();
            item.mobile = reader["RecTime"].ToString();
            //item.holdPlace = (DateTime)reader["UpdateTime"];   隶属货舱   固定值B_Storage下ID=0的Name 

            item.goodsName = reader["SupplierNo"].ToString();
            // item.inventory = 库存 G_StockSub表下的 Qty
            

            return item;
        }
    }
}