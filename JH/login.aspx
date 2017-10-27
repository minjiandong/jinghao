<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="JH.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=Common.Utility.SystemName %></title>
    <link  href="/NewUI/blue/css/public.css" rel="stylesheet" type="text/css"/>
    <link  href="/NewUI/blue/css/login.css" rel="stylesheet" type="text/css"/>
    <link href="/Content/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
     <link href="/Content/lib/ligerUI/skins/Tab/css/grid.css" rel="stylesheet" />
    <script src="/Content/lib/jquery/jquery-1.9.0.min.js"></script>
    <script src="/JJUI/js/bootstrap.min.js"></script>
    <script src="/Content/lib/ligerUI/js/ligerui.all.js"></script>
    <script src="/Content/lib/ligerUI/js/plugins/ligerTree.js"></script>
    <script src="/Content/lib/ligerUI/js/plugins/ligerGrid.js"></script>
    <script src="/Content/lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="/Content/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="/NewUI/lib/jquery.cookie.js"></script>
    <script src="/Scripts/ajax.js"></script>
    <script type="text/javascript">
        
        $(function () {
            
            $("#login").click(function () {
                if ($("#username").val() == "请输入用户名" || $("#username").val() == "") {
                    ErrorInfo("用户名不能为空！");
                    return;
                }
                if ($("#password").val() == "请输入密码" || $("#password").val() == "") {
                    ErrorInfo("密码不能为空！");
                    return;
                }
                $.ajax({
                    url: "login.ashx",
                    data: {
                        p: "login",
                        username: $("#username").val(),
                        password: $("#password").val(),
                        codekey: $("#codekey").val(),
                        r: Math.random()
                    },
                    success: function (data) {
                        if (data.info == "ok") {
                            location.href = "/Account/default";
                        } else {
                            $.ligerDialog.error(data.info);
                            $("#login").button('reset');
                            f_refreshtype();
                        }
                    }, beforeSend: function () {
                        $("#login").button('loading');
                    }, complete: function () {
                    }
                });
            });
        });
        //点击切换验证码
        function f_refreshtype() {
            var Image1 = document.getElementById("code");
            if (Image1 != null) {
                Image1.src = Image1.src + "?";
            }
        }
        //点击回车触发登录按钮
        document.onkeydown = function (event) {
            var e = event || window.event || arguments.callee.caller.arguments[0];
            if (e && e.keyCode == 13) {
                document.getElementById("login").click();
            }
        };
    </script>
</head>
<body class="login-body">
    <div class="wrapper login-warp">
        <div class="login-header-warp">
            <div class="login-header">
                <%--<img class="login-logo" src="/NewUI/blue/images/login-logo.png">--%>
                <span class="login-title"><%=Common.Utility.SystemName %></span>
                <img class="login_x_title" src="/NewUI/blue/images/login_x_title.png">
            </div>
        </div>
        <div class="login-center">
            <div class="login-main">
                <img class="login-img" src="/NewUI/blue/images/login-img1.png" />
                <div class="login-main-right">
                    <div class="login-box-title">
                        Hi!欢迎登录账户
                    </div>
                    <div class="login-box-form">
                        <input type="text" value="请输入用户名" id="username" onfocus="if (value =='请输入用户名'){value =''}" onblur="if (value ==''){value='请输入用户名'}" />
                        <input type="password" value="请输入密码" id="password" onfocus="if (value =='请输入密码'){value =''}" onblur="if (value ==''){value='请输入密码'}" />
                        <div>
                            <div style="display: inline; float: left;">
                                <input type="text" id="codekey" value="请输入验证码" onfocus="if (value =='请输入验证码'){value =''}" onblur="if (value ==''){value='请输入验证码'}" />
                            </div>
                            <div style="display: inline; float: left;">
                                <img alt="" src="png.aspx" id="code" onclick="f_refreshtype()" />
                            </div>
                        </div>
                        <input type="button" id="login" value="登   录" />
                    </div>
                </div>
            </div>
        </div>
        <div class="login-footer">
            技术支持：杭州景好网络科技有限公司
        </div>
    </div>
</body>
</html>