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
    public class Chat
        : CommandBase<ChatSession, StringRequestInfo>
    {
        public override void ExecuteCommand(ChatSession session, StringRequestInfo requestInfo)
        {
            //需要传递两个参数，1 消息要发给谁，  2 消息的内容
            if (requestInfo.Parameters != null && requestInfo.Parameters.Length == 2)
            {                
                string toId = requestInfo.Parameters[0];
                string message = requestInfo.Parameters[1];

                var toSession = session.AppServer.GetAllSessions().FirstOrDefault(a => a.Id == toId);
                string modelId = Guid.NewGuid().ToString();
                if (null != toSession)   //目标已存在，则直接发送信息
                {
                    toSession.Send($"{session.Id}给你发消息：{message} {modelId}");
                    ChatDataManager.Add(toId, new ChatModel()
                    {
                        FromId=session.Id,
                        ToId=toId,
                        Message=message,
                        Id=modelId,
                        State=1,   //已发送，待确认
                        CreateTime=DateTime.Now
                    });
                }
                else   //目标不在线，则暂时保存起来
                {
                    ChatDataManager.Add(toId, new ChatModel()
                    {
                        FromId = session.Id,
                        ToId = toId,
                        Message = message,
                        Id = modelId,
                        State = 0,   //未发送，离线存储起来，待该用户上线时再发送
                        CreateTime = DateTime.Now
                    });
                    session.Send("消息未发送成功");
                }

            }
            else
            {
                session.Send("参数错误");
            }
        }
    }
}
