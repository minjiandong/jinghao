using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JH
{
    public partial class userbind : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckHasBindUser();
            }
        }
        protected string openid = string.Empty;
        protected string debugmsg = string.Empty;
        protected string nickname = string.Empty;
        protected string headimgurl = string.Empty; 
        /// <summary>
        /// 绑定检查
        /// </summary>
        public void CheckHasBindUser()
        {
            string strCode = Request["code"] ?? "";
            string errmsg = string.Empty;
            try
            { 
                if (!string.IsNullOrEmpty(strCode))
                {
                    string _unionid = string.Empty;
                    string appid = Common.Utility.AppID;
                    string appsecret = Common.Utility.AppSecret;
                    
                    Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result =
                    Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthApi.GetAccessToken(appid, appsecret, strCode, "authorization_code");
                    if (result.errcode == Senparc.Weixin.ReturnCode.请求成功)
                    {
                        openid = result.openid;
                        openid1.Value = openid;
                        string resul = JH.App_Start.HttpUtility.GetData(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, appsecret)); 
                        if (string.IsNullOrWhiteSpace(resul))
                        {
                            Response.Write("获取access_token失败！");
                            return;
                        }
                        JObject jo = JsonConvert.DeserializeObject(resul) as JObject; 
                        string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", jo["access_token"].ToString(), openid); 
                        string json = JH.App_Start.HttpUtility.GetData(url);
                        if (string.IsNullOrWhiteSpace(json))
                        {
                            Response.Write("获取微信用户信息失败！");
                            return;
                        }  
                        JObject jo1 = JsonConvert.DeserializeObject(json) as JObject; 
                        if (jo1["subscribe"].ToString() == "1")
                        {
                            _unionid = jo1["unionid"].ToString();
                            Response.Cookies["_usercode"].Value = _unionid;
                            Response.Cookies["_usercode"].Expires = DateTime.Now.AddDays(7);
                            Response.Cookies["openid"].Value = jo1["openid"].ToString();
                            Response.Cookies["openid"].Expires = DateTime.Now.AddDays(7);
                            if (isbind(_unionid))
                            {
                                DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, string.Format("update zy_users set head_pic='{0}' where openid='{1}'", jo1["headimgurl"].ToString(), _unionid));
                                Response.Redirect("index", true);
                            }
                            else
                            {
                                nickname = jo1["nickname"].ToString();
                                nickname1.Value = nickname;
                                headimgurl = jo1["headimgurl"].ToString();
                                headimgurl1.Value = headimgurl;
                                sex.Value = jo1["sex"].ToString();
                                language.Value = jo1["language"].ToString();
                                city.Value = jo1["city"].ToString();
                                province.Value = jo1["province"].ToString();
                                country.Value = jo1["country"].ToString();
                                subscribe_time.Value = jo1["subscribe_time"].ToString();
                                unionid.Value = _unionid;
                            }
                        }
                        else
                        {
                            Response.Redirect("https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MzAxNzQ3ODgzNg==#wechat_redirect");
                        }
                        
                    }
                    else
                    {
                        debugmsg = result.errmsg;
                        Button1.Enabled = false;
                        Response.Write("微信请求："+debugmsg);
                    }
                }
                else
                {
                    Literal1.Text = "<span style='color:#FFFFFF;'>获取信息失败，请重试！</span>";
                }
            }
            catch (Exception ex)
            {
                debugmsg = ex.Message;
                Button1.Enabled = false;
                Response.Write("错误提示：" + debugmsg + ";" + errmsg);
            }
        }
        /// <summary>
        /// 判断用户是否绑定
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        bool isbind(string openid)
        {
            object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select count(1) from zy_users where openid='" + openid + "'"); 
            if (DBUtility.Model.DbValue.GetInt(obj) == 1)
                return true;
            else
                return false;
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
            TimeSpan seconds = end.AddDays(1) - startdate;
            result = Convert.ToInt32(seconds.TotalSeconds);
            return result;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Model.JH_USERS m = new Model.JH_USERS();
            m.language = language.Value;
            m.nickname = nickname1.Value;
            m.openid = unionid.Value;
            m.province = province.Value;
            m.sex = sex.Value;
            m.subscribe_time = subscribe_time.Value;
            m.city = city.Value;
            m.country = country.Value;
            m.headimgurl = headimgurl1.Value;
            m.unionid = unionid.Value;
            int reg_time = DBUtility.Model.DbValue.GetInt(m.subscribe_time);
            //object obj = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select count(1) from JH_USERS WHERE OPENID='" + m.openid + "'");
            object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select count(1) from zy_users WHERE OPENID='" + m.openid + "'");
            if (DBUtility.Model.DbValue.GetInt(obj) <= 0)
            {
                string sqlstring = string.Format("insert into zy_users(reg_time,oauth,openid,head_pic,nickname,pid,sex)values({0},'Wechat','{1}','{2}','{3}',0,{4})", reg_time, m.unionid, m.headimgurl, m.nickname,m.sex); 
                int tr = DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, sqlstring); //Repository.BaseBll<Model.JH_USERS>.Add(m);
                if (tr > 0)
                {
                    Response.Cookies["_usercode"].Value = m.openid;
                    Response.Cookies["_usercode"].Expires = DateTime.Now.AddDays(7);
                    Response.Redirect("/index", true);
                }
                else
                {
                    Literal1.Text = "<script>alert('绑定用户失败！');</script>";
                }
            }
            else
            {
                Literal1.Text = "<script>alert('你已绑定！');location.href='/index';</script>";
            }
        }
    }
}