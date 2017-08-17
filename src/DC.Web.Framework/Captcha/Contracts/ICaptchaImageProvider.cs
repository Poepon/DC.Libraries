using System;
using System.Collections.Generic;
using System.Text;

namespace CX.Web.Captcha.Contracts
{
    /// <summary>
    /// Captcha Image Provider
    /// </summary>
    public interface ICaptchaImageProvider
    {
        /// <summary>
        /// Creates the captcha image.
        /// </summary>
        byte[] DrawCaptcha(string message, string foreColor, string backColor, float fontSize, string fontName);
    }
}
