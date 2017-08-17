using System.Collections.Generic;
using System.Linq;
using CX.Web.Captcha.Contracts;
using CX.Web.Captcha.Enums;

namespace CX.Web.Captcha.Providers
{
    /// <summary>
    /// Convert a number into words
    /// </summary>
    public class HumanReadableIntegerProvider : IHumanReadableIntegerProvider
    {
        private readonly IDictionary<Language, string> _and = new Dictionary<Language, string>
        {
            { Language.English, " " },
            { Language.Persian, " و " },
            {Language.Chinese, "" }
        };
        private readonly IList<NumberWord> _numberWords = new List<NumberWord>
        {
            new NumberWord { Group= DigitGroup.Ones, Language= Language.Chinese, Names=
                new List<string> { string.Empty, "一", "二", "三", "四", "五", "六", "七", "八", "九" }},
            new NumberWord { Group= DigitGroup.Ones, Language= Language.English, Names=
                new List<string> { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }},
            new NumberWord { Group= DigitGroup.Ones, Language= Language.Persian, Names=
                new List<string> { string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" }},

            new NumberWord { Group= DigitGroup.Teens, Language= Language.Chinese, Names=
                new List<string> { "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九" }},
            new NumberWord { Group= DigitGroup.Teens, Language= Language.English, Names=
                new List<string> { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" }},
            new NumberWord { Group= DigitGroup.Teens, Language= Language.Persian, Names=
                new List<string> { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" }},

            new NumberWord { Group= DigitGroup.Tens, Language= Language.Chinese, Names=
                new List<string> { "二十", "三十", "四十", "五十", "六十", "七十", "八十", "九十" }},
            new NumberWord { Group= DigitGroup.Tens, Language= Language.English, Names=
                new List<string> { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" }},
            new NumberWord { Group= DigitGroup.Tens, Language= Language.Persian, Names=
                new List<string> { "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" }},

            new NumberWord { Group= DigitGroup.Hundreds, Language= Language.Chinese, Names=
                new List<string> {string.Empty, "一百", "二百", "三百", "四百","五百", "六百", "七百", "八百", "九百" }},
            new NumberWord { Group= DigitGroup.Hundreds, Language= Language.English, Names=
                new List<string> {string.Empty, "One Hundred", "Two Hundred", "Three Hundred", "Four Hundred",
                    "Five Hundred", "Six Hundred", "Seven Hundred", "Eight Hundred", "Nine Hundred" }},
            new NumberWord { Group= DigitGroup.Hundreds, Language= Language.Persian, Names=
                new List<string> {string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد" , "نهصد" }},

            new NumberWord { Group= DigitGroup.Thousands, Language= Language.Chinese, Names=
                new List<string> { string.Empty, " 千", " 百万", " 十亿"," 兆" }},
            new NumberWord { Group= DigitGroup.Thousands, Language= Language.English, Names=
                new List<string> { string.Empty, " Thousand", " Million", " Billion"," Trillion"}},
            new NumberWord { Group= DigitGroup.Thousands, Language= Language.Persian, Names=
                new List<string> { string.Empty, " هزار", " میلیون", " میلیارد"," تریلیون" }},
        };
        private readonly IDictionary<Language, string> _negative = new Dictionary<Language, string>
        {
            { Language.Chinese, "负 " },
            { Language.English, "Negative " },
            { Language.Persian, "منهای " }
        };
        private readonly IDictionary<Language, string> _zero = new Dictionary<Language, string>
        {
            { Language.Chinese, "零" },
            { Language.English, "Zero" },
            { Language.Persian, "صفر" }
        };

        // Public Methods (5)

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public string NumberToText(int number, Language language)
        {
            return NumberToText((long)number, language);
        }
        
        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public string NumberToText(long number, Language language)
        {
            if (number == 0)
            {
                return _zero[language];
            }

            if (number < 0)
            {
                return _negative[language] + NumberToText(-number, language);
            }

            return Wordify(number, language, string.Empty, 0);
        }
        // Private Methods (2)

        private string GetName(int idx, Language language, DigitGroup group)
        {
            return _numberWords.First<NumberWord>(x => x.Group == group && x.Language == language).Names[idx];
        }

        private string Wordify(long number, Language language, string leftDigitsText, int thousands)
        {
            if (number == 0)
            {
                return leftDigitsText;
            }

            var wordValue = leftDigitsText;
            if (wordValue.Length > 0)
            {
                wordValue += _and[language];
            }

            if (number < 10)
            {
                wordValue += GetName((int)number, language, DigitGroup.Ones);
            }
            else if (number < 20)
            {
                wordValue += GetName((int)(number - 10), language, DigitGroup.Teens);
            }
            else if (number < 100)
            {
                wordValue += Wordify(number % 10, language, GetName((int)(number / 10 - 2), language, DigitGroup.Tens), 0);
            }
            else if (number < 1000)
            {
                wordValue += Wordify(number % 100, language, GetName((int)(number / 100), language, DigitGroup.Hundreds), 0);
            }
            else
            {
                wordValue += Wordify(number % 1000, language, Wordify(number / 1000, language, string.Empty, thousands + 1), 0);
            }

            if (number % 1000 == 0) return wordValue;
            return wordValue + GetName(thousands, language, DigitGroup.Thousands);
        }
    }
}