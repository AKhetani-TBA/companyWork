<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Holiday.aspx.cs" Inherits="HR_Holiday" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript">
    function checkDate(sender, args) {
        var StartDate = document.getElementById('startDate').value;
        var EndDate = document.getElementById('endDate').value;
        var eDate = new Date(EndDate);
        var sDate = new Date(StartDate);
        if (StartDate != '' && StartDate != '' && sDate > eDate) {
            alert("Please ensure that the End Date is greater than or equal to the Start Date.");
            sender._selectedDate = new Date();
            sender._textbox.set_Value(sender._selectedDate.format(sender._format));
            return false;
        }
    }
    function checkDates(sender, args) {
        var StartDate = document.getElementById('start').value;
        var EndDate = document.getElementById('end').value;
        var eDate = new Date(EndDate);
        var sDate = new Date(StartDate);
        if (StartDate != '' && StartDate != '' && sDate > eDate) {
            alert("Please ensure that the End Date is greater than or equal to the Start Date.");
            sender._selectedDate = new Date();
            sender._textbox.set_Value(sender._selectedDate.format(sender._format));
            return false;
        }
    }
</script>
 <script type="text/javascript">
     function checkDt(sender, args) {
         var StartDate = document.getElementById('startDate').value;
         var EndDate = document.getElementById('endDate').value;
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

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" DisplayMode="List" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="SearchMode" runat="server">

                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search
                        </legend>
                        <div>
                            <table width="100%">
                                <tr>
                                       <td > 
                        <fieldset style="width:450px;" >
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search  Range
                        </legend> 
                                    From Date:
                                    <asp:TextBox ID="startDate" runat="server" AutoPostBack="true" 
                                        Font-Bold="True" ontextchanged="startDate_TextChanged" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendaceDate" runat="server" Format="dd-MMM-yyyy" 
                                        TargetControlID="startDate">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp; To Date :
                                    <asp:TextBox ID="endDate" runat="server" Font-Bold="True" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendancetodate" runat="server" Format="dd-MMM-yyyy" 
                                       TargetControlID="endDate">
                                    </asp:CalendarExtender>                                    
                                    <br />
                                    <asp:LinkButton ID="CY" runat="server" onclick="CY_Click"> Current Year </asp:LinkButton> 
                                      &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;  &nbsp;&nbsp;     &nbsp;&nbsp;    &nbsp;&nbsp; 
                                     &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp; 
                                    <asp:LinkButton ID="LinkButton1" runat="server" onclick="CM_Click"> Current Month </asp:LinkButton>
                             </fieldset>
                    </td>  
       
                                    <td>
                                        Location :
                                        </td>
                                        <td>
                                        <asp:DropDownList ID="showLocation" runat="server" OnSelectedIndexChanged="showLocation_SelectedIndexChanged"
                                             AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Type :
                                        </td>
                                        <td>
                                        <asp:DropDownList ID="showLeaveType" runat="server" OnSelectedIndexChanged="showLeaveType_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                            OnClick="btnSearch_Click" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnAdd" runat="server" Text="Add Holiday" OnClick="btnAdd_Click" />
                                    </td>
                                       <td > &nbsp;&nbsp;&nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" 
                        ImageUrl="~/images/excelicon.png" Width="23px"
                                        OnClick="btnexcel_Click" /><br />
                                </td>     
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div id="EditMode" runat="server" class="EMS_font">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Edit Holidays
                        </legend>
                        <table>
                            <tr>
                                <td>
                                    Start Date
                                </td>
                                <td width="75%">
                                    <asp:TextBox ID="start" runat="server" AutoPostBack="True" Height="20px" Width="162px"
                                        OnTextChanged="start_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="start_CalendarExtender" runat="server" Format="dd-MMM-yyyy"
                                        OnClientDateSelectionChanged="checkDates" TargetControlID="start">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="reqDateFrom" runat="server" ControlToValidate="start"
                                        ErrorMessage="Please select Start date" Display="None" InitialValue="">*</asp:RequiredFieldValidator>
<%--                                    <asp:RegularExpressionValidator ID="rgProperDateFrom" runat="server" ControlToValidate="start"
                                        ErrorMessage="Improper date" Display="None" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>
--%>                                </td>
                            </tr>
                            <tr>
                                <td>
                                    End Date
                                </td>
                                <td>
                                    <asp:TextBox ID="end" runat="server" Height="20px" Width="162px"></asp:TextBox>
                                    <asp:CalendarExtender ID="end_CalendarExtender1" runat="server" Format="dd-MMM-yyyy"
                                        OnClientDateSelectionChanged="checkDates" TargetControlID="end">
                                    </asp:CalendarExtender>
                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="end"
                                        ErrorMessage="Please select End date"  Display="None" InitialValue="">*</asp:RequiredFieldValidator>
--%>
                                    <%--<asp:RegularExpressionValidator ID="rgProperDateTo" runat="server" ControlToValidate="end"
                                        ErrorMessage="Improper date" Display="None" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Purpose of Holiday
                                </td>
                                <td>
                                    &nbsp;<asp:DropDownList ID="ddlPurpose" runat="server" Width="50%" OnSelectedIndexChanged="ddlPurpose_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <%--<asp:DropDownList ID="ddlPurpose" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPurpose_SelectedIndexChanged"
                                        OnLoad="ddlPurpose_SelectedIndexChanged" width="50%">
                                        <asp:ListItem  Selected="True">--  Select Purpose --</asp:ListItem>
                                        <asp:ListItem >New Years Day</asp:ListItem>
                                         <asp:ListItem >Makar Sankranti</asp:ListItem>
                                        <asp:ListItem >Republic Day</asp:ListItem>
                                        <asp:ListItem >Holi</asp:ListItem>
                                        <asp:ListItem >Independence Day</asp:ListItem>
                                        <asp:ListItem >Raksha Bandhan</asp:ListItem>
                                        <asp:ListItem >Mahatma Gandhi's Birthday</asp:ListItem>
                                        <asp:ListItem >Diwali (Deepavali)</asp:ListItem>
                                        <asp:ListItem >Bhai Duj</asp:ListItem>
                                        <asp:ListItem >Christmas Day</asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <asp:RequiredFieldValidator ID="reqPurpose" runat="server" ControlToValidate="ddlPurpose"
                                        ErrorMessage="Please Select Purpose" Display="None" InitialValue="--  Select Purpose --">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtPurpose" runat="server" Width="30%" Visible="False" ToolTip="Enter Holiday Purpose"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Location Of Holiday
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlHolidayLocation" runat="server" Width="50%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlHolidayLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqLocation" runat="server" ControlToValidate="ddlHolidayLocation"
                                        ErrorMessage="Please Select Holiday Location" Display="None" InitialValue="-- Select Location --">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtHolidayLocation" runat="server" Width="30%" Visible="False" ToolTip="EnterHoliday Location"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Holiday Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLeavetype" runat="server" Width="50%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlLeavetype_SelectedIndexChanged">                                        
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqLeavetype" runat="server" ControlToValidate="ddlLeavetype"
                                        ErrorMessage="Please Select Leave Type" Display="None" InitialValue="-- Select Leave Type --">*</asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLeaveType" runat="server" Visible="False" Width="30%" ToolTip="Enter Leave Type"></asp:TextBox>
                                </td>
                            </tr>
                            <table style="text-align: center; width: 40%;">
                                <tr>
                                    <td>
                                        <asp:Button ID="saveBtn" runat="server" CssClass="button" OnClick="saveBtn_Click"
                                            Text="Save" />
                                        <asp:Button ID="cancelBtn" runat="server" CausesValidation="False" CssClass="button"
                                            OnClick="cancelBtn_Click" Text="Cancel" Width="62px" />
                                    </td>
                                </tr>
                            </table>
                    </fieldset>
                </div>
                <div id="DisplayMode" runat="server">
                <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Holiday List
                        </legend>
                   <div style="height: 1150px; width: 100%; overflow-y: auto; overflow-x: auto;">
                        <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False" 
                            OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                            OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged" OnSorting="displayAll_OnSorting"
                           Width="100%" Font-Bold="False" AllowSorting="True"  Font-Size="12px" Font-Names="Microsoft Sans Serif">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                     ForeColor="White" Font-Bold="false"/>
                            <Columns>
                                <asp:TemplateField HeaderText="Holiday Id">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHolidayId" runat="server" Text='<%# Bind("HolidayId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="StartDate" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false"
                                    HeaderText="Start Date" SortExpression="StartDate"></asp:BoundField>
                                <asp:BoundField DataField="StartDate" DataFormatString="  {0:dddd}" HtmlEncode="false"
                                    HeaderText="Day" SortExpression="StartDate"></asp:BoundField>
                                <asp:BoundField DataField="EndDate" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="End Date"
                                    SortExpression="EndDate"></asp:BoundField>
                                <%-- <asp:BoundField DataField="EndDate" DataFormatString="  {0:dddd}"  HtmlEncode="false" 
                                                 HeaderStyle-CssClass="gridlink" HeaderText="Day" SortExpression="EndDate">
                                             </asp:BoundField>--%>
                                <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Purpose">
                                </asp:BoundField>
                                <asp:BoundField DataField="LeaveTypeName" HeaderText="Leave Type" SortExpression="LeaveTypeName">
                                </asp:BoundField>
                                <%--    <asp:BoundField DataField="Day" HeaderText="Day" SortExpression="Day" HeaderStyle-CssClass="gridlink">
                                    <HeaderStyle CssClass="gridlink"  />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location">
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:ImageButton ID="deltebtn" runat="server" CausesValidation="False" CommandArgument="delete"
                                                ImageUrl="~/images/Deletings.PNG" OnClientClick="javascript:if(!confirm('Are you sure ,you want to Delete Record?')){return false;}"
                                                 />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                                <%--    <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:ImageButton ID="statusImageButton" runat="server" CausesValidation="False" Height="31px"
                                                Width="31px" ImageUrl="~/images/info.bmp" CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClick="statusImageButton_Click " onmouseover="setstatusdivExtra();" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>--%>
                                <asp:CommandField ShowSelectButton="True">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:CommandField>
                            </Columns>
                            <PagerStyle />
                            <EmptyDataTemplate>
                                Sorry... No Data Found...!
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        </asp:GridView>
                  </div>
                        <asp:HiddenField ID="hidproject" runat="server" />
                    </fieldset>
                </div>
            </ContentTemplate>
             <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
