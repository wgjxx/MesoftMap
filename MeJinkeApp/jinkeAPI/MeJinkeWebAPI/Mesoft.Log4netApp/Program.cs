using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Log4netApp
{
    class Program
    {
        static void Main(string[] args)
        {
            

            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.cfg.xml");
            var fileInfo = new FileInfo(filepath);
            XmlConfigurator.Configure(fileInfo);
            ILog Log = LogManager.GetLogger("Mesoft");
            for(int i=0; i<10; i++)
            {
                Log.Error(new { Message = $"this is {i} error log to sql server." });
                Log.Debug(new { Message = $"this is {i} debug log to sql server." });
                Log.Warn(new { Message = $"this is {i} warn log to sql server." });
                Log.Fatal(new { Message = $"this is {i} fatal log to sql server." });
                Log.Info(new { Message = $"this is {i} info log to sql server." });
            }
            
            Console.WriteLine("log to sql server completed.");
            Console.Read();

        }
    }
}
