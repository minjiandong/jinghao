using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    /// <summary>
    /// 实体类 JH_NEWS, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_NEWS
    {
        public JH_NEWS() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [Display(Name = "标题")]
        [StringLength(50, ErrorMessage = "标题必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Required]
        [Display(Name = "摘要")]
        [StringLength(300, ErrorMessage = "摘要必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_abstract { get; set; }

        /// <summary>
        /// 阅读数
        /// </summary>
        [Required]
        [Display(Name = "阅读数")]
        public int n_consult { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [Display(Name = "内容")]
        [StringLength(1073741823, ErrorMessage = "内容必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_content { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Required]
        [Display(Name = "发布时间")]
        [StringLength(30, ErrorMessage = "发布时间必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_ReleaseTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Required]
        [Display(Name = "n_updateTime")]
        [StringLength(30, ErrorMessage = "n_updateTime必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_updateTime { get; set; }

        /// <summary>
        /// 文章类型（1软文）
        /// </summary>
        [Required]
        [Display(Name = "文章类型（1软文2头条）")]
        [StringLength(5, ErrorMessage = "文章类型（1软文）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_type { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [Required]
        [Display(Name = "图片路径")]
        [StringLength(50, ErrorMessage = "图片路径必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string n_img { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_NEWS";
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
            BaseField.Add("n_title", "n_title");
            BaseField.Add("n_abstract", "n_abstract");
            BaseField.Add("n_consult", "n_consult");
            BaseField.Add("n_content", "n_content");
            BaseField.Add("n_ReleaseTime", "n_ReleaseTime");
            BaseField.Add("n_updateTime", "n_updateTime");
            BaseField.Add("n_type", "n_type");
            BaseField.Add("n_img", "n_img");
            return BaseField;
        }
        #endregion

    }
}