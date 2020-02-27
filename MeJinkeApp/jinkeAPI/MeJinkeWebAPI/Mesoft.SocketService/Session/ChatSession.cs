using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mesoft.SocketService.Extends;
using SuperSocket.SocketBase.Protocol;

namespace Mesoft.SocketService.Session
{
    /// <summary>
    /// 表示用户连接
    /// 
    /// </summary>
    public class ChatSession : AppSession<ChatSession>
    {
        public string Id { get; set; }
        public string PassWord { get; set; }
        public bool IsLogion { get; set; }
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime LastHBTime { get; set; }

        public bool IsOnline { get
            {
                return this.LastHBTime.AddSeconds(10) > DateTime.Now;   //10秒钟之内有心跳表示在线
            }
        }

        protected override void OnInit()
        {
            this.Charset = Encoding.GetEncoding("gb2312");
            base.OnInit();
        }
        public override void Send(string message)
        {
            Console.WriteLine($"准备发送给{Id}:{message}");
            base.Send(message.Format());
        }
        protected override void OnSessionStarted()
        {
            this.Send("Welcome to Mesoft Socket Chat Server");
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            Console.WriteLine($"收到命令：{requestInfo.Key.ToString()}");
            this.Send($"不知道如何处理{requestInfo.Key.ToString()}命令");
        }
        protected override void HandleException(Exception e)
        {
            this.Send($"\n\r异常信息：{e.Message}");
        }
        /// <summary>
        /// 连接关闭
        /// </summary>
        /// <param name="reason"></param>
        protected override void OnSessionClosed(CloseReason reason)
        {
            Console.WriteLine("连接已关闭");
            base.OnSessionClosed(reason);
        }
    }
}
