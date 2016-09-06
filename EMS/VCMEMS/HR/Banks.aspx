<%@ Page Title="" Language="C#" AutoEventWireup="true"  CodeFile="Banks.aspx.cs" Inherits="HR_Banks" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 393px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
   

                  <div>
        <div class="EMS_font">
            <div>
                <fieldset style="padding: 5px;">
                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/ Update
                        Banks </legend>
                    <div style= "float:left">
                        <asp:GridView ID="showBanks" runat="server" AutoGenerateColumns="False" Width="340px"
                    AllowPaging="True" OnRowCommand="showBanks_RowCommand" OnRowDataBound="showBanks_RowDataBound"
                    OnSelectedIndexChanged="showBanks_SelectedIndexChanged" 
                    onpageindexchanging="showBanks_PageIndexChanging" PageSize="15">
                    <RowStyle BorderColor="#333333" />
                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="serialId">
                            <ControlStyle CssClass="hideselect" />
                            <FooterStyle CssClass="hideselect" />
                            <HeaderStyle CssClass="hideselect" />
                            <ItemStyle CssClass="hideselect" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Bank Name">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("bankName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                    CommandArgument="bankName" CommandName="sort" CssClass="gridlink">Bank Name</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("bankName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="True">
                            <ControlStyle CssClass="hideselect" />
                            <FooterStyle CssClass="hideselect" />
                            <HeaderStyle CssClass="hideselect" />
                            <ItemStyle CssClass="hideselect" />
                        </asp:CommandField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                    Width="16px" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the department?');" />
                            </ItemTemplate>
                            <ItemStyle Width="16px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Bank Details Added...
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="#959595" ForeColor="White" Height="19px" Font-Names="Tw Cen MT Condensed"
                        Font-Size="17px" HorizontalAlign="Left" />
                </asp:GridView>
                </div>
                <table> 
                        <tr>
                            <td style="width: 145px; text-align: right;">
                                Bank Name <span style="color: #FF0000">*</span>
                            </td>
                            <td class="style1">
                                <asp:TextBox ID="bankname" runat="server" Width="180px"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="bankname"
                                    ErrorMessage="Please enter bank name" CssClass="hideselect"></asp:RequiredFieldValidator>
                                    
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ErrorMessage="Bank name only includes alphabets" 
                                    ControlToValidate="bankname" ValidationExpression="^[a-zA-Z ]+$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 145px;">
                                &nbsp;
                            </td>
                            <td class="style1">
                                <asp:Button ID="ins" runat="server" Font-Bold="False" CssClass="button" ForeColor="#333333"
                                    OnClick="ins_Click" Text="Save" />
                                &nbsp;
                                <asp:Button ID="cncl" runat="server" Font-Bold="False" CssClass="button" ForeColor="#333333"
                                    OnClick="cncl_Click" Text="Reset" CausesValidation="False" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="True" ShowSummary="False" />
                </fieldset>
            </div>
        </div>
       
    </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
