<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="functionManage.aspx.cs" Inherits="JH.Account.SysFile.functionManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        #ulIcon li {
            padding: 3px;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">

        var manager;
        var superior;
        var url = "/Account/ashx/SysAshx/functionManage.ashx";
        //var urlcity = "/Account/ashx/SysAshx/indexManage.ashx";
        $(function () {

            $("#btnSearch").click(function () {
                $.ligerDialog.hide();
                manager.loadServerData({
                    keyword: $("#keyword").val(),
                    menuType: $("#sMenuType").val(),
                    cityId: ""
                });
            });
            
            $("#ulIcon>li>i").click(function () {
                $("#icon").val($(this).attr('title'));
            });


            $.get(url, { p: "functionList", r: Math.random() }, function (dt) {
                superior=$("#SuperiorID").ligerComboBox({
                    width: 250,
                    slide: false,
                    selectBoxWidth: 450,
                    selectBoxHeight: 240,
                    valueField: 'FUNCTIONID',
                    textField: 'FUNCTIONNAME',
                    grid: dt,
                    condition: { fields: [{ name: 'FUNCTIONNAME', label: '功能名称', width: 120, type: 'text' }] }
                });
            });
            
            manager = $("#maingrid").ligerGrid({
                columns: [
                { display: '功能名称', name: 'FUNCTIONNAME', id: 'FUNCTIONNAME', align: 'left', width: 400, frozen: true },
                { display: '功能类型', name: 'FUNCTIONTYPE', align: 'left', width: 100 },
                //{ display: '所属地市ID', name: 'CITYID', align: 'center', width: 100 },
                {
                    display: '是否可用', name: 'isEnable'.toUpperCase(), align: 'left', width: 70, render: function (rowdata, rowindex, value) {
                        var h = "";
                        h += "<img src=\"/JJUI/images/" + rowdata['isEnable'.toUpperCase()] + ".png\"/>";
                        return h;
                    }
                },
                {
                    display: '是否展开', name: 'isOpen'.toUpperCase(), align: 'left', width: 70, render: function (rowdata, rowindex, value) {
                        var h = "";
                        h += "<img src=\"/JJUI/images/" + rowdata['isOpen'.toUpperCase()] + ".png\"/>";
                        return h;
                    }
                },
                { display: '标志', name: 'FunctionID'.toUpperCase(), align: 'left', width: 120 },
                //{ display: '父级标志', name: 'SuperiorID', align: 'left',width:80 },
                { display: '排序', name: 'Sort'.toUpperCase(), align: 'left', width: 50 },
                { display: '菜单类型', name: 'MenuType'.toUpperCase(), align: 'left', width: 80 },
                { display: '图标', name: 'icon'.toUpperCase(), align: 'left', width: 100 },
                { display: '代码', name: 'Code'.toUpperCase(), align: 'left', width: 300 },
                {
                    display: '操作', align: 'left', width: 80, render: function (rowdata, rowindex, value) {
                        var h = "";
                        h += "<a href=\"javascript:editFun('" + rowdata["FunctionID".toUpperCase()] + "')\">[修改]</a>";
                        return h;
                    }
                }
                ], isScroll: true,
                height: 400,
                url: url,
                urlParms: { p: "List", r: Math.random() },
                enabledSort: true,
                checkbox: true,
                rownumbers: true,
                usePager: false,
                alternatingRow: true,
                tree: {
                    columnId: 'FunctionName'.toUpperCase(),
                    idField: 'FunctionID'.toUpperCase(),
                    parentIDField: 'SuperiorID'.toUpperCase()
                }
            });
            //绑定地市
            //$.get(urlcity, { p: "citylist", r: Math.random() }, function (data) {
            //    var combo2 = $("#selectCityId").ligerComboBox({
            //        data: data,
            //        valueField: 'ID',
            //        textField: 'NAME',
            //        valueFieldID: 'value_mean',
            //        selectBoxWidth: 335,
            //        autocomplete: true,
            //        keySupport: true,
            //        width: 335
            //    });
            //    $("#sCityId").ligerComboBox({
            //        data: data,
            //        valueField: 'ID',
            //        textField: 'NAME',
            //        valueFieldID: 'value_mean',
            //        selectBoxWidth: 140,
            //        autocomplete: true,
            //        keySupport: true,
            //        width: 140
            //    });
            //});

            //新增
            $("#add").click(function () {
                $.ligerDialog.open({ target: $("#target"), title: "新增功能", width: 600, showMax: true, showToggle: true, height: 400 });
                location.href = "#divContent";
                empty();
            });

            $("#FunctionType").change(function () {
                var val = $(this);
                if (val.val() == "menu") {
                    $('#isOpenid').show();
                    $("#MenuTypeid").show();
                } else {
                    $('#isOpenid').hide();
                    $("#MenuTypeid").hide();
                    $("#selectCity").hide();
                    $("#selectCityId").ligerComboBox("setValue", null);
                }
            });
            $("#MenuType").change(function () {
                var val = $(this);
                if (val.val() == "FunctionMenu") {
                    $("#selectCity").show();
                }
                else {
                    $("#selectCity").hide();
                    $("#selectCityId").ligerComboBox("setValue", null);
                }
            });

            //提交保存
            $("#save").click(function () {
                var FunctionName = $("#FunctionName").val();
                var FunctionType = $("#FunctionType").val();
                var isEnable = "";
                if ($("#isEnable").is(":checked") == true) {
                    isEnable = "1";
                } else {
                    isEnable = "0";
                }
                var isOpen = "";
                if ($("#isOpen").is(":checked") == true) {
                    isOpen = "1";
                } else {
                    isOpen = "0";
                }
                var SuperiorID = $("#SuperiorID").val();
                var Sort = $("#Sort").val();
                var MenuType = $("#MenuType").val();
                var Code = $("#Code").val();
                var Remarks = $("#Remarks").val();
                var icon = $("#icon").val();
                var CityId = $("#selectCityId").ligerComboBox("getValue");
                if (FunctionName == "") {
                    ErrorInfo("功能名称不能为空！");
                    $("#FunctionName").focus();
                    return false;
                }
                if (FunctionType == "-1") {
                    ErrorInfo("请选择功能类型");
                    $("#FunctionType").focus();
                    return false;
                }
                if (Sort == "") {
                    ErrorInfo("请填写功能排序");
                    return false;
                }
                if (FunctionType == "menu") {
                    if (MenuType == "-1") {
                        ErrorInfo("请选择菜单类型");
                        $("#MenuType").focus();
                        return false;
                    }
                }
                if (MenuType == "FunctionMenu") {
                    if (CityId == "") {
                        ErrorInfo("请选择所属地市");
                        return false;
                    }
                }
                var FunctionID = $("#Hidden1").val();
                var data = {
                    p: "add",
                    FunctionID: FunctionID,
                    FunctionName: FunctionName,
                    FunctionType: FunctionType,
                    isEnable: isEnable,
                    isOpen: isOpen,
                    SuperiorID: SuperiorID,
                    Sort: Sort,
                    MenuType: MenuType,
                    Code: Code,
                    Remarks: Remarks,
                    icon: icon,
                    CityId: CityId,
                    r: Math.random()
                }
                EM_AJAX(url, data, "save", function () {
                    empty();
                    manager.loadServerData({
                        keyword: $("#keyword").val(),
                        menuType: $("#sMenuType").val(),
                        cityId: ""//$("#sCityId").ligerComboBox("getValue")
                    });
                    location.href = "#divContent";
                    $.ligerDialog.hide();
                });
            });

            $("#deletebutton").click(function () {
                if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                   if (e) {
                        var FunctionIDArror = "";
                        var row = manager.getSelectedRows();
                        if (row.length == 0) {
                            $.ligerDialog.error("请选择信息再点删除！"); return false;
                }

                        for (var i = 0; i < row.length; i++) {
                            FunctionIDArror += row[i].FUNCTIONID;
                            if ((row.length - i) > 1)
                                FunctionIDArror += ",";
                }

                        var data = {
                    p: "delete",
                    FunctionID: FunctionIDArror,
                    r: Math.random()
                }

                    EMAJAX(url, data, "deletebutton", function () {
                        manager.loadServerData({
                    keyword: $("#keyword").val(),
                    menuType: $("#sMenuType").val(),
                    cityId: ""//$("#sCityId").ligerComboBox("getValue")
                });
                });
                } else {
                        return false;
                }
                }));
            });

            $("#Search").click(function () {
                $.ligerDialog.open({ target: $("#_search"), title: "搜索", width: 300, showMax: true, showToggle: true, height: 400 });
            });
        });

        var empty = function () {
            $("#divContent input").val("");
            $("#divContent textarea").val("");
            $("#divContent select").val("-1");
            $("#divContent input[type='checkbox']").each(function () {
                $(this).prop("checked", false);
            });

        }



        var editFun = function (FunctionID) {
            $.ligerDialog.waitting("稍等程序加载中...");
            var data = {
                p: "getinfo",
                FunctionID: FunctionID,
                r: Math.random()
            }
            $.get(url, data, function (data) {
                $.ligerDialog.open({ target: $("#target"), title: "编辑功能", width: 600, showMax: true, showToggle: true, height: 400 });
                $("#Hidden1").val(data.FUNCTIONID);
                $("#FunctionName").val(data.FUNCTIONNAME);
                $("#FunctionType").val(data.FUNCTIONTYPE);
                if (data.ISENABLE == "1")
                    $("#isEnable").prop("checked", true);
                else
                    $("#isEnable").prop("checked", false);
                if (data.ISOPEN == "1")
                    $("#isOpen").prop("checked", true);
                else
                    $("#isOpen").prop("checked", false);
                $("#SuperiorID").val(data.SUPERIORID);
                superior.setValue(data.SUPERIORID);
                $("#Sort").val(data.SORT);
                $("#MenuType").val(data.MENUTYPE);
                $("#Code").val(data.CODE);
                $("#Remarks").val(data.REMARKS);
                $("#icon").val(data.ICON);
                //$("#selectCityId").ligerComboBox("setValue", data.CityID);
                var val = data.FUNCTIONTYPE;
                if (val == "menu") {
                    $('#isOpenid').show();
                    $("#MenuTypeid").show();
                } else {
                    $('#isOpenid').hide();
                    $("#MenuTypeid").hide();
                }
                var menu = data.MENUTYPE;
                if (menu == "FunctionMenu") {
                    $("#selectCity").show();
                }
                else {
                    $("#selectCity").hide();
                }

                location.href = "#divContent";
                $.ligerDialog.closeWaitting();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">功能列表管理
                </span>
                <div class="query">
                    <button class="btn btn-success" id="Search">搜索</button>
                </div>
            </div>
            <div class="itemInner">
                <div style="padding-bottom: 5px;">
                    <table>
                        <tr>
                            <td>
                                <button class="btn btn-success" id="add">新增</button>
                            </td>
                            <td>
                                <button class="btn btn-danger" id="deletebutton">删除</button>
                            </td>
                            <%--<td>
                                <script type="text/javascript">
                                    $(function () {
                                        $("#sMenuType").change(function (e) {
                                            $('#butSearch').trigger("click");
                                        });
                                    });
                                </script>
                            </td>--%>
                        </tr>
                    </table>

                </div>
                <div id="maingrid"></div>
            </div>
        </div>
    </div>

    <div class="content" id="target" style="display: none;">
        <div class="item">
            <div class="itemInner" id="divContent">
                <table width="100%">

                    <tr>
                        <td style="width: 150px; height: 45px; text-align: right;">功能名称：</td>
                        <td align="left">
                            <input type="text" id="FunctionName" placeholder="请输入功能名称（必填项）" class="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">功能类型：                        </td>
                        <td align="left">

                            <select id="FunctionType" class="l-selectorwin">
                                <option value="-1">请选择功能类型</option>
                                <option value="html">input button textarea 元素</option>
                                <option value="link">连接</option>
                                <option value="image">图片</option>
                                <option value="menu">菜单</option>
                                <option value="table">表格</option>
                                <option value="div">DIV</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">图表：</td>
                        <td align="left">
                                                        <div class="input-group">
                                <input type="text" class="form-control" id="icon" placeholder="输入图表标签" />
                                <div class="input-group-btn open">
                                    <button class="btn btn-white dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="true" style="line-height: 24px">
                                        选择
                                        <span class="caret"></span>
                                    </button>
                                    <ul id="ulIcon" class="dropdown-menu pull-right" style="min-width: 70px;">
                                        <li>
                                            <i class="arrowRightIcon" title="icon-angle-right"></i>
                                        </li>
                                        <li>
                                            <i class="icon01" title="icon01"></i>
                                        </li>
                                        <li>
                                            <i class="icon02" title="icon02"></i>
                                        </li>
                                        <li>
                                            <i class="icon03" title="icon03"></i>
                                        </li>
                                        <li>
                                            <i class="icon04" title="icon04"></i>
                                        </li>
                                        <li>
                                            <i class="icon05" title="icon05"></i>
                                        </li>
                                        <li>
                                            <i class="icon06" title="icon06"></i>
                                        </li>
                                        <li>
                                            <i class="icon07" title="icon07"></i>
                                        </li>
                                        <li>
                                            <i class="icon08" title="icon08"></i>
                                        </li>
                                        <li>
                                            <i class="icon09" title="icon09"></i>
                                        </li>
                                        <li>
                                            <i class="icon10" title="icon10"></i>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">是否可用：                        </td>
                        <td align="left">
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" id="isEnable" />是/否
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none;" id="isOpenid">
                        <td style="height: 45px; text-align: right;">是否展开：                        </td>
                        <td align="left">
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" id="isOpen" />是/否
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">父级标志：</td>
                        <td align="left">
                           <input type="text" id="SuperiorID"  style="display:none" />              
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">排序：                        </td>
                        <td align="left">
                            <input type="text" placeholder="请填写排序（必填项）" id="Sort" class="form-control" />
                        </td>
                    </tr>
                    <tr style="display: none;" id="MenuTypeid">
                        <td style="height: 45px; text-align: right;">菜单类型：                        </td>
                        <td align="left">
                            <select id="MenuType" onchange="typeChange();" style="width: 140px; height: 25px;">
                                <option value="">请选择菜单类型</option>
                                <option value="SystemMenu">系统菜单</option>
                                <option value="FunctionMenu">业务功能菜单</option>
                                <option value="TopMenu">系统头部菜单</option>
                            </select>
                        </td>
                    </tr>
                    <tr style="display: none;" id="selectCity">
                        <td style="width: 150px; height: 45px; text-align: right;">所属地市：</td>
                        <td style="text-align: left;">
                            <input type="text" class="form-control" style="width: 280px; display: none;" id="selectCityId" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">代码：                        </td>
                        <td align="left">
                            <input type="text" class="form-control" id="Code" placeholder="填写代码不允许重复" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 45px; text-align: right;">备注：                        </td>
                        <td align="left">
                            <textarea class="form-control" style="height: 150px;" id="Remarks" placeholder="填写功能备注（字符限制200）"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="height: 45px;"></td>
                        <td>
                            <button class="btn btn-success" id="save">提交保存</button>
                            <input id="Hidden1" type="hidden" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="_search" style="display: none;">
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: right; height: 34px;">关键字：
                    </td>
                    <td style="text-align: left;">
                        <input type="text" class="l-text" style="width: 140px;" id="keyword" placeholder="请输入关键字" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; height: 34px;">菜单类别：
                    </td>
                    <td style="text-align: left;">
                        <select id="sMenuType" class="l-text" style="width: 140px; height: 32px;">
                            <option value="">全部</option>
                            <option value="SystemMenu">系统菜单</option>
                            <option value="FunctionMenu">业务功能菜单</option>
                            <option value="TopMenu">系统头部菜单</option>
                            <option value="-1">html</option>
                        </select>
                    </td>
                </tr>
                <%--<tr>
                    <td style="text-align: right; height: 34px;">选择地市：
                    </td>
                    <td style="text-align: left;">
                        <input type="text" class="form-control" style="width: 140px; display: none;" id="sCityId" />
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: right; height: 34px;"></td>
                    <td>
                        <button class="btn btn-success" id="btnSearch">搜索</button>
                    </td>
                </tr>
            </table>
        </div>

    </div>
</asp:Content>
