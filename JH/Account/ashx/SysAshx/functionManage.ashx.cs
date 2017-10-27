using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// functionManage 的摘要说明
    /// </summary>
    public class functionManage : IHttpHandler
    {
        JH.Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string mt = context.Request["p"] ?? "";
            try
            {
                switch (mt)
                {
                    case "List":
                        List(context);
                        break;
                    case "add":
                        add(context);
                        break;
                    case "delete":
                        delete(context);
                        break;
                    case "getinfo":
                        getinfo(context);
                        break;
                    case "functionList":
                        GetFunctionList(context);
                        break;
                    default:
                        json.info = "操作失败";
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

        /// <summary>
        /// 获取下拉选择父级功能
        /// </summary>
        /// <param name="c"></param>
        void GetFunctionList(HttpContext c)
        {
            List<Model.JH_FUNCTION> model = Repository.BaseBll<Model.JH_FUNCTION>.GetList();
            if (model.Count > 0)
            {
                List<object> l = new List<object>();
                l.Add(new
                {
                    display = "功能名称",
                    name = "FunctionName".ToUpper(),
                    align = "left",
                    width = 140,
                    minWidth = 60
                });
                l.Add(new
                {
                    display = "标识",
                    name = "FunctionID".ToUpper(),
                    align = "left",
                    width = 180,
                    minWidth = 60
                });

                var CustomersData = new { Rows = model, Total = model.Count };
                var options = new
                {
                    columns = l,
                    switchPageSizeApplyComboBox = false,
                    data = CustomersData,
                    pageSize = 10,
                    checkbox = false
                };
                c.Response.Write(JsonConvert.SerializeObject(options));
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        void getinfo(HttpContext c)
        {
            string FunctionID = c.Request["FunctionID"] ?? "";
            Model.JH_FUNCTION m = Repository.BaseBll<Model.JH_FUNCTION>.Get(new Model.JH_FUNCTION() { FUNCTIONID = FunctionID });
            c.Response.Write(JsonConvert.SerializeObject(m));
        }
        /// <summary>
        /// 删除
        /// </summary>
        void delete(HttpContext c)
        {
            string FunctionID = c.Request["FunctionID"] ?? "";
            string[] FunctionArror = FunctionID.Split(',');
            int i = 0;
            try
            {
                List<string> listsql = new List<string>();
                foreach (var item in FunctionArror)
                {
                    listsql.Add(string.Format("delete from JH_FUNCTION where FunctionID='{0}'", item));
                    //bool u = Repository.BaseBll<Model.JH_FUNCTION>.Remove(new Model.JH_FUNCTION() { FunctionID = item });
                    //if (u)
                    //    i += 1;
                    //else
                    //    t += 1;
                }
                i = DBUtility.DALHelper.dbHelper.ExecuteNonQuery(listsql);
                if (i > 0)
                {
                    json.info = "删除成功！";
                }
                else
                {
                    json.info = "删除失败！";
                }

                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        void add(HttpContext c)
        {
            string FunctionName = c.Request["FunctionName"] ?? "";
            string FunctionType = c.Request["FunctionType"] ?? "";
            string isEnable = c.Request["isEnable"] ?? "";
            string isOpen = c.Request["isOpen"] ?? "";
            string SuperiorID = c.Request["SuperiorID"] ?? "";
            string Sort = c.Request["Sort"] ?? "";
            string MenuType = c.Request["MenuType"] ?? "";
            string Code = c.Request["Code"] ?? "";
            string Remarks = c.Request["Remarks"] ?? "";
            string icon = c.Request["icon"] ?? "";
            string FunctionID = Common.Utility.GetMd5HashCode(Common.Utility.GetDataRandom());
            string Fid = c.Request["FunctionID"] ?? "";
            string CityId = c.Request["CityId"] ?? "";
            try
            {
                Model.JH_FUNCTION m = new Model.JH_FUNCTION();
                if (!string.IsNullOrWhiteSpace(Fid))
                {
                    m = Repository.BaseBll<Model.JH_FUNCTION>.Get(new Model.JH_FUNCTION() { FUNCTIONID = Fid });
                }
                else
                {
                    m.FUNCTIONID = FunctionID;
                }
                m.FUNCTIONNAME = FunctionName;
                m.FUNCTIONTYPE = FunctionType;
                m.ISENABLE = isEnable;
                m.ISOPEN = isOpen;
                m.SUPERIORID = SuperiorID;
                m.SORT = Sort;
                m.MENUTYPE = MenuType;
                m.CODE = Code;
                m.REMARKS = Remarks;
                m.ICON = icon;
                bool t = false;
                if (!string.IsNullOrWhiteSpace(Fid))
                    t = Repository.BaseBll<Model.JH_FUNCTION>.Save(m);
                else
                    t = Repository.BaseBll<Model.JH_FUNCTION>.Add(m);
                if (t)
                    json.info = "ok";
                else
                    json.info = "新增功能失败！";
                log.Sys_log.Info("功能管理：操作功能,操作结果：" + json.info);
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }
        /// <summary>
        /// 获取功能列表
        /// </summary>
        void List(HttpContext c)
        {
            string strSql = string.Empty;
            try
            {
                Model.JH_SYS_USER user = SetCookie.GetUserInfo();
                string sortname = c.Request["sortname"] ?? "SORT";//排序的字段
                string sortorder = c.Request["sortorder"] ?? "asc";//排序的方式
                string keyword = c.Request["keyword"] ?? "";//查询关键字
                string menuType = c.Request["menuType"] ?? "";//菜单类型
                //string cityid = c.Request["cityId"] ?? user.CityID;//查询地市的功能菜单

                int page = Convert.ToInt32(c.Request["page"] ?? "1");
                int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
                int totalCount = 0;

                StringBuilder tr = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(keyword))
                    tr.AppendFormat(" and FUNCTIONNAME like '%{0}%'", keyword);
                else
                    tr.AppendFormat(" and SUPERIORID='0'");
                if (!string.IsNullOrWhiteSpace(menuType))
                    tr.AppendFormat(" and MENUTYPE='{0}'", menuType);
                //if (!string.IsNullOrEmpty(cityid))
                //{
                //    tr.AppendFormat(" and CITYID='{0}'", cityid);
                //}
                strSql = string.Format(@"WITH CTE(FUNCTIONID, FUNCTIONNAME, FUNCTIONTYPE, ISENABLE, ISOPEN, SUPERIORID, SORT, REMARKS, CODE, MENUTYPE, ICON) AS
                (
                  SELECT FUNCTIONID, FUNCTIONNAME, FUNCTIONTYPE, ISENABLE, ISOPEN, SUPERIORID, SORT, REMARKS, CODE, MENUTYPE, ICON FROM JH_FUNCTION WHERE 1=1  {0} 
                  UNION ALL
                  SELECT C.FUNCTIONID, C.FUNCTIONNAME, C.FUNCTIONTYPE, C.ISENABLE, C.ISOPEN, C.SUPERIORID, C.SORT, C.REMARKS, C.CODE, C.MENUTYPE, C.ICON FROM CTE AS A,JH_FUNCTION AS C WHERE A.FUNCTIONID=C.SUPERIORID 
                )
                SELECT FUNCTIONID, FUNCTIONNAME, FUNCTIONTYPE, ISENABLE, ISOPEN, SUPERIORID, SORT, REMARKS, CODE, MENUTYPE, ICON FROM CTE where 1=1 order by convert(int,SORT) asc ", tr);

                DataTable dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text, strSql);
                //List<object> list = DBUtility.ORM.GetListPage(pagesize, page, out totalCount, strsql);

                //List<Model.JH_FUNCTION> model = Repository.BaseBll<Model.JH_FUNCTION>.GetList(string.Format("1=1 order by {0} {1}", sortname, sortorder)); //
                //Repository.BaseBll<Model.JH_FUNCTION>.GetList(pagesize, page, out totalCount, string.Format("1=1 order by {0} {1}", sortname, sortorder));

                //if (pagesize == 0)
                //{
                //    var griddata = new { Rows = dt, Total = dt.Rows.Count };
                //    c.Response.Write(JsonConvert.SerializeObject(griddata));
                //}
                //else
                //{
                var griddata = new { Rows = dt, Total = dt.Rows.Count };
                c.Response.Write(JsonConvert.SerializeObject(griddata));
                //}

            }
            catch (Exception ex)
            {
                log.Sys_log.Error(ex.Message);
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
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