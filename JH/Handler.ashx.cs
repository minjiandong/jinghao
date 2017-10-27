using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Data;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;

namespace JH
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler
    {
        JH.Models.jsoninfo json = new Models.jsoninfo();
        private static double PI = 3.14159265358979324;       //圆周率 
        private static string DOUBLE_FORMAT = string.Format("#.000000");
        private static double R = 6378245.0;                  //地球半径 单位：米
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string m = context.Request["p"] ?? "";
                switch (m)
                {
                    case "GetTop":
                        GetTop(context);
                        break;
                    case "getPosition":
                        getPosition(context);
                        break;
                    case "getCity":
                        getCity(context);
                        break;
                    case "search":
                        search(context);
                        break;
                    case "valid":
                        validCode(context);
                        break;
                    case "pay":
                        pay(context);
                        break;
                    case "buy":
                        buy(context);
                        break;
                    case "sq":
                        sq(context);
                        break;
                    case "PlayCount":
                        PlayCount(context);
                        break;
                    case "Scan":
                        Scan(context);
                        break;
                    case "buyhandler":
                        buyhandler(context);
                        break;
                    case "validalipay":
                        validalipay(context);
                        break;
                    case "codeAlipay":
                        codeAlipay(context);
                        break;
                    default:
                        json.info = "请求错误！";
                        log.Sys_log.Error(json.info);
                        context.Response.Write(JsonConvert.SerializeObject(json));
                        break;
                }
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                log.Sys_log.Error(json.info);
                context.Response.Write(JsonConvert.SerializeObject(json));
            }
            finally
            {
                context.Response.End();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void buyhandler(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + DBUtility.Model.DbValue.GetInt(id) + "");
            if (_dt.Rows.Count > 0)
            {
                string openid = c.Request["openid"] ?? "";
                string order_sn = "G" + Common.Utility.GetDataRandom();
                int scenic_id = DBUtility.Model.DbValue.GetInt(id);
                int user_id = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where openid='" + openid + "'").ToString());
                int add_time = getSecondEnd(DateTime.Now);
                string is_pay = "1";
                int pay_type = 1;
                string strsql = "insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type)values('" + order_sn + "'," + scenic_id + "," + user_id + "," + add_time + ",'" + is_pay + "'," + DBUtility.Model.DbValue.GetDecimal(_dt.Rows[0]["cost_money"].ToString()) + "," + pay_type + ")";
                int ret = DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, strsql);
                if (ret > 0)
                {
                    string MapHtml = string.Format("/MapHtml?id={0}",id); 
                    string MapTest = string.Format("/MapTest?id={0}", id);

                    if (string.IsNullOrWhiteSpace(_dt.Rows[0]["map_img"].ToString())) 
                        c.Response.Write(JsonConvert.SerializeObject(new
                        {
                            state = "ok",
                            url = MapHtml,
                            id = id
                        }));
                    else
                        c.Response.Write(JsonConvert.SerializeObject(new
                        {
                            state = "ok",
                            url = MapTest,
                            id = id
                        }));
                }
                else
                {
                    c.Response.Write(JsonConvert.SerializeObject(new
                    {
                        state = "no",
                        url = "index",
                        id = ""
                    }));
                }
            }
        }
        void Scan(HttpContext c)
        {
            string par = c.Request["par"] ?? "";
            string r = c.Request["r"] ?? "";
            string url = string.Format("https://cloud.jhlxw.com/map-{0}.html?r={1}", par,r);
            string appId = Common.Utility.AppID;
            string secret = Common.Utility.AppSecret; 
            string timestamp = string.Empty;
            string nonceStr = string.Empty;
            string signature = string.Empty;
            string ticket = string.Empty;
            timestamp = JSSDKHelper.GetTimestamp();
            nonceStr = JSSDKHelper.GetNoncestr();
            JSSDKHelper jssdkhelper = new JSSDKHelper();
            ticket = JsApiTicketContainer.TryGetTicket(appId, secret);
            signature = jssdkhelper.GetSignature(ticket, nonceStr, timestamp, url); 
            var SmPar = new
            {
                appId = appId,
                timestamp = timestamp,
                nonceStr = nonceStr,
                signature = signature
            };
            c.Response.Write(JsonConvert.SerializeObject(SmPar));
        }
        void PlayCount(HttpContext c)
        {
            int Scenic_id = DBUtility.Model.DbValue.GetInt(c.Request["Scenic_id"] ?? "");
            string strsql = string.Format("update JH_ScenicSpot set Play_number=Play_number+1 where id={0}", Scenic_id);
            DBUtility.DALHelper.dbHelper.ExecuteNonQuery(CommandType.Text, strsql);
        }
        /// <summary>
        /// 支付宝授权码支付
        /// </summary>
        /// <param name="c"></param>
        void codeAlipay(HttpContext c)
        {
            decimal money = DBUtility.Model.DbValue.GetDecimal(c.Request["money"] ?? "");
            int Scenic_id = DBUtility.Model.DbValue.GetInt(c.Request["Scenic_id"] ?? "");
            string code = c.Request["code"] ?? "";
            DataTable count = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select value from zy_activationcode where is_issue=1 and is_use=0 and codename='" + code + "'");
            if (count.Rows.Count > 0)
            {
                if (DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["value"].ToString()) == money)
                {
                    json.info = "ok";
                }
                else
                {
                    json.info = "抱歉，您输入的激活码金额无法购买该景区！";
                }
            }
            else
            {
                json.info = "你输入的激活码无效！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void sq(HttpContext c) 
        {

            decimal money = DBUtility.Model.DbValue.GetDecimal(c.Request["money"] ?? "");
            int Scenic_id = DBUtility.Model.DbValue.GetInt(c.Request["Scenic_id"] ?? "");
            string openid = c.Request["openid"] ?? "";
            string code = c.Request["code"] ?? "";
            DataTable count = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select value from zy_activationcode where is_issue=1 and is_use=0 and codename='" + code + "'");
            if (count.Rows.Count > 0)
            {
                if (DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["value"].ToString()) >= money)
                {
                    List<string> listsql = new List<string>();
                    string order_sn = "G" + Common.Utility.GetDataRandom();
                    int scenic_id = Scenic_id;
                    int user_id = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where openid='" + openid + "'").ToString());
                    int add_time = getSecondEnd(DateTime.Now);
                    string is_pay = "1";
                    int pay_type = 4;
                    listsql.Add("insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type)values('" + order_sn + "'," + scenic_id + "," + user_id + "," + add_time + ",'" + is_pay + "'," + money + "," + pay_type + ")");

                    if (DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["value"].ToString()) > money)
                    {
                        decimal facemoney = DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["value"].ToString()) - money;
                        listsql.Add("update zy_users set user_money=user_money+" + facemoney + " where openid='" + openid + "'");
                    }
                    listsql.Add("update zy_activationcode set is_use=1,use_time='" + getSecondEnd(DateTime.Now) + "' where codename='" + code + "'");
                    int i = DBUtility.DALHelper.myHelper.ExecuteNonQuery(listsql);
                    if (i > 0)
                        json.info = "ok";
                    else
                        json.info = "充值失败！";
                }
                else
                {
                    json.info = "抱歉，您输入的激活码金额无法购买该景区！";
                }
            }
            else
            {
                json.info = "你输入的激活码无效！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
            //decimal money = DBUtility.Model.DbValue.GetDecimal(c.Request["money"] ?? "");
            //int Scenic_id = DBUtility.Model.DbValue.GetInt(c.Request["Scenic_id"] ?? "");
            //string openid = c.Request["openid"] ?? "";
            //string code = c.Request["code"] ?? "";
            //DataTable count = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, "select faceValue from JH_ActivationCard where code='" + code + "' and isRelease='1' and isUse='0'");
            //if (count.Rows.Count > 0)
            //{
            //    if (DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["faceValue"].ToString()) >= money)
            //    {
            //        List<string> listsql = new List<string>();
            //        Model.JH_PAYLIST model = new Model.JH_PAYLIST();
            //        model.money = money;
            //        model.openid = openid;
            //        model.PayDate = DateTime.Now.ToString();
            //        model.PayType = "2";
            //        listsql.Add("insert into JH_PAYLIST(money,openid,PayDate,PayType)values(" + model.money + ",'" + model.openid + "','" + model.PayDate + "','" + model.PayType + "')");

            //        Model.JH_Activation m = new Model.JH_Activation();
            //        m.openid = openid;
            //        m.money = money;
            //        m.Scenic_id = Scenic_id;
            //        m.paymentType = "0";
            //        m.consumptionDate = DateTime.Now.ToString();
            //        listsql.Add("insert into JH_Activation(openid,money,Scenic_id,consumptionDate,paymentType)values('" + m.openid + "'," + m.money + "," + m.Scenic_id + ",'" + m.consumptionDate + "','0')");

            //        if (DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["faceValue"].ToString()) > money)
            //        {
            //            decimal facemoney = DBUtility.Model.DbValue.GetDecimal(count.Rows[0]["faceValue"].ToString()) - money;
            //            listsql.Add("update JH_USERS set balance=balance+" + facemoney + " where openid='" + openid + "'");
            //        }
            //        listsql.Add("update JH_ActivationCard set isUse='1',UseDate='" + DateTime.Now.ToString() + "',Userid='" + openid + "' where Code='" + code + "'");
            //        int i = DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql);
            //        if (i > 0)
            //            json.info = "ok";
            //        else
            //            json.info = "充值失败！";
            //    }
            //    else
            //    {
            //        json.info = "抱歉，您输入的激活码金额无法购买该景区！";    
            //    }
            //}
            //else
            //{
            //    json.info = "你输入的激活码无效！";
            //}
            
        }
        //购买景区
        void buy(HttpContext c)
        {
            decimal money = DBUtility.Model.DbValue.GetDecimal(c.Request["money"] ?? "");
            int Scenic_id = DBUtility.Model.DbValue.GetInt(c.Request["Scenic_id"] ?? "");
            string openid = c.Request["openid"] ?? "";
            decimal balance = DBUtility.Model.DbValue.GetDecimal(DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select balance from JH_USERS where openid='" + openid + "'").ToString());
            if (money > balance)
            {
                json.info = "你当前余额不足，请到个人中心充值后在购买！";
            }
            else
            {
                Model.JH_Activation model = new Model.JH_Activation();
                model.openid = openid;
                model.money = money;
                model.Scenic_id = Scenic_id;
                model.consumptionDate = DateTime.Now.ToString();
                List<string> listsql = new List<string>();
                listsql.Add("insert into JH_Activation(openid,money,Scenic_id,consumptionDate,paymentType)values('" + model.openid + "'," + model.money + "," + model.Scenic_id + ",'" + model.consumptionDate + "','0')");
                listsql.Add("update JH_USERS set balance=balance-" + model.money + " where openid='" + model.openid + "'");
                int i = DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql);
                if (i > 0)
                    json.info = "ok";
                else
                    json.info = "支付失败，请重试！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        /// <summary>
        /// 将结束时间转换成INT型
        /// </summary>
        /// <param name="end">结束时间</param>
        /// <returns>int值</returns>
        private int getSecondEnd(DateTime end)
        {
            int result = 0;
            DateTime startdate = new DateTime(1970, 1, 1, 8, 0, 0);
            TimeSpan seconds = end.AddDays(0) - startdate;
            result = Convert.ToInt32(seconds.TotalSeconds);
            return result;
        }
        void pay(HttpContext c)
        {
            var userid = c.Request["userid"] ?? "";
            var val = c.Request["val"] ?? "";
            var statie = c.Request["statie"] ?? "";

            if (string.IsNullOrWhiteSpace(userid))
            {
                json.info = "充值失败！";
            }
            else
            {
                if (statie == "jhm")
                {

                    DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select value from zy_activationcode where is_issue=1 and is_use=0 and codename='" + val + "'");
                    if (dt.Rows.Count > 0)
                    {
                        List<string> listsql = new List<string>();

                        listsql.Add("update zy_users set user_money=user_money+" + DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["value"].ToString()) + " where openid='" + userid + "'");
                        listsql.Add("update zy_activationcode set is_use=1,use_time='" + getSecondEnd(DateTime.Now) + "' where codename='" + val + "'");
                        //listsql.Add("insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type)values()");
                        int i = DBUtility.DALHelper.myHelper.ExecuteNonQuery(listsql);
                        if (i > 0)
                            json.info = "ok";
                        else
                            json.info = "充值失败！";
                    }
                    else
                    {
                        json.info = "激活码无效！";
                    }



                    //DataTable dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, "select faceValue from JH_ActivationCard where isRelease='1' and isUse='0' and Code='" + val + "'");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    List<string> listsql = new List<string>();
                    //    Model.JH_PAYLIST model = new Model.JH_PAYLIST();
                    //    model.money = DBUtility.Model.DbValue.GetDecimal(val);
                    //    model.openid = userid;
                    //    model.PayDate = DateTime.Now.ToString();
                    //    model.PayType = "2";
                    //    listsql.Add("insert into JH_PAYLIST(money,openid,PayDate,PayType)values(" + model.money + ",'" + model.openid + "','" + model.PayDate + "','" + model.PayType + "')");
                    //    listsql.Add("update JH_USERS set balance=balance+" + DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["faceValue"].ToString()) + " where openid='" + userid + "'");
                    //    listsql.Add("update JH_ActivationCard set isUse='1',UseDate='" + DateTime.Now.ToString() + "',Userid='" + userid + "' where Code='" + val + "'");
                    //    int i = DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql);
                    //    if (i > 0)
                    //        json.info = "ok";
                    //    else
                    //        json.info = "充值失败！";
                    //}
                    //else
                    //{
                    //    json.info = "激活码无效！";
                    //}
                }
                else
                {
                    json.info = "暂未开通！";
                }
            }
            
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        /// <summary>
        /// 通过验证码判断是否授权
        /// </summary>
        /// <param name="c"></param>
        void validalipay(HttpContext c)
        {
            string validCode = c.Request["validCode"] ?? "";//用户ID
            int scenicId = DBUtility.Model.DbValue.GetInt(c.Request["scenicId"] ?? "");//景区ID
            object sobj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select cost_money from zy_scenics where scenic_id=" + scenicId + "");
            if (DBUtility.Model.DbValue.GetDecimal(sobj.ToString()) <= 0.00m)
            {
                json.info = "ok";
            }
            else
            {
                string strsql = "select count(1) from zy_scenic_orders as b,zy_users as a where b.user_id=a.user_id and b.scenic_id=" + scenicId + " and b.is_pay=1 and a.user_id='" + validCode + "'";
                object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, strsql);
                if (DBUtility.Model.DbValue.GetInt(obj) > 0)
                {
                    json.info = "ok";
                }
                else
                {
                    json.info = "no";
                }
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        } 
        void validCode(HttpContext c)
        {
            string validCode = c.Request["validCode"] ?? "";//用户ID
            int scenicId = DBUtility.Model.DbValue.GetInt(c.Request["scenicId"] ?? "");//景区ID
            object sobj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select cost_money from zy_scenics where scenic_id=" + scenicId + "");
            if (DBUtility.Model.DbValue.GetDecimal(sobj.ToString()) <= 0.00m)
            {
                json.info = "ok";
            }
            else
            {
                string strsql = "select count(1) from zy_scenic_orders as b,zy_users as a where b.user_id=a.user_id and b.scenic_id=" + scenicId + " and b.is_pay=1 and a.openid='" + validCode + "'"; 
                object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, strsql);
                if (DBUtility.Model.DbValue.GetInt(obj) > 0)
                {
                    json.info = "ok";
                }
                else
                {
                    json.info = "no";
                }
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
            //object sobj = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select Monetary from JH_ScenicSpot where id=" + scenicId + "");
            //if (DBUtility.Model.DbValue.GetDecimal(sobj.ToString()) == 0)
            //{
            //    json.info = "ok";
            //}
            //else
            //{
            //    object obj = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select count(1) from JH_Activation where openid='" + validCode + "' and Scenic_id=" + scenicId + "");
            //    if (DBUtility.Model.DbValue.GetInt(obj) > 0)
            //    {
            //        json.info = "ok";
            //    }
            //    else
            //    {
            //        json.info = "no";
            //    }
            //}
            
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
        public static double toRadians(double x)
        {
            return x * PI / 180;
        }
        void getPosition(HttpContext c)
        {
            Double lat = DBUtility.Model.DbValue.GetDouble(c.Request["lat"] ?? "");
            Double lon = DBUtility.Model.DbValue.GetDouble(c.Request["lon"] ?? "");
            string city = c.Request["city"] ?? "";
            Double mglat = 0.00;
            Double mglon = 0.00;
            Common.EvilTransform.transform(lat, lon, out mglat, out mglon);
            List<Model.JH_ScenicSpot> list = Repository.BaseBll<Model.JH_ScenicSpot>.GetList("Recommend='0' and city='" + city + "' ");
            
            foreach (var item in list)
            {
                int dou = distance1(mglon, mglat, DBUtility.Model.DbValue.GetDouble(item.CoreCoordinate.Split(',')[1]), DBUtility.Model.DbValue.GetDouble(item.CoreCoordinate.Split(',')[0]));
                if (dou <= 200)
                {
                    if (item.is_app == "2")
                    {
                        json.info = "http://app.jhlxw.com/index.php/Api/Load/Html_share";
                    }
                    else
                    {
                        if (item.MapImageUrl == "")
                        {
                            json.info = string.Format("MapHtml?id={0}", item.id);
                        }
                        else
                        {
                            json.info = string.Format("MapTest?id={0}", item.id);
                        }
                        
                        break;
                    } 
                }
                else
                {
                    json.info = "no";
                }
            }
            c.Response.Write(JsonConvert.SerializeObject(json));

            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //List<Dictionary<string, string>> diclist = new List<Dictionary<string, string>>();
            //List<Model.JH_ScenicSpot> list = Repository.BaseBll<Model.JH_ScenicSpot>.GetList("1=1");
            //foreach (var item in list)
            //{
            //    string[] arr = item.CoreCoordinate.Split(',');
            //    string lat = arr[0].ToString();
            //    string lng = arr[1].ToString();
            //    dic.Add("lat", lat);
            //    dic.Add("lng", lng);
            //    dic.Add("id",item.id.ToString());
            //    diclist.Add(dic);
            //}
            //c.Response.Write(JsonConvert.SerializeObject(diclist));
        }
        void search(HttpContext c)
        {
            string city = c.Request["city"] ?? "";
            string keyword = c.Request["keyword"] ?? "";
            Double lat = DBUtility.Model.DbValue.GetDouble(c.Request["lat"] ?? "");
            Double lon = DBUtility.Model.DbValue.GetDouble(c.Request["lon"] ?? "");
            Double mglat = 0.00;
            Double mglon = 0.00;
            Common.EvilTransform.transform(lat, lon, out mglat, out mglon); 
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = 6;
            int totalCount = 0; 
            StringBuilder tr = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                tr.AppendFormat(" and mapname like '%{0}%'",keyword);
            }

            Model.JH_ScenicSpot model = new Model.JH_ScenicSpot();
            //List<Model.JH_ScenicSpot> list = Repository.BaseBll<Model.JH_ScenicSpot>.GetList(pagesize, page, out totalCount, string.Format("Recommend='0' and city='{1}' {0} ", tr.ToString(), city)); 
            List<Model.JH_ScenicSpot> list = Repository.BaseBll<Model.JH_ScenicSpot>.GetList("is_on_sale='1' and city='" + city + "' " + tr.ToString());
            List<object> li = new List<object>();
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("showsImg", typeof(string));
            dt.Columns.Add("MapName", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("distance", typeof(double));
            dt.Columns.Add("Play_number", typeof(int));
            dt.Columns.Add("level",typeof(string));
            dt.Columns.Add("MapImageUrl",typeof(string));
            dt.Columns.Add("is_app",typeof(string));
            
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = item.id;
                dr["showsImg"] = item.showsImg;
                dr["MapName"] = item.MapName;
                dr["Remarks"] = item.Remarks;
                dr["distance"] = distance(mglon, mglat, DBUtility.Model.DbValue.GetDouble(item.CoreCoordinate.Split(',')[1]), DBUtility.Model.DbValue.GetDouble(item.CoreCoordinate.Split(',')[0]));
                dr["Play_number"] = item.Play_number;
                dr["level"] = item.level;
                dr["MapImageUrl"] = item.MapImageUrl;
                dr["is_app"] = item.is_app;
                dt.Rows.Add(dr);  
            }
            DataView dv = new DataView(dt);
            dv.Sort = "distance asc";
            DataTable dtv = dv.ToTable();
            List<object> _li = new List<object>();
            for (int i = 0; i < dtv.Rows.Count; i++)
            {
                IDictionary<object, object> dict = new Dictionary<object, object>();
                dict.Add("id", dtv.Rows[i]["id"].ToString());
                dict.Add("showsImg", dtv.Rows[i]["showsImg"].ToString());
                dict.Add("MapName", dtv.Rows[i]["MapName"].ToString());
                dict.Add("Remarks", dtv.Rows[i]["Remarks"].ToString());
                dict.Add("distance", dtv.Rows[i]["distance"]);
                dict.Add("Play_number", dtv.Rows[i]["Play_number"]);
                dict.Add("level", dtv.Rows[i]["level"]);
                dict.Add("MapImageUrl", dtv.Rows[i]["MapImageUrl"].ToString());
                dict.Add("is_app",dtv.Rows[i]["is_app"].ToString());
                _li.Add(dict);
            }
            totalCount = _li.Count;
            List<object> _list = _li.Skip(pagesize * (page - 1)).Take(pagesize).ToList();
            c.Response.Write(JsonConvert.SerializeObject(_list));
        }
        double distance(double lng1, double lat1, double lng2, double lat2)
        {
            var i = getDistance(lng1, lat1, lng2, lat2);
            float m = (float)i / 1000;
            return Math.Round(m, 2);
        }
        int distance1(double lng1, double lat1, double lng2, double lat2)
        {
            var i = getDistance(lng1, lat1, lng2, lat2); 
            return i;
        }
        void getCity(HttpContext c)
        {
            string lat = c.Request["lat"] ?? "";
            string lon = c.Request["lon"] ?? "";
            json.info = getCity(lat, lon);
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        public string getCity(string lat, string lon)
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
        void GetTop(HttpContext c)
        {
            StringBuilder t = new StringBuilder();
            t.Append("<li>");
            List<Model.JH_ScenicSpot> list = Repository.BaseBll<Model.JH_ScenicSpot>.GetList("Recommend='1'");
            foreach (var item in list)
            {
                t.AppendFormat("< a href = \"/MapHtml?id={0}\"><img alt = \"{1}\" src =\"{2}\" /></a>",item.id,item.MapName,item.showsImg);
            }
            t.Append("</li>");
            json.info = t.ToString();
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}