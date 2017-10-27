<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alipayBuy.aspx.cs" Inherits="JH.alipayBuy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width,initial-scale=0.5,maximum-scale=0.5,user-scalable=no"/>
    <title><%=_title %>导游授权</title>
    <script type="text/javascript">
        (function(f,j){var i=document,d=window;var b=i.documentElement;var c;function g(){var k=b.getBoundingClientRect().width;if(!j){j=1080}if(k>j){k=j}var l=k*100/f;b.style.fontSize=l+"px"}g();d.addEventListener("resize",function(){clearTimeout(c);c=setTimeout(g,300);if(i.body.offsetHeight<i.body.offsetWidth){document.getElementsByTagName("body")[0].classList.add("crosswise")}else{document.getElementsByTagName("body")[0].classList.remove("crosswise")}},false);d.addEventListener("pageshow",function(k){if(k.persisted){clearTimeout(c);c=setTimeout(g,300)}},false)})(750,1080);
    </script>
    <link href="css/pay.css" rel="stylesheet" />
</head>
<body class="loading">
    <div class='square-spin'>
       <div></div>
    </div>
    <div class='paymentStep1'>
		<div class='good-info ' >
			<div class='viwe-logo' style="background-image:url('<%=_url%>')" alt="<%=_title %>"></div>
			<div class='viwe-info'>
				<h2 class='viwe-title text-omit'><%=_title %></h2>
				<p class='slogon text-omit'><%=_content %></p>
				<span class='price'>￥<sapn id='price'><%=cost_money %></sapn>元<span class='unit'>/张</span></span>
				<a href='/sysConf/buy_ticket_notice' class='should-know' >购票须知></a>
			</div>
		</div>
		<ul class="goods-info hide">

			<li data-price="2.0" data-scenic="true" data-id="<%=Scenic_id %>">
				<p class="scenicName"><span><%=_title %></span>景区智能导游</p>
				<p class="info">仅限于<%=_title %>导游授权</p>
				<span class='price'><span>￥</span><%=cost_money %></span>
			</li>
		</ul>
		<form id='inputInfo' method="post">	
			<div class="goods-setting hideDate">		


			   <div>
			   		<label for="goods-number">购买数量</label>
			   		<span  class='number-unit '>
			   		   <span class='minus disable' id="minusGoods1">-</span> 
			   			<input type="tel" name='goods-number' id='goods-number' disabled="disabled" value="1" min='1' max='99'  >  
			   			<span class='add ' id="addGoods1">+</span> 
			   		</span>
			   		
			   		
	          
			   </div>
			</div>

			<div class="userInfo">		
			   <div>
	   		   		<label for="userName">使用人</label>
	          		<input type="input" name="userName" id="userName" placeholder="不是必填项" autocomplete='off'><br>
			   </div>	

			   <div>
			   		<label for="telNumber">手机号</label>
			   		<input type="tel" name='telNumber' id="telNumber" placeholder="用于接收确认短信" autocomplete='off' />
			   </div>
			</div>
			<input type="hidden" name='price' id="price_input" value="<%=cost_money %>" />
    	  	<input type="hidden" name='totalPrice' id="totalPrice" value="0" /> 
		</form>
	    <div class='confirm-payment'>
	                       支付金额：￥<span id='count'><%=cost_money %></span>
	    	<div id="paymentNow" class='confirm-btn'>立即支付</div>
	    </div>    	
    </div>


 <!--  购票须知开始 -->
    <div class='need-know'>
         <div class='head'>
             <div class='title'>购票须知</div> 
             <p class='no-alter'>本产品预订后不支持修改。</p>
         </div>
         <div id='contentScroll'>
           <div class='content'>
               <p class='tit'>手机导游(不包含门票，授权码以短信方式发送)</p>
               <ul>
                  <li>
                 		   包含项目
                    <p>电子导游链接及语音讲解授权码。</p>
                  </li>
                  <li>
                 
                    		 使用说明

                    <p>① 景好云导游不包含景区门票。</p>

                    <p>② 不含景区个人消费及其他未提及的费用。</p>

                    <p>③ 景好云导游可以24小时全天使用。</p>

                    <p>④ 建议在景区景点现场使用，体验景好云导游的最佳效果。</p>

                    <p>⑤ 下载景好云导游APP，进入APP首页，可查看当前所有景区的电子语音讲解。</p>

                    <p>⑥ APP将自动定位到用户所在的景区，用户通过购买或输入激活码，来激活景区的电子讲解。</p>

                    <p>⑦ 景好云导游一经预订成功，不支持修改、退款，敬请谅解。</p>
                  </li>
               </ul>
           </div>
          </div>
         <button class='close'>关闭</button>
    </div>
 <!--购票须知结束--> 
    <script src="/Scripts/jquery-1.7.2.min.js"></script>
    <script src="/leaflet/iscroll.js"></script>
	<script type="text/javascript">

	    var groupList = [];
	    var priceList = [2.0];

	    var goodsNumber = $('#goods-number');
	    var addBtn = $("#addGoods");
	    var minusBtn = $("#minusGoods");
	    beginPayment({});

	    function beginPayment(viweObj) {

	        typeof viweObj == "object" ? '' : viweObj = eval("(" + viweObj + ")");
	        var obj = {};

	        obj.canAdd = true;
	        obj.canMinus = true;
	        obj.canInputNumber = true
	        $(".viwe-title.text-omit").text("<%=_title %>智能导游授权");
	        $("#scenicName").val("<%=_title %>智能导游授权");
	        $(".slogon.text-omit").html(obj.intro);
	        $(".viwe-info #price").text(obj.price);
	        goodsNumber.attr({
	            'min': obj.min ? obj.min : 1,
	            'max': obj.max ? obj.max : 99
	        }).val(obj.min ? obj.min : 1);

	        calculatePrice();

	        $(".goods-info .viwe-logo,.good-info .viwe-logo").attr('style', "background-image:url(" + "<%=_url%>)");
	        obj.canAdd === false && addBtn.addClass('disable');
	        obj.canMinus === false && minusBtn.addClass('disable');
	        obj.canInputNumber === false && goodsNumber.prop('readonly', true);
	        if (groupList.length > 0) {
	            var li = "";
	            for (var i = 0, len = groupList.length; i < len; i++) {
	                li += '<li data-price="' + priceList[i + 1] + '" data-id="' + groupList[i].id + '">'
                           + '<p class="scenicName"><span>' + groupList[i].name + '</span>景区智能导游</p>'
                           + '<p class="info">包含' + groupList[i].name + '所有景区的导游授权</p>'
                           + '<span class="price"><span>￥</span>' + priceList[i + 1] + '</span>'
                        + '</li>'
	            }
	            $(".paymentStep1>.good-info").hide();
	            $(".paymentStep1>.goods-info").removeClass("hide").prepend(li);
	            setTimeout(function () { $(".paymentStep1>.goods-info").find("li:eq(0)").trigger("click"); }, 50)

	        }

	        $("body").removeClass('loading');
	        $("body>.paymentStep1").fadeIn();

	    }
	    $(".goods-info>li").bind("click", function () {
	        if (!$(this).hasClass(".choosed")) {
	            $(this).addClass("choosed");
	            $(this).siblings(".choosed").removeClass("choosed");
	            $("#price_input").val($(this).attr("data-price"));
	            $("input#scenicName").val($(this).find("p.scenicName").text());
	            calculatePrice();
	        }
	    })


	    $("#minusGoods").bind('click', function () { //购买数量-1效果实现
	        if ($(this).hasClass('disable')) {
	            return false;
	        }
	        var gNumber = parseInt(goodsNumber.val());
	        if (gNumber > 1) {
	            goodsNumber.val(gNumber - 1);
	        }
	        calculatePrice();

	    });

	    $("#addGoods").bind('touchstart', function () {//购买数量+1实现
	        if ($(this).hasClass('disable')) {
	            return false;
	        }
	        var gNumber = parseInt(goodsNumber.val());
	        if (goodsNumber.attr('max') && gNumber < parseInt(goodsNumber.attr('max'))) {
	            goodsNumber.val(gNumber + 1);
	        }
	        calculatePrice();
	    });

	    $("#goods-number").keydown(function (event) { //验证键盘输入
	        if (!((event.keyCode < 58 && event.keyCode > 47) || (event.keyCode < 106 && event.keyCode > 95) || event.keyCode == 8)) {  //键值只能是数字或者退格键
	            return false;
	        }
	        var gNumber = parseInt(goodsNumber.val());
	        if (event.keyCode === 8 && goodsNumber.length < 2) { //当表单的购买数量为一位数并且按下退格键时，将购买数量设为最小。
	            goodsNumber.val(goodsNumber.attr('min') || '1');
	            return false;
	        }
	        calculatePrice();
	    });

	    $("#goods-number").keyup(function (event) {  //键盘输入数值向上超出，调整为最大；向下超出，调整为最小。
	        goodsNumber.val(parseInt(goodsNumber.val()))
	        var gNumber = parseInt(goodsNumber.val());
	        if (gNumber > parseInt(goodsNumber.attr('max'))) {
	            goodsNumber.val(goodsNumber.attr('max'));
	        }
	        if (gNumber < parseInt(goodsNumber.attr('min'))) {
	            goodsNumber.val(goodsNumber.attr('min'));

	        }
	        if (event.keyCode === 8 && goodsNumber.length < 2) {
	            goodsNumber.select();
	        }
	        calculatePrice();
	    });



	    function calculatePrice() { //购买数量调整后，相应改变总价格. 控制增加和减小符号的可用状态 
	        $("#count").text(($("#price_input").val() * 100 * $("#goods-number").val() / 100));
	        $("#totalPrice").val(($("#price_input").val() * 100 * $("#goods-number").val() / 100));
	        if (parseInt(goodsNumber.val()) <= parseInt(goodsNumber.attr('min'))) {
	            minusBtn.addClass('disable');
	        } else {
	            minusBtn.removeClass('disable');
	        }
	        if (parseInt(goodsNumber.val()) >= parseInt(goodsNumber.attr('max'))) {
	            addBtn.addClass('disable');
	        } else {
	            addBtn.removeClass('disable');
	        }
	    }

	    $('#paymentNow').bind('click', function () { //提交验证
	        var mobile = $("#telNumber").val();
	        var ua = navigator.userAgent.toLowerCase();
	        var isWeixin = ua.indexOf('micromessenger') != -1;

	        var gNumber = parseInt(goodsNumber.val());
	        if (!gNumber || gNumber < parseInt(goodsNumber.attr('min')) || gNumber > parseInt(goodsNumber.attr('max'))) { //购买数量验证
	            alert('请填写正确购买的数量！')
	            goodsNumber.focus();
	            return false;
	        }
	        //if ($("#userName").val() === '') {//姓名非空验证
	        //    alert('请填写姓名！')
	        //    $("#userName").focus();
	        //    return false;
	        //}
	        if (!/^(17[0-9]|13[0-9]|14[0-9]|15[0-9]|18[0-9])\d{8}$/i.test(mobile)) { //手机号码验证
	            alert('请输入正确的手机号码！');
	            $("#telNumber").focus();
	            return false;
	        }
	        calculatePrice();
	        var scenicId = "&scenicId=" + "<%=Scenic_id%>";
	        if ($(".paymentStep1>.goods-info").is(":visible")) {
	            if ($(".paymentStep1>.goods-info>.choosed").length !== 1) {
	                alert('请选择需要够买的景区');
	                return false;
	            }
	            if ($(".paymentStep1>.goods-info>.choosed").attr("data-scenic") !== "true") {

	                scenicId = "&scenicGroupId=" + $(".paymentStep1>.goods-info>.choosed").attr("data-id");
	            }
	        }
	        var argument = "totalCount=" + gNumber + "&price=" + $("#price_input").val() + "&totalPrice=" + $("#totalPrice").val() + scenicId + "&userName=" + $("#userName").val() + "&title=<%=_title%>&account=" + mobile;
	        window.location.href = '/payConfirm?' + argument; 
	    });

	    // 购票须知交互开始 
	    var detailsScroll;
	    $(".paymentStep1 .should-know").bind('click', function () {
	        $(".need-know").fadeIn(200, function () {
	            if (detailsScroll && detailsScroll != "") {
	                detailsScroll.destroy();
	                detailsScroll = null;
	            }

	            detailsScroll = new IScroll("#contentScroll", {
	                scrollbars: true,
	                mouseWheel: true,
	                interactiveScrollbars: true,
	                shrinkScrollbars: 'scale',
	                click: true,
	                fadeScrollbars: true
	            });
	        });
	        return false;
	    })

	    $(".need-know .close").bind('click', function () {
	        $(".need-know").fadeOut();
	        return false;
	    })
	    // 购票须知交互结束
	    if (!!navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/)) {
	        $("body").bind('touchmove', function (event) {
	            event.preventDefault();
	            return false;
	        })
	    }


	</script>
</body>
</html>
