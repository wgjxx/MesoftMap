using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始启动服务器......");
                SuperSocketMain.Init();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"启动失败{ex.Message}......");
            }
            Console.Read();
        }
    }
}
