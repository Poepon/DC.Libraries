using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DC.Monitor.Models;
using Microsoft.Extensions.Configuration;
using DC.Monitor.Web.Models;

namespace DC.Monitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var domains = _configuration.GetSection("Domains").Get<List<HostModel>>();
            WebServerCore webServer = new WebServerCore();
            ConcurrentBag<SystemInfoModel> bag = new ConcurrentBag<SystemInfoModel>();
            Parallel.ForEach(domains, (s => bag.Add(webServer.GetSystemInfo(s))));

            return View(bag.ToList());
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
    }
}
