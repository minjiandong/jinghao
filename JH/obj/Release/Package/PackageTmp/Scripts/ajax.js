//ScriptWeb/ajax全局变量
$.ajaxSetup({
    cache: false,
    contentType: "application/x-www-form-urlencoded; charset=utf-8",
    type: "POST",
    dataType: 'json'
});
/*正常消息*/
var AlertInfo = function (msg) {
    $.ligerDialog.success(msg); 
}
/*错误消息*/
var ErrorInfo = function (msg) {
    $.ligerDialog.error(msg);
}
///自定义AJAX操作方法
///URL 请求的地址
///data 传递的参数
///button 点击提交的按钮ID
var EM_AJAX = function (url, data, button, callback) {
    $.ajax({
        url: url,
        data: data,
        error: function () {
            ErrorInfo("系统错误！");
        },
        success: function (data) {
            if (data.info == "ok") {
                AlertInfo("操作成功！");
                callback();
            } else {
                ErrorInfo(data.info);
            }
        }, beforeSend: function () {
            $("#"+button).button('loading');
        }, complete: function () {
            $("#" + button).button('reset');
        }
    });
}

var EM = function (url, data, button, callback) {
    $.ajax({
        url: url,
        data: data,
        error: function () {
            ErrorInfo("系统错误！");
        },
        success: function (data) {
            if (data.type == "ok") {
                AlertInfo(data.info);
                callback();
            } else {
                ErrorInfo(data.info);
            }
        }, beforeSend: function () {
            $("#" + button).button('loading');
        }, complete: function () {
            $("#" + button).button('reset');
        }
    });
}


var EMAJAX = function (url, data, button, callback) {
    $.ajax({
        url: url,
        data: data,
        error: function () {
            ErrorInfo("系统错误！");
        },
        success: function (data) {
            AlertInfo(data.info);
            callback();
        }, beforeSend: function () {
            $("#" + button).button('loading');
        }, complete: function () {
            $("#" + button).button('reset');
        }
    });
}

var _EMAJAX = function (url, data, button, callback) {
    $.ajax({
        url: url,
        data: data,
        error: function () {
            ErrorInfo("系统错误！");
        },
        success: function (data) {
            try {
                if (data.type == "error") {
                    ErrorInfo(data.info);
                }
            } catch (e) {}
            callback(data);
        }, beforeSend: function () {
            $("#" + button).button('loading');
        }, complete: function () {
            $("#" + button).button('reset');
        }
    });
}
///判断是否为空
var isEmpty = function (id) {
    if ($("#" + id).val() == "") {
        $('#'+id).popover('toggle');
        $('#'+id).focus();
        return false;
    } else {
        $('#' + id).popover('hide');
        return true;
    }
}

//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
//默认高亮显示当前菜单位置
$(function () {
    var F = getUrlParam("F");
    var Z = getUrlParam("Z");
    $("#" + F).attr("class", "sub-menu opened");
    $("#" + Z).attr("class", "current");
});

//格式化JSON格式
function format(txt, compress/*是否为压缩模式*/) {/* 格式化JSON源码(对象转换为JSON文本) */
    var indentChar = '    ';
    if (/^\s*$/.test(txt)) {
        alert('数据为空,无法格式化! ');
        return txt;
    }
    try { var data = eval('(' + txt + ')'); }
    catch (e) {
        alert('数据源语法错误,格式化失败! 错误信息: ' + e.description, 'err');
        return txt;
    };
    var draw = [], last = false, This = this, line = compress ? '' : '\n', nodeCount = 0, maxDepth = 0;

    var notify = function (name, value, isLast, indent/*缩进*/, formObj) {
        nodeCount++;/*节点计数*/
        for (var i = 0, tab = ''; i < indent; i++) tab += indentChar;/* 缩进HTML */
        tab = compress ? '' : tab;/*压缩模式忽略缩进*/
        maxDepth = ++indent;/*缩进递增并记录*/
        if (value && value.constructor == Array) {/*处理数组*/
            draw.push(tab + (formObj ? ('"' + name + '":') : '') + '[' + line);/*缩进'[' 然后换行*/
            for (var i = 0; i < value.length; i++)
                notify(i, value[i], i == value.length - 1, indent, false);
            draw.push(tab + ']' + (isLast ? line : (',' + line)));/*缩进']'换行,若非尾元素则添加逗号*/
        } else if (value && typeof value == 'object') {/*处理对象*/
            draw.push(tab + (formObj ? ('"' + name + '":') : '') + '{' + line);/*缩进'{' 然后换行*/
            var len = 0, i = 0;
            for (var key in value) len++;
            for (var key in value) notify(key, value[key], ++i == len, indent, true);
            draw.push(tab + '}' + (isLast ? line : (',' + line)));/*缩进'}'换行,若非尾元素则添加逗号*/
        } else {
            if (typeof value == 'string') value = '"' + value + '"';
            draw.push(tab + (formObj ? ('"' + name + '":') : '') + value + (isLast ? '' : ',') + line);
        };
    };
    var isLast = true, indent = 0;
    notify('', data, isLast, indent, false);
    return draw.join('');
}

