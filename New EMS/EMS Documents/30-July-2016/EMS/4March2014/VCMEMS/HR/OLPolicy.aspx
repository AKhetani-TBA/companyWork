<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OLPolicy.aspx.cs" Inherits="HR_OLPolicy"
    EnableViewState="true" EnableEventValidation="false" Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <script type="text/javascript">

        function isNumeric(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
            { return false; }
            else { return true; }
        }


    </script>

    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .style14
        {
            width: 400px;
        }
        .style24
        {
            width: 17%;
        }
        .style26
        {
            width: 256px;
        }
        .style28
        {
            width: 256px;
            font-weight: bold;
        }
        .style29
        {
            width: 327px;
        }
        .style30
        {
            width: 85px;
        }
        .style32
        {
            width: 85px;
            font-weight: bold;
        }
        .style33
        {
            width: 125px;
        }
        .style35
        {
            width: 175px;
        }
        .style36
        {
            width: 80px;
        }
    </style>
</head>
<body style="width: 90px">
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
            </Triggers>--%>
            <ContentTemplate>
                <div>
                    <table align="left" style="width: 100%">
                        <tr>
                            <td class="style14">
                                <div>
                                    <div id="Div6" runat="server" style="overflow-y: auto; overflow-x: auto">
                                        <fieldset style="margin-top: 5px; width: 340px; direction: inherit; height: 50px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search:</legend>
                                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtStartDateSearch" runat="server" ValidationGroup="search" Width="90px"
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
                                                            <asp:TextBox ID="txtEndDateSearch" runat="server" ValidationGroup="search" Width="90px" />
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
                                        <fieldset style="margin-top: 5px; width: 340px; direction: inherit; height: 360px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Available
                                                Policy:</legend>
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
                                                        <asp:BoundField DataField="LeaveTempName" HeaderText="Policy Name" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LeaveType" Visible="false" HeaderText="Leave Type" ItemStyle-HorizontalAlign="Left">
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
                            <td align="left" valign="top">
                                <div>
                                    <div id="divAddNew" runat="server" visible="true">
                                        <fieldset style="margin-top: 5px; width: 580px; direction: inherit; height: 50px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add Leave
                                                Policy</legend>
                                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                                <asp:Button ID="btnAddNew" runat="server" Text="Add Leave Policy" CssClass="button"
                                                    OnClick="btnAddNew_Click1" />
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div id="divRules" runat="server" visible="false">
                                        <fieldset style="margin-top: 5px; width: 580px; direction: inherit; height: 270px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add Leave
                                                Policy</legend>
                                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td align="right" class="style26">
                                                            <asp:Label ID="Label1" runat="server" Text="Policy Name:" Width="100px" Style="font-weight: 700"></asp:Label>
                                                        </td>
                                                        <td align="left" class="style29">
                                                            <asp:TextBox ID="txtPolicyName" runat="server" MaxLength="100" Width="230px"
                                                                ToolTip="Please enter leave policy name upto 100 char..."></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="style24">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPolicyName"
                                                                ErrorMessage="Policy Name is required..." ValidationGroup="ins">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top" class="style26">
                                                            <b>Description:</b>
                                                        </td>
                                                        <td align="left" class="style29">
                                                            <asp:TextBox ID="txtLeavePolicyDesc" runat="server" Width="230px" MaxLength="250"
                                                                Rows="4" TextMode="MultiLine" 
                                                                ToolTip="Please enter description upto 250 char"></asp:TextBox>
                                                        </td>
                                                        <td align="left" class="style24">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Policy Description is required..."
                                                                ValidationGroup="ins" ControlToValidate="txtLeavePolicyDesc">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style26">
                                                            <b>Leave Type:</b>
                                                        </td>
                                                        <td align="left" class="style29">
                                                            <asp:DropDownList ID="drpLeaveTypeIns" runat="server" CssClass="ddl" Height="21px"
                                                                Width="235px" ValidationGroup="ins">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" class="style24">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Select valid Leave type..."
                                                                ValidationGroup="ins" ControlToValidate="drpLeaveTypeIns" InitialValue="0">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style26">
                                                            <b>Start &amp; End Date: </b>
                                                        </td>
                                                        <td class="style29">
                                                            <table>
                                                                <tr>
                                                                    <td align="left" valign="middle">
                                                                        <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="ins" MaxLength="10"
                                                                            Width="80px" OnTextChanged="txtStartDate_TextChanged" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Image ID="imgStartDate" runat="server" BorderStyle="None" Height="20px" ImageUrl="~/images/calIcon.png"
                                                                            ToolTip="Please click here to select policy start date..." />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CalendarExtender ID="attendaceDate" runat="server" PopupPosition="Right" Animated="true"
                                                                            Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                                                        </asp:CalendarExtender>
                                                                        <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" 
                                                                            OnTextChanged="txtEndDate_TextChanged" ValidationGroup="ins" Width="80px" />
                                                                        <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Format="dd/MM/yyyy"
                                                                            PopupButtonID="imsEndDate" TargetControlID="txtEndDate">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Image ID="imsEndDate" runat="server" BorderStyle="None" Height="20px" ImageUrl="~/images/calIcon.png"
                                                                            ToolTip="Please click here to select policy end date..." ImageAlign="Baseline" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class="style24">
                                                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txtEndDate"
                                                                ControlToValidate="txtStartDate" ErrorMessage="End date must be Grether then start date..."
                                                                Operator="GreaterThan" Type="Date" ValidationGroup="ins">*</asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style26">
                                                            <b>Carry Forward? </b>
                                                        </td>
                                                        <td class="style29">
                                                            <table>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:CheckBox ID="chkIsApplicable" runat="server" Text="" 
                                                                            ToolTip="Click here to apply applicable rules..." AutoPostBack="True" 
                                                                            OnCheckedChanged="chkIsApplicable_CheckedChanged" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label4" runat="server" Text="MaxAllouced:" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMaxAllouced" runat="server" MaxLength="5" Width="30px" CssClass="txtclass">00.00</asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <tr>
                                                            <td align="right" class="style26">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Min Leave For One Month:"
                                                                    Width="170px"></asp:Label>
                                                            </td>
                                                            <td class="style29">
                                                                <asp:TextBox ID="txtMinLeavePM" runat="server" Width="40px" MaxLength="5" Text="00.00"
                                                                    CssClass="txtclass"></asp:TextBox>
                                                            </td>
                                                            <td class="style24">
                                                            </td>
                                                        </tr>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="style28">
                                                            <asp:Label ID="lbl" runat="server" Text="Max Leaves For Period:" Font-Bold="true"
                                                                Width="170px" />
                                                        </td>
                                                        <td class="style29">
                                                            <asp:TextBox ID="txtMaxLeavePP" runat="server" CssClass="txtclass" MaxLength="5"
                                                                Width="40px">00.00</asp:TextBox>
                                                        </td>
                                                        <td class="style24">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div id="divRuleDtl" runat="server" visible="false">
                                        <fieldset style="margin-top: 5px; width: 580px; direction: inherit; height: 85px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                                <table width="100%" align="center">
                                                    <tr>
                                                        <th class="style30">
                                                        </th>
                                                        <th align="center" class="style33">
                                                            Field Name
                                                        </th>
                                                        <th align="center" class="style35">
                                                            Operator
                                                        </th>
                                                        <th align="center" class="style30">
                                                            Dur. In Unit
                                                        </th>
                                                        <th align="center" class="style36">
                                                            Duration
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td class="style32" align="right">
                                                            Start Criteria:
                                                        </td>
                                                        <td align="left" class="style33">
                                                            <asp:DropDownList ID="ddlStartField" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="125px" />
                                                        </td>
                                                        <td align="left" align="left" class="style35">
                                                            <asp:DropDownList ID="ddlStartOperator" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="175px" />
                                                        </td>
                                                        <td align="center" class="style30">
                                                            <asp:DropDownList ID="ddlStartDurationUnit" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="85px" />
                                                        </td>
                                                        <td align="center" class="style36">
                                                            <asp:TextBox ID="txtStartDuration" runat="server" Width="55px" CssClass="txtclass"
                                                                MaxLength="3" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style32" align="right">
                                                            End Criteria:
                                                        </td>
                                                        <td align="left" class="style33">
                                                            <asp:DropDownList ID="ddlEndField" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="125px" />
                                                        </td>
                                                        <td align="left" class="style35">
                                                            <asp:DropDownList ID="ddlEndOperator" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="175px" />
                                                        </td>
                                                        <td align="center" class="style30">
                                                            <asp:DropDownList ID="ddlEndDurationUnit" runat="server" CssClass="ddl" DataTextField="Text"
                                                                DataValueField="Id" Width="85px" />
                                                        </td>
                                                        <td align="center" class="style36">
                                                            <asp:TextBox ID="txtEndDuration" runat="server" Width="55px" CssClass="txtclass"
                                                                MaxLength="3" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div id="divSavebutton" runat="server" visible="false">
                                        <fieldset style="margin-top: 5px; width: 580px; direction: inherit; height: 40px">
                                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="button" ValidationGroup="ins"
                                                    OnClick="btnSubmit_Click" />
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" 
                                                    OnClick="btnShowDetails_Click" 
                                                    ToolTip="Click here to add new record or reset the page..." />
                                                <br />
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="lblLeaveTempId" runat="server" EnableViewState="true" Value="0"
                                    Visible="False" />
                            </td>
                            <td>
                                <asp:HiddenField ID="lblRuleId" runat="server" EnableViewState="true" Value="0" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
