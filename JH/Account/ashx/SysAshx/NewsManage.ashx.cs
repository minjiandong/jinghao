using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// NewsManage 的摘要说明
    /// </summary>
    public class NewsManage : IHttpHandler
    {
        JH.Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string m = context.Request["p"] ?? "";
                switch (m)
                {
                    case "getlist":
                        getlist(context);
                        break;
                    case "newsSave":
                        newsSave(context);
                        break;
                    case "get":
                        get(context);
                        break;
                    case "imgUpload":
                        imgUpload(context);
                        break;
                    case "newdelete":
                        newdelete(context);
                        break;
                    default:
                        json.info = "参数错误！";
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
            finally
            {
                context.Response.End();
            }
        }
        void newdelete(HttpContext c)
        {
            string arr = c.Request["IDArror"] ?? "";
            string[] array = arr.Split(',');
            int i = 0;
            int t = 0;
            foreach (var item in array)
            {
                Model.JH_NEWS model = new Model.JH_NEWS();
                model.id = DBUtility.Model.DbValue.GetInt(item);
                bool re = Repository.BaseBll<Model.JH_NEWS>.Remove(model);
                if (re)
                    i += 1;
                else
                    t += 1;
            }
            json.info = "成功删除{" + i + "}条数据,失败{" + t + "}条";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void imgUpload(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            string serverpath = string.Empty;
            string uploadFileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool upbool = false;
                try
                {
                    var file = c.Request.Files["imgdata"];
                    string Extension = Path.GetExtension(file.FileName);
                    uploadFileName = Common.Utility.GetDataRandom() + Extension;
                    serverpath = c.Server.MapPath("/upload/") + uploadFileName;
                    file.SaveAs(serverpath);
                    upbool = true;
                }
                catch
                {
                    upbool = false;
                }
                if (upbool)
                {
                    Model.JH_NEWS model = new Model.JH_NEWS();
                    model.id = DBUtility.Model.DbValue.GetInt(id);
                    model = Repository.BaseBll<Model.JH_NEWS>.Get(model);
                    model.n_img = "/upload/" + uploadFileName;
                    bool t = Repository.BaseBll<Model.JH_NEWS>.Save(model);
                    if (t)
                        json.info = "ok";
                    else
                        json.info = "上传失败！";
                }
                else
                {
                    json.info = "上传文件出错！";
                }
            }
            else
            {
                json.info = "无效ID参数！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));

        }
        void get(HttpContext c)
        {
            int id = DBUtility.Model.DbValue.GetInt(c.Request["id"] ?? "");
            if (id > 0)
            {
                Model.JH_NEWS m = Repository.BaseBll<Model.JH_NEWS>.Get(new Model.JH_NEWS() { id = id });
                c.Response.Write(JsonConvert.SerializeObject(m));
            }
            else
            {
                json.info = "获取数据失败！";
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }
        void newsSave(HttpContext c)
        {
            string n_title = c.Request["n_title"] ?? "";
            string n_type = c.Request["n_type"] ?? "";
            string n_abstract = c.Request["n_abstract"] ?? "";
            string n_content = c.Request["n_content"] ?? "";
            int n_consult = 0;
            string n_ReleaseTime = DateTime.Now.ToString();
            string n_updateTime = DateTime.Now.ToString();
            int id = DBUtility.Model.DbValue.GetInt(c.Request["id"] ?? "");
            Model.JH_NEWS m = new Model.JH_NEWS();
            if (id > 0)
            {
                m.id = id;
                m = Repository.BaseBll<Model.JH_NEWS>.Get(m);
                m.n_updateTime = n_updateTime;
            }
            m.n_title = n_title;
            m.n_abstract = n_abstract;
            m.n_content = n_content;
            m.n_consult = n_consult;
            m.n_ReleaseTime = n_ReleaseTime;
            m.n_type = n_type;
            bool ret = false;
            if (id > 0)
                ret = Repository.BaseBll<Model.JH_NEWS>.Save(m);
            else
                ret = Repository.BaseBll<Model.JH_NEWS>.Add(m);
            if (ret)
                json.info = "ok";
            else
                json.info = "保存失败！";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void getlist(HttpContext c)
        {
            StringBuilder tr = new StringBuilder();
            string n_title = c.Request["n_title"] ?? "";
            if (!string.IsNullOrWhiteSpace(n_title))
            {
                tr.AppendFormat(" and n_title like '%{0}%'", n_title);
            }
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            Model.JH_NEWS model = new Model.JH_NEWS();
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