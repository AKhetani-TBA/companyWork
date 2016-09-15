<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PLPolicy.aspx.cs" Inherits="HR_PLPolicy"
    EnableViewState="true" EnableEventValidation="false" Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

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
        }
   
    </script>

    <script type="text/javascript">

        function isNumeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            { return false; }
            else { return true; }
        }


    </script>

    <style type="text/css">
        .style14
        {
            width: 10px;
        }
        .style15
        {
            width: 62px;
        }
        .style16
        {
            width: 100px;
        }
        .style18
        {
            width: 174px;
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
                <table align="left" style="width: 100%; height: 100%">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ins"
                                ShowSummary="false" ShowMessageBox="true" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <div>
                                <div>
                                    <fieldset style="margin-top: 5px; width: 340px; direction: inherit; height: 55px">
                                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search:</legend>
                                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtStartDateSearch" runat="server" ValidationGroup="search" Width="70px"
                                                            AutoPostBack="True" OnTextChanged="txtStartDateSearch_TextChanged" />
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="imsEndDateSearch"
                                                            TargetControlID="txtStartDateSearch">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imsEndDateSearch" runat="server" BorderStyle="None" Height="20px"
                                                            ImageUrl="~/images/calIcon.png" ToolTip="" ImageAlign="Baseline" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEndDateSearch" runat="server" ValidationGroup="search" Width="70px" />
                                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="Image1"
                                                            TargetControlID="txtEndDateSearch">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="Image1" runat="server" BorderStyle="None" Height="20px" ImageUrl="~/images/calIcon.png"
                                                            ToolTip="" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="imgGvSearch" runat="server" CssClass="button" ImageUrl="~/images/searchbtn.png"
                                                            OnClick="imgGvSearch_Click" ValidationGroup="search" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </div>
                                <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto">
                                    <fieldset style="margin-top: 5px; width: 340px; direction: inherit; height: 450px">
                                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Available
                                            Policy Template(s). </legend>
                                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                            <asp:GridView ID="gvPolicyTemp" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPolicyTemp_RowCommand"
                                                OnRowDataBound="gvPolicyTemp_RowDataBound" OnSelectedIndexChanged="gvPolicyTemp_SelectedIndexChanged"
                                                Width="98%" AllowPaging="True" OnPageIndexChanging="gvPolicyTemp_PageIndexChanging"
                                                PageSize="15" HeaderStyle-CssClass="gridheader"  ToolTip="Leave policy information...">
                                                <RowStyle BorderColor="#333333" />
                                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                <EmptyDataTemplate>
                                                    No Record(s) found it...</EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGvLeaveTempID" runat="server" Text='<%# Bind("LeaveTempID") %>'
                                                                Visible="false" />
                                                            <asp:Label ID="lblGvLeaveTypeId" runat="server" Text='<%# Bind("LeaveTypeId") %>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="16px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LeaveTempName" Visible="false" HeaderText="Policy Name"
                                                        ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LeaveType" HeaderText="Leave Type" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" ItemStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" ItemStyle-HorizontalAlign="Center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField ButtonType="Image" Text="View" CommandName="SELECT" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-ForeColor="Black" ImageUrl="~/images/edit_btn.png">
                                                        <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                    <asp:ButtonField ButtonType="Image" Text="Copy" CommandName="COPY" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-ForeColor="Black" ImageUrl="~/images/copy.png">
                                                        <ItemStyle ForeColor="Black" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <HeaderStyle CssClass="gridheader" />
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                        </td>
                        <td valign="top" align="left">
                            <div id="divAddnew" runat="server" visible="true">
                                <fieldset style="margin-top: 5px; width: 550px; direction: inherit; height: 50px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add Leave
                                        Policy</legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <asp:Button ID="btnAddNew" runat="server" CssClass="button" OnClick="btnAddNew_Click1"
                                             Text="Add Leave Policy" />
                                    </div>
                                </fieldset>
                            </div>
                            <div id="divRules" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 550px; direction: inherit; height: 235px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add Leave
                                        Policy</legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <table width="100%">
                                            <tr>
                                                <td align="right" class="style18">
                                                    <asp:Label ID="Label1" runat="server" Text="Policy Name:" Width="100px" Style="font-weight: 700"></asp:Label>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtPolicyName" runat="server" MaxLength="100" Width="225px" 
                                                        ToolTip="Please enter leave policy name upto 100 char..."></asp:TextBox>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPolicyName"
                                                        ErrorMessage="Policy Name is required..." ValidationGroup="ins">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" class="style18">
                                                    <b>&nbsp;Description: </b>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:TextBox ID="txtLeavePolicyDesc" runat="server" Width="225px" MaxLength="250"
                                                        Rows="4" TextMode="MultiLine"  ToolTip="Please enter description upto 250 char"></asp:TextBox>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Policy Description is required..."
                                                        ValidationGroup="ins" ControlToValidate="txtLeavePolicyDesc">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style18">
                                                    <b>Leave Type:</b>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:DropDownList ID="drpLeaveTypeIns" runat="server" CssClass="ddl" Height="21px"
                                                        Width="230px"  ValidationGroup="ins">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" class="style14">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select valid Leave type..."
                                                        ValidationGroup="ins" ControlToValidate="drpLeaveTypeIns" InitialValue="0">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style18">
                                                    <b>Start &amp; End Date: </b>
                                                </td>
                                                <td align="left" valign="middle" style="width: 125px">
                                                    <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="ins" MaxLength="10"
                                                        Width="80px"  AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged" />
                                                    <asp:Image ID="imgStartDate" runat="server" BorderStyle="None" Height="17px" ImageUrl="~/images/calIcon.png"
                                                        ToolTip="Please click here to select policy start date..." />
                                                </td>
                                                <td style="width: 125px">
                                                    <asp:CalendarExtender ID="attendaceDate" runat="server" PopupPosition="Right" Animated="true"
                                                        Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                                    </asp:CalendarExtender>
                                                    <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10"  ValidationGroup="ins"
                                                        Width="80px" OnTextChanged="txtEndDate_TextChanged" />
                                                    <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                        PopupButtonID="imsEndDate" TargetControlID="txtEndDate">
                                                    </asp:CalendarExtender>
                                                    <asp:Image ID="imsEndDate" runat="server" BorderStyle="None" Height="17px" ImageUrl="~/images/calIcon.png"
                                                        ToolTip="Please click here to select policy end date..." ImageAlign="Baseline" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="style18">
                                                    <b>Carry Forward? </b>
                                                </td>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkIsApplicable" runat="server" Text="" ToolTip="Click here to apply applicable rules..."
                                                                     AutoPostBack="True" OnCheckedChanged="chkIsApplicable_CheckedChanged" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" Text="MaxAllouced:" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaxAllouced" runat="server" MaxLength="3" Width="30px" CssClass="txtclass"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Min Leave For One Month:"
                                                        Width="170px"></asp:Label>
                                                </td>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtMinLeavePM" runat="server" CssClass="txtclass" MaxLength="5"
                                                                     Text="00.00" ValidationGroup="ins" Width="45px" />
                                                            </td>
                                                            <td align="right">
                                                                &nbsp;
                                                                <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Max Leaves For Period:"
                                                                    Width="170px" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtMaxLeavePP" runat="server" AutoPostBack="True" CssClass="txtclass"
                                                                    MaxLength="5"  Text="00.00" ValidationGroup="ins" Width="45px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="divRulesDtl" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 550px; direction: inherit; height: 220px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <asp:Panel ID="pnlPLLeave1" runat="server" Height="220px" ScrollBars="Auto">
                                            <table id="pnlPLLeave" runat="server" visible="false" width="80%" align="center">
                                                <tr>
                                                    <td align="right">
                                                        <asp:ImageButton ID="imgEmptyAdd" runat="server" CommandName="EmptySave" Height="20px"
                                                            ImageUrl="~/images/add.png" OnClick="imgEmptyAdd_Click" Width="20px" />
                                                        <asp:ImageButton ID="imgRemoveGV" runat="server" CommandName="EmptySave" Height="22px"
                                                            ImageUrl="~/images/gnome_edit_delete.png" OnClick="imgRemoveGV_Click" Width="22px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="gvPLRules" runat="server" AutoGenerateColumns="false" OnRowCommand="gvPLRules_RowCommand"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRuleID" runat="server" Text='<%# Bind("RuleID") %>' Width="80px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Start Date">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPLStartDate" runat="server" Text='<%# Bind("StartDate") %>' Width="80px" />
                                                                        <asp:CalendarExtender ID="plattendaceDatee" runat="server" Animated="true" Format="dd/MM/yyyy"
                                                                            PopupButtonID="txtPLStartDate" PopupPosition="Right" TargetControlID="txtPLStartDate">
                                                                        </asp:CalendarExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="End Date">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPLEndDate" runat="server" Text='<%# Bind("EndDate") %>' Width="80px" />
                                                                        <asp:CalendarExtender ID="plattendaceDate" runat="server" Animated="true" Format="dd/MM/yyyy"
                                                                            PopupButtonID="txtPLEndDate" PopupPosition="Right" TargetControlID="txtPLEndDate">
                                                                        </asp:CalendarExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="WorkingDays">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPLWorkingDays" runat="server" CssClass="txtclass" Text='<%# Bind("RequiredWorkDays") %>'
                                                                            Width="40px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Leaves">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPLLeave" runat="server" CssClass="txtclass" Text='<%# Bind("Leave") %>'
                                                                            Width="40px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgplremove" runat="server" CommandName="select" ImageUrl="~/images/gnome_edit_delete.png"
                                                                            OnClick="imgplremove_Click" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                </fieldset>
                            </div>
                            <div id="divSavebutton" runat="server" visible="false">
                                <fieldset style="margin-top: 5px; width: 550px; direction: inherit; height: 40px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                        <asp:Button ID="btnSubmit" runat="server" CssClass="button" OnClick="btnSubmit_Click"
                                             Text="Save" ValidationGroup="ins" />
                                        <asp:Button ID="btnReset" runat="server" CssClass="button" OnClick="btnShowDetails_Click"
                                             Text="Reset" ToolTip="Click here to add new record or reset the page..." />
                                        <br />
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="lblLeaveTempId" runat="server" EnableViewState="true" Value="0"
                            Visible="False" />
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
