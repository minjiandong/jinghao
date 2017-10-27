using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

 
    public class SetCookie
    {
        static string CookieName = "JH_COOKIE";
        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="strID">用户ID</param>
        /// <param name="strtime">过期时间（分钟）</param>
        /// <param name="roles">允许访问的目录</param>
        /// <returns></returns>
        public static string WriteCookie(string strID, int strtime, string roles)
        {
            HttpCookie sysck = new HttpCookie(CookieName);
            sysck.Path = "/";
            sysck.Expires = DateTime.Now.AddMinutes(strtime);
            sysck.Values.Add("USER_ID", strID);
            HttpContext.Current.Response.Cookies.Add(sysck);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, strID, DateTime.Now, DateTime.Now.AddMinutes(strtime), false, roles);
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            HttpCookie sysck1 = new HttpCookie(FormsAuthentication.FormsCookieName);
            sysck1.Value = cookieValue;
            sysck1.Expires = DateTime.Now.AddMinutes(strtime);
            HttpContext.Current.Response.Cookies.Add(sysck1);
            return FormsAuthentication.GetRedirectUrl(strID, true);
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="value">Cookie值，名称</param>
        public static void DelCookie(string strUrl)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];//UserCookieName
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(-1);
                cookie.Values.Clear();
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
            }
            System.Web.Security.FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect(strUrl);
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="value">Cookie值，名称</param>
        public static void DelCookieNourl()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddMinutes(-1);
                cookie.Values.Clear();
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
            }
            System.Web.Security.FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 读取Cookie值
        /// </summary>
        /// <param name="str">名称</param>
        /// <param name="value">Cookie值，名称</param>
        /// <returns></returns>
        public static string ReadCookid(string str)
        {
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
                return HttpContext.Current.Request.Cookies[CookieName][str].ToString();
            else
                return null;
        }

        /// <summary>
        /// 当前用户编号
        /// </summary>
        public static int USER_ID
        {
            get
            {
                int i = 0;
                string id = ReadCookid("USER_ID");
                int.TryParse(id, out i);
                return i;
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static Model.JH_SYS_USER GetUserInfo()
        {
            try
            {
                Model.JH_SYS_USER model = new Model.JH_SYS_USER();
                model.UserID = SetCookie.USER_ID;
                Model.JH_SYS_USER m = Repository.BaseBll<Model.JH_SYS_USER>.Get(model);
                if (m != null)
                    return m;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.Sys_log.Error(ex.Message);
                return null;
            }
        }
    }
 