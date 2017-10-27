using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class show : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.JH_NEWS m = Repository.BaseBll<Model.JH_NEWS>.Get(new Model.JH_NEWS() { id = id });
            if (m != null)
            {
                content = m.n_content; 
                n_title = m.n_title;
            }
        }
        public string n_title { get; set; }
        public string content { get; set; }
        public int id
        {
            get
            {
                return DBUtility.Model.DbValue.GetInt(Request["id"] ?? "");
            }
        }
    }
}