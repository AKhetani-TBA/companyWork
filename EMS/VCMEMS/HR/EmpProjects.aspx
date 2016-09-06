<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpProjects.aspx.cs" Inherits="HR_EmpProjects" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Check() {
            var valcmbEmp = document.getElementById('ddlEmp').value;
            var valcmbProject = document.getElementById('ddlProject').value;
            var valFromDate = document.getElementById('txtFrom').value;
            var valToDate = document.getElementById('txtTo').value;
            var valcmbProjectrole = document.getElementById('ddlrole').value;

            var selectedProject;
            if (valcmbProject != "- Select Project -") {
                if (valcmbProject == "11") {
                    if (document.getElementById('txtProject').value == "") {
                        alert("Please enter project name.");
                        document.getElementById('txtProject').focus();
                        return false;
                    }
                }
            }

            if (valcmbProjectrole != "- Select Project Role -") {
                if (valcmbProjectrole == "4") {
                    if (document.getElementById('txtrole').value == "") {
                        alert("Please enter project role.");
                        document.getElementById('txtrole').focus();
                        return false;
                    }
                }
            }

            if (valcmbEmp == "- Select Employee -") {
                alert("Please select employee");
                document.getElementById('ddlEmp').focus();
                return false;
            }
            else if (valcmbProject == "- Select Project -") {
                alert("Please select project");
                document.getElementById('ddlProject').focus();
                return false;
            }
            else if (valcmbProjectrole == "- Select Project Role -") {
                alert("Please select project role");
                document.getElementById('ddlrole').focus();
                return false;
            }
            else if (valFromDate == "") {
                alert("Please enter fromdate");
                document.getElementById('txtFrom').focus();
                return false;
            }

        }

    </script>

</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <br />
                <table id="proj" runat="server" width="100%">
                    <tr>
                        <td>
                            Department :
                        </td>
                        <td>
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                Width="168px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
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
                            <td style="width: 50px;">
                                <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="imgbtnSearch_Click" />
                            </td>
                              <td>
                            <asp:Button ID="btnAddProject" runat="server" Font-Bold="False" ForeColor="#333333"
                                Text="Add Project / Product" CausesValidation="False" OnClick="btnAddProject_Click"
                                CssClass="button" />
                        </td>
                                                       <td > &nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"   
                                        OnClick="btnexcel_Click" /><br />
                                </td>     
                    </tr>
                </table>
                <div id="employerDiv" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Project /
                            Product Details </legend>
                        <table>
                            <tr>
                                <td>
                                    Employee<span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEmp" runat="server" AutoPostBack="false" Width="200px" CssClass="ddl">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Project/ Product <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" Width="200px"
                                        CssClass="ddl" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trpro" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProject" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Role <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlrole" runat="server" AutoPostBack="true" Width="200px" CssClass="ddl"
                                        OnSelectedIndexChanged="ddlrole_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trrole" runat="server">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtrole" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Time Period <span style="color: #FF0000">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFrom" runat="server" Width="150px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtFrom_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                        TargetControlID="txtFrom">
                                    </asp:CalendarExtender>
                                    &nbsp; &nbsp;&nbsp; to&nbsp; &nbsp;<asp:TextBox ID="txtTo" runat="server" Width="150px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtTo_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                        TargetControlID="txtTo">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Remark :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="addEmployer" runat="server" Font-Bold="False" Font-Names="Tw Cen MT Condensed"
                                        Font-Size="18px" ForeColor="#333333" Height="28px" OnClientClick="javascript:return Check()"
                                        OnClick="addEmployer_Click" Text="Save" />
                                    &nbsp;
                                    <asp:Button ID="reset" runat="server" CausesValidation="False" Font-Bold="False"
                                        Font-Names="Tw Cen MT Condensed" Font-Size="18px" ForeColor="#333333" Height="28px"
                                        OnClick="reset_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="BindDiv" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px; width: 100%; overflow-y: auto; overflow-x: auto;">
                            <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                                OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged"
                                OnSorting="displayAll_OnSorting" AllowSorting="true" PageSize="20" Width="100%">
                                <RowStyle BorderColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                     ForeColor="White" Font-Bold="false"/>
                                <Columns>
                                    <asp:TemplateField HeaderText="Employee">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empName"
                                                CommandName="sort" CssClass="gridlink">Employee</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblempname" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product / Project">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="projectName"
                                                CommandName="sort" CssClass="gridlink">Product / Project</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblprojectname" runat="server" Text='<%# Bind("Projectname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton22" runat="server" CausesValidation="False" CommandArgument="roleName"
                                                CommandName="sort" CssClass="gridlink">Role</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblrolename" runat="server" Text='<%# Bind("roleName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="fromDate"
                                                CommandName="sort" CssClass="gridlink">From</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblfrom" runat="server" Text='<%# Bind("FromDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To"  >
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandArgument="location"
                                               CssClass="gridlink">To</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate  >
                                            <asp:Label ID="lblto" runat="server" Text='<%# Bind("ToDate") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:ImageButton ID="deltebtn" runat="server" CausesValidation="False" Height="16px"
                                                    ImageUrl="~/images/delete.ico" CommandArgument="delete" OnClientClick="return confirm('Are you sure you want to delete the project?');" />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="16px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Id">
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
                                    No Project / Product Details Added...
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <asp:HiddenField ID="hidproject" runat="server" />
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
