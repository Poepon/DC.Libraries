using DC.Libraries.Extensions.Captcha.Enums;

namespace DC.Libraries.Extensions.Captcha.Contracts
{
    /// <summary>
    /// Convert a number into words
    /// </summary>
    public interface IHumanReadableIntegerProvider
    {
        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        string NumberToText(int number, Language language);

    }
}