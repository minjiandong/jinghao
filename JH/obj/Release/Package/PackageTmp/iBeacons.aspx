<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iBeacons.aspx.cs" Inherits="JH.iBeacons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>iBeacons接口设备测试</title> 
     <script src="https://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
 
    <script>
        try {
            wx.config({
                beta: true,
                debug: false,
                appId: '<%=appId%>',
                timestamp: '<%=timestamp%>',
                nonceStr: '<%=nonceStr%>',
                signature: '<%=signature%>',
                jsApiList: [
                   'startSearchBeacons', 'stopSearchBeacons', 'onSearchBeacons'
                ]
            });

            wx.ready(function () { 
                wx.startSearchBeacons({
                    ticket: "",
                    complete: function (argv) {
                        alert("1" + JSON.stringify(argv));
                    }
                });
                wx.stopSearchBeacons({
                    complete: function (res) {
                        alert("2" + JSON.stringify(res));
                    }
                }); 
                wx.onSearchBeacons({
                    complete: function (argv) {
                        alert("3" + JSON.stringify(argv));
                    }
                });
            });
        } catch (e) {
            alert(e);
        }

</script>
</head>
<body>
    
</body>
</html>
