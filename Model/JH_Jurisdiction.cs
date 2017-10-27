using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_Jurisdiction, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_Jurisdiction
    {
        public JH_Jurisdiction() { }

        #region 实体属性

        /// <summary>
        /// 权限ID编号
        /// </summary>
        [Display(Name = "权限ID编号")]
        [Required(ErrorMessage="权限ID编号不能为空。")]
        [StringLength(32, ErrorMessage = "权限ID编号必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JurisdictionID { get; set; }

        /// <summary>
        /// 权限组ID编号
        /// </summary>
        [Required]
        [Display(Name = "权限组ID编号")]
        [StringLength(32, ErrorMessage = "权限组ID编号必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JurisdictionGroupID { get; set; }

        /// <summary>
        /// 权限对象ID编号
        /// </summary>
        [Required]
        [Display(Name = "权限对象ID编号")]
        [StringLength(32, ErrorMessage = "权限对象ID编号必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string JurisdictionObjectID { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        [Required]
        [Display(Name = "对象类型")]
        [StringLength(32, ErrorMessage = "对象类型必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string objectType { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_Jurisdiction";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("JurisdictionID", "JurisdictionID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("JurisdictionGroupID", "JurisdictionGroupID");
            BaseField.Add("JurisdictionObjectID", "JurisdictionObjectID");
            BaseField.Add("objectType", "objectType");
            return BaseField;
        }
        #endregion

    }
}