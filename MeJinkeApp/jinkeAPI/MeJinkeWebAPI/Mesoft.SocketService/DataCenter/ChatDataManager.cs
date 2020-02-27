using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.DataCenter
{
    public class ChatDataManager
    {
        /// <summary>
        /// key是用户Id
        /// List是这个用户的全部消息
        /// </summary>
        private static Dictionary<string, List<ChatModel>> Dictionary = new Dictionary<string, List<ChatModel>>();
        public static void Add(string userId, ChatModel model)
        {
            if (Dictionary.ContainsKey(userId))
            {
                Dictionary[userId].Add(model);
            }
            else
            {
                Dictionary[userId] = new List<ChatModel>() { model };
            }
        }

        public static void Remove(string userId, string modelId)
        {
            if (Dictionary.ContainsKey(userId))
            {
                Dictionary[userId] = Dictionary[userId].Where(model => model.Id != modelId).ToList();
            }

        }
        /// <summary>
        /// 向用户userId发送别的用户向其发送的离线消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        public static void SendLogin(string userId, Action<ChatModel> action)
        {
            if (Dictionary.ContainsKey(userId))
            {
                foreach (var item in Dictionary[userId].Where(m => m.State == 0))
                {
                    action.Invoke(item);
                    item.State = 1;
                }
            }
        }
    }
}
