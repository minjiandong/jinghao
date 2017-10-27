using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// ScenicSpotManage 的摘要说明
    /// </summary>
    public class ScenicSpotManage : IHttpHandler
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
                    case "ScenicSave":
                        ScenicSave(context);
                        break;
                    case "get":
                        get(context);
                        break;
                    case "MarkersList":
                        MarkersList(context);
                        break;
                    case "MarkersSave":
                        MarkersSave(context);
                        break;
                    case "getM":
                        getM(context);
                        break;
                    case "fileUpload":
                        fileUpload(context);
                        break;
                    case "imgUpload":
                        imgUpload(context);
                        break;
                    case "fileUploadimg":
                        fileUploadimg(context);
                        break;
                    case "detailedSave":
                        detailedSave(context);
                        break;
                    case "generate":
                        generate(context);
                        break;
                    case "deleteM":
                        deleteM(context);
                        break;
                    case "delete":
                        delete(context);
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
            finally
            {
                context.Response.End();
            }
        }
        void delete(HttpContext c)
        {
            string IDArror = c.Request["IDArror"] ?? "";
            if (!string.IsNullOrWhiteSpace(IDArror))
            {
                string[] Arror = IDArror.Split(',');
 
                List<string> listsql = new List<string>();
                foreach (var item in Arror)
                {
                    int id = DBUtility.Model.DbValue.GetInt(item);
                    listsql.Add("delete from JH_ScenicSpot where id="+id+"");
                    listsql.Add("delete from JH_MarkersList where ScenicID="+id+"");
                }
                int i = DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql);
                if (i > 0)
                    json.info = "删除成功！";
                else
                    json.info = "删除失败！";
            }
            else
            {
                json.info = "删除出错了！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void deleteM(HttpContext c)
        {
            string IDArror = c.Request["IDArror"] ?? "";
            if (!string.IsNullOrWhiteSpace(IDArror))
            {
                string[] Arror = IDArror.Split(',');
                int t = 0;
                int i = 0;
                foreach (var item in Arror)
                {
                    int id = DBUtility.Model.DbValue.GetInt(item);
                    bool bools = Repository.BaseBll<Model.JH_MarkersList>.Remove(new Model.JH_MarkersList() { id = id });
                    if (bools)
                        t += 1;
                    else
                        i += 1;
                }
                json.info = "成功删除" + t + "条数据" + "失败" + i + "条数据";
            }
            else
            {
                json.info = "删除出错了！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void generate(HttpContext c)
        {
            string IDArror = c.Request["IDArror"] ?? "";
            if (!string.IsNullOrWhiteSpace(IDArror))
            {
                string[] Arror = IDArror.Split(',');
                foreach (var item in Arror)
                {
                    int id = DBUtility.Model.DbValue.GetInt(item);
                    SavePage(id);
                }
            }
            json.info = "ok";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
       
        /// <summary>
        /// 生成试图页面
        /// </summary>
        void SavePage(int id)
        {
            Model.JH_ScenicSpot m = Repository.BaseBll<Model.JH_ScenicSpot>.Get(new Model.JH_ScenicSpot() { id = id });
            if (m != null)
            {
                string strScript = string.Empty;
                string csFile = string.Empty;
                string designerCS = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("/Account/xml/Page.xml"));
                foreach (XmlNode item in xmlDoc.SelectSingleNode("configuration").ChildNodes)
                {
                    XmlElement xx = (item as XmlElement);
                    if (xx.GetAttribute("name") == "htmlStr")
                    {
                        StringBuilder tr = new StringBuilder();
                        StringBuilder trs = new StringBuilder();
                        List<Model.JH_MarkersList> list = Repository.BaseBll<Model.JH_MarkersList>.GetList("ScenicID=" + id + "");
                        foreach (var M_item in list)
                        {
                            double lat = 0.00;
                            double lon = 0.00;
                            Common.EvilTransform.transform(double.Parse(M_item.position.Split(',')[0]),double.Parse(M_item.position.Split(',')[1]), out lat, out lon);
                            string position = lat + "," + lon;
                            string icon = string.Empty;
                            string viweType = "scenic";
                            if (M_item.icon == "2")
                            {
                                icon = "icon:'" + M_item.zicon + "',";
                                viweType = "Hotel";
                            }
                            if (M_item.icon == "1")
                            {
                                icon = "icon:'" + M_item.zicon + "',";
                                viweType = "toilet";
                            }
                            string _viweImgUrl = "https://cloud.jhlxw.com/app" + M_item.viweImgUrl;
                            string _audioUrl = "https://cloud.jhlxw.com/app" + M_item.audioUrl;
                            tr.Append("markers.push(");
                            tr.Append("{ " + icon + "viweID: '" + M_item.id + "',distance:" + M_item.distance + ",viweName: '" + M_item.viweName + "',viweImgUrl: '" + _viweImgUrl + "',viweType: '" + viweType + "', introduction: '" + M_item.introduction + "',audioUrl:{'audioId1-2': '" + _audioUrl + "'},detailsURL:'/details?id=" + M_item.id + "',position:[" + position + "],area:[" + M_item.area + "]});"); 
                            trs.Append("{  'position': '"+ position + "', 'mark': '1' },");
                        }
                        string t = string.Empty; 
                        string display =  "";
                        if (m.Play_type == "1")
                        {
                            t = "<a class=\"aActivate\" style=\"width:80%;\" href=\"/buy?money=" + m.Monetary.ToString() + "&Scenic_id=" + m.id.ToString() + "\">立即购买</a>";
                            display = "style=\"display:none;\"";
                        }
                        else if (m.Play_type == "2")
                        {
                            t = "<a class=\"aActivate\" style=\"width:80%;\" href=\"javascript:sq();\">验证授权</a>";
                        }
                        else
                        {
                            t = "<a class=\"aBuy\" href=\"/buy?money=" + m.Monetary.ToString() + "&Scenic_id=" + m.id.ToString() + "\">立即购买</a>";
                            t += "<a class=\"aActivate\" href=\"javascript:sq();\">验证授权</a>";
                        }
                        strScript = xx.InnerText;
                        strScript = strScript.Replace("$button$", t);
                        strScript = strScript.Replace("$display$", display);
                        strScript = strScript.Replace("$id$", m.id.ToString());
                        strScript = strScript.Replace("$title$", m.MapName);
                        strScript = strScript.Replace("$MapName$", m.MapName); 
                        strScript = strScript.Replace("$CoreCoordinate$", m.CoreCoordinate); 
                        strScript = strScript.Replace("$beginCoordinate$", m.beginCoordinate); 
                        strScript = strScript.Replace("$endCoordinate$", m.endCoordinate);
                        strScript = strScript.Replace("$MapZoom$", m.MapZoom);
                        strScript = strScript.Replace("$MapZoomRange$", m.MapZoomRange);
                        strScript = strScript.Replace("$markers$", tr.ToString());
                        strScript = strScript.Replace("$lineTrack$", trs.ToString().Substring(0,trs.ToString().Length - 1));
                        strScript = strScript.Replace("$money$", m.Monetary.ToString());
                        strScript = strScript.Replace("$openTime$", m.openTime.ToString());
                        strScript = strScript.Replace("$address$", m.address.ToString());
                        strScript = strScript.Replace("$tel$", m.tel.ToString());
                        strScript = strScript.Replace("$level$", m.level.ToString());
                        strScript = strScript.Replace("$showsImg$", "https://cloud.jhlxw.com/app" + m.showsImg);
                        strScript = strScript.Replace("$start_Play$", "https://cloud.jhlxw.com/app" + m.start_Play);
                        strScript = strScript.Replace("$Remarks$", jq(m.Remarks));
                    }
                }
                SaveTempletHtml(HttpContext.Current.Server.MapPath("/Map-"+id+".html"), strScript, "");
            }
        }
        string jq(string val)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                if (val.Length >= 200)
                    return val.Substring(0, 200) + "...";
                else
                    return val;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 创建html模板
        /// </summary>
        /// <param name="FilePath">文件名称和路径</param>
        /// <param name="ScriptContent">脚本文件</param>
        /// <param name="CssContent">CSS样式</param>
        /// <param name="htmlContent">html内容</param>
        /// <param name="FileTitle">页面title标题名称</param>
        void SaveTempletHtml(string FilePath, string ScriptContent, string FileTitle)
        {
            FileStream o = new FileStream(FilePath, FileMode.Create);//覆盖原来的问题件
            using (StreamWriter sw = new StreamWriter(o))
            {
                try
                {
                    StringBuilder FileContent = new StringBuilder();
                    FileContent.Append(ScriptContent);
                    sw.WriteLine(FileContent.ToString());
                }
                catch (Exception ex)
                {
                    log.Sys_log.Error(ex.Message);
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                    o.Close();
                }
            }
        }

        void detailedSave(HttpContext c)
        {
            string detailed = c.Request["detailed"] ?? "";
            string id = c.Request["id"] ?? "";
            if (!string.IsNullOrWhiteSpace(id))
            {
                Model.JH_MarkersList model = new Model.JH_MarkersList();
                model.id = DBUtility.Model.DbValue.GetInt(id);
                model = Repository.BaseBll<Model.JH_MarkersList>.Get(model);
                model.detailed = detailed;
                bool t = Repository.BaseBll<Model.JH_MarkersList>.Save(model);
                if (t)
                    json.info = "ok";
                else
                    json.info = "操作失败！";
            }
            else
            {
                json.info = "参数失败！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void fileUploadimg(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            string zicon = c.Request["zicon"] ?? "";
            string serverpath = string.Empty;
            string uploadFileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool upbool = false;
                try
                {
                    var file = c.Request.Files["filedata"];
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
                    Model.JH_MarkersList model = new Model.JH_MarkersList();
                    model.id = DBUtility.Model.DbValue.GetInt(id);
                    model = Repository.BaseBll<Model.JH_MarkersList>.Get(model);
                    if (zicon == "1")
                        model.zicon = "/upload/" + uploadFileName;
                    else
                        model.viweImgUrl = "/upload/" + uploadFileName;
                    bool t = Repository.BaseBll<Model.JH_MarkersList>.Save(model);
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
        void imgUpload(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            string type = c.Request["type"] ?? "";
            string serverpath = string.Empty;
            string uploadFileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool upbool = false;
                try
                {
                    var file = c.Request.Files["imgdata"];
                    string Extension =  Path.GetExtension(file.FileName);
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
                    Model.JH_ScenicSpot model = new Model.JH_ScenicSpot();
                    model.id = DBUtility.Model.DbValue.GetInt(id);
                    model = Repository.BaseBll<Model.JH_ScenicSpot>.Get(model);
                    if (type == "1")
                    {
                        model.start_Play = "/upload/" + uploadFileName;
                    }
                    else
                    {
                        model.showsImg = "/upload/" + uploadFileName;
                    } 
                    bool t = Repository.BaseBll<Model.JH_ScenicSpot>.Save(model);
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
        void fileUpload(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            string serverpath = string.Empty;
            string uploadFileName = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool upbool = false;
                try
                {
                    var file = c.Request.Files["filedata"];
                    uploadFileName = Common.Utility.GetDataRandom() + ".mp3";
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
                    Model.JH_MarkersList model = new Model.JH_MarkersList();
                    model.id = DBUtility.Model.DbValue.GetInt(id);
                    model = Repository.BaseBll<Model.JH_MarkersList>.Get(model);
                    model.audioUrl = "/upload/" + uploadFileName;
                    bool t = Repository.BaseBll<Model.JH_MarkersList>.Save(model);
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
      
        void getM(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            Model.JH_MarkersList model = new Model.JH_MarkersList();
            model.id = DBUtility.Model.DbValue.GetInt(id);
            model = Repository.BaseBll<Model.JH_MarkersList>.Get(model);
            c.Response.Write(JsonConvert.SerializeObject(model));
        }
        void MarkersSave(HttpContext c)
        {
            string viweName = c.Request["viweName"] ?? "";
            string detailsURL = c.Request["detailsURL"] ?? "";
            string position = c.Request["position"] ?? "";
            string area = c.Request["area"] ?? "";
            string introduction = c.Request["introduction"] ?? "";
            string id = c.Request["id"] ?? "";
            string ScenicID = c.Request["ScenicID"] ?? "";
            string icon = c.Request["icon"] ?? "";
            string distance = c.Request["distance"] ?? "";

            Model.JH_MarkersList model = new Model.JH_MarkersList();
            bool ret = false;
            if (!string.IsNullOrWhiteSpace(id))
            {
                ret = true;
                model.id = DBUtility.Model.DbValue.GetInt(id);
                model = Repository.BaseBll<Model.JH_MarkersList>.Get(model);
            }
            model.viweName = viweName;
            model.detailsURL = detailsURL;
            model.position = position;
            model.area = area;
            model.introduction = introduction; 
            model.ScenicID = DBUtility.Model.DbValue.GetInt(ScenicID);
            model.icon = icon;
            model.distance = DBUtility.Model.DbValue.GetInt(distance);
            bool t = false;
            if (ret)
                t = Repository.BaseBll<Model.JH_MarkersList>.Save(model);
            else
                 t = Repository.BaseBll<Model.JH_MarkersList>.Add(model);
            if (t)
                json.info = "ok";
            else
                json.info = "保存失败！";
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void MarkersList(HttpContext c)
        {
            StringBuilder tr = new StringBuilder();
            string id = c.Request["id"] ?? "";
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            Model.JH_MarkersList model = new Model.JH_MarkersList();
            List<object> m = DBUtility.ORM.GetList(model, pagesize, page, out totalCount, string.Format("ScenicID='{3}' {0} order by {1} {2}", tr.ToString(), sortname, sortorder,id));
            var griddata = new { Rows = m, Total = totalCount };
            c.Response.Write(JsonConvert.SerializeObject(griddata));
        }
        void get(HttpContext c)
        {
            string id = c.Request["id"] ?? "";
            Model.JH_ScenicSpot model = new Model.JH_ScenicSpot();
            model.id = DBUtility.Model.DbValue.GetInt(id);
            model = Repository.BaseBll<Model.JH_ScenicSpot>.Get(model);
            c.Response.Write(JsonConvert.SerializeObject(model));
        }
        void ScenicSave(HttpContext c)
        {
            string MapName = c.Request["MapName"] ?? "";
            string MapImageUrl = c.Request["MapImageUrl"] ?? "";
            string CoreCoordinate = c.Request["CoreCoordinate"] ?? "";
            string beginCoordinate = c.Request["beginCoordinate"] ?? "";
            string endCoordinate = c.Request["endCoordinate"] ?? "";
            string MapZoom = c.Request["MapZoom"] ?? "";
            string MapZoomRange = c.Request["MapZoomRange"] ?? "";
            string id = c.Request["id"] ?? "";
            string Remarks = c.Request["Remarks"] ?? "";
            string Recommend = c.Request["Recommend"] ?? "";
            string city = c.Request["city"] ?? "";
            string sheng = c.Request["sheng"] ?? "";
            string level = c.Request["level"] ?? "";
            string Play_type = c.Request["Play_type"] ?? ""; 
            string openTime = c.Request["openTime"] ?? "";
            string address = c.Request["address"] ?? "";
            string tel = c.Request["tel"] ?? "";
            decimal Monetary = DBUtility.Model.DbValue.GetDecimal(c.Request["Monetary"] ?? "");
            int Play_number = DBUtility.Model.DbValue.GetInt(c.Request["Play_number"] ?? "");

            Model.JH_ScenicSpot model = new Model.JH_ScenicSpot();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model.id = DBUtility.Model.DbValue.GetInt(id);
                model = Repository.BaseBll<Model.JH_ScenicSpot>.Get(model);
            }
            model.MapName = MapName;
            model.MapImageUrl = MapImageUrl;
            model.beginCoordinate = beginCoordinate;
            model.endCoordinate = endCoordinate;
            model.MapZoom = MapZoom;
            model.MapZoomRange = MapZoomRange;
            model.CoreCoordinate = CoreCoordinate;
            model.Remarks = Remarks;
            model.Recommend = Recommend;
            model.Monetary = Monetary;
            model.Play_number = Play_number;
            model.city = city;
            model.sheng = sheng;
            model.level = level;
            model.Play_type = Play_type;
            model.openTime = openTime;
            model.address = address;
            model.tel = tel; 
            bool t = false;
            if (!string.IsNullOrWhiteSpace(id))
            {
               t = Repository.BaseBll<Model.JH_ScenicSpot>.Save(model);
            }
            else
            {
               t = Repository.BaseBll<Model.JH_ScenicSpot>.Add(model);
            }
            if (t)
            {
                json.info = "ok";
            }
            else
            {
                json.info = "保存失败！";
            }
            c.Response.Write(JsonConvert.SerializeObject(json));
        }
        void list(HttpContext c)
        {
            StringBuilder tr = new StringBuilder();
            string nickname = c.Request["MapName"] ?? "";
            if (!string.IsNullOrWhiteSpace(nickname))
            {
                tr.AppendFormat(" and MapName like '%{0}%'", nickname);
            }
            string sortname = c.Request["sortname"] ?? "id";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            Model.JH_ScenicSpot model = new Model.JH_ScenicSpot();
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