using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// ScenicSpotUser 的摘要说明
    /// </summary>
    public class ScenicSpotUser : IHttpHandler
    {
        JH.Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string mt = context.Request["p"] ?? "";
                switch (mt)
                {
                    case "list":
                        list(context);
                        break;
                    default:
                        json.info = "非法操作";
                        log.Sys_log.Error(json.info);
                        context.Response.Write(JsonConvert.SerializeObject(json));
                        break;
                }
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                log.Sys_log.Error(ex.Message);
            }
            finally {
                context.Response.End();
            }
            
        }
        void list(HttpContext c)
        {
            StringBuilder tr = new StringBuilder();
            string nickname = c.Request["nickname"] ?? "";
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                tr.AppendFormat(" and nickname like '%{0}%'", nickname);
            }
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            Model.JH_USERS model = new Model.JH_USERS();
            List<object> m = DBUtility.ORM.GetList(model, pagesize, page, out totalCount, string.Format("1=1 {0} order by {1} {2}", tr.ToString(), sortname, sortorder));
            var griddata = new { Rows = m, Total = totalCount };
            c.Response.Write(JsonConvert.SerializeObject(griddata));
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