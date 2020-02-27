using Mesoft.SocketService.CommandsFilter;
using Mesoft.SocketService.Session;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.AppServer
{
    [AuthorizeFilterAttribute]   //表示需要全局登录后才能操作
    public class ChatServer : AppServer<ChatSession>
    {
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            Console.WriteLine("准备读取配置文件......");
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            Console.WriteLine("Socket服务启动......");
            base.OnStarted();
        }
        protected override void OnStopped()
        {
            Console.WriteLine("Socket服务停止......");
            base.OnStopped();
        }
        protected override void OnNewSessionConnected(ChatSession session)
        {
            Console.WriteLine($"Socket服务加入新的连接：{session.LocalEndPoint.Address.ToString()}");
            base.OnNewSessionConnected(session);
        }
    }
}
