<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveAssignment.aspx.cs" Inherits="HR_LeaveAssignment"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">

   
        .style1
        {
            width: 510px;
            height: 100px;
            float:left;
        }
        .style2
        {
            width: 160px;
        }
        .style3
        {
            color: #FF0066;
        }
        .style19
        {
            color: #FF0000;
        }
        .style22
        {
            color: #993399;
        }
        .style23
        {
            width: 194px;
            height: 109px;
        }
        .style25
        {
            width: 134px;
        }
        .newStyle1
        {
            border-left-style: inset;
            border-left-width: 1px;
            border-color: #CCCCCC;
        }
        .newStyle2
        {
        }
        </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
    <div class="EMS_font" id="mainDiv">
     <br />
            <div id="searchPane" runat="server" >
            <table>
                                 <tr>
                                     <td class="style11" nowrap="nowrap">
                                         <asp:Label ID="lblDeptName" runat="server" Text="Department : "></asp:Label>
                                     </td>
                                     <td class="style17" nowrap="nowrap">
                                         <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                             
                                             Width="100px" 
                                             onselectedindexchanged="showDepartments_SelectedIndexChanged">
                                            
                                         </asp:DropDownList>
                                     </td>
                                     <td class="style12" nowrap="nowrap">
                                         &nbsp;&nbsp;&nbsp; Employee :</td>
                                     <td class="style4">
                                         <asp:DropDownList ID="showEmployees" runat="server" CssClass="ddl"
                                             Width="160px" onselectedindexchanged="showEmployees_SelectedIndexChanged" >
                                            
                                         </asp:DropDownList>
                                         <asp:Label ID="lblempname" runat="server" Visible="False"></asp:Label>
                                     </td>
                                     <td class="style18">
                                         &nbsp;&nbsp;&nbsp;</td>
                                     <td class="style9">
                                     <asp:DropDownList ID="showLeaveTypes" style="display:none" runat="server" CssClass="ddl" 
                                             Width="110px">
                                         </asp:DropDownList>
                                         <asp:ImageButton ID="btnSearch" runat="server" 
                                             ImageUrl="~/images/searchbtn.png" onclick="btnSearch_Click" />
                                     </td>
                                     <td style="height: 26px; width: 224px;">
                                         <asp:Button ID="assignBalance" runat="server" CssClass="button" 
                                             Font-Bold="False" ForeColor="#333333" Style="float: right" 
                                             Text="Assign Leave" Width="110px" onclick="assignBalance_Click"  />
                                         &nbsp;&nbsp;
                                         </td>
                                 </tr>
                             </table>
             </div>
             <div id="searchResults" runat ="server"  visible="True">
               <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                             <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                             </legend>
                 <asp:GridView ID="LeaveAssign" runat="server" AllowPaging="True" 
                     AutoGenerateColumns="False" HorizontalAlign="Justify" 
                     OnPageIndexChanging="LeaveAssign_PageIndexChanging" 
                     OnRowCommand="LeaveAssign_RowCommand" 
                     OnRowDataBound="LeaveAssign_RowDataBound" 
                     OnSelectedIndexChanged="LeaveAssign_SelectedIndexChanged" Width="100%">
                     <RowStyle BorderColor="#333333" />
                     <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                     <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" 
                         Font-Size="17px" ForeColor="White" Height="19px" />
                     <Columns>
                      <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="chkSendMail" CommandName ="mail" runat="server" ImageUrl="~/images/e_mail.png"  CommandArgument="<%# Container.DataItemIndex %>"
                                           Width="20px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                         <asp:BoundField DataField="empId" HeaderText="emp">
                             <ControlStyle CssClass="hideselect" />
                             <FooterStyle CssClass="hideselect" />
                             <HeaderStyle CssClass="hideselect" />
                             <ItemStyle CssClass="hideselect" />
                         </asp:BoundField>
                        
                         <asp:TemplateField HeaderText="Employee">
                             <HeaderTemplate>
                                 <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                     CommandArgument="empName" CommandName="sort" CssClass="gridlink">Employee</asp:LinkButton>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="Label2" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                          <asp:TemplateField HeaderText="Department">
                             <EditItemTemplate>
                                 <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                             </EditItemTemplate>
                             <HeaderTemplate>
                                 <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                     CommandArgument="deptName" CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="Label1" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderText="Type">
                           <ControlStyle CssClass="hideselect" />
                             <FooterStyle CssClass="hideselect" />
                             <HeaderStyle CssClass="hideselect" />
                             <ItemStyle CssClass="hideselect" />
                             <EditItemTemplate>
                                 <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:TextBox>
                             </EditItemTemplate>
                             <HeaderTemplate>
                                 <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                                     CommandArgument="LeaveTypeName" CommandName="sort" CssClass="gridlink">Type</asp:LinkButton>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="Label3" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                        
                       
                       
                          <asp:TemplateField HeaderText="From Date">
                             <HeaderTemplate>
                                 <asp:LinkButton ID="linkfromdate" runat="server" CausesValidation="False" 
                                     CommandArgument="FromDate" CommandName="sort" CssClass="gridlink">From Date</asp:LinkButton>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="lblfromdate1" runat="server" Text='<%# Bind("FromDate") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                        
                          <asp:TemplateField HeaderText="To Date">
                             <HeaderTemplate>
                                 <asp:LinkButton ID="linktodate" runat="server" CausesValidation="False" 
                                     CommandArgument="ToDate" CommandName="sort" CssClass="gridlink">To Date</asp:LinkButton>
                             </HeaderTemplate>
                             <ItemTemplate>
                                 <asp:Label ID="lbltodate1" runat="server" Text='<%# Bind("ToDate") %>'></asp:Label>
                             </ItemTemplate>
                         </asp:TemplateField>
                         <asp:BoundField DataField="LeaveReason" HeaderText="Reason">
                         </asp:BoundField>
                        
                         <asp:BoundField DataField="leaveType" HeaderText="Half/Full" />
                        
                         <asp:CommandField ShowSelectButton="True">
                             <ControlStyle CssClass="hideselect" />
                             <FooterStyle CssClass="hideselect" />
                             <HeaderStyle CssClass="hideselect" />
                             <ItemStyle CssClass="hideselect" />
                         </asp:CommandField>
                         <asp:TemplateField ShowHeader="False">
                             <ItemTemplate>
                                 <asp:ImageButton ID="delLeaveBalance" runat="server" CommandName ="del"
                                     ImageUrl="~/images/delete.ico" 
                                     OnClientClick="return confirm('Are you sure you want to delete?');" 
                                     Width="16px" />
                             </ItemTemplate>
                             <ItemStyle Width="20px" />
                         </asp:TemplateField>
                         <asp:BoundField DataField="LeaveId" HeaderText="LeaveId">
                             <ControlStyle CssClass="hideselect" />
                             <FooterStyle CssClass="hideselect" />
                             <HeaderStyle CssClass="hideselect" />
                             <ItemStyle CssClass="hideselect" />
                         </asp:BoundField>
                     </Columns>
                     <EmptyDataTemplate>
                         No Leave Record Found
                     </EmptyDataTemplate>
                 </asp:GridView>
             </fieldset>
             </div>
             <div id="assignLeave" runat="server" visible="false">
             
                 <fieldset style="padding: 5px;">
                     <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update 
                         Leave&nbsp; </legend>
                     <table class="style1">
                         <tr>
                             <td class="style2">
                                 Employee</td>
                             <td>
                                 <asp:Label ID="empName" runat="server" Text="Label"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                             <td class="style2">
                                 Leave Type <span class="style3">*</span>
                             </td>
                             <td>
                                 <asp:RadioButton ID="typePl" runat="server" Checked="True" 
                                     GroupName="leaveTypesBtn" Text="PL" 
                                     oncheckedchanged="typePl_CheckedChanged" />
                                 <asp:RadioButton ID="typeCl" runat="server" GroupName="leaveTypesBtn" 
                                     style="padding-left:5px" Text="CL/SL" 
                                     oncheckedchanged="typeCl_CheckedChanged" />
                                 <asp:ImageButton ID="editLeavesBtn" runat="server" Height="20px" 
                                     ImageUrl="~/images/pen(2).ico" 
                                     style="padding-left:10px" ToolTip="Edit Leaves" Width="20px" 
                                     onclick="editLeavesBtn_Click" CausesValidation="False" />
                                 <asp:DropDownList ID="leaveTypes" runat="server" Visible="False" Width="120px" 
                                     onselectedindexchanged="leaveTypes_SelectedIndexChanged">
                                 </asp:DropDownList>
                                 &nbsp;
                                 
                             </td>
                         </tr>
                         <tr>
                             <td class="style2">
                                 From Date <span class="style19">*</span></td>
                             <td>
                                 <asp:TextBox   ID="fromDate" runat="server" Width="200px"></asp:TextBox>
                                 <asp:CalendarExtender ID="fromDate_CalendarExtender" runat="server" 
                                     DaysModeTitleFormat="dd MMMM yyyy" Enabled="True" 
                                     TargetControlID="fromDate" Format="dd MMMM yyyy">
                                 </asp:CalendarExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fromDate"
                                     ErrorMessage="From date required" Enabled="False"></asp:RequiredFieldValidator>
                             </td>
                         </tr>
                         <tr>
                             <td class="style2">
                                 To Date <span class="style19">*</span></td>
                             <td>
                                 <asp:TextBox  ID="toDate" runat="server" Width="200px" AutoPostBack="True" 
                                     ontextchanged="toDate_TextChanged"></asp:TextBox>
                                 <asp:CalendarExtender ID="toDate_CalendarExtender" runat="server" 
                                     DaysModeTitleFormat="dd MMMM yyyy" Enabled="True" TargetControlID="toDate" 
                                     Format="dd MMMM yyyy">
                                 </asp:CalendarExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  ControlToValidate="toDate"
                                     ErrorMessage="To date required"></asp:RequiredFieldValidator>
                             </td>
                         </tr>
                          <tr>
                             <td class="style2">
                                 &nbsp;<asp:Label ID="daysLabel" runat="server" Text="Days"></asp:Label>
                              </td>
                             <td>
                                 
                                 <asp:Label ID="noOfDays" runat="server" ></asp:Label>
                                 <asp:RadioButton ID="fullday" runat="server" GroupName="halfType" 
                                     Text="Full Day" Visible="false" 
                                     oncheckedchanged="fullday_CheckedChanged" />
                                 <asp:RadioButton ID="firstHalf" runat="server"  Visible="false" Checked="true" 
                                     GroupName="halfType" Text="First Half" 
                                     oncheckedchanged="firstHalf_CheckedChanged" />
                                 <asp:RadioButton ID="secondHalf" runat="server" Visible="false" 
                                     GroupName="halfType" Text="Second Half" 
                                     oncheckedchanged="secondHalf_CheckedChanged" />
                                 </td>
                         </tr>
                         <tr>
                             <td class="style2">
                                 Reason </td>
                             <td>
                                 <asp:TextBox ID="leaveReason" runat="server" Rows="3" TextMode="MultiLine" 
                                     Width="200px"></asp:TextBox>
                             </td>
                         </tr>
                         <tr>
                             <td class="style2">
                                 &nbsp;</td>
                             <td>
                                 <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                      Text="Save" onclick="saveBtn_Click" />
                                 &nbsp;
                                 <asp:Button ID="resetBtn" runat="server" CausesValidation="False" 
                                     CssClass="button"   
                                     Text="Back" onclick="resetBtn_Click" />
                                 <div ID="feed" class="style22" 
                                     style="display:none ;border: thin inset #ACA899; padding: 10px; background-color :#FFFFCC">
                                     Record Added Succesfully...</div>
                             </td>
                         </tr>
                     </table>
                     <div id="leaveBal" runat="server" style=" width:249px;float:left ">
                       <table class="style23"  style="border-left-style: solid; border-left-width: 1px; border-left-color: grey;display :none " >
                             <tr>
                                 <td colspan="2" style="background-color :#DBDBDB">
                                     Initail&nbsp; Balances for the year</td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     PL&nbsp;</td>
                                 <td style="background-color :#FFF0F0; ">
                                     <asp:Label ID="plBf" runat="server" Text="Label" 
                                         style="color: #9966FF;  "></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     CL</td>
                                 <td style="background-color :#FFF0F0; ">
                                     <asp:Label ID="clBf" runat="server" Text="Label" style="color: #9966FF"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     Total</td>
                                 <td style="background-color :#ECECFF; ">
                                     <asp:Label ID="totalInitialBalances" runat="server" Text="Label" style="color: #660033"></asp:Label>
                                 </td>
                             </tr>
                         </table>
                         <table class="style23" style="border-left-style: solid; border-left-width: 1px; border-left-color: grey">
                             
                             <tr>
                                 <td colspan="2" style="background-color :#DBDBDB">
                                     Balances for the month
                                     </td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     PL
                                 </td>
                                 <td style="background-color :#FFF0F0; ">
                                     <asp:Label ID="plBal" runat="server" Text="Label" style="color: #9966FF"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style25" >
                                     CL/SL</td>
                                 <td style="background-color :#FFF0F0; ">
                                     <asp:Label ID="clBal" runat="server" Text="Label" style="color: #9966FF"></asp:Label>
                                 </td>
                             </tr>
                              <tr>
                                 <td class="style25" >
                                     C.OFF</td>
                                 <td style="background-color :#FFF0F0; ">
                                     <asp:Label ID="cofs" runat="server" Text="Label" style="color: #9966FF"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     Total</td>
                                 <td style="background-color :#ECECFF; ">
                                     <asp:Label ID="leaveTotal" runat="server" Text="Label" style="color: #660033"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style25">
                                     Total Unpaid</td>
                                 <td style="background-color :#DDDDFF; ">
                                     <asp:Label ID="unpaidLeave" runat="server" Text="Label" style="color: #FF5050"></asp:Label>
                                 </td>
                             </tr>
                         </table>
                     </div>
                     <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                         DisplayMode="List" ShowMessageBox="True" ShowSummary="False" />
                 </fieldset></div>
    </div>
    </ContentTemplate> 
    </asp:UpdatePanel> 
    </form>
</body>
</html>
