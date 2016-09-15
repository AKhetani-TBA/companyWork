<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveBalanceDetails.aspx.cs" Inherits="HR_LeaveBalanceDetails"  EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
    .td_style
    {
    width:50%;
    border-left-style: solid ;
     border-left-width: 1px;
      border-left-color: #E8E8E8 ;
      text-align :center ;
    
    }
    .headerSeperator
    {
    border-left-style:inset ;
    border-left-width: thin ;
    border-left-color: #FFFFEC;
    }
        .newStyle1
        {
            border-right-style: solid;
            border-right-width: thin;   
            border-right-color: #C0C0C0;
        }
        .style27
        {
            width: 100%;
            height: 30px;
        }
        .maintainHeight
        {
        	 width: 100%;
        	 height :100%;
        }
         .style28
         {
             width: 80px;
             text-align: right;
         }
        </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0; padding: 0;">
    <form id="form2" runat="server">
    <div class="EMS_font" id="innerDiv">
    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
         <div>
             
                  <br />
                 <div id="search_grid" runat="server">
                     <table>
                         <tr>
                             <td>
                                
                                 <asp:Label ID="lbldeptname" runat="server" Text="Department : "></asp:Label>
                             
                                 <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="100px">
                                    
                                 </asp:DropDownList>
                             </td>
                             <td>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Employee:</td>
                             <td>
                                 <asp:DropDownList ID="showEmployees" runat="server"  CssClass="ddl"
                                     Width="160px" onselectedindexchanged="showEmployees_SelectedIndexChanged">
                                    
                                 </asp:DropDownList>
                                 <asp:Label ID="lblEmpName" runat="server" Visible="False"></asp:Label>
                             </td>
                             <td>
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Year : </td>
                             <td>
                                 <asp:DropDownList ID="showYear" runat="server"  CssClass="ddl"
                                   
                                     Width="100px">
                                 </asp:DropDownList>
                             </td>
                             <td class="style28">
                                 &nbsp;<asp:ImageButton ID="btnSearch" runat="server" 
                                     ImageUrl="~/images/searchbtn.png" onclick="btnSearch_Click" />
                             </td>
                         </tr>
                     </table>
                     <div id="CardDetail">
                         <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                             <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                             </legend>
                             <asp:GridView ID="LeaveBalance" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                 HorizontalAlign="Justify" 
                                 OnPageIndexChanging="LeaveBalance_PageIndexChanging" OnRowCommand="LeaveBalance_RowCommand"
                                 OnRowDataBound="LeaveBalance_RowDataBound" 
                                 Width="100%" PageSize="15" 
                                 onselectedindexchanged="LeaveBalance_SelectedIndexChanged">
                                 <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                 <HeaderStyle CssClass="gridheader" />
                                 <Columns>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                                                 CommandName="sort" CssClass="gridlink">Month</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("Slot") %>'></asp:Label>                   
                                         </ItemTemplate>
                                             
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox2" runat="server"  Text='<%# Bind("ClBf") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <table align="center" class="maintainHeight">
                                                 <tr style="height:45px">
                                                     <td colspan="2" 
                                                         style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #ECE9D8">
                                                         &nbsp; Leave b/f</td>
                                                 </tr>
                                                 <tr>
                                                     <td>
                                                         PL
                                                        </td>
                                                     <td style="border-left-style: solid; border-left-width: 1px; border-left-color: #ECE9D8">
                                                         CL</td>
                                                 </tr>
                                             </table>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                              <table align="center" style="height:100%;width:100%">
                                                 <tr>
                                                     <td style="width:50%;">
                                                       <asp:Label ID="Lafgbel1" runat="server" Text='<%# Bind("PlBf") %>'></asp:Label>                   
                                                     </td>
                                                     <td class="td_style">
                                                         <asp:Label ID="Label88" runat="server" Text='<%# Bind("ClBf") %>'></asp:Label>
                                                     </td>
                                                 </tr>
                                                </table>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox3" runat="server" ></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <table align="center" class="style27">
                                                 <tr>
                                                     <td colspan="2" 
                                                         style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #ECE9D8">
                                                         &nbsp; Credit Leaves</td>
                                                 </tr>
                                                 <tr>
                                                     <td colspan="2" 
                                                         style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #ECE9D8">
                                                         Leave Entitlement
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td>
                                                         PL</td>
                                                     <td style="border-left-style: solid; border-left-width: 1px; border-left-color: #ECE9D8">
                                                         CL</td>
                                                 </tr>
                                             </table>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                            <table align="center" class="style27">
                                                 <tr>
                                                     <td style="width:50%;text-align :center ;">
                                                       <asp:Label ID="Labelf1" runat="server" Text='<%# Bind("PlEnt") %>'></asp:Label>                   
                                                     </td>
                                                     <td class="td_style">
                                                         <asp:Label ID="Labedfl88" runat="server" Text='<%# Bind("ClEnt") %>'></asp:Label>
                                                     </td>
                                                 </tr>
                                                </table>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <table align="center" class="maintainHeight">
                                                 <tr >
                                                     <td colspan="3" 
                                                         style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #ECE9D8">
                                                         Actual Leaves</td>
                                                 </tr>
                                                 <tr style="height:45px">
                                                     <td style="width:33%" >
                                                         Full Day</td>
                                                     <td style="width:33%; border-left-style: solid; border-left-width: 1px; border-left-color: #ECE9D8">
                                                         Half Day</td>
                                                         <td style=" width:33%; border-left-style: solid; border-left-width: 1px; border-left-color: #ECE9D8">
                                                        Total Leave Taken</td>
                                                 </tr>
                                             </table>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <table align="center" style="width:100%;height:100%">
                                                 <tr>
                                                     <td  class="td_style" style="width:33%;">
                                                       <asp:Label ID="Labegflf1" runat="server" Text='<%# Bind("FullLeave") %>'></asp:Label>                   
                                                     </td>
                                                     <td class="td_style" style="width:33%">
                                                         <asp:Label ID="Labefgdfl88" runat="server" Text='<%# Bind("HalfLeave") %>'></asp:Label>
                                                     </td>
                                                      <td class="td_style" style="width:33%>
                                                         <asp:Label ID="Ladfbel2" runat="server" Text='<%# Bind("TotalLeaveTaken") %>'></asp:Label>
                                                     </td>
                                                 </tr>
                                                </table>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate >
                                         C.Off
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label5" runat="server" Text='<%# Bind("Cof") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label6" runat="server" Text='<%# Bind("UnpaidLeave") %>'></asp:Label>
                                         </ItemTemplate>
                                          <HeaderTemplate >
                                     Unpaid Leave
                                     </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                    
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <table align="center" class="maintainHeight">
                                                 <tr style="height:45px">
                                                     <td colspan="2" 
                                                         style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #ECE9D8">
                                                         Balance Leaves</td>
                                                 </tr>
                                                 <tr>
                                                     <td>
                                                         PL
                                                        </td>
                                                     <td style="border-left-style: solid; border-left-width: 1px; border-left-color: #ECE9D8">
                                                         CL</td>
                                                 </tr>
                                             </table>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                            <table align="center" class="style27">
                                                 <tr>
                                                     <td style="width:50%;">
                                                       <asp:Label ID="Labsdfsdelf1" runat="server" Text='<%# Bind("PlBal") %>'></asp:Label>                   
                                                     </td>
                                                     <td class="td_style">
                                                         <asp:Label ID="Labedgdfl88" runat="server" Text='<%# Bind("ClBal") %>'></asp:Label>
                                                     </td>
                                                 </tr>
                                                </table>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate >
                                         Days Payable
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label8" runat="server" Text='<%# Bind("DaysPayable") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 </Columns>
                                 <EmptyDataTemplate>
                                     No Leave Record Found...</EmptyDataTemplate></asp:GridView></fieldset>
                     </div>
                 </div>
         </div>
         <br />
         </div>
    </ContentTemplate>
     </asp:UpdatePanel>
    </div>
            
    </form>
    </body>
</html>
