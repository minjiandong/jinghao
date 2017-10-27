<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="JH.Account._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=Common.Utility.SystemName %></title>
    <link href="/NewUI/blue/css/public.css" rel="stylesheet" type="text/css" />
    <link href="/Content/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" />
    <link href="/NewUI/blue/css/myStyle.css" rel="stylesheet" type="text/css">
    <link href="/NewUI/lib/ligerUI/skins/gray2014/css/all.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" id="mylink" />
    <script src="/NewUI/lib/jquery/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script src="/NewUI/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="/NewUI/lib/jquery.cookie.js"></script>
    <script src="/NewUI/lib/json2.js"></script>
    <link href="/NewUI/scrollbar/perfect-scrollbar.css" rel="stylesheet" />
    <script src="/NewUI/scrollbar/jquery.mousewheel.js"></script>
    <script src="/NewUI/scrollbar/perfect-scrollbar.js"></script>
    <script src="/Scripts/jquerysession.js"></script>
    <script>
        jQuery(document).ready(function ($) {
            $('#left-sidebar-warp').perfectScrollbar();
        });
    </script>
    <script type="text/javascript">
        var tab = null;
        var accordion = null;
        var tree = null;
        var tabItems = [];
        var layout = null;
        $(function () {
     
            //布局
            layout = $("#layout1").ligerLayout({
                leftWidth: 240,
                height: '100%',
                heightDiff: -34,
                allowLeftResize: true,
                space: 4,
                onHeightChanged: f_heightChanged,
                onLeftToggle: function () {
                    tab && tab.trigger('sysWidthChange');
                },
                onRightToggle: function () {
                    tab && tab.trigger('sysWidthChange');
                }
            });
            $("#left-sidebar-warp").css({ "height": $(".l-layout-left").height() });
            var height = $(".l-layout-center").height();

            //Tab
            tab = $("#framecenter").ligerTab({
                height: height,
                showSwitchInTab: false,
                showSwitch: false,
                contextmenu: true,
                onAfterAddTabItem: function (tabdata) {
                    tabItems.push(tabdata);
                    //saveTabStatus();
                },
                onAfterRemoveTabItem: function (tabid) {
                    for (var i = 0; i < tabItems.length; i++) {
                        var o = tabItems[i];
                        if (o.tabid == tabid) {
                            tabItems.splice(i, 1);
                            //saveTabStatus();
                            break;
                        }
                    }
                },
                onReload: function (tabdata) {
                    var tabid = tabdata.tabid;
                    addFrameSkinLink(tabid);
                }
            });

            //面板
            $("#accordion1").ligerAccordion({
                height: height - 24, speed: null
            });

            $(".l-link").hover(function () {
                $(this).addClass("l-link-over");
            }, function () {
                $(this).removeClass("l-link-over");
            });
            //树
            //$("#tree1").ligerTree({
            //    data: indexdata,
            //    checkbox: false,
            //    slide: false,
            //    nodeWidth: 180,
            //    attribute: ['nodename', 'url'],
            //    render: function (a) {
            //        if (!a.isnew) return a.text;
            //        return '<a href="' + a.url + '" target="_blank">' + a.text + '</a>';
            //    },
            //    onSelect: function (node) {
            //        if (!node.data.url) return;
            //        if (node.data.isnew) {
            //            return;
            //        }
            //        var tabid = $(node.target).attr("tabid");
            //        if (!tabid) {
            //            tabid = new Date().getTime();
            //            $(node.target).attr("tabid", tabid)
            //        }
            //        f_addTab(tabid, node.data.text, node.data.url);
            //    }
            //});



            function openNew(url) {
                var jform = $('#opennew_form');
                if (jform.length == 0) {
                    jform = $('<form method="post" />').attr('id', 'opennew_form').hide().appendTo('body');
                } else {
                    jform.empty();
                }
                jform.attr('action', url);
                jform.attr('target', '_blank');
                jform.trigger('submit');
            };
            tab = liger.get("framecenter");
            accordion = liger.get("accordion1");
            tree = liger.get("tree1");
            $("#pageloading").hide();

            css_init();
            pages_init();

          

            //查询功能模块
            $("#btnSearch").click(function () {
                $.ligerDialog.hide();
                var name = $("#searchstr").val();
                if ($.trim(name) != "") {
                    highlight();
                }
                else {
                    clearSelection();
                    close();
                }
            });
           
            $("#btnClose").click(function () {
                $.ligerDialog.hide();
                close();
                location.href = "#md_1";
            });
            var _top;
            var i = 0;
            var sCurText;
            //给关键字覆盖样式
            function highlight() {
                clearSelection();//先清空一下上次高亮显示的内容； 

                var flag = 0;
                var bStart = true;

                //查找匹配 
                var searchText = $('#searchstr').val();//获取你输入的关键字； 
                var _searchTop = $('#searchstr').offset().top + 30;
                var _searchLeft = $('#searchstr').offset().left;
                var regExp = new RegExp(searchText, 'g');//创建正则表达式，g表示全局的，如果不用g， 
                //则查找到第一个就不会继续向下查找了；
                var content = $("#content").text();
                if (!regExp.test(content)) {
                    $.ligerDialog.warn('没有找到要查找的菜单');
                    return;
                } else {
                    open();
                    if (sCurText != searchText) {
                        i = 0;
                        sCurText = searchText;
                    }
                }
                //高亮显示 
                $('b').each(function () {
                    var html = $(this).html();
                    //将找到的关键字替换，加上highlight属性； 
                    var newHtml = html.replace(regExp, '<span class="highlight">' + searchText + '</span>');
                    $(this).html(newHtml);//更新； 
                    flag = 1;
                });
                if (flag == 1) {
                    if ($(".highlight").size() > 1) {
                        _top = $(".highlight").eq(i).offset().top + $(".highlight").eq(i).height();
                    } else {
                        _top = $(".highlight").offset().top + $(".highlight").height();
                    }
                    $("html,body").animate({ scrollTop: _top - 50 }, 500);
                    i++;
                    if (i > $(".highlight").size() - 1) {
                        i = 0;
                    }
                }
            }
        });


        //清除搜索后关键字的样式
        function clearSelection() {
            $('b').each(function () {
                //找到所有highlight属性的元素； 
                $(this).find('.highlight').each(function () {
                    $(this).replaceWith($(this).html());//将他们的属性去掉； 
                });
            });
        }
        //展开所有菜单
        function open() {
            var node = $(".oneMenuTitle").next();
            if (node.attr('data-state') == 0) {
                node.slideDown();
                node.attr('data-state', 1);
                $(this).addClass('close-menu');
            }
            var nodetwo = $(".towMenuTitle").next();
            if (nodetwo.attr('data-state') == 0) {
                nodetwo.slideDown();
                nodetwo.attr('data-state', 1);
            }
            var nodethree = $(".threeMenuTitle").next();
            $('.threeMenuTitle').parent().siblings().find('.threeMenuTitle').removeClass('threeMenuTitle-hover');
            if (nodethree.attr('data-state') == 0) {
                nodethree.slideDown();
                nodethree.attr('data-state', 1);
            }
        }
        function close() {
            var node = $(".oneMenuTitle").next();
            if (node.attr('data-state') == 1) {
                node.slideUp();
                node.attr('data-state', 0);
                $(this).removeClass('close-menu');
            }
            var nodetwo = $(".towMenuTitle").next();
            if (nodetwo.attr('data-state') == 1) {
                nodetwo.slideUp();
                nodetwo.attr('data-state', 0);
            }
            var nodethree = $(".threeMenuTitle").next();
            $('.threeMenuTitle').parent().siblings().find('.threeMenuTitle').removeClass('threeMenuTitle-hover');
            if (nodethree.attr('data-state') == 1) {
                nodethree.slideUp();
                nodethree.attr('data-state', 0);
            }
        }

        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 24 > 0)
                accordion.setHeight(options.middleHeight - 24);
        }
        function f_addTab(tabid, text, url) {

            tab.addTabItem({
                tabid: tabid,
                text: text,
                url: url,
                callback: function () {
                    addFrameSkinLink(tabid);
                }
            });
        }


        function addFrameSkinLink(tabid) {
            var prevHref = getLinkPrevHref(tabid) || "";
            var skin = getQueryString("skin");
            if (!skin) return;
            skin = skin.toLowerCase();
            attachLinkToFrame(tabid, prevHref + skin_links[skin]);
        }
        var skin_links = {
            "aqua": "/NewUI/lib/ligerUI/skins/Aqua/css/ligerui-all.css",
            "gray": "/NewUI/lib/ligerUI/skins/Gray/css/all.css",
            "silvery": "/NewUI/lib/ligerUI/skins/Silvery/css/style.css",
            "gray2014": "/NewUI/lib/ligerUI/skins/gray2014/css/all.css"
        };
        function pages_init() {
            ///var tabJson = $.cookie('liger-home-tab');
            //  if (tabJson) {
            //  var tabitems = JSON2.parse(tabJson);
            //   for (var i = 0; tabitems && tabitems[i]; i++) {
            //      f_addTab(tabitems[i].tabid, tabitems[i].text, tabitems[i].url);
            //      }
            // }
        }
        function saveTabStatus() {
            //$.cookie('liger-home-tab', JSON2.stringify(tabItems));
        }
        function css_init() {
            var css = $("#mylink").get(0), skin = getQueryString("skin");
            $("#skinSelect").val(skin);
            $("#skinSelect").change(function () {
                if (this.value) {
                    location.href = "index.htm?skin=" + this.value;
                } else {
                    location.href = "index.htm";
                }
            });


            if (!css || !skin) return;
            skin = skin.toLowerCase();
            $('body').addClass("body-" + skin);
            $(css).attr("href", skin_links[skin]);
        }
        function getQueryString(name) {
            var now_url = document.location.search.slice(1), q_array = now_url.split('&');
            for (var i = 0; i < q_array.length; i++) {
                var v_array = q_array[i].split('=');
                if (v_array[0] == name) {
                    return v_array[1];
                }
            }
            return false;
        }
        function attachLinkToFrame(iframeId, filename) {
            if (!window.frames[iframeId]) return;
            var head = window.frames[iframeId].document.getElementsByTagName('head').item(0);
            var fileref = window.frames[iframeId].document.createElement("link");
            if (!fileref) return;
            fileref.setAttribute("rel", "stylesheet");
            fileref.setAttribute("type", "text/css");
            fileref.setAttribute("href", filename);
            head.appendChild(fileref);
        }
        function getLinkPrevHref(iframeId) {
            //if (!window.frames[iframeId]) return;
            //var head = window.frames[iframeId].document.getElementsByTagName('head').item(0);
            //var links = $("link:first", head);
            //for (var i = 0; links[i]; i++) {
            //    var href = $(links[i]).attr("href");
            //    if (href && href.toLowerCase().indexOf("ligerui") > 0) {
            //        return href.substring(0, href.toLowerCase().indexOf("lib"));
            //    }
            //}
        }

        function openTab(tabid, text, url) {
            if (url == "" || url == "#")
                return;
            //if (node.data.isnew) {
            //    return;
            //}
            //var tabid = tabid;
            //if (!tabid) {
            //    tabid = new Date().getTime();
            //    $(node.target).attr("tabid", tabid)
            //}
            f_addTab(tabid, text, "" + url + "?r="+Math.random());
        }
      

        function exit() {
            if ($.ligerDialog.confirm("确定要退出系统吗？", "系统提示", function (e) {
                if (e) {
                    $.ajax({
                        url: "../login.ashx",
                        data: {
                        p: "exit",
                        r: Math.random()
                    },
                    success: function (data) {
                            window.location.href = "../login.aspx";
                    }
                });
            }
            }
            ));
        }
       
       
    </script>
    <style type="text/css">
        body, html {
            height: 100%;
        }

        body {
            padding: 0px;
            margin: 0;
            overflow: hidden;
        }

        .logotitle {
            display: inline-block;
            float: left;
            margin: 20px 0 0 10px;
            width: 20px;/*265px;*/
            height: 42px;
            background:url("") no-repeat;
        }

        .l-link {
            display: block;
            height: 26px;
            line-height: 26px;
            padding-left: 10px;
            text-decoration: underline;
            color: #333;
        }

        .l-link2 {
            text-decoration: underline;
            color: white;
            margin-left: 2px;
            margin-right: 2px;
        }

        .l-layout-top {
            background: #102A49;
            color: White;
        }

        .l-layout-bottom {
            background: #E5EDEF;
            text-align: center;
        }

        #pageloading {
            position: absolute;
            left: 0px;
            top: 0px;
            background: white url('loading.gif') no-repeat center;
            width: 100%;
            height: 100%;
            z-index: 99999;
        }

        .l-link {
            display: block;
            line-height: 22px;
            height: 22px;
            padding-left: 16px;
            border: 1px solid white;
            margin: 4px;
        }

        .l-link-over {
            background: #FFEEAC;
            border: 1px solid #DB9F00;
        }

        .l-winbar {
            background: #2B5A76;
            height: 30px;
            position: absolute;
            left: 0px;
            bottom: 0px;
            width: 100%;
            z-index: 99999;
        }

        .space {
            color: #E7E7E7;
        }
        /* 顶部 */
        .l-topmenu {
            margin: 0;
            padding: 0;
            height: 80px;
            line-height: 80px;
            background: url('lib/images/top.jpg') repeat-x bottom;
            position: relative;
            border-top: 1px solid #1D438B;
        }

        .l-topmenu-logo {
            color: #E7E7E7;
            padding-left: 35px;
            line-height: 26px;
            background: url('lib/images/topicon.gif') no-repeat 10px 5px;
        }

        .l-topmenu-welcome {
            position: absolute;
            height: 24px;
            line-height: 24px;
            right: 30px;
            top: 2px;
            color: #070A0C;
        }

            .l-topmenu-welcome a {
                color: #E7E7E7;
                text-decoration: underline;
            }

        .body-gray2014 #framecenter {
            margin-top: 3px;
        }

        .viewsourcelink {
            background: #B3D9F7;
            display: block;
            position: absolute;
            right: 10px;
            top: 3px;
            padding: 6px 4px;
            color: #333;
            text-decoration: underline;
        }

        .viewsourcelink-over {
            background: #81C0F2;
        }

        .l-topmenu-welcome label {
            color: white;
        }

        #skinSelect {
            margin-right: 6px;
        }

        .highlight {
            background: yellow;
            color: red;
        }

        .btn {
            *padding-top: 3px;
            *padding-bottom: 3px;
        }

        .btn-success {
            color: #ffffff;
            text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.25);
            background-color: #5bb75b;
            background-image: -moz-linear-gradient(top, #62c462, #51a351);
            background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#62c462), to(#51a351));
            background-image: -webkit-linear-gradient(top, #62c462, #51a351);
            background-image: -o-linear-gradient(top, #62c462, #51a351);
            background-image: linear-gradient(to bottom, #62c462, #51a351);
            background-repeat: repeat-x;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ff62c462', endColorstr='#ff51a351', GradientType=0);
            border-color: #51a351 #51a351 #387038;
            border-color: rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.25);
            *background-color: #51a351;
            /* Darken IE7 buttons by default so they stand out more given they won't have borders */
            filter: progid:DXImageTransform.Microsoft.gradient(enabled = false);
        }
    </style>
</head>
<body style="padding: 0px; background: #EAEEF5;">
    <div id="pageloading"></div>
    <div class="header">
        <a id="logo" href="#" class="logotitle" ></a>
        <span class="title"><%=Common.Utility.SystemName %></span>
        <div class="toolbar">
            <ul>  
                <%if (uppass){ %>
                <li>
                    <div class="toolbar-title">
                        <label><i class="userIcon"></i><span class="span1"><a onclick="openTab('123456789','修改密码','/Account/FunctionMenu/editPassword.aspx');">修改密码</a></span></label>
                    </div>
                </li>
                <%} %>
                <li>
                    <div class="toolbar-title">
                        <label><i class="userIcon"></i><span class="span1">欢迎 [<%=fullname %>]</span></label>
                    </div>
                </li>

                <li>
                    <div class="toolbar-title">
                        <label><i class="signOut-icon"></i><span class="span1"><a onclick="exit();">退出</a></span></label>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div id="layout1" style="width: 99.2%; margin: 0 auto; margin-top: 4px;">
        <div position="left" title="主要菜单" id="content">
            <div class="sidebar spread" id="left-sidebar-warp" style="position: relative; margin: 0px auto; padding: 0px; overflow: hidden;" data-state="1">
                <ul class="clearfix MenuWarp" id="tree"> 
                    <%=FunctionMenu_tree %>
                    <%=system_tree %>
                </ul>
            </div>
        </div>
        <div position="center" id="framecenter">
            <div tabid="home" title="首页" style="height: 300px;">
                <iframe frameborder="0" name="home" id="home" src="/Account/home.html"></iframe>
            </div>
        </div>
     
    </div>
    <div style="height: 32px; line-height: 32px; text-align: center;" id="divSupport">
         Copyright  2016 杭州景好网络科技有限公司
    </div>
    <div style="display: none"></div>
    <script src="/NewUI/blue/js/main.js"></script>
 
</body>
</html>
