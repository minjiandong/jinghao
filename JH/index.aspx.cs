using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Xml;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;

namespace JH
{
    public partial class index : System.Web.UI.Page
    {
        public string city { get; set; }


        private static double PI = 3.14159265358979324;       //圆周率 
        private static string DOUBLE_FORMAT = string.Format("#.000000");
        private static double R = 6378245.0;                  //地球半径 单位：米


        public string timestamp = string.Empty;
        public string nonceStr = string.Empty;
        public string signature = string.Empty;

        protected string appId = Common.Utility.AppID;
        private string secret = Common.Utility.AppSecret;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AccessTokenContainer.Register(Common.Utility.AppID, Common.Utility.AppSecret);
                string ticket = string.Empty;
                timestamp = JSSDKHelper.GetTimestamp();
                nonceStr = JSSDKHelper.GetNoncestr();
                JSSDKHelper jssdkhelper = new JSSDKHelper();
                ticket = JsApiTicketContainer.TryGetTicket(appId, secret);
                signature = jssdkhelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri.ToString());
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('"+ex.Message+"');</script>");
            }
        }

        public static double toRadians(double x)
        {
            return x * PI / 180;
        }
        /**
     * 获取坐标距离(米)
     *
     * @param lng1 起始经度
     * @param lat1 起始纬度
     * @param lng2 目地地经度
     * @param lat2 目的地纬度
     * @return
     */
        public static int getDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double dx = lng1 - lng2; // 经度差值
            double dy = lat1 - lat2; // 纬度差值
            double b = (lat1 + lat2) / 2.0; // 平均纬度
            double Lx = toRadians(dx) * R * Math.Cos(toRadians(b)); // 东西距离
            double Ly = R * toRadians(dy); // 南北距离
            return (int)Math.Sqrt(Lx * Lx + Ly * Ly);
        }
        /**
    * 根据距离返回,经纬度范围 返回顺序 minLat,minLng,maxLat,maxLng
    *
    * @param lat
    * @param lon
    * @param raidus 距离（半径）单位:米
    * @return
    */
        public static double[] getAround(double lat, double lon, int raidus)
        { 
            try
            {
                Double latitude = lat;
                Double longitude = lon; 
                Double degree = (24901 * 1609) / 360.0; // 赤道周长24901英里 1609是转换成米的系数 
                Double dpmLat = 1 / degree;
                Double radiusLat = dpmLat * raidus;
                Double minLat = latitude - radiusLat;
                Double maxLat = latitude + radiusLat;

                Double mpdLng = degree * Math.Cos(toRadians(latitude));
                Double dpmLng = 1 / mpdLng;
                Double radiusLng = dpmLng * raidus;
                Double minLng = longitude - radiusLng;
                Double maxLng = longitude + radiusLng;

                // 格式化
                minLat = Double.Parse(minLat.ToString());
                minLng = Double.Parse(minLng.ToString());
                maxLat = Double.Parse(maxLat.ToString());
                maxLng = Double.Parse(maxLng.ToString());

                return new double[] { minLat, minLng, maxLat, maxLng };
            }
            catch (Exception ex)
            {
            }
            return null;
        } 
        /// <summary>
        /// 计算出所在城市
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public string getCity(string lat,string lon)
        {
            try
            {
                string xml = GetInfo("http://api.map.baidu.com/geocoder/v2/?ak=cAlALbY22hDluR99lZNdpgMPPalLyGkt&callback=renderReverse&location=" + lat + "," + lon + "&output=xml&pois=1");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                string json = JsonConvert.SerializeXmlNode(doc);
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                return jo["GeocoderSearchResponse"]["result"]["addressComponent"]["city"].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>  
        /// 获取url的返回值  
        /// </summary>   
        public string GetInfo(string url)
        {
            string strBuff = "";
            Uri httpURL = new Uri(url); 
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL); 
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse(); 
            Stream respStream = httpResp.GetResponseStream(); 
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
            strBuff = respStreamReader.ReadToEnd();
            return strBuff;
        }
    }
}