//用正则表达式获取占位符
//返回占位符数组
var GetPlaceholder = function (str) {
    var reg = /\{.*?\}/g;
    var t = str.match(reg);
    return t;
}
//处理SQL语句，并把占位符转译
//Sql 输入的SQL语句
var handleSql = function (Sql) {
    var arraySql = GetPlaceholder(Sql);
    if (arraySql == null)
        return Sql;
    $.each(arraySql, function (i, e) {
        if (e.indexOf('-') > 0) {
            var tr = e.substring(1, e.length - 1).split('-');
            var key = "";
            var value = "";
            if (tr.length == 1) {
                key = tr[0];
                if (key == "YEAR_MONTH") {//年月201605
                    var date = new Date().Format("yyyyMM","");
                    Sql = Sql.replace(key, date);
                } else if (key == "MONTH_DAY") {//日期20160507
                    var date = new Date().Format("yyyyMMdd","");
                    Sql = Sql.replace(key, date);
                } else if (key == "DISTRICT_ID") {//区域

                }
            } else {
                key = tr[0];
                value = tr[1];
                if (key == "YEAR_MONTH") {
                    var date = new Date().Format("yyyyMM", "-" + value);
                    Sql = Sql.replace('{' + key + '-' + value + '}', date);
                } else if (key == "MONTH_DAY") {
                    var date = new Date().Format("yyyyMMdd", "-" + value);
                    Sql = Sql.replace('{' + key + '-' + value + '}', date);
                } else if (key == "DISTRICT_ID") {

                }
            }
        }else if (e.indexOf('+') > 0) {
            var tr = e.substring(1, e.length - 1).split('+');
            var key = "";
            var value = "";
            if (tr.length == 1) {
                key = tr[0];
                if (key == "YEAR_MONTH") {//年月201605
                    var date = new Date().Format("yyyyMM","");
                    Sql = Sql.replace(key, date);
                } else if (key == "MONTH_DAY") {//日期20160507
                    var date = new Date().Format("yyyyMMdd","");
                    Sql = Sql.replace(key, date);
                } else if (key == "DISTRICT_ID") {//区域

                }
            } else {
                key = tr[0];
                value = tr[1];
                if (key == "YEAR_MONTH") {
                    var date = new Date().Format("yyyyMM", "+" + value);
                    Sql = Sql.replace('{' + key + '+' + value + '}', date);
                } else if (key == "MONTH_DAY") {
                    var date = new Date().Format("yyyyMMdd", "+" + value);
                    Sql = Sql.replace('{' + key + '+' + value + '}', date);
                } else if (key == "DISTRICT_ID") {

                }
            }
        } else {
            key = e;
            if (key == "{YEAR_MONTH}") {//年月201605
                var date = new Date().Format("yyyyMM","");
                Sql = Sql.replace(key, date);
            } else if (key == "{MONTH_DAY}") {//日期20160507
                var date = new Date().Format("yyyyMMdd","");
                Sql = Sql.replace(key, date);
            } else if (key == "{DISTRICT_ID}") {//区域

            }
        }
    });
    return Sql;
}

