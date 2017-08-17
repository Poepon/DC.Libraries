using System.Linq;
using CX.Web.Captcha.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CX.Web.Captcha.Providers
{
    public class SessionCaptchaStorageProvider : ICaptchaStorageProvider
    {
        private readonly ICaptchaProtectionProvider _captchaProtectionProvider;
        private readonly ILogger<SessionCaptchaStorageProvider> _logger;
        public SessionCaptchaStorageProvider(ICaptchaProtectionProvider captchaProtectionProvider, ILogger<SessionCaptchaStorageProvider> logger)
        {
            _captchaProtectionProvider = captchaProtectionProvider;
            _logger = logger;
        }

        public void Add(HttpContext context, string token, string value)
        {
            context.Session.SetString(token,value);
        }

        public bool Contains(HttpContext context, string token)
        {
            return context.Session.Keys.Contains(token);
        }

        public string GetValue(HttpContext context, string token)
        {
            string cookieValue= context.Session.GetString(token);
            if (string.IsNullOrEmpty(cookieValue))
            {
                _logger.LogWarning("Couldn't find the captcha cookie in the request.");
                return null;
            }

            Remove(context, token);

            var decryptedValue = _captchaProtectionProvider.Decrypt(cookieValue);
            return decryptedValue;
        }

        public void Remove(HttpContext context, string token)
        {
            if (context.Session.Keys.Contains(token))
            {
                context.Session.Remove(token);
            }
        }
    }
}