using MesoftMap.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Mesoft.Utility
{
    public static class CookieSessionHelper
    {
        public static void SetCookies(this HttpContext httpContext, string key, string value, int minutes = 30)
        {
            httpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }
        public static void DeleteCookies(this HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }

        public static string GetCookiesValue(this HttpContext httpContext, string key)
        {
            httpContext.Request.Cookies.TryGetValue(key, out string value);
            return value;
        }

        public static Operator GetCurrentUserBySession(this HttpContext context)
        {
            string sUser = context.Session.GetString("CurrentUser");
            if (sUser == null)
            {
                return null;
            }
            else
            {
                Operator currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<Operator>(sUser);
                return currentUser;
            }
        }
    }
}
