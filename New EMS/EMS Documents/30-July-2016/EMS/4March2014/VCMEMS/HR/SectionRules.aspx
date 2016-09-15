<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SectionRules.aspx.cs" Inherits="HR_SectionRules"
    EnableEventValidation="false" %>
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
            width: 70px;
        }
        .style2
        {
            color: #CC3300;
        }
        .style3
        {
            width: 612px;
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
            
    
        <table>
            <tr>
                <td >
                   Section : 
                </td>
                <td style="width: 180px">
                    <asp:DropDownList ID="showSections" runat="server" Width="150px" CssClass="ddl" 
                        AutoPostBack="True" onselectedindexchanged="showSections_SelectedIndexChanged1">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                        OnClick="btnSearch_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>
       
           <div id="griddiv" runat="server">
            <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                </legend>
                <table style="margin-bottom:5px">
                    <tr>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Section :
                        </td>
                        <td style="width: 150px">
                            <asp:Label ID="lblSectionName" runat="server"></asp:Label>
                        </td>
                        <td>
                            Max Limit :
                        </td>
                        <td style="width: 150px">
                            <asp:Label ID="lblSectionLimit" runat="server"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                </table>
             
                    <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowDataBound="srchView_RowDataBound"
                        OnSelectedIndexChanged="srchView_SelectedIndexChanged"  
                        onrowcommand="srchView_RowCommand" AllowPaging="True" 
                    onpageindexchanging="srchView_PageIndexChanging" PageSize="15">
                        <RowStyle BorderColor="#333333" />
                        <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        <HeaderStyle CssClass="gridheader" />
                        <Columns>
                           
                            <asp:BoundField DataField="sectionDetailId" HeaderText="id">
                               
                                <ControlStyle CssClass="hideselect" />
                                <FooterStyle CssClass="hideselect" />
                                <HeaderStyle CssClass="hideselect" />
                                <ItemStyle CssClass="hideselect" />
                            </asp:BoundField>
                            <asp:BoundField DataField="sectionId" HeaderText="sectionId">
                             
                                <ControlStyle CssClass="hideselect" />
                                <FooterStyle CssClass="hideselect" />
                                <HeaderStyle CssClass="hideselect" />
                                <ItemStyle CssClass="hideselect" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Rule Name">
                               <HeaderTemplate>
                                            <asp:LinkButton ID="lbtn1" runat="server" CausesValidation="False" CommandArgument="subSectionName"
                                                CommandName="sort" CssClass="gridlink">Rule Name</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl1" runat="server" Text='<%# Bind("subSectionName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min Limit">
                                 <HeaderTemplate>
                                            <asp:LinkButton ID="lbtn2" runat="server" CausesValidation="False" CommandArgument="downLimit"
                                                CommandName="sort" CssClass="gridlink">Min Limit</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl2" runat="server" Text='<%# Bind("downLimit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Max Limit">
                                 <HeaderTemplate>
                                            <asp:LinkButton ID="lbtn3" runat="server" CausesValidation="False" CommandArgument="upLimit"
                                                CommandName="sort" CssClass="gridlink">Max Limit</asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl3" runat="server" Text='<%# Bind("upLimit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="delCard" CommandName="delIt" runat="server" ImageUrl="~/images/delete.ico"
                                        Width="16px" OnClientClick="return confirm('Are you sure you want to delete?');" />
                                </ItemTemplate>
                                <ItemStyle Width="16px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Rules Assigned...</EmptyDataTemplate>
                    </asp:GridView>
               
                  </fieldset>
                   </div>
            
                   <div id="editdiv" runat="server">
                   <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Manage Section Rule
                </legend>
               
                    <table>
                        <tr>
                            <td class="style1">
                                Give Title <span class="style2">*</span> :&nbsp;
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="txtrulename" Width="400px" runat="server"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtrulename" ErrorMessage="Please Enter Rule Title" 
                                    SetFocusOnError="True" CssClass="hideselect"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Min Limit :&nbsp;
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="txtMinLimit" runat="server"></asp:TextBox>
                                &nbsp;Rs&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtMinLimit" ErrorMessage="Only enter numerics" 
                                    ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                Max Limit :
                            </td>
                            <td class="style3">
                                <asp:TextBox ID="txtMaxLimit" runat="server"></asp:TextBox>
                                &nbsp;Rs&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                    ControlToValidate="txtMaxLimit" ErrorMessage="Only enter numerics" 
                                    ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                &nbsp;
                                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                    ControlToCompare="txtMinLimit" ControlToValidate="txtMaxLimit" 
                                    CssClass="hideselect" 
                                    ErrorMessage="Max Limit should not be less than Min Limit" 
                                    Operator="GreaterThan" SetFocusOnError="True"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td class="style3">
                                <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                                    Text="Save" />
                                &nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click"
                                    Text="Cancel" Visible="False" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
               
            </fieldset>
             </div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
        
        
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
