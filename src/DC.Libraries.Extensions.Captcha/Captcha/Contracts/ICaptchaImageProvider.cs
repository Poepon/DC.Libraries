﻿namespace DC.Libraries.Extensions.Captcha.Contracts
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