<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="Departments.aspx.cs" Inherits="HR_Departments" %>
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
            width: 409px;
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
                        <fieldset style="padding: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">
                                Add/Update Department</legend>
                                    <div style="float:left">
                    
                        <asp:GridView ID="showDepartments" runat="server" AutoGenerateColumns="False" OnRowCommand="showDepartments_RowCommand"
                            OnRowDataBound="showDepartments_RowDataBound" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                            Width="340px" AllowPaging="True" 
                            OnPageIndexChanging="showDepartments_PageIndexChanging" PageSize="15">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="deptId" HeaderText="Department Code">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                            CommandArgument="deptName" CommandName="sort" CssClass="gridlink">Department Name</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowSelectButton="True">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:CommandField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                            Width="16px" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the department?');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Department Details Added...
                            </EmptyDataTemplate>
                            <HeaderStyle BackColor="#959595" ForeColor="White" Height="19px" Font-Names="Tw Cen MT Condensed"
                                Font-Size="17px" HorizontalAlign="Left" />
                        </asp:GridView>
                  
                </div>
                 
                            <table>
                                <tr>
                                    <td style="text-align: right;  width: 145px;" >
                                        Department Name <span style="color: #FF0000">*</span>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="deptname" runat="server" Width="180px"></asp:TextBox>
                                        &nbsp;
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="deptname"
                                            ErrorMessage="Please enter department" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        &nbsp;
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width: 145px;">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        <asp:Button ID="ins" runat="server"  CssClass="button"
                                           OnClick="ins_Click" Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="cncl" runat="server"  CssClass="button" OnClick="cncl_Click" Text="Reset"
                                            CausesValidation="False" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                           
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                DisplayMode="List" ShowMessageBox="True" ShowSummary="False" />
                        </fieldset>
                    </div>
               
            <br />
                 </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>