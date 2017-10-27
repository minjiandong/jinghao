using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    /// <summary>
    /// 实体类 JH_ActivationCard, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_ActivationCard
    {
        public JH_ActivationCard() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 激活码面值
        /// </summary>
        [Required]
        [Display(Name = "激活码面值")]
        public decimal faceValue { get; set; }

        /// <summary>
        /// 是否发布（1发布，2作废，0未发布）
        /// </summary>
        [Required]
        [Display(Name = "是否发布（1发布，2作废，0未发布）")]
        [StringLength(1, ErrorMessage = "是否发布（1发布，2作废，0未发布）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string isRelease { get; set; }

        /// <summary>
        /// 是否使用(1使用0未使用)
        /// </summary>
        [Required]
        [Display(Name = "是否使用(1使用0未使用)")]
        [StringLength(1, ErrorMessage = "是否使用(1使用0未使用)必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string isUse { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Required]
        [Display(Name = "发布时间")]
        [StringLength(30, ErrorMessage = "发布时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string ReleaseDate { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        [Required]
        [Display(Name = "使用时间")]
        [StringLength(30, ErrorMessage = "使用时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string UseDate { get; set; }

        /// <summary>
        /// 面值标题
        /// </summary>
        [Required]
        [Display(Name = "面值标题")]
        [StringLength(50, ErrorMessage = "面值标题必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Title { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [Required]
        [Display(Name = "所属用户")]
        public string Userid { get; set; }

        /// <summary>
        /// 激活代码
        /// </summary>
        [Required]
        [Display(Name = "激活代码")]
        [StringLength(10, ErrorMessage = "激活代码必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Code { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_ActivationCard";
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
            BaseField.Add("faceValue", "faceValue");
            BaseField.Add("isRelease", "isRelease");
            BaseField.Add("isUse", "isUse");
            BaseField.Add("ReleaseDate", "ReleaseDate");
            BaseField.Add("UseDate", "UseDate");
            BaseField.Add("Title", "Title");
            BaseField.Add("Userid", "Userid");
            BaseField.Add("Code", "Code");
            return BaseField;
        }
        #endregion

    }
}