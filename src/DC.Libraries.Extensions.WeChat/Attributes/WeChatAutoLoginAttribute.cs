using System;
using DC.Libraries.Extensions.WeChat.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace DC.Libraries.Extensions.WeChat.Attritues
{
    public class WeChatAutoLoginAttribute : ActionFilterAttribute
    {
        public WeChatAutoLoginAttribute()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userAgent = context.HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();
            bool isWeiXinBrowser = userAgent != null &&
                                userAgent.Contains("MicroMessenger");
            if (isWeiXinBrowser)
            {
                int idx = userAgent.IndexOf("MicroMessenger", StringComparison.Ordinal);
                string vs = userAgent.Substring(idx + "MicroMessenger".Length + 1, 3);
                var version = Convert.ToDecimal(vs);
                if (context.HttpContext.Session.GetOAuthAccessToken() == null)
                {
                    context.Result = new RedirectToRouteResult("WeChatLogin",
                        new { returnUrl = context.HttpContext.Request.Path.ToString() });
                }
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }

    }
}
