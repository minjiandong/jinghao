using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Data;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// ActivationCard 的摘要说明
    /// </summary>
    public class ActivationCard : IHttpHandler
    {
        JH.Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string p = context.Request["p"] ?? "";
                switch (p)
                {
                    case "list":
                        list(context);
                        break;
                    case "add":
                        add(context);
                        break;
                    case "Release":
                        Release(context);
                        break;
                    case "delete":
                        delete(context);
                        break;
                    default:
                        json.info = "操作失败-001";
                        context.Response.Write(JsonConvert.SerializeObject(json));
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Sys_log.Error(ex.Message);
                json.info = ex.Message;
                context.Response.Write(JsonConvert.SerializeObject(json));
            }
        }
        void delete(HttpContext c)
        {
            string IDArror = c.Request["IDArror"] ?? "";
            string[] arr = IDArror.Split(',');
            int i = 0;
            foreach (var item in arr)
            {
                int id = DBUtility.Model.DbValue.GetInt(item);
                object counts = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select count(1) from JH_ActivationCard where isRelease in('0','2') and id=" + id + "");
                if (int.Parse(counts.ToString()) == 1)
                {
                    bool t = Repository.BaseBll<Model.JH_ActivationCard>.Remove(new Model.JH_ActivationCard() { id = id });
                    if (t)
                        i += 1;
                }
            }
            json.type = "ok";
            json.info = "成功删除" + i + "条信息！";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void Release(HttpContext c)
        {
            string IDArror = c.Request["IDArror"] ?? "";
            string[] arr = IDArror.Split(',');
            int i = 0;
            foreach (var item in arr)
            {
                int id = DBUtility.Model.DbValue.GetInt(item);
                object counts = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, "select count(1) from JH_ActivationCard where isRelease='0' and id="+id+"");
                if (int.Parse(counts.ToString()) == 1)
                {
                    Model.JH_ActivationCard m = Repository.BaseBll<Model.JH_ActivationCard>.Get(new Model.JH_ActivationCard { id = id });
                    m.isRelease = "1";
                    m.ReleaseDate = DateTime.Now.ToString();
                    bool t = Repository.BaseBll<Model.JH_ActivationCard>.Save(m);
                    if (t)
                        i += 1;
                }
            }
            json.type = "ok";
            json.info = "成功操作" + i + "条数据";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void add(HttpContext c)
        {
            string counts = c.Request["counts"] ?? "";
            string values = c.Request["values"] ?? "";
            int f = 0;
            for (int i = 0; i < DBUtility.Model.DbValue.GetInt(counts); i++)
            {
                Model.JH_ActivationCard m = new Model.JH_ActivationCard();
                m.Code = Common.Utility.GetDataRandom();
                m.faceValue = DBUtility.Model.DbValue.GetDecimal(values);
                m.isRelease = "0";
                m.isUse = "0"; 
                List<Model.JH_ActivationCard> list = Repository.BaseBll<Model.JH_ActivationCard>.GetList("Code='"+ m.Code + "'");
                if (list.Count == 0)
                {
                    bool t = Repository.BaseBll<Model.JH_ActivationCard>.Add(m);
                    if (t)
                        f += 1;
                }
            }
            json.type = "ok";
            json.info = "成功生成" + f + "张激活码";
            c.Response.Write(JsonConvert.SerializeObject(json));

        }
        void list(HttpContext c)
        {
            StringBuilder tr = new StringBuilder();
            //string nickname = c.Request["MapName"] ?? "";
            //if (!string.IsNullOrWhiteSpace(nickname))
            //{
            //    tr.AppendFormat(" and MapName like '%{0}%'", nickname);
            //}
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            Model.JH_ActivationCard model = new Model.JH_ActivationCard();
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