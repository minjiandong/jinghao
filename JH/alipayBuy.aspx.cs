using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class alipayBuy : System.Web.UI.Page
    {
        public string _title, _content, _url, _yue, cost_money;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + Scenic_id + "");
            if (_dt.Rows.Count > 0)
            {
                _title = _dt.Rows[0]["scenic_name"].ToString();
                _content = jq(_dt.Rows[0]["scenics_text"].ToString());
                _url = "https://cloud.jhlxw.com/app" + _dt.Rows[0]["scenic_img"].ToString();
                cost_money = _dt.Rows[0]["cost_money"].ToString();
            }
        }
        public int Scenic_id
        {
            get
            {
                return DBUtility.Model.DbValue.GetInt(Request["Scenic_id"] ?? "");
            }
        }
        string jq(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                if (val.Length >= 200)
                    return val.Substring(0, 200) + "...";
                else
                    return val;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}