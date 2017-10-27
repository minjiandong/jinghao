using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using Newtonsoft.Json.Linq;

namespace JH
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  
            AccessTokenContainer.Register(Common.Utility.AppID, Common.Utility.AppSecret);
        } 

        protected void Button1_Click1(object sender, EventArgs e)
        {
            string exectype = DropDownList1.SelectedValue;
            string json = string.Empty;
            string url = string.Empty;
            if (exectype == "add")
            {
                json = "{\"group_name\":\"" + group_name.Text.Trim() + "\"}"; 
                url = "https://api.weixin.qq.com/shakearound/device/group/add";
            }
            else if (exectype == "edit")
            {
                json = "{ \"group_id\":" + group_id.Text.Trim() + ",\"group_name\":\"" + group_name.Text.Trim() + "\"}";
                url = "https://api.weixin.qq.com/shakearound/device/group/update";
            }
            else
            {
                json = "{\"group_id\":" + group_id.Text.Trim() + "}";
                url = "https://api.weixin.qq.com/shakearound/device/group/delete";
            }
            string AccessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.GetToken(Common.Utility.AppID);
            string str = JH.App_Start.HttpUtility.SendHttpRequest(url + "?access_token=" + AccessToken, json);
            Response.Write(str);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string exectype = DropDownList2.SelectedValue;
            string json = string.Empty;
            string url = string.Empty;
            if (exectype == "add")
            {
                json = "{\"group_id\": " + _group_id.Text.Trim() + ",\"device_identifiers\":[{\"device_id\":\"\",\"uuid\":\"" + uuid.Text.Trim() + "\",\"major\":" + major.Text.Trim() + ",\"minor\":" + minor.Text.Trim() + "	}]}";
                url = "https://api.weixin.qq.com/shakearound/device/group/adddevice"; 
            }
            else
            {
                json = "{\"group_id\": " + _group_id.Text.Trim() + ",\"device_identifiers\":[{\"device_id\":" + _device_id.Text.Trim() + ",	\"uuid\":\"" + uuid.Text.Trim() + "\",\"major\":" + major.Text.Trim() + ",\"minor\":" + minor.Text.Trim() + "	}]}";
                url = "https://api.weixin.qq.com/shakearound/device/group/deletedevice";
            }
            string AccessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.GetToken(Common.Utility.AppID);
            string str = JH.App_Start.HttpUtility.SendHttpRequest(url + "?access_token=" + AccessToken, json);
            Response.Write(str);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string json = "{\"begin\": 0,\"count\": 100}";
            string url = "https://api.weixin.qq.com/shakearound/device/group/getlist";
            
            string AccessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.GetToken(Common.Utility.AppID);
            string str = JH.App_Start.HttpUtility.SendHttpRequest(url + "?access_token=" + AccessToken, json);
            JObject jo = JsonConvert.DeserializeObject(str) as JObject;
            //GridView1.DataSource = jo["data"]["groups"];
            //GridView1.DataBind();
            DataTable dt = new DataTable();
            dt.Columns.Add("group_id", typeof(string));
            dt.Columns.Add("group_name",typeof(string)); 
            foreach (var item in jo["data"]["groups"])
            {
                DataRow dr = dt.NewRow();
                dr["group_id"] = item["group_id"].ToString();
                dr["group_name"] = item["group_name"].ToString();
                dt.Rows.Add(dr);
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();  
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string json = "{\"group_id\":" + _groupid.Text.Trim() + ",\"begin\": 0,\"count\": 1000}";
            string url = "https://api.weixin.qq.com/shakearound/device/group/getdetail";

            string AccessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.GetToken(Common.Utility.AppID);
            string str = JH.App_Start.HttpUtility.SendHttpRequest(url + "?access_token=" + AccessToken, json);
            JObject jo = JsonConvert.DeserializeObject(str) as JObject; 
            DataTable dt = new DataTable();
            dt.Columns.Add("device_id", typeof(string));
            dt.Columns.Add("uuid", typeof(string));
            dt.Columns.Add("major", typeof(string));
            dt.Columns.Add("minor", typeof(string));
            dt.Columns.Add("comment", typeof(string));
            dt.Columns.Add("poi_id", typeof(string));
          
            foreach (var item in jo["data"]["devices"])
            {
                DataRow dr = dt.NewRow();
                dr["device_id"] = item["device_id"].ToString();
                dr["uuid"] = item["uuid"].ToString();
                dr["major"] = item["major"].ToString();
                dr["minor"] = item["minor"].ToString();
                dr["comment"] = item["comment"].ToString();
                dr["poi_id"] = item["poi_id"].ToString();
                dt.Rows.Add(dr);
            }
            GridView2.DataSource = dt;
            GridView2.DataBind();  
        }
    }
}