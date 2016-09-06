<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveType.aspx.cs" Inherits="HR_LeaveType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 728px;
            height: 100px;
        }
        .style2
        {
            width: 88px;
            height: 32px;
        }
        .style3
        {
            color: #FF0066;
        }
        .style4
        {
            height: 32px;
        }
        .style5
        {
            height: 6px;
        }
    </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font">
    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
     
      
         <br />
            <div>
             <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update 
                            Leave Type
                        </legend>
                    
                <table >
                    <tr>
                        <td>
                            Leave Type <span class="style3">*</span></td>
                        <td >
                            <asp:TextBox ClientIDMode="Static" ID="leaveType" runat="server" MaxLength="10"></asp:TextBox>
                            
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="leaveType" CssClass="hideselect" 
                                ErrorMessage="Please specify Leave Type"></asp:RequiredFieldValidator>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="leaveType" 
                                ErrorMessage="Leave type only includes alphabets" 
                                ValidationExpression="^[a-zA-Z/_]+$"></asp:RegularExpressionValidator>
                            
                        </td>
                    </tr>
                    <tr>
                        <td >
                            </td>
                        <td >
                            <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                onclick="saveBtn_Click" Text="Save"  />
                            &nbsp;<asp:Button ID="resetBtn" runat="server"  onclick="resetBtn_Click" 
                                Text="Reset" CausesValidation="False" CssClass="button"  />
                          
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
                </fieldset>
                </div>
                
                
                   <div>
               <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Leave 
                            Types
                        </legend>
                <asp:GridView ID="LeaveTypeDetail" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" HorizontalAlign="Justify" 
                     onrowcommand="LeaveTypeDetail_RowCommand" 
                    onrowdatabound="LeaveTypeDetail_RowDataBound" 
                    onselectedindexchanged="LeaveTypeDetail_SelectedIndexChanged" 
                    onpageindexchanging="LeaveTypeDetail_PageIndexChanging">
                    <RowStyle BorderColor="#333333" />
                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                    <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                        Font-Size="17px" ForeColor="White" Height="19px" />
                    <Columns>
                        <asp:BoundField DataField="LeaveTypeId" HeaderText="LeaveTypeId">
                            <ControlStyle CssClass="hideselect" />
                            <FooterStyle CssClass="hideselect" />
                            <HeaderStyle CssClass="hideselect" />
                            <ItemStyle CssClass="hideselect" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Leave Type">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                    CommandArgument="LeaveTypeName" CommandName="sort" CssClass="gridlink">Leave Type</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemStyle Width="250px" />
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="True">
                        <ControlStyle CssClass="hideselect" />
                        <FooterStyle CssClass="hideselect" />
                        <HeaderStyle CssClass="hideselect" />
                        <ItemStyle CssClass="hideselect" />
                        </asp:CommandField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="delCard" runat="server" ImageUrl="~/images/delete.ico" 
                                    onclientclick="return confirm('Are you sure you want to delete?');" 
                                    Width="16px" />
                            </ItemTemplate>
                            <ItemStyle Width="18px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        Leavetype not added...
                    </EmptyDataTemplate>
                </asp:GridView>
                </fieldset>
                <br />
                </div>
    </ContentTemplate>
     </asp:UpdatePanel>
    </div>
            
    </form>
</body>
</html>
