using System.Collections.Generic;

namespace DC.Libraries.Extensions.MultiThemes
{
    /// <summary>
    /// Theme
    /// </summary>
    public class ThemeItem
    {
        /// <summary>
        /// Theme名称
        /// </summary>
        public string ThemeName { get; set; }
        
        /// <summary>
        /// 支持手机
        /// </summary>
        public bool SupportMobile { get; set; }
        
        /// <summary>
        /// 支持PC
        /// </summary>
        public bool SupportPC { get; set; }

        /// <summary>
        /// 支持域名适配
        /// </summary>
        public bool SupportDomainAdapter { get; set; }

        /// <summary>
        /// 适配的域名，多个域名中间用逗号分隔。
        /// </summary>
        public string Domains { get; set; }

        /// <summary>
        /// 是否支持正则表达式
        /// </summary>
        public bool SupportRegex { get; set; }

        /// <summary>
        /// Theme目录相对路径。如：/Themes/Xpj/PC
        /// </summary>
        public string ThemeDirPath { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    public class ThemeConfiguration
    {
        public string DefaultTheme { get; set; }

        public IList<ThemeItem> Themes { get; set; } 
    }
}