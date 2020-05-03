using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MesoftMap.Models;
using Mesoft.MapAPP.Interface;
using Mesoft.Libraries.Shared;
using Microsoft.AspNetCore.Authorization;
using MesoftMap.Utility;

namespace MesoftMap.Controllers
{
    public class HomeController : Controller
    {
        const string _PassSalt = "MEsoft0622";
        private readonly ILogger<HomeController> _logger;
        private IMapService _MapService;
        public HomeController(ILogger<HomeController> logger, IMapService mapService)
        {
            _logger = logger;
            this._MapService = mapService;
        }
       [LoginCheckFilterAttribute]
        public IActionResult Index()
        {            
            return View();
        }
        /// <summary>
        /// 只允许朝夕软件的客户端调用，access_token设置为"MEsoft0622"的MD5加密值
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public IActionResult ClientMap(string access_token)
        {            var pass = MD5Helper.MD5Encrypt(_PassSalt);
            if (string.IsNullOrWhiteSpace(access_token))
            {
                return Json("您无权调用");
            }
            
            if (!pass.Equals(access_token))
            {
                return Json("您无权调用");                
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
