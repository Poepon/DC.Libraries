namespace DC.Libraries.Extensions.Captcha.Contracts
{
    public interface ICaptchaCodeMain
    {
        byte[] GeneratorCaptcha(string name, string foreColor, string backColor, float fontSize, string fontName);

        bool VerifyCaptcha(string name, string value);

        void Remove(string name);
        
    }
}