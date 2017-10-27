using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Data;
using Newtonsoft.Json;

namespace JH
{
    /// <summary>
    /// Synchro 的摘要说明
    /// </summary>
    public class Synchro : IHttpHandler
    {
        Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                DataTable list = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=136");
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    int id = DBUtility.Model.DbValue.GetInt(list.Rows[i]["scenic_id"].ToString());
                    SavePage(id);
                }
                json.info = "ok";
                json.type = "synchro";
                context.Response.Write(JsonConvert.SerializeObject(json)); 
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                json.type = "error";
                context.Response.Write(JsonConvert.SerializeObject(json));
            }
            finally
            {
                context.Response.End();
            }
        }

        /// <summary>
        /// 生成试图页面
        /// </summary>
        void SavePage(int id)
        {
            List<string> listsql = new List<string>();
            Model.JH_ScenicSpot m = null;
            
            DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenics where scenic_id=" + id + "");
            if (dt.Rows.Count > 0)
            {
                string _beginCoordinate = dt.Rows[0]["scenic_scope"].ToString().Replace("[", "").Replace("]", "");
                string _city = string.Empty;
                string _sheng = string.Empty; 

                _city = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select name from zy_region where id=" + DBUtility.Model.DbValue.GetInt(dt.Rows[0]["city"].ToString()) + "").ToString();

                _sheng = DBUtility.DALHelper.myHelper.ExecuteScalar(CommandType.Text, "select name from zy_region where id=" + DBUtility.Model.DbValue.GetInt(dt.Rows[0]["province"].ToString()) + "").ToString();

                m = new Model.JH_ScenicSpot()
                {
                    address = "",
                    beginCoordinate = _beginCoordinate.Split(',')[0] + "," + _beginCoordinate.Split(',')[1],
                    endCoordinate = _beginCoordinate.Split(',')[2] + "," + _beginCoordinate.Split(',')[3],
                    CoreCoordinate = dt.Rows[0]["map_centre"].ToString(),
                    city = _city,
                    id = DBUtility.Model.DbValue.GetInt(dt.Rows[0]["scenic_id"].ToString()),
                    level = dt.Rows[0]["scenic_rank"].ToString(),
                    MapName = dt.Rows[0]["scenic_name"].ToString(),
                    MapZoom = dt.Rows[0]["map_zoom"].ToString(),
                    MapZoomRange = dt.Rows[0]["zoom_scope"].ToString(),
                    Monetary = DBUtility.Model.DbValue.GetDecimal(dt.Rows[0]["cost_money"].ToString()),
                    openTime = "",
                    MapImageUrl = dt.Rows[0]["is_hand_draw"].ToString() == "0" ? "" : dt.Rows[0]["map_img"].ToString(),
                    Play_number = DBUtility.Model.DbValue.GetInt(dt.Rows[0]["listen_num"].ToString()),
                    Play_type = dt.Rows[0]["buy_type"].ToString(),
                    Recommend = "0",
                    Remarks = dt.Rows[0]["scenics_text"].ToString(),
                    sheng = _sheng,
                    showsImg = dt.Rows[0]["scenic_img"].ToString(),
                    start_Play = "",
                    tel = "",
                    detailed = dt.Rows[0]["riding_text"].ToString(),
                    is_bluetooth = dt.Rows[0]["is_bluetooth"].ToString(),
                    is_app = dt.Rows[0]["is_app"].ToString(),
                    is_on_sale = dt.Rows[0]["is_on_sale"].ToString(),
                    _route = dt.Rows[0]["route"].ToString(),
                    route1 = dt.Rows[0]["route1"].ToString(),
                    route2 = dt.Rows[0]["route2"].ToString(),
                    route_name = dt.Rows[0]["route_name"].ToString(),
                    route_name1 = dt.Rows[0]["route_name1"].ToString(),
                    route_name2 = dt.Rows[0]["route_name2"].ToString()
                };
                if (dt.Rows[0]["synchronize"].ToString() == "1")
                {
                    DBUtility.DALHelper.dbHelper.ExecuteNonQuery(CommandType.Text, "delete from JH_ScenicSpot where id=" + id + "");
                    listsql.Add(string.Format(@"INSERT INTO JH_ScenicSpot (
                    [id]
                   ,[MapName]
                   ,[MapImageUrl]
                   ,[Remarks]
                   ,[beginCoordinate]
                   ,[endCoordinate]
                   ,[MapZoom]
                   ,[MapZoomRange]
                   ,[CoreCoordinate]
                   ,[showsImg]
                   ,[Recommend]
                   ,[city]
                   ,[Monetary]
                   ,[sheng]
                   ,[Play_number]
                   ,[level]
                   ,[Play_type]
                   ,[openTime]
                   ,[address]
                   ,[tel]
                   ,[start_Play]
                   ,[detailed],[is_bluetooth],[is_app],[is_on_sale],[_route],[route1],[route2],[route_name],[route_name1],[route_name2])VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},'{13}',{14},'{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}')", m.id, m.MapName, m.MapImageUrl, m.Remarks, m.beginCoordinate, m.endCoordinate, m.MapZoom, m.MapZoomRange, m.CoreCoordinate, m.showsImg, m.Recommend, m.city, m.Monetary, m.sheng, m.Play_number, m.level, m.Play_type, m.openTime, m.address, m.tel, m.start_Play, m.detailed, m.is_bluetooth, m.is_app, m.is_on_sale,m._route,m.route1,m.route2,m.route_name,m.route_name1,m.route_name2));
                    DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, "update zy_scenics set synchronize=0 where scenic_id=" + id + "");
                }
            }

            if (m != null)
            {
                DataTable _dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(CommandType.Text, "select * from zy_scenic_spots where scenic_id=" + id + " and is_on_sale=1 ");
                for (int i = 0; i < _dt.Rows.Count; i++)
                {  
                    Model.JH_MarkersList _m = new Model.JH_MarkersList()
                    {
                        area = "",
                        audioUrl = _dt.Rows[i]["voice_url"].ToString(),
                        detailed = _dt.Rows[i]["spots_content"].ToString(),
                        detailsURL = "",
                        id = DBUtility.Model.DbValue.GetInt(_dt.Rows[i]["spots_id"].ToString()),
                        distance = DBUtility.Model.DbValue.GetInt(_dt.Rows[i]["spots_scope"].ToString()),
                        icon = _dt.Rows[i]["spots_type"].ToString(),
                        introduction = _dt.Rows[i]["abstract"].ToString(),
                        Monetary = 0.00m,
                        position = _dt.Rows[i]["spots_centre"].ToString(),
                        ScenicID = id,
                        viweImgUrl = _dt.Rows[i]["img_url"].ToString(),
                        viweName = _dt.Rows[i]["name"].ToString(),
                        zicon = _dt.Rows[i]["lab_url"].ToString(),
                        major = _dt.Rows[i]["major"].ToString(),
                        sort = DBUtility.Model.DbValue.GetInt(_dt.Rows[i]["sort"].ToString()),
                        sort2 = DBUtility.Model.DbValue.GetInt(_dt.Rows[i]["sort2"].ToString()),
                        sort3 = DBUtility.Model.DbValue.GetInt(_dt.Rows[i]["sort3"].ToString()),
                        url_out = _dt.Rows[i]["url_out"].ToString()
                    };
                    DBUtility.DALHelper.dbHelper.ExecuteNonQuery(CommandType.Text, "delete from JH_MarkersList where id=" + _m.id + "");
                    listsql.Add(string.Format(@"INSERT INTO JH_MarkersList
                       ([id]
                       ,[viweName]
                       ,[viweImgUrl]
                       ,[introduction]
                       ,[audioUrl]
                       ,[detailsURL]
                       ,[position]
                       ,[area]
                       ,[detailed]
                       ,[ScenicID]
                       ,[Monetary]
                       ,[icon]
                       ,[zicon]
                       ,[distance],[major],[sort],[sort2],[sort3],[url_out])
                 VALUES
                       (" + _m.id + ",'" + _m.viweName + "','" + _m.viweImgUrl + "','" + _m.introduction + "','" + _m.audioUrl + "','" + _m.detailsURL + "','" + _m.position + "','" + _m.area + "','" + _m.detailed + "'," + _m.ScenicID + "," + _m.Monetary + ",'" + _m.icon + "','" + _m.zicon + "'," + _m.distance + ",'" + _m.major + "'," + _m.sort + "," + _m.sort2 + "," + _m.sort3 + ",'"+_m.url_out+"')"));
                    DBUtility.DALHelper.myHelper.ExecuteNonQuery(CommandType.Text, "update zy_scenic_spots set synchronize=0 where spots_id=" + _m.id + "");
                    
                }
                string errorstring = string.Empty;
                DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql, out errorstring);
            }
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