<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsApiPayPage.aspx.cs" Inherits="JH.example.JsApiPayPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/> 
    <title>微信支付</title>
    <link href="/Content/bootstrap.min.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.8.2.min.js"></script>
</head>

           <script type="text/javascript">

               //调用微信JS api 支付
               function jsApiCall()
               {
                   WeixinJSBridge.invoke(
                   'getBrandWCPayRequest',
                   <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                        WeixinJSBridge.log(res.err_msg);
                        var val = res.err_msg.split(':')[1];
                        if (val == "cancel"){
                            alert("你已取消支付！");
                            location.href='/index';
                        }else if(val == "fail"){
                            alert("支付失败！");
                            location.href='/index';
                        }else if (val == "ok") {
                            alert("恭喜你，支付成功！");
                            $.ajax({
                                url: "/Handler.ashx",
                                dataType: "json",
                                async: false,
                                data: { p:"buyhandler",id:'<%=Scenic_id%>',openid:'<%=unionid%>',money:<%=_money %>, r: Math.random() }, 
                                success: function (data) { 
                                    if (data.state == "ok")
                                        location.href = data.url;
                                    else
                                        location.href = "/"+data.url;
                                }
                            }); 
                        }
                     }
                    );
               }

               function callpay()
               {
                   if (typeof WeixinJSBridge == "undefined")
                   {
                       if (document.addEventListener)
                       {
                           document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                       }
                       else if (document.attachEvent)
                       {
                           document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                           document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                       }
                   }
                   else
                   {
                       jsApiCall();
                   }
               }
               
     </script>

<body style="background-color:#dedede; margin:0px; padding:0px;">
    <div style=" text-align:center; margin-top:10px;">
            <span style="font-size:20px; "><%=_title %></span>
            <br />
        <br />
        <br />
            <span style="font-size:40px;">￥<%=_money %></span> 
           <br />
	    </div>
        <div style="background-color:white; width:100%; height:45px; border-bottom:1px #cccccc double; border-top:1px #cccccc double; line-height:45px;">
            <span style="float:left;margin-left:10px; font-weight:900;">收款方</span>
            <span style="float:right;margin-right:10px;">景好云导游</span>
        </div>
        <div style="margin-top:10px;">
            <form runat="server" style="margin:0px; padding:0px;"> 
                <button type="button" onclick="callpay();" style="width:98%; margin-left:1%; height:35px; border-radius: 15px;background-color:#00CD00; border:0px #FE6714 solid; cursor: pointer;  color:white;  font-size:16px;">立即支付</button>
                </form>
        </div>
</body>
</html>