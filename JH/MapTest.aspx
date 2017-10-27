<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapTest.aspx.cs" Inherits="JH.MapTest" %>
 
<!DOCTYPE HTML>
<html style="font-size:100px;">  
  <head>
    <meta content="telephone=no" name="format-detection" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=0.5,maximum-scale=0.5,user-scalable=no">
    <title>景好云导游</title> 
     <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=4c37e67bdb9969ca4ea8ed2c656f0893&plugin=AMap.ToolBar"></script>
   <script type="text/javascript">

       (function (f, j) {
           var i = document, d = window;
           var b = i.documentElement;
           var c;
           function g() {
               b.style.fontSize = 90 + "px";
           }
           g();
       })(750, 1080);

    </script>
    <link href="/web/favicon.ico" rel="icon" type="image/x-icon" />
    <link href="/leaflet/css/leaflet.min.css" rel="stylesheet" />
    <link href="/leaflet/css/index.min.css" rel="stylesheet" />
   
  </head>

  <body class=' no-popularize '>
	  <div class='browse-open'>
		     点击右上角菜单<br>
		     在默认浏览器中打开并立即体验
	  </div>
	  
      <div class='mapContainer19'>
        <div id="MapContainer">
        </div>
      </div>
      
	    <div id="disableyMove">
	      <audio id="videoPlay" guideinview='<%=start_Play %>' guideplayed='false'></audio>
	         
	        <div class='local-btn'></div>
	        <a href="#" class="error-correction" style="display:none;"></a>
	        <div class='zoomControl'>        	
	        	<span class='zoombtn add'></span>
	        	<span class='zoombtn minus'></span>
	        </div>
	        
	        <div id="recommendDom" class="recommend  ">
	        	<div class='ad-container'>
		            
	            </div>
	            <div class='help_icon' style="display:none;"></div>
	            					
	            	<div id="autoPlay" class='auto-play' style="top:3.26rem;"></div>	                
	            

   
	            
	                  
	        </div>    
	        <!--recommendDom-->
	
	        <div id='feedBackDom' class="feedbacks">
	            <div class='header'>
	            	
	                <div class='back'></div>
	                <div class='title'>投诉与反馈</div>
	                <div class="close-btn"></div>
	               
	            </div>
				<div  class='ul-choose-scroll '>
					<div class='feedBack-tip' >使用中遇到问题？可尝试联系客服哟！</div>
					<dl>
						<dt error-text='地图问题'><span class='error map'></span>	</dt>
						<dt error-text='语音问题'><span class="audio_error error"></span></dt>
						<dt error-text='功能问题'><span class="function_error error"></span></dt>
						<dt error-text='其他'><span class="others_error error"></span></dt>
					</dl>
					<div class="feedback_foot">
						<a class='feedBack-tle' href="tel:02029878842"></a>
					</div>
		         </div>
		         <div class='inupt_div'>
		         	<form action="#" id="backform">
		         	 <lable class='tel_number'>	
		         	 	      <span>手机号：</span>   	 		
		         	 		<input id="tel_number" name='tel_number' type="tel"  placeholder='请输入您的手机号' autocomplete='off'>
		         	 </lable>
		         	 <lable class='textarea_lable'>
		         	 	 <span>描 &nbsp;&nbsp;述：</span>
		         	 	 <textarea class='textarea' placeholder='请写下您的宝贵建议，驴小迹一定会认真改进的呢“^_^”！'></textarea>
		         	 </lable>
		         	 
						<div class='submit_nav'><div class='submit disable'></div>	</div>
					</form> 
					<div class='error_tip'>error-text</div>	
		         </div>

	        </div>
	        <!--feedbacks-->
	        <div id="viweDetails" class="viweDetails">
	                <div class="header">
	                    <div class='goToHere'></div>
	                    <h3></h3>
	                    <div id='CloseviweDetails' class="close-btn"></div>
	                </div>
	                <div id='DetailsContainerScroll'  class="container">
	                    <div  id="DetailsContainer">


	                                       
	                 </div>
	                </div>
	        </div>
	        <!--viweDetails-->
	        
	        <div id="helpViweDetails" class="viweDetails">
	                <div class="header">
	                    <h3>帮助中心</h3>
	                    <div id='helpCloseviweDetails' class="close-btn"></div>
	                </div>
	                <div id='helpViweDetailsContainerScroll'  class="container">
	                    <div  id="helpViweDetailsContainer"></div>
	                </div>
	        </div>
	        <!--viweDetails-->

	 
	        <!--autoPlay-confirm-->
            <div class='autoPlay-confirm' style="display:none;">
            <div class='closeAd'>
                <div id="adCloseBtn" class="ad-close-btn"></div>
            </div>
            <div class='advertisement'>



                <img src="/images/autoC-advertisement.png">


                <div class='looklook'></div>
            </div>

            <div id="confirmYes" class="confirm-on on"></div>

        </div>


	        <div class='reject-local-box'>
	            <div class='reject-header'>温馨提示</div>  
	            <div class="reject-content">您未授予地理位置的权限，我们无法为您开启自动导游及定位功能，请尝试使用其它浏览器打开或重新授予地理位置的权限。
	                    <div class='leave'></div>
	            </div>  
	        </div>
	        <!--reject-local-box-->
	<!--头部弹出层-->
        <div style="position: fixed; top: 0px; left: 0px; width:100%; line-height:35px; background-color:#ffffff;border-bottom:1px double #808080;" id="topdiv">
            <div style="height:10px; width:100%; background-color:#ff6a00;"></div>
            <div style=" border-radius:10%; position:absolute; top:40px; right:10px; height:35px; width:1.6rem;font-size:0.26rem; line-height:35px; text-align:center; border:1px double #ff6a00; background-color:#ffffff; color:#ff6a00;">
                <a href="javascript:window.history.go(-1);">返回首页</a>
            </div>
            <div style=" border-radius:10%; position:absolute; top:100px; right:10px; height:35px; width:1.6rem;font-size:0.26rem; line-height:35px; text-align:center; border:1px double #ff6a00; background-color:#ffffff; color:#ff6a00;">
                <a href="/showdetails?id=<%=_id %>">景区综述</a>
            </div>
            <div style="width:100%;    height: 3.2rem;">
                <div style="width:1.6rem; margin-left:15px; margin-top:15px; float:left;"><img src="<%=showsImg %>" style="width:1.6rem;height:1.6rem; border-radius:50%;" /></div>
                <div style="width:5.1rem; margin-left:15px; margin-top:30px; font-size:0.36rem; float:left; ">
                    <span style="font-weight:900;"><%=title %></span>
                    <br>
                    <br>
                    <span style="margin-right:10px;">票价：<span style="color:#ff6a00;font-weight:900;">￥<%=money %></span></span><span>等级：<%=level %></span>
                </div>
            </div>
            <div style="width: 95%; height: 2.00rem;  line-height:50px;  font-size: 0.26rem;position:absolute; top:2.0rem;left:0.43rem;">
                 <%=Remarks %>
            </div>
        </div>
        <div style="width:100%;background-color:#ffffff; height:40px; text-align:center;position: fixed; top: 290px; left: 0px; width:100%;" id="tog" onclick="hides();">
            <img src="/images/up_arrow.png" id="imgtop" style="margin-top:10px;">
            <input type="hidden" value="0" id="div1" />
        </div>
        <script type="text/javascript">
            var hides = function () {
                $("#topdiv").slideToggle();
                if ($("#div1").val() == "0") {
                    $("#tog").css({ "top": "0px" });
                    $("#imgtop").attr("src", "/images/down_arrow.png");
                    $("#div1").val("1");
                } else {
                    $("#tog").css({ "top": "290px" });
                    $("#imgtop").attr("src", "/images/up_arrow.png");
                    $("#div1").val("0");
                }

            }
        </script>
	         <div id="m-tool-Dom" class="m-tool" style="width:100%;">
            <div class="toolNav">
                <ul>
                    <li class=""><span>推荐线路</span></li>
                    <li class="viweList-li"><span>景点列表</span></li>
                    <li class=""><span>周边服务</span></li>
                </ul>
            </div>

        </div>
	                <div id="menu-list" class="menu-list">
            <dl class="selectLine">
                <dt>
                    <%=title %> 
                    <a class="amap-info-close" href="javascript: void(0)" style="right: 5px;"></a>
                </dt>
                <dd class="listDetail" id="scrollCont">
                    <ul class="scroll"></ul>
                </dd>
            </dl>
            <dl class="selectSpots">
                <dt>
                    <%=title %> <a class="amap-info-close" href="javascript: void(0)" style="right: 5px;"></a>
                    <form id="searchForm" action="#">
                        <input type='text' autocomplete="off" autocorrect="off" id="spotsSearch" placeholder='请输入景点'>
                    </form>
                </dt>
                <dd class="listDetail">
                    <ul id="viweLists" class="scroll"></ul>
                </dd>
            </dl>
            <dl class="selectLanguage">
                <dt><%=title %>  <a class="amap-info-close" href="javascript: void(0)" style="right: 5px;"></a></dt>
               <dd class="listDetail">
                    <ul id="viweLists1" class="scroll"></ul>
                </dd>
            </dl>
        </div>
	        <!--m-tool-Dom-->    
	
	        <!--shou弹窗 Start-->
	        <div class="popBox"  ></div>
	          <div class="keyPop keyPop_Code " style="height: 6.00rem;">
            <div class='verifyColse'>×</div>
           <dl>
              <dt>授权激活</dt>
               <dd class=" ddOnlyAct" style=" color:coral; font-weight:900; font-size:40px; padding-bottom:50px; padding-top:20px;">需支付费用：￥<%=money %></dd>
                <dd style="padding-bottom:20px;" class="ddInput" <%=_display %>>
                <div style="position: relative;">
		              <i id="sm" style="background-image: url(/images/Scan_Barco.png);position: absolute;left: 0;z-index:5;background-repeat: no-repeat;background-position: 0px 0px; width: 75px;height: 64px;"></i>
			              <textarea type="tel" id="code" style="padding-left:80px;"  placeholder="请输入激活码"></textarea>
		              </div>
                </dd>
               <dd class=" ddOnlyAct" style="padding:10px 30px;font-size:0.25rem; height:40px;line-height:40px; margin-bottom:80px;text-align:left;">
	       亲爱的旅客您好，景好智能云导游服务需要您提供激活码后方可使用！
	       </dd>
                <dd class="ddBottom ">
                    <%=button %>
                </dd>
                
                <!--onclick="buys();"-->
            </dl>
            <script type="text/javascript">
                var sq = function () {
                    if ($("#code").val() == "") {
                        alert("请填写激活码！");
                        return false;
                    }
                    $.getJSON("/Handler.ashx", { p: "sq", money: '<%=money%>', Scenic_id: '<%=_id%>', openid: commonMethod.getCookie('_usercode'), code: $("#code").val(), r: Math.random() }, function (data) {
                        if (data.info == "ok") {
                            alert('购买成功！');
                            window.CodeVerify = true;
                            location.reload();
                        } else {
                            alert(data.info);
                        }
                    });
                }
            </script>
        </div>
	        <!--弹窗 End-->    
	         <div class="message"></div>
	         
	         
	         

	         
	         
		     
		     <a class="goIntegrate bigArea hide" href="javascript:history.go(-1);" target="_blank"></a>
	         
	    </div>

   

   <script type="text/javascript" src="/Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src='/leaflet/jquery-double-tap.js'></script>
    <script type="text/javascript" src='/leaflet/zepto.js'></script>
    <script type="text/javascript" src='/leaflet/iscroll.js'></script>

    <!-- 引用leaflet.js组件 -->
    <script type="text/javascript" src="/leaflet/leaflet.min.js"></script>
    <script type="text/javascript" src='/leaflet/leaflet.ChineseTmsProviders.js'></script>
    <script src="/leaflet/L.Control.Locate.min.js"></script>
    <script src="/leaflet/wgs2mars.min.js"></script>
       <script type="text/javascript" src='/leaflet/index.js'></script>
      <script src="https://res.wx.qq.com/open/js/jweixin-1.1.0.js"></script>
