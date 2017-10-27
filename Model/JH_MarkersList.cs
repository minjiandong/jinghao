using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Model
{
    /// <summary>
    /// 实体类 JH_MarkersList, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_MarkersList
    {
        public JH_MarkersList() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 景点名称
        /// </summary>
        [Required]
        [Display(Name = "景点名称")]
        [StringLength(50, ErrorMessage = "景点名称必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string viweName { get; set; }

        /// <summary>
        /// 景点图片
        /// </summary>
        [Required]
        [Display(Name = "景点图片")]
        [StringLength(100, ErrorMessage = "景点图片必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string viweImgUrl { get; set; }

        /// <summary>
        /// 简单介绍300字内
        /// </summary>
        [Required]
        [Display(Name = "简单介绍300字内")]
        [StringLength(300, ErrorMessage = "简单介绍300字内必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string introduction { get; set; }

        /// <summary>
        /// 音频播放地址
        /// </summary>
        [Required]
        [Display(Name = "音频播放地址")]
        [StringLength(100, ErrorMessage = "音频播放地址必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string audioUrl { get; set; }

        /// <summary>
        /// 详细页地址
        /// </summary>
        [Required]
        [Display(Name = "详细页地址")]
        [StringLength(100, ErrorMessage = "详细页地址必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string detailsURL { get; set; }

        /// <summary>
        /// 景点坐标
        /// </summary>
        [Required]
        [Display(Name = "景点坐标")]
        [StringLength(100, ErrorMessage = "景点坐标必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string position { get; set; }

        /// <summary>
        /// 坐标范围（1个坐标逆时针填写）
        /// </summary>
        [Required]
        [Display(Name = "坐标范围（1个坐标逆时针填写）")]
        [StringLength(300, ErrorMessage = "坐标范围（1个坐标逆时针填写）必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string area { get; set; }

        /// <summary>
        /// 页面详细介绍
        /// </summary>
        [Required]
        [Display(Name = "页面详细介绍")]
        [StringLength(1073741823, ErrorMessage = "页面详细介绍必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string detailed { get; set; }

        /// <summary>
        /// 所属景区ID
        /// </summary>
        [Required]
        [Display(Name = "所属景区ID")]
        public int ScenicID { get; set; }

        /// <summary>
        /// 购买金额
        /// </summary>
        [Required]
        [Display(Name = "购买金额")]
        public decimal Monetary { get; set; }
        /// <summary>
        /// 自定义图标1厕所2酒店 现在（0默认景点，1卫生间，2停车场，3工艺广告，4其他）
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 自定义图标路径
        /// </summary>
        public string zicon { get; set; }
        /// <summary>
        /// 感应距离
        /// </summary>
        public int distance { get; set; }
        /// <summary>
        /// iBeacons设备major号
        /// </summary>
        public string major { get; set; }
        public int sort { get; set; }
        public int sort2 { get; set; }
        public int sort3 { get; set; }
        public string url_out { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_MarkersList";
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
            BaseField.Add("viweName", "viweName");
            BaseField.Add("viweImgUrl", "viweImgUrl");
            BaseField.Add("introduction", "introduction");
            BaseField.Add("audioUrl", "audioUrl");
            BaseField.Add("detailsURL", "detailsURL");
            BaseField.Add("position", "position");
            BaseField.Add("area", "area");
            BaseField.Add("detailed", "detailed");
            BaseField.Add("ScenicID", "ScenicID");
            BaseField.Add("Monetary", "Monetary");
            BaseField.Add("icon", "icon");
            BaseField.Add("zicon", "zicon");
            BaseField.Add("distance", "distance");
            BaseField.Add("major", "major");
            BaseField.Add("sort", "sort");
            BaseField.Add("sort2", "sort2");
            BaseField.Add("sort3", "sort3");
            BaseField.Add("url_out", "url_out");
            return BaseField;
        }
        #endregion

    }
}