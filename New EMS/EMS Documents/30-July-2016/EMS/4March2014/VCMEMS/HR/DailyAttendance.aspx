﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyAttendance.aspx.cs"
    EnableEventValidation="false" EnableViewState="true" Inherits="HR_DailyAttendance"
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
        #imgload
        {
            width: 170px;
            height: 12px;
        }
        #imgload2
        {
            height: 12px;
            width: 166px;
        }
        .linkNoUnderline a
        {
            text-decoration: none;
        }
        .LabelText
        {
            font-family: Verdana;
            font-size: 9px;
            font-weight: bold;
            color: Gray;
        }
        .ControlText
        {
            font-family: Verdana;
            font-size: 11px;
            color: Black;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function showlogdiv()
        {
            var dv = document.getElementById( "in_out_logs" );
            dv.style.pixelLeft = event.x - 380;
            dv.style.pixelTop = event.y + 15;
        }
        function OpenPopup( strURL )
        {
            window.open( strURL, "List", "scrollbars=yes,resizable=yes,width=600,height=450,top=175,left=475" );
            return false;
        }

        function SendAttach()
        {
            try
            {
                //alert(document.getElementById('mailto').value);
                var MailTo = document.getElementById( 'mailto' ).value;
                var msg = document.getElementById( 'mailtext' ).value;
                var suj = document.getElementById( 'subjectline' ).value;
                var outlookApp = new ActiveXObject( "Outlook.Application" );
                var mailItem = outlookApp.CreateItem( 0 ); // getNameSpace("MAPI");
                // mailFolder = nameSpace.getDefaultFolder(6);
                //mailItem = mailFolder.Items.add('IPM.Note.FormA');
                mailItem.Subject = suj;
                mailItem.To = MailTo; //"kpatel@thebeastapps.com"; //
                mailItem.HTMLBody = msg;
                //mailItem.BodyFormat = Html; //MailFormat.Html;
                mailItem.display( 0 );


            }
            catch ( err )
            {
                alert( err );
            }
        }

        function Check_Click( objRef )
        {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName( "input" );

            for ( var i = 0; i < inputList.length; i++ )
            {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if ( inputList[i].type == "checkbox" && inputList[i] != headerCheckBox )
                {
                    if ( !inputList[i].checked )
                    {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll( objRef )
        {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName( "input" );
            for ( var i = 0; i < inputList.length; i++ )
            {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if ( inputList[i].type == "checkbox" && objRef != inputList[i] )
                {
                    if ( objRef.checked )
                    {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        // row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else
                    {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
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
                <asp:PostBackTrigger ControlID="btnexcel" />
                <asp:PostBackTrigger ControlID="btnword" />
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField ID="mailto" runat="server" />
                <asp:HiddenField ID="mailtext" runat="server" />
                <asp:HiddenField ID="subjectline" runat="server" />
                <div id="search_grid" runat="server">
                    <fieldset style="margin-top: 5px;">
                        <table style="width: 100%;">
                            <tr>
                                <td width="3%">
                                    Date
                                </td>
                                <td width="15%">
                                    <asp:TextBox ID="dateAttendance" runat="server" Font-Bold="True" Width="145px"></asp:TextBox>
                                    <asp:CalendarExtender ID="attendaceDate" runat="server" TargetControlID="dateAttendance"
                                        Format="dd MMMM yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td width="2%">
                                    Dept.
                                </td>
                                <td width="15%">
                                    <asp:DropDownList ID="showDepartments" runat="server" CssClass="ddl" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                                        Width="168px">
                                    </asp:DropDownList>
                                </td>
                                <td width="38%">
                                    <asp:RadioButton ID="rbtnAll" runat="server" Text="All" GroupName="status" />
                                    &nbsp;<asp:RadioButton ID="rbtnIn" runat="server" Text="In" GroupName="status" />
                                    &nbsp;<asp:RadioButton ID="rbtnOut" runat="server" Text="Out" GroupName="status" />
                                    &nbsp;<asp:RadioButton ID="rbtnAbsent" runat="server" Text="Absent" GroupName="status" />
                                    &nbsp;<asp:RadioButton ID="rbtnPresent" runat="server" Text="Present" GroupName="status" />
                                    &nbsp;<asp:RadioButton ID="rbtnHalf" runat="server" Text="Half day" GroupName="status" />
                                </td>
                                <td width="17%">
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                        DynamicLayout="true">
                                        <ProgressTemplate>
                                            <img id="imgload2" alt="Please wait..." src="../images/update-progress.gif" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td style="text-align: right" width="10%">
                                    <asp:Button ID="btnShowDetails" runat="server" Text="Show Details" CssClass="button"
                                        OnClick="btnShowDetails_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <fieldset style="margin-top: 5px;">
                        <table width="100%">
                            <tr>
                                <td style="width: 110px;text-align:center;">
                                    <asp:Button ID="dwnloadLogs" runat="server" CssClass="button" OnClick="dwnloadLogs_Click"
                                        Text="Download Logs" />
                                </td>
                                <%--<asp:Button ID="btnAddlog" runat="server" Text="Add log" onclick="btnAddlog_Click" />--%>
                                <td style="width: 120px;">
                                    
                                <asp:Button ID="AssignCoff" runat="server" CssClass="button" OnClick="AssignCoff_Click"
                                        Text="Assign Comp. Off" /></td>
                                <%-- <td style="width: 130px; text-align: right">
                                    <asp:Button ID="abMail" runat="server" CssClass="button" 
                                        Text="Absent mail" onclick="abMail_Click" />                                 
                                </td> --%>
                                <td style="width: 235px;text-align:left;">
                                    <%--<asp:UpdateProgress ID="updProgress1" runat="server" 
                                    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <img id="imgload" alt="Please wait..." src="../images/update-progress.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>--%>
                                    <asp:ImageButton ID="btnOutlook" runat="server" ImageUrl="~/images/Irregularity_btn.PNG"
                                        OnClick="ImageButton2_Click" Width="150px" Height="25px" ToolTip="Send Irregularity Mail" />
                                    &nbsp; &nbsp; 
                                    <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="25px"
                                        Height="25px" OnClick="btnexcel_Click" />
                                    &nbsp &nbsp; 
                                    <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" Width="25px"
                                        Height="25px" OnClick="btnword_Click" />
                                </td>
                                <td style="width: 260px; text-align: left">
                                    <asp:Label ID="Label1" runat="server" Text="Last Download Time :"></asp:Label>
                                    <%-- </td>                           
                            <td style="width:160px"> --%>
                                    <asp:Label ID="lblDownloadTime" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="height: 1050px;">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 5px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowDataBound="srchView_RowDataBound"
                                AllowSorting="true" OnSorting="srchView_Sorting" OnSelectedIndexChanged="srchView_SelectedIndexChanged"
                                PageSize="40" Width="100%" OnRowCommand="srchView_RowCommand" AllowPaging="false"
                                OnPageIndexChanging="srchView_PageIndexChanging">
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                    Font-Size="17px" ForeColor="White" Height="19px" />
                                <RowStyle Wrap="true" />
                                <Columns>
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="chkSendMail" runat="server" ImageUrl="~/images/e_mail.png" CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClick="chkSendMail_Click" Width="18px" Style="height: 18px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("machine_code") %>' Visible="False"></asp:Label>
                                            <asp:ImageButton ID="onlineImage" runat="server" ImageUrl="~/images/yellow_light.png"
                                                Visible="False" Width="16px" />
                                            <asp:ImageButton ID="offlineImage" runat="server" ImageUrl="~/images/black_light.png"
                                                Visible="False" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="empName" HeaderText="Employee" SortExpression="empName"
                                        ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="deptName" HeaderText="Department" SortExpression="deptName"
                                        ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="intime" HeaderText="In Time" SortExpression="intime" ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="outtime" HeaderText="Out Time" SortExpression="outtime"
                                        ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="GrossTime" HeaderText="Gross Hrs" SortExpression="GrossTime"
                                        ItemStyle-Width="6%" />
                                    <asp:BoundField DataField="DurationOUT" HeaderText="Net Out" SortExpression="DurationOUT"
                                        ItemStyle-Width="6%" />
                                    <asp:BoundField DataField="DurationIn" HeaderText="Net In" SortExpression="DurationIn"
                                        ItemStyle-Width="6%" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="statusImageButton" runat="server" ImageUrl="~/images/pen(2).ico"
                                                Width="16px" onmouseover="setstatusdiv();" CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClick="statusImageButton_Click" Style="height: 16px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments" ItemStyle-Wrap="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkCategory" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="true" Width="25%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Day">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCategory" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="true" Width="5%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="logsImageButton" runat="server" CommandArgument='<%# Eval("empId") + "," + Eval("empName") %>'
                                                ImageUrl="~/images/clock.ico" OnClick="logsImageButton_Click" onmouseout="hidelogdiv()"
                                                onmouseover="showlogdiv();" Style="width: 18px;" Width="18px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Emp ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("empId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" Width="2%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approved">
                                        <ItemTemplate>
                                            <asp:Button ID="btnApproved" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                onmouseover="appsetstatusdiv();" Style="height: 20px" Text=" " Width="50px" OnClick="btnApproved_Click" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnApproved" runat="server" ImageUrl="~/images/pen(2).ico"
                                                Width="16px" onmouseover="setstatusdiv();" CommandArgument="<%# Container.DataItemIndex %>"
                                                OnClick="statusImageButton_Click" Style="height: 16px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Logs">
                                        <ItemStyle Width="5%" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="logsImage" runat="server" ImageUrl="~/images/doc.png" Width="30px"
                                                Height="20px" />
                                            <%--OnClick="logsImage_Click" --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblempty" runat="server" Text=""></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Records Found..
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                        <div id="statusDiv" class="status_div">
                            <fieldset id="Fieldset2" style="margin-right: 5px; margin-left: 5px; margin-bottom: 10px;
                                margin-top: 10px; padding-bottom: 5px; padding-left: 5px; width: 165px; padding-right: 5px;"
                                onmouseover="showstatusdiv();">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Status
                                </legend>
                                <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblCommentDate" runat="server"></asp:Label>
                                <div style="text-align: left" onmouseover="showstatusdiv();">
                                    <asp:CheckBox ID="chkcl" runat="server" Text="Came Late" onmouseover="showstatusdiv();" />
                                    <br>
                                    <asp:CheckBox ID="chkel" runat="server" onmouseover="showstatusdiv();" Text="Left Early" />
                                    <br>
                                    <asp:CheckBox ID="chkthrnot" runat="server" onmouseover="showstatusdiv();" Text="Total Hrs Not Maintained" />
                                    <br>
                                    <asp:CheckBox ID="chknight" runat="server" onmouseover="showstatusdiv();" Text="Night" />
                                    <asp:RadioButtonList ID="StatusRBList" runat="server" CellPadding="0" CellSpacing="0"
                                        onmouseover="showstatusdiv();" RepeatDirection="Horizontal" RepeatColumns="2">
                                        <asp:ListItem ID="itemFullLeave" runat="server" Text="Full Day " Value="0"></asp:ListItem>
                                        <asp:ListItem ID="itemFirstHalfLeave" runat="server" Text="Half Day" Value="1"></asp:ListItem>
                                        <asp:ListItem ID="itemSecondHalfLeave" runat="server" Text="Absent" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:TextBox ID="txtComment" runat="server" Height="70px" Width="96%" TextMode="MultiLine"
                                    onmouseover="showstatusdiv();"></asp:TextBox>
                                <div style="text-align: center; margin-top: 5px;" onmouseover="showstatusdiv();">
                                    <asp:Button ID="btnStatusSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnStatusSubmit_Click"
                                        onmouseover="showstatusdiv();" />
                                    <asp:Button ID="btnClosed" runat="server" Text="Cancel" CssClass="button" OnClientClick="self.close();"
                                        onmouseover="showstatusdiv();" />
                                </div>
                            </fieldset>
                        </div>
                        <div id="appstatusDiv" class="status_div">
                            <fieldset id="fld" style="margin-right: 5px; margin-left: 5px; margin-bottom: 10px;
                                margin-top: 10px; padding-bottom: 5px; padding-left: 5px; width: 165px; padding-right: 5px;"
                                onmouseover="appshowstatusdiv();">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Approved/Reject
                                </legend>
                                <asp:Label ID="lblemp" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblDate" runat="server"></asp:Label>
                                <div style="text-align: left" onmouseover="appshowstatusdiv();">
                                    <asp:RadioButtonList ID="rblapprove" runat="server" CellPadding="0" CellSpacing="0"
                                        onmouseover="appshowstatusdiv();" RepeatDirection="Vertical">
                                        <asp:ListItem ID="liApp" runat="server" Text="Approved " Value="0"></asp:ListItem>
                                        <asp:ListItem ID="liRej" runat="server" Text="Reject" Value="1"></asp:ListItem>
                                        <%--<asp:ListItem ID="lipen" runat="server" Text="Pending" Value="2"></asp:ListItem>--%>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:TextBox ID="txtAppCom" runat="server" Height="70px" Width="96%" TextMode="MultiLine"
                                    onmouseover="appshowstatusdiv();"> </asp:TextBox>
                                <div style="text-align: center; margin-top: 5px;" onmouseover="appshowstatusdiv();">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Submit" CssClass="button" OnClick="btnUpdate_Click"
                                        onmouseover="appshowstatusdiv();" />
                                    <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="button" OnClientClick="self.close();"
                                        onmouseover="appshowstatusdiv();" />
                                </div>
                            </fieldset>
                        </div>
                    </fieldset>
                    <div id="in_out_logs" class="logs_div">
                        <div>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblname" runat="server" Text="" ForeColor="Black"></asp:Label>
                            <asp:Label ID="logdate" runat="server" Text=""></asp:Label>
                        </div>
                        <fieldset id="in_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                            padding-bottom: 5px; padding-left: 5px; float: left; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Inside Details
                            </legend>
                            <asp:Label ID="lblDetailLogs" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                        <fieldset id="out_logs" style="margin-right: 5px; margin-bottom: 5px; padding-bottom: 5px;
                            padding-left: 5px; float: right; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Outside Details
                            </legend>
                            <asp:Label ID="lblOutsideDetails" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
