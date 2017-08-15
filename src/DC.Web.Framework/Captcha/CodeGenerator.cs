using System;

namespace CX.Web.Captcha
{
    public class CodeGenerator
    {
        private const string Str = "QWERTYUIOPASDFGHJKLZXCVBNM0123456789";

        public static string GenCode(int len)
        {
            char[] chastr = Str.ToCharArray();
            Random rd = new Random();
            string code = "";
            for (var i = 0; i < len; i++)
            {
                code += chastr[rd.Next(0, Str.Length)];
            }
            return code;
        }
    }
}