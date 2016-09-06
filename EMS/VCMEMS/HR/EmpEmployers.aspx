<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpEmployers.aspx.cs" Inherits="HR_EmpEmployers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <style type="text/css">
        </style>

    <script type="text/javascript">
             
    </script>

</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <table id="Employer" runat="server">
                    <tr>
                        <td style="width: 10%">
                            Department :
                        </td>
                        <td style="width: 20%">
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                Width="168px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                            Employee :
                        </td>
                        <td style="width: 20%">
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                Visible="False" Width="180px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                             <td style="width: 10%;">&nbsp;&nbsp;
                                <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="imgbtnSearch_Click" />
                            </td>                        
                        <td  style="width:20%;" >
                            <asp:Button ID="btnAddEmplr" runat="server" Font-Bold="False" ForeColor="#333333"
                                Text="Add Employer" CausesValidation="False" OnClick="btnAddEmplr_Click" CssClass="button" />
                        </td>
                             <td  style="width: 10%;" > &nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" 
                                     ImageUrl="~/images/excelicon.png" Width="23px"   
                                        OnClick="btnexcel_Click" style="height: 21px" /><br />
                          </td>                                                     
                    </tr>
                </table>
                <div id="employerDiv" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Employer Details
                        </legend>
                        <table>
                            <tr>
                                <td style="width: 15%;" valign="middle">
                                    Employee <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 35%" valign="middle">
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="200px" CssClass="ddl">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEmpName" runat="server" ErrorMessage="Please select employee name."
                                        InitialValue="0" ControlToValidate="ddlEmp" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 15%;" valign="middle">
                                    Company Name <span style="color: #FF0000">*</span>
                                </td>
                                <td style="width: 35%" valign="middle">
                                    <asp:TextBox ID="txtEmplrName" runat="server" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCom" runat="server" ControlToValidate="txtEmplrName"
                                        ErrorMessage="Please enter Employer name" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Designation <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDesignation" runat="server" Width="200px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="rfvDes" runat="server" ControlToValidate="txtDesignation"
                                        ErrorMessage="Please enter designation" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Location <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLocation" runat="server" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLoc" runat="server" ControlToValidate="txtLocation"
                                        ErrorMessage="Please enter location" Display="None"></asp:RequiredFieldValidator>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Time Period <span style="color: #FF0000">*</span>
                                </td>
                                <td colspan="3">
                                    &nbsp;<asp:DropDownList ID="ddlFromMonth" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:TextBox ID="txtFromYear" runat="server" Width="60px" MaxLength="4" onmousedown="this.value = '';"></asp:TextBox>
                                    &nbsp; To&nbsp;
                                    <asp:DropDownList ID="ddlToMonth" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtToYear" runat="server" Width="60px" MaxLength="4" onmousedown="this.value = '';"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td style="text-align: right;">
                                    <asp:Button ID="addEmployer" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" Text="Save" OnClick="addEmployer_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                    <asp:Button ID="reset" runat="server" CausesValidation="False" Font-Bold="False"
                                        Font-Names="Tw Cen MT Condensed" Font-Size="18px" ForeColor="#333333" Height="28px"
                                        OnClick="reset_Click" Text="Cancel" />
                                </td>
                                <td>
                                    <asp:RegularExpressionValidator ID="revvYear" runat="server" ControlToValidate="txtToYear"
                                        Display="None" ErrorMessage="Enter proper To year" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                    &nbsp;<asp:RegularExpressionValidator ID="revFromYear" runat="server" ControlToValidate="txtToYear"
                                        Display="None" ErrorMessage="Enter proper From year" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvfy" runat="server" ControlToValidate="txtFromYear"
                                        Display="None" ErrorMessage="Please enter From year"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvtoyear" runat="server" ControlToValidate="txtToYear"
                                        Display="None" ErrorMessage="Please enter To year"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvtoyr" runat="server" ControlToValidate="ddlFromMonth"
                                        Display="None" ErrorMessage="Please enter From month" InitialValue="Select.."></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5fvuomon" runat="server" ControlToValidate="ddlToMonth"
                                        Display="None" ErrorMessage="Please enter To month" InitialValue="Select.."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" />
                    </div>
                <div id="Grid" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 650px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged"
                                PageSize="20" Width="100%">
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
                                    <asp:TemplateField HeaderText="Company Name">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="emplrName"
                                                CommandName="sort" CssClass="gridlink">Company Name</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("emplrName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Title">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="title"
                                                CommandName="sort" CssClass="gridlink">Title</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="location"
                                                CommandName="sort" CssClass="gridlink">Location</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbllocation" runat="server" Text='<%# Bind("location") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton5" runat="server" CausesValidation="False" CommandArgument="from"
                                                CommandName="sort" CssClass="gridlink">From</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblfrom" runat="server" Text='<%# Bind("from") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButto" runat="server" CausesValidation="False" CommandArgument="to"
                                                CommandName="sort" CssClass="gridlink">To</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblto" runat="server" Text='<%# Bind("to") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Duration">
                                        <ItemTemplate>
                                            <asp:Label ID="lblduration" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:ImageButton ID="deltebtn" runat="server" CausesValidation="False" Height="16px"
                                                    ImageUrl="~/images/delete.ico" CommandArgument="delete" OnClientClick="return confirm('Are you sure you want to delete the Employer?');" />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="emplrId">
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
                                    No Employer Details Added...
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
