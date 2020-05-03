using Mesoft.Utility;
using MesoftMap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Utility
{
    public class LoginCheckFilterAttribute : ActionFilterAttribute
    {
        #region Identity
        private readonly ILogger<LoginCheckFilterAttribute> _logger;

        //public LoginCheckFilterAttribute(ILogger<LoginCheckFilterAttribute> logger)
        //{
        //    this._logger = logger;
        //}
        #endregion
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Operator currentUser = context.HttpContext.GetCurrentUserBySession();
            if (currentUser == null)
            {
                //if (this.IsAjaxRequest(context.HttpContext.Request))
                //{ }
                context.Result = new RedirectResult("~/Home/Login");
            }
            //else
            //{
            //    this._logger.LogDebug($"{currentUser.Name} 访问系统");
            //}
        }
    }
}
