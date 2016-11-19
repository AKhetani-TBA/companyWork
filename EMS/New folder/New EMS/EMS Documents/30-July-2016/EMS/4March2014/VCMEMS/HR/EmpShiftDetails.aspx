<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpShiftDetails.aspx.cs"
    Inherits="HR_EmpShiftDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .hideselect
        {
            display: none;
        }
        .style4
        {
            height: 26px;
            width: 185px;
        }
        .style6
        {
            height: 26px;
            width: 28px;
        }
        .style8
        {
            height: 26px;
            width: 27px;
        }
        .style9
        {
            height: 26px;
            width: 139px;
        }
        .style11
        {
            height: 26px;
            width: 68px;
        }
        .style12
        {
            height: 26px;
            width: 45px;
        }
        .style13
        {
            height: 26px;
            width: 112px;
        }
        .style16
        {
            width: 114px;
        }
        .style17
        {
            height: 26px;
            width: 104px;
        }
        .style20
        {
            width: 86px;
        }
        .style22
        {
            width: 89px;
        }
        .style23
        {
            width: 85px;
        }
        #editPage
        {
            height: 420px;
        }
    </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font" style="height: 737px;">
        <br />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="search_grid" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                Department :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                    Width="168px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Employee :&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="false" Width="180px"
                                    CssClass="ddl">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImageButton1" runat="server" CssClass="Button" ImageUrl="~/images/searchbtn.png"
                                    OnClick="ImageButton1_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnAddShift" runat="server" Font-Bold="False" ForeColor="#333333"
                                    Text="Add Shift Details" CausesValidation="False" CssClass="button" OnClick="btnAddShift_Click" />
                            </td>
                        </tr>
                    </table>
                    <div>
                        <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                            </legend>
                            <asp:GridView ID="srchView" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                HorizontalAlign="Justify" OnPageIndexChanging="srchView_PageIndexChanging" OnRowCommand="srchView_RowCommand"
                                OnRowDataBound="srchView_RowDataBound" OnSelectedIndexChanged="srchView_SelectedIndexChanged"
                                Width="100%" PageSize="30">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle CssClass="gridheader" />
                                <Columns>
                                    <asp:BoundField DataField="ShiftId" HeaderText="">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmployeeId" HeaderText="Employee ID">
                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Employee Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee Name</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("empName") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="deptName"
                                                CommandName="sort" CssClass="gridlink">Department</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="120px" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ShiftDetail" HeaderText="Shift Detail"></asp:BoundField>
                                    <asp:BoundField DataField="FromDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}">
                                        <ItemStyle Width="140px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ToDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}">
                                        <ItemStyle Width="140px" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:CommandField ShowSelectButton="True">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:CommandField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="delCard" CommandName="delIt" runat="server" ImageUrl="~/images/delete.ico"
                                                Width="16px" OnClientClick="return confirm('Are you sure you want to delete?');" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Shift Assigned...</EmptyDataTemplate>
                            </asp:GridView>
                        </fieldset>
                    </div>
                </div>
                <div id="editPage" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Shift Details</legend>
                        <div id="left" style="float: left; width: 600px;" onclick="destroyPicker();">
                            <table>
                                <tr>
                                    <td class="style20">
                                        Employee <span style="color: Red">* </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="showEmployeesBySearch" runat="server" AutoPostBack="false"
                                            CssClass="ddl" Width="180px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="showEmployeesBySearch"
                                            CssClass="hideselect" ErrorMessage="Please select Employee" InitialValue="--Please select Employee--"></asp:RequiredFieldValidator>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="showEmployeesBySearch"
                                            ValidationGroup="Shift" ErrorMessage="Please Select Employee" InitialValue="0">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style22">
                                        Shift Detail
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="shiftDetails" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="General" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Night" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style20">
                                        Start Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="StartDate" runat="server" Width="176px"></asp:TextBox>
                                        <asp:CalendarExtender ID="StartDate_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd MMM yyyy" TargetControlID="StartDate">
                                        </asp:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                            ControlToValidate="StartDate" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="StartDate"
                                            ErrorMessage="Please enter start date" CssClass="hideselect"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style20">
                                        End Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="EndDate" runat="server" Width="176"></asp:TextBox>
                                        <asp:CalendarExtender ID="EndDate_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd MMM yyyy" TargetControlID="EndDate">
                                        </asp:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="EndDate"
                                            ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="EndDate"
                                            ErrorMessage="Please enter end date" CssClass="hideselect"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style16">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="okBtn" runat="server" CssClass="button" OnClick="okBtn_Click" Text="Save" />
                                        &nbsp;
                                        <asp:Button ID="cancelBtn" runat="server" CausesValidation="False" CssClass="button"
                                            OnClick="cancelBtn_Click" Text="Back" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                            ShowMessageBox="True" ShowSummary="False" />
                                        <asp:HiddenField ID="hidshiftId" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnexcel" />
                <asp:PostBackTrigger ControlID="btnword" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
