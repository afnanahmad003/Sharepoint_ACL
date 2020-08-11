<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="manager-access.aspx.cs" Inherits="selftprovisioning.web.gerenciar_acessos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td valign="top">
                <asp:TreeView ID="TreeView1" runat="server" NodeIndent="15" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                    <NodeStyle />
                    <ParentNodeStyle Font-Bold="False" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                    <SelectedNodeStyle Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                </asp:TreeView>
            </td>
            <td></td>
            <td valign="top">
                <asp:Repeater ID="rptPermission" runat="server" OnItemDataBound="rptPermission_ItemDataBound">
                    <HeaderTemplate>
                        <table border="1">
                            <tr style="background-color:black;color:white">
                                <td></td>
                                <td>User</td>
                                <td>Access</td>
                                <td>Share</td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                </td>
                                <td id="tdUser" runat="server"></td>
                                <td id="tdAccess" runat="server"></td>
                                <td id="tdShare" runat="server"></td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</asp:Content>