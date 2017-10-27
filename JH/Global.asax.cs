using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace JH
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //判断正在请求页的用户的身份验证信息是否为空
            if (HttpContext.Current.User != null)
            {
                //判断用户是否已经进行了身份验证 
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //判断当前用户身份验证的方式是否为Forms身份验证方式
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        //获得进行了Forms身份验证的用户标识
                        FormsIdentity UserIdent = (FormsIdentity)(HttpContext.Current.User.Identity);
                        //从身份验证票中获得用户数据 
                        string UserData = UserIdent.Ticket.UserData;
                        //分割用户数据得到用户角色数组 
                        string[] rolues = UserData.Split(',');
                        //从用户标识和角色组初始化GenericPrincipal类
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(UserIdent, rolues);
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}