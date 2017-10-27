<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="ScenicSpotUser.aspx.cs" Inherits="JH.Account.FunctionMenu.ScenicSpotUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/ScenicSpotUser.ashx";
        $(function () {  
            var manager = $("#maingrid").ligerGrid({
                columns: [
                { display: '用户的标识', name: 'openid', align: 'left', width: 120 },
                { display: '用户的昵称', name: 'nickname', align: 'left', width: 120 },
                {
                    display: '性别', name: 'sex', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                        if (value == "1")
                            return "男";
                        else if (value == "2")
                            return "女";
                        else
                            return "未知";
                    }
                },
                { display: '用户所在城市', name: 'city', align: 'left', width: 120 },
                { display: '用户所在国家', name: 'country', align: 'left', width: 120 },
                { display: '用户所在省份', name: 'province', align: 'left', width: 120 },
                { display: '用户的语言', name: 'language', align: 'left', width: 120 },
                { display: '头像', name: 'headimgurl', align: 'left', width: 120 },
                { display: '用户关注时间', name: 'subscribe_time', align: 'left', width: 120 },
                { display: '用户所在的分组ID', name: 'groupid', align: 'left', width: 120 },
                ], isScroll: false,
                url: url,
                urlParms: { p: "list", r: Math.random()},
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
        });
 


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">旅客信息列表
                </span>
                <div class="query">
                    <button class="btn btn-success" id="search">搜索</button>
                </div>
            </div>
            <div class="itemInner" id=""> 
                <div id="maingrid"></div>
            </div>

        </div>
    </div>
    <div class="content">
       
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
</asp:Content>
