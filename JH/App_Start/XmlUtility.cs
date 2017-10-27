using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// XmlUtility 的摘要说明
/// </summary>
public class XmlUtility
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="json"></param>
    /// <param name="rootName"></param>
    /// <returns></returns>
    public static XDocument ParseJson(string json, string rootName)
    {
        return JsonConvert.DeserializeXNode(json, rootName);
    }
}