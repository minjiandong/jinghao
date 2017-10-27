<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="JH.user" %>

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
	<title>个人中心</title>


   
</head>
<body>
    <div id="user-b">
		<!--html5 nav-->
		<nav class="j-nav navbar">
	        <div class="logo fl">
				<a href="index"></a>
			</div>
			<span class="user-title">个人中心</span>
			<!--<div class="shopping-cart fr">
				<a href="index.html"></a>
			</div>-->
	    </nav>
		<section class="m-component-user" id="m-user">
	        <div class="m-user-avatar text-center">
	            <span class="avatarPic"><img style="display: inline; " class="lazy img-circle" id="sm" src="<%=headimgurl %>"/></span>
	             <span><%=nickname %></span>
	        </div>
	        
	        <div class="head_list">
	        	<ul class="m-user-list">
	        		<li><%--<a href="pay">--%><span>￥<%=balance %></span><br>账户余额<%--</a>--%></li>
	        	<%--	<li><a href="#"><span class="bar"></span><span>4</span><br>优惠卷</a></li>
	        		<li><a href="#"><span class="bar"></span><span>2</span><br>我的订单</a></li>--%>
	        	</ul>
	        </div>
	        <ul class="m-user-content">
                <li>
	                <div class="m-user-item">
	                   <div class="user-order"><a href="/Order"><span class="pull-right">查看所有订单</span>我的订单</a></div>
	                </div>
	            </li>
	            <li>
	                <div class="m-user-item">
	                   <div class="user-score"><span class="pull-right">0571-29605717</span>联系客服</div>
	                </div>
	            </li>
	            <li class="m-user-footer">
	          <%--  浙ICP备8888888 浙江.杭州--%>
	            </li>
	        </ul>
	    </section>
	    <!--footer begin-->
		<footer class="footer">
	        <nav>
	            <ul>
	                <li><a href="index" class="nav-controller"><div class="fb-home"></div>首页</a></li>
	                <li><a href="travels" class="nav-controller"><div class="fb-list"></div>游记</a></li>
	                <li><a href="javascript:void(0);" class="nav-controller active"><div class="fb-user"></div>个人中心</a></li>
	            </ul>
	        </nav>
	    </footer>
	    <!--footer end-->
	</div>
</body>
</html>
