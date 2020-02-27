using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.DataCenter
{
    /// <summary>
    /// 一条消息的记录
    /// </summary>
    public class ChatModel
    {
        /// <summary>
        /// 每条消息分配一个唯一Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// 目标编号
        /// </summary>
        public string ToId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消息状态。 0 未发送， 1 已发待确认， 2 确认收到
        /// </summary>
        public int State { get; set; }
    }
}
