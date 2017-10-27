<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buy.aspx.cs" Inherits="JH.buy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1,user-scalable=no" />
    <meta name="renderer" content="webkit" />
    <meta http-equiv="Cache-Control" content="no-siteapp" />
    <!-- 启用360浏览器的极速模式(webkit) -->
    <meta name="renderer" content="webkit" />
    <!-- 避免IE使用兼容模式 -->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!-- 针对手持设备优化，主要是针对一些老的不识别viewport的浏览器，比如黑莓 -->
    <meta name="HandheldFriendly" content="true" />
    <!-- 微软的老式浏览器 -->
    <meta name="MobileOptimized" content="320" />
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait" />
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait" />
    <!-- UC强制全屏 -->
    <meta name="full-screen" content="yes" />
    <!-- QQ强制全屏 -->
    <meta name="x5-fullscreen" content="true" />
    <!-- UC应用模式 -->
    <meta name="browsermode" content="application" />
    <!-- QQ应用模式 -->
    <meta name="x5-page-mode" content="app" />
    <!-- windows phone 点击无高光 -->
    <meta name="msapplication-tap-highlight" content="no" />
    <!-- 适应移动端end -->
    <title>智能导游授权</title>
    <link rel="stylesheet" href="/shuiguo/css/index.css" />
    <link href="/Content/bootstrap/bootstrap.min.css" rel="stylesheet" /> 
    <style type="text/css">
        .inputid {
            height: 40px;
            line-height: 40px;
            width: 200px;
            border-radius: 3px;
            border: 1px double #CCCCCC ;
            position: relative;
            margin-left: 10px;
            
        }
        body{
            font-size:14px;
            font-family:'Microsoft YaHei UI';
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="user-b">
            <!--html5 nav-->
            <nav class="j-nav navbar" style="min-height: 40px;">
                <div class="logo fl">
                    <a href="index"></a>
                </div>
                <span class="user-title">智能导游授权</span>
            </nav>
            <div style="height: 120px; width: 100%;position:relative; background-color: #FFFFFF;">
                <div style="float: left;position:absolute; margin-left: 10px; margin-bottom: 10px; margin-top: 10px;">
                    <img src="<%=_url %>" alt="" style="border-radius: 50%; height: 70px; width: 70px; border: 1px double #cccccc;" />
                </div>
                <div style="float: left;min-width:200px; position:absolute;left:90px; margin-left: 5px; margin-right: 5px; margin-top: 15px;">
                    <ul>
                        <li><span style="font-size: 2.0rem; font-weight: 900;"><%=_title %></span></li>
                        <li style=" height: 40px; overflow: hidden; color: #353434;"><span><%=_content %></span></li>
                        <li style="text-align: right; color: #13bb0b; margin-top:10px;margin-right:10px;">
                            <span style="float:left; font-size:15px; color:#000000; "><span style="color:#ff0000;">￥<%=m %></span>/张</span>
                            <a href="javascript:show();">购票须知</a></li>
                    </ul>
                </div>
            </div>
            <div style="height: 40px; line-height: 40px; text-align: left; margin-top: 10px; width: 100%; background-color: #FFFFFF;">
                <span style="float: left; margin-left: 10px;">账户余额</span>
                <span style="float: right; margin-right: 15px; color: #ff0000;">￥<%=balance %></span>
            </div>

            <div style="height: 40px; line-height: 40px; text-align: left; width: 100%;  margin-top:10px;  background-color: #FFFFFF;">
                <span style="float: left; margin-left: 10px;">购买数量</span>
                <span style="float: right; margin-right: 10px; padding-left: 5px; padding-right: 5px; border-radius: 5px;">1 张</span>
            </div>

            <%--<div style="height: 50px; line-height: 50px; margin-top: 10px; background-color: #FFFFFF; border-bottom: 1px double #CCCCCC; margin-top: 10px;">
                <div><span style="font-size: 1.3rem; font-weight: 900; margin-left: 10px;">支付方式：</span></div>
            </div>--%>
            <div style="height: 120px; padding-top: 15px;margin-top: 10px; background-color: #FFFFFF;">
                <ul>
                    <li style=" height: 45px;  border-bottom:1px double #ebe8e8; margin-left: 10px;">
                        <img alt="" src="/images/weixin.png" style="height: 40px; width: 40px;" />
                        <span style="padding-left: 10px; font-size:18px;">微信支付</span>
                       
                        <span style="float: right; margin-right: 20px;">
                            <input type="checkbox" id="sz1" style="height: 20px; width: 20px;margin-top: 10px; " name="doc-radio-1" value="option1" onclick="sz('0');" checked="checked" />
                        </span>
                    </li>
                    <li style="height: 50px;line-height:50px; margin-left: 10px;<%=_yue%>">
                        <img alt="" src="/images/zhifu.png" style="height: 40px; width: 40px;" />
                        <span style="padding-left: 10px;font-size:18px;">余额支付</span>
			<span style="float: right; margin-right: 20px;">
			<input type="checkbox" style="margin-top: 20px; height: 20px; width: 20px;" id="sz2" name="doc-radio-1" value="option2" onclick="sz('1');" />
			</span>

                    </li>
                </ul>
                <input type="hidden" id="balances" value="<%=balance %>" runat="server" />
                <input type="hidden" id="Hidden1" value="<%=ZFmoney %>" runat="server" />
                <input type="hidden" id="statie" value="0" runat="server" />
            </div>



            <footer class="footer" style="height: 45px;">
                <nav>
                    <div style="font-size: 1.2em; text-align: left; color: #ff6a00; margin-left: 10px; margin-top: 10px; float: left;">
                        <span>需支付金额：￥<%=m %></span>
                    </div>
                    <div style="float: right;">
                        <style type="text/css">
                            .butt {
                                border-radius: 0px;
                                background-color: #ffd800;
                                border: 0px;
                                font-size:1.5em;
                                color:#353434; 
				width:120px;
                            }
                        </style>
                        <asp:Button ID="Button1" runat="server" Text="立即支付" CssClass="btn btn-info butt" Height="45" OnClick="Button1_Click" />
                    </div>
                </nav>
            </footer>
        </div>
       
    </form>
    
    <script src="/Scripts/jquery-1.10.2.min.js"></script>
    <script src="/bootstrap/js/bootstrap.min.js"></script> 
    <script src="/Content/lib/ligerUI/js/ligerui.all.js"></script>
    <link href="/Content/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />   
    <script src="/Content/lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="/Content/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    
    
    <script src="/Scripts/ajax.js"></script>
    <script type="text/javascript">

        var show = function () {
            AlertInfo('购买须知：<br>（一）费用说明：<br>1、景好云导游不包含景区门票。<br>2、不含景区个人消费及其他未提及的费用。<br>（二）产品说明：<br>1、景好云导游可以24小时全天使用。<br>2、建议在景区景点现场使用，体验景好云导游的最佳效果。<br>（三）使用方法：<br>1、下载景好云导游APP，进入APP首页，可查看当前所有景区的电子语音讲解。<br>2、APP将自动定位到用户所在的景区，用户通过购买或输入激活码，来激活景区的电子讲解。<br>（四）退改政策：<br>景好云导游一经预订成功，不支持修改、退款，敬请谅解。');
        }

        var sz = function (s) {
            if (s == 0) {
                $("#sz2").prop('checked', false);
            }
            if (s == 1) {
                $("#sz1").prop('checked', false);
            }
            $("#<%=statie.ClientID%>").val(s);
        }
    </script>
</body>
</html>
