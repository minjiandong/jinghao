<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="NewsManage.aspx.cs" Inherits="JH.Account.FunctionMenu.NewsManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script> 
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
     <script src="/Scripts/jquery.upload.js"></script>
    <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/NewsManage.ashx";
        var manager;
        $(function () {
            var ue = UE.getEditor('n_content');
            manager = $("#maingrid").ligerGrid({
                columns: [
                     {
                         display: '展示图片', name: 'n_img', align: 'center', width: 150, render: function (rowdata, rowindex, value) {
                             var h = "";
                             h += "<img src=\"" + value + "\" style=\"width:150px;\"/>";
                             return h;
                         }
                     },
                { display: '标题', name: 'n_title', align: 'left', width: 250 },
                {
                    display: '性别', name: 'n_type', align: 'center', width: 100, render: function (rowdata, rowindex, value) {
                        if (value == "1")
                            return "软文";
                        else if (value == "2")
                            return "头条";
                        else
                            return "未知";
                    }
                },
                { display: '阅读数', name: 'n_consult', align: 'left', width: 120 },
                { display: '发布时间', name: 'n_ReleaseTime', align: 'left', width: 200 },
                { display: '更新时间', name: 'n_updateTime', align: 'left', width: 200 }
                ], isScroll: false,
                url: url,
                urlParms: { p: "getlist", r: Math.random() },
                parms: {
                    "n_title": $("#txtSearch").val()
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
                $.ligerDialog.open({ target: $("#_search"), title: "搜索", width: 300, showMax: true, showToggle: true, height: 200 });
            });

            $("#delete").click(function () {
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择软文！"); return false;
                }
                if (row.length != 0) {
                    var IDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        IDArror += row[i].id;
                        if ((row.length - i) > 1)
                            IDArror += ",";
                    }

                    if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                           if (e) {
                                var data = { p: "newdelete", IDArror: IDArror, r: Math.random() }
                                $.get(url, data, function () {
                                    manager.reload();
                    });
                    }
                    }));
                }
            });

            $("#add").click(function () {
                $("#addnews input").val("");
                $("#addnews textarea").val("");
                $("#addnews").show();
            });

            $("#newsSave").click(function () {
                var n_title = $("#n_title").val();
                var n_type = $("#n_type").val();
                if (n_type == "-1") {
                    ErrorInfo("请选择文章类型！");
                    return false;
                }
                var n_abstract = $("#n_abstract").val();
                var n_content = UE.getEditor('n_content').getContent();
                var data = {
                    p: "newsSave",
                    n_title: n_title,
                    n_type:n_type,
                    n_abstract: n_abstract,
                    n_content: n_content,
                    id:$("#id").val(),
                    r: Math.random()
                }
                EM_AJAX(url, data, "newsSave", function () { manager.reload(); });
            });
            $("#edit").click(function () {
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择一条信息后再编辑"); return false;
                }
                if (row.length > 1) {
                    ErrorInfo("只能选择一条信息进行操作，请重新选择！"); return false;
                }  
                $.get(url, { p: "get", r: Math.random(), id: manager.getSelectedRow().id }, function (data) {
                    $("#n_title").val(data.n_title);
                    $("#n_type").val(data.n_type);
                    $("#n_abstract").val(data.n_abstract);
                    ue.ready(function () {
                        ue.setContent(data.n_content);
                    });
                    $("#id").val(data.id);
                    $("#addnews").show();
                });
            });

            //查询
            $("#btnSearch").click(function () {
                $.ligerDialog.hide();
                var name = $("#txtSearch").val();
                manager.loadServerData({ "n_title": name });
            });

            $("#uploadimg").click(function () {
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
                    params: { p: 'imgUpload', id: manager.getSelectedRow().id },
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
            });
        });

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">软文管理
                </span>
                <div class="query">
                    <button class="btn btn-success" id="search">搜索</button>
                </div>
            </div>
            <div class="itemInner" id=""> 
                <div style="padding-bottom: 5px;">
                    <table>
                        <tr>
                            <td style="padding-left:5px;">
                                <button class="btn btn-primary" id="add">新增</button>
                                <button class="btn btn-info" id="edit">编辑</button> 
                                <button class="btn btn-danger" id="delete">删除</button> 
                                <button class="btn btn-default" id="uploadimg">展示图</button>
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
            <table style="width:100%;">
                <tr>
                    <td style="text-align:right;height:34px;">
                        关键字：
                    </td>
                    <td style="text-align:left;">
                        <input type="text" class="l-text" id="txtSearch"  placeholder="请输入关键字" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;height:34px;"></td>
                    <td>
                        <button class="btn btn-success" id="btnSearch">搜索</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="addnews" style="display:none;">
     <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">操作软文
                </span>
                <div class="query">
                   
                </div>
            </div>
            <div class="itemInner" id=""> 
                <div style="padding-bottom: 5px;">
                    <table style="width:100%;">
                        <tr>
                            <td style="height:40px; width:150px;">
                                标题：
                            </td>
                            <td>
                                <input type="text" class="l-text" id="n_title" style="width:200px;" placeholder="请输入标题" />
                            </td>
                        </tr>
                         <tr>
                            <td style="height:40px; width:150px;">
                                文章类型：
                            </td>
                            <td>
                                <select id="n_type">
                                    <option value="-1">请选择</option>
                                    <option value="1">软文</option>
                                    <option value="2">头条</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                             <td style="height:90px; width:150px;">
                                摘要：
                            </td>
                            <td>
                                <textarea style="width:240px; height:80px;" class="l-text" placeholder="请输入摘要" id="n_abstract"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                内容：
                            </td>
                            <td>
                                <textarea id="n_content" style="height:400px; width:95%;"></textarea>
                            </td>
                        </tr>
                        <tr>
                             <td style="height:40px;width:150px;"></td>
                            <td>
                                <button type="button" class="btn btn-danger" id="newsSave">提交保存</button>
                                <input type="hidden" id="id" />
                            </td>
                        </tr>
                    </table>
                    </div>
                </div>
            </div>
         </div>
        </div>
     
</asp:Content>
