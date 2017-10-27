using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH.Account.FunctionMenu
{
    public partial class editPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Model.JH_SYS_USER user = SetCookie.GetUserInfo();
                if (user != null)
                {
                    gysname = user.FullName;
                    Phone = user.Phone;
                    if (user.UserType == "1")
                    {
                        gys = "供应商名称"; 
                    }
                    else
                    {
                        gys = "姓名";
                    }
                }
            }
        }
        protected string gysname { get; set; }
        protected string Phone { get; set; }
        protected string gys { get; set; }
    }
}