///把加参数的占位符替换成没有参数的占位符
var th = function (Sql) {
    var arraySql = GetPlaceholder(Sql);
    if (arraySql == null)
        return Sql;
    $.each(arraySql, function (i, e) {
        if (e.indexOf('-') > 0) {
            var tr = e.substring(1, e.length - 1).split('-');
            var key = "";
            var value = "";
            key = tr[0];
            Sql = Sql.replace(e, "{" + key + "}");
        }
        if (e.indexOf('+') > 0) {
            var tr = e.substring(1, e.length - 1).split('+');
            var key = "";
            var value = "";
            key = tr[0];
            Sql = Sql.replace(e, "{" + key + "}");
        }
    });
    return Sql;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt,v) { //author: meizz 
    var o = {
        "M+": eval("this.getMonth() + 1" + v), //月份 
        "d+": eval("this.getDate()" + v), //日 
        "h+": eval("this.getHours()" + v), //小时 
        "m+": eval("this.getMinutes()" + v), //分 
        "s+": eval("this.getSeconds()" + v), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": eval("this.getMilliseconds()" + v) //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
///替换字符方法
///String 需要替换的字符串
///ReplaceKey 替换的关键字
///ReplaceValue 替换后的字符
var StrReplace = function (String,ReplaceKey,ReplaceValue) {
    var str = new RegExp("{" + ReplaceKey + "}", "g");
    return String.replace(str, ReplaceValue);
}
///去除空格和回车换行
///需要去除的字符串
var RemoveSpace = function (String) {
    var str = String.replace(/\ +/g, " ").replace(/[\r\n]/g, "").replace(/[ ]/g," ");
    return str;
}
//字符串转换成16进制
function stringToHex(str) {
    var val = "";
    for (var i = 0; i < str.length; i++) {
        if (val == "")
            val = str.charCodeAt(i).toString(16);
        else
            val += "," + str.charCodeAt(i).toString(16);
    }
    return val;
}

///页面不允许复制和右键的代码

//function disableselect(e) {
//    return false
//}
//function reEnable() {
//    return true
//}
//document.onselectstart = new Function("return false")
//if (window.sidebar) {
//    document.onmousedown = disableselect
//    document.onclick = reEnable
//}

//function MM_reloadPage(init) {    //reloads the window if Nav4 resized
//    if (init == true) with (navigator) {
//        if ((appName == "Netscape") && (parseInt(appVersion) == 4)) {
//            document.MM_pgW = innerWidth; document.MM_pgH = innerHeight; onresize = MM_reloadPage;
//        }
//    }
//    else if (innerWidth != document.MM_pgW || innerHeight != document.MM_pgH) location.reload();
//}
//MM_reloadPage(true);

//if (window.Event)
//    document.captureEvents(Event.MOUSEUP);
//function nocontextmenu() {
//    event.cancelBubble = true
//    event.returnValue = false;
//    return false;
//}
//function norightclick(e) {
//    if (window.Event) {
//        if (e.which == 2 || e.which == 3)
//            return false;
//    } else {
//        if (event.button == 2 || event.button == 3) {
//            event.cancelBubble = true
//            event.returnValue = false;
//            return false;
//        }
//    }
//}

//document.oncontextmenu = nocontextmenu; // for IE5+
//document.onmousedown = norightclick; // for all others


///页面加水印

function GetWaterMarked(targetObj, jpgUrl, targetStr) {
    var windowobj = targetObj;
    var waterMarkPicUrl = jpgUrl;
    var controlWindowStr = targetStr;
    if (windowobj.document.getElementById("waterMark") != null)
        return;
    var m = "waterMark";
    var newMark = windowobj.document.createElement("div");
    newMark.id = m;
    newMark.style.position = "absolute";
    newMark.style.zIndex = "9527";
    newMark.style.top = "0px";
    newMark.style.left = "0px";
    newMark.style.width = windowobj.document.body.clientWidth;
    if (parseInt(windowobj.document.body.scrollHeight) > parseInt(windowobj.document.body.clientHeight)) {
        newMark.style.height = windowobj.document.body.scrollHeight;
    } else {
        newMark.style.height = windowobj.document.body.clientHeight;
    }
    newMark.style.backgroundImage = "url(" + waterMarkPicUrl + ")";
    newMark.style.filter = "alpha(opacity=50)";
    windowobj.document.body.appendChild(newMark);
    var markStr = "var sobj =" + controlWindowStr + ".document.getElementById('waterMark');sobj.style.width =" + controlWindowStr + ".document.body.clientWidth;sobj.style.height =" + controlWindowStr + ".document.body.clientHeight;";
    if (windowobj.document.body.onresize != null) {
        var oldResiae = windowobj.document.body.onresize.toString();
        var oldResiaeStr = oldResiae.substr(oldResiae.indexOf("{") + 1);
        var oldResiaeStr = oldResiaeStr.substr(0, oldResiaeStr.lastIndexOf("}"));
        oldResiaeStr += ";" + markStr;
        windowobj.document.body.onresize = new Function(oldResiaeStr);
    }
    else {
        windowobj.document.body.onresize = new Function(markStr);
    }
}
///获取虚拟目录地址
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (postPath);
}

$(function () {
    var inputs = $("body select,input,button");
    $("#showJquery").hide();
    $("#showlog").hide();
    $("#download").hide();
    try {
        var userid = $.cookie('EM_GX_COOKIE');
        userid = userid.substring(8, userid.length);
        $.get(getRootPath() + "/Account/initialization/qx_" + userid + ".json", { p: "qx", r: Math.random() }, function (d) {
            $.each(inputs, function () {
                var id = $(this).attr("id");
                for (var i = 0; i < d.length; i++) {
                    if (d[i].id == id) {
                        $("#" + id).show();
                    }
                }
            });
        });
    } catch (e) {

    }
    
    
});


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