<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="IncomeTax_Master.aspx.cs" Inherits="IncomeTax_Master" %>
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
                    
                        <asp:GridView ID="showTax" runat="server" 
            AutoGenerateColumns="False" OnRowCommand="showTax_RowCommand"
                            OnRowDataBound="showTax_RowDataBound" OnSelectedIndexChanged="showTax_SelectedIndexChanged"
                            Width="340px" AllowPaging="True" 
                            OnPageIndexChanging="showTax_PageIndexChanging" 
            PageSize="15">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            <Columns>
                                <asp:BoundField DataField="taxMasterId" HeaderText="Tax Master Code">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Name">
                                    
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                            CommandArgument="taxMasterName" CommandName="sort" CssClass="gridlink">Tax Slab </asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="taxMasterNameLabel" runat="server" 
                                            Text='<%# Bind("taxMasterName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButdfton1" runat="server" CausesValidation="False" 
                                            CommandArgument="wef" CommandName="sort" CssClass="gridlink">WEF</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="wefLabel" runat="server" Text='<%# Bind("wef") %>'></asp:Label>
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
                                        <asp:ImageButton ID="delId" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                         CommandName="deleteIt"   Width="16px" CausesValidation="False" 
                                            
                                            OnClientClick="return confirm('Are you sure you want to delete the department?');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                                 <asp:BoundField DataField="sexType" HeaderText="Tax Master Code">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="ageLimit" HeaderText="Tax Master Code">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Tax Details Details Added...
                            </EmptyDataTemplate>
                            <HeaderStyle BackColor="#959595" ForeColor="White" Height="19px" Font-Names="Tw Cen MT Condensed"
                                Font-Size="17px" HorizontalAlign="Left" />
                        </asp:GridView>
                  
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
                    
                    <div>
                        <fieldset style="padding: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">
                                Add/Update Department</legend>
                                    <div style="float:left">
                    
                </div>
                 
                            <table>
                               <tr>
                                    <td style="text-align: right;  width: 145px;" >
                                        WEF&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="wef" runat="server" Width="180px"></asp:TextBox>
                                        <asp:CalendarExtender ID="wef_CalendarExtender" runat="server" Enabled="True" 
                                            TargetControlID="wef" DaysModeTitleFormat="dd MMMM yyyy">
                                        </asp:CalendarExtender>
                                        &nbsp;
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="taxMasterName"
                                            ErrorMessage="Please enter Tax Label" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        &nbsp;
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;  width: 145px;" >
                                        Tax Label <span style="color: #FF0000">*</span>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="taxMasterName" runat="server" Width="180px"></asp:TextBox>
                                        &nbsp;
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="taxMasterName"
                                            ErrorMessage="Please enter Tax Label" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        &nbsp;</td>
                                </tr>
                                 <tr>
                                    <td style="text-align: right;  width: 145px;" >
                                       Apply On <span style="color: #FF0000">*</span>
                                    </td>
                                    <td class="style1">
                                      
                                        
                                        <asp:RadioButton ID="maleBtn" runat="server" Text="Male" Checked="true" 
                                            GroupName="SexType" />
                                        <asp:RadioButton ID="femaleBtn" runat="server"  Text="Female" 
                                            GroupName="SexType"/>
                                        <asp:RadioButton ID="bothBtn" runat="server"  Text="Both" GroupName="SexType"/>
                                      
                                        
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="text-align: right;  width: 145px;" >
                                        Age <span style="color: #FF0000">*</span>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="ageText" runat="server" Width="180px"></asp:TextBox>
                                        &nbsp;
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ageText"
                                            ErrorMessage="Please enter Tax Label" CssClass="hideselect" ></asp:RequiredFieldValidator>
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