<script type="text/javascript">
    var isiBeacons;
    var is_app;   
    var iscode;
    wx.config({
        beta: true,
        debug: false,
        appId: '<%=appId%>',
        timestamp: '<%=timestamp%>',
        nonceStr: '<%=nonceStr%>',
        signature: '<%=signature%>',
        jsApiList: ['scanQRCode', 'startSearchBeacons', 'stopSearchBeacons', 'onSearchBeacons']
    });
    <%
    if (is_bluetooth == "1") {
    %>
    wx.ready(function () { 

        wx.onSearchBeacons({
            complete: function (argv) {
                isiBeacons = argv; 
            }
        });
	    
        wx.startSearchBeacons({
            ticket: "",
            complete: function (argv) {
                if (argv.errMsg == "startSearchBeacons:bluetooth power off"){
                    alert('蓝牙未打开');
                }else if (argv.errMsg == "startSearchBeacons:location service disable"){
                    alert('地理位置服务未打开');
                }else if (argv.errMsg == "startSearchBeacons:system unsupported"){
                    alert('系统不支持');
                }
                //alert("1" + JSON.stringify(argv));
            }
        });
        wx.stopSearchBeacons({
            complete: function (argv) {
                if (argv.errMsg == "startSearchBeacons:bluetooth power off"){
                    alert('蓝牙未打开');
                }else if (argv.errMsg == "startSearchBeacons:location service disable"){
                    alert('地理位置服务未打开');
                }else if (argv.errMsg == "startSearchBeacons:system unsupported"){
                    alert('系统不支持');
                }
            }
        });   
    });
    <%}%>
	 



    </script>
    <script type="text/javascript">
        $(function () {  
            $("#sm").click(function () { 
                wx.scanQRCode({
                    needResult: 1,
                    desc: 'scanQRCode desc',
                    success: function (res) { 
                        $("#code").val(JSON.parse(res.resultStr).token);
                    }
                });
            });      
        });
       
        
        var buys = function () {

            $.getJSON("/Handler.ashx", { p: "buy", money: '<%=money%>', Scenic_id: '<%=_id%>', openid: commonMethod.getCookie('_usercode'), r: Math.random() }, function (data) {
                if (data.info == "ok") {
                    alert('购买成功！');
                    location.reload();
                    window.CodeVerify = true;
                } else {
                    alert(data.info);
                }
            });
        }



        var isiOS = !!navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
        var coordinateOffset1 = [];
        var coordinateOffset2 = [];
        var staticLogo = "";
        var l = $("body").width() / 7.5;
        var audioPath = '';
        coordinateOffset1[14] = 0.0024 * 100 / l;
        coordinateOffset2[14] = isiOS ? 0.00662 * 100 / l : 0.00615 * 100 / l;
        for (var c = 15; c < 25; c++) {
            coordinateOffset1[c] = coordinateOffset1[c - 1] / 2;
            coordinateOffset2[c] = coordinateOffset2[c - 1] / 2;
        }
        for (var c = 13; c > 0; c--) {
            coordinateOffset1[c] = coordinateOffset1[c + 1] * 2;
            coordinateOffset2[c] = coordinateOffset2[c + 1] * 2;
        }
        



        var AppUrlString = location.href.split('urlObj=')[1];
        if (AppUrlString) {

            var AppUrlObj = eval("(" + (decodeURI(AppUrlString).match(/\{.*\}/)[0]) + ")");
            if (typeof AppUrlObj === 'object' && AppUrlString) {

                if (isiOS) {

                    if (AppUrlObj.singleIosAppLink.trim() != '') {

                        $("body").removeClass('no-popularize');

                    }

                } else {
                    if (AppUrlObj.singleAndroidAppLink.trim() != '') {

                        $("body").removeClass('no-popularize');
                    }
                }
            }
        }

      

        var scenicId = '<%=_id%>',//景区id
        	scenicName = '<%=MapName%>',
            beginCoordinate = [<%=beginCoordinate%>], //起始位置
            endCoordinate = [<%=endCoordinate%>], //结束为止
            bounds = L.bounds([beginCoordinate, endCoordinate]),
            MapcenterCoordinate = [bounds.getCenter().x, bounds.getCenter().y], //地图中心点坐标
            MapZoom = <%=MapZoom%>, //地图默认缩放等级
            MapZoomRange = [<%=MapZoomRange%>],//地图缩放范围
            audioType = "1-1",//音频类型
            markers = [],//景点标签
            mapMethod; //地图方法
        var distributorId;
        var imageLayerBA = [];
        var imageLayer = [];
        var imageLayer2 = [];
        var viwes;
        var mapAgruments;

        var nonShowConfirm = (location.href.split('nonShowConfirm=')[1] ? location.href.split('nonShowConfirm=')[1] : "").split("&")[0] === 'true';
        var specialHandling = location.href.search("clpjschysyjbsdjn") >= 0 && scenicId == 1554;

        var distributorId = '0'; //分销商id

        if (specialHandling) {
            $(".mapContainer19").css("top", "0px");
            $("#m-tool-Dom,.error-correction,#recommendDom,.autoPlay-confirm,.autoPlay-confirm-bg").hide();
        }
        var temporaryMarkerObj;



       <%=markers%>

        var lineTrack = {};
         
        <%=lineTrack%>
        <%--var lineTrack = {
            '全景路线': [<%=lineTrack%>]
        };--%>
     

        var isAuthUrl = "";
        var needVerify = true;


        verifyCookie(isAuthUrl, scenicId);

        $("#autoPlay").trigger("tap");
        $(".autoPlay-confirm").addClass('active free');
        needVerify = false;




        function loadMap() {
            mapMethod = new LaeflatMapModule();
            mapAgruments = { //初始化地图参数
                center: MapcenterCoordinate,
                zoom: MapZoom,
                hideTitZoom: 6,/* 17.0 */
                minZoom: MapZoomRange[0],
                maxZoom: MapZoomRange[1],
                zoomControl: false,
                zoomAnimationThreshold: 1
            };

            mapAgruments.maxBounds = [[beginCoordinate[0] - (!specialHandling ? ($("body").hasClass('no-popularize') ? 0.003 : 0.0065) : -0.0065), beginCoordinate[1]], endCoordinate];

            map = mapMethod.MapInitialize(mapAgruments);   //地图初始化


            var imageLayerAgr;

            imageLayerAgr = {
                'imageUrl': '<%=imageUrl%>',
                'imageBounds': [[<%=beginCoordinate%>], [<%=endCoordinate%>]]
            }
            imageLayer.push(mapMethod.addImageLayer(imageLayerAgr, map));

            isiOS && (imageLayer2.push(mapMethod.addImageLayer(imageLayerAgr, map)));


            for (var i = 0; i < imageLayer.length; i++) {
                imageLayer[i].addTo(map);
                imageLayerBA.push([imageLayer[i]._bounds._northEast.lat, imageLayer[i]._bounds._northEast.lng]);
                imageLayerBA.push([imageLayer[i]._bounds._southWest.lat, imageLayer[i]._bounds._southWest.lng]);

            }



            map.on('zoomstart', function () {
                $("body").addClass('zoomstart')
            });
            map.on('zoomend', function () {
                $("body").removeClass('zoomstart')
            })


            for (var i = 0; i < imageLayer.length; i++) {
                imageLayer[i].addTo(map);
                imageLayer2[i] && imageLayer2[i].addTo(map);
                imageLayer2[i] && map.removeLayer(imageLayer2[i]);
            }

            viwes = mapMethod.addMarkers(markers); //遍历景点 
            mapMethod.startLocate(); //添加定位 
            mapMethod.addPolylines(lineTrack); //添加路线     

            DomOperate(mapMethod, map);

            $("#menu-list .selectLanguage").find('li[audioId]').length > 0 && (mapMethod.audioChange($("#menu-list .selectLanguage").find('li[audioId]')));

            audioType = $(".selectLanguage .liSelect").attr("audioid"); //获取当前音频类型



            var imgWidth = $(".leaflet-image-layer.leaflet-zoom-animated").width() > 0 ? $(".leaflet-image-layer.leaflet-zoom-animated").width() : 1;

            var mapDivWidth = $("#MapContainer").width(),
                  imgHeight = $(".leaflet-image-layer.leaflet-zoom-animated").height() > 0 ? $(".leaflet-image-layer.leaflet-zoom-animated").height() : 1,
                  mapDivHeight = $(document).height() - $("#m-tool-Dom").height();
            var wPercent = mapDivWidth / imgWidth,
                hPercent = mapDivHeight / imgHeight;
            var mImgWidth = imgWidth, mImgHeight = imgHeight;
            var nowMinZoom = map.getZoom() + (wPercent > hPercent ? wPercent : hPercent) - 1;
            for (var z = 0; z < nowMinZoom; z++) {

                if (mImgWidth * 2 > mapDivWidth && mImgHeight * 2 > mapDivHeight) {
                    break;
                } else {
                    mImgHeight = mImgHeight * 2;
                    mImgWidth = mImgWidth * 2;
                }
            }
            nowMinZoom = map.getZoom() + z;
            map.setZoom(nowMinZoom, { animate: false });
            wPercent = mapDivWidth / mImgWidth;
            hPercent = mapDivHeight / mImgHeight;
            nowMinZoom = map.getZoom() + (wPercent > hPercent ? wPercent : hPercent) - 1;
            if (wPercent > 1 || hPercent > 1) {


                map.options.minZoom < nowMinZoom && (map.options.minZoom = nowMinZoom + 0.15);
                map.options.maxZoom < nowMinZoom && (map.options.maxZoom = Math.ceil(nowMinZoom + 0.15) + 1);

                map.setZoom(nowMinZoom + 0.15, { animate: false });


            }


            map.setMaxBounds();

            map.setMaxBounds([[beginCoordinate[0] - ($("body").hasClass('no-popularize') ? coordinateOffset1[parseInt(nowMinZoom)] : coordinateOffset2[parseInt(nowMinZoom)]), beginCoordinate[1]], endCoordinate]);

            if (commonMethod.IsPC) {

                map.setMaxBounds([beginCoordinate, endCoordinate]);
            }
            if (!mapMethod.rejectLocate && !nonShowConfirm && !specialHandling && distributorId != 22) {
                $("body").addClass('show-aConfirm');

            }

            $("#confirmYes").bind('click', function () {


                $("body").removeClass('show-aConfirm');



                if ($("body").attr("outViwe") == 'true') {
                    commonMethod.setMessage('您不在当前景区范围內！');
                }

            })

            $(".advertisement .looklook").bind('click', function () {

                if ($(".autoPlay-confirm-bg").hasClass('active')) {
                    return false
                } else {

                    $("body").removeClass('show-aConfirm');
                }
            })
            if (isiOS) {
                $("#disableyMove>.local-btn").on("touchstart", function (e) {
                    $(this).hide();
                    e.preventDefault();
                    $(this).show();
                }
                )

            } else {
                //定位按钮点击添加click属性

                $("#disableyMove>.local-btn").bind(commonMethod.clickEvent, function () {
                    if (mapMethod.rejectLocate) {
                        $("body").addClass('show-reject-box');

                        commonMethod.slideIn({ "$obj": $('.reject-local-box'), "shadeDiv": $(".autoPlay-confirm-bg") });

                        return false;
                    }
                    if ($(this).hasClass('panto')) return false;
                    $(this).addClass('panto');

                    mapMethod.locateObj.stop();
                    mapMethod.locateObj.start();
                    return false;

                });
            }

        }

        $(function () {

            var noTouchmove = true;
            L.DomEvent.addListener($(".keyPop_Code .ddInput>textarea")[0], "touchmove", function (event) {

                noTouchmove = false;

            })

            L.DomEvent.addListener(document.getElementById("disableyMove"), "touchmove", function (event) {
                if (noTouchmove) {

                    event.preventDefault();
                    return false;
                } else {
                    noTouchmove = true;
                }


            });

            $(document).on('touchmove', '.leaflet-popup-pane .leaflet-popup', function () {

                event.preventDefault();
                return false;
            });

            if (isiOS) {
                $("#disableyMove").doubletap(
                      function (event) {

                          event.preventDefault();

                          return false;
                      },
                      function (event) {


                      },
                      400
                  );




            }
            var urlCode = location.href.match(/\&c\=([0-9]+)/) && location.href.match(/\&c\=([0-9]+)/)[1];
            urlCode && ($(".keyPop_Code .ddInput").addClass("active").children("textarea").val(urlCode), $(".keyPop_Code .ddTxt>.pTxt1").html("已自动识别您所购买的授权码，点击“验证授权”，即可获取授权！"))

            $("body").bind(commonMethod.clickEvent, function (event) {
                var thisElemet = $(event.target);
                if (!(thisElemet.hasClass(".menu-list") || thisElemet.parents('.menu-list').length > 0) && $('dl.navAni').length > 0) {
                    $('dl.navAni').removeClass('navAni');
                    $('body').removeClass('show-menu');
                }
            });

            

            $(".browse-open").bind(commonMethod.clickEvent, function () {
                $("body").css("background-color", "#505556");
                $("body").removeClass('opentip');
                setTimeout(function () {
                    $("body").removeAttr('style');
                }, 300);

            });
            if (!commonMethod.IsPC) {
                $("#recommendDom>a").bind("click", function (event) {
                    event.preventDefault();
                });
                $("#recommendDom>a").bind("tap", function (event) {
                    location.href = $(this).attr('href');
                });
            }

        })

    </script>
  </body>
</html>