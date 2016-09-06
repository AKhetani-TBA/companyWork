<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="HR_ManageUsers"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 79px;
        }
        .style2
        {
            width: 197px;
        }
        .style3
        {
            width: 75px;
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
                <div id="search_grid" runat="server">
                    <table>
                        <tr>
                            <td>
                                Department
                            </td>
                            <td class="style2">
                                <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                    OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" Width="168px">
                                </asp:DropDownList>
                            </td>
                            <td class="style3">
                                Employee
                            </td>
                            <td >
                                <asp:DropDownList ID="showEmployees" runat="server" OnSelectedIndexChanged="ddlEmployees_SelectedIndexChanged"
                                    Width="190px" CssClass="ddl">
                                </asp:DropDownList>
                            </td>
                            <td>
                            Status :
                            </td>
                            <td>
                             <asp:DropDownList ID="ddlEmpStatus" runat="server" OnSelectedIndexChanged="ddlEmpStatus_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Hired</asp:ListItem>
                                    <asp:ListItem Value="2">Resigned</asp:ListItem>
                                    <%--<asp:ListItem Value="3">Deleted</asp:ListItem>--%>
                                    <asp:ListItem Value="3">ALL</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 100px">
                                <asp:ImageButton ID="srchbtn" runat="server" ImageUrl="~/images/searchbtn.png" 
                                    OnClick="srchbtn_Click" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="searchresult">
                    <fieldset style="padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <%--<div style="float: left; width: 75%">--%>
                         <div style="float:left; height: 700px; width: 75%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowCommand="srchView_RowCommand"
                                OnRowDataBound="srchView_RowDataBound" OnSelectedIndexChanged="srchView_SelectedIndexChanged" AllowSorting="true"
                                OnSorting="srchView_OnSorting" AllowPaging="false" OnPageIndexChanging="srchView_PageIndexChanging" PageSize="20">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField HeaderText="Code" DataField="empId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Name" SortExpression="empName">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="sortNameBtn" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Name</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" runat="server" Text='<%# Bind("empName") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="160px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" SortExpression="deptName">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("deptName") %>' ></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="sortDeptBtn" runat="server" CausesValidation="False" CommandArgument="deptName"
                                                CommandName="sort" CssClass="gridlink">Department</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("deptName") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation" SortExpression="empDomicile">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empDomicile"
                                                CommandName="sort" CssClass="gridlink">Designation</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Type" SortExpression="userType">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="linkusertype" runat="server" CausesValidation="False" CommandArgument="userType"
                                                CommandName="sort" CssClass="gridlink">User Type</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblusertype" runat="server" Text='<%# Bind("userType") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Rights Details Added...
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="#959595" ForeColor="White" Height="19px" Font-Names="Tw Cen MT Condensed"
                                    Font-Size="17px" HorizontalAlign="Left" />
                            </asp:GridView>
                        </div>
                        <div id="userrightdiv" class="userrightpopup">
                            <fieldset id="Fieldset2" style="margin-right: 5px; margin-left: 5px; margin-bottom: 5px;
                                padding-bottom: 5px; padding-left: 5px; width: 202px; padding-right: 5px;">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Change Employee's
                                    Role </legend>
                                <div style="text-align: left">
                                    <asp:Label ID="empName" runat="server" Text="Select Employee" Font-Bold="True"></asp:Label><br />
                                    <br />
                                    User Type :
                                    <br />
                                    <asp:RadioButtonList ID="rights" runat="server">
                                        <asp:ListItem Value="0">Employee</asp:ListItem>
                                        <asp:ListItem Value="1">HR</asp:ListItem>
                                        <asp:ListItem Value="2">Account</asp:ListItem>
                                        <asp:ListItem Value="3">Admin</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div style="text-align: center; margin-top: 5px;">
                                    <asp:Button ID="btnStatusSubmit" runat="server" Text="Submit" CssClass="button" 
                                        OnClick="btnStatusSubmit_Click" CausesValidation="False" />
                                </div>
                            </fieldset>
                            <fieldset id="Fieldset1" style="margin-right: 5px; margin-left: 5px; margin-bottom: 5px;
                                padding-bottom: 5px; padding-left: 5px; width: 202px; padding-right: 5px;">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Change Employee's
                                    Status </legend>Resign Date * :
                                <br />
                                <br />
                                <asp:TextBox ID="resignDate" runat="server" Width="100px"></asp:TextBox>
                                &nbsp;
                                <asp:Label ID="ack" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:CalendarExtender ID="hireDate_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                    TargetControlID="resignDate">
                                </asp:CalendarExtender>
                                &nbsp; &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server"
                                    ControlToValidate="resignDate" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                      <%--          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter resign date"
                                    ControlToValidate="resignDate" CssClass="hideselect"></asp:RequiredFieldValidator>--%>
                                <br />
                                <asp:Button ID="submitResign" runat="server" CssClass="button" Text="Submit" OnClick="submitResign_Click" />
                            </fieldset>
                        </div>
                    </fieldset>
                </div>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                    ShowMessageBox="True" ShowSummary="False" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
