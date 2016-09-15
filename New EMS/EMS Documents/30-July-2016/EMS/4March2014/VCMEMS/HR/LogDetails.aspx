<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogDetails.aspx.cs" EnableEventValidation="false"
    EnableViewState="true" Inherits="HR_LogDetails" Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
                            <%--<td>
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
                            </td>--%>
                            <td>
                                Start Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server" Width="176px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                    TargetControlID="txtStartDate">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                End Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" runat="server" Width="176px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                    TargetControlID="txtEndDate">
                                </asp:CalendarExtender>
                            </td>
                            <td valign="middle">
                                <asp:ImageButton ID="ImageButton1" runat="server" CssClass="" ImageUrl="~/images/searchbtn.png"
                                    OnClick="ImageButton1_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnAddShift" runat="server" Font-Bold="False" ForeColor="#333333"
                                    Text="Add Log Detail" CausesValidation="False" CssClass="button" OnClick="btnAddShift_Click" />
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
                                    <asp:TemplateField HeaderText="Employee Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee Name</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("empName") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Department">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="deptName"
                                                CommandName="sort" CssClass="gridlink">Department</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="120px" Wrap="False" />
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="Machine_Code" HeaderText="Machine Code"></asp:BoundField>
                                    <asp:BoundField DataField="Card_No" HeaderText="Card No" />
                                    <asp:BoundField DataField="TimeStamp" HeaderText="TimeStamp" DataFormatString="{0:dd/mm/yy hh:mm:ss}">
                                        <%--{0:MM/dd/yyyy}--%>
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
                                    No Record Found...</EmptyDataTemplate>
                            </asp:GridView>
                        </fieldset>
                    </div>
                </div>
                <div id="editPage" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Log Details</legend>
                        <div id="left" style="float: left; width: 600px;">
                            <table>
                                <tr>
                                    <td>
                                        Employee <span style="color: Red">* </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="showEmployeesBySearch" runat="server" AutoPostBack="false"
                                            CssClass="ddl" Width="180px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="showEmployeesBySearch"
                                            CssClass="hideselect" ErrorMessage="Please select Employee" InitialValue="--Please select Employee--"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Machine Code
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblMachineCode" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="In" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Out" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        TimeStamp
                                    </td>
                                    <td>
                                        <asp:TextBox ID="StartDate" runat="server" Width="176px"></asp:TextBox>
                                        <asp:CalendarExtender ID="StartDate_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd/MM/yyyy" TargetControlID="StartDate">
                                        </asp:CalendarExtender>
                                        <asp:DropDownList ID="ddlhr" runat="server" ToolTip="Select Hour">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlmin" runat="server" ToolTip="Select Minute">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlsec" runat="server" ToolTip="Select Second">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
