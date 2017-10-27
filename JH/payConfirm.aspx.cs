using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using JH.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace JH
{
    public partial class payConfirm : System.Web.UI.Page
    {
        public string totalCount, price, totalPrice, userName, title, account, scenicid;
        protected void Page_Load(object sender, EventArgs e)
        {
            totalCount = Request["totalCount"] ?? "";//数量
            price = Request["price"] ?? "";//单价
            totalPrice = Request["totalPrice"] ?? "";//总价
            userName = Request["userName"] ?? "";
            title = Request["title"] ?? "";
            account = Request["account"] ?? "";//手机号码
            scenicid = Request["scenicid"] ?? "";//景区ID
        }

        protected void BtnAlipay_Click(object sender, EventArgs e)
        { 
            try
            {
                object _c = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, string.Format("select count(1) from zy_scenic_orders where scenic_id={0} and is_pay='1' and mobile='{1}'", DBUtility.Model.DbValue.GetInt(scenicid), account));
                if (DBUtility.Model.DbValue.GetInt(_c) > 0)
                {
                    int userid = DBUtility.Model.DbValue.GetInt(DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select user_id from zy_users where mobile='" + account + "'").ToString());
                    Response.Cookies["_usercode"].Value = userid.ToString();
                    Response.Cookies["_usercode"].Expires = DateTime.Now.AddDays(30);
                    Response.Write("<script>alert('您已购买请不要重复购买！');location.href='/alipayHtml.aspx?id=" + scenicid + "';</script>");
                }

                string APPID = System.Configuration.ConfigurationManager.AppSettings["APP_ID"].ToString();//APPID即创建应用后生成
                string APP_PRIVATE_KEY = System.Configuration.ConfigurationManager.AppSettings["APP_PRIVATE_KEY"].ToString();//开发者应用私钥，由开发者自己生成
                string ALIPAY_PUBLIC_KEY = System.Configuration.ConfigurationManager.AppSettings["ALIPAY_PUBLIC_KEY"].ToString();//支付宝公钥，由支付宝生成
                string CHARSET = "UTF-8";//编码格式
                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
                AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
                request.SetReturnUrl("https://cloud.jhlxw.com/return_url.aspx");
                request.SetNotifyUrl("https://cloud.jhlxw.com/notify_url.aspx");
                string out_trade_no = Common.Utility.GetDataRandom();
                request.BizContent = "{" +
                "    \"body\":\"" + title + "景区智能导游授权\"," +
                "    \"subject\":\"" + title + "授权\"," +
                "    \"out_trade_no\":\"" + out_trade_no + "\"," +
                "    \"timeout_express\":\"90m\"," +
                "    \"total_amount\":" + totalPrice + "," +
                "    \"product_code\":\"QUICK_WAP_WAY\"" +
                "  }";
                AlipayTradeWapPayResponse response = client.pageExecute(request);
                Response.Write("<div style=\"display:none;\">" + response.Body + "</div>");
                if (!response.IsError)
                {
                    List<string> listsql = new List<string>();
                    object obj = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select count(1) from zy_users where mobile='" + account + "'");
                    if (DBUtility.Model.DbValue.GetInt(obj) == 0)
                    {
                        listsql.Add(string.Format("insert into zy_users(reg_time,mobile,oauth,nickname)values(UNIX_TIMESTAMP(SYSDATE()),'{0}','Mobile','{1}')", account, userName));
                    }
                    listsql.Add(string.Format("insert into zy_scenic_orders(order_sn,scenic_id,user_id,add_time,is_pay,money,pay_type,out_trade_no,mobile)values('{0}',{1},-1,UNIX_TIMESTAMP(SYSDATE()),0,{2},2,'{3}','{4}')", Common.Utility.GetDataRandom(), DBUtility.Model.DbValue.GetInt(scenicid), DBUtility.Model.DbValue.GetDouble(totalPrice), out_trade_no, account));
                    int rets = DBUtility.DALHelper.myHelper.ExecuteNonQuery(listsql);
                    if (!(rets > 0))
                    {
                        Response.Write("创建订单失败！");
                        return;
                    }
                }
                else
                {
                    Response.Write(response.Msg);
                }
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}