using System;
using DC.Libraries.Extensions.WeChat.Models;
using DC.Libraries.Extensions.WeChat.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin.Exceptions;

namespace DC.Libraries.Extensions.WeChat.Controllers
{
    public class WeChatController : Controller
    {
        private readonly WeChatSetting _config;

        public WeChatController(IOptions<WeChatSetting> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("WeChatLogin",Name = "WeChatLogin")]
        public IActionResult Login(string returnUrl)
        {

            var backUrl = $"{Request.Scheme}://{Request.Host.Host}/WeChatLoginCallback?returnUrl={returnUrl}";

            var authUrl = OAuthApi.GetAuthorizeUrl(
                appId: _config.WeixinAppId,
                redirectUrl: backUrl,
                state: _config.Token,
                scope: OAuthScope.snsapi_base);
            return Redirect(authUrl);
        }

        [Route("WeChatLoginCallback",Name = "WeChatLoginCallback")]
        public IActionResult LoginCallback()
        {
            string code = Request.Query["code"];
            string state = Request.Query["state"];
            string returnUrl = Request.Query["returnUrl"];
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    OAuthAccessTokenResult result =
                        OAuthApi.GetAccessToken(_config.WeixinAppId, _config.WeixinAppSecret, code);

                    HttpContext.Session.SetOAuthAccessToken(new OAuthAccessToken()
                    {
                        errcode = (int) result.errcode,
                        errmsg = result.errmsg,
                        access_token = result.access_token,
                        expires_in = result.expires_in,
                        openid = result.openid,
                        refresh_token = result.refresh_token,
                        scope = result.scope,
                        unionid = result.unionid
                    });
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            if (string.IsNullOrEmpty(_config.LoginCallbackUrl))
            {
                return Redirect(returnUrl);
            }
            var backUrl = $"{Request.Scheme}://{Request.Host.Host}{_config.LoginCallbackUrl}?returnUrl={returnUrl}";
            return Redirect(backUrl.ToString());
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect($"{Request.Scheme}://{Request.Host.Host}");
            }
        }

        #region 微信菜单

        [HttpPost]
        public ActionResult CreateMenu(string token, GetMenuResultFull resultFull, MenuMatchRule menuMatchRule)
        {
            var useAddCondidionalApi = menuMatchRule != null && !menuMatchRule.CheckAllNull();
            var apiName = string.Format("使用接口：{0}。", (useAddCondidionalApi ? "个性化菜单接口" : "普通自定义菜单接口"));
            try
            {
                //重新整理按钮信息
                WxJsonResult result = null;
                IButtonGroupBase buttonGroup = null;
                if (useAddCondidionalApi)
                {
                    //个性化接口
                    buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ConditionalButtonGroup()).menu;

                    var addConditionalButtonGroup = buttonGroup as ConditionalButtonGroup;
                    addConditionalButtonGroup.matchrule = menuMatchRule;
                    result = CommonApi.CreateMenuConditional(token, addConditionalButtonGroup);
                    apiName += string.Format("menuid：{0}。", (result as CreateMenuConditionalResult).menuid);
                }
                else
                {
                    //普通接口
                    buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                    result = CommonApi.CreateMenu(token, buttonGroup);
                }

                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = "菜单更新成功。" + apiName
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = string.Format("更新失败：{0}。{1}", ex.Message, apiName) };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        [HttpPost]
        public ActionResult CreateMenuFromJson(string token, string fullJson)
        {
            //TODO:根据"conditionalmenu"判断自定义菜单

            var apiName = "使用JSON更新";
            try
            {
                GetMenuResultFull resultFull = Newtonsoft.Json.JsonConvert.DeserializeObject<GetMenuResultFull>(fullJson);

                //重新整理按钮信息
                WxJsonResult result = null;
                IButtonGroupBase buttonGroup = null;

                buttonGroup = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
                result = CommonApi.CreateMenu(token, buttonGroup);

                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = "菜单更新成功。" + apiName
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = string.Format("更新失败：{0}。{1}", ex.Message, apiName) };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        public ActionResult GetMenu(string token)
        {
            try
            {
                var result = CommonApi.GetMenu(token);
                if (result == null)
                {
                    return Json(new { error = "菜单不存在或验证失败！" });
                }
                return Json(result);
            }
            catch (WeixinMenuException ex)
            {
                return Json(new { error = "菜单不存在或验证失败：" + ex.Message }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                return Json(new { error = "菜单不存在或验证失败：" + ex.Message }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        public ActionResult DeleteMenu(string token)
        {
            try
            {
                var result = CommonApi.DeleteMenu(token);
                var json = new
                {
                    Success = result.errmsg == "ok",
                    Message = result.errmsg
                };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
            catch (Exception ex)
            {
                var json = new { Success = false, Message = ex.Message };
                return Json(json, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }

        }


        #endregion

    }
}

