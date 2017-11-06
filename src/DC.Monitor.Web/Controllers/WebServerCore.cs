
using DC.Monitor.Web.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DC.Monitor.Controllers
{
    public class WebServerCore
    {
        private HttpClient _client = null;

        protected HttpClient client
        {
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                    _client.Timeout = TimeSpan.FromSeconds(1);
                }
                return _client;
            }
        }

        private async Task<TResult> SendAsync<TResult>(string url) where TResult : class
        {
            try
            {
                string response = await client.GetStringAsync(url);
                TResult result;
                if (typeof(TResult) == typeof(String))
                {
                    result = response as TResult;
                }
                else
                {
                    result = JsonConvert.DeserializeObject<TResult>(response);
                }
                return result;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public SystemInfoModel GetSystemInfo(HostModel domain)
        {
            var infoTask = SendAsync<SystemInfoModel>(domain.Host + "/WebSite/SystemInfo");
            infoTask.Wait();
            var model = infoTask.Result;
            if (model == null)
            {
                model = new SystemInfoModel();
            }
            var aliveTask = KeepAlive(domain.Host);
            aliveTask.Wait();
            var aliveResult = aliveTask.Result;
            model.ETime = aliveResult.etime;
            model.IsAlive = aliveResult.isAlive;
            model.HttpHost = domain.Host;
            model.HostName = domain.Name;
            return model;
        }

        public async Task<(bool isAlive, int etime)> KeepAlive(string domain)
        {
            var start = DateTime.Now;
            var res = await SendAsync<string>(domain + "/WebSite/KeepAlive");
            var end = DateTime.Now;
            if (res == "别担心,我还活着!")
            {
                return (true, (end - start).Milliseconds);
            }
            return (false, 0);
        }

    }

}