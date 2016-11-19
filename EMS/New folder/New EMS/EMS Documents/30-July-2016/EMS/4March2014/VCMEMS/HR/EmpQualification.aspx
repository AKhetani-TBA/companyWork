<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpQualification.aspx.cs"
    Inherits="HR_EmpQualification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
                <table id="Quali" runat="server" width="100%">
                    <tr>
                        <td style="width: 40%;">
                            Department :&nbsp;&nbsp;
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                Width="168px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 40%;">
                            Employee :&nbsp;&nbsp;
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                Width="180px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                             <td style="width: 50px;">
                                <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="imgbtnSearch_Click" />
                            </td>

                        <td style="width: 15%;">
                            <asp:Button ID="AddQualif" runat="server" Font-Bold="False" ForeColor="#333333" Text="Add Qualification"
                                CausesValidation="False" OnClick="AddQualif_Click" CssClass="button" />
                        </td>
                             <td  style="width: 5%;" > &nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"   
                                        OnClick="btnexcel_Click" /><br />
                          </td>                             
                    </tr>
                </table>
                <div id="qualificatinDiv" runat="server" style="width: 100%;">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Qualification
                            Details </legend>
                        <table>
                            <tr>
                                <td style="width: 15%">
                                    Employee
                                </td>
                                <td style="width: 25%">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="180px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 15%">
                                    Degree <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 25%">
                                    <asp:TextBox ID="txtQialName" runat="server" Width="160px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvqua" runat="server" ControlToValidate="txtQialName"
                                        Display="None" ErrorMessage="Please enter qualification name"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    University / Board <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 25%">
                                    <asp:TextBox ID="txtQualBoard" runat="server" Width="160px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtQualBoard"
                                        CssClass="hideselect" ErrorMessage="Please enter university/board"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Month - Year Of Passing <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtQualYear" runat="server" Width="70px" MaxLength="4"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revyear" runat="server" Display="None" ControlToValidate="txtQualYear"
                                        ErrorMessage="Enter proper year" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvmonth" runat="server" ControlToValidate="ddlMonth"
                                        Display="None" ErrorMessage="Please enter month" InitialValue="Select.."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Percentage <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQualPerc" runat="server" Width="170px" MaxLength="5"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revqa" runat="server" Display="None" ControlToValidate="txtQualPerc"
                                        ErrorMessage="Enter proper percentage" ValidationExpression="^[0-9.]+$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvper" runat="server" ControlToValidate="txtQualPerc"
                                        Display="None" ErrorMessage="Please enter percentage"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td style="text-align: right;">
                                    <asp:Button ID="addQualification" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" Text="Save" OnClick="addQualification_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                    <asp:Button ID="reset" runat="server" CausesValidation="False" Font-Bold="False"
                                        Font-Names="Tw Cen MT Condensed" Font-Size="18px" ForeColor="#333333" Height="28px"
                                        OnClick="reset_Click" Text="Cancel" />
                                </td>
                                <td>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="grid" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                Width="100%" PageSize="20" OnSelectedIndexChanged="displayAll_SelectedIndexChanged"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                    ForeColor="White" Height="19px" HorizontalAlign="Left" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="qualifName"
                                                CommandName="sort" CssClass="gridlink">Qualification</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("qualifName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relationship">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="qualifBoard"
                                                CommandName="sort" CssClass="gridlink">University/Board</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("qualifBoard") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relative Name">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="qualifYear"
                                                CommandName="sort" CssClass="gridlink">Year of Passing</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("qualifYear") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Relative Name">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandArgument="qualifPerc"
                                                CommandName="sort" CssClass="gridlink">Percentage</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label33" runat="server" Text='<%# Bind("qualifPerc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" HeaderText="">
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" Height="16px"
                                                    ImageUrl="~/images/delete.ico" OnClientClick="return confirm('Are you sure you want to delete the qualification?');" />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="qualifId">
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
                                    No Qualification Details Added...
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
                 <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
       </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
