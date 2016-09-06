<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonthlyAttendance.aspx.cs"
    EnableEventValidation="false" EnableViewState="true" Inherits="HR_MonthlyAttendance"
    Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        #imgLoad
        {
            height: 17px;
            width: 132px;
        }
        #imgload
        {
            width: 170px;
            height: 12px;
        }
        #tblResult td
        {
            border: solid 1px black;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function showlogdiv() {
            var dv = document.getElementById("in_out_logs");
            dv.style.pixelLeft = event.x - 480;  //380
            dv.style.pixelTop = event.y + 50;    //15
        }
        //       function hidelogdiv() {
        //           var dv = document.getElementById("in_out_logs");
        //           dv.style.display = "none";
        //           document.getElementById("lblDetailLogs").innerHTML = "";
        //           document.getElementById("lblOutsideDetails").innerHTML = "";
        //       }
        function OpenPopup(strURL) {
            window.open(strURL, "List", "scrollbars=yes,resizable=yes,width=600,height=450,top=175,left=475");
            return false;
        }

        function SendAttach() {
            try {
                var subject = "Regarding Irregular  Hours in Attendance";
                //                alert(document.form1.elements('<%=mailto.ClientID %>').value);               
                var MailTo = document.getElementById('<%=mailto.ClientID %>').value;
                var msg = document.getElementById('mailtext').value;
                var outlookApp = new ActiveXObject("Outlook.Application");
                var mailItem = outlookApp.CreateItem(0); // getNameSpace("MAPI");
                // mailFolder = nameSpace.getDefaultFolder(6);
                //mailItem = mailFolder.Items.add('IPM.Note.FormA');
                mailItem.Subject = subject;
                mailItem.To = MailTo; //"kpatel@thebeastapps.com"; //
                mailItem.HTMLBody = msg;
                //mailItem.BodyFormat = Html; //MailFormat.Html;                 
                mailItem.display(0);
            }
            catch (err) {
                alert(err);
            }
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //            if (objRef.checked) {
            //                //If checked change color to Aqua
            //                row.style.backgroundColor = "aqua";
            //            }
            //            else {
            //                //If not checked change back to original color
            //                if (row.rowIndex % 2 == 0) {
            //                    //Alternating Row Color
            //                    row.style.backgroundColor = "#C2D69B";
            //                }
            //                else {
            //                    row.style.backgroundColor = "white";
            //                }
            //            }

            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        //                        if (row.rowIndex % 2 == 0) {
                        //                            //Alternating Row Color
                        //                            row.style.backgroundColor = "#C2D69B";
                        //                        }
                        //                        else {
                        //                            row.style.backgroundColor = "white";
                        //                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>

</head>
<body onload="hideProgress();">
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnexcel"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="btnword"></asp:PostBackTrigger>
                <%--<asp:PostBackTrigger ControlID="btnOutlook"></asp:PostBackTrigger>
                <asp:PostBackTrigger ControlID="mailtext" />
                <asp:PostBackTrigger ControlID="mailto" />--%>
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField ID="mailto" runat="server" />
                <asp:HiddenField ID="mailtext" runat="server" />
                <asp:HiddenField ID="monthyear" runat="server" />
                <div id="search_grid" runat="server">
                    <fieldset style="margin-top: 5px">
                        <table>
                            <tr>
                                <td nowrap="nowrap">
                                    Month
                                </td>
                                <td nowrap="nowrap">
                                    <asp:DropDownList ID="ddlMonths" runat="server" OnSelectedIndexChanged="ddlMonths_SelectedIndexChanged"
                                        Width="100px" CssClass="ddl">
                                    </asp:DropDownList>
                                </td>
                                <td nowrap="nowrap">
                                    Year
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYears" runat="server" Width="75px" CssClass="ddl" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                        OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" Width="168px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblemp" runat="server" Text="Employee"></asp:Label>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="showEmployees" runat="server" OnSelectedIndexChanged="ddlEmployees_SelectedIndexChanged"
                                        Width="190px" CssClass="ddl">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Button ID="btnShowDetails" runat="server" Text="Show Details" CssClass="button"
                                        OnClick="btnShowDetails_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="divdown" runat="server">
                    <fieldset style="margin-top: 5px">
                        <table id="dwnlodLog" width="100%">
                            <tr>
                                <td style="width: 15%;" align="center">
                                    <div id="reportdiv" runat="server" style="text-align: center; margin-left: 5px;">
                                        <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="22px"
                                            OnClick="btnexcel_Click" ToolTip="Excel Report" />
                                        &nbsp;
                                        <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" Width="22px"
                                            OnClick="btnword_Click" Style="height: 21px" ToolTip="Word Report" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="btnOutlook" runat="server" ImageUrl="~/images/mailicon.png"
                                            OnClick="ImageButton2_Click" Width="22px" ToolTip="Outlook" Style="height: 21px" />
                                    </div>
                                </td>
                                <%-- <td style="width: 45%;">
                                    <asp:Label ID="lblStartdate" runat="server" Text="Start Date" CssClass="style2"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:CalendarExtender ID="StartDateCal" runat="server" DaysModeTitleFormat="dd MMMM yyyy"
                                        Enabled="True" TargetControlID="txtStartDate" Format="dd MMMM yyyy">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblEnddate" runat="server" Text="End Date"></asp:Label>&nbsp;&nbsp;
                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100px"></asp:TextBox>
                                    <asp:CalendarExtender ID="EndDateCal" runat="server" DaysModeTitleFormat="dd MMMM yyyy"
                                        Enabled="True" TargetControlID="txtEndDate" Format="dd MMMM yyyy">
                                    </asp:CalendarExtender>
                                </td> --%>
                                <td style="width: 15%;" align="center">
                                    <asp:Button ID="dwnloadLogs" runat="server" CssClass="button" Width="111px" OnClick="dwnloadLogs_Click"
                                        Text="Download Logs" />
                                </td>
                                <td style="text-align: left; width: 45%;">
                                    Last Download Time : &nbsp;&nbsp;
                                    <asp:Label ID="lblDownloadTime" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td style="width: 25%; text-align: center">
                                    <asp:UpdateProgress ID="updProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                        DynamicLayout="true">
                                        <ProgressTemplate>
                                            <img id="imgload" alt="Please wait..." src="../images/update-progress.gif" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <%--<td style="width: 20%;" align="center">
                                    <asp:Button ID="sendToAll" runat="server" CssClass="button" OnClientClick="return confirm('Are you sure you want to send report to all employees?');"
                                        OnClick="sendToAll_Click" Text="Send Report to All Employee" />
                                </td>--%>
                            </tr>
                            <%--<tr>
                                <td style="text-align: left; width: 50%;" colspan="2">
                                    Last Download Time : &nbsp;&nbsp;
                                    <asp:Label ID="lblDownloadTime" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <td style="width: 50%; text-align: center" colspan="2">
                                    <asp:UpdateProgress ID="updProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                        DynamicLayout="true">
                                        <ProgressTemplate>
                                            <img id="imgload" alt="Please wait..." src="../images/update-progress.gif" /></ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>--%>
                        </table>
                    </fieldset>
                </div>
                <div id="middle">
                    <fieldset>
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">&nbsp;<asp:Label
                            ID="lblEmpName" runat="server" Font-Bold="true" ForeColor="#555555"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;Report
                            Month :&nbsp;&nbsp;
                            <asp:Label ID="txtReportDate" runat="server" ForeColor="#555555"></asp:Label>
                        </legend>
                        <div id="rsult" style="height: 60px;">
                            <table id="tblResult" style="border: solid 1px black; border-collapse: collapse;
                                background-color: #E2E2E2;" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 5%" align="center">
                                        <%--<asp:Label ID="lblEmpName" runat="server" Font-Bold="true" ForeColor="#555555"></asp:Label>--%>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td style="width: 8%; text-align: center;">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Avr In Time (FD)" Width="60px"></asp:Label>
                                    </td>
                                    <td style="width: 8%; text-align: center;">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Avr Out Time (FD)" Width="60px"></asp:Label>
                                    </td>
                                    <td style="width: 7%; text-align: center;">
                                        <asp:Label ID="Label6" runat="server" Font-Bold="true" Text="Avr Hr (FD)" Width="50px"></asp:Label>
                                    </td>
                                    <td style="width: 8%; text-align: center;">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Avr In Time (HD)" Width="60px"></asp:Label>
                                    </td>
                                    <td style="width: 8%; text-align: center;">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Avr Out Time (HD)" Width="60px"></asp:Label>
                                    </td>
                                    <td style="width: 8%; text-align: center;">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Avr Hr (HD)" Width="40px"></asp:Label>
                                    </td>
                                    <td style="width: 5%; text-align: center;">
                                        <asp:Label ID="lblhrs2" runat="server" Font-Bold="true" Text="0 - 4.5" Width="45px"></asp:Label>
                                    </td>
                                    <td style="width: 5%; text-align: center;">
                                        <asp:Label ID="lblhrs4" runat="server" Font-Bold="true" Text="4.5 - 6" Width="55px"></asp:Label>
                                    </td>
                                    <td style="width: 4%; text-align: center;">
                                        <asp:Label ID="lblhrs5" runat="server" Font-Bold="true" Text="6 - 7" Width="50px"></asp:Label>
                                    </td>
                                    <td style="width: 4%; text-align: center;">
                                        <asp:Label ID="lblhrs6" runat="server" Font-Bold="true" Text="7-8" Width="45px"></asp:Label>
                                    </td>
                                    <td style="width: 5%; text-align: center;">
                                        <asp:Label ID="lblhrs7" runat="server" Font-Bold="true" Text="8 - 9" Width="55px"></asp:Label>
                                    </td>
                                    <td style="width: 6%; text-align: center;">
                                        <asp:Label ID="lblhrs8" runat="server" Font-Bold="true" Text="9-10" Width="55px"></asp:Label>
                                    </td>
                                    <td style="width: 5%; text-align: center;">
                                        <asp:Label ID="lblhrs9" runat="server" Font-Bold="true" Text="10 - 11" Width="50px"></asp:Label>
                                    </td>
                                    <td style="width: 5%; text-align: center;">
                                        <asp:Label ID="lblhrs10" runat="server" Font-Bold="true" Text="11-12" Width="45px"></asp:Label>
                                    </td>
                                    <td style="width: 4%; text-align: center;">
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="&gt;12" Width="40px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        Weekdays
                                        <%--Report Date :&nbsp;&nbsp;
                                        <asp:Label ID="txtReportDate" runat="server" ForeColor="#555555"></asp:Label>--%>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWFIT" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWOTFD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWeekAveFD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWHIT" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWOTHD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblWeekAveHD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours2" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours4" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours5" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours6" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours7" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours8" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours9" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours10" runat="server" Text="0" Height="100%" Width="25px" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblHours11" runat="server" Text="0" Height="100%" Width="25px" ForeColor="#555555"></asp:Label>
                                    </td>
                                </tr>
                            <%--    <tr>
                                    <td align="center">
                                        Saturdays
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSFIT" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSOTFD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSatAvgFD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSHIT" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSOTHD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSatAvgHD" runat="server" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat2" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat4" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat5" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat6" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat7" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat8" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat9" runat="server" ForeColor="#555555" Height="100%" Text="0"
                                            Width="25px"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat10" runat="server" Text="0" Height="100%" Width="25px" ForeColor="#555555"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblSat11" runat="server" Text="0" Height="100%" Width="25px" ForeColor="#555555"></asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                        </div><br />
                        <div id="summery" runat="server" style="text-align: center">
                            <fieldset style="margin-top: 5px;">
                                Total Full Days :
                                <asp:Label ID="lblfulldays" runat="server" Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp
                                Total Half Days :
                                <asp:Label ID="lblhalfdays" runat="server" Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp
                                Total Absent Days :
                                <asp:Label ID="lblabsent" runat="server" Font-Bold="True"></asp:Label>&nbsp&nbsp&nbsp&nbsp
                                Total Working Days :
                                <asp:Label ID="wrkingdays" runat="server" Font-Bold="True"></asp:Label>
                            </fieldset>
                        </div>
                        <div>
                            <div id="griddiv">
                                <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" HorizontalAlign="Justify"
                                    OnRowDataBound="srchView_RowDataBound" Width="100%" AllowSorting="true" OnSorting="srchView_Sorting">
                                    <%--<HeaderStyle CssClass="gridheader" />
                                    <RowStyle Wrap="true" />--%>
                                    <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="17px" ForeColor="White" Height="19px" />
                                    <RowStyle Wrap="true" />
                                    <Columns>
                                        <%--<asp:ImageButton ID="ImageButton1" runat="server" CommandName="mailIt" ImageUrl="~/images/e_mail.png"
                                                    Width="20px" OnClick="ImageButton1_Click" />--%>
                                        <%--<asp:TemplateField>
                                            <HeaderTemplate>                                                
                                                  <asp:CheckBox ID="cbSelectAll" runat="server" />   
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                               <asp:CheckBox ID="cbSelectAll" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                            <HeaderStyle Width="3%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Date" SortExpression="Date" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:d}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="CheckIn" SortExpression="CheckIn" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIn" runat="server" Text='<%# Bind("CheckIn", "{0: hh:mm tt}") %>'></asp:Label>
                                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="CheckOut" SortExpression="CheckOut" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOut" runat="server" Text='<%# Bind("CheckOut", "{0:hh:mm tt}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <%--// "{0:hh:mm tt}"--%>
                                        <%-- <asp:TemplateField HeaderText="GrossTime" SortExpression="GrossTime" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGross" runat="server" Text='<%# Bind("GrossTime") %>'></asp:Label>                                              
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="Out Time" SortExpression="DurationOut" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDurationOutTime" runat="server" Text='<%# Bind("DurationOut") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In Time" SortExpression="DurationIn" HeaderStyle-ForeColor="White">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDuration" runat="server" Text='<%# Bind("DurationIn") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>--%>
                                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="CheckIn" HeaderText="In Time" SortExpression="CheckIn"
                                            DataFormatString="{0:T}" />
                                        <asp:BoundField DataField="CheckOut" HeaderText="Out Time" DataFormatString="{0:T}"
                                            SortExpression="CheckOut" />
                                        <asp:BoundField DataField="GrossTime" HeaderText="Gross Hrs" SortExpression="GrossTime" />
                                        <asp:BoundField DataField="DurationOut" HeaderText="Net Out" SortExpression="DurationOut" />
                                        <asp:BoundField DataField="DurationIn" HeaderText="Net In" SortExpression="DurationIn" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="statusImageButton" runat="server" CommandArgument='<%# Eval("Index") %>'
                                                    ImageUrl="~/images/pen(2).ico" OnClick="statusImageButton_Click" onmouseover="setstatusdiv();"
                                                    Width="15px" CausesValidation="False" Style="height: 16px" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comments" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <div style="width: 275px; word-wrap: break-word;">
                                                    <asp:Label ID="lblWorkCategory" runat="server"></asp:Label>
                                                    <asp:Label ID="lblHoliday" runat="server"></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="34%" Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Day">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLeaveCategory" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="true" />
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemStyle Width="5%" />
                                            <ItemTemplate>
                                                <div id="mydiv">
                                                    <asp:ImageButton ID="logsImageButton" runat="server" CommandArgument='<%# Eval("LogIn") + "*" + Eval("LogOut") + "*" + Eval("Index") + "*" + Eval("DurationIn")+ "*" + Eval("DurationOut")  %>'
                                                        ImageUrl="~/images/clock.ico" onmousemove="showlogdiv();" OnClick="logsImageButton_Click"
                                                        onmouseout="hidelogdiv()" Style="width: 18px;" Width="19px" CausesValidation="False" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InLog">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLog" runat="server" Text='<%# Bind("LogIn") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle CssClass="hideselect" />
                                            <FooterStyle CssClass="hideselect" />
                                            <HeaderStyle CssClass="hideselect" />
                                            <ItemStyle CssClass="hideselect" />
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Logs">
                                            <ItemStyle Width="5%" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="logsImage" runat="server" CommandArgument='<%# Eval("Date") %>'
                                                    ImageUrl="~/images/doc.png" Width="30px" Height="20px" />
                                                <%--OnClick="logsImage_Click" --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Records Found..
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                    </fieldset>
                    <div id="in_out_logs" class="logs_div">
                        <fieldset id="in_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                            padding-bottom: 5px; padding-left: 5px; float: left; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Inside Details</legend>
                            <asp:Label ID="lblDetailLogs" runat="server" Style="text-align: center" CssClass="EMS_font_small" />
                        </fieldset>
                        <fieldset id="out_logs" style="margin-right: 5px; margin-bottom: 5px; padding-bottom: 5px;
                            padding-left: 5px; float: right; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Outside Details</legend>
                            <asp:Label ID="lblOutsideDetails" runat="server" Style="text-align: center" CssClass="EMS_font_small" />
                        </fieldset>
                    </div>
                    <div id="statusDiv" class="status_div">
                        <%--onmouseout="this.style.display = 'none'"--%>
                        <fieldset id="Fieldset2" style="margin-right: 5px; margin-left: 5px; margin-bottom: 10px;
                            margin-top: 10px; padding-bottom: 5px; padding-left: 5px; width: 165px; padding-right: 5px;"
                            onmouseover="showstatusdiv();">
                            <%--onmouseout="showstatusdiv();"--%>
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Status</legend>
                            <asp:Label ID="lblCommentDate" runat="server" />
                            <div style="text-align: left" onmouseover="showstatusdiv();">
                                <%--onmouseout="showstatusdiv();"--%>
                                <%--<asp:CheckBox ID="ChkWorkFromHome" runat="server" Text="Work from home" onmouseover="showstatusdiv();" />--%>
                                <%--onmouseout="showstatusdiv();"--%><br>
                                <%-- <asp:CheckBox ID="ChkIRHr" runat="server" onmouseover="showstatusdiv();" Text="IR Hrs" /><br>
                                <asp:CheckBox ID="chkgoneout" runat="server" onmouseover="showstatusdiv();" Text="Gone Out" />--%>
                                <asp:CheckBox ID="chkcl" runat="server" Text="Came Late" onmouseover="showstatusdiv();" />
                                <br>
                                <asp:CheckBox ID="chkel" runat="server" onmouseover="showstatusdiv();" Text="Left Early" />
                                <br>
                                <asp:CheckBox ID="chkthrnot" runat="server" onmouseover="showstatusdiv();" Text="Total Hrs Not Maintained" />
                                <br>
                                <asp:CheckBox ID="chknight" runat="server" onmouseover="showstatusdiv();" Text="Night" />
                                <asp:RadioButtonList ID="StatusRBList" runat="server" CellPadding="0" CellSpacing="0"
                                    RepeatDirection="Horizontal" onmouseover="showstatusdiv();" RepeatColumns="2">
                                    <%--onmouseout="showstatusdiv();"--%>
                                    <asp:ListItem ID="itemFullLeave" runat="server" Text="Full Day " Value="0"> </asp:ListItem>
                                    <asp:ListItem ID="itemFirstHalfLeave" runat="server" Text="Half Day" Value="1"></asp:ListItem>
                                    <asp:ListItem ID="itemSecondHalfLeave" runat="server" Text="Absent" Value="2"> </asp:ListItem>
                                    <%--<asp:ListItem ID="itemNone" runat="server" Text="Night" Value="3"></asp:ListItem>--%>
                                    <%--<asp:ListItem ID="itemPlaned" runat="server" Text="Planed" Value="4"></asp:ListItem>
                                    <asp:ListItem ID="itemUnPlaned" runat="server" Text="UnPlaned" Value="5"></asp:ListItem>
                                    <asp:ListItem ID="itemNone" runat="server" Text="None" Value="3"></asp:ListItem>--%>
                                </asp:RadioButtonList>
                            </div>
                            <asp:TextBox ID="txtComment" runat="server" Height="70px" Width="96%" TextMode="MultiLine"
                                onmouseover="showstatusdiv();"> </asp:TextBox>
                            <%--onmouseout="showstatusdiv();"--%>
                            <div style="text-align: center; margin-top: 5px;" onmouseover="showstatusdiv();">
                                <%--onmouseout="showstatusdiv();"--%>
                                <asp:Button ID="btnStatusSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnStatusSubmit_Click"
                                    onmouseover="showstatusdiv();" onmouseout="showstatusdiv();" />
                                <asp:Button ID="btnClosed" runat="server" Text="Cancel" CssClass="button" OnClientClick="self.close();"
                                    onmouseover="showstatusdiv();" />
                                <%--onmouseout="showstatusdiv();"--%>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </ContentTemplate>
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
                <asp:PostBackTrigger ControlID="btnword" />
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
