var commonMethod = (function () {
    var MsgTimeAuto;
    /**
     * 消息提示公共方法
     * @param {string} title 提示的内容
     * @param {number} time  提示框消失时间，缺省则为默认2000ms
     */
    var setMessage = function (title, time, highly) {
        var DomMessage;
        var outTime = time ? time : 2000;
        clearTimeout(MsgTimeAuto);
        MsgTimeAuto = null;

        DomMessage = $('body .message');
        DomMessage.text(title);
        $("body").addClass('messageShow');
        highly && $("body").addClass('messageShow').addClass('highly');
        MsgTimeAuto = setTimeout(function () {
            $("body").removeClass('messageShow').removeClass('highly');
        }, outTime);

    }
    /**
     * [isNullObj 判断对象是否为空]
     * @param  {[object]}  obj [需要判断的对象]
     * @return {Boolean}     [返回布尔值]
     */

    function isNullObj(obj) {
        for (var i in obj) {
            if (obj.hasOwnProperty(i)) {
                return false;
            }
        }
        return true;
    }
    /**
     * 获取cookie值
     * @param  {string} c_name cookie名称
     * @return {string}        存在返回cookies值，否则返回空。
     */
    function getCookie(c_name) {
        if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf(c_name + "=")
            if (c_start != -1) {
                c_start = c_start + c_name.length + 1
                c_end = document.cookie.indexOf(";", c_start)
                if (c_end == -1) c_end = document.cookie.length
                return unescape(document.cookie.substring(c_start, c_end))
            }
        }
        return "";
    }

    /**
     * 保存cookies
     * @param {string} c_name       cookie的名称
     * @param {string} value        cookie值
     * @param {numbers} expiredays  cookie有效时间
     */
    function setCookie(c_name, value, expiredays) {
        var exdate = new Date()
        exdate.setDate(exdate.getDate() + expiredays)
        document.cookie = c_name + "=" + escape(value) + ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString());
    }
    /**
     * 判断是够是pc设备
     */
    function IsPC() {

        var userAgentInfo = navigator.userAgent;
        var Agents = ["Android", "iPhone", "SymbianOS", "Windows Phone", "iPod"];
        var flag = true;
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) {
                flag = false;
                break;
            }
        }
        if (window.screen.width >= 768) {
            flag = true;
        }
        return flag;
    }

    function hideMolde(showObj) {
        $("dl.navAni").removeClass("navAni");
        $("#viweLists").removeClass('hasResult resultNull');
        $("#viweLists>li.result").removeClass("result");
        $("#spotsSearch").val('');
        if (showObj != 'details' && $(".viweDetails.show-details").length) {
            slideOut({ "$obj": $(".viweDetails.show-details"), "shadeDiv": $(".autoPlay-confirm-bg") });
            $(".viweDetails.show-details,body").removeClass('show-details');
        }
        if (showObj != 'feedBackDom' && $("body").hasClass('show-feedBack')) {
            slideOut({ "$obj": $("#feedBackDom") });
            $("body").removeClass('show-feedBack');
        }

        $("body").hasClass("show-aConfirm") && (slideOut({ "$obj": $(".autoPlay-confirm"), "shadeDiv": $(".autoPlay-confirm-bg") }), $("body").removeClass('show-aConfirm'));
        $(".link-code.big").hide();
    }
    function slideIn(arg) {
        var marginTop = -arg.$obj.height() / 2 - parseInt(arg.$obj.css('padding-top')) / 2 - parseInt(arg.$obj.css('padding-bottom')) / 2;
        if (Math.abs(marginTop) * 2 < $(".mapContainer19").height() - $("#m-tool-Dom").height()) {
            marginTop -= $("#m-tool-Dom").height() / 2
            if (!$("body").hasClass('no-popularize')) marginTop += $(".app-popularize").height() / 2;
        }
        arg.$obj.show();
        arg.$obj.animate({ "top": '50%', "margin-top": marginTop + 'px' }, arg.time ? arg : 300, 'linear');
        arg.shadeDiv && arg.shadeDiv.fadeIn();
    }
    function slideOut(arg, backfun) {
        arg.$obj.animate({ "top": '150%' }, arg.time ? arg : 300, 'swing', function () {
            $(this).css({ "top": '-150%' });
            backfun && backfun();
        });
        arg.shadeDiv && arg.shadeDiv.fadeOut();
    }
    return {
        'setMessage': setMessage,
        'isNullObj': isNullObj,
        'getCookie': getCookie,
        'setCookie': setCookie,
        'IsPC': IsPC(),
        'hideMolde': hideMolde,
        'slideOut': slideOut,
        'slideIn': slideIn,
        'clickEvent': IsPC() ? "click" : "tap"
    }
})();


/**
 * 将地图的一些方法进行模块化
 */
