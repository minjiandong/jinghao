using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JH
{
    public partial class user : System.Web.UI.Page
    {
        protected string headimgurl = string.Empty;
        protected string nickname = string.Empty;
        protected decimal balance = 0.00m; 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["_usercode"] == null)
                {
                    Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6fb51a67c530c955&redirect_uri=http://cloud.jhlxw.com/userbind.aspx&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect");
                    return;
                }

                string openid = Request.Cookies["_usercode"].Value;
                DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_users where openid='" + openid + "'");
                if (dt.Rows.Count > 0)
                {
                    nickname = dt.Rows[0]["nickname"].ToString();
                    headimgurl = dt.Rows[0]["head_pic"].ToString();
                    balance = DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["user_money"].ToString());
                }
                if (Request.Cookies["openid"] == null)
                    return;
                string _openid = Request.Cookies["openid"].Value;
                
                string AccessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.GetToken(Common.Utility.AppID);
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", AccessToken, _openid);
                string json = JH.App_Start.HttpUtility.GetData(url);
                JObject jo1 = JsonConvert.DeserializeObject(json) as JObject;
                headimgurl = jo1["headimgurl"].ToString();
               
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            } 
        }
    }
}