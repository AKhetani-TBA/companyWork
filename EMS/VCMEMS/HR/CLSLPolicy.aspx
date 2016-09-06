<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CLSLPolicy.aspx.cs" Inherits="HR_CLSLPolicy"
    EnableViewState="true" EnableEventValidation="false" Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script language="Javascript" type="text/javascript">
        function isNumeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            { return false; }

            return true;
        };

        function isNumeric_OldwithValidation(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            { return false; }
            else {
                var colsum = 0;
                var grid = document.getElementById('gvRules');
                var MaxLeave_a = grid.getElementsByTagName('INPUT');
                var MaxLeave = document.getElementById('txtMaxLeavePP').value;

                var totalLenth = MaxLeave_a.length;
                var totallenthp = MaxLeave_a.length / 13;

                for (var j = 1; j <= totallenthp; j++) {
                    for (var i = 0; i < MaxLeave_a.length; i++) {
                        if (i == ((j * 13) - 1)) {
                            colsum = colsum + parseFloat(MaxLeave_a[i].value);
                        }
                    }
                }

                var total = parseFloat(colsum);
                if (total > MaxLeave) {
                    return false;
                }

            }

            return true;
        };




        //--> </script>

    <script type="text/javascript">
        function checkDt(sender, args) {
            var StartDate = document.getElementById('txtStartDate').value;
            var EndDate = document.getElementById('txtEnddate').value;
            var eDate = new Date(EndDate);
            var sDate = new Date(StartDate);
            //            if (StartDate != '' && EndDate != '' && sDate > eDate)

            if (EndDate < StartDate) {
                alert("Please ensure that the To Date is greater than or equal to the From Date.");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
                return false;
            }
        };

        function TotalLeave_new(jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, total) {

            var vjan = document.getElementById(jan);
            var vfeb = document.getElementById(feb);
            var vmar = document.getElementById(mar);
            var vapr = document.getElementById(apr);
            var vmay = document.getElementById(may);
            var vjun = document.getElementById(jun);
            var vjul = document.getElementById(jul);
            var vaug = document.getElementById(aug);
            var vsep = document.getElementById(sep);
            var voct = document.getElementById(oct);
            var vnov = document.getElementById(nov);
            var vdec = document.getElementById(dec);
            var MaxLeave = document.getElementById('txtMaxLeavePP').value;
            var tt = (parseFloat(vjan.value) + parseFloat(vfeb.value) + parseFloat(vmar.value) + parseFloat(vapr.value) + parseFloat(vmay.value) + parseFloat(vjun.value) + parseFloat(vjul.value) + parseFloat(vaug.value) + parseFloat(vsep.value) + parseFloat(voct.value) + parseFloat(vnov.value) + parseFloat(vdec.value));

            var colsum = 0;
            var grid = document.getElementById('gvRules');
            var MaxLeave_a = grid.getElementsByTagName('INPUT');

            var totalLenth = MaxLeave_a.length;
            var totallenthp = MaxLeave_a.length / 13;

            for (var j = 1; j <= totallenthp; j++) {
                for (var i = 0; i < MaxLeave_a.length; i++) {
                    if (i == ((j * 13) - 1)) {
                        colsum = colsum + parseFloat(MaxLeave_a[i].value);
                    }
                }
            }


            var RemLeave = parseFloat(MaxLeave) - parseFloat(colsum);

            document.getElementById('lblRemLeave').value = RemLeave.toPrecision(2);

            document.getElementById(total).value = tt.toPrecision(2);

        };

       
    </script>

    <script language="Javascript" type="text/javascript">
        
      
    </script>

    <style type="text/css">
        .style10
        {
            height: 20px;
            font-family: Microsoft Sans Serif;
            font-size: 12px;
            vertical-align: top;
            width: 72px;
        }
        .style14
        {
            width: 10px;
        }
        .style16
        {
            width: 100px;
        }
        .style20
        {
            width: 184px;
        }
        .style21
        {
            width: 30%;
        }
        .style24
        {
            width: 415px;
        }
        #Div3
        {
            width: 111px;
        }
        .style25
        {
            width: 45px;
        }
        .style26
        {
            width: 146px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
            </Triggers>--%>
            <ContentTemplate>
                <table align="left" style="width: 100%">
                    <tr>
                        <td align="center" colspan="3">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ins"
                                ShowSummary="false" ShowMessageBox="true" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style21">
                            <div>
                                <div id="Div6" runat="server" style="overflow-y: auto; overflow-x: auto">
                                    <fieldset style="margin-top: 5px; width: 330px; direction: inherit; height: 55px">
                                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search:</legend>
                                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtStartDateSearch" runat="server" ValidationGroup="search" Width="70px"
                                                            AutoPostBack="True" OnTextChanged="txtStartDateSearch_TextChanged" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDt"
                                                            PopupButtonID="imsEndDateSearch" TargetControlID="txtStartDateSearch">
                                                        </asp:CalendarExtender>
                                                        <asp:Image ID="imsEndDateSearch" runat="server" BorderStyle="None" Height="20px"
                                                            ImageUrl="~/images/calIcon.png" ToolTip="" ImageAlign="Baseline" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txtEndDateSearch" runat="server" ValidationGroup="search" Width="70px" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDt"
                                                            PopupButtonID="Image1" TargetControlID="txtEndDateSearch">
                                                        </asp:CalendarExtender>
                                                        <asp:Image ID="Image1" runat="server" BorderStyle="None" Height="20px" ImageUrl="~/images/calIcon.png"
                                                            ToolTip="" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgGvSearch" runat="server" CssClass="button" ImageUrl="~/images/searchbtn.png"
                                                            OnClick="imgGvSearch_Click" ValidationGroup="search" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </div>
                                <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto;">
                                    <fieldset style="margin-top: 5px; width: 330px; direction: inherit; height: 465px">
                                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Available
                                            Policy:</legend>
                                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        </div>
                                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                            <asp:GridView ID="gvPolicyTemp" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPolicyTemp_RowCommand"
                                                OnRowDataBound="gvPolicyTemp_RowDataBound" OnSelectedIndexChanged="gvPolicyTemp_SelectedIndexChanged"
                                                Width="100%" AllowPaging="True" OnPageIndexChanging="gvPolicyTemp_PageIndexChanging"
                                                PageSize="15" BorderColor="Black" RowStyle-BorderColor="Black"  HeaderStyle-CssClass="gridheader" ToolTip="Leave policy information...">
                                                <RowStyle BorderColor="#333333" />
                                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                <EmptyDataTemplate>
                                                    No Record(s) found it...</EmptyDataTemplate>
                                                 <HeaderStyle BorderColor="Black" BorderWidth="1px"/>   
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-BorderColor="Black">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGvLeaveTempID" runat="server" Text='<%# Bind("LeaveTempID") %>'
                                                                Visible="false" />
                                                            <asp:Label ID="lblGvLeaveTypeId" runat="server" Text='<%# Bind("LeaveTypeId") %>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="16px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LeaveTempName" HeaderText="Policy Name" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" BorderColor="Black" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LeaveType" Visible="false" HeaderText="Leave Type" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" BorderColor="Black" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" ItemStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" BorderColor="Black" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" ItemStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" BorderColor="Black" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" Text="View"  CommandName="SELECT" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-ForeColor="Black" ImageUrl="~/images/edit_btn.png">
                                                        <ItemStyle ForeColor="Black" HorizontalAlign="Center" BorderColor="Black"/>
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image" Text="Copy" CommandName="COPY" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-ForeColor="Black" ImageUrl="~/images/copy.png">
                                                        <ItemStyle ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <HeaderStyle CssClass="gridheader" />
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </td>
                        <td valign="top" align="left" class="style24" colspan="2">
                            <div id="divAddbutton" runat="server" visible="true">
                                <fieldset style="margin-top: 5px; width: 590px; direction: inherit; height: 55px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add New Policy</legend>
                                    <div>
                                        <asp:Button ID="btnAddNew" runat="server" Text="Add New Policy" Height="24px" CssClass="button"
                                            OnClick="btnAddNew_Click1" /></div>
                                </fieldset>
                            </div>
                            <div id="divrules" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 590px; direction: inherit; height: 285px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add New Policy</legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <table width="100%">
                                            <tr>
                                                <td align="right" class="style20">
                                                    <asp:Label ID="Label1" runat="server" Text="Policy Name:" Width="100px" Style="font-weight: 700"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtPolicyName" runat="server" MaxLength="100" Width="250px" ToolTip="Please enter leave policy name upto 100 char..."></asp:TextBox>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPolicyName"
                                                        ErrorMessage="Policy Name is required..." ValidationGroup="ins">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" class="style20">
                                                    <b>&nbsp;Description: </b>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtLeavePolicyDesc" runat="server" Width="250px" MaxLength="250"
                                                        Rows="4" TextMode="MultiLine" ToolTip="Please enter description upto 250 char"></asp:TextBox>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Policy Description is required..."
                                                        ValidationGroup="ins" ControlToValidate="txtLeavePolicyDesc">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style20">
                                                    <b>Leave Type:</b>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:DropDownList ID="drpLeaveTypeIns" runat="server" CssClass="ddl" Height="21px"
                                                        Width="255px" ValidationGroup="ins">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select valid Leave type..."
                                                        ValidationGroup="ins" ControlToValidate="drpLeaveTypeIns" InitialValue="0">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style20">
                                                    <b>Start &amp; End Date: </b>
                                                </td>
                                                <td align="left" valign="middle" style="width: 135px" class="style26">
                                                    <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="ins" MaxLength="10"
                                                        Width="80px" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="True" />
                                                    <asp:Image ID="imgStartDate" runat="server" BorderStyle="None" Height="17px" ImageUrl="~/images/calIcon.png"
                                                        ToolTip="Please click here to select policy start date..." />
                                                </td>
                                                <td style="width: 135px">
                                                    <asp:CalendarExtender ID="attendaceDate" runat="server" PopupPosition="Right" Animated="true"
                                                        Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDt" TargetControlID="txtStartDate"
                                                        PopupButtonID="imgStartDate">
                                                    </asp:CalendarExtender>
                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" OnTextChanged="txtEndDate_TextChanged"
                                                        ValidationGroup="ins" Width="80px" />
                                                    <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDt" PopupButtonID="imsEndDate" TargetControlID="txtEndDate">
                                                    </asp:CalendarExtender>
                                                    <asp:Image ID="imsEndDate" runat="server" BorderStyle="None" Height="17px" ImageUrl="~/images/calIcon.png"
                                                        ToolTip="Please click here to select policy end date..." ImageAlign="Baseline" />
                                                </td>
                                                <td>
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txtEndDate"
                                                        ControlToValidate="txtStartDate" ErrorMessage="End date must be Grether then start date..."
                                                        Operator="GreaterThan" Type="Date" ValidationGroup="ins">*</asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style20">
                                                    <b>Carry Forward? </b>
                                                </td>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkIsApplicable" runat="server" Text="" Checked="true" ToolTip="Click here to apply applicable rules..."
                                                                    AutoPostBack="True" OnCheckedChanged="chkIsApplicable_CheckedChanged" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" Text="MaxAllouced:" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaxAllouced" runat="server" MaxLength="5" Width="45px" CssClass="txtclass">00.00</asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trRule1" runat="server">
                                                <td align="right" class="style20">
                                                    <b>If Joining Date:</b>
                                                </td>
                                                <td align="left" class="style10" valign="middle" colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRulesTo" runat="server" MaxLength="2" Text="00" Width="30px"
                                                                    ToolTip="please enter day upto 31 number" ValidationGroup="ins" CssClass="txtclass" />
                                                            </td>
                                                            <td>
                                                                <b>To</b>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRulesFrom" runat="server" MaxLength="2" Text="00" Width="30px"
                                                                    ToolTip="please enter day upto 31 number" ValidationGroup="ins" CssClass="txtclass" />
                                                            </td>
                                                            <td>
                                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtRulesTo"
                                                                    ErrorMessage="Day To must be between 1 To 31 &amp; Less Than Day From." MaximumValue="31"
                                                                    MinimumValue="1" Type="Integer" ValidationGroup="ins">*</asp:RangeValidator>
                                                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txtRulesTo"
                                                                    ControlToValidate="txtRulesFrom" ErrorMessage="Day From must be Greater Than Day To."
                                                                    Operator="GreaterThan" Text="*" Type="Integer" ValidationGroup="ins" />
                                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtRulesFrom"
                                                                    ErrorMessage="Day To must be between 1 To 31 &amp; Grether Than Day From." MaximumValue="31"
                                                                    MinimumValue="1" Type="Integer" ValidationGroup="ins">*</asp:RangeValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" class="style14">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trRule11" runat="server">
                                                <td align="right" class="style20">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Min Leave For One Month:"
                                                        Width="170px"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td align="left" class="style25">
                                                                <asp:TextBox ID="txtMinLeavePM" runat="server" MaxLength="5" Text="00.00" Width="45px"
                                                                    ValidationGroup="ins" CssClass="txtclass" />
                                                            </td>
                                                            <td align="right">
                                                                &nbsp;
                                                                <asp:Label ID="lbl" runat="server" Text="Max Leaves For Period:" Font-Bold="true"
                                                                    Width="170px" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtMaxLeavePP" runat="server" MaxLength="5" Text="00.00" ValidationGroup="ins"
                                                                    Width="45px" AutoPostBack="True" OnTextChanged="txtMaxLeave_TextChanged" CssClass="txtclass" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trRule1111" runat="server">
                                                <td align="right" class="style20">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Set Remaining Leave:"
                                                        Width="145px"></asp:Label>
                                                </td>
                                                <td align="left" class="style26">
                                                    <asp:TextBox ID="lblRemLeave" runat="server" Text="0.0" BorderWidth="0" Font-Bold="True"
                                                        Font-Size="Medium" ForeColor="Red" Width="40px" BorderColor="Red" Enabled="False"
                                                        EnableViewState="False" />
                                                </td>
                                                <td class="style16" align="right">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="divrulesdtl" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 590px; direction: inherit; height: 170px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <asp:Panel ID="pnlgv" runat="server" Width="100%" Height="100%" ScrollBars="Auto">
                                            <asp:GridView ID="gvRules" runat="server" Width="100%" AutoGenerateColumns="false"
                                                OnRowDataBound="gvRules_RowDataBound" EmptyDataText="Record(s) no found it.">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Year" DataField="YY" />
                                                    <asp:TemplateField HeaderText="Jan" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvJan" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Jan") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Feb" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvFeb" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Feb") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mar" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvMar" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Mar") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Apr" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvApr" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Apr") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="May" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvMay" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("May") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Jun" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvJun" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Jun") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Jul" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvJul" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Jul") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Aug" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvAug" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Aug") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sep" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvSep" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Sep") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Oct" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvOct" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Oct") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nov" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvNov" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Nov") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dec" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvDec" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Dec") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="gvTotal" runat="server" CssClass="txtclass" Width="35px" MaxLength="5"
                                                                Text='<%# Bind("Total") %>'> ></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="divSavebutton" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 590px; direction: inherit; height: 50px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="button" ValidationGroup="ins"
                                            OnClick="btnSubmit_Click" />
                                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" OnClick="btnShowDetails_Click"
                                            ToolTip="Click here to add new record or reset the page..." />
                                        <br />
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="lblLeaveTempId" runat="server" EnableViewState="true" Value="0"
                                Visible="False" />
                        </td>
                        <td align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="lblRuleId" runat="server" Value="0" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
