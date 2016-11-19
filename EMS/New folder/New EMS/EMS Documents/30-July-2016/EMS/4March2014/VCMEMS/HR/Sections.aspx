<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="Sections.aspx.cs" Inherits="Sections" %>
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
        
        .style2
        {
            width: 119px;
        }
        
        .style3
        {
            color: #FF0000;
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
            
                    
                   
                       
                            <div style="height: 1050px;width: 100%; overflow-y: auto; overflow-x: auto;" >                         
                                
                     <fieldset style="padding: 5px;width:250px;">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Sections</legend>
                                <asp:GridView ID="showSlabs" runat="server" AllowPaging="false" 
                                    AutoGenerateColumns="False" 
                                    onpageindexchanging="showSlabs_PageIndexChanging" 
                                    onrowcommand="showSlabs_RowCommand" 
                                    onrowdatabound="showSlabs_RowDataBound" 
                                    onrowupdating="showSlabs_RowUpdating" 
                                    onselectedindexchanged="showSlabs_SelectedIndexChanged" Width="100%" 
                                    DataKeyNames="sectionId" PageSize="15">
                                    <RowStyle BorderColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="sectionId" HeaderText="sectionId">
                                            <ControlStyle CssClass="hideselect" />
                                            <FooterStyle CssClass="hideselect" />
                                            <HeaderStyle CssClass="hideselect" />
                                            <ItemStyle CssClass="hideselect" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Name">
                                           
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkfdfdfButton1" runat="server" CausesValidation="False" 
                                                    CommandArgument="sectionName" CommandName="sort" CssClass="gridlink">Section Name</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="sectionNameLabel" runat="server" Text='<%# Bind("sectionName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Limit">
                                           
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkgjhghrButton1" runat="server" CausesValidation="False" 
                                                    CommandArgument="sectionLimit" CommandName="sort" CssClass="gridlink">Limit</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="sectionLimitLabel" runat="server" Text='<%# Bind("sectionLimit") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                 
                                        <asp:BoundField DataField="sectionOrder" HeaderText="Order" />
                                 
                                        <asp:CommandField ShowSelectButton="True">
                                            <ControlStyle CssClass="hideselect" />
                                            <FooterStyle CssClass="hideselect" />
                                            <HeaderStyle CssClass="hideselect" />
                                            <ItemStyle CssClass="hideselect" />
                                        </asp:CommandField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                                    CommandName="deleteIt" Height="16px" ImageUrl="~/images/delete.ico" 
                                                    OnClientClick="return confirm('Are you sure you want to delete this?');" 
                                                    Width="16px" />
                                            </ItemTemplate>
                                            <ItemStyle Width="16px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Exemptiion Section Details Added...
                                    </EmptyDataTemplate>
                                    <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                        Font-Size="17px" ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                </asp:GridView>
                               
                      </fieldset>
                         
                   <fieldset>
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update 
                                    Section</legend>
                            <table>
                              
                                <tr>
                                    <td style="text-align: right;  " class="style2" >
                                        Section Name<span style="color: #FF0000"> *</span>
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="exemptionName" runat="server" Width="180px"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="exemptionName"
                                            ErrorMessage="Please enter Section Name" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        &nbsp;
                                        
                                    </td>
                                </tr>
                                  <tr>
                                    <td style="text-align: right;  " class="style2" >
                                        Limit&nbsp; 
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="limitAmount" runat="server" Width="180px"></asp:TextBox>
                                        
                                        &nbsp;
                                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                            runat="server" ControlToValidate="limitAmount" 
                                            ErrorMessage="Limit includes only numerics" ValidationExpression="^[0-9 ]+$"></asp:RegularExpressionValidator>
&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="style2" style="text-align: right;  ">
                                        Order <span class="style3">*</span>
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtSectionOrder" runat="server" Width="40px"></asp:TextBox>
                                        &nbsp; 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidat" runat="server" ControlToValidate="txtSectionOrder"
                                            ErrorMessage="Please enter Section Order" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        &nbsp;
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="ins" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                            Font-Size="18px" ForeColor="#333333" Height="28px" OnClick="ins_Click" Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="cncl" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                            Font-Size="18px" ForeColor="#333333" Height="28px" OnClick="cncl_Click" Text="Reset"
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
