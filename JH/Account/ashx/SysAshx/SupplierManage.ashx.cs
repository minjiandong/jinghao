using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// SupplierManage 的摘要说明
    /// </summary>
    public class SupplierManage : IHttpHandler
    {
        Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string mt = context.Request["p"] ?? "";
                switch (mt)
                {
                    case "ligerList":
                        ligerList(context);
                        break;
                    case "add":
                        add(context);
                        break;
                    case "getsupp":
                        getsupp(context);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }
        void ligerList(HttpContext c)
        { 
            string sql = "select id,mapname as text from JH_ScenicSpot order by mapname desc";
            DataTable dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text,sql);
            var griddata = new { Rows = dt, Total = dt.Rows.Count };
            c.Response.Write(JsonConvert.SerializeObject(griddata)); 
        }
        void add(HttpContext c) 
        {
            string userid = c.Request["userid"] ?? "";
            string SupplierID = c.Request["SupplierID"] ?? "";
            string[] array = SupplierID.Split(';');
            DBUtility.DALHelper.dbHelper.ExecuteNonQuery(CommandType.Text, "delete from JH_ScenicList where User_id=" + DBUtility.Model.DbValue.GetInt(userid) + "");
            foreach (string item in array)
            {
                string strsql = "insert into JH_ScenicList(User_id,Scenicid)values(" + DBUtility.Model.DbValue.GetInt(userid) + "," + DBUtility.Model.DbValue.GetInt(item) + ")";
                DBUtility.DALHelper.dbHelper.ExecuteNonQuery(CommandType.Text,strsql);
            }
            json.info = "ok";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void getsupp(HttpContext c) 
        {
            string userid = c.Request["userid"] ?? "";
            string sql = "select b.MapName,a.Scenicid from JH_ScenicList as a join  JH_ScenicSpot as b on b.id=a.Scenicid  where a.user_id=" + DBUtility.Model.DbValue.GetInt(userid) + "";
            DataTable dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text,sql);
            string id = string.Empty;
            string text = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
			{
                id += dt.Rows[i]["Scenicid"].ToString();
                if ((dt.Rows.Count - 1) > i)
                    id += ";";
                text += dt.Rows[i]["mapname"].ToString();
                if ((dt.Rows.Count - 1) > i)
                    text += ";";
			}
            var list = new
            {
                id = id,
                text = text
            };
            //string _sql = "select id,mapname as text from JH_ScenicSpot order by mapname desc";
            //DataTable _dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, _sql); 
            //var griddata = new { Rows = _dt, Total = dt.Rows.Count };
            //var listinfo = new
            //{
            //    griddata = griddata,
            //    list = list
            //};
            c.Response.Write(JsonConvert.SerializeObject(list));
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}