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
    public partial class MapPage : System.Web.UI.Page
    {

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["_usercode"] != null)
            {
                usercode = Request.Cookies["_usercode"].Value;
            }
        }
        public string usercode { get; set; }
       
        public string city {
            get
            {
                string citystring = Request["city"] ?? "";
                if (string.IsNullOrWhiteSpace(citystring))
                    return string.Empty;
                if (citystring.Length > 4)
                    return citystring.Substring(0, 4) + "...";
                else
                    return citystring;
            }
        }
    }
}