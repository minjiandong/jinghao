using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_FUNCTION, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_FUNCTION
    {
        public JH_FUNCTION() { }

        #region 实体属性

        /// <summary>
        /// FUNCTIONID
        /// </summary>
        [Display(Name = "FUNCTIONID")]
        [Required(ErrorMessage="FUNCTIONID不能为空。")]
        [StringLength(32, ErrorMessage = "FUNCTIONID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string FUNCTIONID { get; set; }

        /// <summary>
        /// FUNCTIONNAME
        /// </summary>
        [Required]
        [Display(Name = "FUNCTIONNAME")]
        [StringLength(40, ErrorMessage = "FUNCTIONNAME必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string FUNCTIONNAME { get; set; }

        /// <summary>
        /// FUNCTIONTYPE
        /// </summary>
        [Required]
        [Display(Name = "FUNCTIONTYPE")]
        [StringLength(32, ErrorMessage = "FUNCTIONTYPE必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string FUNCTIONTYPE { get; set; }

        /// <summary>
        /// ISENABLE
        /// </summary>
        [Required]
        [Display(Name = "ISENABLE")]
        [StringLength(1, ErrorMessage = "ISENABLE必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string ISENABLE { get; set; }

        /// <summary>
        /// ISOPEN
        /// </summary>
        [Required]
        [Display(Name = "ISOPEN")]
        [StringLength(1, ErrorMessage = "ISOPEN必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string ISOPEN { get; set; }

        /// <summary>
        /// SUPERIORID
        /// </summary>
        [Required]
        [Display(Name = "SUPERIORID")]
        [StringLength(32, ErrorMessage = "SUPERIORID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string SUPERIORID { get; set; }

        /// <summary>
        /// SORT
        /// </summary>
        [Required]
        [Display(Name = "SORT")]
        [StringLength(10, ErrorMessage = "SORT必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string SORT { get; set; }

        /// <summary>
        /// REMARKS
        /// </summary>
        [Required]
        [Display(Name = "REMARKS")]
        [StringLength(200, ErrorMessage = "REMARKS必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string REMARKS { get; set; }

        /// <summary>
        /// CODE
        /// </summary>
        [Required]
        [Display(Name = "CODE")]
        [StringLength(200, ErrorMessage = "CODE必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string CODE { get; set; }

        /// <summary>
        /// MENUTYPE
        /// </summary>
        [Required]
        [Display(Name = "MENUTYPE")]
        [StringLength(20, ErrorMessage = "MENUTYPE必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string MENUTYPE { get; set; }

        /// <summary>
        /// ICON
        /// </summary>
        [Required]
        [Display(Name = "ICON")]
        [StringLength(50, ErrorMessage = "ICON必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string ICON { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_FUNCTION";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("FUNCTIONID", "FUNCTIONID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("FUNCTIONID", "FUNCTIONID");
            BaseField.Add("FUNCTIONNAME", "FUNCTIONNAME");
            BaseField.Add("FUNCTIONTYPE", "FUNCTIONTYPE");
            BaseField.Add("ISENABLE", "ISENABLE");
            BaseField.Add("ISOPEN", "ISOPEN");
            BaseField.Add("SUPERIORID", "SUPERIORID");
            BaseField.Add("SORT", "SORT");
            BaseField.Add("REMARKS", "REMARKS");
            BaseField.Add("CODE", "CODE");
            BaseField.Add("MENUTYPE", "MENUTYPE");
            BaseField.Add("ICON", "ICON");
            return BaseField;
        }
        #endregion

    }
}