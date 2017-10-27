using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// MenuManager 的摘要说明
/// </summary>
public class MenuManager 
{
    /// <summary>
    /// 菜单文件路径
    /// </summary>
    private static readonly string Menu_Data_Path = System.AppDomain.CurrentDomain.BaseDirectory + "/Data/menu.txt";
    /// <summary>
    /// 获取菜单
    /// </summary>
    public static string GetMenu()
    {
        string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", Context.AccessToken);

        return JH.App_Start.HttpUtility.GetData(url);
    }
    /// <summary>
    /// 创建菜单
    /// </summary>
    public static void CreateMenu(string menu)
    {
        string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", Context.AccessToken);
        //string menu = FileUtility.Read(Menu_Data_Path);
        JH.App_Start.HttpUtility.SendHttpRequest(url, menu);
    }
    /// <summary>
    /// 删除菜单
    /// </summary>
    public static void DeleteMenu()
    {
        string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", Context.AccessToken);
        JH.App_Start.HttpUtility.GetData(url);
    }
  
}