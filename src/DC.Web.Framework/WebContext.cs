using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CX.Web
{
    public class WebContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WebContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public int? UserId
        {
            get
            {

                return null;
            }
        }

    }
}