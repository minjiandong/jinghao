<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="JH.pay" %>

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
	<title>充值</title>
    <link href="/Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/shuiguo/css/index.css"/>
    <style type="text/css">
        .inputid {
				height: 40px;
				line-height: 40px; 
                width:200px;
				border-radius: 3px;
				border: 1px double #CCCCCC; 
                position: relative;
                margin-left:10px;
			}
    </style>
    <script type="text/javascript">
        var pays = function (str) {
            if (str == "weixin") {
                $("#payid").show();
                $("#payjhm").hide();
                $("#statie").val("weixin");
            } else {
                $("#payjhm").show();
                $("#payid").hide();
                $("#statie").val("jhm");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <div id="user-b">
    <!--html5 nav-->
    <nav class="j-nav navbar">
      <div class="logo fl">
        <a href="user"></a>
      </div>
      <span class="user-title">充值</span>
    </nav>
    <!-- 支付拼团 -->
    <div class="pay-box">
      <!-- 支付方式 -->
      <div class="pay-m">
           <div style="height:50px; line-height:50px; margin-top:10px;border-bottom:1px double #cccccc; background-color:#FFFFFF;">
          <div><span style="font-size:1.7rem; font-weight:900; margin-left:10px;">充值方式:</span></div>
      </div>
         <div style="height:100px;  background-color:#FFFFFF;margin-top:10px;">
           <ul>
               <li style="font-weight:900; height:50px; line-height:30px; margin-left:10px;"><img alt="" src="/images/weixin.png" style="height:25px; width:25px;" /><span style="padding-left:10px;">微信充值</span><span style="float:right; margin-right:20px;">  <input type="radio" checked="checked" name="doc-radio-1" value="option1" onclick="pays('weixin');"/></span></li>
               <li style="font-weight:900; height:50px; line-height:30px; margin-left:10px;"><img alt="" src="/images/zhifu.png" style="height:25px; width:25px;" /><span style="padding-left:10px;">余额充值</span><span style="float:right; margin-right:20px;"> <input type="radio" name="doc-radio-1" value="option2" onclick="pays('jhm');"/></span></li>
                
           </ul> 
  
       </div>
           <div class="pay-mu" id="payid">
              <input type="text" placeholder="请输入充值金额" id="weixin" class="inputid" />
          </div>
          <div class="pay-mu" id="payjhm" style="display:none;">
              <input type="text" placeholder="请输入激活码" id="jhm" class="inputid" />
          </div> 
           <input type="hidden" id="statie" /> 
          <button class="btn btn-danger" type="button" onclick="payss();">立即支付</button>   
      </div>  
    </div>
  </div>
    </form>
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script src="/Scripts/ajax.js"></script>
    <script type="text/javascript">
        var payss = function () {
            var val = "";
            var weixin = $("#weixin").val();
            var jhm = $("#jhm").val();
            var statie = $("#statie").val();
            if (statie == "weixin") {
                val = weixin;
            } else {
                val = jhm;
            }
            var data = {
                p: "pay",
                val: val,
                statie: statie,
                userid: getCookie("_usercode"),
                r: Math.random()
            }
            $.get("Handler.ashx", data, function (data) {
                if (data.info == "ok") {
                    alert("充值成功！");
                } else {
                    alert("充值失败！");
                }
            });
        }
        
    </script>
</body>
    
</html>
