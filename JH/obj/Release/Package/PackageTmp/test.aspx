<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="JH.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 
    <title> </title>
 
     
    <style type="text/css">
        .auto-style1 {
            height:45px;
            text-align: center;
        }
        .auto-style2 {
            width: 158px;
            height: 45px;
        }
        .auto-style3 {
            height: 45px;
            text-align: center;
        }
        .auto-style4 {
            width: 158px;
            height: 45px;
        }
        .auto-style5 {
            width: 432px;
        }
        .auto-style6 {
            height: 45px;
            width: 158px;
            text-align: center;
        }
        .auto-style7 { 
            height: 45px;
            text-align: center;
            }
        .auto-style8 {
            height: 45px;
            text-align: center;
        }
        .auto-style9 {
            height: 45px;
            text-align: center;
        }
        .auto-style10 {
            height: 45px;
            width: 159px;
            text-align: center;
        }
    </style>
 
     
</head>
<body>
  
    <form id="form1" runat="server">
        <table style="width:600px;" border="1">
            <tr>
                <td class="auto-style1" colspan="2">
                    创建分组/删除分组</td>
            </tr>
            <tr>
                <td class="auto-style2">
                    分组名称：</td>
                <td class="auto-style5">
                    <asp:TextBox ID="group_name" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    分组ID：</td>
                <td class="auto-style5">
                    <asp:TextBox ID="group_id" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    执行类型：</td>
                <td class="auto-style5">
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Value="add">添加分组</asp:ListItem>
                        <asp:ListItem Value="edit">修改分组</asp:ListItem>
                        <asp:ListItem Value="del">删除分组</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    &nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="提交" />
                </td>
            </tr>
            </table>
        <br />

        <table style="width:600px;" border="1">
            <tr>
                <td class="auto-style3" colspan="2">
                    添加设备到分组/删除分组中的设备</td>
            </tr>
            <tr>
                <td class="auto-style4">
                    
                    执行类型：</td>
                <td class="auto-style5">

                    <asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem Value="add">添加设备到分组</asp:ListItem>
                        <asp:ListItem Value="delete">删除分组中的设备</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    
                    分组ID：</td>
                <td class="auto-style5">

                    <asp:TextBox ID="_group_id" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    
                    设备ID：</td>
                <td class="auto-style5">

                    <asp:TextBox ID="_device_id" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    
                    UUID:</td>
                <td class="auto-style5">

                    <asp:TextBox ID="uuid" runat="server" Width="398px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    major:
                    &nbsp;</td>
                <td class="auto-style5">

                    <asp:TextBox ID="major" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    minor:
                    &nbsp;</td>
                <td class="auto-style5">

                    <asp:TextBox ID="minor" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    &nbsp;</td>
                <td class="auto-style5">

                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="提交" />
                </td>
            </tr>
        </table>
        <br />
        <table style="width:600px;" border="1">
            <tr>
                <td class="auto-style6">
                    查询分组列表
                
                    <asp:Button ID="Button3" runat="server" Text="查询" OnClick="Button3_Click" />
                </td>
            </tr>
            
            <tr>
                <td class="auto-style7">
                    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
 
                     
                     </td>
            </tr>
            
        </table>
        <br />
        <table style="width:600px;" border="1">
            <tr>
                <td class="auto-style8" colspan="2">

                    查询分组详情
                
                </td>
            </tr>
            <tr>
                <td class="auto-style10">

                    组设备ID编号</td>
                <td>

                    <asp:TextBox ID="_groupid" runat="server"></asp:TextBox>
                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="查询" />

                </td>
            </tr>
            <tr>
                <td class="auto-style9" colspan="2">

                    <asp:GridView ID="GridView2" runat="server">
                    </asp:GridView>

                </td>
            </tr>
        </table>
    </form>
  
</body>
</html>
