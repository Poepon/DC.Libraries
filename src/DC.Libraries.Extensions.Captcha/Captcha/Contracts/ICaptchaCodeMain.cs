﻿namespace DC.Libraries.Extensions.Captcha.Contracts
{
    public interface ICaptchaCodeMain
    {
        byte[] GeneratorCaptcha(string name);

        bool VerifyCaptcha(string name, string value);

        void Remove(string name);
        
    }
}