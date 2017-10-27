using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class MapHtml : System.Web.UI.Page
    {
        public string timestamp = string.Empty;
        public string nonceStr = string.Empty;
        public string signature = string.Empty;

        protected string appId = Common.Utility.AppID;
        private string secret = Common.Utility.AppSecret;
        protected string is_app = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string ticket = string.Empty;
            timestamp = JSSDKHelper.GetTimestamp();
            nonceStr = JSSDKHelper.GetNoncestr();
            JSSDKHelper jssdkhelper = new JSSDKHelper();
            ticket = JsApiTicketContainer.TryGetTicket(appId, secret);
            signature = jssdkhelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri.ToString()); 
            if (!IsPostBack)
            {
                int id = DBUtility.Model.DbValue.GetInt(Request["id"] ?? "");
                string agent = Request.Headers["User-Agent"];
                if (!choose_net(agent))//如果不是微信浏览器
                {
                    Response.Redirect("/alipayHtml.aspx?id=" + id);//跳转到非微信支付页面
                    return;
                }
                if (Request.Cookies["_usercode"] == null)
                {
                    Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6fb51a67c530c955&redirect_uri=http://cloud.jhlxw.com/userbind.aspx&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect");
                }
                SavePage(id);
            }
        }
        public bool choose_net(string userAgent)
        {
            if (userAgent.IndexOf("MicroMessenger") > -1)// Nokia phones and emulators   
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
        /// <summary>
        /// 生成试图页面
        /// </summary>
        void SavePage(int id)
        {
            Model.JH_ScenicSpot m = Repository.BaseBll<Model.JH_ScenicSpot>.Get(new Model.JH_ScenicSpot() { id = id });
            if (m != null)
            {
                is_app = m.is_app;
                string strScript = string.Empty;
                string csFile = string.Empty;
                string designerCS = string.Empty;
                StringBuilder tr = new StringBuilder();
                StringBuilder trs = new StringBuilder();
                List<object> strList = new List<object>();
                List<object> strList2 = new List<object>();
                List<object> strList3 = new List<object>();

                if (!string.IsNullOrWhiteSpace(m._route))
                {
                    string[] _strlist = m._route.Replace("],[", "]|[").Split('|');
                    foreach (var item in _strlist)
                    {
                        double lat = 0.00;
                        double lon = 0.00;
                        //Common.EvilTransform.transform(double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]), double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]), out lat, out lon);
                        string position = double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]) + "," + double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]);
                        var sort = new
                        {
                            lon = position.Split(',')[1],
                            lat = position.Split(',')[0]
                        };
                        strList.Add(sort);
                    }
                }
                if (!string.IsNullOrWhiteSpace(m.route1))
                {
                    string[] _strlist = m.route1.Replace("],[", "]|[").Split('|');
                    foreach (var item in _strlist)
                    {
                        double lat = 0.00;
                        double lon = 0.00;
                        //Common.EvilTransform.transform(double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]), double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]), out lat, out lon);
                        string position = double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]) + "," + double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]);
                        var sort2 = new
                        {
                            lon = position.Split(',')[1],
                            lat = position.Split(',')[0]
                        };
                        strList2.Add(sort2);
                    }
                }
                if (!string.IsNullOrWhiteSpace(m.route2))
                {
                    string[] _strlist = m.route2.Replace("],[", "]|[").Split('|');
                    foreach (var item in _strlist)
                    {
                        double lat = 0.00;
                        double lon = 0.00;
                        //Common.EvilTransform.transform(double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]), double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]), out lat, out lon);
                        string position = double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[0]) + "," + double.Parse(item.Replace("[", "").Replace("]", "").Split(',')[1]);
                        var sort3 = new
                        {
                            lon = position.Split(',')[1],
                            lat = position.Split(',')[0]
                        };
                        strList3.Add(sort3);
                    }
                }

                List<Model.JH_MarkersList> list = Repository.BaseBll<Model.JH_MarkersList>.GetList("ScenicID=" + id + " order by sort asc");
                foreach (var M_item in list)
                {
                    double lat = 0.00;
                    double lon = 0.00;
                    Common.EvilTransform.transform(double.Parse(M_item.position.Split(',')[0]), double.Parse(M_item.position.Split(',')[1]), out lat, out lon);
                    string position = lat + "," + lon;
                    string icon = string.Empty;
                    string viweType = "scenic";
                    if (M_item.icon == "4")//酒店
                    {
                        icon = "icon:'" + "https://cloud.jhlxw.com/app" + M_item.zicon + "',url_out:'" + M_item.url_out + "',";
                        viweType = "Hotel";
                    }
                    else  if (M_item.icon == "1" || M_item.icon == "2")//卫生间和停车场
                    {
                        string _zicon = "https://cloud.jhlxw.com/app" + M_item.zicon;
                        icon = "icon:'" + _zicon + "',";
                        viweType = "toilet";
                    }
                    else if (M_item.icon == "0")//默认景点
                    {
                        viweType = "scenic";
                    }
                    else if (M_item.icon == "3")//工艺广告
                    {
                        icon = "icon:'" + "https://cloud.jhlxw.com/app" + M_item.zicon + "',";
                        viweType = "Advertisement";
                    }
                    string _viweImgUrl = "https://cloud.jhlxw.com/app" + M_item.viweImgUrl;
                    string _audioUrl = "https://cloud.jhlxw.com/app" + M_item.audioUrl;
                    tr.Append("markers.push(");
                    tr.Append("{ " + icon + "viweID: '" + M_item.id + "',distance:" + M_item.distance + ",viweName: '" + M_item.viweName + "',major:'" + M_item.major + "',viweImgUrl: '" + _viweImgUrl + "',viweType: '" + viweType + "', introduction: '" + M_item.introduction + "',audioUrl:{'audioId1-2': '" + _audioUrl + "'},detailsURL:'/details?id=" + M_item.id + "',position:[" + position + "],area:[" + M_item.area + "]});");
                    //trs.Append("{  'position': '" + position + "', 'mark': '1' },");

                    //if (M_item.sort > 0)
                    //{
                    //    var sort = new
                    //    {
                    //        sort = M_item.sort,
                    //        lon = position.Split(',')[1],
                    //        lat = position.Split(',')[0]
                    //    };
                    //    strList.Add(sort);
                    //}
                }
                //List<Model.JH_MarkersList> list2 = Repository.BaseBll<Model.JH_MarkersList>.GetList("ScenicID=" + id + " order by sort2 asc");
                //foreach (var M_item in list2)
                //{
                //    double lat = 0.00;
                //    double lon = 0.00;
                //    Common.EvilTransform.transform(double.Parse(M_item.position.Split(',')[0]), double.Parse(M_item.position.Split(',')[1]), out lat, out lon);
                //    string position = lat + "," + lon;
                //    if (M_item.sort2 > 0)
                //    {
                //        var sort2 = new
                //        {
                //            sort = M_item.sort2,
                //            lon = position.Split(',')[1],
                //            lat = position.Split(',')[0]
                //        };
                //        strList2.Add(sort2);
                //    }
                //}
                //List<Model.JH_MarkersList> list3 = Repository.BaseBll<Model.JH_MarkersList>.GetList("ScenicID=" + id + " order by sort3 asc");
                //foreach (var M_item in list3)
                //{
                //    double lat = 0.00;
                //    double lon = 0.00;
                //    Common.EvilTransform.transform(double.Parse(M_item.position.Split(',')[0]), double.Parse(M_item.position.Split(',')[1]), out lat, out lon);
                //    string position = lat + "," + lon;
                //    if (M_item.sort3 > 0)
                //    {
                //        var sort3 = new
                //        {
                //            sort = M_item.sort3,
                //            lon = position.Split(',')[1],
                //            lat = position.Split(',')[0]
                //        };
                //        strList3.Add(sort3);
                //    }
                //}

                //if (strList.Count > 0)
                //{
                //    trs.Append("lineTrack['路线推荐1']=[];");
                //    foreach (var item in strList)
                //    {
                //        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                //        trs.Append("lineTrack['路线推荐1'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                //    }
                //}
                //if (strList2.Count > 0)
                //{
                //    trs.Append("lineTrack['路线推荐2']=[];");
                //    foreach (var item in strList2)
                //    {
                //        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                //        trs.Append("lineTrack['路线推荐2'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                //    }
                //}
                //if (strList3.Count > 0)
                //{
                //    trs.Append("lineTrack['路线推荐3']=[];");
                //    foreach (var item in strList3)
                //    {
                //        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                //        trs.Append("lineTrack['路线推荐3'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                //    }
                //} 

                if (strList.Count > 0)
                {
                    string trackName = "路线推荐1";
                    if (!string.IsNullOrWhiteSpace(m.route_name))
                    {
                        trackName = m.route_name;
                    }
                    trs.Append("lineTrack['" + trackName + "']=[];");
                    foreach (var item in strList)
                    {
                        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                        trs.Append("lineTrack['" + trackName + "'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                    }
                }
                if (strList2.Count > 0)
                {
                    string trackName = "路线推荐2";
                    if (!string.IsNullOrWhiteSpace(m.route_name1))
                    {
                        trackName = m.route_name1;
                    }
                    trs.Append("lineTrack['" + trackName + "']=[];");
                    foreach (var item in strList2)
                    {
                        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                        trs.Append("lineTrack['" + trackName + "'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                    }
                }
                if (strList3.Count > 0)
                {
                    string trackName = "路线推荐3";
                    if (!string.IsNullOrWhiteSpace(m.route_name2))
                    {
                        trackName = m.route_name2;
                    }
                    trs.Append("lineTrack['" + trackName + "']=[];");
                    foreach (var item in strList3)
                    {
                        JObject json = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(item)) as JObject;
                        trs.Append("lineTrack['" + trackName + "'].push({'position': '" + json["lon"] + "," + json["lat"] + "', 'mark': '1'}); ");
                    }
                }
                string t = string.Empty;
                string display = "";
                if (m.Play_type == "1")
                {
                    t = "<a class=\"aActivate\" style=\"width:80%;\" href=\"/buy?money=" + m.Monetary.ToString() + "&Scenic_id=" + m.id.ToString() + "\">立即购买</a>";
                    display = "style=\"display:none;\"";
                }
                else if (m.Play_type == "2")
                {
                    t = "<a class=\"aActivate\" style=\"width:80%;\" href=\"javascript:sq();\">验证授权</a>";
                }
                else
                {
                    t = "<a class=\"aBuy\" href=\"/buy?money=" + m.Monetary.ToString() + "&Scenic_id=" + m.id.ToString() + "\">立即购买</a>";
                    t += "<a class=\"aActivate\" href=\"javascript:sq();\">验证授权</a>";
                }
                button = t;
                _display = display;
                _id = m.id.ToString();
                title = m.MapName;
                MapName = m.MapName;
                CoreCoordinate = m.CoreCoordinate;
                beginCoordinate = m.beginCoordinate;
                endCoordinate = m.endCoordinate;
                MapZoom = m.MapZoom;
                MapZoomRange = m.MapZoomRange;
                markers = tr.ToString();
                if (!string.IsNullOrWhiteSpace(trs.ToString()))
                {
                    lineTrack += trs.ToString().Substring(0, trs.ToString().Length - 1);
                } 
                money = m.Monetary.ToString();
                openTime = m.openTime.ToString();
                address = m.address.ToString();
                tel = m.tel.ToString();
                level = m.level.ToString();
                showsImg = "https://cloud.jhlxw.com/app" + m.showsImg;
                start_Play = "https://cloud.jhlxw.com/app" + m.start_Play;
                Remarks = jq(m.Remarks);
                is_bluetooth = m.is_bluetooth;
                if (!string.IsNullOrWhiteSpace(m.MapImageUrl))
                    Response.Redirect("Maptest?id=" + id);
            }
        }
        public string is_bluetooth { get; set; }
        public string button { get; set; }
        public string _display { get; set; }
        public string _id { get; set; }
        public string title { get; set; }
        public string MapName { get; set; }
        public string CoreCoordinate { get; set; }
        public string beginCoordinate { get; set; }
        public string endCoordinate { get; set; }
        public string MapZoom { get; set; }
        public string MapZoomRange { get; set; }
        public string markers { get; set; }
        public string lineTrack { get; set; }
        public string money { get; set; }
        public string openTime { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
        public string level { get; set; }
        public string showsImg { get; set; }
        public string start_Play { get; set; }
        public string Remarks { get; set; }
        /// <summary>
        /// 字段截取
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        string jq(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                if (val.Length >= 200)
                    return val.Substring(0, 200) + "...";
                else
                    return val;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}