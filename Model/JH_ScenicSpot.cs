using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model
{
    /// <summary>
    /// 实体类 JH_ScenicSpot, 此类请勿动，以方便表字段更改时重新生成覆盖
    /// </summary>
    public class JH_ScenicSpot
    {
        public JH_ScenicSpot() { }

        #region 实体属性

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        public int id { get; set; }

        /// <summary>
        /// 景区名称
        /// </summary>
        [Required]
        [Display(Name = "景区名称")]
        [StringLength(50, ErrorMessage = "MapName必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string MapName { get; set; }

        /// <summary>
        /// 景区覆盖图片图层
        /// </summary>
        [Required]
        [Display(Name = "景区覆盖图片图层")]
        [StringLength(100, ErrorMessage = "MapImageUrl必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string MapImageUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [Display(Name = "备注")]
        [StringLength(300, ErrorMessage = "备注必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Remarks { get; set; }

        /// <summary>
        /// 坐标范围起点
        /// </summary>
        [Required]
        [Display(Name = "beginCoordinate")]
        [StringLength(100, ErrorMessage = "beginCoordinate必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string beginCoordinate { get; set; }

        /// <summary>
        /// 坐标范围终点
        /// </summary>
        [Required]
        [Display(Name = "endCoordinate")]
        [StringLength(100, ErrorMessage = "endCoordinate必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string endCoordinate { get; set; }

        /// <summary>
        /// 地图默认缩放等级
        /// </summary>
        [Required]
        [Display(Name = "地图默认缩放等级")]
        [StringLength(10, ErrorMessage = "MapZoom必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string MapZoom { get; set; }

        /// <summary>
        /// 地图缩放范围
        /// </summary>
        [Required]
        [Display(Name = "地图缩放范围")]
        [StringLength(50, ErrorMessage = "MapZoomRange必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string MapZoomRange { get; set; }

        /// <summary>
        /// 中心坐标
        /// </summary>
        [Required]
        [Display(Name = "中心坐标")]
        [StringLength(100, ErrorMessage = "中心坐标必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string CoreCoordinate { get; set; }

        /// <summary>
        /// 展示图地址
        /// </summary>
        [Required]
        [Display(Name = "展示图地址")]
        [StringLength(50, ErrorMessage = "展示图地址必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string showsImg { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [Required]
        [Display(Name = "是否推荐")]
        [StringLength(1, ErrorMessage = "是否推荐必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string Recommend { get; set; }

        /// <summary>
        /// 所属城市
        /// </summary>
        [Required]
        [Display(Name = "所属城市")]
        [StringLength(50, ErrorMessage = "city必须至少包含1个字符。", MinimumLength = 1)] //此默认生成的，请根据业务逻辑修改
        public string city { get; set; }

        /// <summary>
        /// 购买金额
        /// </summary>
        [Required]
        [Display(Name = "购买金额")]
        public decimal Monetary { get; set; }

        public string detailed { get; set; }
 
       
        /// <summary>
        /// 省
        /// </summary>
        public string sheng { get; set; }
        /// <summary>
        /// 播放数量
        /// </summary>
        public int Play_number { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 支付类型（1微信支付,2激活码支付,0都支持）
        /// </summary>
        public string Play_type { get; set; }

        /// <summary>
        /// 景区开放时间
        /// </summary>
        public string openTime { get; set; }

        /// <summary>
        /// 具体地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// 页面初始化播放音频
        /// </summary>
        public string start_Play { get; set; }
        /// <summary>
        /// 是否支持蓝牙触发0不支持1支持
        /// </summary>
        public string is_bluetooth { get; set; }

        public string is_app { get; set; }
        /// <summary>
        /// 是否上架0下架1上架
        /// </summary>
        public string is_on_sale { get; set; }

        public string _route { get; set; }
        public string route1 { get; set; }
        public string route2 { get; set; }
        public string route_name { get; set; }
        public string route_name1 { get; set; }
        public string route_name2 { get; set; }

        #endregion

        #region 实体属性Mapping
        /// <summary>
        /// 获取表名称
        /// </summary>
        public static string GetTableName()
        {
            return "JH_ScenicSpot";
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
            BaseField.Add("MapName", "MapName");
            BaseField.Add("MapImageUrl", "MapImageUrl");
            BaseField.Add("Remarks", "Remarks");
            BaseField.Add("beginCoordinate", "beginCoordinate");
            BaseField.Add("endCoordinate", "endCoordinate");
            BaseField.Add("MapZoom", "MapZoom");
            BaseField.Add("MapZoomRange", "MapZoomRange");
            BaseField.Add("CoreCoordinate", "CoreCoordinate");
            BaseField.Add("showsImg", "showsImg");
            BaseField.Add("Recommend", "Recommend");
            BaseField.Add("city", "city");
            BaseField.Add("Monetary", "Monetary");
            BaseField.Add("sheng", "sheng");
            BaseField.Add("Play_number", "Play_number");
            BaseField.Add("level", "level");
            BaseField.Add("Play_type", "Play_type");
            BaseField.Add("openTime", "openTime");
            BaseField.Add("address", "address");
            BaseField.Add("tel", "tel");
            BaseField.Add("start_Play", "start_Play");
            BaseField.Add("detailed", "detailed");
            BaseField.Add("is_bluetooth", "is_bluetooth");
            BaseField.Add("is_app", "is_app");
            BaseField.Add("is_on_sale", "is_on_sale");
            BaseField.Add("_route", "_route");
            BaseField.Add("route1", "route1");
            BaseField.Add("route2", "route2");
            BaseField.Add("route_name", "route_name");
            BaseField.Add("route_name1", "route_name1");
            BaseField.Add("route_name2", "route_name2");
            return BaseField;
        }
        #endregion

    }
}