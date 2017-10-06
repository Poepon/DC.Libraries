using DC.Libraries.Extensions.WeChat.Runtime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DC.Libraries.Extensions.WeChat.Attritues
{
    public class WeChatAutoLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isWeiXinBrowser = context.HttpContext.Request.IsWeiXinBrowser();
            if (isWeiXinBrowser)
            {
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
