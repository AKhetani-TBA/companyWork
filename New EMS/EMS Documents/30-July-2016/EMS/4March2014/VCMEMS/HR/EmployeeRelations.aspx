<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EmployeeRelations.aspx.cs"
    Inherits="HR_EmployeeRelations" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <br />
                <table id="Relation" runat="server">
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
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                Width="180px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                            <td>
                    <asp:ImageButton ID="btnsearch" runat="server" ImageUrl="~/images/searchbtn.png"
                        OnClick="btnsearch_Click" CausesValidation="False" style="height: 23px" />
                </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="AddRel" runat="server" Font-Bold="False" ForeColor="#333333" Text="Add Relation"
                                CausesValidation="False" OnClick="AddRel_Click" CssClass="button" />
                        </td>
                             <td > &nbsp;&nbsp;&nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" 
                        ImageUrl="~/images/excelicon.png" Width="23px"
                                        OnClick="btnexcel_Click" style="height: 21px" /><br />
                                </td>     
                    </tr>
                </table>
                <div id="addrelationdiv" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Relative Details
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 10%">
                                    Employee<span style="color: #FF0000">*</span> 
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 10%">
                                    Relationship
                                </td>
                                <td style="width: 15%">
                                    <asp:DropDownList ID="rl" runat="server" Width="180px" CssClass="ddl">
                                        <asp:ListItem>Brother</asp:ListItem>
                                        <asp:ListItem>Cousin</asp:ListItem>
                                        <asp:ListItem>Daughter</asp:ListItem>
                                        <asp:ListItem>Father</asp:ListItem>
                                        <asp:ListItem>Landlord</asp:ListItem>
                                        <asp:ListItem>Mother</asp:ListItem>
                                        <asp:ListItem>Neighbour</asp:ListItem>
                                        <asp:ListItem>Owner</asp:ListItem>
                                        <asp:ListItem>Roommate</asp:ListItem>
                                        <asp:ListItem>Sister</asp:ListItem>
                                        <asp:ListItem>Son</asp:ListItem>
                                        <asp:ListItem>Spouse</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%">
                                  &nbsp;&nbsp;&nbsp;&nbsp;  Date of Birth
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="dob" runat="server" Width="174px"></asp:TextBox>
                                    <asp:CalendarExtender ID="dob_CalendarExtender" runat="server" DaysModeTitleFormat="d MMMM, yyyy"
                                        Enabled="True" Format="dd MMMM yyyy" TargetControlID="dob" TodaysDateFormat="d MMMM,  yyyy">
                                    </asp:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server"
                                        ControlToValidate="dob" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle">
                                    Relative Name <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="relativename" runat="server" Width="174px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="relativename"
                                    ErrorMessage="Please enter relative name" Display="None" ></asp:RequiredFieldValidator>
                                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="relativename" ErrorMessage="Enter proper name" ValidationExpression="^[a-zA-Z. ]+$"></asp:RegularExpressionValidator>
                                </td>
                                <td valign="middle">
                                    Contact No.
                                </td>
                                <td>
                                    <asp:TextBox ID="relativeno" runat="server" Width="174px" MaxLength="14"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="relativeno"
                                        ErrorMessage="Enter proper Contact No." ValidationExpression="^[0-9+-]+$"></asp:RegularExpressionValidator>
                                </td>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;" colspan="6">
                                    <asp:Button ID="addrelative" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" OnClick="addrelative_Click"
                                        Text="Save" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="reset" runat="server" CausesValidation="False" Font-Bold="False"
                                        Font-Names="Tw Cen MT Condensed" Font-Size="18px" ForeColor="#333333" Height="28px"
                                        OnClick="reset_Click" Text="Cancel" />
                                    &nbsp;
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="divgrid" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 600px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged"
                                Width="100%" PageSize="40">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                    ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee" SortExpression="empName">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="deptName"
                                                CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relationship">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("relationship") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="relationship"
                                                CommandName="sort" CssClass="gridlink">Relationship</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("relationship") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relative Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("relativeName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="relativeName"
                                                CommandName="sort" CssClass="gridlink">Relative Name</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("relativeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="relativeContactNo" HeaderText="Contact">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="relativeDOB" HeaderText="DOB">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False" HeaderText="">
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" Height="16px"
                                                    ImageUrl="~/images/delete.ico" OnClientClick="return confirm('Are you sure you want to delete the relation?');" />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="relationId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:CommandField ShowSelectButton="True">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="empId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Relative Details Added...
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
                <br />
            </div>
        </ContentTemplate>
             <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
