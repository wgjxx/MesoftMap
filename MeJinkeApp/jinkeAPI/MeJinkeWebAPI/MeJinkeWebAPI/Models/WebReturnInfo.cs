using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeJinkeWebAPI.Models
{
    /*
        返回信息样例：
    {   "code": "SUCCESS",   "msg": "请求成功！",   "subCode": "200",   "subMsg": "OK",   "data": "{\"x\": \"xx\"}",   "signature": "fr245t46" }
    */
    public class WebReturnInfo
    {
        public WebReturnInfo()
        {
            code = "SUCCESS";
            msg = "请求成功！";
            subCode = "200";
            subMsg = "OK";
            signature = "fr245t46";
        }
        public virtual string code { get; set; }
        public virtual string msg { get; set; }
        public virtual string subCode { get; set; }
        public virtual string subMsg { get; set; }
        public virtual object data { get; set; }
        public virtual string signature { get; set; }
    }



    /*
     * 请求信息样式
     * {   "seqNo":"JKZX22344556566677",   "appId":"JKZX",   "timestamp": "130000002222",   "signature": "fr245t46",   "v": "1.0",   "method":"pay.preOrder",   "data":"{\"x\": \"xx\"}" }
     */
    public class WebRequeryInfo
    {
        public virtual string seqNo { get; set; }//:"JKZX22344556566677",  
        public virtual string appId { get; set; }//:"JKZX",   
        public virtual string timestamp { get; set; }//: "130000002222",  
        public virtual string signature { get; set; }//: "fr245t46",   
        public virtual string v { get; set; }//: "1.0",   
        public virtual string method { get; set; }//:"pay.preOrder",   
        public virtual string data { get; set; }//:"{\"x\": \"xx\"}" }    
        
        public string GetPlaintext()
        {
            string plaintext = string.Format("appId={0}&data={1}&method={2}&seqNo={3}&timestamp={4}&v={5}",
                    this.appId, this.data, this.method, this.seqNo, this.timestamp, this.v);
            return plaintext;
        }
    }
}