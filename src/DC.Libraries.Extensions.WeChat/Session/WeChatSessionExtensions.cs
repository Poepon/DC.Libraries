using System;
using Microsoft.AspNetCore.Http;
using DC.Libraries.Extensions.WeChat.Models;
using Newtonsoft.Json;

namespace DC.Libraries.Extensions.WeChat.Session
{
    public static class WeChatSessionExtensions
    {
        public static void SetOAuthAccessToken(this ISession session, OAuthAccessToken data)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            session.SetString(nameof(OAuthAccessToken), JsonConvert.SerializeObject(data));
        }

        public static OAuthAccessToken GetOAuthAccessToken(this ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }
            var str = session.GetString(nameof(OAuthAccessToken));
            return JsonConvert.DeserializeObject<OAuthAccessToken>(str);
        }
    }
}
