using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mesoft.Helpers.Interfaces;
using Mesoft.MapAPP.Interface;
using Mesoft.Utility;
using MesoftMap.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MesoftMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly ILogger<MapController> _logger;
        IOperatorService _service;
        public OperatorController(ILogger<MapController> logger, IOperatorService service)
        {
            this._logger = logger;
            this._service = service;
        }

        [HttpGet("[action]")]
        public WebApiReturnCode Register(string id, string name, string password, int departmentID=0, int groupID=0)
        {
            var item = this._service.GetUser(id);
            if (null != item)
            {
                return new WebApiReturnCode { CodeId =  -1, ErrMessage = "用户名已存在", Data = null };
            }
            var user = this._service.AddOperator(id, name, password, departmentID, groupID, out string msg);
            onLogin(user);
            return new WebApiReturnCode { CodeId = user != null ? 1 : -1, ErrMessage = msg, Data = null }; 
        }

        [HttpGet("[action]")]
        public WebApiReturnCode Login(string id, string pass)
        {
            var ret = this._service.IsUserValid(id, pass, out string msg);
            if (ret) //登录成功
            {
                Operator currentUser = _service.GetUser(id);
                onLogin(currentUser);
            }

            return new WebApiReturnCode { CodeId = ret ? 1 : -1, ErrMessage = msg, Data = null };
        }

        private void onLogin(Operator user)
        {
            #region Cookie/Session 自己写
            //base.HttpContext.SetCookies("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(currentUser), 30);
            base.HttpContext.Session.SetString("CurrentUser", Newtonsoft.Json.JsonConvert.SerializeObject(user));
            #endregion
            #region MyRegion
            List<OperatorRight> rights = _service.GetUserRights(user);
            string IsDelete = rights.Count > 0 ? rights[0].IsDelete.ToString() : "false";
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim("IsDelete",IsDelete),//可以写入任意数据
                        new Claim("Account","Administrator"),
                        //new Claim("Role", currentUser.Memo)   //此处需要扩展
                    };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
            }).Wait();//没用await
                      //cookie策略--用户信息---过期时间
            #endregion
        }

        [HttpGet("[action]")]
        public WebApiReturnCode Logout()
        {
            #region Cookie
            base.HttpContext.Response.Cookies.Delete("CurrentUser");
            #endregion

            #region Session
            Operator sessionUser = base.HttpContext.GetCurrentUserBySession();
            if (sessionUser != null)
            {
                Console.WriteLine(string.Format("用户id={0} Name={1}退出系统", sessionUser.ID, sessionUser.Name));
            }
            base.HttpContext.Session.Remove("CurrentUser");
            base.HttpContext.Session.Clear();
            #endregion Session
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            
            return new WebApiReturnCode { CodeId = 1, ErrMessage = "ok", Data = null };
        }
    }
}