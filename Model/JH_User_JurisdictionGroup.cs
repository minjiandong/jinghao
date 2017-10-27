using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    /// <summary>
    /// 实体类 JH_User_JurisdictionGroup, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_User_JurisdictionGroup
    {
        public JH_User_JurisdictionGroup() { }

        #region 实体属性

        /// <summary>
        /// USERID
        /// </summary>
        [Required]
        [Display(Name = "USERID")]
        [StringLength(32, ErrorMessage = "USERID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string USERID { get; set; }

        /// <summary>
        /// JURISDICTIONGROUPID
        /// </summary>
        [Required]
        [Display(Name = "JURISDICTIONGROUPID")]
        [StringLength(32, ErrorMessage = "JURISDICTIONGROUPID必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JURISDICTIONGROUPID { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_User_JurisdictionGroup";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("USERID", "USERID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("JURISDICTIONGROUPID", "JURISDICTIONGROUPID");
            return BaseField;
        }
        #endregion

    }
}