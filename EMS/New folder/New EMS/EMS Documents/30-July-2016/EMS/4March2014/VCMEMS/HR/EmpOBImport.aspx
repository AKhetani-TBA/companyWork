<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpOBImport.aspx.cs" Inherits="HR_EmpOBImport"
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
                <table>
                    <tr>
                        <td>
                            <div style="overflow-y: auto; overflow-x: auto;">
                                <fieldset style="margin-top: 5px; width: 850px; direction: inherit; height: 55px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="overflow-y: auto; overflow-x: auto;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="" BackColor="Red" Enabled="false" Width="15px"
                                                        Height="15px" />
                                                </td>
                                                <td>
                                                    Indicates Opening Balance <b>Not Entered</b>.
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button2" runat="server" Text="" BackColor="Green" Enabled="false"
                                                        Width="15px" Height="15px" />
                                                </td>
                                                <td>
                                                    Indicates Opening Balance <b>Entered</b>.
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button3" runat="server" Text="" BackColor="Yellow" Enabled="false"
                                                        Width="15px" Height="15px" />
                                                </td>
                                                <td>
                                                    Indicates Opening Balance <b>Edited</b>.
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow-y: auto; overflow-x: auto;">
                                <fieldset style="margin-top: 5px; width: 850px; direction: inherit; height: 55px">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                    <div style="overflow-y: auto; overflow-x: auto;">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <b>Select Department:</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                                        OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" Width="168px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <b>Employee:</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="showEmployees" runat="server" OnSelectedIndexChanged="ddlEmployees_SelectedIndexChanged"
                                                        Width="190px" CssClass="ddl">
                                                    </asp:DropDownList>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <b>OL-Balance Type:</b>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOlType" runat="server" Width="90px" CssClass="ddl">
                                                        <asp:ListItem Value="0" Text="All" Selected="True" />
                                                        <asp:ListItem Value="1" Text="Entered" />
                                                        <asp:ListItem Value="2" Text="Not Entered" />
                                                        <asp:ListItem Value="3" Text="Edited" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnShow" runat="server" CssClass="button" Text="Show" OnClick="btnShow_Click1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow-y: auto; overflow-x: auto;">
                                <fieldset style="margin-top: 5px; direction: inherit;">
                                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Employee Opening
                                        Leaves Balance </legend>
                                    <div style="overflow-y: auto; width: 850px; height: 400px; overflow-x: auto;">
                                        <asp:GridView ID="gvOL" runat="server" AutoGenerateColumns="False" Width="98%" HeaderStyle-CssClass="gridheader"
                                            TabIndex="1" ToolTip="Leave policy information..." OnPageIndexChanging="gvOL_PageIndexChanging"
                                            OnRowCancelingEdit="gvOL_RowCancelingEdit" OnRowCommand="gvOL_RowCommand" OnRowDeleting="gvOL_RowDeleting"
                                            OnRowEditing="gvOL_RowEditing" OnRowUpdating="gvOL_RowUpdating" OnRowDataBound="gvOL_RowDataBound">
                                            <RowStyle BorderColor="#333333" Height="100%" />
                                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                            <EmptyDataTemplate>
                                                No Record(s) found it...</EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="EmpId">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpId" runat="server" Text='<%# Bind("EmpId") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="16px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="DeptName" runat="server" Text='<%# Bind("DeptName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Month" HeaderStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="ddl" Width="80px" Enabled="false"
                                                            SelectedValue='<%# Bind("MM") %>'>
                                                            <asp:ListItem Text="January" Value="1" />
                                                            <asp:ListItem Text="February" Value="2" />
                                                            <asp:ListItem Text="March" Value="3" />
                                                            <asp:ListItem Text="April" Value="4" />
                                                            <asp:ListItem Text="May" Value="5" />
                                                            <asp:ListItem Text="Jun" Value="6" />
                                                            <asp:ListItem Text="July" Value="7" />
                                                            <asp:ListItem Text="August" Value="8" />
                                                            <asp:ListItem Text="September" Value="9" />
                                                            <asp:ListItem Text="Octomber" Value="10" />
                                                            <asp:ListItem Text="November" Value="11" />
                                                            <asp:ListItem Text="December" Value="12" />
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="75px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Bind("YYYY") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%-- <asp:BoundField HeaderText="PL OP-Bal." ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false"
                                                    HeaderStyle-Wrap="false" DataField="PL" ControlStyle-Width="50px" />
                                                <asp:BoundField HeaderText="CO OP-Bal." ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false"
                                                    HeaderStyle-Wrap="false" DataField="CO" ItemStyle-Width="50px" ControlStyle-Width="50px" />--%>
                                                <asp:TemplateField HeaderText="PL OL-Balance" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPL" runat="server" Text='<%# Bind("PL") %>' Visible="false" Width="60"
                                                            MaxLength="5" CssClass="txtclass"></asp:TextBox>
                                                        <asp:Label ID="lblPL" runat="server" Text='<%# Bind("PL") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CO OL-Balance" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCO" runat="server" Text='<%# Bind("CO") %>' Visible="false" Width="60"
                                                            MaxLength="5" CssClass="txtclass"></asp:TextBox>
                                                        <asp:Label ID="lblCO" runat="server" Text='<%# Bind("CO") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle Wrap="False" />
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="gvFlag" runat="server" BackColor="Red" Height="20" Width="20" Text=""
                                                            ToolTip='<%# Bind("LockFlag") %>' Enabled="false" />
                                                        <asp:Label ID="lblLockFalg" runat="server" Text='<%# Bind("LockFlag") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEdit" Text="EDIT" runat="server" CommandName="Edit" ForeColor="Black" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="btnUpdate" Text="UPDATE" runat="server" CommandName="Update"
                                                            ForeColor="Black" />&nbsp;|&nbsp;
                                                        <asp:LinkButton ID="btnCancel" Text="CANCEL" runat="server" CommandName="Cancel"
                                                            ForeColor="Black" />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="gridheader" />
                                        </asp:GridView>
                                    </div>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
