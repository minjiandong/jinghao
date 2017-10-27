using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;

namespace JH
{
    public partial class buyHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + DBUtility.Model.DbValue.GetInt(id) + ""); 
            if (_dt.Rows.Count > 0)
            {
                string openid = Request["openid"] ?? "";
                string order_sn = "G" + Common.Utility.GetDataRandom();
                int scenic_id = DBUtility.Model.DbValue.GetInt(id);
                int user_id = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where openid='" + openid + "'").ToString());
                int add_time = getSecondEnd(DateTime.Now);
                string is_pay = "1";
                int pay_type = 1;
                string strsql = "insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type)values('" + order_sn + "'," + scenic_id + "," + user_id + "," + add_time + ",'" + is_pay + "'," + DBUtility.Model.DbValue.GetDecimal(_dt.Rows[0]["cost_money"].ToString()) + "," + pay_type + ")";
                int ret = DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, strsql);
                if (ret > 0)
                {
                    if (string.IsNullOrWhiteSpace(_dt.Rows[0]["MapImageUrl"].ToString()))
                        Response.Write(JsonConvert.SerializeObject(new {
                            state = "ok",
                            url = "MapHtml",
                            id = id
                        }));
                    else
                        Response.Write(JsonConvert.SerializeObject(new
                        {
                            state = "ok",
                            url = "MapTest",
                            id = id
                        }));
                }
                else
                {
                    Response.Write(JsonConvert.SerializeObject(new
                    {
                        state = "no",
                        url = "index",
                        id = ""
                    }));
                } 
            }
        }
        /// <summary>
        /// 将结束时间转换成INT型
        /// </summary>
        /// <param name="end">结束时间</param>
        /// <returns>int值</returns>
        private int getSecondEnd(DateTime end)
        {
            int result = 0;
            DateTime startdate = new DateTime(1970, 1, 1, 8, 0, 0);
            TimeSpan seconds = end.AddDays(0) - startdate;
            result = Convert.ToInt32(seconds.TotalSeconds);
            return result;
        }
        protected string id
        {
            get
            {
                return Request["id"] ?? "";
            }
        }
    }
}