using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class Order : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["_usercode"] == null)
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6fb51a67c530c955&redirect_uri=http://cloud.jhlxw.com/userbind.aspx&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect");
                return;
            }
         
            openid = Request.Cookies["_usercode"].Value;
             
        }
        public string openid
        {
            get;
            set;
        }
        protected string s(string val)
        {
            switch (val)
            {
                case "1":
                    return "微信";
                case "2":
                    return "支付宝";
                case "3":
                    return "余额";
                case "4":
                    return "激活码";
                default:
                    return "未知";
            }
             
        }
        protected string gettime(string val)
        {
            double miao = DBUtility.Model.DbValue.GetDouble(val);
            DateTime s = new DateTime(1970, 1, 1, 8, 0, 0);
            s = s.AddSeconds(miao);
            return s.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}