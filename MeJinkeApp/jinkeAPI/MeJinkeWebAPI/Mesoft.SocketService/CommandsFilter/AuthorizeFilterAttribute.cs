using Mesoft.SocketService.Session;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.CommandsFilter
{
    public class AuthorizeFilterAttribute : CommandFilterAttribute
    {
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            ChatSession session = (ChatSession)commandContext.Session;
            string command = commandContext.CurrentCommand.Name;
            if (!session.IsLogion)
            {
                if (!command.Equals("Check"))
                {
                    session.Send("请先登录，再操作");
                    commandContext.Cancel = true;   //
                }
                else  //正在登录
                {

                }
            }
            else if (!session.IsOnline)
            {                
                session.LastHBTime = DateTime.Now;
            }
                
        }
        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
            
        }

        
    }
}
