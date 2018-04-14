using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using DC.Libraries.Extensions.WeChat.Runtime;
using Microsoft.AspNetCore.Authentication.Cookies;
using DC.Libraries.Extensions.Captcha;
using DC.Libraries.Extensions.WeChat.Attributes;
using Microsoft.AspNetCore.Hosting;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationLifetime _applicationLifetime;

        public HomeController(IApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ValidateCaptcha]
        public IActionResult TestCaptcha(bool captchaValid)
        {
            return Content(captchaValid.ToString());
        }

        public async Task<IActionResult> WeChat(string returnUrl)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "test")
                        };
          
            var claimsIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal);
            return Redirect(returnUrl);
        }

        [WeChatAutoLogin]
        public IActionResult AutoLogin()
        {
            return Content("登录成功");
        }

        public IActionResult ShowOpenId()
        {
            return Content(HttpContext.Session.GetOAuthAccessToken().openid);
        }

        public IActionResult Restart()
        {
            _applicationLifetime.StopApplication();
            return Redirect("/");
        }
    }
}
