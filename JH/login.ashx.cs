using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JH
{
    /// <summary>
    /// login1 的摘要说明
    /// </summary>
    public class login1 : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        Models.jsoninfo json = new Models.jsoninfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string mt = context.Request["p"] ?? "";
            try
            {
                switch (mt)
                {
                    case "login":
                        Login(context);
                        break;
                    case "exit":
                        Exit(context);
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
        /// 用户登录
        /// </summary>
        /// <param name="context"></param>
        private void Login(HttpContext context)
        {

            string name = context.Request["username"] ?? "";
            string pwd = context.Request["password"] ?? "";
            string code = context.Request["codekey"] ?? "";
            if (!string.IsNullOrEmpty(name))
            {
                if (context.Session["CheckCode"] == null)
                {
                    json.info = "验证码失效！";
                }
                else
                {
                    string codeKey = context.Session["CheckCode"].ToString();
                    if (codeKey == code)
                    {
                        Model.JH_SYS_USER user = new Model.JH_SYS_USER();
                        string strWhere = string.Format(" UserName='{0}' and UserState='1'", name);
                        user = Repository.BaseBll<Model.JH_SYS_USER>.Get(user, strWhere);
                        if (user != null)
                        {
                            if (user.UserPassword == Common.Utility.GetMd5HashCode(pwd))
                            {
                                SetCookie.WriteCookie(user.UserID.ToString(), 4320, "admin");
                                json.info = "ok";
                                log.Sys_log.Info("登录管理,登录用户：" + name + ",操作结果：" + json.info);
                            }
                            else
                            {
                                json.info = "密码错误！";
                                log.Sys_log.Error("登录管理,登录用户：" + name + ",操作结果：" + json.info);
                            }
                        }
                        else
                        {
                            json.info = "用户不存在！";
                            log.Sys_log.Error("登录管理,登录用户：" + name + ",操作结果：" + json.info);
                        }
                    }
                    else
                    {
                        json.info = "验证码错误！";
                        log.Sys_log.Error("登录管理,登录用户：" + name + ",操作结果：" + json.info);
                    }
                }
            }
            context.Response.Write(JsonConvert.SerializeObject(json));
        }
        
        /// <summary>
        /// 退出登录
        /// </summary>
        private void Exit(HttpContext context)
        {
            SetCookie.DelCookieNourl();
            json.info = "ok";
            json.type = "ok";
            log.Sys_log.Info("登录管理：退出登录，操作结果：" + json.info);
            context.Response.Write(JsonConvert.SerializeObject(json));

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