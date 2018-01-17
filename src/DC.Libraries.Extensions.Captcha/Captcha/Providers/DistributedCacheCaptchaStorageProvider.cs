using DC.Libraries.Extensions.Captcha.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DC.Libraries.Extensions.Captcha.Providers
{
    public class DistributedCacheCaptchaStorageProvider : ICaptchaStorageProvider
    {

        private readonly ILogger<DistributedCacheCaptchaStorageProvider> _logger;
        private readonly IDistributedCache _cache;

        public DistributedCacheCaptchaStorageProvider(ILogger<DistributedCacheCaptchaStorageProvider> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public void Add(HttpContext context, string token, string value)
        {
            _cache.SetString(context.Session.Id + "-" + token, value);
        }

        public bool Contains(HttpContext context, string token)
        {
            var value = _cache.GetString(context.Session.Id + "-" + token);
            return !string.IsNullOrEmpty(value);
        }

        public string GetValue(HttpContext context, string token)
        {
            var value = _cache.GetString(context.Session.Id + "-" + token);
            if (string.IsNullOrEmpty(value))
            {
                _logger.LogWarning("Couldn't find the captcha value in the request.");
                return null;
            }

            return value;
        }

        public void Remove(HttpContext context, string token)
        {
            if (Contains(context, token))
            {
                _cache.Remove(context.Session.Id + "-" + token);
            }
        }
    }
}