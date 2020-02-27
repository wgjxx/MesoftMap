using MeJinkeWebAPI.BLL;
using MeJinkeWebAPI.Codes;
using MeJinkeWebAPI.Controllers;
using MeJinkeWebAPI.Models;
using MeSoftOA.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace MeJinkeWebAPI.DAL.SqlClient
{
    public class SqlCsSiteProvider: CSSiteProvider
    {
        internal override bool InsertUserImage(int id, byte[] image, out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = "insert   into   S_UserInfoes(Image,UnionID, Gender, RecordTime)   values   (@i,'Hi image', 1, '2019-1-1')";
                SqlCommand cmd = new SqlCommand(sql, cn);
               // cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                cmd.Parameters.Add("@i", SqlDbType.Image, (int)image.Length);
                cmd.Parameters["@i"].Value = image;

                cn.Open();
                cmd.ExecuteNonQuery();
                msg = "ok";
                return true;
            }
        }
        //public override List<T> GetAllItems<T>(out string msg)
        //{
        //    using (SqlConnection cn = new SqlConnection(this.ConnectionString))
        //    {
        //        var type = typeof(T);
        //        object oObject = Activator.CreateInstance(type);
        //        string sql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{ p.Name}]"))} " +
        //            $"FROM [{typeof(T).GetField("VisionName").GetValue(oObject)}]";

        //        try
        //        {

        //            IDataReader reader = GetIdataReader(sql, cn);
        //            var ret = new List<T>();

        //            while (null != reader && reader.Read())
        //            {                        
        //                foreach (var prop in type.GetProperties())
        //                {
        //                    prop.SetValue(oObject, reader[prop.Name]);
        //                }
        //                ret.Add((T)oObject);
        //                oObject = Activator.CreateInstance(type);
        //            }
        //            msg = "ok";
        //            return ret;
        //        }
        //        catch (Exception e)
        //        {
        //            e = Helpers.GetOriginalException(e);
        //            msg = e.Message;
        //            return null;
        //        }

        //    }
        //}
        internal override byte[] GetUserImage(int id)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = "select   Image from   S_UserInfoes where   id = '" + id + "'";//"insert   into   S_UserInfoes(Image,UnionID, Gender, RecordTime)   values   (@i,'Hi image', 1, '2019-1-1')";
                SqlCommand cmd = new SqlCommand(sql, cn);
                // cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                System.Drawing.Image image=null;
                byte[] b = null;
                while (reader.Read())
                {
                    b = (byte[])reader[0];
                    MemoryStream s = new MemoryStream(b);
                    //image = System.Drawing.Image.FromStream(s);
                    Bitmap bmp = new Bitmap(s);
                    image = bmp;

                    s.Close();
                }
                reader.Close();
                return b;
            }
        }
    }
    public partial class SqlSiteProvider : JinkeSiteProvider
    {
        
        public override int DeleteTableRecords(string tableName, string con)
        {
            throw new NotImplementedException();
        }

        internal override int ExecuteNonQuery(string sql)
        {
            throw new NotImplementedException();
        }
        public override int GetRecordsCount(string tableName, string con)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {

                string scon = con.ToLower().Replace("where", "").Trim();
                string sql = string.Format("select count(ID) AS RecordsNum from {0} where {1}", tableName, scon);
                if (string.IsNullOrEmpty(scon))
                {
                    sql = sql.Replace(" where ", "");
                }
                IDataReader reader = GetIdataReader(sql, cn);
                int ret = 0;
                while (null != reader && reader.Read())
                {
                    ret = (int)reader["RecordsNum"];
                }
                return ret;
            }
        }
        internal override bool InsertGoodsImage(int goodsid, byte[] image, out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = "update  B_Goods set Image=@i where GoodsID=" + goodsid; 
                SqlCommand cmd = new SqlCommand(sql, cn);
                // cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                cmd.Parameters.Add("@i", SqlDbType.Image, (int)image.Length);
                cmd.Parameters["@i"].Value = image;

                cn.Open();
                cmd.ExecuteNonQuery();
                msg = "ok";
                return true;
            }
        }
        
        internal override List<GoodsInfo> GetAllGoods(out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = string.Format("select *  from {0}", GoodsInfo.VisionName /* GoodsInfo.TableName*/);

                try
                {
                    IDataReader reader = GetIdataReader(sql, cn);
                    var ret = new List<GoodsInfo>();
                    while (null != reader && reader.Read())
                    {
                        ret.Add( GoodsInfo.GetItemFromReader(reader));
                    }
                    msg = "ok";
                    return ret;
                }
                catch (Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                    return null;
                }

            }
        }
        internal override GoodsInfo GetGoodsInfo(string goodsId, out string msg)
        {            
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = string.Format("select *  from {0} where GoodsID={1}", GoodsInfo.TableName, goodsId);

                try
                {
                    IDataReader reader = GetIdataReader(sql, cn);
                    GoodsInfo ret = null;
                    msg = "未找到记录。";
                    if (null != reader && reader.Read())
                    {
                        ret = GoodsInfo.GetItemFromReader(reader);
                        msg = "ok";
                    }
                    
                    return ret;
                } catch( Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                   return null;
                }
                
            }
        }
        internal override GoodsExpressInfo QueryExpress(string expressNo, string orderNo, out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = string.Format("select *  from {0} where OrderNo='{1}'", GoodsExpressInfo.TableName, orderNo);

                try
                {
                    IDataReader reader = GetIdataReader(sql, cn);
                    GoodsExpressInfo ret = null;
                    msg = "not find record.";
                    if (null != reader && reader.Read())
                    {
                        ret = GoodsExpressInfo.GetItemFromReader(reader);
                        msg = "ok";
                    }                    
                    return ret;
                }
                catch (Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                    return null;
                }

            }
        }
        public override string GetStorageName(int id)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = string.Format("select *  from {0} where ID={1}", StorageInfo.TableName, id);


                IDataReader reader = GetIdataReader(sql, cn);
                var ret = "";
                if (null != reader && reader.Read())
                {
                    ret = (string)reader["Name"];
                }
                return ret;
            }
        }
        internal override StockSub GetStock(int storageID, string goodsid)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                string sql = string.Format("select *  from {0} where StorageID={1} AND GoodsID={2}", StockSub.TableName, storageID, goodsid);


                IDataReader reader = GetIdataReader(sql, cn);
                StockSub ret = null;
                if (null != reader && reader.Read())
                {
                    ret = new StockSub();
                    ret.StorageID = (int)reader["StorageID"];
                    ret.Qty = Convert.ToInt32((decimal)reader["Qty"]);
                }
                return ret;
            }
        }

        /// <summary>
        /// 订单支付通知。金科通知海尔待发货的清单，包含订单号、 发货状态、收货人姓名、收货手机号、收货地址、商品名称、商品代码、商品规格、结算价，支付宝结算流水号，订单交易时间。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        internal override bool PayOrder(payOrderInfo item, out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_G_OrderToSellBill", cn);
                cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = item.orderNo; // item.alipayTradeNo + ", " + item.orderTradeTime.ToString();

                cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar).Value = item.orderStatus;

                cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = item.name;
                cmd.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = item.mobile;

                cmd.Parameters.Add("@receiveName", SqlDbType.VarChar).Value = item.receiveName;

                cmd.Parameters.Add("@receiveMobile", SqlDbType.VarChar).Value = item.receiveMobile;
                cmd.Parameters.Add("@ReceiveAddress", SqlDbType.VarChar).Value = item.receiveAddress;
                cmd.Parameters.Add("@GoodsID", SqlDbType.VarChar).Value = item.goodsId;
                cmd.Parameters.Add("@GoodsName", SqlDbType.VarChar).Value = item.goodsName;

                cmd.Parameters.Add("@GoodsSpecification", SqlDbType.VarChar).Value = item.goodsSpecification;
                DateTime ltime = Helpers.GetDateTime(item.orderTradeTime);
                cmd.Parameters.Add("@orderTradeTime", SqlDbType.DateTime).Value = ltime;

                cmd.Parameters.Add("@alipayTradeNo", SqlDbType.VarChar).Value = item.alipayTradeNo;
                cmd.Parameters.Add("@SettlePrice", SqlDbType.Money).Value = item.settlePrice;

                try
                {
                    cn.Open();
                    IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                    //cmd.ExecuteNonQuery();
                    //int ret = (int)cmd.Parameters["@ReturnID"].Value;

                    string billVrID = "";
                    if (null != reader && reader.Read())
                    {
                        billVrID = reader["BillVrID"].ToString();
                    }
                    msg = billVrID;
                    return !string.IsNullOrEmpty(billVrID);
                }catch(Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                    return false;
                }
                
            }
        }
        internal override bool OrderRefundInform(string orderNo, string status, decimal amount, out string msg)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_G_OrderToBackBill", cn);
                cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo; // item.alipayTradeNo + ", " + item.orderTradeTime.ToString();

                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                cmd.Parameters.Add("@Amount", SqlDbType.Money).Value = amount;
                msg = "ok";
                try
                {
                    cn.Open();
                    IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                    
                    if (null != reader && reader.Read())
                    {                                     
                        return true;
                        
                    }else
                    {
                        msg = "未查到记录！";
                        return false;
                    }
                    
                }
                catch (Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    msg = e.Message;
                    return false;
                }

            }
        }
        /// <summary>
        /// v退款申请
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="descp"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        internal override bool Apply4Refund(string orderNo, string descp, out Apply4RefundResult result)
        {
            using (SqlConnection cn = new SqlConnection(this.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_G_SellToBackBill", cn);
                cmd.CommandType = CommandType.StoredProcedure;   //执行存储过程
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = orderNo; // item.alipayTradeNo + ", " + item.orderTradeTime.ToString();

                cmd.Parameters.Add("@descp", SqlDbType.VarChar).Value = descp;

                result = new Apply4RefundResult();
                result.orderNo = orderNo;
                result.msg = "ok";
                try
                {
                    cn.Open();
                    IDataReader reader = ExecuteReader(cmd, CommandBehavior.SingleRow);
                    //cmd.ExecuteNonQuery();
                    //int ret = (int)cmd.Parameters["@ReturnID"].Value;
                    
                    if (null != reader && reader.Read())
                    {
                        result.status = reader["Status"].ToString();
                        result.amount = reader["Amount"].ToString();
                    }
                   
                    return !string.IsNullOrEmpty(result.status);
                }
                catch (Exception e)
                {
                    e = Helpers.GetOriginalException(e);
                    result.msg = e.Message;
                    return false;
                }

            }
        }
    }
}