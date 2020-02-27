using Mesoft.SocketService.DataCenter;
using Mesoft.SocketService.Session;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.Commands
{
    /// <summary>
    /// 心跳检测,检测客户端是否掉线
    /// 客户端在线时，间隔一定时间向服务器发送一条消息（用此命令），服务器马上回复一条消息，如果间隔时间收不到服务器发送的回复消息则认为断线，需要断线重连
    /// 断线重连机制：如果客户端发现自己掉线，马上（或过几秒钟以后）重新连接
    /// </summary>
    public class HeartBit : CommandBase<ChatSession, StringRequestInfo>
    {
        public override void ExecuteCommand(ChatSession session, StringRequestInfo requestInfo)
        {
            //一个参数
            if (requestInfo.Parameters != null && requestInfo.Parameters.Length == 1)
            {
                
                var recParameter = requestInfo.Parameters[0];
                if ("HB".Equals(recParameter))
                {
                    session.LastHBTime = DateTime.Now;
                    session.Send("OK");
                }
                else
                {
                    session.Send("参数错误");
                }
            }
            else
            {
                session.Send("参数错误");
            }
        }
    }
}
