using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_SYS_JurisdictionGroup, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_SYS_JurisdictionGroup
    {
        public JH_SYS_JurisdictionGroup() { }

        #region 实体属性

        /// <summary>
        /// 权限组ID编号
        /// </summary>
        [Display(Name = "权限组ID编号")]
        [Required(ErrorMessage="权限组ID编号不能为空。")]
        [StringLength(32, ErrorMessage = "权限组ID编号必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JurisdictionGroupID { get; set; }

        /// <summary>
        /// JurisdictionGroupName
        /// </summary>
        [Required]
        [Display(Name = "权限组名称")]
        [StringLength(40, ErrorMessage = "JurisdictionGroupName必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JurisdictionGroupName { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        [Required]
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "Remarks必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Remarks { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_SYS_JurisdictionGroup";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("JurisdictionGroupID", "JurisdictionGroupID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("JurisdictionGroupName", "JurisdictionGroupName");
            BaseField.Add("Remarks", "Remarks");
            return BaseField;
        }
        #endregion

    }
}