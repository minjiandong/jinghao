<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="editPassword.aspx.cs" Inherits="JH.Account.FunctionMenu.editPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #passStrength {
            height: 10px;
            width: 210px;
            border: 1px solid #ccc;
            padding: 1px;
        }

        .strengthLv1 {
            background: red;
            height: 6px;
            width: 70px;
        }

        .strengthLv2 {
            background: orange;
            height: 6px;
            width: 140px;
        }

        .strengthLv3 {
            background: green;
            height: 6px;
            width: 208px;
        }
    </style>
    <script type="text/javascript">
        var url = "/Account/ashx/SysAshx/userManage.ashx";
        $(function () {
            $("#old").blur(function () {
                var oldpwd = $("#old").val();
                if (oldpwd != "") {
                    var d = {
                        oldpwd: oldpwd,
                        r: Math.random(),
                        p: "oldpwd"
                    }
                    $.ajax({
                        url: url,
                        data: d,
                        success: function (data) {
                            if (data.info == "ok") {
                            } else {
                                $.ligerDialog.error(data.info);
                                $("#old").val(null);
                            }
                        }
                    });
                }
                else {
                    ErrorInfo("请输入原密码");
                    return;
                }
            });

            $("#save").click(function () {
                var gysname = $("#gysname").val();
                var Phone = $("#Phone").val();
                var oldpwd = $("#old").val();
                var newpwd = $("#new").val();
                var surepwd = $("#sure").val();
                if (gysname == "") {
                    ErrorInfo("供应商名称不能为空！");
                    return;
                }
                if (oldpwd == "") {
                    ErrorInfo("请输入原密码");
                    return;
                }
                if (newpwd == "") {
                    ErrorInfo("请输入新密码");
                    return;
                }
                if (surepwd == "") {
                    ErrorInfo("请输入确认密码");
                    return;
                }
                if (newpwd != surepwd) {
                    ErrorInfo("确认密码与新密码不一致");
                    $("#sure").val(null);
                    return;
                }
                var data = {
                    oldpwd: oldpwd,
                    gysname: gysname,
                    Phone:Phone,
                    newpwd: newpwd,
                    r: Math.random(),
                    p: "editpwd"
                }
                EM_AJAX(url, data, "save", function () {
                    empty();
                });
            });
        });
        var empty = function () {
            $("#old").val(null);
            $("#new").val(null);
            $("#sure").val(null);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">修改密码
                </span>
                <div class="query">
                </div>

            </div>
            <div class="itemInner" id="divContent">
                <table style="width: 700px;">
                    <tr>
                        <td width="20%" align="right" style="height: 50px;"><%=gys %>：</td>
                        <td width="50%" align="left">
                            <input type="text" style="height: 34px;" class="form-control" id="gysname" value="<%=gysname %>" data-container="body" />
                        </td>
                        <td width="30%"></td>
                    </tr>
                    <tr>
                        <td width="20%" align="right" style="height: 50px;">电话号码：</td>
                        <td width="50%" align="left">
                            <input type="text" style="height: 34px;" class="form-control" id="Phone" value="<%=Phone %>" data-container="body" />
                        </td>
                        <td width="30%"></td>
                    </tr>
                    <tr>
                        <td width="20%" align="right" style="height: 50px;">原密码：</td>
                        <td width="50%" align="left">
                            <input type="password" style="height: 34px;" class="form-control" id="old" placeholder="请输入原密码" maxlength="16" data-container="body" />
                        </td>
                        <td width="30%"></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">新密码：</td>
                        <td align="left">
                            <input type="password" style="height: 34px;" class="form-control input_i" id="new" placeholder="请输入新密码" maxlength="16" data-container="body" />
                        </td>
                        <td align="left"><div style="float:left">密码强度：</div><div style="float:left" id="passStrength"></div></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">确认密码：</td>
                        <td align="left">
                            <input type="password" style="height: 34px;" class="form-control input_i" id="sure" placeholder="请输入确认密码" maxlength="16" data-container="body" /></td>
                    </tr>
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
    </div>
    <script type="text/javascript">
        PasswordStrength('new', 'passStrength');
        function PasswordStrength(passwordID, strengthID) {
            init(strengthID);
            var _this = this;
            document.getElementById(passwordID).onkeyup = function () {
                check(this.value);
            }
        };
        function init(strengthID) {
            var id = document.getElementById(strengthID);
            var div = document.createElement('div');
            var strong = document.createElement('strong');
            this.oStrength = id.appendChild(div);
            this.oStrengthTxt = id.parentNode.appendChild(strong);
        };
        function check(val) {
            var aLvTxt = ['', '低', '中', '高'];
            var lv = 0;
            if (val.match(/[a-z]/g)) { lv++; }
            if (val.match(/[0-9]/g)) { lv++; }
            if (val.match(/(.[^a-z0-9])/g)) { lv++; }
            if (val.length < 6) { lv = 0; }
            if (lv > 3) { lv = 3; }
            this.oStrength.className = 'strengthLv' + lv;
            this.oStrengthTxt.innerHTML = aLvTxt[lv];
        };
    </script>
</asp:Content>
