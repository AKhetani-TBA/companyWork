<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RFID.aspx.cs" Inherits="HR_RFID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        .style2
        {
            width: 127px;
            text-align:right;
        }
        .hideselect
        {
            display: none;
        }
        .style3
        {
            color: #FF0066;
        }
        </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font" >
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
              <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update 
                            Card No.
                        </legend>
              <div style=" width:349px; float:left">
              <table>
              <tr>
              <td>
                  Status&nbsp;&nbsp;
              </td>
              <td>
                
                    <asp:DropDownList ID="ddlCardStatus" runat="server">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">Used</asp:ListItem>
                        <asp:ListItem Value="2">Free</asp:ListItem>
                    </asp:DropDownList>
                    
              </td>
              <td>
                  &nbsp;Card Type &nbsp;</td>
              <td>
               
             
                   <asp:DropDownList ID="cardType" runat="server">
                       <asp:ListItem Value="0">All</asp:ListItem>
                       <asp:ListItem Value="1">Permanent</asp:ListItem>
                       <asp:ListItem Value="2">Temporary</asp:ListItem>
                   </asp:DropDownList>
                
               </td>
               <td>
                   &nbsp;&nbsp;&nbsp;
               <asp:ImageButton ID="srchBtn"   runat="server" CssClass="Button" 
                       ImageUrl="~/images/searchbtn.png"   OnClick="srchBtn_Click" 
                       CausesValidation="False" />
              </td>
               </tr>
               </table>
             
  
                   <caption>
                       <br />
                        <div style="height: 1050px;width: 100%; overflow-y: auto; overflow-x: auto;" >
                       <asp:GridView ID="rfidDetail" runat="server" AllowPaging="false" 
                           AutoGenerateColumns="False" HorizontalAlign="Justify" 
                           onpageindexchanging="rfidDetail_PageIndexChanging" 
                           onrowcommand="rfidDetail_RowCommand" onrowdatabound="rfidDetail_RowDataBound" 
                           onselectedindexchanged="rfidDetail_SelectedIndexChanged" PageSize="19">
                           <RowStyle BorderColor="#333333" />
                           <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                           <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                               Font-Size="17px" ForeColor="White" Height="19px" />
                           <Columns>
                               <asp:BoundField DataField="RFIDId" HeaderText="RFIDId">
                                   <ControlStyle CssClass="hideselect" />
                                   <FooterStyle CssClass="hideselect" />
                                   <HeaderStyle CssClass="hideselect" />
                                   <ItemStyle CssClass="hideselect" />
                               </asp:BoundField>
                               <asp:TemplateField HeaderText="Name">
                                   <HeaderTemplate>
                                       <asp:LinkButton ID="sortCardNoBtn" runat="server" CausesValidation="False" 
                                           CommandArgument="RFIDNo" CommandName="sort" CssClass="gridlink">Card Number</asp:LinkButton>
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                       <asp:Label ID="Label1" runat="server" Text='<%# Bind("RFIDNo") %>'></asp:Label>
                                   </ItemTemplate>
                                   <ItemStyle Width="150px" />
                               </asp:TemplateField>
                               <asp:TemplateField HeaderText="Name">
                                   <HeaderTemplate>
                                       <asp:LinkButton ID="sortCardTypeBtn" runat="server" CausesValidation="False" 
                                           CommandArgument="IsTemp" CommandName="sort" CssClass="gridlink">Card Type</asp:LinkButton>
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                       <asp:Label ID="Label2" runat="server" Text='<%# Bind("IsTemp") %>'></asp:Label>
                                   </ItemTemplate>
                                   <ItemStyle Width="100px" />
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
                                   <ItemStyle Width="20px" />
                               </asp:TemplateField>
                           </Columns>
                           <EmptyDataTemplate>
                               No RFID card added...
                           </EmptyDataTemplate>
                       </asp:GridView>
                  </caption>
                   </div>
                        </div><div>
           
                    
                <table>
                    <tr>
                        <td class="style2">
                            Card Number <span class="style3">*&nbsp;&nbsp; </span></td>
                        <td>
                            <asp:TextBox ClientIDMode="Static" ID="rfidNo" runat="server" MaxLength="10" 
                                Width="165px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="rfidNo" CssClass="hideselect" 
                                ErrorMessage="Please enter RFID number"></asp:RequiredFieldValidator>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="rfidNo" 
                                ErrorMessage="Card number only includes numerics" 
                                ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator></td></tr><tr>
                        <td class="style2">
                            Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                        <td>
                            <asp:RadioButton ID="isTemp" runat="server" Checked="True" GroupName="type" 
                                Text="Temporary" />
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="isPermanent" runat="server" Text="Permanent" 
                                GroupName="type" oncheckedchanged="isPermanent_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            </td>
                        <td>
                            <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                onclick="saveBtn_Click" Text="Save"  />
                            &nbsp;<asp:Button ID="resetBtn" runat="server"  onclick="resetBtn_Click" 
                                Text="Reset" CausesValidation="False" CssClass="button"  />
                          
                        </td>
                    </tr>
                </table>
              
                </div>
                  </fieldset>
                &nbsp;&nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
             
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
