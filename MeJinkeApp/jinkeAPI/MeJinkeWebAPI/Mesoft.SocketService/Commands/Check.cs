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
    public class Check : CommandBase<ChatSession, StringRequestInfo>
    {
        public override void ExecuteCommand(ChatSession session, StringRequestInfo requestInfo)
        {
            //
            if (requestInfo.Parameters != null && requestInfo.Parameters.Length == 2)
            {
                //这里可以增加验证登录的用户数据是否正确合法等的逻辑
                var loginedSession = session.AppServer.GetAllSessions().FirstOrDefault(a => a.Id == requestInfo.Parameters[0]);
                if (null != loginedSession)
                {
                    loginedSession.Send("您的账号已在其他地方登录，此处的您将被踢下线");
                    loginedSession.Close();
                }

                session.Id = requestInfo.Parameters[0];
                session.PassWord = requestInfo.Parameters[1];
                session.IsLogion = true;
                session.LoginTime = DateTime.Now;
                session.Send("老弟你来了,已经登录成功。");
                //获取当前登录用户的离线消息并发送
                ChatDataManager.SendLogin(session.Id, c =>
                {
                    session.Send($"{c.FromId}给你发送消息：{c.Message} {c.Id}；发送时间{c.CreateTime.ToString("yyyy-mm-dd hh:mm:ss")}");
                    
                });
            }
            else
            {
                session.Send("参数错误");
            }
        }
    }
}
