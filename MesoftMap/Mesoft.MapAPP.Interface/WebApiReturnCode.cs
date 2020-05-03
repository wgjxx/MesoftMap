using System;
using System.Collections.Generic;
using System.Text;

namespace Mesoft.Helpers.Interfaces
{
    public class WebApiReturnCode
    {
        public WebApiReturnCode() { }
        public WebApiReturnCode(int id, string msg)
        {
            CodeId = id;
            ErrMessage = msg;
        }
        public int CodeId { get; set; }
        public string ErrMessage { get; set; }

        public object Data { get; set; }
    }
}
