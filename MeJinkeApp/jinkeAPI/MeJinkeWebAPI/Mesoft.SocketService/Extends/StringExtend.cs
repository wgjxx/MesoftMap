using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.SocketService.Extends
{
    public static class StringExtend
    {
        public static string Format(this string s)
        {
            return s + "\r\n";
        }
    }
}
