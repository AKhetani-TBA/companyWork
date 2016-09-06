<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeCard.aspx.cs" Inherits="HR_EmployeeCard"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
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
                            <td nowrap="nowrap" class="style11">
                                Department&nbsp;
                            </td>
                            <td nowrap="nowrap" class="style13">
                                <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                                    Width="100px" CssClass="ddl">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Value="Id">Employee Code</asp:ListItem>
                                    <asp:ListItem Value="Name">Employee Name</asp:ListItem>
                                    <asp:ListItem>Email</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style12" nowrap="nowrap">
                                Employee
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" Width="171px"
                                    CssClass="ddl" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Value="Id">Employee Code</asp:ListItem>
                                    <asp:ListItem Value="Name">Employee Name</asp:ListItem>
                                    <asp:ListItem>Email</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style8">
                                Type
                            </td>
                            <td class="style9">
                                <asp:DropDownList ID="showType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                                    Width="125px" CssClass="ddl">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Permanent</asp:ListItem>
                                    <asp:ListItem Selected="True">Temporary</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style6">
                                Status
                            </td>
                            <td class="style17">
                                <asp:DropDownList ID="showStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                                    Width="102px" CssClass="ddl">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem Selected="True">Issued</asp:ListItem>
                                    <asp:ListItem>Revoked</asp:ListItem>
                                    <asp:ListItem>Terminated</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="AddCard" runat="server" Font-Bold="False" ForeColor="#333333" OnClick="AddCard_Click"
                                    Style="float: right" Text="Assign Card" CssClass="button" />
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="ImageButton1_Click" />
                            </td>
                            <td style="width: 5%;">
                                &nbsp;
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/excelicon.png"
                                    Width="23px" OnClick="btnexcel_Click" /><br />
                            </td>
                        </tr>
                    </table>
                    <div id="CardDetail">
                        <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                            </legend>
                            <div style="height: 1050px; width: 100%; overflow-y: auto; overflow-x: auto;">
                                <asp:GridView ID="srchView" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                    HorizontalAlign="Justify" OnPageIndexChanging="srchView_PageIndexChanging" OnRowCommand="srchView_RowCommand"
                                    OnRowDataBound="srchView_RowDataBound" OnSelectedIndexChanged="srchView_SelectedIndexChanged"
                                    Width="100%" PageSize="30">
                                    <RowStyle BorderColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                    <HeaderStyle CssClass="gridheader" />
                                    <Columns>
                                        <asp:BoundField DataField="empId" HeaderText="Employee ID">
                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                            <ControlStyle CssClass="hideselect" />
                                            <FooterStyle CssClass="hideselect" />
                                            <HeaderStyle CssClass="hideselect" />
                                            <ItemStyle CssClass="hideselect" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Sr No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerial" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="45px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="serialId" HeaderText="SerialId">
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
                                        <asp:TemplateField HeaderText="Type">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("cardType") %>'></asp:TextBox></EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="isTemp" CommandName="sort"
                                                    CssClass="gridlink">Type</asp:LinkButton></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("cardType") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="70px" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Card No.">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("RFIDNo") %>'></asp:TextBox></EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="RFIDNo"
                                                    CommandName="sort" CssClass="gridlink">Card No.</asp:LinkButton></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("RFIDNo") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("status") %>'></asp:TextBox></EditItemTemplate>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandArgument="status"
                                                    CommandName="sort" CssClass="gridlink">Status</asp:LinkButton></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("status") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="60px" Wrap="True" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issued Date">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="LinkButto" runat="server" CausesValidation="False" CommandArgument="IssuedDate"
                                                    CommandName="sort" CssClass="gridlink">Issued Date</asp:LinkButton></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Labe" runat="server" Text='<%# Bind("IssuedDate") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="130px" Wrap="False" />
                                        </asp:TemplateField>
                                        <%-- <asp:BoundField DataField="formatedDate" HeaderText="Issued Date" >
                                        <ItemStyle Width="140px" Wrap="False" />
                                    </asp:BoundField>--%>
                                        <asp:BoundField DataField="RevokedDate" HeaderText="Revoked Date">
                                            <ItemStyle Width="130px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Reason" HeaderText="Reason">
                                            <ItemStyle Width="140px" />
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
                                        No Card Assigned...</EmptyDataTemplate>
                                </asp:GridView>
                                </div>
                        </fieldset>
                    </div>
                    <div id="reportdiv" runat="server" style="text-align: center">
                        <fieldset style="margin-top: 7px; padding-top: 3px; padding-bottom: 3px">
                            <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="22px"
                                OnClick="btnexcel_Click" Style="height: 20px" />
                            &nbsp;
                            <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" Width="22px"
                                OnClick="btnword_Click" />
                        </fieldset>
                    </div>
                </div>
                </div>
                <div id="editPage" runat="server" onclick="destroyPicker();">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Issue/ Revoke/
                            Terminate Card </legend>
                        <div id="left" style="float: left; width: 600px;" onclick="destroyPicker();">
                            <div id="actiondiv" runat="server">
                                <table>
                                    <tr>
                                        <td class="style23">
                                            Action
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="status" runat="server" AutoPostBack="True" CssClass="ddl" OnSelectedIndexChanged="status_SelectedIndexChanged"
                                                Width="180px">
                                                <asp:ListItem>Issue</asp:ListItem>
                                                <asp:ListItem>Reissue</asp:ListItem>
                                                <asp:ListItem>Revoke</asp:ListItem>
                                                <asp:ListItem>Terminate</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="typediv" runat="server">
                                <table>
                                    <tr>
                                        <td class="style22">
                                            Type
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="permanent" runat="server" AutoPostBack="True" Checked="True"
                                                GroupName="cardtype" OnCheckedChanged="permanent_CheckedChanged" Text="Permanent" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="temporary" runat="server" AutoPostBack="True" GroupName="cardtype"
                                                OnCheckedChanged="temporary_CheckedChanged" Text="Temporary" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="empdiv" runat="server">
                                <table>
                                    <tr>
                                        <td class="style20">
                                            Employee
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="showEmployeesBySearch" runat="server" AutoPostBack="True" CssClass="ddl"
                                                OnSelectedIndexChanged="showEmployeesBySearch_SelectedIndexChanged" Width="180px">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="carddiv" runat="server">
                                <table>
                                    <tr>
                                        <td class="style20">
                                            Card
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="CardIds" runat="server" AutoPostBack="True" CssClass="ddl"
                                                OnSelectedIndexChanged="CardIds_SelectedIndexChanged" Width="180px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="reasondiv" runat="server">
                                <table>
                                    <tr>
                                        <td class="style20">
                                            Reason
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cardReason" runat="server" AutoPostBack="True" CssClass="ddl"
                                                Width="180px">
                                                <asp:ListItem>Lost</asp:ListItem>
                                                <asp:ListItem>Forgot</asp:ListItem>
                                                <asp:ListItem>Not Working</asp:ListItem>
                                                <asp:ListItem>Resigned</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="issuedatediv" runat="server" style="width: 600px">
                                <div style="float: left">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                Issue Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="issueDate" runat="server" Width="176px"></asp:TextBox>
                                                <asp:CalendarExtender ID="issueDate_CalendarExtender" runat="server" Enabled="True"
                                                    Format="dd MMM yyyy" TargetControlID="issueDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="issueTimeDiv" runat="server">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                &nbsp;&nbsp;&nbsp;Issue Time
                                            </td>
                                            <td id="issueTimeTd">
                                                <asp:TextBox ID="issueTime" onclick="selectedBoxTd='issueTimeTd';selectedBox=this.id; showPicker();"
                                                    runat="server" OnTextChanged="issueTime_TextChanged" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="durationdiv" runat="server" style="width: 600px">
                                <div style="float: left">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                Duration Till
                                            </td>
                                            <td>
                                                <asp:TextBox ID="duration" runat="server" OnTextChanged="duration_TextChanged" Width="176px"></asp:TextBox>
                                                <asp:CalendarExtender ID="duration_CalendarExtender" runat="server" Enabled="True"
                                                    Format="dd MMM yyyy" TargetControlID="duration">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="durationTimeDiv" runat="server">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                &nbsp;&nbsp;&nbsp;End time
                                            </td>
                                            <td id="endtimeTd">
                                                <asp:TextBox ID="durationTime" runat="server" onclick="selectedBoxTd='endtimeTd';selectedBox = this.id;showPicker();"
                                                    Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="revokediv" runat="server" style="width: 600px">
                                <div style="float: left">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                Revoke Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="revokedate" runat="server" Width="176"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd MMM yyyy"
                                                    TargetControlID="revokedate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="revokeTimeDiv" runat="server">
                                    <table>
                                        <tr>
                                            <td class="style20">
                                                &nbsp;&nbsp; Revoke Time
                                            </td>
                                            <td id="revoketimeTd">
                                                <asp:TextBox ID="revoketime" runat="server" onclick="selectedBoxTd='revoketimeTd';selectedBox = this.id;showPicker();"
                                                    Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div style="float: left">
                                <table>
                                    <tr>
                                        <td class="style16">
                                        </td>
                                        <td>
                                            <asp:Button ID="okBtn" runat="server" CssClass="button" OnClick="okBtn_Click" Text="Save" />
                                            &nbsp;
                                            <asp:Button ID="cancelBtn" runat="server" CausesValidation="False" CssClass="button"
                                                OnClick="cancelBtn_Click" Text="Back" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="right" style="float: right; width: 330px;">
                            <div id="TimePicker" style="position: absolute; width: 67px; display: none;" class="timepicker">
                                &nbsp;<span id="am" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor = 'white';"
                                    onclick="amPm='AM';picktime();">AM</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id="pm"
                                        onclick="amPm='PM';picktime();" onmouseover="brighttime(this.id); this.style.cursor='hand'"
                                        onmouseout="this.style.backgroundColor = 'white';">PM</span><br>
                                &nbsp;<span id="1" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">01:00</span>&nbsp;&nbsp;<span id="Span4"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">01:30</span>
                                <br>
                                &nbsp;<span id="2" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">02:00</span>&nbsp;&nbsp;<span id="Span6"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">02:30</span>
                                <br>
                                &nbsp;<span id="3" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">03:00</span>&nbsp;&nbsp;<span id="330"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">03:30</span>
                                <br>
                                &nbsp;<span id="4" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">04:00</span>&nbsp;&nbsp;<span id="430"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">04:30</span>
                                <br>
                                &nbsp;<span id="5" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">05:00</span>&nbsp;&nbsp;<span id="530"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">05:30</span>
                                <br>
                                &nbsp;<span id="6" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">06:00</span>&nbsp;&nbsp;<span id="630"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">06:30</span>
                                <br>
                                &nbsp;<span id="7" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">07:00</span>&nbsp;&nbsp;<span id="730"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">07:30</span> &nbsp;<span id="8" onmouseover="brighttime(this.id); this.style.cursor='hand'"
                                            onmouseout="this.style.backgroundColor='white'" onclick="selectedTimeId=this.id;;picktime();">08:00</span>&nbsp;
                                <span id="830" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">08:30</span>
                                <br>
                                &nbsp;<span id="9" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">09:00</span>&nbsp;&nbsp;<span id="930"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">09:30</span>
                                <br>
                                &nbsp;<span id="10" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">10:00</span>&nbsp;&nbsp;<span id="1030"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">10:30</span>
                                <br>
                                &nbsp;<span id="11" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">11:00</span>&nbsp;&nbsp;<span id="1130"
                                        onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                        onclick="selectedTimeId=this.id;;picktime();">11:30</span>
                                <br>
                                &nbsp;<span id="Span1" onmouseover="brighttime(this.id); this.style.cursor='hand'"
                                    onmouseout="this.style.backgroundColor='white'" onclick="selectedTimeId=this.id;;picktime();">12:00</span>&nbsp;
                                <span id="Span2" onmouseover="brighttime(this.id); this.style.cursor='hand'" onmouseout="this.style.backgroundColor='white'"
                                    onclick="selectedTimeId=this.id;;picktime();">12:30</span>
                                <br>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
                <asp:PostBackTrigger ControlID="btnword" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
