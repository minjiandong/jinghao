using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace JH
{
    public partial class notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                //商户订单号
                string out_trade_no = Request.Form["out_trade_no"];
                //支付宝交易号
                string trade_no = Request.Form["trade_no"];
                DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenic_orders where out_trade_no='" + out_trade_no + "'");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["is_pay"].ToString() == "0")//未支付
                    {
                        int userid = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where mobile='" + dt.Rows[0]["mobile"].ToString() + "'").ToString());
                        string strsql = string.Format("update zy_scenic_orders set user_id={0},is_pay=1 where out_trade_no='{1}'", userid, out_trade_no);
                        DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, strsql);
                        Response.Cookies["_usercode"].Value = userid.ToString();
                        Response.Cookies["_usercode"].Expires = DateTime.Now.AddDays(30);
                    }
                }
            }
            else
            {
                Response.Write("无通知参数");
            }
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}