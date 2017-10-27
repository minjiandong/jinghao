<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="SupplierManage.aspx.cs" Inherits="JH.Account.FunctionMenu.SupplierManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var manager, maingrid4, combobox;
        var url = "/Account/ashx/SysAshx/userManage.ashx";
        $(function () {

             
            $("#ScenicSave").click(function () {
                var userid = $("#id").val();
                var SupplierID = $("#SupplierID").val();  
                var data = {
                    p: "add",
                    userid: userid,
                    SupplierID: SupplierID
                }
                EM_AJAX("/Account/ashx/SysAshx/SupplierManage.ashx", data, "ScenicSave", function () {
                    manager.reload();
                });
              
            });
            
            $.get("/Account/ashx/SysAshx/SupplierManage.ashx", { p: "ligerList", r: Math.random() }, function (d) {

                combobox = $("#SupplierID").ligerComboBox({
                    width: 250,
                    slide: false,
                    selectBoxWidth: 500,
                    selectBoxHeight: 240, 
                    grid: {
                        columns: [
                        { display: '景区名称', name: 'text', minWidth: 120, width: 100 }
                        ],
                        switchPageSizeApplyComboBox: true,
                        data: d,
                        checkbox: true,
                        usePager: false
                    },
                    isMultiSelect: true, 
                    condition: { fields: [{ name: 'text', label: '景区名称', width: 200, type: 'text' }] }
                });

            });
           

            $("#timek").ligerDateEditor({ label: '时间（起止）', labelWidth: 100, labelAlign: 'right', format: 'yyyy/MM/dd' });
            $("#timej").ligerDateEditor({ label: '时间（结束）', labelWidth: 100, labelAlign: 'right', format: 'yyyy/MM/dd' });

            list();

            


            $("#save").click(function () {
                var loginName = $("#loginName").val();
                var userName = $("#userName").val();
                var phone = $("#phone").val();
                var sex = $("#selectSex").val();
                var use = $("#selectState").val();
                var Commission = $("#Commission").val();
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
                //if (isPhoneNo(phone) == false) {
                //    ErrorInfo("电话号码格式错误");
                //    return;
                //}
                //if (CityID == "") {
                //    ErrorInfo("请选择所属地市！");
                //    return;
                //}
                var Hidden1 = $("#Hidden1").val();
                var data = {
                    loginName: loginName,
                    userName: userName,
                    phone: phone,
                    Commission: Commission,
                    sex: sex,
                    use: use,
                    Hidden1: Hidden1,
                    UserType: "1",
                    CityID: "",
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
                if (row.length == 0) {
                    $.ligerDialog.error("请选择行再点编辑！"); return false;
                }
                if (row.length > 1) {
                    ErrorInfo("只能选择一条信息进行编辑，请重新选择！"); return false;
                }
                else {
                    location.href = "#divContent";

                    $.get(url, { p: "edit", r: Math.random(), UserID: manager.getSelectedRow().UserID }, function (data) {
                        $("#loginName").val(data.UserName);
                        $("#userName").val(data.FullName);
                        $("#Commission").val(data.Commission);
                        $("#phone").val(data.Phone);
                        //$("#selectSex").val(data.UserSex);
                        $("#selectState").val(data.UserState);
                        $("#Hidden1").val(data.UserID);

                    });
                }
            });


            $("#set_supplier").click(function () {
                
                var row = manager.getSelectedRows();
                if (row.length == 0) {
                    $.ligerDialog.error("请选择一条信息后在操作！"); return false;
                }
                if (row.length > 1) {
                    ErrorInfo("只能选择一条供应商！"); return false;
                } else {
                    $("#id").val(manager.getSelectedRow().UserID);
                    $.get("/Account/ashx/SysAshx/SupplierManage.ashx", { p: "getsupp", r: Math.random(), userid: manager.getSelectedRow().UserID }, function (d) { 
                        combobox.selectValue(d.id);
                        combobox.reload();
                    });
                    $.ligerDialog.open({ target: $("#supplier"), title: "设置供应商", width: 500, showMax: true, showToggle: true, height: 300 });
                }


            });


            //删除
            $("#delete").click(function () {
                var row = manager.getSelectedRows();
                if (row.length != 0) {
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

            //查询
            $("#btnSearch").click(function () {
                list();
            });
        });
       
        var list = function () {
            manager = $("#maingrid").ligerGrid({
                columns: [
                { display: '登录名', name: 'UserName', align: 'left', width: 120 },
                { display: '供应商名称', name: 'FullName', align: 'left', width: 120 },
                { display: '电话', name: 'Phone', align: 'left', width: 120 },
                 {
                     display: '是否启用', name: 'UserState', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                         var h = "";
                         h += "<img src=\"/JJUI/images/" + value + ".png\"/>";
                         return h;
                     }
                 },
                {
                    display: '供应商数量', name: 'ScenicCount', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                        var h = ""; 
                        h += "<a href=\"javascript:gyscount('" + rowdata.UserID + "');\">" + value + "</a>"; 
                        return h;
                    }
                },
                { display: '提成比例', name: 'Commission', align: 'center', width: 120 },
                { display: '提成金额', name: 'Cost', align: 'center', width: 120 }
                ], isScroll: false,
                url: url,
                urlParms: { p: "list", r: Math.random(), UserType: "1", name: $("#txtSearch").val(), ScenicName: $("#ScenicName").val(), timek: $("#timek").val(), timej: $("#timej").val() },
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
        }
        var gyscount = function (userid) { 
            $.ligerDialog.open({ target: $("#gys"), title: "供应列表", width: 500, showMax: true, showToggle: true, height: 300 });
            var _manager = $("#listcount").ligerGrid({
                columns: [
                { display: '景区名称', name: 'mapname', align: 'left', width: 120 },
                { display: '电话', name: 'tel', align: 'left', width: 120 },
                { display: '等级', name: 'level', align: 'left', width: 120 }
                ], isScroll: false,
                url: url,
                urlParms: { p: "c_list", r: Math.random(), Userid: userid },
                method: "post",
                usePager: false,
                enabledSort: true,
                rownumbers: true,
                colDraggable: true
            });
        }
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
    <div id="supplier" style="display: none;">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: right; height: 40px; width: 30%;">请选择供应商：
                </td>
                <td style="text-align: left;">
                    <input type="text" id="SupplierID"  style="display:none;"/>
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


    <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">用户列表
                </span>
                <div class="query">
                    
                </div>

            </div>
            <div class="itemInner" id="">

                <div style="padding-bottom: 5px;">
                    <table>
                        <tr>
                            <td style="padding-left: 5px;">
                                <button class="btn btn-primary" id="add">新增</button>
                                <button class="btn btn-info" id="edit">编辑</button>
                                <button class="btn btn-danger" id="delete">删除</button>
                                <button class="btn btn-inverse" id="reset">重置密码</button>
                                <button class="btn btn-info" id="set_supplier">设置管辖景区</button>
                                 <div>
                    <ul style="margin-top: 10px;">
                        <li style="width: 210px; float: left;">
                            供应商名称：<input type="text" class="l-text" id="txtSearch" placeholder="请输入供应商名称" /> 
                        </li>
                        <li style="width: 200px; float: left;">
                            景区名称：<input type="text" class="l-text" id="ScenicName" placeholder="请输入景区名称" />
                        </li>
                        <li style="width:230px; float:left;">
                            <input type="text" placeholder=""  id="timek"/>
                        </li>
                        <li style="width:240px; float:left;">
                            <input type="text" placeholder="" id="timej" />
                        </li>
                        <li style="width: 153px; float: left;">
                            <button class="btn btn-success" id="btnSearch">搜索</button></li>
                    </ul>
                </div>
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
                        <td align="right" style="height: 50px;">供应商名称：</td>
                        <td align="left">
                            <input type="text" class="form-control input_i" id="userName" placeholder="请输入供应商名称" data-container="body" /></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">提成比例：</td>
                        <td align="left">
                            <input type="text" class="form-control input_i" id="Commission" placeholder="请输入小数如（20%请输入0.2）" data-container="body" /></td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 50px;">手机号码：</td>
                        <td align="left">
                            <input type="text" style="height: 34px;" class="form-control input_i" id="phone" placeholder="请输入手机号码" data-container="body" /></td>
                    </tr>
                    <tr style="display: none;">

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
        <div id="gys" style="display: none;">
             <div id="listcount"></div>
        </div>
    </div>
</asp:Content>
