<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="JH.Order" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1,user-scalable=no">
  <meta name="renderer" content="webkit">
  <meta http-equiv="Cache-Control" content="no-siteapp"/>
  <!-- 启用360浏览器的极速模式(webkit) -->
  <meta name="renderer" content="webkit">
  <!-- 避免IE使用兼容模式 -->
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <!-- 针对手持设备优化，主要是针对一些老的不识别viewport的浏览器，比如黑莓 -->
  <meta name="HandheldFriendly" content="true">
  <!-- 微软的老式浏览器 -->
  <meta name="MobileOptimized" content="320">
  <!-- uc强制竖屏 -->
  <meta name="screen-orientation" content="portrait">
  <!-- QQ强制竖屏 -->
  <meta name="x5-orientation" content="portrait">
  <!-- UC强制全屏 -->
  <meta name="full-screen" content="yes">
  <!-- QQ强制全屏 -->
  <meta name="x5-fullscreen" content="true">
  <!-- UC应用模式 -->
  <meta name="browsermode" content="application">
  <!-- QQ应用模式 -->
  <meta name="x5-page-mode" content="app">
  <!-- windows phone 点击无高光 -->
  <meta name="msapplication-tap-highlight" content="no">
    <title>订单管理</title>
    <style type="text/css">
        li {list-style-type:none;}
        body{ background-color:#d5d2d2;margin:0px;padding:0px;}
    </style>
</head>
<body>
    <%
        System.Data.DataTable dt = DBUtility.DALHelper.myHelper.ExecuteDataTable(System.Data.CommandType.Text, "select * from zy_scenic_orders as a,zy_scenics as b,zy_users as c where a.scenic_id=b.scenic_id and a.user_id=c.user_id and a.is_pay=1 and c.openid='" + openid + "' order by add_time desc");
        for (int i = 0; i < dt.Rows.Count; i++)
        { 
         %>
    <div style="height:150px; width:95%; margin:5px; background-color:#ffffff; border-radius:5px; padding-right:5px; padding-top:5px;  padding-left:5px;  ">
    
            <div style="height:40px; line-height:40px; ">
                
                <img src="https://cloud.jhlxw.com/app<%=dt.Rows[i]["scenic_img"].ToString() %>" style="float:left;  width:35px;height:35px; border-radius:50%;" />
                <span style="float:left; margin-left:5px;"><%=dt.Rows[i]["scenic_name"].ToString() %>></span>
                <span style="color:#808080; float:right; ">已授权（<%=s(dt.Rows[i]["pay_type"].ToString()) %>）</span>
            </div>
            <div style="margin-left:50px; line-height:25px; height:50px;border-top:1px dotted #808080;">
                <span style="color:#808080;"><%=dt.Rows[i]["scenic_name"].ToString() %>的智能导游授权 <br />（自动定位讲解）</span>
                <span style="float:right; ">1件</span>
            </div>
            <div style="height:25px; line-height:25px;">
                <span style="float:right; color:#808080;">共1件商品，实付￥<%=dt.Rows[i]["money"].ToString() %></span>
            </div>
           <div style="height:25px;  line-height:25px;">
                <span style="float:left; ">购买时间：<%=gettime(dt.Rows[i]["add_time"].ToString()) %></span>
            </div>
        
    </div>
    <%} %>
</body>
</html>