var LaeflatMapModule = function () {
    if (!(this instanceof LaeflatMapModule)) {
        return new LaeflatMapModule();
    }
    this.prioriAutoPlay = true; //是否优先自动播放
    this.boundsImgAlter = null; //定义图片闪烁interval对象。。。
    this.currentBoundsImg = null;
    this.fontsizePer = parseInt($("html").css('font-size')) / 100;
    this.rejectLocate = false; //是否拒绝了位置共享
    this.locateObj;//定位对象
    this.inViwe = false;//保存是否定位到景区内
    var mapObj, //保存地图对象
        MapModule = this,
        loadFirst = true, //用来保存是否第一次载入
        OutViweHint = true, //用来保存是否在景点内
        markerIndex, //保存当前播放音频的marker景点
        openMarkerIndex, //保存当前显示详情的近点
        audioDom = $("audio#videoPlay"), //播放音频的DOM元素
        markersList, //保存景区景点列表
        markersAllList,//保存景区生活和景点所有marker
        markersListGroup, //将marker标签保存在同一个组，批量操作
        divIconsListGroup,//将景点标题marker标签保存在同一个组，用于批量操作
        mapBounds = null, //用来判断是否设置限制视野范围
        imgMapBounds, //保存图片范围
        currentViwe = {}, //当前处于的景点
        fontSize = parseInt($("html").css("font-size")), //保存字体大小
        currentTrack = {}, //保存当前路线


        audioIcon = L.icon({ //默认有音频marker图标
            className: 'viweScenic',
            zIndexOffset: 10,
            iconUrl: '/images/Ht-iconMarkerAudio.png',
            iconRetinaUrl: '/images/Ht-iconMarkerAudio.png',
            iconSize: [62 * this.fontsizePer, 62 * this.fontsizePer],
            iconAnchor: [30 * this.fontsizePer, 48 * this.fontsizePer],
            popupAnchor: [-8 * this.fontsizePer, -60 * this.fontsizePer]
        }), //定义有语音景点标记图标

        icon = L.icon({ //默认无音频marker图标
            className: 'viweScenic',
            iconUrl: '/images/Ht-iconMarker.png',
            iconRetinaUrl: '/images/Ht-iconMarker.png',
            iconSize: [62 * this.fontsizePer, 62 * this.fontsizePer],
            iconAnchor: [30 * this.fontsizePer, 48 * this.fontsizePer],
            popupAnchor: [-8 * this.fontsizePer, -60 * this.fontsizePer]
        }), //定义五语音景点标记图标 

        DimIcon = L.icon({ //默认图标黑暗版，用来实现闪烁效果。
            className: 'viweScenic',
            iconUrl: '/images/Ht-iconMarkerAudioDim.png',
            iconRetinaUrl: '/images/Ht-iconMarkerAudioDim.png',
            iconSize: [62 * this.fontsizePer, 62 * this.fontsizePer],
            iconAnchor: [30 * this.fontsizePer, 48 * this.fontsizePer],
            popupAnchor: [-8 * this.fontsizePer, -60 * this.fontsizePer]
        }),

        locationMark = null, //定位成功标签
        locationCircle = null, //定位成功圆环
        mapContainerDom = $("#MapContainer,.mapContainer19"), //用来初始化地图的DOM节点
        mapContainerH = $(document).height() - $("#m-tool-Dom").height() + 2, //地图区域高度
        gaodeLayers = L.tileLayer.chinaProvider('GaoDe.Normal.Map', {
            minZoom: 3,
            maxZoom: 18
        }); //背景图层

    var detailsScrollInter, //设置详细滚动条加载
        detailsScroll; //设置详细滚动条加载
    /**
     * //gps转换相关的方法。。。
     * @type {Object}
     */
    var PointTransformation = {
        PI: 3.14159265358979324,
        x_pi: 3.14159265358979324 * 3000.0 / 180.0,
        delta: function (lat, lon) {
            var a = 6378245.0; //  a: 卫星椭球坐标投影到平面地图坐标系的投影因子。
            var ee = 0.00669342162296594323; //  ee: 椭球的偏心率。
            var dLat = this.transformLat(lon - 105.0, lat - 35.0);
            var dLon = this.transformLon(lon - 105.0, lat - 35.0);
            var radLat = lat / 180.0 * this.PI;
            var magic = Math.sin(radLat),
                magic = 1 - ee * magic * magic;
            var sqrtMagic = Math.sqrt(magic),
                dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * this.PI),
                dLon = (dLon * 180.0) / (a / sqrtMagic * Math.cos(radLat) * this.PI);
            var pt = {
                'lat': dLat,
                'lon': dLon
            };
            return pt;
        },

        //WGS-84 to GCJ-02
        gcj_encrypt: function (wgsLat, wgsLon) {
            if (this.outOfChina(wgsLat, wgsLon))
                return {
                    'lat': wgsLat,
                    'lon': wgsLon
                };

            var d = this.delta(wgsLat, wgsLon);
            var pt = {
                'lat': wgsLat + d.lat,
                'lon': wgsLon + d.lon
            };
            return pt;
        },
        outOfChina: function (lat, lon) {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        },
        transformLat: function (x, y) {
            var ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.sqrt(Math.abs(x));
            ret += (20.0 * Math.sin(6.0 * x * this.PI) + 20.0 * Math.sin(2.0 * x * this.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.sin(y * this.PI) + 40.0 * Math.sin(y / 3.0 * this.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.sin(y / 12.0 * this.PI) + 320 * Math.sin(y * this.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        },
        transformLon: function (x, y) {
            var ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.sqrt(Math.abs(x));
            ret += (20.0 * Math.sin(6.0 * x * this.PI) + 20.0 * Math.sin(2.0 * x * this.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.sin(x * this.PI) + 40.0 * Math.sin(x / 3.0 * this.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.sin(x / 12.0 * this.PI) + 300.0 * Math.sin(x / 30.0 * this.PI)) * 2.0 / 3.0;
            return ret;
        }
    };

    /**
     * [//判断点是否在区域类，根据点和矩形每条边组成的三角形等于矩形面积判断是否在区域内。]
     * @param  {[object]}  point   [地图点对象]
     * @param  {[数组]}  polygon [矩形点]
     * @return {Boolean}         [返回是否成功]
     */
    function isPointInPolygon(point, polygon) {

        var Lag = point ? point : [0, 0];
        var polygonE = L.latLng(polygon[1]).distanceTo(polygon[3]);
        var polygonA = L.latLng(polygon[1]).distanceTo(polygon[2]);
        var polygonB = L.latLng(polygon[2]).distanceTo(polygon[3]);
        var polygonC = L.latLng(polygon[3]).distanceTo(polygon[0]);
        var polygonD = L.latLng(polygon[0]).distanceTo(polygon[1]);

        var Parae = triangleArea(polygonE, polygonA, polygonB) + triangleArea(polygonE, polygonC, polygonD);
        var Parae = parseInt(Parae)
        var nParae = 0;
        for (var i = 0, len = polygon.length; i < len; i++) {
            if (i + 1 < len) {
                var a = Lag.distanceTo(polygon[i]),
                    b = Lag.distanceTo(polygon[i + 1]),
                    c = L.latLng(polygon[i]).distanceTo(polygon[i + 1]);
                nParae += triangleArea(a, b, c);
            } else {
                var a = Lag.distanceTo(polygon[i]),
                    b = Lag.distanceTo(polygon[0]),
                    c = L.latLng(polygon[i]).distanceTo(polygon[0]);
                nParae += triangleArea(a, b, c);
            }
        }
        nParae = parseInt(nParae);


        function triangleArea(a, b, c) {
            if (a + b < c || a + c < b || b + c < a) return 0;
            var area, p = (a + b + c) / 2;
            area = Math.sqrt(p * (p - a) * (p - b) * (p - c));
            return area;

        }

        return Parae === nParae;

    }

    if (!$("body").hasClass('no-popularize')) {
        mapContainerH -= $(".app-popularize").height();
    }
    $("body").width() < 960 && mapContainerDom.height(mapContainerH); //设置地图高度

    //放大缩小地图

    $(".zoomControl>.zoombtn").bind(commonMethod.clickEvent, function () {
        if ($(this).hasClass('add')) {
            mapObj.zoomIn()
        } else {
            mapObj.zoomOut()
        }
    })


    /**
     * 初始化地图
     * @param {object} MapArguments 初始化地图参数
     * @return {object} 高德地图对象
     */
    this.MapInitialize = function (MapArguments) {
        var mapArguments = MapArguments;
        mapObj = L.map('MapContainer', MapArguments);
        mapBounds = mapArguments.maxBounds ? L.latLngBounds(mapArguments.maxBounds) : 'noBounds';
        imgMapBounds = L.latLngBounds([beginCoordinate, endCoordinate]);




        //if (isiOS) {

        //    mapObj.on('zoomend', function() {
        //        for (var i = 0; i < imageLayer.length; i++) {
        //            mapObj.addLayer(imageLayer2[i]);
        //            mapObj.removeLayer(imageLayer[i]);
        //        };

        //        var t = imageLayer;
        //        imageLayer = imageLayer2;
        //        imageLayer2 = t;
        //        markersListGroup && mapObj.removeLayer(markersListGroup), mapObj.addLayer(markersListGroup);
        //        divIconsListGroup && mapObj.removeLayer(divIconsListGroup), mapObj.addLayer(divIconsListGroup);
        //        if (markerIndex) {
        //            MapModule.currentBoundsImg = markersList[markerIndex]._icon;

        //        }
        //        openMarkerIndex && markersList[openMarkerIndex].openPopup();
        //        currentTrack.polyline && (mapObj.removeLayer(currentTrack.polyline), mapObj.addLayer(currentTrack.polyline));
        //        currentTrack.markers && (mapObj.removeLayer(currentTrack.markers), mapObj.addLayer(currentTrack.markers));

        //        setLocationMarkStyle();



        //    });
        //}
        var hideTitZoom = MapArguments.hideTitZoom ? MapArguments.hideTitZoom : 16;

        mapObj.on('zoomend', function () {
            if (divIconsListGroup && !commonMethod.IsPC) {

                if (parseInt(mapObj.getZoom()) <= hideTitZoom) {
                    mapObj.hasLayer(divIconsListGroup) && mapObj.removeLayer(divIconsListGroup);
                } else {
                    !mapObj.hasLayer(divIconsListGroup) && mapObj.addLayer(divIconsListGroup);
                }
            }

            if (!commonMethod.IsPC) {
                mapObj.setMaxBounds();
                if (!isiOS && $("body").attr("keyboardUp")) {
                    mapObj.setMaxBounds([beginCoordinate, endCoordinate]);
                } else {
                    mapObj.setMaxBounds([[beginCoordinate[0] - ($("body").hasClass('no-popularize') ? coordinateOffset1[parseInt(mapObj.getZoom())] : coordinateOffset2[parseInt(mapObj.getZoom())]) * 2, beginCoordinate[1]], endCoordinate]);
                }
            }

        });

        return mapObj;

    }


    /**
     * 添加自定义图层
     * @param {object} LayerArguments 图层参数
     * @param {object} mapObj         高德地图对象
     */
    this.addImageLayer = function (LayerArguments) {
        var imageLayer = L.imageOverlay(LayerArguments.imageUrl, LayerArguments.imageBounds);
        return imageLayer;
    }

    /**
     * 添加景点标签、景点范围
     * @param  {array} markers  景点数据列表
     * @return {object}          高德marker对象数组
     */
    this.addMarkers = function (markers) {
        var markers = markers ? markers : [];
        markersList = [];
        markersAllList = [];
        var divIcons = [];
        var iconAnchorX = [[fontSize * 0.20 * 0, 92], [fontSize * 0.20 * 1 + 10, 92], [fontSize * .20 * 2, 92], [fontSize * 0.20 * 3 - 12, 92], [fontSize * .20 * 4 - 22, 92], [fontSize * 0.20 * 5 - 32, 92], [fontSize * 0.20 * 6 - 42, 92], [fontSize * 0.20 * 7 - 52, 92], [fontSize * 0.20 * 8 - 65, 92], [fontSize * 0.20 * 9 - 75, 92], [fontSize * 0.20 * 10 - 85, 92], [fontSize * 0.20 * 11 - 95, 92], [fontSize * 0.20 * 12 - 105, 92], [fontSize * 0.20 * 13 - 115, 92], [fontSize * 0.20 * 14 - 127, 92], [fontSize * 0.20 * 15 - 138, 92], [fontSize * 0.20 * 16 - 149, 92], [fontSize * 0.20 * 17 - 160, 92], [fontSize * 0.20 * 18 - 171, 92], [fontSize * 0.20 * 19 - 182, 92], [fontSize * 0.20 * 20 - 193, 92]];
        var iconAnchorXPC = [[fontSize * 0.25 * 0, 41], [fontSize * 0.25 * 1 + 6, 41], [fontSize * .20 * 2 + 5, 41], [fontSize * 0.25 * 3 - 7, 41], [fontSize * .20 * 4 - 3, 41], [fontSize * 0.25 * 5 - 20, 41], [fontSize * 0.25 * 6 - 25, 41], [fontSize * 0.25 * 7 - 32, 41], [fontSize * 0.25 * 8 - 38, 41], [fontSize * 0.25 * 9 - 43, 41], [fontSize * 0.25 * 10 - 50, 41], [fontSize * 0.25 * 11 - 57, 41], [fontSize * 0.25 * 12 - 62, 41], [fontSize * 0.25 * 13 - 68, 41], [fontSize * 0.25 * 14 - 75, 41], [fontSize * 0.25 * 15 - 82, 41], [fontSize * 0.25 * 16 - 88, 41], [fontSize * 0.25 * 17 - 94, 41], [fontSize * 0.25 * 18 - 100, 41], [fontSize * 0.25 * 19 - 106, 41], [fontSize * 0.25 * 20 - 112, 41]];
        markers.forEach(function (marker) {
            //添加marker标签 

            var markerIcon = commonMethod.isNullObj(marker.audioUrl) ? icon : audioIcon; //根据景点是否包含音频设置icon图标
            if (marker.icon) { //如果小景点自定义了图标，则设置自定义图标
                markerIcon = L.icon({
                    className: marker.viweType,
                    iconUrl: marker.icon,
                    iconRetinaUrl: marker.icon,
                    iconSize: [marker.viweType == "nearService" ? 60 : 87, marker.viweType == "nearService" ? 60 : 81],
                    iconAnchor: commonMethod.IsPC ? [marker.viweType == "nearService" ? 20 : 30, marker.viweType == "nearService" ? 25 : 41] : [30, 41],
                    popupAnchor: [-3, -76]

                });
            }
            if (marker.position != null) {
                var TMaker = MapModule.createMarker(marker, {
                    icon: markerIcon
                });
            }
            TMaker.markerData = marker;
            //L.polygon(marker.area,{}).addTo(mapObj); //添加区域
            if (marker.viweType == 'scenic') {
                markersList.push(TMaker);
            }
            markersAllList.push(TMaker)



            divIcons.push(L.marker(
                marker.position, {
                    icon: L.divIcon({
                        className: marker.viweType == 'scenic' ? 'my-div-icon' : 'my-div-icon nearService',
                        html: marker.viweName,
                        /*iconSize: [commonMethod.IsPC?fontSize*0.25*(marker.viweName.length+1):fontSize*0.20*(marker.viweName.length+1),fontSize*0.33],*/
                        iconSize: [commonMethod.IsPC ? fontSize * 0.25 * (marker.viweName.length + 1) : fontSize * 0.20 * (marker.viweName.length + 1), fontSize * 0.33],
                        iconAnchor: commonMethod.IsPC ? iconAnchorXPC[marker.viweName.length] : marker.viweType == "nearService" ? [iconAnchorX[marker.viweName.length][0] - 10, iconAnchorX[marker.viweName.length][1] - 10] : iconAnchorX[marker.viweName.length],
                        zIndexOffset: 8
                    })
                }
                )
            );


        });
        markersListGroup = L.layerGroup(markersAllList);
        divIconsListGroup = L.layerGroup(divIcons);
        this.markersListGroup = markersListGroup;
        this.divIconsListGroup = divIconsListGroup;
        mapObj.addLayer(markersListGroup);
        mapObj.addLayer(divIconsListGroup);
        this.markersList = markersList;
        return markersAllList;
    }
    //计算坐标之间的距离
    function getDistance(lat1, lng1, lat2, lng2) {
        var dis = 0;
        var radLat1 = toRadians(lat1);
        var radLat2 = toRadians(lat2);
        var deltaLat = radLat1 - radLat2;
        var deltaLng = toRadians(lng1) - toRadians(lng2);
        var dis = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(deltaLat / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(deltaLng / 2), 2)));
        return dis * 6378137;

        function toRadians(d) { return d * Math.PI / 180; }
    }



    /********************************************景区景点数据遍历结束**********************************************************************/


    /**
     * 创建marker标签，并为此标签的信息框绑定响应事件
     * @param {object} marker          marker标签所有数据，包括音频、图片等。。
     * @param {object} MarkerArguments [marker标签相关参数]
     */
    this.createMarker = function (marker, MarkerArguments) {

        var TMaker = L.marker(marker.position, MarkerArguments); //创建marker标签     

        if (marker.viweType == 'scenic') {
            var Popup = MapModule.createrPopup(marker, TMaker);

            var viweList = $("<li>"); //给景点列表添加景点
            viweList.attr('id', 'marker' + marker.viweID);
            if (marker.audioUrl != null) {
                viweList.addClass("hasVoice");
            }
            viweList.append("<span class='sTxt'>" + marker.viweName + "</span>");

            viweList.bind(commonMethod.clickEvent, function () { //景点列表点击打开景点弹窗

                if (viweList.hasClass('liSelect')) {
                    $(this).removeClass('liSelect');
                    openMarkerIndex = null;

                    TMaker.closePopup();
                } else {
                    Popup.openOn(TMaker);
                    openMarkerIndex = $("#marker" + marker.viweID).index();
                    mapObj.addOneTimeEventListener('moveend', function () {
                        setTimeout(popupLocationReset, 50);
                    });
                    $(this).siblings().removeClass('liSelect');
                    $(this).addClass('liSelect');
                    if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                        $("#controlPlay").addClass("playing").removeClass('playPause');
                    }
                }

                $("#controlPlay").attr('class', "control-play");
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                    $("#controlPlay").addClass("playing").removeClass('playPause');
                }
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playPause')) {
                    $("#controlPlay").addClass("playPause");
                }


            });

            $("#viweLists").append(viweList);
            TMaker.bindPopup(Popup).openPopup();

            L.DomEvent.addListener(TMaker, 'click', function () {

                if ($(".leaflet-popup").length > 0) {

                    viweList.siblings().removeClass('liSelect');
                    viweList.addClass('liSelect');
                    popupLocationReset();
                    openMarkerIndex = $("#marker" + marker.viweID).index();


                } else {
                    viweList.removeClass('liSelect');
                    openMarkerIndex = null;
                };
                $("#controlPlay").attr('class', "control-play");
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                    $("#controlPlay").addClass("playing").removeClass('playPause');
                }
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playPause')) {
                    $("#controlPlay").addClass("playPause");
                }

            })
        } else if (marker.viweType == 'Hotel') {
            var Popup = MapModule.createrPopup(marker, TMaker);

            var viweList = $("<li>"); //给景点列表添加景点
            viweList.attr('id', 'marker' + marker.viweID);
            if (marker.audioUrl != null) {
                viweList.addClass("hasVoice");
            }

            alert(getjuli(marker.position[0], marker.position[1]));

            viweList.append("<span class='sTxt'>" + marker.viweName + "<span style='float:right; right:100px;'>" + getjuli(marker.position[0], marker.position[1]) + " km</span></span>");

            viweList.bind(commonMethod.clickEvent, function () { //景点列表点击打开景点弹窗

                if (viweList.hasClass('liSelect')) {
                    $(this).removeClass('liSelect');
                    openMarkerIndex = null;

                    TMaker.closePopup();
                } else {
                    Popup.openOn(TMaker);
                    openMarkerIndex = $("#marker" + marker.viweID).index();
                    mapObj.addOneTimeEventListener('moveend', function () {
                        setTimeout(popupLocationReset, 50);
                    });
                    $(this).siblings().removeClass('liSelect');
                    $(this).addClass('liSelect');
                    if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                        $("#controlPlay").addClass("playing").removeClass('playPause');
                    }
                }

                $("#controlPlay").attr('class', "control-play");
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                    $("#controlPlay").addClass("playing").removeClass('playPause');
                }
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playPause')) {
                    $("#controlPlay").addClass("playPause");
                }


            });

            $("#viweLists1").append(viweList);
            TMaker.bindPopup(Popup).openPopup();

            L.DomEvent.addListener(TMaker, 'click', function () {

                if ($(".leaflet-popup").length > 0) {

                    viweList.siblings().removeClass('liSelect');
                    viweList.addClass('liSelect');
                    popupLocationReset();
                    openMarkerIndex = $("#marker" + marker.viweID).index();


                } else {
                    viweList.removeClass('liSelect');
                    openMarkerIndex = null;
                };
                $("#controlPlay").attr('class', "control-play");
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playing')) {
                    $("#controlPlay").addClass("playing").removeClass('playPause');
                }
                if (currentViwe.viweID === marker.viweID && audioDom.hasClass('playPause')) {
                    $("#controlPlay").addClass("playPause");
                }

            });
        }

        return TMaker;
    }
    /********************************************创建marker标签，并为此标签的信息框绑定响应事件结束**********************************************************************/

    /**
     * 新建一个信息框，并绑定到marker标签
     * @param  {object} marker [弹窗相关数据]
     * @param  {object} TMaker    [要绑定的景点]
     * @return {object}        [popup对象]
     */
    this.createrPopup = function (marker, TMaker) { //
        var info = $("<div></div>");
        var image = $("<img class='viwe-photo'>");
        image.one('error', function () { $(this).attr('src', '/images/viwephoto_null.jpg') });
        info.attr("href", marker.detailsURL);
        info.attr("viewId", marker.viweID);
        info.append("<h3 class='view-name'>" + marker.viweName + "</h3><a class='leaflet-popup-close-button' href='javascript: void(0)' >×</a>");
        info.append(image.attr('src', marker.viweImgUrl ? marker.viweImgUrl : 'images/viwephoto_null.jpg'));
        info.append("<p class='introduction'>" + marker.introduction + "</p>");

        if (!commonMethod.isNullObj(marker.audioUrl)) {
            if (marker.viweType == "Hotel") {
                info.append("<div class='show-details'></div>");
            } else {
                info.append("<div id='controlPlay' class='control-play'><span></span></div><div class='show-details'></div>");
            }


            var playBtn = info.find('#controlPlay');
            /** 点击播放按钮开始或暂停音频的播放*/
            playBtn.bind(commonMethod.IsPC ? 'click' : 'touchstart', function (e) {
                if (!authorization()) { return false };
                playcount();
                if (playBtn.hasClass('playing') || playBtn.hasClass('Loading')) { //如果音频已在播放，则暂停，否则播放音频。

                    audioDom[0].pause();
                    playBtn.attr('class', 'control-play playPause');
                    audioDom.attr('class', 'playPause');
                    $("#autoPlay").removeClass('manualPlay');
                    imgAnimateEnd();

                } else { //播放

                    if (marker.audioUrl['audioId' + audioType] != null) { //如果当前景点存在当前选中的语音类型则直接播放，否者切换为其它类型的语音。

                        if (!playBtn.hasClass('playPause')) {
                            audioDom[0].src = marker.audioUrl['audioId' + audioType];
                        }
                        if (audioDom[0].src && (audioDom[0].src).indexOf(marker.audioUrl['audioId' + audioType]) < 0) {
                            audioDom[0].src = marker.audioUrl['audioId' + audioType];
                        }
                        audioDom[0].play();

                        playBtn.attr('class', 'control-play playing');
                        audioDom.attr('class', 'playing');
                    } else {
                        commonMethod.setMessage('当前景点没有该类型的语音！已自动切换！');
                        var prtName = '';
                        for (var name in marker.audioUrl) {
                            prtName = name;
                            break;
                        }
                        if (!playBtn.hasClass('playPause')) {
                            audioDom[0].src = marker.audioUrl[prtName];
                        }
                        audioDom[0].play();
                        playBtn.attr('class', 'control-play playing');
                        audioDom.attr('class', 'playing');
                    }


                    imgAnimateEnd();
                    markerIndex = $("#marker" + marker.viweID).index();
                    $("#autoPlay").addClass('manualPlay');
                    currentViwe = marker;
                    MapModule.prioriAutoPlay = false;
                    imgAnimateBegin();

                }


            });

            /***********************************************点击播放按钮开始或暂停音频的播放 结束******************************************************/
        } else {
            info.append("<div class='show-details'></div>");
        }


        /*展开景点详情*/
        info.find('.show-details').bind(commonMethod.IsPC ? 'click' : 'touchend', function (e) {

            if ($('#viweDetails').attr('src') === marker.detailsURL) {
                showDetails(marker, '');
            } else {

                $.ajax({
                    type: 'get',
                    url: marker.detailsURL,
                    success: function (data) {
                        detailsScrollInter = setInterval(function () {
                            detailsScroll.refresh();
                        }, 500);
                        $('#DetailsContainer').html(data);
                        $('#viweDetails').attr('src', marker.detailsURL);
                        var images = $('#DetailsContainer img');
                        if (images && images.length > 0) {
                            images[images.length - 1].onload = function () {
                                setTimeout(function () {
                                    clearTimeout(detailsScrollInter);
                                    detailsScrollInter = null;
                                }, 3000);
                            }
                        } else {
                            setTimeout(function () {
                                clearTimeout(detailsScrollInter);
                                detailsScrollInter = null;
                            }, 3000);
                        }

                    },
                    error: function (jqXHR) {
                        commonMethod.setMessage("发生错误：" + jqXHR.status);

                    }
                });


            }

            showDetails(marker, '');
            detailsScrollDo();


        });

        //点击关闭弹窗
        (function (info, TMaker) {
            info.find('.leaflet-popup-close-button').bind('click', function () {
                openMarkerIndex = null;
                TMaker.closePopup();
            });
        })(info, TMaker)


        var popup = L.popup({
            maxWidth: 'auto',
            keepInView: true,
            closeButton: false
        })
            .setLatLng(marker.position)
            .setContent(info[0]);

        return popup;
    }

    function popupLocationReset() {
        if (!mapObj.options.maxBounds) {
            return false;
        } else {
            $(".leaflet-popup .leaflet-popup-content").css('margin-left', '0');
        }
        var content = $(".leaflet-popup .leaflet-popup-content");
        var leftWidth = content.offset().left + content.width();
        var dw = $(document).width();
        var rw = fontSize * 0.82;



        if (leftWidth > dw - rw) {
            content.css('margin-left', -rw + 'px');
        }
    }

    //关闭景点详情    
    $("#CloseviweDetails").bind(commonMethod.clickEvent, function () {
        commonMethod.slideOut({ "$obj": $('#viweDetails'), "shadeDiv": $(".autoPlay-confirm-bg") });
        $('#viweDetails,body').removeClass('show-details');

    });
    /********************************************新建一个信息框结束********************************************/

    /********************************************根据音频的不同状态，改变播放按钮的样式********************************************/
    var audioPlaying = function () { //播放视频中 
        if (!audioDom.hasClass('playing') && audioDom[0].paused === false) {
            audioDom.attr('class', 'playing');

            audioDom.Error && (audioDom.Error = false);
        }
        if (!$("#marker" + currentViwe.viweID).hasClass('playing') && !(audioDom.Error === true)) {
            $("#marker" + currentViwe.viweID).siblings().removeClass("playing");
            $("#marker" + currentViwe.viweID).addClass('playing');
        }

        $(".leaflet-popup-content>div").attr('viewid') == currentViwe.viweID && !($("#controlPlay").hasClass('playing')) && $("#controlPlay").attr('class', 'control-play playing');

    }
    var audioPlayWaiting = function () { //缓冲时效果

        if (!$("#controlPlay").hasClass('Loading') && audioDom[0].paused === false && /(.mp3|.mp4)$/.test(audioDom[0].src)) {
            $(".leaflet-popup-content>div").attr('viewid') == currentViwe.viweID && $("#controlPlay").attr('class', 'control-play Loading');
            audioDom.attr('class', 'Loading');
            audioDom.Error && (audioDom.Error = false);
            $("#viweLists>li.playing").removeClass('playing');
        }

    }

    var audioPlayEnd = function () { //播放结束响应事件
        $(".leaflet-popup-content>div").attr('viewid') == currentViwe.viweID && $("#controlPlay").attr('class', 'control-play played');
        $(".leaflet-popup-content>div").attr('viewid') != currentViwe.viweID && $("#autoPlay").removeClass('manualPlay');
        !MapModule.prioriAutoPlay && (MapModule.prioriAutoPlay = true);
        clearTimeout(MapModule.boundsImgAlter);
        MapModule.currentBoundsImg && (MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
        audioDom.Error && (audioDom.Error = false);
        $("#viweLists>li.playing").removeClass('playing');
    }
    var audioPlayPause = function () { //播放出错
        if (!audioDom.Error) {
            $(".leaflet-popup-content>div").attr('viewid') == currentViwe.viweID && $("#controlPlay").attr('class', 'control-play playPause');
            audioDom.attr('class', 'playPause');
            $("#viweLists>li.playing").removeClass('playing');
        }

    }
    var audioPlayError = function () { //播放暂停   
        if (audioDom[0].src != '' && (/(.mp3|.mp4)$/.test(audioDom[0].src))) {
            //commonMethod.setMessage('音频播放出错了，您可以尝试切换语言类型或向我们投诉...');
            commonMethod.setMessage('加载音频中...');
            $("#controlPlay").attr('class', 'control-play playError');
            audioDom.attr('class', 'playError');
            audioDom.Error = true;
            clearTimeout(MapModule.boundsImgAlter);
            markersList[markerIndex].closePopup();
            openMarkerIndex = null;
            MapModule.currentBoundsImg && (MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
            $("#viweLists>li.playing").removeClass('playing');
        }


    }
    audioDom.bind('timeupdate', audioPlaying); //播放时间变化时更改播放按钮显示状态
    audioDom.bind('waiting', audioPlayWaiting); //监控缓冲时更改播放按钮显示状态    
    audioDom.bind('pause', audioPlayPause); //音频结束时更改播放按钮显示状态  
    audioDom.bind('ended', audioPlayEnd); //音频结束时更改播放按钮显示状态
    audioDom.bind('error', audioPlayError); //音频结束时更改播放按钮显示状态


    /********************************************根据音频的不同状态，改变播放按钮的样式结束********************************************/

    /******************************************************GPS定位设置开始**********************************************/

    /**
     * 设置定位
     */
    this.startLocate = function () {
        //定位 参数说明 watch 是否持续定位 ，setView 定位后是否设置到当前位置 enableHighAccuracy 是否启用高经度
        /*        	mapObj.locate({
                        watch: true,
                        setView: false,
                        enableHighAccuracy: true,
                        timeout: 5 * 1000
                    });*/
        MapModule.locateObj = L.control.locate({
            showPopup: false,
            drawCircle: false,
            drawMarker: false,
            setView: false,
            onLocationError: onLocationError,
            locateOptions: {
                watch: true,
                setView: false,
                enableHighAccuracy: true,
                timeout: 10 * 1000
            }
        }).addTo(map);
        //注册定位成功方法
        MapModule.locateObj.start();

        mapObj.on('locationfound', onLocationFound);
        //注册定位失败方法
        mapObj.on('locationerror', onLocationError);

    };

    //定位失败处理方法
    function onLocationError(e) {
        var errorType = ['您拒绝了位置请求服务', '正在持续为您定位......', '正在持续为您定位......'];

        //触发定位失败事件
        if (e.code == 1) {
            if (!$('body').hasClass('show-aConfirm')) {
                $('body').addClass('show-reject-box');
                commonMethod.slideIn({ "$obj": $('.reject-local-box'), "shadeDiv": $(".autoPlay-confirm-bg") });
            }

            MapModule.rejectLocate = true;
        } else {

            commonMethod.setMessage(errorType[e.code - 1]);

        }

    };

    //定位成功处理函数
    function onLocationFound(e) {
        var i = 0;
        var radius = e.accuracy / 2;
        radius = radius > 300 ? 50 : radius;
        var latlng;
        //alert("第一步：开始定位");
        //gps需要转换
        //var point = PointTransformation.gcj_encrypt(e.latlng.lat, e.latlng.lng);
        var point = transformFromWGSToGCJ(
                parseFloat(e.latlng.lng),
                parseFloat(e.latlng.lat)
            );

        latlng = L.latLng(point.lat, point.lng);
        MapModule.CurrentLatlng = latlng;
        if ($("#disableyMove>.local-btn").hasClass('panto') && !(mapObj.options.maxBounds)) {
            $("#viweLists>li.liSelect").trigger(commonMethod.clickEvent);
            mapObj.panTo(latlng);
            $("#disableyMove>.local-btn").removeClass('panto');
        }

        $("#daohang").click(function () {

            map = new AMap.plugin(['AMap.Driving'], function () {
                var drivingOption = {
                    map: map,
                    panel: "panel"
                };

                var driving = new AMap.Driving(drivingOption); //构造驾车导航类
                //根据起终点坐标规划驾车路线  
                driving.searchOnAMAP({
                    origin: ['' + MapModule.CurrentLatlng.lng + '', '' + MapModule.CurrentLatlng.lat + ''],
                    destination: ['' + mapAgruments.center[1] + '', '' + mapAgruments.center[0] + '']
                });
            });



            //var url = "http://m.amap.com/navi/?start=" + MapModule.CurrentLatlng.lng + ',' + MapModule.CurrentLatlng.lat + "&dest=" + mapAgruments.center[1] + ',' + mapAgruments.center[0] + "&destName=景好云导游&naviBy=walk&key=4c37e67bdb9969ca4ea8ed2c656f0893";
            //window.location.href = url;
        });
        //alert("第二步：已获取当前坐标：" + point.lat + ',' +  point.lng);
        imageLayerBA.length > 0 && mapBounds && (mapBounds = L.latLngBounds(imageLayerBA));
        if (mapBounds === 'noBounds' || (mapBounds && mapBounds.contains(latlng))) { //在当前景区内
            //alert("第三步：定位在景区内。");
            imageLayerBA.length > 0 && (imgMapBounds = L.latLngBounds(imageLayerBA));
            if (imgMapBounds && imgMapBounds.contains(latlng)) {
                //alert("第四步：定位在景区内。");
                if (is_app == "2") {
                    location.href = 'http://app.jhlxw.com/index.php/Api/Load/Html_share';
                } else {
                    loadFirst && (mapObj.panTo(latlng), $("#autoPlay").hasClass('on') && (commonMethod.setMessage('定位到您在景区内，很高兴为您导游')), loadFirst = false);
                    MapModule.inViwe = true;
                    $("#disableyMove>.local-btn").hasClass('panto') && (mapObj.panTo(latlng), $("#disableyMove>.local-btn").removeClass('panto'));
                }
            } else {
                //alert("第五步：您不在当前景区范围內。");
                if ($("body").hasClass("show-aConfirm")) {
                    OutViweHint = false;
                    $("body").attr("outViwe", 'true');
                } else {
                    OutViweHint && (commonMethod.setMessage('您不在当前景区范围內！'));
                    OutViweHint = false;
                    $("body").attr("outViwe", 'true');
                }
                $("#disableyMove>.local-btn.panto").length > 0 && (mapBounds === 'noBounds' && ($("#viweLists>li.liSelect").trigger(commonMethod.clickEvent), mapObj.panTo(latlng)), $("#disableyMove>.local-btn.panto").removeClass('panto'), commonMethod.setMessage('您不在当前景区范围內！'));
                MapModule.inViwe = false;

                return false;

            }

        } else {
            $("#disableyMove>.local-btn.panto").length > 0 && ($('#mapFeatures').hasClass('rich') && ($("#viweLists>li.liSelect").trigger(commonMethod.clickEvent), mapObj.panTo(latlng)), $("#disableyMove>.local-btn.panto").removeClass('panto'), commonMethod.setMessage('您不在当前景区范围內！'));
            //alert("第六步：您不在当前景区范围內。");
            if ($("body").hasClass("show-aConfirm")) {
                OutViweHint = false;
                $("body").attr("outViwe", 'true');
            } else {
                OutViweHint && (commonMethod.setMessage('您不在当前景区范围內！'));
                OutViweHint = false;
                $("body").attr("outViwe", 'true');
            }



        }
        if (locationMark == null) {
            var icon = L.divIcon({
                className: '',
                html: '<span id="localIcon" class="icon_local" ></span>'
            });
            locationMark = L.marker(latlng, {
                icon: icon
            });
            locationCircle = L.circle(latlng, radius, {
                weight: '2',
                'color': '#1f6cf4',
                'fillColor': '#1f6cf4'
            });
            markersListGroup.addLayer(locationCircle);
            markersListGroup.addLayer(locationMark);
            setLocationMarkStyle();
        } else {
            locationMark.setLatLng(latlng);
            locationCircle.setLatLng(latlng);
            locationCircle.setRadius(radius);
        }
        if (mapMethod.inViwe && audioDom.attr('guidePlayed') == 'false' && $("#autoPlay").hasClass('on')) {

            $('#marker' + $(".leaflet-popup-pane .leaflet-popup-content>div").attr('viewid')).trigger(commonMethod.clickEvent);
            currentViwe = {};
            MapModule.currentBoundsImg = '';
            audioDom[0].src = audioDom.attr('guideInView');
            audioDom[0].play();
            document.addEventListener("WeixinJSBridgeReady", function () {
                audioDom[0].play();
            }, false);
            audioDom.attr('class', 'playing');
            audioDom.attr('guidePlayed', 'true');
            $("#controlPlay").attr('class', 'control-play');
            $("#viweLists>li.liSelect").trigger(commonMethod.clickEvent);

            imgAnimateEnd();
            return false;
        }
        if (/images\/test\/autoguide.mp3/.test(audioDom[0].src) && audioDom.hasClass('playing')) {
            return false;
        }
        if (audioDom.attr('guidePlayed') == 'false') {
            return false;
        }
        if ($("#autoPlay").hasClass('on')) { //判断是否自动播放音频
            //alert("第八步：是否开启自动播放。");
            if (!MapModule.prioriAutoPlay && (audioDom.hasClass("playing") || audioDom.hasClass("playPause") || audioDom.hasClass("Loading"))) return false; //自动播放不优先并且信息窗口已打开并且处于播放状态或暂停状态，不进行自动播放。

            var inAnyoneViwe = inAutoPlayRange(latlng);

            if (inAnyoneViwe) {
                if (currentViwe.viweID != inAnyoneViwe.viweID) {
                    $("#viweLists>#marker" + inAnyoneViwe.viweID).trigger(commonMethod.clickEvent);
                }
                //alert("第九步");
                markerIndex = inAnyoneViwe.index;
                inAnyoneViwe = inAnyoneViwe.data;
                if (commonMethod.isNullObj(inAnyoneViwe.audioUrl)) {
                    commonMethod.setMessage('当前景点没有语音！');
                } else {
                    var Url = inAnyoneViwe.audioUrl['audioId' + audioType] ? inAnyoneViwe.audioUrl['audioId' + audioType] : 'noUrl'
                    //alert("第十步"+Url);
                    if (Url === 'noUrl') {
                        commonMethod.setMessage('当前景点没有您选中的语音类型,已切换其它类型语音！');
                        for (var name in inAnyoneViwe.audioUrl) {
                            Url = inAnyoneViwe.audioUrl[name];
                            break;
                        }
                    }
                    //alert("第十一步");
                    audioDom[0].loop = false;
                    var urlEquivalent = new RegExp(Url + '$').test(audioDom[0].src);
                    if (currentViwe.viweID === inAnyoneViwe.viweID && audioDom.hasClass('playError') && urlEquivalent) return false; //如果语音错误，退出。。。


                    if (!(currentViwe.viweID === inAnyoneViwe.viweID && (audioDom.hasClass('playing') || audioDom.hasClass('playPause') || audioDom.hasClass('played')))) {
                        commonMethod.setMessage('亲爱的，您到"' + inAnyoneViwe.viweName + '"了。');

                        _hides();

                        //alert("第十二步");
                        playcount();
                        //alert("第十三步：" + Url);
                        audioDom[0].src = Url;
                        audioDom[0].play();
                        document.addEventListener("WeixinJSBridgeReady", function () {
                            audioDom[0].play();
                        }, false);
                        audioDom.addClass('playing').removeClass('playPause');
                        currentViwe.viweID !== inAnyoneViwe.viweID && markersList[markerIndex].openPopup();
                        currentViwe = inAnyoneViwe;
                        imgAnimateBegin();
                        //alert("第十四步");
                    }
                }
            } else {
                if (!audioDom.hasClass('playing')) {
                    imgAnimateEnd();
                }

            }


        }

        if ($('#DetailsContainer').attr('url')) {
            if (confirm('是否跳转到高德地图导航?')) {
                var destination = $('#DetailsContainer').attr('url').split(',');

                map = new AMap.plugin(['AMap.Driving'], function () {
                    var drivingOption = {
                        map: map,
                        panel: "panel"
                    };

                    var driving = new AMap.Driving(drivingOption); //构造驾车导航类
                    //根据起终点坐标规划驾车路线  
                    driving.searchOnAMAP({
                        origin: ['' + MapModule.CurrentLatlng.lng + '', '' + MapModule.CurrentLatlng.lat + ''],
                        destination: ['' + marker.position[1] + '', '' + marker.position[0] + '']
                    });
                });
                //var url = "http://m.amap.com/navi/?start=" + MapModule.CurrentLatlng.lng + ',' + MapModule.CurrentLatlng.lat + "&dest=" + marker.position[1] + ',' + marker.position[0] + "&destName=" + marker.viweName + "&naviBy=walk&key=4c37e67bdb9969ca4ea8ed2c656f0893";
                //window.location.href = url;
            }
            $('#DetailsContainer').removeAttr('url');
        }




    };
    var _hides = function () {
        if ($("#div1").val() == "0") {
            $("#topdiv").slideToggle();
            $("#tog").css({ "top": "0px" });
            $("#imgtop").attr("src", "/images/down_arrow.png");
            $("#div1").val("1");
        }
    }
    function imgAnimateBegin() {
        clearTimeout(MapModule.boundsImgAlter);
        MapModule.currentBoundsImg && (MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
        MapModule.currentBoundsImg = markersList[markerIndex]._icon;
        MapModule.currentBoundsImg && (MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
        MapModule.boundsImgAlter = setInterval(MarkerImgAnimate, 300);
    }

    function imgAnimateEnd() {
        clearTimeout(MapModule.boundsImgAlter);
        MapModule.currentBoundsImg && (MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
    }

    function setLocationMarkStyle() {
        $("span#localIcon").parent().css({
            width: '.32rem',
            height: '.32rem',
            margin: '-.16rem 0 0 -.16rem',
            "border-radius": "50%",
            border: 'solid white 3px',
            'background-color': "#3d80f5",
            'box-sizing': 'border-box'
        });
    }

    /******************************************************GPS定位设置结束**********************************************/
    function toRad(d) { return d * Math.PI / 180; }
    function getDisance(lat1, lng1, lat2, lng2) { //lat为纬度, lng为经度
        var dis = 0;
        var radLat1 = toRad(lat1);
        var radLat2 = toRad(lat2);
        var deltaLat = radLat1 - radLat2;
        var deltaLng = toRad(lng1) - toRad(lng2);
        var dis = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(deltaLat / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(deltaLng / 2), 2)));
        return dis * 6378137;
    }
    /******************************************************GPS定位设置结束**********************************************/

    function inAutoPlayRange(latlng) { //判断是否在自动播放的范围內
        var _lat = latlng.lat;
        var _lng = latlng.lng;

        //if (currentViwe && currentViwe.area && isPointInPolygon(latlng, currentViwe.area)) return {
        //    data: currentViwe,
        //    index: markerIndex
        //};

        if (currentViwe && currentViwe.distance > 0) {
            var lat = currentViwe.position[0];
            var lng = currentViwe.position[1];
            var count = getDisance(_lat, _lng, lat, lng);
            if (count <= currentViwe.distance) return {
                data: currentViwe,
                index: markerIndex
            };
        }

        for (var i = 0, len = markersList.length; i < len; i++) {
            var set_lat = markersList[i].markerData.position[0];
            var set_lng = markersList[i].markerData.position[1];
            var _count = getDisance(_lat, _lng, set_lat, set_lng);
            var is_bea = false;
            if (isiBeacons) {
                for (var _i = 0; _i < isiBeacons.beacons.length; _i++) {

                    if (isiBeacons.beacons[_i].minor == markersList[i].markerData.major) {
                        if (markersList[i].markerData.distance >= isiBeacons.beacons[_i].accuracy) {
                            return {
                                data: markersList[i].markerData,
                                index: i
                            };
                        }
                    }
                }
            } else {
                if (_count <= markersList[i].markerData.distance) {
                    return {
                        data: markersList[i].markerData,
                        index: i
                    };
                };
            }


            //if (isPointInPolygon(latlng, markersList[i].markerData.area)) {
            //    return {
            //        data: markersList[i].markerData,
            //        index: i
            //    };
            //};
        }

        return false;
    }

    //音频切换   
    this.audioChange = function (lis) {
        for (var i = lis.length - 1; i >= 0; i--) {
            $(lis[i]).bind(commonMethod.clickEvent, function () {

                if (!$(this).hasClass('liSelect')) { //当前语音类型没有被选中则切换语音类型。

                    $(".navAni .liSelect").removeClass('liSelect');
                    $(this).addClass("liSelect");
                    audioType = $(this).attr("audioid");
                    commonMethod.setMessage('您选择了 ' + $(this).find(".sTxt").text());
                    var playBtn = $('#controlPlay'),
                        audioDom = $('#videoPlay');
                    if (audioDom.hasClass("playing")) { //如果当前语音正在播放
                        if (currentViwe.audioUrl['audioId' + audioType] != null) { //如果这个景点存在该类型语音，则切换到当前类型的音频并播放。否则，提示没有该类型语音。
                            audioDom[0].src = currentViwe.audioUrl['audioId' + audioType];
                            audioDom[0].load();
                            audioDom[0].play();
                            document.addEventListener("WeixinJSBridgeReady", function () {
                                audioDom[0].play();
                            }, false);
                        } else {
                            setMessage('当前景点没有该类型的语音！');
                        }

                    }

                }
            })
        }
    }
    //音频切换结束

    /*        //设置性能模式
            $("#mapFeatures").bind(commonMethod.clickEvent, function() {
                if (!$(this).hasClass('rich')) {
                    commonMethod.setMessage('已切换为全局模式，可查看当前景区以外区域地图！');
                    mapObj.setMaxBounds();
                    mapObj.addLayer(gaodeLayers);
                    $(this).addClass('rich');
                    mapObj.options.minZoom = 3;
                    mapObj.options.maxZoom = 18;
                } else {
                    $(this).removeClass('rich');
                    mapObj.setMaxBounds(mapAgruments.maxBounds);
                    mapObj.removeLayer(gaodeLayers);
                    $(".mapContainer19.size19").removeClass('size19');
                    mapObj.setView(MapcenterCoordinate, MapZoomRange[0] > mapObj.getZoom() ? MapZoomRange[0] : mapObj.getZoom());
                    mapObj.options.minZoom = MapZoomRange[0];
                    mapObj.options.maxZoom = MapZoomRange[1];
                    commonMethod.setMessage('已切换为局部模式，只能查看当前景区地图！');
                }
            });
            //设置性能模式结束
    */

    /******************************************************景点详情展示**********************************************/

    var showDetails = function (marker, content) {

        var DetailsHeader = $('#viweDetails .header>h3');
        var DetailsContainer = $('#DetailsContainer');
        var showedViweName = DetailsHeader.text();
        var goToHere = function () { //去哪儿            
            if (MapModule.CurrentLatlng) {
                //var url = "http://m.amap.com/navi/?start=" + MapModule.CurrentLatlng.lng + ',' + MapModule.CurrentLatlng.lat + "&dest=" + marker.position[1] + ',' + marker.position[0] + "&destName=" + marker.viweName + "&naviBy=walk&key=4c37e67bdb9969ca4ea8ed2c656f0893";

                if (confirm('是否跳转到高德地图导航?')) {

                    map = new AMap.plugin(['AMap.Driving'], function () {
                        var drivingOption = {
                            map: map,
                            panel: "panel"
                        };

                        var driving = new AMap.Driving(drivingOption); //构造驾车导航类
                        //根据起终点坐标规划驾车路线  
                        driving.searchOnAMAP({
                            origin: ['' + MapModule.CurrentLatlng.lng + '', '' + MapModule.CurrentLatlng.lat + ''],
                            destination: ['' + marker.position[1] + '', '' + marker.position[0] + '']
                        });
                    });
                    //window.location.href = url;
                }
            } else {
                commonMethod.setMessage('路线规划中，请稍等。');
                DetailsContainer.attr('url', marker.position[1] + ',' + marker.position[0] + ',' + marker.viweName);
            }


        }
        if (showedViweName === marker.viweName) {
            commonMethod.hideMolde('details');
            $('#viweDetails,body').addClass('show-details');
            commonMethod.slideIn({ "$obj": $('#viweDetails'), "shadeDiv": $(".autoPlay-confirm-bg") });
        } else {
            DetailsHeader.text(marker.viweName);

            if (content != "") {
                DetailsContainer.html(content)
            }
            commonMethod.hideMolde('details')
            $('#viweDetails,body').addClass('show-details');
            commonMethod.slideIn({ "$obj": $('#viweDetails'), "shadeDiv": $(".autoPlay-confirm-bg") });
            /** 点击去这获取位置并跳转到高德页面查询路线*/
            !$("#viweDetails .goToHere").hasClass('bindgoToHere') && ($("#viweDetails .goToHere").bind(commonMethod.clickEvent, goToHere).addClass('bindgoToHere'));
        }
    }

    var detailsScrollDo = function () {
        if (detailsScroll && detailsScroll != "") {
            detailsScroll.destroy();
            detailsScroll = null;
        }
        detailsScroll = new IScroll("#DetailsContainerScroll", {
            scrollbars: true,
            interactiveScrollbars: true,
            shrinkScrollbars: 'scale',
            fadeScrollbars: true
        });
    }

    /******************************************************景点详情结束**********************************************/



    /*********************************************************路线相关操作********************************************************/
    this.addPolylines = function (lines) {
        var lines = lines ? lines : {};
        var lineUl = $("#menu-list .selectLine>dd>ul");
        var index = 1;
        lineUl.html("");

        //<li class="lineItem"><span class="sTxt"><em>1</em>民俗文化体验线</span></li>
        for (var name in lines) {
            var li = $("<li class='lineItem'><span class='sTxt'><em>" + index + "</em>" + name + "</span></li>");
            var point = [];
            var markers = [];

            for (var i = 0, len = lines[name].length; i < len; i++) {
                var position = lines[name][i].position.split(',');
                point[point.length] = L.latLng(position[1], position[0]);
                if (lines[name][i].mark === '1') {
                    markers.push(L.marker([position[1], position[0]], {
                        icon: L.divIcon({
                            className: '',
                            html: '<span  class="icon_track" >' + (markers.length + 1) + '</span>'
                        })
                    }));
                }
            }

            var polyline = L.polyline(point, {
                color: '#ff664f',
                weight: 10,
                opacity: 1
            });
            var markers = L.layerGroup(markers);
            polyline.index = index;
            // var polylineAll =L.layerGroup([polyline]);
            lineUl.append(li);
            //polyline.addTo(mapObj)
            (function (p, m, i, li) {

                L.DomEvent.addListener($(li)[0], 'click', function () {

                    $("body").removeClass('show-menu');
                    if (!authorization()) { return false };
                    if ($(this).hasClass('liSelect')) {
                        $(this).removeClass('liSelect');
                        mapObj.removeLayer(currentTrack.polyline);
                        mapObj.removeLayer(currentTrack.markers);
                        currentTrack.polyline = null;
                        currentTrack.markers = null;
                        $(".leaflet-overlay-pane.filter").removeClass('filter');
                    } else {
                        if ($(this).siblings('li.liSelect').length > 0) {
                            $(this).siblings('li.liSelect').removeClass('liSelect');
                            mapObj.removeLayer(currentTrack.polyline)
                            mapObj.removeLayer(currentTrack.markers);
                        }

                        currentTrack.polyline = p;
                        currentTrack.markers = m;
                        $(this).addClass('liSelect');
                        p.addTo(mapObj)
                        m.addTo(mapObj);
                        $(".leaflet-overlay-pane").addClass('filter');
                        $('.icon_track').each(function () {
                            $(this).parent().css({
                                margin: 0,
                                width: $(this).width(),
                                height: $(this).height()
                            })
                        })
                    }

                })
            })(polyline, markers, index, li);

            index++;
        }
    }

    /**********************************************************路线相关操作结束*******************************************************/

    var MarkerImgAnimate = function () {
        if (/images\/Ht-iconMarkerAudioDim.png/.test(MapModule.currentBoundsImg.src)) {
            MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png'
        } else {
            MapModule.currentBoundsImg.src = '/images/Ht-iconMarkerAudioDim.png'
        }
    }
    //将一些共用的变量绑定到this上
    this.mapContainerDom = mapContainerDom;
    this.mapContainerH = mapContainerH;
    this.CurrentLatlng = null;



}
/****************************************地图方法模块化结束***********************************************************************************************/

/****************************************DOM操作开始***********************************************************************************************/
var DomOperate = function (mapMethod, mapObj) {
    var isiOS = !!navigator.userAgent.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
    //给下方列表设置滚动条
    var mToolScroll = new IScroll("#scrollCont", {
        scrollbars: true,
        mouseWheel: true,
        interactiveScrollbars: true,
        shrinkScrollbars: 'scale',
        click: true,
        tap: true,
        fadeScrollbars: true
    });
    var audioDom = $("#videoPlay");

    /*************关闭顶部APP广告条结束************************************/

    //底部菜单列表效果

    //底部菜单列表展开
    $(".toolNav>ul>li").bind(commonMethod.clickEvent, function () {
        $("#viweLists").removeClass('hasResult resultNull');
        $("#viweLists>li.result").removeClass("result");
        $("#spotsSearch").val('');
        if ($("#menu-list>dl").eq($(this).index()).hasClass('navAni')) {
            $("#menu-list>dl").eq($(this).index()).removeClass('navAni');
            $("body").removeClass('show-menu');
            return false;
        } else {
            commonMethod.hideMolde();
            $("#menu-list>dl.navAni").removeClass('navAni');
            $("body").addClass('show-menu');
            $("#menu-list>dl").eq($(this).index()).addClass('navAni');
            if (!$("#menu-list>dl").eq($(this).index()).find("dd#scrollCont").length > 0) {
                $("#menu-list>dl dd#scrollCont").removeAttr('id');
                $("#menu-list>dl").eq($(this).index()).find("dd").attr("id", "scrollCont");
                mToolScroll.destroy();
                mToolScroll = new IScroll("#scrollCont", {
                    scrollbars: true,
                    mouseWheel: true,
                    interactiveScrollbars: true,
                    shrinkScrollbars: 'scale',
                    click: true,
                    fadeScrollbars: true
                });
                mToolScroll.on("scrollStart", function () {
                    /**/
                });
            }

            if ($(this).hasClass('viweList-li') && $("#viweLists>li.liSelect") && $(".leaflet-popup").length <= 0) { //打开景点列表时如果当前没有打开窗口，取消选中。
                $("#viweLists>li.liSelect").removeClass('liSelect');
            }

            return false;


        }
    });
    //底部菜单列表展开结束



    $(document).on(commonMethod.clickEvent, "#menu-list dl>dd>ul>li", function () {
        $("body").addClass('cancelSearch');
        $("#viweLists").removeClass('hasResult resultNull');
        $("#viweLists>li.result").removeClass("result");
        $("#spotsSearch").val('');
        $(this).parents('dl.navAni').removeClass('navAni');
        $("body").removeClass('show-menu').removeClass("SearchFocus").removeClass('cancelSearch');
        $("#spotsSearch").blur();
        return false;
    });

    //底部菜单列表关闭

    //底部菜单列表效果结束
    /*******************************************底部菜单列表效果结束************************************/

    //定位按钮点击添加click属性

    $("#disableyMove>.local-btn").bind(commonMethod.clickEvent, function () {
        if (mapMethod.rejectLocate) {
            $("body").addClass('show-reject-box');

            commonMethod.slideIn({ "$obj": $('.reject-local-box'), "shadeDiv": $(".autoPlay-confirm-bg") });

            return false;
        }
        if ($(this).hasClass('panto')) return;
        $(this).addClass('panto');

        mapMethod.locateObj.stop();
        mapMethod.locateObj.start();


    });
    //自动导游
    $("#autoPlay").bind(commonMethod.clickEvent, function () {

        if (!authorization()) {
            return false;
        };
        if ($(this).hasClass('on')) {
            $(this).removeClass('on');
            commonMethod.setMessage('自动播放已关闭！');
            if (!$(this).hasClass('manualPlay')) { //如果是手动点击播放按钮并且音频未播放结束，关闭自动播放后不暂停当前播放音频
                audioDom[0].pause();
                audioDom.removeClass('playing');
                $("#controlPlay").removeClass('playing');
                clearTimeout(mapMethod.boundsImgAlter);
                mapMethod.currentBoundsImg && (mapMethod.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
            }



        } else {
            if (mapMethod.rejectLocate) {
                $("body").addClass('show-reject-box');
                commonMethod.slideIn({ "$obj": $('.reject-local-box'), "shadeDiv": $(".autoPlay-confirm-bg") });
                return false;
            }
            $(this).addClass('on');

            commonMethod.setMessage(mapMethod.inViwe ? '定位到您在景区内，很高兴为您导游！' : '当您到达景区后，自动播放自动开始');
            if (!audioDom.hasClass('playing') && mapMethod.inViwe && audioDom.attr('guidePlayed') == 'false') {

                $("#controlPlay").attr('class', 'control-play');
                $('#marker' + $(".leaflet-popup-pane .leaflet-popup-content>div").attr('viewid')).trigger(commonMethod.clickEvent);
                audioDom[0].src = audioDom.attr('guideinview');
                audioDom[0].play();
                document.addEventListener("WeixinJSBridgeReady", function () {
                    audioDom[0].play();
                }, false);
                audioDom.attr('class', 'playing');
                audioDom.attr('guidePlayed', 'true');
            } else {

                audioDom[0].src = '';
                audioDom[0].play();
                document.addEventListener("WeixinJSBridgeReady", function () {
                    audioDom[0].play();
                }, false);
                audioDom.attr('class', '');
                clearTimeout(mapMethod.boundsImgAlter);
                mapMethod.currentBoundsImg && (mapMethod.currentBoundsImg.src = '/images/Ht-iconMarkerAudio.png');
                $("#controlPlay").attr('class', 'control-play');
                $('#marker' + $(".leaflet-popup-pane .leaflet-popup-content>div").attr('viewid')).trigger(commonMethod.clickEvent);

            }

            mapMethod.prioriAutoPlay = true;



        }
    })


    /*******************************************错误反馈交互************************************/
    var feedBackDom = $('#feedBackDom');
    var backBtn = $("#feedBackDom div.back");
    var ClosefeedBack = $("#feedBackDom div.close-btn");
    var feedBackSbt = $("#feedBackDom div.submit");
    var showFeedBackBtn = $(".error-correction");


    $("#m-tool-Dom").bind(commonMethod.clickEvent, function () {
        commonMethod.hideMolde()
    })

    $("#backform").submit(function () {
        feedBackSbt.trigger(commonMethod.clickEvent);
        return false;
    })







    $(".ul-choose-scroll>dl>dt").bind(commonMethod.clickEvent, function () {
        var This = this;
        setTimeout(function () {
            feedBackDom.find('.header .title').text($(This).attr("error-text"));
            $(This).siblings().removeClass('selected');
            $(This).addClass('selected');
            feedBackDom.addClass('show-menu')
        }, commonMethod.clickEvent == 'tap' ? 350 : 1)


    });
    //底部菜单列表关闭
    $("#menu-list a.amap-info-close").bind(commonMethod.clickEvent, function () {
        $("body").addClass('cancelSearch');

        $("#viweLists").removeClass('hasResult resultNull');
        $("#viweLists>li.result").removeClass("result");
        $("#spotsSearch").val('').blur();
        $("body").removeClass('SearchFocus');
        $(this).parents('dl.navAni').removeClass('navAni');
        mToolScroll.refresh();
        $("body").removeClass('show-menu');
        return false;
    });
    function spotsSearch(key) {
        var result = $("#viweLists>li:contains('" + key + "')").not(".nullResult");

        if (!key == '' && result.length > 0) {
            $("#viweLists").removeClass('resultNull').addClass("hasResult");
            result.addClass('result');

        } else {
            $("#viweLists>li.result").removeClass("result");
            $("#viweLists").addClass('resultNull');
            if ($("#viweLists>.nullResult").length <= 0) {

                $("<li class='nullResult'>未找到含“" + key + "”的景点或目的地</li>").appendTo($("#viweLists"));
            } else {

                $("#viweLists>.nullResult").text('未找到含“' + key + '”的景点或目的地');
            }

        }
        if (key == '') {
            $("#viweLists").removeClass('hasResult resultNull');
            $("#viweLists>li.result").removeClass("result");

        }
    }


    var HEIGHT = $('body').height();
    if (isiOS) {
        $("input#spotsSearch").bind("focus", function () {
            $('body').scrollTop(0)
        });
    } else {

        $(window).resize(function () {
            if (HEIGHT < $('body').height()) {
                $("body").attr("keyboardUp", 'false');
                $("input#spotsSearch").blur();
                mToolScroll.refresh();
                HEIGHT = $('body').height();

            } else {
                HEIGHT = $('body').height();
                $("body").attr("keyboardUp", 'true');

            }

            mapObj.setMaxBounds();
            mapObj.setMaxBounds([beginCoordinate, endCoordinate]);



        });
    }

    $("input#spotsSearch").on("blur", function () {
        $("body").attr("keyboardUp", 'false');
        $("body").addClass("cancelSearch");
        $("body").removeClass("SearchFocus");
        setTimeout(function () {
            $("body").removeClass("cancelSearch");
        }, 100)
        spotsSearch($(this).val());
        mToolScroll.refresh();
    });
    var detailsScroll, detailsScrollInter;
    $("#recommendDom .help_icon").bind(commonMethod.clickEvent, function () {
        $.get('help.html', function (data) {
            commonMethod.hideMolde('details');
            $('#helpViweDetails,body').addClass('show-details');
            commonMethod.slideIn({ "$obj": $('#helpViweDetails'), "shadeDiv": $(".autoPlay-confirm-bg") });
            $("#helpDetailsContainer").html(data);
            if (!detailsScroll || detailsScroll == "") {
                detailsScroll = new IScroll("#helpDetailsContainerScroll", {
                    scrollbars: true,
                    interactiveScrollbars: true,
                    shrinkScrollbars: 'scale',
                    fadeScrollbars: true
                });
            }
            var images = $('#helpDetailsContainerScroll img');
            detailsScrollInter = setInterval(function () {
                detailsScroll.refresh();
            }, 500);
            if (images && images.length > 0) {
                images[images.length - 1].onload = function () {
                    setTimeout(function () {
                        clearTimeout(detailsScrollInter);
                        detailsScrollInter = null;
                    }, 3000);
                }
            } else {
                setTimeout(function () {
                    clearTimeout(detailsScrollInter);
                    detailsScrollInter = null;
                }, 3000);
            }



        });
    })
    //关闭帮助    
    $("#helpCloseviweDetails").bind(commonMethod.clickEvent, function () {
        commonMethod.slideOut({ "$obj": $('#helpViweDetails'), "shadeDiv": $(".autoPlay-confirm-bg") });
        $('#helpViweDetails,body').removeClass('show-details');
    });



}
/****************************************DOM操作结束***********************************************************************************************/
/****************************************授权码验证***********************************************************************************************/

var verifyCookie = function () {
    loadMap();

    var validCode = commonMethod.getCookie('_usercode');

    if (validCode != null && validCode != "") {

        //如果存在则不显示弹窗
        var flag = false;
        $.ajax({
            url: "/Handler.ashx?p=valid&validCode=" + validCode + "&scenicId=" + scenicId,
            type: "GET",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.info == "ok") {
                    $(".popBox,.popBox .keyPop_Code").hide();
                    $("body").removeClass("hideMap");
                    commonMethod.setMessage('恭喜您已激活该景区！');
                    window.CodeVerify = true;
                    flag = true;
                    $(".autoPlay-confirm").addClass('active');
                    if ((location.href.split('nonShowConfirm=')[1] ? location.href.split('nonShowConfirm=')[1] : "").split("&")[0] === 'true') {
                        $("body").addClass('show-aConfirm');
                        commonMethod.slideIn({ "$obj": $(".autoPlay-confirm"), "shadeDiv": $(".autoPlay-confirm-bg") });
                    }

                }
            }
        }
        );
        return flag;
    } else {
        //$("#autoPlay").trigger("tap");
        window.CodeVerify = false;
        return false;
    }



}
var playcount = function () {
    $.get("/Handler.ashx", { p: "PlayCount", Scenic_id: scenicId, r: Math.random() }, function () { });
}
var authorization = function () {
    var endDate, inputCode;
    if (window.CodeVerify) {
        //授权码验证  
        return true;
    } else {
        $("body").addClass('show-reject-box');
        commonMethod.slideIn({ "$obj": $(".keyPop"), "shadeDiv": $(".popBox") });
        return false;
    }

    //$("body").addClass('show-reject-box');
    //commonMethod.slideIn({ "$obj": $(".keyPop"), "shadeDiv": $(".popBox") });
    //return false;


}



//给激活按钮添加事件
//$(".keyPop_Code .aActivate").bind(commonMethod.clickEvent, function() {
//    var inputCode = $(".keyPop_Code .ddInput textarea").val().trim();
//    var includeText = false;
//    if(inputCode==''){
//    	 $(".keyPop_Code .ddTxt>p").hide();
//        $(".keyPop_Code .input_null").show();
//        return false;
//    }
//    if(inputCode.search(/[^0-9]/)!=-1){
//    	includeText = true;
//    } 

//    if(inputCode.search(/取票凭证码|授权码|【电子票|预订成功/)>-1){
//    	if(inputCode.match(/(取票凭证码|授权码|【电子票|预订成功).*?([0-9]{6,10})/)){
//        	inputCode = inputCode.match(/(取票凭证码|授权码|【电子票|预订成功).*?([0-9]{6,10})/)[2]
//        }else{
//            $(".keyPop_Code .ddTxt>p").hide();
//            $(".keyPop_Code .input_error").show();
//            return false;
//        }
//    }else{
//        if(inputCode.match(/[0-9]{6,10}/)){
//        	inputCode = 	inputCode.match(/[0-9]{6,10}/)[0]
//        }else{
//            $(".keyPop_Code .ddTxt>p").hide();
//            $(".keyPop_Code .input_error").show();
//            return false;
//        }
//    }
//    $.ajax({
//        url: isAuthUrl + inputCode + "&scenicId=" + scenicId,
//        type: "GET",
//        dataType: "json",
//        async:false,
//        success: function(data) {
//            endDate = data.endDate;

//            $(".keyPop_Time .dateEnd").html(endDate);
//            if (data.errorMessage == undefined && data.beginUseDate != "") {
//                commonMethod.setCookie('validCode', inputCode, 365);           
//                window.CodeVerify = true; 
//                $(".keyPop.keyPop_Code").addClass('success');
//                $(".keyPop.keyPop_Code .successTip>span").html(data.useEndDate);
//                $(".keyPop.keyPop_Code .understand").bind(commonMethod.clickEvent,function(){
//                	commonMethod.slideOut({"$obj":$(".keyPop"),"shadeDiv":$(".popBox")});
//             	    $("body").removeClass('show-reject-box'); 
//             	   $(".keyPop_Code .ddInput>textarea").val();
//            	   $(".keyPop_Code .ddTxt>p").hide();
//            	   $(".keyPop_Code .ddTxt>.pTxt1").show();
//            	   $("#autoPlay").trigger(commonMethod.clickEvent);
//                });


//            } else if (data.errorMessage == "1" || data.errorMessage == "2") {

//                $(".keyPop_Code .ddTxt>p").hide();
//            	if(includeText){
//            		$(".keyPop_Code .pTxt2-1").show();
//            	}else{
//            		 $(".keyPop_Code .pTxt2").show();
//            	}

//            } else if (data.errorMessage == "4") {
//                $(".keyPop_Code .ddTxt>p").hide();
//                if(data.beginUseDate){
//                	$(".pTxt3 .expireTip").html("有效时间："+data.beginUseDate.replace(/-/g, '')+'-'+data.useEndDate.replace(/-/g, ''));
//                }else{
//                	$(".pTxt3 .expireTip").html("您未在"+data.endDate.replace(/-/g, '')+"前激活");
//                }

//                $(".keyPop_Code .pTxt3").show();

//            } 

//        }
//    });
//});

//关闭激活框
$(".keyPop_Code .verifyColse").bind(commonMethod.clickEvent, function () {
    commonMethod.slideOut({ "$obj": $(".keyPop"), "shadeDiv": $(".popBox") });
    $("body").removeClass('show-reject-box');
    $(".keyPop_Code .ddTxt>p").hide();
    $(".keyPop_Code .ddTxt>.pTxt1").show();
    $(".keyPop_Code .ddInput>textarea").val();
})
$(".keyPop_Code .ddInput>textarea").bind("focus", function () {
    !$(this).parent().hasClass('active') && $(this).parent().addClass('active');

})
$(".keyPop_Code .ddInput>textarea").bind("blur", function () {
    if (!$(this).val()) {
        $(this).parent().removeClass('active');
    }
})
/*$(".keyPop_Code .ddInput>.clear_icon").bind(commonMethod.clickEvent,function(){
	$(this).parent().removeClass('active');
	$(".keyPop_Code .ddInput>input").val('').focus();
});
commonMethod.slideIn({"$obj":$(".keyPop"),"shadeDiv":$(".popBox")});
$("body").removeClass('show-reject-box'); */
/****************************************授权码验证结束***********************************************************************************************/