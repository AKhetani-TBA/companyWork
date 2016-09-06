<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="Deductions.aspx.cs" Inherits="Deductions" %>
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
            width: 79px;
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
            
                    
                    <div style="overflow:hidden; height: 355px;">
                       
                                    <div style="float:left">
                     <fieldset style="padding: 5px;width:250px; height:340px;">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Earnings</legend>
                                <asp:GridView ID="showEarnings" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" DataKeyNames="slabId" 
                                    onpageindexchanging="showDeduction_PageIndexChanging" 
                                    onrowcommand="showDeduction_RowCommand" 
                                    onrowdatabound="showDeduction_RowDataBound" 
                                    onrowupdating="showDeduction_RowUpdating" 
                                    onselectedindexchanged="showDeduction_SelectedIndexChanged" Width="100%">
                                    <RowStyle BorderColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="slabId" HeaderText="SlabId">
                                            <ControlStyle CssClass="hideselect" />
                                            <FooterStyle CssClass="hideselect" />
                                            <HeaderStyle CssClass="hideselect" />
                                            <ItemStyle CssClass="hideselect" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Name">
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkfdfdfButton1" runat="server" CausesValidation="False" 
                                                    CommandArgument="slabName" CommandName="sort" CssClass="gridlink">Earnings</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="earningSlabName" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:BoundField DataField="slabOrder" HeaderText="Seq. No." >
                                             <ItemStyle Width="55px" />
                                        </asp:BoundField>
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
                                                    OnClientClick="return confirm('Are you sure you want to delete the department?');" 
                                                    Width="16px" />
                                            </ItemTemplate>
                                            <ItemStyle Width="16px" />
                                        </asp:TemplateField>
                                       
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Deduction/Contribution Details Added...
                                    </EmptyDataTemplate>
                                    <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                        Font-Size="17px" ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                </asp:GridView>
                      
                  
                </div>
                <div style="float:left ">
                            <fieldset style="padding: 5px;width:250px; height:340px; margin-left:5px">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Deductions</legend>
                                          <asp:GridView ID="showDeduction" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" 
                                    onpageindexchanging="showDeduction_PageIndexChanging" 
                                    onrowcommand="showDeduction_RowCommand" 
                                    onrowdatabound="showDeduction_RowDataBound" 
                                    onrowupdating="showDeduction_RowUpdating" 
                                    onselectedindexchanged="showDeduction_SelectedIndexChanged" Width="100%" 
                                    DataKeyNames="slabId">
                                              <RowStyle BorderColor="#333333" />
                                              <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                              <Columns>
                                                  <asp:BoundField DataField="slabId" HeaderText="SlabId">
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
                                                              CommandArgument="slabName" CommandName="sort" CssClass="gridlink">Deduction/Contribution</asp:LinkButton>
                                                      </HeaderTemplate>
                                                      <ItemTemplate>
                                                          <asp:Label ID="Label1" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                                      </ItemTemplate>
                                                  </asp:TemplateField>
                                               <asp:BoundField DataField="slabOrder" HeaderText="Seq. No." >
                                             <ItemStyle Width="55px" />
                                        </asp:BoundField>
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
                                                              OnClientClick="return confirm('Are you sure you want to delete the department?');" 
                                                              Width="16px" />
                                                      </ItemTemplate>
                                                      <ItemStyle Width="16px" />
                                                  </asp:TemplateField>
                                              </Columns>
                                              <EmptyDataTemplate>
                                                  No Deduction/Contribution Details Added...
                                              </EmptyDataTemplate>
                                              <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                                                  Font-Size="17px" ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                </asp:GridView>
                            </fieldset>
                </div>
                            <div style="float: left">
                                <fieldset style="padding: 5px; width: 250px;  height:340px; margin-left:5px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Bonuses</legend>
                                    <asp:GridView ID="showBonuses" runat="server" AutoGenerateColumns="False" Width="100%"
                                        AllowPaging="True" OnSelectedIndexChanged="showDeduction_SelectedIndexChanged"
                                        OnPageIndexChanging="showDeduction_PageIndexChanging" OnRowCommand="showDeduction_RowCommand"
                                        OnRowDataBound="showDeduction_RowDataBound" OnRowUpdating="showDeduction_RowUpdating">
                                        <RowStyle BorderColor="#333333" />
                                        <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                        <Columns>
                                            <asp:BoundField DataField="slabId" HeaderText="SlabId">
                                                <ControlStyle CssClass="hideselect" />
                                                <FooterStyle CssClass="hideselect" />
                                                <HeaderStyle CssClass="hideselect" />
                                                <ItemStyle CssClass="hideselect" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Name">
                                                <EditItemTemplate>
                                                  
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="Lin5465Button1" runat="server" CausesValidation="False" CommandArgument="slabName"
                                                        CommandName="sort" CssClass="gridlink">Bonus</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="bonusslabName" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:BoundField DataField="slabOrder" HeaderText="Seq. No." >
                                             <ItemStyle Width="55px" />
                                        </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True">
                                                <ControlStyle CssClass="hideselect" />
                                                <FooterStyle CssClass="hideselect" />
                                                <HeaderStyle CssClass="hideselect" />
                                                <ItemStyle CssClass="hideselect" />
                                            </asp:CommandField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageBfgutton1" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                                        Width="16px" CommandName="deleteIt" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the department?');" />
                                                </ItemTemplate>
                                                <ItemStyle Width="16px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Deduction/Contribution Details Added...
                                        </EmptyDataTemplate>
                                        <HeaderStyle BackColor="#959595" ForeColor="White" Height="19px" Font-Names="Tw Cen MT Condensed"
                                            Font-Size="17px" HorizontalAlign="Left" />
                                    </asp:GridView>
                                </fieldset>
                            </div>
                            </div>
                     
                            <div >
                   <fieldset>
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update Slabs</legend>
                            <table>
                              <tr>
                                <td style="text-align :right ;" class="style2">Type&nbsp;:&nbsp;&nbsp;
                                </td>
                                <td>
                                
                                    &nbsp;
                                
                                    <asp:RadioButton style="padding-left:3px;" ID="typeEarning" Checked="true"  
                                        Text="Earning"  runat="server" GroupName="slabTypes" />
                                    <asp:RadioButton style="padding-left:6px;" ID="typeDeduction"  Text="Deduction" runat="server" GroupName="slabTypes"  />
                                    <asp:RadioButton style="padding-left:6px;" ID="typeBonus"  Text="Bonus" runat="server" GroupName="slabTypes"  />
                                 
                                
                                </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;  " class="style2" >
                                        Slab Name <span style="color: #FF0000">:*</span>
                                    </td>
                                    <td class="style1">
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="deductionName" runat="server" Width="180px"></asp:TextBox>
                                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="deductionName"
                                            ErrorMessage="Please enter Deduction/Contribution" CssClass="hideselect" ></asp:RequiredFieldValidator>
                                        &nbsp;
                                        
                                    </td>
                                </tr>
                              
                                <tr>
                                    <td class="style2" style="text-align: right;  ">
                                        Sequence&nbsp; No :&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td class="style1">
                                    &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtOrder" runat="server" Width="35px"></asp:TextBox>
                                        &nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ControlToValidate="txtOrder" ErrorMessage="Enter only numerics" 
                                            ValidationExpression="^[0-9+]+$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ControlToValidate="txtOrder" CssClass="hideselect" 
                                            ErrorMessage="Please enter order no"></asp:RequiredFieldValidator>
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
