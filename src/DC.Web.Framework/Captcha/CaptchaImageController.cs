using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CX.Web.Captcha.Contracts;
using CX.Web.Captcha.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace CX.Web.Captcha
{
    /// <summary>
    /// Captcha Image Controller
    /// </summary>
    [AllowAnonymous]
    public class CaptchaImageController : Controller
    {
        private readonly ICaptchaImageProvider _captchaImageProvider;
        private readonly ICaptchaProtectionProvider _captchaProtectionProvider;
        private readonly ICaptchaStorageProvider _captchaStorageProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger<CaptchaImageController> _logger;
        private readonly ICaptchaCodeGenerator _captchaCodeGenerator;
        /// <summary>
        /// Captcha Image Controller
        /// </summary>
        public CaptchaImageController(
            ICaptchaImageProvider captchaImageProvider,
            ICaptchaProtectionProvider captchaProtectionProvider,
            ITempDataProvider tempDataProvider,
            ICaptchaStorageProvider captchaStorageProvider,
            ILogger<CaptchaImageController> logger, ICaptchaCodeGenerator captchaCodeGenerator)
        {

            _captchaImageProvider = captchaImageProvider;
            _captchaProtectionProvider = captchaProtectionProvider;
            _tempDataProvider = tempDataProvider;
            _captchaStorageProvider = captchaStorageProvider;
            _logger = logger;
            _captchaCodeGenerator = captchaCodeGenerator;
        }

        /// <summary>
        /// The ViewContext Provider
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Refresh the captcha
        /// </summary>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult Refresh(string rndDate, CaptchaTagHelperHtmlAttributes model)
        {
            if (!IsAjaxRequest())
            {
                return BadRequest();
            }

            if (IsImageHotlinking())
            {
                return BadRequest();
            }

            _captchaStorageProvider.Remove(HttpContext, model.CaptchaToken);

            var tagHelper = HttpContext.RequestServices.GetRequiredService<CaptchaTagHelper>();
            tagHelper.BackColor = model.BackColor;
            tagHelper.FontName = model.FontName;
            tagHelper.FontSize = model.FontSize;
            tagHelper.ForeColor = model.ForeColor;
            tagHelper.Language = model.Language;
            tagHelper.Max = model.Max;
            tagHelper.Min = model.Min;
            tagHelper.Placeholder = model.Placeholder;
            tagHelper.TextBoxClass = model.TextBoxClass;
            tagHelper.TextBoxTemplate = model.TextBoxTemplate;
            tagHelper.ValidationErrorMessage = model.ValidationErrorMessage;
            tagHelper.ValidationMessageClass = model.ValidationMessageClass;
            tagHelper.RefreshButtonClass = model.RefreshButtonClass;

            var tagHelperContext = new TagHelperContext(
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput(
                tagName: "div",
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            tagHelper.ViewContext = ViewContext ?? new ViewContext(
                                        new ActionContext(this.HttpContext, HttpContext.GetRouteData(), ControllerContext.ActionDescriptor),
                                        new FakeView(),
                                        new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                                        {
                                            Model = null
                                        },
                                        new TempDataDictionary(this.HttpContext, _tempDataProvider),
                                        TextWriter.Null,
                                        new HtmlHelperOptions());

            tagHelper.Process(tagHelperContext, tagHelperOutput);

            var attrs = new StringBuilder();
            foreach (var attr in tagHelperOutput.Attributes)
            {
                attrs.Append($" {attr.Name}='{attr.Value}'");
            }

            var content = $"<div {attrs}>{tagHelperOutput.Content.GetContent()}</div>";
            return Content(content);
        }

        /// <summary>
        /// Creates the captcha image.
        /// </summary>
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult Show(string name, string rndDate, string foreColor = "#1B0172",
            string backColor = "", float fontSize = 12, string fontName = "Tahoma")
        {
            if (IsImageHotlinking())
            {
                return BadRequest();
            }
            var text = _captchaCodeGenerator.OutputText(true, true, true, 4);
            _captchaStorageProvider.Add(HttpContext, name, text);
            byte[] image;
            try
            {
                image = _captchaImageProvider.DrawCaptcha(text, foreColor, backColor, fontSize, fontName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(1001, ex, "DrawCaptcha error.");
                return BadRequest(ex.Message);
            }
            return new FileContentResult(image, "image/png");
        }

        private bool IsAjaxRequest()
        {
            return Request?.Headers != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private bool IsImageHotlinking()
        {
            var applicationUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var urlReferrer = (string)Request.Headers[HeaderNames.Referer];
            return string.IsNullOrEmpty(urlReferrer) ||
                   !urlReferrer.StartsWith(applicationUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}