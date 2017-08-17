using System;
using CX.Web.Captcha.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace CX.Web.Captcha
{
    /// <summary>
    /// Represents a filter attribute enabling CAPTCHA validation
    /// </summary>
    public class ValidateCaptchaAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Create instance of the filter attribute 
        /// </summary>
        /// <param name="actionParameterName">The name of the action parameter to which the result will be passed</param>
        public ValidateCaptchaAttribute(string actionParameterName = "captchaValid") : base(
            typeof(ValidateCaptchaFilter))
        {
            this.Arguments = new object[] { actionParameterName };
        }
        #region Nested filter

        /// <summary>
        /// Represents a filter enabling CAPTCHA validation
        /// </summary>
        private class ValidateCaptchaFilter : IActionFilter
        {

            #region Fields

            private readonly string _actionParameterName;

            #endregion

            #region Ctor

            public ValidateCaptchaFilter(string actionParameterName)
            {
                this._actionParameterName = actionParameterName;
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Validate CAPTCHA
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <returns>True if CAPTCHA is valid; otherwise false</returns>
            protected bool ValidateCaptcha(ActionExecutingContext context)
            {
                var isValid = false;

                var form = context.HttpContext.Request.Form;
                var captchaName = (string)form[CaptchaTagHelper.CaptchaHiddenTokenName];
                var inputText = (string)form[CaptchaTagHelper.CaptchaInputName];
                if (!StringValues.IsNullOrEmpty(captchaName) && !StringValues.IsNullOrEmpty(inputText))
                {

                    //validate request

                    var captchaText = context.HttpContext.Session.GetString(captchaName);

                    if (inputText.Equals(captchaText, StringComparison.OrdinalIgnoreCase))
                    {
                        isValid = true;
                    }
                }

                return isValid;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called before the action executes, after model binding is complete
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context == null)
                    return;

                //whether CAPTCHA is enabled
                if (context.HttpContext?.Request != null && string.Equals("POST", context.HttpContext.Request.Method, StringComparison.OrdinalIgnoreCase))
                {
                    //push the validation result as an action parameter
                    context.ActionArguments[_actionParameterName] = ValidateCaptcha(context);
                }
                else
                    context.ActionArguments[_actionParameterName] = false;

            }

            /// <summary>
            /// Called after the action executes, before the action result
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// Validate Captcha Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    class ValidateCaptcha2Attribute : ActionFilterAttribute
    {
        /// <summary>
        /// The language of captcha generator. It's default value is Persian.
        /// </summary>
        public Language CaptchaGeneratorLanguage { set; get; } = Language.Chinese;


        /// <summary>
        /// Captcha validator.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            var form = filterContext.HttpContext.Request.Form;

            var captchaName = (string)form[CaptchaTagHelper.CaptchaHiddenTokenName];
            var inputText = (string)form[CaptchaTagHelper.CaptchaInputName];
            var captchaText = filterContext.HttpContext.Session.GetString(captchaName);


            if (string.IsNullOrEmpty(inputText) || !inputText.Equals(captchaText))
            {

            }

            base.OnActionExecuting(filterContext);
        }

    }
}
