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
    /// 客户端确认消息 
    /// </summary>
    public class Confirm
        : CommandBase<ChatSession, StringRequestInfo>
    {
        public override void ExecuteCommand(ChatSession session, StringRequestInfo requestInfo)
        {
            //需要传递一个参数，消息的Id号
            if (requestInfo.Parameters != null && requestInfo.Parameters.Length == 2)
            {                
                string modelId = requestInfo.Parameters[0];
                Console.WriteLine($"{session.Id}已确认收到消息{modelId}");
                ChatDataManager.Remove(session.Id, modelId);
            }
            else
            {
                session.Send("参数错误。Confirm命令需要一个ModelId参数");
            }
        }
    }
}
