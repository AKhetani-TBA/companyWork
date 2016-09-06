<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EmployeeBank.aspx.cs"
    Inherits="HR_EmployeeBank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <br />
                <table id="Bank" runat="server">
                    <tr>
                        <td>
                            Department :
                        </td>
                        <td>
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" Width="168px"
                                OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Employee :
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
                            <asp:Button ID="AddBank" runat="server" Text="Add Bank" CausesValidation="False"
                                OnClick="AddBank_Click" />
                        </td>
                          <td > &nbsp;&nbsp;&nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" 
                        ImageUrl="~/images/excelicon.png" Width="23px"
                                        OnClick="btnexcel_Click" style="height: 21px" /><br />
                                </td>     
                    </tr>
                </table>
                <div id="addbankdiv" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Bank Details
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 10%">
                                    Employee :
                                </td>
                                <td style="width: 10%">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 10%">
                                    Bank Name <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 10%">
                                    <asp:DropDownList ID="banklist" runat="server" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 10%">
                                    Account Number <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox ID="acname" runat="server" Width="175px"></asp:TextBox>&nbsp;<asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator2" runat="server" ControlToValidate="acname" ErrorMessage="Enter proper account number"
                                        ValidationExpression="^[0-9a-zA-Z]+$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="acname"
                                        ErrorMessage="Please enter account number" CssClass="hideselect"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Branch <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="bnkbrnch" runat="server" Width="175px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="bnkbrnch"
                                        CssClass="hideselect" ErrorMessage="Please enter branch name"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:Label ID="lblAccountType" runat="server" Text="Account Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="accountType" runat="server" Width="180px" AutoPostBack="True">
                                        <asp:ListItem Value="1">Salary</asp:ListItem>
<%--                                        <asp:ListItem Value="2">Provident Fund</asp:ListItem>
                                        <asp:ListItem Value="3">ESIC</asp:ListItem>
--%>                                        <asp:ListItem Value="4">Other</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center;" colspan="6">
                                    <asp:Button ID="saveBankDetails" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" Text="Save" OnClick="saveBankDetails_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="cncl" runat="server" CausesValidation="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" OnClick="cncl_Click" Text="Cancel" />
                                    &nbsp;
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" DisplayMode="List" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="DivGrid" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 600px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged"
                                Width="100%" PageSize="40">
                                <RowStyle BorderColor="#333333" />
                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                    ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">EmployeeName</asp:LinkButton></HeaderTemplate>
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
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="bankId" HeaderText="Bank Id">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Account Number">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("accountNo") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="accountNo"
                                                CommandName="sort" CssClass="gridlink">Account Number</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("accountNo") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bank Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("bankName") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="bankName"
                                                CommandName="sort" CssClass="gridlink">Bank Name</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("bankName") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bank Branch">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("bankBranch") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandArgument="bankBranch"
                                                CommandName="sort" CssClass="gridlink">Bank Branch</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("bankBranch") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Account Type">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("isSalaryAccount") %>'></asp:TextBox></EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton6" runat="server" CausesValidation="False" CommandArgument="isSalaryAccount"
                                                CommandName="sort" CssClass="gridlink">Account Type</asp:LinkButton></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("isSalaryAccount") %>'></asp:Label></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowSelectButton="True">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:CommandField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" ImageUrl="~/images/delete.ico"
                                                OnClientClick="return confirm('Are you sure to want to delete the bank?');" Width="16px"
                                                OnClick="ImageButton2_Click" />
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="empId">
                                        <ControlStyle CssClass="hideselect" />
                                        <FooterStyle CssClass="hideselect" />
                                        <HeaderStyle CssClass="hideselect" />
                                        <ItemStyle CssClass="hideselect" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Bank Details Added...</EmptyDataTemplate>
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
            <%-- <asp:ObjectDataSource ID="getBankDetails" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetBankDetail" TypeName="EmpDataTableAdapters.Emp_Bank_DetailsTableAdapter"
                    UpdateMethod="Update">
                    <DeleteParameters>
                        <asp:Parameter Name="Original_bankId" Type="Int64" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="empId" Type="Int64" />
                        <asp:Parameter Name="accountNo" Type="String" />
                        <asp:Parameter Name="bankName" Type="String" />
                        <asp:Parameter Name="bankBranch" Type="String" />
                        <asp:Parameter Name="isSalaryAccount" Type="String" />
                        <asp:Parameter Name="Original_bankId" Type="Int64" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:SessionParameter Name="empid" SessionField="empid" Type="Int64" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="empId" Type="Int64" />
                        <asp:Parameter Name="accountNo" Type="String" />
                        <asp:Parameter Name="bankName" Type="String" />
                        <asp:Parameter Name="bankBranch" Type="String" />
                        <asp:Parameter Name="isSalaryAccount" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>--%>
        </ContentTemplate>
           <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
