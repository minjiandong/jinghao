<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userbind.aspx.cs" Inherits="JH.userbind" %>

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
  <!-- 适应移动端end --> 
	<link rel="stylesheet" href="/shuiguo/css/frameui.css"/>
    <link rel="stylesheet" href="/shuiguo/css/index.css"/>
    <title>景好用户绑定</title>
</head>
<body>
     <div id="user-b">
		<!--html5 nav-->
		<nav class="j-nav navbar"> 
			<span class="user-title">用户绑定</span>
			<!--<div class="shopping-cart fr">
				<a href="index.html"></a>
			</div>-->
	    </nav>
		<section class="m-component-user" id="m-user">
	        <div class="m-user-avatar text-center">
	            <span class="avatarPic"><img style="display: inline;" class="lazy img-circle" src="<%=headimgurl %>" <%--src="images/user-img0.jpg"--%>></span>
	             <span><%=nickname %></span>
	        </div>  
           
	    </section>
         <div style="height:60px; line-height:60px; text-align:center;">
             <span style="font-weight:900;">绑定后您将享受智能云导游服务</span>
         </div>
  <form id="form1" runat="server">
                <asp:HiddenField ID="nickname1" runat="server" />
                <asp:HiddenField ID="headimgurl1" runat="server" />
                <asp:HiddenField ID="sex" runat="server"  />
                <asp:HiddenField ID="language" runat="server" />
                <asp:HiddenField ID="city" runat="server" />
                <asp:HiddenField ID="country" runat="server" />
                <asp:HiddenField ID="subscribe_time" runat="server" />
                <asp:HiddenField ID="province" runat="server" />
                <asp:HiddenField ID="openid1" runat="server" />
                <asp:HiddenField ID="unionid" runat="server" />
            <asp:Button ID="Button1" runat="server" CssClass="btn" style="width:98%; margin-left:1%; height:35px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px;" Text="确定绑定" OnClick="Button1_Click" />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                </form>
	</div>
	 
</body>
</html>
