<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IncomeTax.aspx.cs" Inherits="IncomeTax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="Head1">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 79%;
        }
        .style2
        {
            color: #FF0000;
        }
        .style3
        {
            width: 21%;
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
                      <div>
                          <div>
                             
                                
                                      <table>
                                          <tr>
                                              <td>
                                                  With Effect From Year :
                                              </td>
                                              <td>
                                                  <asp:DropDownList ID="wefDrop" runat="server" 
                                                      DataTextField="wef" DataValueField="wef" AutoPostBack="True" 
                                                      onselectedindexchanged="wefDrop_SelectedIndexChanged" 
                                                      ondatabound="wefDrop_DataBound">
                                                  </asp:DropDownList>
                                                 <%-- <asp:ObjectDataSource ID="years" runat="server" SelectMethod="GetAllDsGroupWef" 
                                                      TypeName="VCM.EMS.Biz.TaxMaster" 
                                                      OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>--%>
                                              </td>
                                          </tr>
                                      </table>
                                 
                             
                                 
                                      &nbsp;<asp:DataList ID="taxList" runat="server" DataKeyField="taxMasterId" 
                                              OnItemCommand="taxList_ItemCommand" 
                                              OnItemDataBound="taxList_ItemDataBound" 
                                              onselectedindexchanged="taxList_SelectedIndexChanged" RepeatColumns="1">
                                              <ItemTemplate>
                                                  <fieldset style="padding: 5px; margin-right: 5px; width: auto; vertical-align: top;
                                                    float: left; margin-left: 0px;">
                                                      <legend ID="genericLegend" runat="server" style="margin-bottom: 4px; font-weight: normal;
                                                        color: #808080;">
                                                          <asp:Label ID="Label2" runat="server" Text='<%# Bind("taxMasterName") %>'></asp:Label>
                                                      </legend>
                                                      <%--<asp:Label ID="Label2" runat="server"  Text='<%# Bind("slabName") %>'></asp:Label>--%>
                                                      <asp:GridView ID="showTax" runat="server" AllowPaging="True" 
                                                          AutoGenerateColumns="False" OnDataBound="showTax_DataBound" 
                                                          OnPageIndexChanging="showTax_PageIndexChanging" 
                                                          OnRowCommand="showTax_RowCommand" OnRowDataBound="showTax_RowDataBound" 
                                                          OnSelectedIndexChanged="showTax_SelectedIndexChanged" Width="550px">
                                                          <RowStyle BorderColor="#333333" Width="300px"/>
                                                          <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                          <Columns>
                                                              <asp:BoundField DataField="taxId">
                                                                  <ControlStyle CssClass="hideselect" />
                                                                  <FooterStyle CssClass="hideselect" />
                                                                  <HeaderStyle CssClass="hideselect" />
                                                                  <ItemStyle CssClass="hideselect" />
                                                              </asp:BoundField>
                                                                <asp:BoundField DataField="taxMasterId">
                                                                  <ControlStyle CssClass="hideselect" />
                                                                  <FooterStyle CssClass="hideselect" />
                                                                  <HeaderStyle CssClass="hideselect" />
                                                                  <ItemStyle CssClass="hideselect" />
                                                              </asp:BoundField>
                                                              <asp:TemplateField HeaderText="Start Range">
                                                                  <HeaderTemplate>
                                                                      <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                                          CommandArgument="startRange" CommandName="sort" CssClass="gridlink">Start Range</asp:LinkButton>
                                                                  </HeaderTemplate>
                                                                  <ItemTemplate>
                                                                      <asp:Label ID="l1" runat="server" Text='<%# Bind("startRange") %>'></asp:Label>
                                                                  </ItemTemplate>
                                                              </asp:TemplateField>
                                                              <asp:TemplateField HeaderText="End Range">
                                                                  <HeaderTemplate>
                                                                      <asp:LinkButton ID="LinkBfgutton1" runat="server" CausesValidation="False" 
                                                                          CommandArgument="endRange" CommandName="sort" CssClass="gridlink">End Range</asp:LinkButton>
                                                                  </HeaderTemplate>
                                                                  <ItemTemplate>
                                                                      <asp:Label ID="l2" runat="server" Text='<%# Bind("endRange") %>'></asp:Label>
                                                                  </ItemTemplate>
                                                              </asp:TemplateField>
                                                             
                                                              <asp:TemplateField HeaderText="Tax(%)">
                                                                  <HeaderTemplate>
                                                                      <asp:LinkButton ID="LinkBddafdfutton1" runat="server" CausesValidation="False" 
                                                                          CommandArgument="taxPercentage" CommandName="sort" CssClass="gridlink">Tax(%)</asp:LinkButton>
                                                                  </HeaderTemplate>
                                                                  <ItemTemplate>
                                                                      <asp:Label ID="l4" runat="server" Text='<%# Bind("taxPercentage") %>'></asp:Label>
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
                                                                      <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                                                          CommandName="deleteIt" Height="16px" ImageUrl="~/images/delete.ico" 
                                                                          OnClientClick="return confirm('Are you sure you want to delete the department?');" 
                                                                          Width="16px" />
                                                                  </ItemTemplate>
                                                                  <ItemStyle Width="16px" />
                                                              </asp:TemplateField>
                                                          </Columns>
                                                          <EmptyDataTemplate>
                                                              Tax records not added...
                                                          </EmptyDataTemplate>
                                                          <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                                              Font-Size="17px" ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                                      </asp:GridView>
                                                  </fieldset>
                                              </ItemTemplate>
                                          </asp:DataList>
                                
                                      <br />
                                       <div id="div1">
                                <asp:Button ID="fireEvent" Style="display: none" OnClick="EvenHandler" runat="server"
                                    Text="Button" />
                                   <asp:HiddenField ID="taxIdField" runat="server" />
                                <script language="javascript">
                                    function HandleEvent(taxId) {
                                        var t = document.getElementById('<%= fireEvent.ClientID %>');
                                        t.value = taxId;
                                        document.getElementById('<%= taxIdField.ClientID %>').value = taxId;

                                       // t.click();
                                        //alert(t.value);
                                    }
    
                                </script>

                            </div>
                                      <fieldset style="padding: 5px; clear: both; width: 560px; margin: 15px 10px 25px 3px;">
                                          <table>
                                           <tr>
                                                  <td colspan="2">
                                                      <table style="width: 100%; height: 100%;">
                                                          <tr>
                                                              <td style="text-align: right" class="style3">
                                                                  Citizen&nbsp; Category :</td>
                                                              <td style="text-align: left">
                                                                  <asp:DropDownList ID="categoryDrop" runat="server" Width="120px">
                                                                  </asp:DropDownList>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td style="width: 145px; text-align: right;">
                                                      Start Range
                                                  </td>
                                                  <td class="style1">
                                                      <asp:TextBox ID="startRange" runat="server" Width="180px"></asp:TextBox>
                                                      <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="startRange" 
                                    ErrorMessage="Bonus name only includes digits" 
                                    ValidationExpression="[0-9]"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="startRange"
                                    ErrorMessage="Please enter bank name" CssClass="hideselect"></asp:RequiredFieldValidator>--%>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td style="width: 145px; text-align: right;">
                                                      End Range
                                                  </td>
                                                  <td class="style1">
                                                      <asp:TextBox ID="endRange" runat="server" Width="180px"></asp:TextBox>
                                                  </td>
                                              </tr>
                                             
                                              <tr>
                                                  <td style="width: 145px; text-align: right;">
                                                      Tax Percentage <span class="style2">* </span>
                                                  </td>
                                                  <td class="style1">
                                                      <asp:TextBox ID="taxPercentage" runat="server" Width="180px"></asp:TextBox>
                                                      &nbsp;%
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td style="width: 145px;">
                                                      &nbsp;
                                                  </td>
                                                  <td class="style1">
                                                      <asp:Button ID="ins" runat="server" CssClass="button" Font-Bold="False" ForeColor="#333333"
                                                          OnClick="ins_Click" Text="Save" />
                                                      <asp:Button ID="cncl" runat="server" CausesValidation="False" CssClass="button" Font-Bold="False"
                                                          ForeColor="#333333" OnClick="cncl_Click" Text="Reset" Visible="False" />
                                                  </td>
                                              </tr>
                                          </table>
                                      </fieldset>
                                      <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                          ShowMessageBox="True" ShowSummary="False" />
                                  
                          </div>
                      </div>
       
    </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
