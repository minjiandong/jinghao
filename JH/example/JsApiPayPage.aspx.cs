using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Threading;
using LitJson;
using System.Web.Security;
using WxPayAPI;
using System.Data;

namespace JH.example
{
    public partial class JsApiPayPage : System.Web.UI.Page
    {
        public static string wxJsApiParam {get;set;} //H5调起JS API参数
        public string total_fee
        {
            get
            {
                return Request.QueryString["total_fee"] ?? "";
            }
        }
        public string Scenic_id
        {
            get
            {
                return Request.QueryString["Scenic_id"] ?? "";
            }
        }
        public string _title
        {
            get;
            set;
        }
        public string _money { get; set; }
        public string openid { get; set; }
        public string unionid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
            {
                openid = Request.QueryString["openid"];
                unionid = Request.QueryString["unionid"];
                //检测是否给当前页面传递了相关参数
                if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(total_fee))
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                    Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit..."); 
                    return;
                }

                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                jsApiPay.openid = openid;
                jsApiPay.unionid = unionid;
                jsApiPay.total_fee = int.Parse(total_fee);

                //JSAPI支付预处理
                try
                {
                    string body = string.Empty;
                    DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + DBUtility.Model.DbValue.GetInt(Scenic_id) + "");
                    if (_dt.Rows.Count > 0)
                    {
                        _title = _dt.Rows[0]["scenic_name"].ToString();
                        _money = _dt.Rows[0]["cost_money"].ToString();
                        body = "购买景区:" + _dt.Rows[0]["scenic_name"].ToString();
                    }
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(body);
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数         



                    
                    //Model.JH_ScenicSpot m = Repository.BaseBll<Model.JH_ScenicSpot>.Get(new Model.JH_ScenicSpot() { id = DBUtility.Model.DbValue.GetInt(Scenic_id) });
                    //if (m != null)
                    //{
                    //    _title = m.MapName;
                    //    _money = m.Monetary.ToString();
                    //    body = "购买景区:" + m.MapName;
                    //}
                    //WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(body);
                    //wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    

                }
                catch(Exception ex)
                {
                    Response.Write("<script>alert('抱歉当前信号弱或者服务器繁忙请重试！');</script>"); 
                }
            }
        }
    }
}