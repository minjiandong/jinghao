using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_SYS_USER, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_SYS_USER
    {
        public JH_SYS_USER() { }

        #region 实体属性

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Required(ErrorMessage="用户ID不能为空。")]
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        [StringLength(30, ErrorMessage = "用户名必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [Display(Name = "密码")]
        [StringLength(100, ErrorMessage = "密码必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string UserPassword { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required]
        [Display(Name = "真实姓名")]
        [StringLength(30, ErrorMessage = "真实姓名必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string FullName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [Display(Name = "手机号码")]
        [StringLength(11, ErrorMessage = "手机号码必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Phone { get; set; }

        /// <summary>
        /// 用户状态（1启用0禁用）
        /// </summary>
        [Required]
        [Display(Name = "用户状态（1启用0禁用）")]
        [StringLength(1, ErrorMessage = "用户状态（1启用0禁用）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string UserState { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        [Display(Name = "性别")]
        [StringLength(1, ErrorMessage = "性别必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string UserSex { get; set; }

        /// <summary>
        /// 所属地市
        /// </summary>
        [Required]
        [Display(Name = "所属地市")]
        [StringLength(32, ErrorMessage = "所属地市必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string CityID { get; set; }

        /// <summary>
        /// 用户类型，1供应商0系统用户
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// 提成点
        /// </summary>
        public decimal Commission { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_SYS_USER";
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        public static Dictionary<string, string> GetIdentityMapping()
        {
            Dictionary<string, string> Identity = new Dictionary<string, string>();
            Identity.Add("UserID", "UserID");
            return Identity;
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        public static Dictionary<string, string> GetBaseFieldMapping()
        {
            Dictionary<string, string> BaseField = new Dictionary<string, string>();
            BaseField.Add("UserName", "UserName");
            BaseField.Add("UserPassword", "UserPassword");
            BaseField.Add("FullName", "FullName");
            BaseField.Add("Phone", "Phone");
            BaseField.Add("UserState", "UserState");
            BaseField.Add("UserSex", "UserSex");
            BaseField.Add("CityID", "CityID");
            BaseField.Add("UserType", "UserType");
            BaseField.Add("Commission", "Commission");
            return BaseField;
        }
        #endregion

    }
}