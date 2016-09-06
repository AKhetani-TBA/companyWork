<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLeaveAllocatting.aspx.cs"
    Inherits="HR_AutoLeaveAllocatting" EnableViewState="true" EnableEventValidation="false"
    Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

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

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .style10
        {
            height: 20px;
            font-family: Microsoft Sans Serif;
            font-size: 12px;
            vertical-align: top;
            width: 72px;
        }
        .style24
        {
            width: 55%;
        }
        .style26
        {
            width: 61%;
        }
        .style30
        {
            width: 18%;
        }
        .style31
        {
            width: 1%;
        }
        .style34
        {
            width: 211px;
        }
        .style35
        {
            width: 95px;
        }
        .style36
        {
            width: 50px;
        }
        .style37
        {
            width: 27px;
        }
        .style38
        {
            width: 5px;
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
                    <div style="overflow-y: auto; overflow-x: auto">
                        <fieldset style="margin-top: 5px; width: 430px; direction: inherit;">
                            <legend style="margin-bottom: 5px; font-weight: normal; color: #808080;">Search Policy:</legend>
                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDateRange" runat="server" Font-Bold="True" Text="Date From:" Width="75px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="ins" MaxLength="10"
                                                Width="70px" OnTextChanged="txtStartDate_TextChanged" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:Image ID="imgStartDate" runat="server" BorderStyle="None" Height="22px" ImageUrl="~/images/calIcon.png"
                                                ToolTip="Please click here to select policy start date..." />
                                            <asp:CalendarExtender ID="attendaceDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtStartDate"
                                                PopupButtonID="imgStartDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="style36">
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="DateTo:"></asp:Label>
                                        </td>
                                        <td class="style35">
                                            <asp:TextBox ID="txtEndDate" runat="server" ValidationGroup="ins" MaxLength="10"
                                                Width="70px" OnTextChanged="txtEndDate_TextChanged" Enabled="False" />
                                        </td>
                                        <td class="style37">
                                            <asp:Image ID="imsEndDate" runat="server" BorderStyle="None" Height="22px" ImageUrl="~/images/calIcon.png"
                                                ToolTip="Please click here to select policy end date..." />
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txtEndDate"
                                                PopupButtonID="imsEndDate">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td class="style38">
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtStartDate"
                                                ControlToValidate="txtEndDate" ErrorMessage="End date must be Grether then start date..."
                                                Operator="GreaterThan" Type="Date" ValidationGroup="ins">*</asp:CompareValidator>
                                        </td>
                                        <td class="style26">
                                            <asp:Button ID="btnshow" runat="server" CssClass="button" OnClick="btnshow_Click"
                                                Text="Show" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto">
                    <fieldset style="margin-top: 5px; width: 430px; direction: inherit;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                        <div>
                            <table align="center">
                                <tr>
                                    <td>
                                        <fieldset style="margin-top: 5px; direction: inherit;">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">
                                                <asp:Label ID="Label4" runat="server" Text="Selece Leave Policy(s):" Font-Bold="true"></asp:Label></legend>
                                            <div style="width: 380px; height: 180px; overflow-y: auto; overflow-x: auto">
                                                <asp:CheckBoxList ID="ddlLeaveType" runat="server" CssClass="ddl" />
                                            </div>
                                        </fieldset>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chkLeaveTypeAll" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chkLeaveTypeAll_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div>
                    <div style="overflow-y: auto; overflow-x: auto">
                        <fieldset style="margin-top: 5px; width: 430px; direction: inherit;">
                            <legend style="margin-bottom: 5px; font-weight: normal; color: #808080;"></legend>
                            <div>
                                <table>
                                    <tr id="trButtons" runat="server">
                                        <td valign="top">
                                            <asp:Button ID="btnAssingLeave" runat="server" CssClass="button" OnClick="btnAssingLeave_Click"
                                                Text="Apply Policy To All Employees" />
                                        </td>
                                        <td colspan="3" align="right">
                                            <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
