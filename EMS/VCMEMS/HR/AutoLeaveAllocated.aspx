<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLeaveAllocated.aspx.cs"
    Inherits="HR_AutoLeaveAllocated" EnableViewState="true" EnableEventValidation="false"
    Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .style23
        {
            width: 150px;
        }
        .style24
        {
            width: 179px;
        }
        .style25
        {
            width: 104px;
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
                <div>
                    <div style="overflow-y: auto; overflow-x: auto;">
                        <fieldset style="margin-top: 5px; width: 850px; direction: inherit; height: 40px">
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
                                            <asp:DropDownList ID="showEmployees" runat="server" Width="190px" CssClass="ddl">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnShow" runat="server" CssClass="button" Text="Show" 
                                                onclick="btnShow_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <div style="overflow-y: auto; overflow-x: auto;">
                            <fieldset style="margin-top: 5px; width: 850px; direction: inherit; height: 40px">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                                <div style="overflow-y: auto; overflow-x: auto;">
                                    <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                        Width="100%" AllowPaging="false" OnRowDataBound="gvMain_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr class="gridheader">
                                                            <td style="width: 110px; text-align: right">
                                                                <b>Employee Name:</b>
                                                            </td>
                                                            <td style="width: 200px; text-align: left">
                                                                <asp:Label ID="lblMainEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                                                <asp:Label ID="lblMainEmpId" runat="server" Text='<%# Eval("EmpId") %>' Visible="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 110px; text-align: right">
                                                                <b>Date Of Joining:</b>
                                                            </td>
                                                            <td style="width: 75px; text-align: left">
                                                                <asp:Label ID="lblMain" runat="server" Text='<%# Eval("HireDate") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 130px; text-align: right">
                                                                <b>Emp Duration:</b>
                                                            </td>
                                                            <td style="width: 75px; text-align: left">
                                                                <asp:Label ID="lblMainTimePeriod" runat="server" Text='<%# Eval("TimePeriod") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 130px; text-align: right">
                                                                <b>Department Name:</b>
                                                            </td>
                                                            <td style="width: 120px; text-align: left">
                                                                <asp:Label ID="lblMainDeptName" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="8">
                                                                <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                                                    Width="100%" AllowPaging="false">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Leave Types" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDetLeaveTypes" runat="server" Text='<%# Eval("Leave_Type") %>'
                                                                                    MaxLength="5" Width="50px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="JAN">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetJAN" runat="server" Text='<%# Eval("JAN") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="FEB">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetFEB" runat="server" Text='<%# Eval("FEB") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="MAR">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetMAR" runat="server" Text='<%# Eval("MAR") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="APR">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetAPR" runat="server" Text='<%# Eval("APR") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="MAY">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetMAY" runat="server" Text='<%# Eval("MAY") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="JUN">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetJUN" runat="server" Text='<%# Eval("JUN") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="JUL">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetJUL" runat="server" Text='<%# Eval("JUL") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="AUG">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetAUG" runat="server" Text='<%# Eval("AUG") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SEP">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetSEP" runat="server" Text='<%# Eval("SEP") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="OCT">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetOCT" runat="server" Text='<%# Eval("OCT") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="NOV">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetNOV" runat="server" Text='<%# Eval("NOV") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="DEC">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDetDEC" runat="server" Text='<%# Eval("DEC") %>' MaxLength="5"
                                                                                    Width="40px" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%--<asp:TemplateField HeaderText="JAN">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtDetJAN" runat="server" Text='<%# Eval("JAN") %>' Width="40px"
                                                                                MaxLength="5" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
