using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JH
{
    public partial class return_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                //商户订单号 
                string out_trade_no = Request.QueryString["out_trade_no"];
                //支付宝交易号 
                string trade_no = Request.QueryString["trade_no"];

                
                DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenic_orders where out_trade_no='" + out_trade_no + "'");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["is_pay"].ToString() == "0")//未支付
                    {
                        int userid = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where mobile='" + dt.Rows[0]["mobile"].ToString() + "'").ToString());
                        string strsql = string.Format("update zy_scenic_orders set user_id={0},is_pay=1 where out_trade_no='{1}'", userid, out_trade_no);
                        int i = DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, strsql);
                        if (i > 0)
                        {
                            Response.Cookies["_usercode"].Value = userid.ToString();
                            Response.Cookies["_usercode"].Expires = DateTime.Now.AddDays(30);
                            Response.Redirect("alipayHtml.aspx?id=" + dt.Rows[0]["scenic_id"].ToString()); 
                        }
                        else
                        {
                            Response.Redirect("/index");
                        } 
                    }
                    else
                    {
                        Response.Redirect("alipayHtml.aspx?id=" + dt.Rows[0]["scenic_id"].ToString());
                    }
                }
                else
                {
                    Response.Redirect("/index");
                }
                

                //https://cloud.jhlxw.com/return_url.aspx?total_amount=0.01&timestamp=2017-06-24+17%3A40%3A36&sign=Nwe3SvAsqtsiZvLtv0rVJ%2FN6C9GX4zGz5Mg9wfsNrtFp5ZFttcC4rGnEHwb5k%2FPe7zR92l7qkdICl8wQYlAw5TL4nrRKvLq3ejpUz6OuE%2F5k7YaO0NpNwxjuKdeoSYJOdAY9aMuMHwQqrRu3qOg42oohSCbbpmUjjYcIpSHJHKC2wnhflFfyO%2FSqBPfnRMLg%2BSbj2P%2B%2FWoUextvCh1nKaRZRYV6kqhk2OJKuVDm1TMaMpcULrlipw7oNhV88cBBHuyxbjTU7woIk3Y8W0LfARG2E%2FckBPep16c0ZQ53YiyjYxBp6kLkjKUw2m8ovUMZpHc04DDbm2k7i1PSoAx97ig%3D%3D&trade_no=2017062421001004440292420429&sign_type=RSA2&auth_app_id=2016011101083104&charset=UTF-8&seller_id=2088021778116807&method=alipay.trade.wap.pay.return&app_id=2016011101083104&out_trade_no=20170624174009&version=1.0
            }
            else//验证失败
            {
                Response.Write("验证失败");
            }
        }
        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}