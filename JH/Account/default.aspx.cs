using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH.Account
{
    public partial class _default : System.Web.UI.Page
    {
        public string jurisdictionId { get; set; }
        public List<Model.JH_FUNCTION> FunctionAll = new List<Model.JH_FUNCTION>();
        public string system_tree { get; set; }
        public string FunctionMenu_tree { get; set; }
        public string userId {get; set; }
        public string fullname { get; set; }
        public bool uppass { get; set; }
        DataTable dts;
        protected void Page_Load(object sender, EventArgs e)
        {
            Model.JH_SYS_USER user = SetCookie.GetUserInfo();
            if (user != null)
            {
                userId = user.UserID.ToString();
                //获取用户所属权限组
                Model.JH_User_JurisdictionGroup userGroup = new Model.JH_User_JurisdictionGroup();
                userGroup.USERID = userId;
                userGroup = DBUtility.ORM.Get(userGroup) as Model.JH_User_JurisdictionGroup;
                fullname = user.FullName;
                //jurisdictionId = userGroup.JURISDICTIONGROUPID;
               
                //FunctionAll = Repository.BaseBll<Model.JH_FUNCTION>.GetList(" exists (select JURISDICTIONOBJECTID from JH_JURISDICTION where JURISDICTIONGROUPID='" + jurisdictionId + "' and OBJECTTYPE='function' AND LOCATE(FunctionID,JURISDICTIONOBJECTID)>0) order by int(Sort) asc");

                //string strsql = string.Format(@"select * from JH_Function where exists (select JURISDICTIONOBJECTID from JH_JURISDICTION where  OBJECTTYPE='function') order by convert(int,Sort) asc", jurisdictionId);//AND LOCATE(FunctionID,JURISDICTIONOBJECTID)>0  JURISDICTIONGROUPID='{0}' and

                if (user.UserType == "1")
                {
                    string strsql = string.Format("select * from JH_Function where ISENABLE='1' and FUNCTIONID IN('528DFEB1B6294C33C2194282220E8A60','424B826E02BA994DC5197324C9BFDBC2') order by convert(int,Sort) asc");
                    dts = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, strsql);
                    FunctionMenu_tree = html_Tree("FunctionMenu", 1, "0", 30);
                    system_tree = html_Tree("SystemMenu", 1, "0", 30);
                    uppass = true;
                }
                else
                {
                    string strsql = string.Format("select * from JH_Function order by convert(int,Sort) asc");
                    dts = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, strsql);
                    FunctionMenu_tree = html_Tree("FunctionMenu", 1, "0", 30);
                    system_tree = html_Tree("SystemMenu", 1, "0", 30);
                    uppass = false;
                }
                
            }
        }

        public string html_Tree(string MenuType, int count, string SuperiorID, int listcount)
        {
            StringBuilder tr = new StringBuilder();
            DataView dv = new DataView(dts);
            dv.RowFilter = "MenuType='" + MenuType + "' and SuperiorID='" + SuperiorID + "'";
            DataTable dt = dv.ToTable();
            int icount = count;
            int f = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                f += 1;
                if (icount == 1)
                {
                    //第一级
                    tr.Append("<li class=\"treeview\" id=\"md_" + f + "\">");
                    tr.Append("<div class=\"oneMenuTitle\">");
                    tr.AppendFormat("<span class=\"oneMenuIconWarp\"><i class=\"{0}\"></i></span>", dt.Rows[i]["icon"].ToString());
                    tr.AppendFormat("<a onclick=\"openTab('{0}','{1}','{2}');\"><b>{3}</b></a>", dt.Rows[i]["FunctionID"].ToString(), dt.Rows[i]["FunctionName"].ToString(), dt.Rows[i]["Code"].ToString(), dt.Rows[i]["FunctionName"]);
                    tr.Append("<i class=\"arrowRightIcon fr\"></i>");
                    tr.Append("</div>");
                    tr.Append("<ul class=\"clearfix towMenu\" data-state=\"0\">");
                    tr.Append(html_Tree(MenuType, icount + 1, dt.Rows[i]["FunctionID"].ToString(), listcount));
                    tr.Append("</ul>");
                    tr.Append("</li>");
                }
                if (icount == 2)
                {
                    //第二级
                    tr.Append("<li class=\"towMenuStyle\">");
                    tr.AppendFormat("<div class=\"towMenuTitle\"><i class=\"arrowRightIcon\"></i><span onclick=\"openTab('{0}','{1}','{2}');\"><b>{3}</b></span></div>", dt.Rows[i]["FunctionID"].ToString(), dt.Rows[i]["FunctionName"].ToString(), dt.Rows[i]["Code"], dt.Rows[i]["FunctionName"]);

                    tr.Append("<ul class=\"clearfix threeMenu\" data-state=\"0\">");
                    tr.Append(html_Tree(MenuType, icount + 1, dt.Rows[i]["FunctionID"].ToString(), listcount));
                    tr.Append("</ul>");
                    tr.Append("</li>");
                }
                if (icount > 2)
                {
                    tr.Append("<li class=\"threeMenuStyle\">");
                    tr.Append("<div class=\"threeMenuTitle\">");
                    tr.AppendFormat(" <i class=\"blackTriangleIcon\" style=\"margin-left: " + (listcount + 20) + "px;\"></i><a onclick=\"openTab('{0}','{1}','{2}');\"><b>{3}</b></a>", dt.Rows[i]["FunctionID"], dt.Rows[i]["FunctionName"], dt.Rows[i]["Code"], dt.Rows[i]["FunctionName"]);
                    tr.Append("</div>");
                    tr.Append("<ul class=\"clearfix threeMenu\" data-state=\"0\">");
                    tr.Append(html_Tree(MenuType, icount + 1, dt.Rows[i]["FunctionID"].ToString(), listcount + 20));
                    tr.Append("</ul>");
                    tr.Append("</li>");
                }
            }

            return tr.ToString();
        }
    }
}