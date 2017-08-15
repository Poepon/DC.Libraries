using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CX.Web.Captcha
{
    [HtmlTargetElement("captcha")]
    public class CaptchaTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContext;

        [HtmlAttributeName("length")]
        public int Length { get; set; } = 4;

        [HtmlAttributeName("error-message")]
        public string ErrorMessage { get; set; }

        [HtmlAttributeName("placeholder")]
        public string PlaceHolder { get; set; }

        public CaptchaTagHelper(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            
            output.TagName = "label";
            output.Attributes.Add("id", context.UniqueId);
            var code = CodeGenerator.GenCode(Length);
            _httpContext.HttpContext.Session.SetString(context.UniqueId, code);
            output.PreContent.SetContent(code);
        }
    }
}