using MeJinkeWebAPI;
using MeJinkeWebAPI.BLL;
using MeJinkeWebAPI.Controllers;
using MeJinkeWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeSoftOA.DAL
{
    public abstract partial class JinkeSiteProvider : DataAccess
    {
        #region "Properties and Initial"
        static private JinkeSiteProvider _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        static public JinkeSiteProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (JinkeSiteProvider)Activator.CreateInstance(
                       Type.GetType(Globals.Settings.ProviderType));
                return _instance;
            }
        }

        

        public JinkeSiteProvider()
        {
            this.ConnectionString = Globals.Settings.ConnectionString;
            //this.OAConnectionString = Globals.Settings.OfficeAutomation.OAConnectionString;
            this.EnableCaching = Globals.Settings.EnableCaching;
            this.CacheDuration = Globals.Settings.CacheDuration;
        }

        #endregion
        internal abstract int ExecuteNonQuery(string sql);

        public abstract int DeleteTableRecords(string tableName, string con);
        /// <summary>
        /// 获取给定表符合条件的记录数，限制条件为给定的表必须有ID字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public abstract int GetRecordsCount(string tableName, string con);

        

        #region "for table B_Goods"
        internal abstract List<GoodsInfo> GetAllGoods(out string msg);
        internal abstract GoodsInfo GetGoodsInfo(string goodsId, out string msg);

        #endregion

        #region "for table B_Storage"
        public abstract string GetStorageName(int id);

        #endregion


        #region "for table G_StockSub"

        internal abstract StockSub GetStock(int storageID, string goodsid);

        #endregion





        internal List<int> GetIDs(string sTblName, string sIDName, string sCondition)
        {
            throw new NotImplementedException();
        }
        #region FOR TABLE G_SellMaster
        internal abstract bool PayOrder(payOrderInfo pdata, out string msg);
        #endregion
        /// <summary>
        /// 订单退款申请
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="descp"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        internal abstract bool Apply4Refund(string orderNo, string descp, out Apply4RefundResult result);

        internal abstract bool InsertGoodsImage(int goodsid, byte[] image, out string msg);

        internal abstract GoodsExpressInfo QueryExpress(string expressNo, string orderNo, out string msg);

        internal abstract bool OrderRefundInform(string orderNo, string status, decimal amount, out string msg);
    }


    public abstract partial class CSSiteProvider : DataAccess
    {
        #region "Properties and Initial"
        static private CSSiteProvider _instance = null;
        /// <summary>
        /// Returns an instance of the provider type specified in the config file
        /// </summary>
        static public CSSiteProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (CSSiteProvider)Activator.CreateInstance(
                       Type.GetType(Globals.Settings.CSProviderType));
                return _instance;
            }
        }


        public CSSiteProvider()
        {
            this.ConnectionString = Globals.Settings.CSConnectionString;
            //this.OAConnectionString = Globals.Settings.OfficeAutomation.OAConnectionString;
            this.EnableCaching = Globals.Settings.EnableCaching;
            this.CacheDuration = Globals.Settings.CacheDuration;
        }

        #endregion
        //public abstract new List<T> GetAllItems<T>(out string msg) where T : class;
        #region table "S_UserInfoes"
        internal abstract bool InsertUserImage(int id, byte[] image, out string msg);
        internal abstract byte[] GetUserImage(int id);
        #endregion
    }
}
