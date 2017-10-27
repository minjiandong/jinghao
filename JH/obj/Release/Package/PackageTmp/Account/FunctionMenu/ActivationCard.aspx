<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="ActivationCard.aspx.cs" Inherits="JH.Account.FunctionMenu.ActivationCard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/ActivationCard.ashx";
        var manager;
        $(function () {
            manager = $("#maingrid").ligerGrid({
                columns: [
                { display: '面值', name: 'faceValue', align: 'center', width: 120 },
                    {
                        display: '是否发布', name: 'isRelease', align: 'center', width: 100, render: function (rowdata, rowindex, value) {
                            if (value == "1")
                                return "已发布";
                            else if (value == "2")
                                return "作废";
                            else
                                return "未发布";
                        }
                    }, 
                {
                    display: '是否使用', name: 'isUse', align: 'center', width: 100, render: function (rowdata, rowindex, value) {
                        if (value == "1")
                            return "使用"; 
                        else
                            return "未使用";
                    }
                },
                { display: '发布时间', name: 'ReleaseDate', align: 'left', width: 200 },
                { display: '使用时间', name: 'UseDate', align: 'left', width: 200 },
                { display: '激活码', name: 'Code', align: 'center', width: 200 }
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

            $("#add").click(function () {
                var data = {
                    p: "add",
                    values:$("#values").val(),
                    counts:$("#counts").val(),
                    r: Math.random()
                }
                EM(url, data, "add", function () {
                    manager.reload();
                });
            });

            $("#Release").click(function () {
                var row = manager.getSelectedRows();
                if (row.length != 0) {
                    var IDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        IDArror += row[i].id;
                        if ((row.length - i) > 1)
                            IDArror += ",";
                    }
                }
                var data = {
                    p: "Release",
                    IDArror:IDArror,
                    r: Math.random()
                }
                EM(url, data, "Release", function () {
                    manager.reload();
                });
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
                }
                if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                         if (e) {
                               var data = {
                    p: "delete",
                    IDArror: IDArror,
                    r: Math.random()
                }
                EM(url, data, "delete", function () {
                    manager.reload();
                });
                }
                }));
                
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">激活码管理
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
                                <input type="text" class="l-text" id="values" placeholder="金额" style="width:80px;" />
                                <input type="text" class="l-text" id="counts" placeholder="张数" style="width:80px;" />
                                <button class="btn btn-primary" id="add" type="button">生成激活码</button>
                                <button class="btn btn-info" id="delete" type="button">删除</button>
                                <button class="btn btn-danger" id="Release" type="button">发布</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="maingrid"></div>
                 
            </div>
              <form runat="server" id="from1">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-info" Text="导出" OnClick="Button1_Click" />
                  <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                </form>
        </div>
    </div>
</asp:Content>
