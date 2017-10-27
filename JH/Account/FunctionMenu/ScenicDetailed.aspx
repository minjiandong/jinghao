<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Site1.Master" AutoEventWireup="true" CodeBehind="ScenicDetailed.aspx.cs" Inherits="JH.Account.FunctionMenu.ScenicDetailed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="/ueditor/ueditor.all.min.js"> </script> 
    <script type="text/javascript" charset="utf-8" src="/ueditor/lang/zh-cn/zh-cn.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content">
        <div class="item">
            <div class="itemTitle">
                <span class="x_title">景点介绍
                </span>
                <div class="query"> </div>
            </div>
            <div class="itemInner" id=""> 
                <div style="padding-bottom: 5px;">
                  <table style="width:100%;">
                      <tr>
                          <td>
                            <textarea id="editor" style="width:100%;height:400px;"></textarea>
                              <script type="text/javascript">
                                  var ue = UE.getEditor('editor');
                                  $(function () {
                                      var id = getUrlParam("id");
                                      $.get('/Account/ashx/SysAshx/ScenicSpotManage.ashx', { p: "getM", r: Math.random(), id: id }, function (data) { 
                                          ue.ready(function () { 
                                              ue.setContent(data.detailed);
                                          });
                                      });
                                  });
                                  var Save = function () {
                                      var url = "/Account/ashx/SysAshx/ScenicSpotManage.ashx";
                                      var detailed = UE.getEditor('editor').getContent();
                                      var data = {
                                          p: "detailedSave",
                                          detailed: detailed,
                                          id: getUrlParam("id"),
                                          r: Math.random()
                                      }
                                      EM_AJAX(url, data, "detailedSave", function () {});
                                  }
                              </script>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              <button type="button" id="detailedSave" class="btn btn-primary" onclick="Save();">提交保存</button>
                          </td>
                      </tr>
                  </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
