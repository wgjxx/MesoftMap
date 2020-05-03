using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Utility.Filter
{
    public class MapResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static Dictionary<string, object> _Cache = new Dictionary<string, object>();
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //在这里缓存数据
            string key = context.HttpContext.Request.Path.ToString();
            ObjectResult objResult = context.Result as ObjectResult;
            _Cache[key] = objResult;   //这里存入缓存
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //这里可以从缓存取数据，如果有数据直接返回
            string key = context.HttpContext.Request.Path.ToString();
            if (_Cache.ContainsKey(key))
            {
                context.Result = _Cache[key] as ObjectResult;    //短路器
            }
        }
    }
}
