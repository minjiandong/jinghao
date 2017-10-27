<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="userManage.aspx.cs" Inherits="JH.Account.SysFile.userManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/userManage.ashx";
        $(function () {  
            var manager = $("#maingrid").ligerGrid({
                columns: [
                { display: '登录名', name: 'UserName', align: 'left', width: 120 },
                { display: '姓名', name: 'FullName', align: 'left', width: 120 },
                { display: '手机号码', name: 'Phone', align: 'left', width: 120 },
                {
                    display: '性别', name: 'UserSex', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                        if (value == "1")
                            return "男";
                        else
                            return "女";
                    }
                },
                 {
                     display: '是否启用', name: 'UserState', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                         var h = "";
                         h += "<img src=\"/JJUI/images/" + value + ".png\"/>";
                         return h;
                     }
                 }
                ], isScroll: false,
                url: url,
                urlParms: { p: "list", r: Math.random(), UserType: "0" },
                parms: {
                    "name": $("#txtSearch").val()
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


            $("#save").click(function () {
                var loginName = $("#loginName").val();
                var userName = $("#userName").val();
                var phone = $("#phone").val();
                var sex = $("#selectSex").val();
                var use = $("#selectState").val();
                //var CityID = $("#CityID_val").val();

                if (loginName == "") {
                    ErrorInfo("请输入登录名");
                    return;
                }
                if (userName == "") {
                    ErrorInfo("请输入姓名");
                    return;
                }
                if (phone == "") {
                    ErrorInfo("请输入手机号码");
                    return;
                }
                if (isPhoneNo(phone) == false) {
                    ErrorInfo("电话号码格式错误");
                    return;
                }
                //if (CityID == "") {
                //    ErrorInfo("请选择所属地市！");
                //    return;
                //}
                var Hidden1 = $("#Hidden1").val();
                var data = {
                    loginName: loginName,
                    userName: userName,
                    phone: phone,
                    sex: sex,
                    use: use,
                    Hidden1: Hidden1,
                    UserType: "0",
                    CityID:"",
                    r: Math.random(),
                    p: "save"
                }
                EM_AJAX(url, data, "save", function () {
                    manager.reload();
                    empty();
                });
            });

            $("#add").click(function () {
                location.href = "#divContent";
                empty();
            });
            $("#edit").click(function () {
                var row = manager.getSelectedRows();
                if (row.length==0) {
                    $.ligerDialog.error("请选择行再点编辑！"); return false;
                }
                if (row.length >1) {
                    ErrorInfo("只能选择一条信息进行编辑，请重新选择！"); return false;
                }
                else {
                    location.href = "#divContent";

                    $.get(url, { p: "edit", r: Math.random(), UserID: manager.getSelectedRow().UserID }, function (data) {
                        $("#loginName").val(data.UserName);
                        $("#userName").val(data.FullName);
                        $("#phone").val(data.Phone);
                        $("#selectSex").val(data.UserSex);
                        $("#selectState").val(data.UserState);
                        $("#Hidden1").val(data.UserID);
                         
                    });
                }
            });

            //删除
            $("#delete").click(function () {
                var row = manager.getSelectedRows();
                if (row.length!=0) {
                    var UserIDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        UserIDArror += row[i].UserID;
                        if ((row.length - i) > 1)
                            UserIDArror += ",";
                    }
                     
                    if ($.ligerDialog.confirm("确定要删除吗？", "系统提示", function (e) {
                       if (e) {
                            var data = {
                        p: "delete",
                        UserID: UserIDArror,
                        r: Math.random()
                    }
                            EM_AJAX(url, data, "delete", function () {
                                manager.reload();
                    });
                    }
                    }));
                }
                else {
                    $.ligerDialog.error("请选择行数据再点删除！"); return false;
                }

            });
            //重置密码
            $("#reset").click(function () {
                var row = manager.getSelectedRows();
                if (row.length != 0) {
                    var UserIDArror = "";
                    for (var i = 0; i < row.length; i++) {
                        UserIDArror += row[i].UserID;
                        if ((row.length - i) > 1)
                            UserIDArror += ",";
                    }
                    if ($.ligerDialog.confirm("确定要重置选中用户密码？", "系统提示", function (e) {
                       if (e) {
                            var data = {
                        p: "reset",
                        UserID: UserIDArror,
                        r: Math.random()
                    }
                        EM_AJAX(url, data, "reset", function () {
                            manager.reload();
                    });
                    }
                    }));
                }
                else {
                    $.ligerDialog.error("请选择行数据再点重置密码！"); return false;
                }

            });
            $("#search").click(function () {
                $.ligerDialog.open({ target: $("#_search"), title: "搜索", width: 300, showMax: true, showToggle: true, height: 400 });
            });
            //查询
            $("#btnSearch").click(function () {
                $.ligerDialog.hide();
                var name = $("#txtSearch").val();
                manager.loadServerData({ "name": name });
            });
        });
        var empty = function () {
            $("#divContent input").val("");
            $("#Hidden1").val("");
            $("#selectSex").val(1);
            $("#selectState").val(1);
        }
        // 验证手机号
        function isPhoneNo(phone) {
            var pattern = /^1[34578]\d{9}$/;
            return pattern.test(phone);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">用户列表
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
                                <button class="btn btn-inverse" id="reset">重置密码</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="maingrid"></div>
            </div>

        </div>
    </div>
    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">用户操作
                </span>
                <div class="query">
                </div>

            </div>
            <div class="itemInner" id="divContent">
                <table style="width: 700px;">
                    <tr>
                        <td width="100" align="right" style="height: 50px;">登录名：                        </td>
                        <td width="85%" align="left">
                            <input type="text" class="form-control" id="loginName" placeholder="请输入登录名" data-container="body" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">姓名：</td>
                        <td align="left">
                            <input type="text" class="form-control input_i" id="userName" placeholder="请输入姓名" data-container="body" /></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">手机号码：</td>
                        <td align="left">
                            <input type="number" style="height:34px;" class="form-control input_i" id="phone" placeholder="请输入手机号码" data-container="body" /></td>
                    </tr>
                    <tr>

                        <td align="right" style="height: 50px;">性别：</td>
                        <td align="left">
                            <select id="selectSex" class="selected" style="width: 150px;">
                                <option value="1">男</option>
                                <option value="0">女</option>
                            </select>
                            <%--<input type="radio" name="sex" value="1" checked="checked" />&nbsp;男&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="sex" value="0" />&nbsp;女--%></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">是否启用：</td>
                        <td align="left">
                            <select id="selectState" class="selected" style="width: 150px;">
                                <option value="1">是</option>
                                <option value="0">否</option>
                            </select>
                            <%--<input type="radio" name="use" value="1" checked="checked" />&nbsp;是&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" name="use" value="0" />&nbsp;否--%>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td style="text-align:right;">
                            归属地市：
                        </td>
                        <td>
                            <input type="text" id="CityID" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right" style="height: 50px;"></td>
                        <td align="left">
                            <button class="btn btn-success" id="save">提交保存</button>
                            <input id="Hidden1" type="hidden" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
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
