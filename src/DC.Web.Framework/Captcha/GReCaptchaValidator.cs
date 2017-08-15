using System;

namespace CX.Web.Captcha
{
    public class FromName
    {
        public const string FieldId = "";
    }
    public class GReCaptchaValidator
    {
        public string SecretKey { get; set; }
        public string RemoteIp { get; set; }
        public string Response { get; set; }
        public string Challenge { get; set; }

     
        public bool Validate()
        {

            return true;
        }

    }
}