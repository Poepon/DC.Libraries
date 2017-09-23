using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace CX.Web.Monitor
{ 
    public class WebSiteController: Controller
    {
        public SystemInfoModel SystemInfo()
        {
            var app = PlatformServices.Default.Application;
               var model = new SystemInfoModel();
            model.AppVersion = app.ApplicationVersion;
            try
            {
                model.MachineName = Environment.MachineName;
                model.OperatingSystem = RuntimeInformation.OSDescription;
            }
            catch (Exception) { }
            try
            {
                model.AspNetInfo = app.RuntimeFramework.FullName;// RuntimeInformation.FrameworkDescription;
            }
            catch (Exception) { }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception) { }
            model.AppName = app.ApplicationName;
            model.ServerTimeZone = TimeZoneInfo.Local.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.HttpHost = HttpContext.Request.Host.Host;
            model.IPAddress = HttpContext.Connection.LocalIpAddress.ToString();
            model.IPAddress = model.IPAddress.TrimEnd(",".ToCharArray());
            DirectoryInfo info = new DirectoryInfo(app.ApplicationBasePath);
            var files = info.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
            model.ReleaseDate = files.Max(f => f.LastWriteTime);

            return model;
        }

        public IActionResult KeepAlive()
        {
            return Content("别担心,我还活着!");
        }

        public const string SystemInfoApiUrl = "/WebSite/SystemInfo";

        public const string KeepAliveApiUrl = "/WebSite/KeepAlive";
    }
}