﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.JH_MarkersList m = Repository.BaseBll<Model.JH_MarkersList>.Get(new Model.JH_MarkersList() { id = id });
            if (m != null)
            {
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
            theString = theString.Replace("&amp;nbsp;"," ");
            return theString;
        }
        public string content { get; set; }
        public int id
        { get
            {
                return DBUtility.Model.DbValue.GetInt(Request["id"] ?? "");
            }
        }
    }
}