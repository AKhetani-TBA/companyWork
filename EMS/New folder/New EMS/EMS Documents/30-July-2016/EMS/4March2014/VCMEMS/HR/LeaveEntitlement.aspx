<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveEntitlement.aspx.cs"
    Inherits="HR_LeaveEntitlement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>
 <script type="text/javascript">
     function checkDt(sender, args) {
         var StartDate = document.getElementById('txtFormDate').value;
         var EndDate = document.getElementById('txtTodate').value;
         var eDate = new Date(EndDate);
         var sDate = new Date(StartDate);
         //            if (StartDate != '' && EndDate != '' && sDate > eDate)

         if (EndDate < StartDate) {
             alert("Please ensure that the End Date is greater than or equal to the Start Date.");
             sender._selectedDate = new Date();
             sender._textbox.set_Value(sender._selectedDate.format(sender._format));
             return false;
         }
     }
      </script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font" id="mainDiv">
                <br />
                <div id="searchPane" runat="server">
                    <table width="100%">
                                        <tr  >
                       <td colspan="4">
                        <asp:LinkButton ID="CY" runat="server" onclick="CY_Click"> Current Year </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="CM" runat="server" onclick="CM_Click"> Current Month </asp:LinkButton>
                     </td>       
  
                        <tr>
                            <td style="width: 8%" align="left">
                                Employee Name:&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 12%">
                                <asp:DropDownList ID="ddlEmpName" runat="server" Width="150px" />
                            </td>
                            <td style="width: 5%" align="left">
                                From Date:
                            </td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtFormDate" runat="server" Font-Bold="True" Width="95px" AutoPostBack="true"
                                    ontextchanged="txtFormDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="attendaceDate" runat="server" OnClientDateSelectionChanged="checkDt" TargetControlID="txtFormDate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td style="width: 5%" align="right">
                                To Date :
                            </td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtTodate" runat="server" Font-Bold="True" Width="95px"></asp:TextBox>
                                <asp:CalendarExtender ID="attendancetodate" runat="server"  OnClientDateSelectionChanged="checkDt" TargetControlID="txtTodate"
                                    Format="dd-MMM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td style="width: 4%" align="center">
                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="btnSearch_Click" />
                            </td>
                            <%--<td style="width: 4%" align="center">
                                <asp:Button ID="btnAddNew" runat="server" Font-Bold="False" ForeColor="#333333" Text="Add COff Details"
                                    CausesValidation="False" CssClass="button" OnClick="btnAddNew_Click" />
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <div id="searchResults" runat="server">
                    <tr>
                        <td colspan="6" valign="top">
                            <div style="height: 1150px; width: 100%; overflow-y: auto; overflow-x: auto;">
                                <asp:GridView ID="gvleave" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                    HorizontalAlign="Justify" OnRowCommand="gvleave_RowCommand" OnRowDataBound="gvleave_RowDataBound"
                                    Width="100%"   OnSorting="gvleave_OnSorting" AllowSorting="true">
                                    <RowStyle BorderColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                     ForeColor="White" Font-Bold="false"/>
                                    <Columns>
                                        <asp:BoundField DataField="EmpName" SortExpression="EmpName"  HeaderText="Employee Name" ItemStyle-Width="15%" />
                                        <asp:BoundField DataField="LeaveDate"  SortExpression="LeaveDate"  HeaderText="Leave Date" DataFormatString="{0:dd-MMM-yyyy}"
                                            HtmlEncode="false" ItemStyle-Width="10%" />
                                        <asp:BoundField DataField="DayTypes" SortExpression="DayTypes"   HeaderText="DayTypes" ItemStyle-Width="10%" />
                                        <%--<asp:BoundField DataField="EmpId" HeaderText="EmpId" ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="LeaveId" HeaderText="LeaveId" ItemStyle-Width="10%" /> --%>
                                        <asp:TemplateField HeaderText="Leave Type" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate >
                                                <%--<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("LeaveId") %>'
                                                CommandName="Editleave" ImageUrl="~/images/edit_btn.jpg" />--%>
                                                <asp:DropDownList ID="ddlLeavetype" runat="server" Width="100px" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                        </asp:TemplateField>
                                        <%--Text='<%# Eval("field1").ToString + " - " +  Eval("field2").ToString %>'--%>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandArgument='<%# Eval("LeaveId") + "-" +  Eval("LeaveDate")+ "-" +  Eval("DayTypes") %>'
                                                    CommandName="Editleave" ImageUrl="~/images/edit_btn.png"  />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandArgument='<%# Eval("LeaveId") %>'
                                                CommandName="Deleteleave" ImageUrl="~/images/delete.ico" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>--%>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Leave Record Found
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <input id="hidleaveID" runat="server" type="hidden" />
                            </div>
                        </td>
                    </tr>
                </div>
                <div id="assignLeave" runat="server" visible="false">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update
                            Leave&nbsp; </legend>
                        <table>
                            <tr>
                                <td>
                                    Employee Name:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlemp" runat="server" Width="170px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    COff Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCDate" runat="server" Font-Bold="True" Width="95px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCDate"
                                        Format="dd MMM yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Comments
                                </td>
                                <td rowspan="2">
                                    <asp:TextBox ID="txtComments" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblstatus" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Approved" Value="A" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" />
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
