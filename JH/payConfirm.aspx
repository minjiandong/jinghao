<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payConfirm.aspx.cs" Inherits="JH.payConfirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width,initial-scale=0.5,maximum-scale=0.5,user-scalable=0"/> 
    <title><%=title %></title>
     <script type="text/javascript">
    (function(f,j){var i=document,d=window;var b=i.documentElement;var c;function g(){var k=b.getBoundingClientRect().width;if(!j){j=1080}if(k>j){k=j}var l=k*100/f;b.style.fontSize=l+"px"}g();d.addEventListener("resize",function(){clearTimeout(c);c=setTimeout(g,300);if(i.body.offsetHeight<i.body.offsetWidth){document.getElementsByTagName("body")[0].classList.add("crosswise")}else{document.getElementsByTagName("body")[0].classList.remove("crosswise")}},false);d.addEventListener("pageshow",function(k){if(k.persisted){clearTimeout(c);c=setTimeout(g,300)}},false)})(750,1080);
    </script><link href="/css/pay.css" rel="stylesheet" />

    <style>
    *{
        margin:0;
        padding:0;
    }
    ul,ol{
        list-style:none;
    }
    body{
        font-family: "Helvetica Neue",Helvetica,Arial,"Lucida Grande",sans-serif;
    }
    .hidden{
        display:none;
    }
    .new-btn-login-sp{
        padding: 1px;
        display: inline-block;
        width: 75%;
    }
    .new-btn-login {
        background-color: #02aaf1;
        color: #FFFFFF;
        font-weight: bold;
        border: none;
        width: 100%;
        height: 30px;
        border-radius: 5px;
        font-size: 16px;
    }
    #main{
        width:100%;
        margin:0 auto;
        font-size:14px;
    }
    .red-star{
        color:#f00;
        width:10px;
        display:inline-block;
    }
    .null-star{
        color:#fff;
    }
    .content{
        margin-top:5px;
    }
    .content dt{
        width:100px;
        display:inline-block;
        float: left;
        margin-left: 20px;
        color: #666;
        font-size: 13px;
        margin-top: 8px;
    }
    .content dd{
        margin-left:120px;
        margin-bottom:5px;
    }
    .content dd input {
        width: 85%;
        height: 28px;
        border: 0;
        -webkit-border-radius: 0;
        -webkit-appearance: none;
    }
    #foot{
        margin-top:10px;
        position: absolute;
        bottom: 15px;
        width: 100%;
    }
    .foot-ul{
        width: 100%;
    }
    .foot-ul li {
        width: 100%;
        text-align:center;
        color: #666;
    }
    .note-help {
        color: #999999;
        font-size: 12px;
        line-height: 130%;
        margin-top: 5px;
        width: 100%;
        display: block;
    }
    #btn-dd{
        margin: 20px;
        text-align: center;
    }
    .foot-ul{
        width: 100%;
    }
    .one_line{
        display: block;
        height: 1px;
        border: 0;
        border-top: 1px solid #eeeeee;
        width: 100%;
        margin-left: 20px;
    }
    .am-header {
        display: -webkit-box;
        display: -ms-flexbox;
        display: box;
        width: 100%;
        position: relative;
        padding: 7px 0;
        -webkit-box-sizing: border-box;
        -ms-box-sizing: border-box;
        box-sizing: border-box;
        background: #1D222D;
        height: 50px;
        text-align: center;
        -webkit-box-pack: center;
        -ms-flex-pack: center;
        box-pack: center;
        -webkit-box-align: center;
        -ms-flex-align: center;
        box-align: center;
    }
    .am-header h1 {
        -webkit-box-flex: 1;
        -ms-flex: 1;
        box-flex: 1;
        line-height: 18px;
        text-align: center;
        font-size: 18px;
        font-weight: 300;
        color: #fff;
        
    }
</style>
</head>
 <body class="loading">     <form id="form1" runat="server">      <div class='square-spin'>
       <div></div>
    </div>
    <div class='paymentStep2'>
        <div class='payment-confirm'>
            <p><span class='tit'>待付金额</span><span class='price'>￥<%=totalPrice %></span></p>
            <p><span class='tit'>商品名称</span><span class='goods-name'><%=title %>智能导游授权码</span></p>
        </div>
        <div class="payment-way">
            <div class='aPay' channel="2">
               <img class='payment-logo' src='/images/zfb.png'  />
               <p class='payment-name'>支付宝</p>
               <p class='describe'>推荐使用支付宝支付</p>
              
            </div> 
        </div>  
        <asp:Button ID="BtnAlipay" name="BtnAlipay" class="new-btn-login" Text="去 支 付" Style="text-align: center;" runat="server" OnClick="BtnAlipay_Click"/>
    </div>
         </form>
     <script src="/Scripts/jquery-1.7.2.min.js"></script>  
      <script type="text/javascript"> 
          $(".payment-way>.aPay").show(); 
          $("body").removeClass('loading'); 
           
          if (!!navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/)) {
              $("body").bind('touchmove', function (event) {
                  event.preventDefault();
                  return false;
              })
          }
    </script>
</body>
</html>
