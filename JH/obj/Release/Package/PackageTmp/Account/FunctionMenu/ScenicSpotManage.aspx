<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="ScenicSpotManage.aspx.cs" Inherits="JH.Account.FunctionMenu.ScenicSpotManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Scripts/jquery.upload.js"></script>
    <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/ScenicSpotManage.ashx";
        var manager;
        var managerM;
        $(function () {
            manager = $("#maingrid").ligerGrid({
                columns: [
                    {
                        display: '展示图片', name: 'showsImg', align: 'center', width: 150, render: function (rowdata, rowindex, value) {
                            var h = "";
                            h += "<img src=\"https://cloud.jhlxw.com/app" + value + "\" style=\"width:150px;\"/>";
                            return h;
                        }
                    },
                { display: '景区名称', name: 'MapName', align: 'left', width: 120 },
                { display: '所在城市', name: 'city', align: 'left', width: 100 },
                { display: '级别', name: 'level', align: 'left', width: 80 },
                {
                    display: '支付类型', name: 'Play_type', align: 'left', width: 150, render: function (rowdata, rowindex, value) {
                        var h = "";
                        if (value == "1")
                        { h = "微信支付"; }
                        else if (value == "2") {
                            h = "激活码支付";
                        } else {
                            h = "微信，激活码支付";
                        }
                        return h;
                    }
                },
                { display: '费用', name: 'Monetary', align: 'center', width: 100 },
                { display: '收听数', name: 'Play_number', align: 'center', width: 60 },
                { display: '中心坐标', name: 'CoreCoordinate', align: 'left', width: 200 },
                { display: '景区地图地址', name: 'MapImageUrl', align: 'left', width: 200 },
                { display: '坐标起始位置', name: 'beginCoordinate', align: 'left', width: 200 },
                { display: '坐标结束位置', name: 'endCoordinate', align: 'left', width: 200 },
                { display: '地图默认缩放等级', name: 'MapZoom', align: 'left', width: 120 },
                { display: '地图缩放范围', name: 'MapZoomRange', align: 'left', width: 120 }
                ], isScroll: false,
                url: url,
                urlParms: { p: "list", r: Math.random() },
                parms: {
                    "nickname": $("#txtSearch").val()
                },
                method: "post",
                usePager: true,
                enabledSort: true,
                checkbox: true,
                rownumbers: true,
                colDraggable: true,
                onDragCol: function (node) {
                    alert(node);
                }
            });

            $("#search").click(function () {
                $.ligerDialog.open({ target: $("#_search"), title: "搜索", width: 300, showMax: true, showToggle: true, height: 400 });
            });
            //查询
            $("#btnSearch").click(function () {
                $.ligerDialog.hide();
                var name = $("#txtSearch").val();
                manager.loadServerData({ "nickname": name });
            });
            $("#add").click(function () {
                empty();
                $.ligerDialog.open({ target: $("#addScenic"), title: "新增", width: 500, showMax: true, showToggle: true, height: 400 });
            });
            //设置景点
            $("#SetMarkers").click(function () {
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择行再点【设置景点】！"); return false;
                }
                if (row.length > 1) {
                    ErrorInfo("只能选择一条信息进行操作，请重新选择！"); return false;
                }
                $("#MarkersList").show();
                $("#ScenicID").val(manager.getSelectedRow().id);
                managerM = $("#maingridM").ligerGrid({
                    columns: [
                    { display: '景点名称', name: 'viweName', align: 'left', width: 120 },
                    {
                        display: '景点类型', name: 'icon', align: 'left', width: 120, render: function (rowdata, rowindex, value) {
                            var h = "";
                            if (value == "1")
                            { h = "卫生间"; }
                            else if (value == "2") {
                                h = "酒店";
                            } else {
                                h = "景点";
                            }
                            return h;
                        }
                    },
                    { display: '景点描述', name: 'introduction', align: 'left', width: 120 },
                    { display: '音频地址', name: 'audioUrl', align: 'left', width: 200 },
                    {
                        display: '图标', name: 'zicon', align: 'left', width: 200, render: function (rowdata, rowindex, value) {
                            var h = "<img src=\"" + value + "\" style='width:50px; height:50px;' />";
                            return h;
                        }
                    },
                    {
                        display: '景点图标', name: 'viweImgUrl', align: 'left', width: 200, render: function (rowdata, rowindex, value) {
                            var h = "<img src=\"https://cloud.jhlxw.com/app" + value + "\" style='width:50px; height:50px;' />";
                            return h;
                        }
                    },
                    { display: '景点坐标', name: 'position', align: 'left', width: 200 }
                    ], isScroll: false,
                    url: url,
                    urlParms: { p: "MarkersList", r: Math.random() },
                    parms: {
                        id: manager.getSelectedRow().id
                    },
                    method: "post",
                    usePager: true,
                    enabledSort: true,
                    checkbox: true,
                    rownumbers: true,
                    colDraggable: true,
                    onDragCol: function (node) {
                        alert(node);
                    }
                });
            });
            //编辑景区
            $("#edit").click(function () {
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择行再点编辑！"); return false;
                }
                if (row.length > 1) {
                    ErrorInfo("只能选择一条信息进行编辑，请重新选择！"); return false;
                }
                else {
                    $.get(url, { p: "get", r: Math.random(), id: manager.getSelectedRow().id }, function (data) {
                        $("#MapName").val(data.MapName);
                        $("#MapImageUrl").val(data.MapImageUrl);
                        $("#beginCoordinate").val(data.beginCoordinate);
                        $("#endCoordinate").val(data.endCoordinate);
                        $("#MapZoom").val(data.MapZoom);
                        $("#MapZoomRange").val(data.MapZoomRange);
                        $("#CoreCoordinate").val(data.CoreCoordinate);
                        $("#Remarks").val(data.Remarks);
                        $("#Monetary").val(data.Monetary);
                         
                        $("#level").val(data.level);
                        $("#Play_type").val(data.Play_type);
                        $("#openTime").val(data.openTime);
                        $("#address").val(data.address);
                        $("#tel").val(data.tel);
                        $("#Play_number").val(data.Play_number);
                        $("#to_cn").val(data.sheng);
                        set_city(document.getElementById("to_cn"), document.getElementById('city'));
                        $("#city").val(data.city.substring(0, data.city.length - 1));
                        if (data.Recommend == "1")
                            $("#Recommend").prop("checked", true);
                        else
                            $("#Recommend").prop("checked", false);
                        $("#id").val(data.id);
                        $.ligerDialog.open({ target: $("#addScenic"), title: "编辑", width: 500, showMax: true, showToggle: true, height: 400 });
                    });
                }
            });

            $("#ScenicSave").click(function () {
                var MapName = $("#MapName").val();
                var MapImageUrl = $("#MapImageUrl").val();
                var beginCoordinate = $("#beginCoordinate").val();
                var endCoordinate = $("#endCoordinate").val();
                var MapZoom = $("#MapZoom").val();
                var MapZoomRange = $("#MapZoomRange").val();
                var CoreCoordinate = $("#CoreCoordinate").val();
                var city = $("#city").val() + "市";
                var sheng = $("#to_cn").val();
                var id = $("#id").val();
                var Monetary = $("#Monetary").val();
               
                var level = $("#level").val();
                var Play_type = $("#Play_type").val();

                var openTime = $("#openTime").val();
                var address = $("#address").val();
                var tel = $("#tel").val();
                var Play_number = $("#Play_number").val();
                var Recommend = "";
                if ($("#Recommend").is(":checked") == true) {
                    Recommend = "1";
                } else {
                    Recommend = "0";
                }
                var Remarks = $("#Remarks").val();
                if (MapName == "") {
                    ErrorInfo("请填写景区名称！");
                    return false;
                }
                if (CoreCoordinate == "") {
                    ErrorInfo("中心坐标不能为空！");
                    return false;
                }
                if (beginCoordinate == "") {
                    ErrorInfo("坐标起始位置不能为空！");
                    return false;
                }
                if (MapZoom == "") {
                    ErrorInfo('地图默认缩放等级不能为空！');
                    return false;
                }
                if (MapZoomRange == "") {
                    ErrorInfo('地图缩放范围不能为空！');
                    return false;
                }
                if (city == "0") {
                    ErrorInfo("请选择所属城市！");
                    return false;
                }
                var data = {
                    p: 'ScenicSave',
                    MapName: MapName,
                    MapImageUrl: MapImageUrl,
                    beginCoordinate: beginCoordinate,
                    endCoordinate: endCoordinate,
                    MapZoom: MapZoom,
                    MapZoomRange: MapZoomRange,
                    CoreCoordinate: CoreCoordinate,
                    Recommend: Recommend,
                    Remarks: Remarks,
                    Monetary: Monetary,
                    
                    level: level,
                    Play_type: Play_type,
                    openTime: openTime,
                    address: address,
                    tel: tel,
                    Play_number: Play_number,
                    sheng: sheng,
                    city: city,
                    id: id,
                    r: Math.random()
                }
                EM_AJAX(url, data, "ScenicSave", function () {
                    empty();
                    manager.reload();
                });

            });

            ///生成地图
            $("#generate").click(function () {
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择景区！"); return false;
                }
                if (row.length != 0) {
                    var IDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        IDArror += row[i].id;
                        if ((row.length - i) > 1)
                            IDArror += ",";
                    }
                    var data = {
                        p: "generate",
                        IDArror: IDArror,
                        r: Math.random()
                    }
                    EM_AJAX(url, data, "generate", function () {
                        manager.reload();
                    });
                }
            });
            $("#delete").click(function () {
                var row = manager.getSelectedRows();
                if (row.length != 0) {
                    var IDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        IDArror += row[i].id;
                        if ((row.length - i) > 1)
                            IDArror += ",";
                    }
                    if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                       if (e) {
                            var data = { p: "delete", IDArror: IDArror, r: Math.random() }
                            $.get(url, data, function () {
                                manager.reload();
                                managerM.reload();
                    });
                    }
                    }));
                }
                else {
                    $.ligerDialog.error("请选择行数据再点删除！"); return false;
                }
            });


            $("#deleteM").click(function () {
                var row = managerM.getSelectedRows();
                if (row.length != 0) {
                    var IDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        IDArror += row[i].id;
                        if ((row.length - i) > 1)
                            IDArror += ",";
                    }

                    if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                       if (e) {
                            var data = { p: "deleteM", IDArror: IDArror, r: Math.random() }
                            $.get(url, data, function () {
                                managerM.reload();
                    });
                    }
                    }));
                }
                else {
                    $.ligerDialog.error("请选择行数据再点删除！"); return false;
                }
            });

            
        });

        var empty = function () {
            $("#addScenic input").val("");
            $("#addScenic textarea").val("");
            $("#addScenic select").val("-1");
            $("#addScenic input[type='checkbox']").each(function () {
                $(this).prop("checked", false);
            });
        }

        var start_Play = function (val) {
            var row = manager.getSelectedRows();
            if (row.length == 0) {
                $.ligerDialog.error("请选择行！"); return false;
            }
            if (row.length > 1) {
                ErrorInfo("只能选择一条信息进行操作，请重新选择！"); return false;
            }

            $.upload({
                url: url,
                fileName: 'imgdata',
                params: { p: 'imgUpload', id: manager.getSelectedRow().id, type: val },
                dataType: 'json',
                onSend: function () {
                    return true;
                },
                onComplate: function (data) {
                    if (data.info == "ok") {
                        AlertInfo("上传成功！");
                    } else {
                        ErrorInfo(data.info);
                    }
                    $.ligerDialog.closeWaitting();
                    manager.reload();
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">景区信息列表
                </span>
                <div class="query">
                    <button class="btn btn-success" id="search">搜索</button>
                </div>
            </div>
            <div class="itemInner" id="">
                <div style="padding-bottom: 5px;">
                    <table>
                        <tr>
                            <td style="padding-left: 5px;">
                                <button class="btn btn-primary" id="add">新增</button>
                                <button class="btn btn-info" id="edit">编辑</button>
                                <button class="btn btn-default" id="SetMarkers">设置景点</button>
                                <input type="hidden" id="ScenicID" />
                                <button class="btn btn-danger" id="delete">删除</button>
                                <button class="btn btn-default" id="generate">生成地图</button>
                                <button class="btn btn-info" onclick="start_Play(0);">展示图</button>
                                <button class="btn btn-info" onclick="start_Play(1);">初始音频</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="maingrid"></div>
            </div>

        </div>
    </div>
    <div class="content">
        <!--搜索-->
        <div id="_search" style="display: none;">
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: right; height: 34px;">关键字：
                    </td>
                    <td style="text-align: left;">
                        <input type="text" class="l-text" id="txtSearch" placeholder="请输入关键字" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; height: 34px;"></td>
                    <td>
                        <button class="btn btn-success" id="btnSearch">搜索</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!--新增-->
    <div id="addScenic" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right; height: 40px; width: 30%;">景区名称：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="MapName" style="width: 230px;" placeholder="请输入进去名称" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">费用：
                </td>
                <td>
                    <input type="text" class="l-text" id="Monetary" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">级别：
                </td>
                <td>
                    <input type="text" class="l-text" id="level" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">支付方式：
                </td>
                <td>
                    <select id="Play_type">
                        <option value="0">所有</option>
                        <option value="1">微信支付</option>
                        <option value="2">激活码支付</option>
                    </select>
                </td>
            </tr>

            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">景区开放时间：
                </td>
                <td>
                    <input type="text" class="l-text" id="openTime" style="width: 200px;" />
                </td>
            </tr>

            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">电话号码：
                </td>
                <td>
                    <input type="text" class="l-text" id="tel" style="width: 200px;" />
                </td>
            </tr>

            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">具体地址：
                </td>
                <td>
                    <input type="text" class="l-text" id="address" style="width: 200px;" />
                </td>
            </tr>

            <tr>
                <td style="height: 40px; width: 30%; text-align: right;">收听数量：
                </td>
                <td>
                    <input type="text" class="l-text" id="Play_number" style="width: 200px;" />
                </td>
            </tr>

            <tr>
                <td style="text-align: right; height: 40px; width: 30%;">中心坐标：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="CoreCoordinate" style="width: 230px;" placeholder="请输入中心坐标" />
                </td>
            </tr>
         

            <tr>
                <td style="text-align: right; height: 40px;">景区图片地址：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="MapImageUrl" placeholder="图片地址" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 40px;">坐标起始位置：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="beginCoordinate" style="width: 230px;" placeholder="请输入坐标起始位置" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 40px;">坐标结束位置：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="endCoordinate" style="width: 230px;" placeholder="请输入坐标结束位置" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 40px;">地图默认缩放等级：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="MapZoom" placeholder="请输入地图默认缩放等级" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 35px;">地图缩放范围：
                </td>
                <td style="text-align: left;">
                    <input type="text" class="l-text" id="MapZoomRange" placeholder="请输入地图缩放范围" />
                </td>
            </tr>

            <tr>
                <td style="text-align: right; height: 35px;">景区摘要：
                </td>
                <td style="text-align: left;">
                    <textarea style="width: 300px; height: 80px;" class="l-text" id="Remarks" placeholder="请填写景区摘要"></textarea>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 35px;">是否推荐首页：
                </td>
                <td style="text-align: left;">
                    <input type="checkbox" id="Recommend" />是/否
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 35px;">所在城市：
                </td>
                <td style="text-align: left;">
                    <select name="sheng" id="to_cn" onchange="set_city(this, document.getElementById('city'));" class="login_text_input">

                        <option value="0">请选择</option>

                        <option value="台湾">台湾</option>

                        <option value="马来西亚">马来西亚</option>

                        <option value="北京">北京</option>

                        <option value="上海">上海</option>

                        <option value="天津">天津</option>

                        <option value="重庆">重庆</option>

                        <option value="河北省">河北省</option>

                        <option value="山西省">山西省</option>

                        <option value="辽宁省">辽宁省</option>

                        <option value="吉林省">吉林省</option>

                        <option value="黑龙江省">黑龙江省</option>

                        <option value="江苏省">江苏省</option>

                        <option value="浙江省">浙江省</option>

                        <option value="安徽省">安徽省</option>

                        <option value="福建省">福建省</option>

                        <option value="江西省">江西省</option>

                        <option value="山东省">山东省</option>

                        <option value="河南省">河南省</option>

                        <option value="湖北省">湖北省</option>

                        <option value="湖南省">湖南省</option>

                        <option value="广东省">广东省</option>

                        <option value="海南省">海南省</option>

                        <option value="四川省">四川省</option>

                        <option value="贵州省">贵州省</option>

                        <option value="云南省">云南省</option>

                        <option value="陕西省">陕西省</option>

                        <option value="甘肃省">甘肃省</option>

                        <option value="青海省">青海省</option>

                        <option value="内蒙古">内蒙古</option>

                        <option value="广西">广西</option>

                        <option value="西藏">西藏</option>

                        <option value="宁夏">宁夏</option>

                        <option value="新疆">新疆</option>

                        <option value="香港">香港</option>

                        <option value="澳门">澳门</option>
                    </select>
                    - 市 
                    <select id="city" class="" name="shi">
                        <option value="0">请选择</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; height: 35px;"></td>
                <td style="text-align: left;">
                    <button class="btn btn-primary" id="ScenicSave">提交保存</button>
                    <input type="hidden" id="id" />
                </td>
            </tr>
        </table>
    </div>

    <!--景点列表-->
    <div id="MarkersList" style="display: none;">
        <div class="content">
            <div class="item">
                <div class="itemTitle">
                    <span class="x_title">景区景点列表
                    </span>
                    <div class="query">
                    </div>
                </div>
                <div class="itemInner" id="">
                    <div style="padding-bottom: 5px;">
                        <table>
                            <tr>
                                <td style="padding-left: 5px;">
                                    <button class="btn btn-primary" id="addM" onclick="addMarkers();">新增景点</button>
                                    <button class="btn btn-info" id="editM" onclick="editM();">编辑景点</button>
                                    <button class="btn btn-danger" id="deleteM">删除景点</button>
                                    <button class="btn btn-primary" onclick="uploadFile();">上传音频</button>
                                    <button class="btn btn-default" onclick="detailed();">填写景点介绍</button>
                                    <button class="btn btn-danger" onclick="upimg('0');">上传图标</button>
                                    <button class="btn btn-danger" onclick="upimg('1');">上传地图图标</button>
                                    <script type="text/javascript">
                                        function addMarkers() {
                                            $("#addMarkers input").val("");
                                            $("#addMarkers textarea").val("");
                                            $.ligerDialog.open({ target: $("#addMarkers"), title: "添加景点", width: 600, showMax: true, showToggle: true, height: 400 });
                                        }
                                        function editM() {
                                            var row = managerM.getSelectedRows();
                                            if (row.length == 0) {
                                                $.ligerDialog.error("请选择行再点编辑！"); return false;
                                            }
                                            if (row.length > 1) {
                                                ErrorInfo("只能选择一条信息进行编辑，请重新选择！"); return false;
                                            }
                                            else {

                                                $.get(url, { p: "getM", r: Math.random(), id: managerM.getSelectedRow().id }, function (data) {
                                                    $("#viweName").val(data.viweName);
                                                    $("#detailsURL").val(data.detailsURL);
                                                    $("#position").val(data.position);
                                                    $("#distance").val(data.distance);
                                                    $("#area").val(data.area);
                                                    $("#introduction").val(data.introduction);
                                                    $("#mid").val(data.id);
                                                    $("#icon").val(data.icon);
                                                    $.ligerDialog.open({ target: $("#addMarkers"), title: "编辑景点", width: 500, showMax: true, showToggle: true, height: 400 });
                                                });
                                            }
                                        }
                                        ///音频上传
                                        function uploadFile() {
                                            var row = managerM.getSelectedRows();
                                            if (row.length == 0) {
                                                $.ligerDialog.error("请选择景点！"); return false;
                                            }
                                            if (row.length > 1) {
                                                ErrorInfo("只能选择一条信息进行上传！"); return false;
                                            }
                                            $("#upid").val(managerM.getSelectedRow().id);
                                            $.ligerDialog.open({ target: $("#upmp3"), title: "上传音频", width: 500, showMax: true, showToggle: true, height: 250 });
                                        }
                                        ///上传图标
                                        function upimg(val) {
                                            var row = managerM.getSelectedRows();
                                            if (row.length == 0) {
                                                $.ligerDialog.error("请选择景点！"); return false;
                                            }
                                            if (row.length > 1) {
                                                ErrorInfo("只能选择一条信息进行上传！"); return false;
                                            }
                                            $("#zicon").val(val);
                                            $("#upimgid").val(managerM.getSelectedRow().id);
                                            $.ligerDialog.open({ target: $("#upimg"), title: "上传图标", width: 500, showMax: true, showToggle: true, height: 250 });
                                        }
                                        ///景点介绍
                                        function detailed() {
                                            var row = managerM.getSelectedRows();
                                            if (row.length == 0) {
                                                $.ligerDialog.error("请选择景点！"); return false;
                                            }
                                            if (row.length > 1) {
                                                ErrorInfo("只能选择一条信息进行操作！"); return false;
                                            }
                                            $.ligerDialog.open({ url: 'ScenicDetailed.aspx?id=' + managerM.getSelectedRow().id, height: '500', width: '700', isResize: true });
                                        }
                                    </script>
                                </td>
                            </tr>
                        </table>
                    </div>
    <div id="maingridM"></div>
    </div>

            </div>
        </div>
    </div>
    <!--添加景点-->
    <div id="addMarkers" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="height: 40px; width: 30%;">景点名称：
                </td>
                <td>
                    <input type="text" class="l-text" id="viweName" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;">请选择景点类型：
                </td>
                <td>
                    <select id="icon">
                        <option value="0">默认景点</option>
                        <option value="1">卫生间</option>
                        <option value="2">酒店</option>
                    </select>
                </td>
            </tr>
            <tr style="display: none;">
                <td style="height: 40px; width: 30%;">详细页面地址：
                </td>
                <td>
                    <input type="text" class="l-text" id="detailsURL" style="width: 300px;" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;">景点坐标：
                </td>
                <td>
                    <input type="text" class="l-text" id="position" style="width: 200px;" />
                </td>
            </tr>

            <tr style="display: none;">
                <td style="height: 110px; width: 30%;">景点范围坐标：
                </td>
                <td>
                    <textarea style="height: 150px; width: 300px;" id="area" class="l-text"></textarea>
                </td>
            </tr>

            <tr>
                <td style="height: 45px; width: 30%;">感应距离(米)：
                </td>
                <td>
                    <input type="text" class="l-text" id="distance" style="width: 100px;" />
                </td>
            </tr>

            <tr>
                <td style="height: 110px; width: 30%;">景点摘要：
                </td>
                <td>
                    <textarea style="height: 100px; width: 300px;" id="introduction" class="l-text"></textarea>
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;"></td>
                <td>
                    <button class="btn btn-info" type="button" onclick="MarkersSave();">提交保存</button>
                    <input type="hidden" id="mid" />
                    <script type="text/javascript">
                        function MarkersSave() {
                            var viweName = $("#viweName").val();
                            var detailsURL = $("#detailsURL").val();
                            var position = $("#position").val();
                            var distance = $("#distance").val();
                            var area = $("#area").val();
                            var introduction = $("#introduction").val();
                            var id = $("#mid").val();
                            var ScenicID = $("#ScenicID").val();
                            var icon = $("#icon").val();
                            var data = {
                                p: "MarkersSave",
                                id: id,
                                viweName: viweName,
                                detailsURL: detailsURL,
                                position: position,
                                distance: distance,
                                area: area,
                                introduction: introduction,
                                ScenicID: ScenicID,
                                icon: icon,
                                r: Math.random()
                            }
                            EM_AJAX(url, data, "MarkersSave", function () {
                                $("#addMarkers input").val("");
                                $("#addMarkers textarea").val("");
                                managerM.reload();
                            });
                        }
                    </script>
                </td>
            </tr>
        </table>
    </div>
    <!--上传图标-->
    <div id="upimg" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="height: 40px; width: 30%;">选择文件：
                </td>
                <td>
                    <button type="button" onclick="Savefile1();">选择文件</button>
                    <input type="hidden" id="upimgid" />
                    <input type="hidden" id="zicon" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;"></td>
                <td>

                    <script type="text/javascript">
                        var Savefile1 = function () {
                            $.upload({
                                url: url,
                                fileName: 'filedata',
                                params: { p: 'fileUploadimg', id: $("#upimgid").val(), zicon: $("#zicon").val() },
                                dataType: 'json',
                                onSend: function () {
                                    return true;
                                },
                                onComplate: function (data) {
                                    if (data.info == "ok") {
                                        AlertInfo("上传成功！");
                                    } else {
                                        ErrorInfo(data.info);
                                    }
                                    $.ligerDialog.closeWaitting();
                                    managerM.reload();
                                }
                            });
                        }
                    </script>
                </td>
            </tr>
        </table>
    </div>
    <!--上传音频-->
    <div id="upmp3" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="height: 40px; width: 30%;">只支持MP3格式的文件
                </td>
                <td>
                    <div id="filename">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;">选择文件：
                </td>
                <td>
                    <button type="button" onclick="Savefile();">选择文件</button>
                    <input type="hidden" id="upid" />
                </td>
            </tr>
            <tr>
                <td style="height: 40px; width: 30%;"></td>
                <td>

                    <script type="text/javascript">
                        var Savefile = function () {
                            $.upload({
                                url: url,
                                fileName: 'filedata',
                                params: { p: 'fileUpload', id: $("#upid").val() },
                                dataType: 'json',
                                onSend: function () {
                                    return true;
                                },
                                onComplate: function (data) {
                                    if (data.info == "ok") {
                                        AlertInfo("上传成功！");
                                    } else {
                                        ErrorInfo(data.info);
                                    }
                                    $.ligerDialog.closeWaitting();
                                    managerM.reload();
                                }
                            });
                        }
                    </script>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        var cities = new Object();

        cities['台湾'] = new Array('台北', '台南', '其他');

        cities['马来西亚'] = new Array('Malaysia');

        cities['北京'] = new Array('北京');

        cities['上海'] = new Array('上海');

        cities['天津'] = new Array('天津');

        cities['重庆'] = new Array('重庆');

        cities['河北省'] = new Array('石家庄', '张家口', '承德', '秦皇岛', '唐山', '廊坊', '保定', '沧州', '衡水', '邢台', '邯郸');

        cities['山西省'] = new Array('太原', '大同', '朔州', '阳泉', '长治', '晋城', '忻州', '吕梁', '晋中', '临汾', '运城');

        cities['辽宁省'] = new Array('沈阳', '朝阳', '阜新', '铁岭', '抚顺', '本溪', '辽阳', '鞍山', '丹东', '大连', '营口', '盘锦', '锦州', '葫芦岛');

        cities['吉林省'] = new Array('长春', '白城', '松原', '吉林', '四平', '辽源', '通化', '白山', '延边');

        cities['黑龙江省'] = new Array('哈尔滨', '齐齐哈尔', '黑河', '大庆', '伊春', '鹤岗', '佳木斯', '双鸭山', '七台河', '鸡西', '牡丹江', '绥化', '大兴安');

        cities['江苏省'] = new Array('南京', '徐州', '连云港', '宿迁', '淮阴', '盐城', '扬州', '泰州', '南通', '镇江', '常州', '无锡', '苏州');

        cities['浙江省'] = new Array('杭州', '湖州', '嘉兴', '舟山', '宁波', '绍兴', '金华', '台州', '温州', '丽水');

        cities['安徽省'] = new Array('合肥', '宿州', '淮北', '阜阳', '蚌埠', '淮南', '滁州', '马鞍山', '芜湖', '铜陵', '安庆', '黄山', '六安', '巢湖', '池州', '宣城');

        cities['福建省'] = new Array('福州', '南平', '三明', '莆田', '泉州', '厦门', '漳州', '龙岩', '宁德');

        cities['江西省'] = new Array('南昌', '九江', '景德镇', '鹰潭', '新余', '萍乡', '赣州', '上饶', '抚州', '宜春', '吉安');

        cities['山东省'] = new Array('济南', '聊城', '德州', '东营', '淄博', '潍坊', '烟台', '威海', '青岛', '日照', '临沂', '枣庄', '济宁', '泰安', '莱芜', '滨州', '菏泽');

        cities['河南省'] = new Array('郑州', '三门峡', '洛阳', '焦作', '新乡', '鹤壁', '安阳', '濮阳', '开封', '商丘', '许昌', '漯河', '平顶山', '南阳', '信阳', '周口', '驻马店');

        cities['湖北省'] = new Array('武汉', '十堰', '襄攀', '荆门', '孝感', '黄冈', '鄂州', '黄石', '咸宁', '荆州', '宜昌', '恩施', '襄樊');

        cities['湖南省'] = new Array('长沙', '张家界', '常德', '益阳', '岳阳', '株洲', '湘潭', '衡阳', '郴州', '永州', '邵阳', '怀化', '娄底', '湘西');

        cities['广东省'] = new Array('广州', '清远', '韶关', '河源', '梅州', '潮州', '汕头', '揭阳', '汕尾', '惠州', '东莞', '深圳', '珠海', '江门', '佛山', '肇庆', '云浮', '阳江', '茂名', '湛江');

        cities['海南省'] = new Array('海口', '三亚');

        cities['四川省'] = new Array('成都', '广元', '绵阳', '德阳', '南充', '广安', '遂宁', '内江', '乐山', '自贡', '泸州', '宜宾', '攀枝花', '巴中', '达川', '资阳', '眉山', '雅安', '阿坝', '甘孜', '凉山');

        cities['贵州省'] = new Array('贵阳', '六盘水', '遵义', '毕节', '铜仁', '安顺', '黔东南', '黔南', '黔西南');

        cities['云南省'] = new Array('昆明', '曲靖', '玉溪', '丽江', '昭通', '思茅', '临沧', '保山', '德宏', '怒江', '迪庆', '大理', '楚雄', '红河', '文山', '西双版纳');

        cities['陕西省'] = new Array('西安', '延安', '铜川', '渭南', '咸阳', '宝鸡', '汉中', '榆林', '商洛', '安康');

        cities['甘肃省'] = new Array('兰州', '嘉峪关', '金昌', '白银', '天水', '酒泉', '张掖', '武威', '庆阳', '平凉', '定西', '陇南', '临夏', '甘南');

        cities['青海省'] = new Array('西宁', '海东', '西宁', '海北', '海南', '黄南', '果洛', '玉树', '海西');

        cities['内蒙古'] = new Array('呼和浩特', '包头', '乌海', '赤峰', '呼伦贝尔盟', '兴安盟', '哲里木盟', '锡林郭勒盟', '乌兰察布盟', '鄂尔多斯', '巴彦淖尔盟', '阿拉善盟');

        cities['广西'] = new Array('南宁', '桂林', '柳州', '梧州', '贵港', '玉林', '钦州', '北海', '防城港', '南宁', '百色', '河池', '柳州', '贺州');

        cities['西藏'] = new Array('拉萨', '那曲', '昌都', '林芝', '山南', '日喀则', '阿里');

        cities['宁夏'] = new Array('银川', '石嘴山', '吴忠', '固原');

        cities['新疆'] = new Array('乌鲁木齐', '克拉玛依', '喀什', '阿克苏', '和田', '吐鲁番', '哈密', '博尔塔拉', '昌吉', '巴音郭楞', '伊犁', '塔城', '阿勒泰');

        cities['香港'] = new Array('香港');

        cities['澳门'] = new Array('澳门');

        function set_city(province, city) {
            var pv, cv;
            var i, ii;
            pv = province.value;
            cv = city.value;
            city.length = 1;
            if (pv == '0') return;
            if (typeof (cities[pv]) == 'undefined') return;
            for (i = 0; i < cities[pv].length; i++) {
                ii = i + 1;
                city.options[ii] = new Option();
                city.options[ii].text = cities[pv][i];
                city.options[ii].value = cities[pv][i];
            }
        }
    </script>
</asp:Content>
