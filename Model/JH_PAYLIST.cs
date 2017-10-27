using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_PAYLIST, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_PAYLIST
    {
        public JH_PAYLIST() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Required]
        [Display(Name = "支付金额")]
        public decimal money { get; set; }

        /// <summary>
        /// 用户唯一识别码
        /// </summary>
        [Required]
        [Display(Name = "用户唯一识别码")]
        [StringLength(70, ErrorMessage = "用户唯一识别码必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string openid { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [Required]
        [Display(Name = "支付时间")]
        [StringLength(30, ErrorMessage = "支付时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string PayDate { get; set; }

        /// <summary>
        /// 支付类型（1微信2激活码）
        /// </summary>
        [Required]
        [Display(Name = "支付类型（1微信2激活码）")]
        [StringLength(1, ErrorMessage = "支付类型（1微信2激活码）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string PayType { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_PAYLIST";
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
            BaseField.Add("money", "money");
            BaseField.Add("openid", "openid");
            BaseField.Add("PayDate", "PayDate");
            BaseField.Add("PayType", "PayType");
            return BaseField;
        }
        #endregion

    }
}