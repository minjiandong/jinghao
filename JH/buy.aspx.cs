using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace JH
{
    public partial class buy : System.Web.UI.Page
    {
        protected decimal balance { get; set; }
        protected string _title { get; set; }
        protected string _content { get; set; }
        protected string _url { get; set; }
        protected string _yue { get; set; }

        public static string wxEditAddrParam { get; set; }
        //对用户访问进行判断 如果是的话返回true，否则返回false  
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //string agent = Request.Headers["User-Agent"];
            //if (!choose_net(agent))
            //{
            //    Response.Redirect("");
            //}

            if (Request.Cookies["_usercode"] == null)
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6fb51a67c530c955&redirect_uri=http://cloud.jhlxw.com/userbind.aspx&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect");
            }
         
            string strsql = "select count(1) from zy_scenic_orders as b,zy_users as a where b.user_id=a.user_id and b.scenic_id=" + DBUtility.Model.DbValue.GetInt(t) + " and b.is_pay=1 and a.openid='" + Request.Cookies["_usercode"] + "'";
            object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, strsql);
            if (DBUtility.Model.DbValue.GetInt(obj) > 0)
            {
                Response.Redirect("/index");
            }

            if (!IsPostBack)
            {
                JsApiPay jsApiPay = new JsApiPay(this);
                try
                {
                    //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                    jsApiPay.GetOpenidAndAccessToken();
                    //获取收货地址js函数入口参数
                    wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                    ViewState["openid"] = jsApiPay.openid;
                    ViewState["unionid"] = jsApiPay.unionid;
                    string openid = Request.Cookies["_usercode"].Value;
                  
                    open_id = openid;

                    DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_users where openid='" + open_id + "'");
                    if (dt.Rows.Count > 0)
                    {
                        balance = DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["user_money"].ToString());
                    }
                    DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + DBUtility.Model.DbValue.GetInt(t) + "");
                    if (_dt.Rows.Count > 0)
                    {
                        _title = _dt.Rows[0]["scenic_name"].ToString();
                        _content = _dt.Rows[0]["scenics_text"].ToString();
                        _url = "https://cloud.jhlxw.com/app" + _dt.Rows[0]["scenic_img"].ToString();
                        if (_dt.Rows[0]["buy_type"].ToString() == "1")
                        {
                            _yue = "display:none;";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + ex.Message + "</span>");
                    Button1.Visible = false;
                }
            }
            


        }
        public string open_id { get; set; }

        protected string m
        {
            get
            {
                string money = Request["moneys"] ?? "";
                if (!string.IsNullOrWhiteSpace(money))
                {
                    return money.Split('|')[0];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected string t
        {
            get
            {
                string money = Request["moneys"] ?? "";
                if (!string.IsNullOrWhiteSpace(money))
                {
                    return money.Split('|')[1];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected string ZFmoney
        {
            get
            {
                return Request["money"] ?? "";
            }
        }
        protected string Scenicid
        {
            get
            {
                return Request["Scenic_id"] ?? "";
            }
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (statie.Value == "0")
            {
                int total_fee =  int.Parse((DBUtility.Model.DbValue.GetDouble(m) * 100).ToString());
                if (ViewState["unionid"] != null)
                {
                    string openid = ViewState["openid"].ToString();
                    string unionid = ViewState["unionid"].ToString();
                    object _c = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, string.Format("select count(1) from zy_scenic_orders where scenic_id={0} and is_pay='1' and user_id={1}", DBUtility.Model.DbValue.GetInt(t), DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where openid='" + unionid + "'").ToString())));
                    if (DBUtility.Model.DbValue.GetInt(_c.ToString()) > 0)
                    {

                        Response.Write("<script>alert('您已购买请不要重复购买！');location.href='/index.aspx';</script>");
                    }
                    else
                    { 
                        string url = Common.Utility.url + "/example/JsApiPayPage.aspx?openid=" + openid + "&unionid=" + unionid + "&total_fee=" + total_fee + "&Scenic_id=" + t;
                        Response.Redirect(url);
                    } 
                }
                else
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面缺少参数，请返回重试" + "</span>");
                    Button1.Visible = false;
                }
            }
            else
            {
                DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_users where openid='" + ViewState["unionid"].ToString() + "'");
                if (dt.Rows.Count > 0)
                {
                    if (DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["user_money"].ToString()) >= DBUtility.Model.DbValue.GetDecimal(m))
                    { 
                        decimal money = DBUtility.Model.DbValue.GetDecimal(m);
                        int Scenic_id = DBUtility.Model.DbValue.GetInt(t);
                        Model.JH_Activation model = new Model.JH_Activation();
                        model.openid = ViewState["unionid"].ToString();
                        model.money = money;
                        model.Scenic_id = Scenic_id; 
                        List<string> listsql = new List<string>(); 
                        string order_sn = "G" + Common.Utility.GetDataRandom();
                        int scenic_id = Scenic_id;
                        int user_id = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where openid='" + model.openid + "'").ToString());
                        int add_time = getSecondEnd(DateTime.Now);
                        string is_pay = "1";
                        int pay_type = 3;
                        object _c = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, string.Format("select count(1) from zy_scenic_orders where scenic_id={0} and is_pay='1' and user_id={1}", scenic_id, user_id));
                        if (DBUtility.Model.DbValue.GetInt(_c.ToString()) > 0)
                        {
                            Response.Write("<script>alert('您已购买请不要重复购买！');location.href='/index.aspx';</script>");
                        }
                        else
                        {
                            listsql.Add("insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type)values('" + order_sn + "'," + scenic_id + "," + user_id + "," + add_time + ",'" + is_pay + "'," + money + "," + pay_type + ")");
                            listsql.Add("update zy_users set user_money=user_money-" + model.money + " where openid='" + model.openid + "'");
                            int i = DBUtility.DALHelper.myHelper.ExecuteNonQuery(listsql);
                            string MapHtml = "/MapHtml.aspx?id=" + t;
                            if (i > 0)
                                Response.Write("<script>alert('购买成功！');location.href='" + MapHtml + "';</script>");
                            else
                                Response.Write("<script>alert('购买失败！');</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('购买失败，您的余额不足！');</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('购买失败，请重试！');</script>");
                } 
               
            }
        }
    }
}