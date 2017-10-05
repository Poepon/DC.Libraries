using Senparc.Weixin.Entities;

namespace DC.Libraries.Extensions.WeChat.Models
{
    /// <summary>
    /// ΢������
    /// </summary>
    public class WeChatSetting : SenparcWeixinSetting
    {
        /// <summary>
        /// �ص���ַ
        /// </summary>
        public string LoginCallbackUrl { get; set; }
    }
}