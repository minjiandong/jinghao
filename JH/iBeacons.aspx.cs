using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class iBeacons : System.Web.UI.Page
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
        }
    }
}