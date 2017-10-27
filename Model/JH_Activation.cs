using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_Activation, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_Activation
    {
        public JH_Activation() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 用户微信识别标志
        /// </summary>
        [Required]
        [Display(Name = "用户微信识别标志")]
        [StringLength(70, ErrorMessage = "用户微信识别标志必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string openid { get; set; }

        /// <summary>
        /// 景区ID编号
        /// </summary>
        [Required]
        [Display(Name = "景区ID编号")]
        public int Scenic_id { get; set; }

        /// <summary>
        /// 消费金额
        /// </summary>
        [Required]
        [Display(Name = "消费金额")]
        public decimal money { get; set; }

        /// <summary>
        /// 消费时间
        /// </summary>
        [Required]
        [Display(Name = "消费时间")]
        [StringLength(30, ErrorMessage = "消费时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string consumptionDate { get; set; }

        /// <summary>
        /// 支付方式（1微信支付0激活码支付，2余额支付）
        /// </summary>
        public string paymentType { get; set; }
        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_Activation";
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
            BaseField.Add("openid", "openid");
            BaseField.Add("Scenic_id", "Scenic_id");
            BaseField.Add("money", "money");
            BaseField.Add("consumptionDate", "consumptionDate");
            BaseField.Add("paymentType", "paymentType");
            return BaseField;
        }
        #endregion

    }
}