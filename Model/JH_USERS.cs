using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_USERS, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_USERS
    {
        public JH_USERS() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        [Required]
        [Display(Name = "用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。")]
        [StringLength(5, ErrorMessage = "用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string subscribe { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [Required]
        [Display(Name = "用户的标识，对当前公众号唯一")]
        [StringLength(100, ErrorMessage = "用户的标识，对当前公众号唯一必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string openid { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        [Required]
        [Display(Name = "用户的昵称")]
        [StringLength(50, ErrorMessage = "用户的昵称必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string nickname { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [Required]
        [Display(Name = "用户的性别，值为1时是男性，值为2时是女性，值为0时是未知")]
        [StringLength(3, ErrorMessage = "用户的性别，值为1时是男性，值为2时是女性，值为0时是未知必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        [Required]
        [Display(Name = "用户所在城市")]
        [StringLength(30, ErrorMessage = "用户所在城市必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string city { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        [Required]
        [Display(Name = "用户所在国家")]
        [StringLength(50, ErrorMessage = "用户所在国家必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string country { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        [Required]
        [Display(Name = "用户所在省份")]
        [StringLength(50, ErrorMessage = "用户所在省份必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string province { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        [Required]
        [Display(Name = "用户的语言，简体中文为zh_CN")]
        [StringLength(30, ErrorMessage = "用户的语言，简体中文为zh_CN必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string language { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        [Required]
        [Display(Name = "用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。")]
        [StringLength(150, ErrorMessage = "用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        [Required]
        [Display(Name = "用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间")]
        [StringLength(40, ErrorMessage = "用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string subscribe_time { get; set; }

        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        [Required]
        [Display(Name = "用户所在的分组ID（兼容旧的用户分组接口）")]
        [StringLength(50, ErrorMessage = "用户所在的分组ID（兼容旧的用户分组接口）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string groupid { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal balance { get; set; }

        /// <summary>
        /// 识别码
        /// </summary>
        public string unionid { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_USERS";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("id", "id");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("subscribe", "subscribe");
            BaseField.Add("openid", "openid");
            BaseField.Add("nickname", "nickname");
            BaseField.Add("sex", "sex");
            BaseField.Add("city", "city");
            BaseField.Add("country", "country");
            BaseField.Add("province", "province");
            BaseField.Add("language", "language");
            BaseField.Add("headimgurl", "headimgurl");
            BaseField.Add("subscribe_time", "subscribe_time");
            BaseField.Add("groupid", "groupid");
            BaseField.Add("balance", "balance");
            BaseField.Add("unionid", "unionid");
            return BaseField;
        }
        #endregion

    }
}