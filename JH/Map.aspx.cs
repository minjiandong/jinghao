using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace JH
{
    public partial class Map : System.Web.UI.Page
    {
        public string timestamp = string.Empty;
        public string nonceStr = string.Empty;
        public string signature = string.Empty;

        protected string appId = Common.Utility.AppID;
        private string secret = Common.Utility.AppSecret;
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
                SavePage(id);
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
                string strScript = string.Empty;
                string csFile = string.Empty;
                string designerCS = string.Empty;
                StringBuilder tr = new StringBuilder();
                StringBuilder trs = new StringBuilder();
                List<Model.JH_MarkersList> list = Repository.BaseBll<Model.JH_MarkersList>.GetList("ScenicID=" + id + "");
                foreach (var M_item in list)
                {
                    double lat = 0.00;
                    double lon = 0.00;
                    Common.EvilTransform.transform(double.Parse(M_item.position.Split(',')[0]), double.Parse(M_item.position.Split(',')[1]), out lat, out lon);
                    string position = lat + "," + lon;
                    string icon = string.Empty;
                    string viweType = "scenic";
                    if (M_item.icon == "2")
                    {
                        icon = "icon:'" + M_item.zicon + "',";
                        viweType = "Hotel";
                    }
                    if (M_item.icon == "1")
                    {
                        icon = "icon:'" + M_item.zicon + "',";
                        viweType = "toilet";
                    }
                    string _viweImgUrl = "https://cloud.jhlxw.com/app" + M_item.viweImgUrl;
                    string _audioUrl = "https://cloud.jhlxw.com/app" + M_item.audioUrl;
                    tr.Append("markers.push(");
                    tr.Append("{ " + icon + "viweID: '" + M_item.id + "',distance:" + M_item.distance + ",viweName: '" + M_item.viweName + "',viweImgUrl: '" + _viweImgUrl + "',viweType: '" + viweType + "', introduction: '" + M_item.introduction + "',audioUrl:{'audioId1-2': '" + _audioUrl + "'},detailsURL:'/details?id=" + M_item.id + "',position:[" + position + "],area:[" + M_item.area + "]});");
                    trs.Append("{  'position': '" + position + "', 'mark': '1' },");
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
                lineTrack = trs.ToString().Substring(0, trs.ToString().Length - 1);
                money = m.Monetary.ToString();
                openTime = m.openTime.ToString();
                address = m.address.ToString();
                tel = m.tel.ToString();
                level = m.level.ToString();
                showsImg = "https://cloud.jhlxw.com/app" + m.showsImg;
                start_Play = "https://cloud.jhlxw.com/app" + m.start_Play;
                Remarks = jq(m.Remarks); 
            }
        }
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