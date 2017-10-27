//using System;
//using System.Collections.Generic;

//using System.Web;

///// <summary>
///// EventHandler 的摘要说明
///// </summary>
//public class EventHandler 
//{
//    /// <summary>
//    /// 请求的xml
//    /// </summary>
//    private string RequestXml { get; set; }
//    /// <summary>
//    /// 构造函数
//    /// </summary>
//    /// <param name="requestXml"></param>
//    public EventHandler(string requestXml)
//    {
//        this.RequestXml = requestXml;
//    }
//    /// <summary>
//    /// 处理请求
//    /// </summary>
//    /// <returns></returns>
//    public string HandleRequest()
//    {
//        string response = string.Empty;
//        EventMessage em = EventMessage.LoadFromXml(RequestXml);
//        if (em != null)
//        {
//            switch (em.Event.ToLower())
//            {
//                case ("subscribe"):
//                    response = SubscribeEventHandler(em);
//                    break;
//                case "click":
//                    response = ClickEventHandler(em);
//                    break;
//            }
//        }

//        return response;
//    }
//    /// <summary>
//    /// 关注
//    /// </summary>
//    /// <param name="em"></param>
//    /// <returns></returns>
//    private string SubscribeEventHandler(EventMessage em)
//    {
//        //回复欢迎消息
//        TextMessage tm = new TextMessage();
//        tm.ToUserName = em.FromUserName;
//        tm.FromUserName = em.ToUserName;
//        tm.CreateTime = Common.GetNowTime();
//        tm.Content = "欢迎您关注***，我是大哥大，有事就问我，呵呵！\n\n";
//        return tm.GenerateContent();
//    }
//    /// <summary>
//    /// 处理点击事件
//    /// </summary>
//    /// <param name="em"></param>
//    /// <returns></returns>
//    private string ClickEventHandler(EventMessage em)
//    {
//        string result = string.Empty;
//        if (em != null && em.EventKey != null)
//        {
//            switch (em.EventKey.ToUpper())
//            {
//                case "BTN_GOOD":
//                    result = btnGoodClick(em);
//                    break;
//                case "BTN_TQ_BEIJING":
//                    result = searchWeather("beijing", em);
//                    break;
//                case "BTN_TQ_SHANGHAI":
//                    result = searchWeather("shanghai", em);
//                    break;
//                case "BTN_TQ_WUHAN":
//                    result = searchWeather("wuhai", em);
//                    break;
//                case "BTN_HELP":
//                    result = btnHelpClick(em);
//                    break;
//            }
//        }

//        return result;
//    }
//    /// <summary>
//    /// 赞一下
//    /// </summary>
//    /// <param name="em"></param>
//    /// <returns></returns>
//    private string btnGoodClick(EventMessage em)
//    {
//        //回复欢迎消息
//        TextMessage tm = new TextMessage();
//        tm.ToUserName = em.FromUserName;
//        tm.FromUserName = em.ToUserName;
//        tm.CreateTime = Common.GetNowTime();
//        tm.Content = @"谢谢您的支持！";
//        return tm.GenerateContent();
//    }
//    /// <summary>
//    /// 帮助
//    /// </summary>
//    /// <param name="em"></param>
//    /// <returns></returns>
//    private string btnHelpClick(EventMessage em)
//    {
//        //回复欢迎消息
//        TextMessage tm = new TextMessage();
//        tm.ToUserName = em.FromUserName;
//        tm.FromUserName = em.ToUserName;
//        tm.CreateTime = Common.GetNowTime();
//        tm.Content = @"查询天气，输入tq 城市名称\拼音\首字母";
//        return tm.GenerateContent();
//    }
//    /// <summary>
//    /// 查询天气
//    /// </summary>
//    /// <param name="cityName"></param>
//    /// <param name="em"></param>
//    /// <returns></returns>
//    private string searchWeather(string cityName, EventMessage em)
//    {
//        TextMessage tm = new TextMessage();
//        tm.Content = WeatherHelper.GetWeather(cityName);
//        //进行发送者、接收者转换
//        tm.ToUserName = em.FromUserName;
//        tm.FromUserName = em.ToUserName;
//        tm.CreateTime = Common.GetNowTime();
//        return tm.GenerateContent();
//    }
//}