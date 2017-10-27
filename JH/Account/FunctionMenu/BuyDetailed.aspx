<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="BuyDetailed.aspx.cs" Inherits="JH.Account.FunctionMenu.BuyDetailed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         var url = "/Account/ashx/SysAshx/BuyDetailed.ashx";

         $(function () {
             $("#timek").ligerDateEditor({ label: '时间（起止）', labelWidth: 100, labelAlign: 'right', format: 'yyyy/MM/dd' });
             $("#timej").ligerDateEditor({ label: '时间（结束）', labelWidth: 100, labelAlign: 'right', format: 'yyyy/MM/dd' });
             var liger = $("#paymentType").ligerComboBox({
                 data: [
                 { text: '全部', id: '-1' },
                 { text: '微信支付', id: '1' },
                 { text: '激活码支付', id: '0' },
                 { text: '余额支付', id: '2' }
                 ], valueFieldID: 'paymentType_val'
             });
             liger.setValue('-1', '全部');
             list();
             //查询
             $("#btnSearch").click(function () {
                 list();
             });
         });
         var list = function () {
             var manager = $("#maingrid").ligerGrid({
                 columns: [
                 { display: '用户微信识别标志', name: 'openid', align: 'left', width: 280 },
                 { display: '消费景区', name: 'ScenicSpotName', align: 'left', width: 200 },
                 {
                     display: '消费类型', name: 'paymentType', align: 'center', width: 120, render: function (rowdata, rowindex, value) {
                         if (value == "1")
                             return "微信支付";
                         else if (value == "0")
                             return "激活码支付";
                         else
                             return "余额支付";
                     }
                 },
                 {
                     display: '消费金额', name: 'money', align: 'center', width: 120, type: 'float', totalSummary:
                      {
                          type: 'sum'
                      }
                 },
                 { display: '消费时间', name: 'consumptionDate', align: 'left', width: 200 }
                 ], isScroll: false,
                 url: url,
                 urlParms: { p: "list", r: Math.random(), nickname: $("#txtSearch").val(), paymentType: $("#paymentType_val").val(), timek: $("#timek").val(), timej: $("#timej").val() },
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

         var download = function () {
             $("#dow").html('加载中...');
             $.get(url, { p: "download", r: Math.random(), nickname: $("#txtSearch").val(), paymentType: $("#paymentType_val").val(), timek: $("#timek").val(), timej: $("#timej").val() }, function (data) {
                 if (data.type == "ok") {
                     $("#dow").html('<span style="color:#ff0000;"><a href="' + data.info + '">系统已下载完成，请点击下载</a></span>');
                 }
             });
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">消费明细
                </span>
                <div class="query">
                    <ul style="margin-top:10px;">
                        <li  style="width:60px; float:left; line-height:25px;">
                            支付类型：
                        </li>
                        <li style="width:153px; float:left;"> 
                            <input type="text" id="paymentType" />

                           <%-- <select id="paymentType">
                        <option value="-1">全部</option>
                        <option value="1">微信支付</option>
                        <option value="0">激活码支付</option>
                        <option value="2">余额支付</option>
                         </select> --%>
                        </li>
                        <li style="width:200px; float:left;">
                            景区名称：<input type="text" class="l-text" placeholder="请输入关键字" id="txtSearch" style=" margin-right:10px;" />
                        </li>
                        <li style="width:230px; float:left;">
                            <input type="text" placeholder=""  id="timek"/>
                        </li>
                        <li style="width:240px; float:left;">
                            <input type="text" placeholder="" id="timej" />
                        </li>
                        <li style="float:left;">
                             <button type="button" id="btnSearch" class=" btn btn-info">搜索</button>
                              <button type="button" class="btn btn-danger" onclick="download();" id="down">下载</button>
                        </li>
                    </ul>
                   
                </div>
            </div>
            <div class="itemInner" id=""> 
                <div id="maingrid"></div>
            </div>
            <div style="height:25px; line-height:25px; padding-left:10px;" id="dow">
                
            </div>
        </div>
    </div>
    
</asp:Content>
