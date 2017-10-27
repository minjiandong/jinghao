using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class showdetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.JH_ScenicSpot m = Repository.BaseBll<Model.JH_ScenicSpot>.Get(new Model.JH_ScenicSpot() { id = id });
            if (m != null)
            {
                n_title = m.MapName;
                content = HtmlDiscode(m.detailed);
            }
        }
        ///<summary>
        ///恢复html中的特殊字符
        ///</summary>
        ///<paramname="theString">需要恢复的文本。</param>
        ///<returns>恢复好的文本。</returns>
        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/>", "\n");
            theString = theString.Replace("&amp;nbsp;", " ");
            return theString;
        }
        protected string n_title { get; set; }
        protected string content { get; set; }
        protected int id
        {
            get
            {
                return DBUtility.Model.DbValue.GetInt(Request["id"] ?? "");
            }
        }
    }
}