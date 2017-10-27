using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JH.Account.ashx.SysAshx
{
    /// <summary>
    /// userManage 的摘要说明
    /// </summary>
    public class userManage : IHttpHandler
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
                    case "list":
                        List(context);
                        break;
                    case "save":
                        Save(context);
                        break;
                    case "delete":
                        Delete(context);
                        break;
                    case "edit":
                        Edit(context);
                        break;
                    case "reset":
                        Reset(context);
                        break;
                    case "editpwd":
                        EditPassword(context);
                        break;
                    case "oldpwd":
                        OldPassword(context);
                        break;
                    case "c_list":
                        c_list(context);
                        break;
                    default:
                        json.info = "操作错误，请重试！";
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
        protected void c_list(HttpContext c)
        {
            int userid = DBUtility.Model.DbValue.GetInt(c.Request["Userid"] ?? "");
            string strsql = string.Format("select b.mapname,b.tel,b.level from JH_ScenicList as a,JH_ScenicSpot as b where a.Scenicid=b.id and a.User_id={0}", userid);
            DataTable dt = DBUtility.DALHelper.dbHelper.ExecuteDataTable(CommandType.Text,strsql);
            var griddata = new { Rows = dt, Total = dt.Rows.Count };
            c.Response.Write(JsonConvert.SerializeObject(griddata));
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="context"></param>
        protected void List(HttpContext c)
        {
            string strWhere = string.Empty;
            string sortname = c.Request["sortname"] ?? "UserID";//排序的字段
            string sortorder = c.Request["sortorder"] ?? "desc";//排序的方式
            int page = Convert.ToInt32(c.Request["page"] ?? "1");
            int pagesize = Convert.ToInt32(c.Request["pagesize"] ?? "10");
            int totalCount = 0;
            string UserType = c.Request["UserType"] ?? "";
            string name = c.Request.Params["name"].ToString();
            string ScenicName = c.Request["ScenicName"] ?? "";
            string timek = c.Request["timek"] ?? "";
            string timej = c.Request["timej"] ?? "";
            if (!string.IsNullOrEmpty(name))
            {
                strWhere += string.Format(" and (UserName like '%{0}%' or FullName like '%{0}%' or Phone  like '%{0}%' )", name);
            }
            if (!string.IsNullOrWhiteSpace(ScenicName))
            {
                strWhere += string.Format(" and exists(select a.id from JH_ScenicList as a,JH_ScenicSpot as b where a.Scenicid=b.id and a.User_id=userid and b.Mapname like '%{0}%')", ScenicName);
            }
            Model.JH_SYS_USER model = new Model.JH_SYS_USER();
            List<object> m = DBUtility.ORM.GetList(model, pagesize, page, out totalCount, string.Format("UserType='{3}' {0} order by {1} {2}", strWhere, sortname, sortorder, UserType));
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID", typeof(int));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("UserState", typeof(string));
            dt.Columns.Add("ScenicCount", typeof(string));
            dt.Columns.Add("Commission", typeof(string));
            dt.Columns.Add("Cost", typeof(string));

            foreach (Model.JH_SYS_USER item in m)
            {
                DataRow dr = dt.NewRow();
                dr["UserID"] = item.UserID;
                dr["UserName"] = item.UserName;
                dr["FullName"] = item.FullName;
                dr["Phone"] = item.Phone;
                dr["UserState"] = item.UserState;
                dr["ScenicCount"] = ScenicCount(item.UserID) + "家";
                dr["Commission"] = (item.Commission * 100) + "%";
                dr["Cost"] = "￥" + Cost(timek, timej, item.UserID, item.Commission).ToString();
                dt.Rows.Add(dr);
            }
            DataView dv = new DataView(dt);
            DataTable _dt = dv.Table; 
            var griddata = new { Rows = _dt, Total = totalCount };
            c.Response.Write(JsonConvert.SerializeObject(griddata));
        }
        /// <summary>
        /// 提成费用
        /// </summary>
        /// <returns></returns>
        decimal Cost(string timek,string timej,int userid,decimal dec)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(timek) && !string.IsNullOrWhiteSpace(timej))
            {
                where = string.Format(" and  CONVERT(varchar(10),b.consumptionDate,120)>='{0}' and CONVERT(varchar(10),b.consumptionDate,120)<='{1}'",timek,timej);
            }
            string strsql = string.Format("select sum(b.money) from JH_ScenicList as a,JH_Activation as b where a.Scenicid=b.Scenic_id and a.User_id={0} {1}", userid, where);
            object count = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, strsql);
            if (DBUtility.Model.DbValue.GetDecimal(count) == null)
                return 0.00m;
            else
                return DBUtility.Model.DbValue.GetDecimal(count.ToString()) * dec;
        }
        int ScenicCount(int userid)
        {
            string strsql = string.Format("select count(1) from JH_ScenicList where user_id={0}",userid);
            object count = DBUtility.DALHelper.dbHelper.ExecuteScalar(CommandType.Text, strsql);
            return DBUtility.Model.DbValue.GetInt(count.ToString());
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="c"></param>
        protected void Save(HttpContext c)
        {
            try
            {
                string loginName = c.Request["loginName"] ?? "";
                string Hidden1 = c.Request["Hidden1"] ?? "";
                Model.JH_SYS_USER m = new Model.JH_SYS_USER();
                if (!string.IsNullOrWhiteSpace(Hidden1))
                {
                    m.UserID = int.Parse(Hidden1);
                    m = DBUtility.ORM.Get(m) as Model.JH_SYS_USER;
                }
                m.UserName = loginName;
                m.FullName = c.Request["userName"] ?? "";
                m.Phone = c.Request["phone"] ?? "";
                m.UserSex = c.Request["sex"] ?? "";
                m.UserState = c.Request["use"] ?? "";
                m.CityID = c.Request["CityID"] ?? "";
                m.UserType = c.Request["UserType"] ?? "";
                m.Commission = DBUtility.Model.DbValue.GetDecimal(c.Request["Commission"] ?? "");
                if (!string.IsNullOrWhiteSpace(Hidden1))
                {
                    Model.JH_SYS_USER user = new Model.JH_SYS_USER();
                    string strWhere = string.Format(" UserName='{0}'", loginName);
                    user = Repository.BaseBll<Model.JH_SYS_USER>.Get(user, strWhere);
                    if (user == null)
                    {
                        m.UserName = loginName;
                        if (DBUtility.ORM.Save(m))
                            json.info = "ok";
                        else
                            json.info = "保存失败";
                    }
                    else
                    {
                        if (user.UserID == m.UserID)
                        {
                            if (DBUtility.ORM.Save(m))
                                json.info = "ok";
                            else
                                json.info = "保存失败";
                        }
                        else
                        {
                            json.info = "保存失败！原因：已存在登录名为：" + loginName + " 的用户";
                        }
                    }
                    log.Sys_log.Info("用户管理：修改用户,操作结果：" + json.info);
                }
                else
                {
                    Model.JH_SYS_USER user = new Model.JH_SYS_USER();
                    string strWhere = string.Format(" UserName='{0}'", loginName);
                    user = Repository.BaseBll<Model.JH_SYS_USER>.Get(user, strWhere);
                    if (user == null)
                    {
                        m.UserPassword = Common.Utility.GetMd5HashCode("123123");
                        if (DBUtility.ORM.Add(m))
                            json.info = "ok";
                        else
                            json.info = "保存失败";
                    }
                    else
                    {
                        json.info = "保存失败！已存在登录名为：" + loginName + " 的用户";
                    }
                    log.Sys_log.Info("用户管理：添加用户,操作结果：" + json.info);
                }
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                string jsonStr = JsonConvert.SerializeObject(json);
                c.Response.Write(jsonStr);
            }
        }

        /// <summary>
        /// 编辑用户时获取用户信息
        /// </summary>
        /// <param name="c"></param>
        protected void Edit(HttpContext c)
        {
            try
            {
                string userID = c.Request["UserID"] ?? "";
                Model.JH_SYS_USER model = new Model.JH_SYS_USER();
                model.UserID = int.Parse(userID);
                var m = DBUtility.ORM.Get(model);
                c.Response.Write(JsonConvert.SerializeObject(m));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="c"></param>
        protected void Delete(HttpContext c)
        {
            try
            {
                string userID = c.Request["UserID"] ?? "";
                string[] str = userID.Split(',');
                Model.JH_SYS_USER model = new Model.JH_SYS_USER();
                int t = 0;
                foreach (var item in str)
                {
                    model.UserID = int.Parse(item);
                    if (DBUtility.ORM.Remove(model))
                        t += 1;
                }
                if (t > 0)
                    json.info = "ok";
                else
                    json.info = "删除失败，请重试！";
                log.Sys_log.Info("用户管理：删除用户,操作结果：" + json.info);
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="c"></param>
        protected void Reset(HttpContext c)
        {
            try
            {
                string userID = c.Request["UserID"] ?? "";
                string[] str = userID.Split(',');
                Model.JH_SYS_USER model = new Model.JH_SYS_USER();
                int t = 0;
                foreach (var item in str)
                {
                    model.UserID = int.Parse(item);
                    model = DBUtility.ORM.Get(model) as Model.JH_SYS_USER;
                    model.UserPassword = Common.Utility.GetMd5HashCode("123123");
                    if (DBUtility.ORM.Save(model))
                        t += 1;
                }
                if (t > 0)
                    json.info = "ok";
                else
                    json.info = "重置密码失败，请重试！";
                log.Sys_log.Info("用户管理：重置密码,操作结果：" + json.info);
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
                json.info = ex.Message;
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="c"></param>
        protected void EditPassword(HttpContext c)
        {
            try
            {
                Model.JH_SYS_USER user = SetCookie.GetUserInfo();
                string oldPwd = c.Request["oldpwd"] ?? "";
                string newPwd = c.Request["newpwd"] ?? "";
                string FullName = c.Request["gysname"] ?? "";
                string Phone = c.Request["Phone"] ?? "";
                if (Common.Utility.GetMd5HashCode(oldPwd) == user.UserPassword)
                {
                    user = DBUtility.ORM.Get(user) as Model.JH_SYS_USER;
                    user.UserPassword = Common.Utility.GetMd5HashCode(newPwd);
                    user.FullName = FullName;
                    user.Phone = Phone;
                    if (DBUtility.ORM.Save(user))
                        json.info = "ok";
                    else
                        json.info = "密码修改失败，请重试";
                    log.Sys_log.Info("用户管理：密码修改,操作结果：" + json.info);
                }
                else
                {
                    json.info = "原密码错误";
                }
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 验证原密码
        /// </summary>
        /// <param name="c"></param>
        protected void OldPassword(HttpContext c)
        {
            try
            {
                Model.JH_SYS_USER user = SetCookie.GetUserInfo();
                string oldPwd = c.Request["oldpwd"] ?? "";
                string newPwd = c.Request["newpwd"] ?? "";
                if (Common.Utility.GetMd5HashCode(oldPwd) == user.UserPassword)
                {
                    json.info = "ok";
                }
                else
                {
                    json.info = "原密码错误";
                }
                c.Response.Write(JsonConvert.SerializeObject(json));
            }
            catch (Exception ex)
            {
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