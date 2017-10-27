using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

/// <summary>
/// EventMessage 的摘要说明
/// </summary>
public class EventMessage
{
    /// <summary>
    /// 微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。
    /// </summary>
    public string signature { get; set; }
    /// <summary>
    /// 时间戳
    /// </summary>
    public string timestamp { get; set; }
    /// <summary>
    /// nonce
    /// </summary>
    public string nonce { get; set; }
    /// <summary>
    /// 随机字符串
    /// </summary>
    public string echostr { get; set; }
    /// <summary>
    /// 开发者微信号
    /// </summary>
    public string ToUserName { get; set; }
    /// <summary>
    /// 发送方帐号（一个OpenID）
    /// </summary>
    public string FromUserName { get; set; }
    /// <summary>
    /// 消息创建时间 （整型）
    /// </summary>
    public string CreateTime { get; set; }
    /// <summary>
    /// 消息类型
    /// </summary>
    public string MsgType { get; set; }
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    public string MsgId { get; set; }
    /// <summary>
    /// 事件
    /// </summary>
    public string Event { get; set; }
    /// <summary>
    /// 事件值
    /// </summary>
    public string EventKey { get; set; }

    public EventMessage() { }
    public static EventMessage LoadFromXml(string RequestXml,HttpContext c)
    { 
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(RequestXml);
        XmlNode root = doc.FirstChild;
        EventMessage m = new EventMessage();
        m.MsgType = root["MsgType"].InnerText;
        if (m.MsgType == "event")
        {
            m.Event = root["Event"].InnerText;
            m.ToUserName = root["ToUserName"].InnerText;
            m.FromUserName = root["FromUserName"].InnerText;
            if (m.Event == "CLICK")
            {
                m.EventKey = root["EventKey"].InnerText;
            }
        }
        else
        {
            m.ToUserName = root["ToUserName"].InnerText;
            m.FromUserName = root["FromUserName"].InnerText;
            m.Content = root["Content"].InnerText;
        }    
        //m.EventKey = root["EventKey"].InnerText;
        m.signature = c.Request["signature"] ?? "";
        m.timestamp = c.Request["timestamp"] ?? "";
        m.nonce = c.Request["nonce"] ?? "";
        return m;
    }

}