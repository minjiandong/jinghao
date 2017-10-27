<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showdetails.aspx.cs" Inherits="JH.showdetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta content="telephone=no" name="format-detection"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no"/>

    <meta name="renderer" content="webkit"/>
  <meta http-equiv="Cache-Control" content="no-siteapp"/>
  <!-- 启用360浏览器的极速模式(webkit) -->
  <meta name="renderer" content="webkit"/>
  <!-- 避免IE使用兼容模式 -->
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <!-- 针对手持设备优化，主要是针对一些老的不识别viewport的浏览器，比如黑莓 -->
  <meta name="HandheldFriendly" content="true"/>
  <!-- 微软的老式浏览器 -->
  <meta name="MobileOptimized" content="320"/>
  <!-- uc强制竖屏 -->
  <meta name="screen-orientation" content="portrait"/>
  <!-- QQ强制竖屏 -->
  <meta name="x5-orientation" content="portrait"/>
  <!-- UC强制全屏 -->
  <meta name="full-screen" content="yes"/>
  <!-- QQ强制全屏 -->
  <meta name="x5-fullscreen" content="true"/>
  <!-- UC应用模式 -->
  <meta name="browsermode" content="application"/>
  <!-- QQ应用模式 -->
  <meta name="x5-page-mode" content="app"/>
  <!-- windows phone 点击无高光 -->
  <meta name="msapplication-tap-highlight" content="no"/>
    <title><%=n_title %></title>
     <link rel="stylesheet" href="/shuiguo/css/frameui.css"/>
    <link rel="stylesheet" href="/shuiguo/css/widget/slider.css"/>
    <link rel="stylesheet" href="/shuiguo/css/index.css"/>
    <script src="/Scripts/jquery-1.10.2.js"></script>
</head>
<body>
          <!--html5 nav-->
    <div id="user-b">
	<nav class="j-nav navbar">
	        <div class="logo fl">
				<a href="javascript:window.history.go(-1);"></a>
			</div>
			<span class="user-title"><%=n_title %></span>
			<!--<div class="shopping-cart fr">
				<a href="index.html"></a>
			</div>-->
	    </nav>
    <div style="margin-bottom: 10px;"></div>
        <div id="tuan" class="tuan" style="padding-left:10px; padding-right:10px;">
             <%=content %>
           
        </div> 
         
        </div>
     <script type="text/javascript">
         $("#tuan img").each(function () {
             var url = this.src;
             $(this).attr("src", "https://cloud.jhlxw.com/app" + GetUrlRelativePath(url));

         });
         function GetUrlRelativePath(url) {
             var arrUrl = url.split("//");

             var start = arrUrl[1].indexOf("/");
             var relUrl = arrUrl[1].substring(start);//stop省略，截取从start开始到结尾的所有字符

             if (relUrl.indexOf("?") != -1) {
                 relUrl = relUrl.split("?")[0];
             }
             return relUrl;
         }

    </script>
</body>
</html>
