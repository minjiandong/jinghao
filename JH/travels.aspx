<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="travels.aspx.cs" Inherits="JH.travels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" class="no-js">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <meta charset="UTF-8"/>
  <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1,user-scalable=no"/>
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
  <!-- 适应移动端end -->
	<title>游记</title>
    <link rel="stylesheet" href="/shuiguo/css/frameui.css"/>
    <link rel="stylesheet" href="/shuiguo/css/widget/slider.css"/>
    <link rel="stylesheet" href="/shuiguo/css/index.css"/>
    <style type="text/css">
        .bg {
            position: absolute;
            left:0px;
            bottom:0px;
            width:100%;
            height:100%;
            background: #000;
            FILTER: alpha(opacity=10);
            opacity: 0.1;
            -moz-opacity: 0.1; 
            z-index: 1;
            border-radius:10px; 
        }
    </style>
</head>
<body>
         <!--html5 nav-->
    <div id="user-b">
	<nav class="j-nav navbar">
	        <div class="logo fl">
				<a href="index"></a>
			</div>
			<span class="user-title">游记</span>
			<!--<div class="shopping-cart fr">
				<a href="index.html"></a>
			</div>-->
	    </nav>
    <div style="margin-bottom: 10px;"></div>
        <div id="tuan" class="tuan" style="padding: 0px 10px;">
             <%
                 List<Model.JH_NEWS> list = Repository.BaseBll<Model.JH_NEWS>.GetList("n_type='1'");
                foreach (var item in list)
                {
                 %>
            <div class="tuan_g" data-vtuan="0" data-cat="1" data-num="12058">
                <i></i>
                <a href="show?id=<%=item.id %>">
                    <div class="tuan_g_img" style="position:relative;"> 
                        <img src="<%=item.n_img %>"/>
                        <div class="bg">  
                        </div>
                    </div>
                    <div class="tuan_g_info">
                        <p class="tuan_g_name"><%=item.n_title %></p>
                        <p style="display: black;" class="tuan_g_cx">
                            <%=item.n_abstract %>
                        </p>
                    </div> 
                </a>
            </div>
            <%} %> 
           
        </div>


        <!--footer begin-->
        <footer class="footer">
            <nav>
                <ul>
                    <li><a href="index" class="nav-controller" style="font-size:1.4rem"><div class="fb-home"></div>首页</a></li> 
	                <li><a href="javascript:void(0);" class="nav-controller active" style="font-size:1.4rem"><div class="fb-list"></div>游记</a></li>
	                <li><a href="user" class="nav-controller" style="font-size:1.4rem"><div class="fb-user" ></div>个人中心</a></li>
                </ul>
            </nav>
        </footer>
        <!--footer end-->
        <!--引入js资源-->
        <script src="/shuiguo/js/jquery.min.js"></script>
        <script src="/shuiguo/js/amazeui.js"></script>
        <script src="/shuiguo/js/handlebars.min.js"></script>
        <script src="/shuiguo/js/amazeui.widgets.helper.js"></script>
       <script src="/Scripts/ajax.js"></script>
        <script type="text/javascript">
            window.onload = function () {
                $("#categoryMenu li").addClass("route");
                $.getScript('http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js', function (_result) {
                    if (remote_ip_info.ret == '1') {
                       
                        if (getUrlParam("city") == null) {
                            location.href = "?city=" + remote_ip_info.city;
                        }  
                        $("#cityid").html(remote_ip_info.city);
                    } else {
                        alert('未能获取到当前位置！');
                    }
                });
            }
        </script>
        </div>
</body>
</html>
