using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CX.Web.Captcha
{
    /// <summary>
    /// Tag helper attributes
    /// </summary>
    public class CaptchaTagHelperHtmlAttributes
    {
        /// <summary>
        /// The back-color of the captcha. It's default value is string.Empty.
        /// </summary>
        [HtmlAttributeName("asp-back-color")]
        public string BackColor { set; get; } = "";

        /// <summary>
        /// The font-name of the captcha. It's default value is Tahoma.
        /// </summary>
        [HtmlAttributeName("asp-font-name")]
        public string FontName { set; get; } = "Tahoma";

        /// <summary>
        /// The font-size of the captcha. It's default value is 12.
        /// </summary>
        [HtmlAttributeName("asp-font-size")]
        public float FontSize { set; get; } = 12;

        /// <summary>
        /// The fore-color of the captcha. It's default value is #1B0172.
        /// </summary>
        [HtmlAttributeName("asp-fore-color")]
        public string ForeColor { set; get; } = "#1B0172";

        [HtmlAttributeName("asp-captcha-name")]
        public string Name { set; get; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// The placeholder value of the captcha. It's default value is `کد امنیتی به رقم`.
        /// </summary>
        [HtmlAttributeName("asp-placeholder")]
        public string Placeholder { set; get; } = "请输入验证码";

        /// <summary>
        /// The css class of the captcha container. It's default value is ``.
        /// </summary>
        [HtmlAttributeName("asp-container-class")]
        public string ContainerClass { set; get; } = "";

        /// <summary>
        /// The css class of the captcha. It's default value is `text-box single-line form-control col-md-4`.
        /// </summary>
        [HtmlAttributeName("asp-text-box-class")]
        public string TextBoxClass { set; get; } = "text-box single-line form-control col-md-4";

        [HtmlAttributeName("asp-container-template")]
        public string ContainerTemplate { set; get; } = "{Textbox}{Image}{RefreshBtn}";

        /// <summary>
        /// The validation-error-message of the captcha. It's default value is `لطفا کد امنیتی را به رقم وارد نمائید`.
        /// </summary>
        [HtmlAttributeName("asp-validation-error-message")]
        public string ValidationErrorMessage { set; get; } = "请输入验证码";

        /// <summary>
        /// The validation-message-class of the captcha. It's default value is `text-danger`.
        /// </summary>
        [HtmlAttributeName("asp-validation-message-class")]
        public string ValidationMessageClass { set; get; } = "text-danger";

        /// <summary>
        /// The refresh-button-class of the captcha. It's default value is `glyphicon glyphicon-refresh btn-sm`.
        /// </summary>
        [HtmlAttributeName("asp-refresh-button-class")]
        public string RefreshButtonClass { set; get; } = "glyphicon glyphicon-refresh btn-sm";

        /// <summary>
        /// The Captcha Token
        /// </summary>
        [HtmlAttributeNotBound]
        public string CaptchaToken { set; get; }
    }
}