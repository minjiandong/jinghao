<%@ WebHandler Language="C#" Class="WeiHandle" %>

using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

public class WeiHandle : IHttpHandler 
{
    
    Hashtable ht = new Hashtable();
    string sToken =  Common.Utility.sToken;
    string sAppID = Common.Utility.AppID;
    string sEncodingAESKey = Common.Utility.AppSecret;
    public void ProcessRequest (HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        if (context.Request.HttpMethod.ToLower() == "post")
        {
            RequestMsg(context);
        }
        else
        {
            //微信通过get请求验证api接口
            string echoStr = context.Request["echoStr"] ?? "";
            if (string.IsNullOrEmpty(echoStr))
            {
                context.Response.End();
            }
            if (CheckSignature(context))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    context.Response.Write(echoStr);
                    context.Response.End();
                }
            }
        }  
    }
    private void RequestMsg(HttpContext context)
    { 
        //接收信息流
        Stream requestStream = System.Web.HttpContext.Current.Request.InputStream;
        byte[] requestByte = new byte[requestStream.Length];
        requestStream.Read(requestByte, 0, (int)requestStream.Length);
        //转换成字符串
        string requestStr = Encoding.UTF8.GetString(requestByte);
        string signature = context.Request["signature"] ?? "";
        string timestamp = context.Request["timestamp"] ?? "";
        string nonce = context.Request["nonce"] ?? "";
        if (!string.IsNullOrEmpty(requestStr))
        {
            try
            {
                EventMessage em = EventMessage.LoadFromXml(requestStr, context);
                if (em != null)
                {
                    switch (em.MsgType)
                    {
                        case "event"://订阅 
                            userevent(em,context);
                            break;
                        case "click"://点击事件 
                            break;
                        case "text":
                            ManageStr(em);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DiskWriteErrorLog(ex.Message);
            } 
        }
    }
    /// <summary>
    /// datetime转换为unixtime
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }
    void ManageStr(EventMessage em)
    {
        int nowtime = ConvertDateTimeInt(DateTime.Now);
        string resxml = string.Empty;
        //if (em.Content.Trim() == "首页")
        //{
             resxml = "<xml><ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[您好，我暂时还无法识别文字]]></Content><FuncFlag>0</FuncFlag></xml>";
        //}
        HttpContext.Current.Response.Write(resxml);
    }
    void userevent(EventMessage em,HttpContext c)
    {
        int nowtime = ConvertDateTimeInt(DateTime.Now);
        string resxml = string.Empty;
        if (em.Event == "subscribe")
        {
            //resxml = "<xml><ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[您好，欢迎您使用景好云导游]]></Content><FuncFlag>0</FuncFlag></xml>";

            //resxml = "<xml><ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[image]]></MsgType><Image><MediaId><![CDATA[-Qjfaw6rvXatz9LQDtvsz9nFcpQkwoDLWppF5dopH5o8OdXuJ_gRELUEVLB2C8s8]]></MediaId></Image></xml>";
            
            resxml +="<xml>";
            resxml += "<ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName>";
            resxml += "<FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName>";
            resxml += "<CreateTime>" + nowtime + "</CreateTime>";
            resxml +="<MsgType><![CDATA[news]]></MsgType>";
            resxml +="<ArticleCount>1</ArticleCount>";
            resxml +="<Articles>";
            resxml +="<item>";
            resxml += "<Title><![CDATA[景好云导游]]></Title> ";
            resxml += "<Description><![CDATA[您好，欢迎您使用景好云导游]]></Description>";
            resxml += "<PicUrl><![CDATA[https://cloud.jhlxw.com/0308160050.jpg]]></PicUrl>";
            resxml += "<Url><![CDATA[https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx6fb51a67c530c955&redirect_uri=https://cloud.jhlxw.com/userbind.aspx&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect]]></Url>";
            resxml +="</item> ";
            resxml +="</Articles>";
            resxml += "</xml>"; 
        }
        else  if (em.Event == "CLICK")
        {
            if (em.EventKey == "download_app")
            {
                resxml = "<xml><ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[您好， 请点击https://www.pgyer.com/KRxa/地址进行下载。]]></Content><FuncFlag>0</FuncFlag></xml>";
            }
            if (em.EventKey == "about")
            {
                resxml = "<xml><ToUserName><![CDATA[" + em.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + em.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[杭州景好网络科技有限公司是一家专注于帮助旅游企业实现在线化，面向旅游行业提供旅游信息化整体解决方案的互联网技术开发公司。公司积极响应国家旅游局“智慧旅游”和“全域旅游”两大政策引领，依靠自身强大的管理团队、技术研发团队、设计团队、顾问团队，牢牢抓住“景区导览”和“全域导览”两大核心，开发一款提供位置服务的智能导游，精确定位、真人讲解、用户在线互动等独特功能。景好云导游是一款为游客打造更轻松、更方便、更舒适的出行方式的软件。助力景区一步跨入移动互联网时代。随着我国生活水平的不断提高，旅游业成为了我国第三产业发展的主力，旅游经济是我国国民经济不可缺少的重要部分。景好云导游以强大的研发技术团队为支撑，实现了景点精确定位，语音自动播放，资讯更新分享等功能，给游客带来全新旅游体验。]]></Content><FuncFlag>0</FuncFlag></xml>";
            } 
        }
        c.Response.Write(resxml);
    }
    /// <summary>
    /// 微信验证URL
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private bool CheckSignature(HttpContext context)
    {
        string signature = context.Request["signature"] ?? "";
        string timestamp = context.Request["timestamp"] ?? "";
        string nonce = context.Request["nonce"] ?? "";
        string[] ArrTmp = { sToken, timestamp, nonce };
        Array.Sort(ArrTmp);　　 //字典排序　
        string tmpStr = string.Join("", ArrTmp);
        tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        tmpStr = tmpStr.ToLower();
        if (tmpStr == signature)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 硬盘写入错误信息
    /// </summary>
    /// <param name="ex">应用程序在执行时的错误</param>
    public static void DiskWriteErrorLog(string ex)
    {
        string filePath = null;
        string Dir = HttpContext.Current.Server.MapPath("/ErrorLog/");// HttpContext.Current.Server.MapPath("~/ErrorLog/");
        if (!Directory.Exists(Dir))
        {
            Directory.CreateDirectory(Dir);
        }
        filePath = Dir + DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff") + ".log";
        using (StreamWriter sw = new StreamWriter(filePath, true, System.Text.Encoding.UTF8))
        {
            sw.WriteLine(ex);
            sw.Flush();
            sw.Close();
        }
    }

    
    
    public bool IsReusable
    {
        get {
            return false;
        }
    }

}