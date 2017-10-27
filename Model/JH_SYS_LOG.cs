using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    /// <summary>
    /// 实体类 JH_SYS_LOG, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_SYS_LOG
    {
        public JH_SYS_LOG() { }

        #region 实体属性

        /// <summary>
        /// LOGID
        /// </summary>
        [Display(Name = "LOGID")]
        [Required(ErrorMessage="LOGID不能为空。")]
        [StringLength(32, ErrorMessage = "LOGID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string LOGID { get; set; }

        /// <summary>
        /// OPERATIONTIME
        /// </summary>
        [Required]
        [Display(Name = "OPERATIONTIME")]
        [StringLength(25, ErrorMessage = "OPERATIONTIME必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string OPERATIONTIME { get; set; }

        /// <summary>
        /// LOGIP
        /// </summary>
        [Required]
        [Display(Name = "LOGIP")]
        [StringLength(50, ErrorMessage = "LOGIP必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string LOGIP { get; set; }

        /// <summary>
        /// USERID
        /// </summary>
        [Required]
        [Display(Name = "USERID")]
        [StringLength(32, ErrorMessage = "USERID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string USERID { get; set; }

        /// <summary>
        /// LOGCONTENT
        /// </summary>
        [Required]
        [Display(Name = "LOGCONTENT")]
        [StringLength(1000, ErrorMessage = "LOGCONTENT必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string LOGCONTENT { get; set; }

        /// <summary>
        /// LOGTYPE
        /// </summary>
        [Required]
        [Display(Name = "LOGTYPE")]
        [StringLength(10, ErrorMessage = "LOGTYPE必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string LOGTYPE { get; set; }

        /// <summary>
        /// VIEWID
        /// </summary>
        [Required]
        [Display(Name = "VIEWID")]
        [StringLength(32, ErrorMessage = "VIEWID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string VIEWID { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_SYS_LOG";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("LOGID", "LOGID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("OPERATIONTIME", "OPERATIONTIME");
            BaseField.Add("LOGID", "LOGID");
            BaseField.Add("LOGIP", "LOGIP");
            BaseField.Add("USERID", "USERID");
            BaseField.Add("LOGCONTENT", "LOGCONTENT");
            BaseField.Add("LOGTYPE", "LOGTYPE");
            BaseField.Add("VIEWID", "VIEWID");
            return BaseField;
        }
        #endregion

    }
}