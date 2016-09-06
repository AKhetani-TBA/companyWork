<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GeneralDetails.aspx.cs" Inherits="HR_GeneralDetails"
    EnableEventValidation="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            overflow: hidden;
            width: 179px;
        }
        .headsty
        {
            font-size: 10pt;
            font-family: Verdana;
        }
    </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <div id="divSearch" runat="server">
                    <table style="margin-top: 5px">
                        <tr>
                            <td style="width: 91px" nowrap="nowrap">
                                Department&nbsp; : &nbsp;
                            </td>
                            <td style="width: 196px;" nowrap="nowrap">
                                <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" Width="168px"
                                    OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" CssClass="ddl">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 66px;" nowrap="nowrap">
                                Employee : &nbsp;
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" Width="171px"
                                    OnSelectedIndexChanged="showEmployees_SelectedIndexChanged" CssClass="ddl">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 50px;">
                                <asp:ImageButton ID="imgbtnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="imgbtnSearch_Click" />
                            </td>
                               <td > &nbsp;&nbsp;&nbsp;
                                  <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"   
                                        OnClick="btnexcel_Click" /><br />
                                </td>     
                        </tr>
                    </table>
                </div>
                <div id="divempdetails" runat="server" visible="false">
                    <fieldset style="padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Employee Information
                        </legend>
                        <table width="100%">
                            <tr>
                                <td style="width: 15%; font-weight: bold" align="left" class="headsty">
                                    Employee Name:
                                </td>
                                <td style="width: 35%;" align="left" class="headsty">
                                    <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%; font-weight: bold" align="left" class="headsty">
                                    Duration:
                                </td>
                                <td style="width: 35%;" align="left" class="headsty">
                                    <asp:Label ID="lblDuration" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Joining Date:
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lbljoin" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Education:
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lblEducation" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Department:
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lblDept" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Designation:
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lblDesi" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <%--<tr>
                            <td>
                                Birth Date:
                            </td>
                            <td>
                                <asp:Label ID="lblBirth" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <%--<tr>
                                <td style="font-weight: bold" class="headsty">
                                    Duration:
                                </td>
                                <td class="headsty">
                                    <asp:Label ID="lblDuration" runat="server"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Skill(s):
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lblSkill" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold" class="headsty">
                                    Project(s):
                                </td>
                                <td class="headsty" colspan="3">
                                    <asp:Label ID="lblProject" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnBack" runat="server" Text="Back" Font-Bold="true" OnClick="btnBack_Click" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="divGrid" runat="server">
                    <fieldset style="padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px;width: 100%; overflow-y: auto; overflow-x: auto;" > 
                            <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowDataBound="srchView_RowDataBound"
                                HorizontalAlign="Justify" Width="100%" OnPageIndexChanging="srchView_PageIndexChanging"
                                AllowPaging="false" PageSize="25" OnSelectedIndexChanged="srchView_SelectedIndexChanged" OnSorting="srchView_OnSorting" AllowSorting="true">
                                <RowStyle BorderColor="#333333" BorderWidth="0px" Height="20px" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                     ForeColor="White" Font-Bold="false"/>
                                <Columns>
                                    <asp:BoundField DataField="empName" SortExpression="empName" HeaderText="Employee Name" ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="Qualification" SortExpression="Qualification"   HeaderText="Education" ItemStyle-Width="15%" />
                                    <asp:BoundField DataField="deptName" SortExpression="deptName"  HeaderText="Department" ItemStyle-Width="6%" />
                                    <asp:BoundField DataField="empDomicile" SortExpression="empDomicile"   HeaderText="Designation" ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="empHireDate"  SortExpression="empHireDate"  HeaderText="Joining Date" ItemStyle-Width="6%"
                                        DataFormatString="{0:dd/MMM/yyyy}" />
                                    <%--<asp:BoundField DataField="empDOB" HeaderText="Birth Date" ItemStyle-Width="8%" DataFormatString="{0:dd/MMM/yyyy}" />--%>
                                    <asp:BoundField DataField="Duration" SortExpression="Duration" HeaderText="Duration (year)" ItemStyle-Width="5%" />
<%--                                    <asp:BoundField DataField="SkillName" HeaderText="Skill(s)" ItemStyle-Width="15%" />
--%>                                    <asp:BoundField DataField="ProjectName" SortExpression="ProjectName"  HeaderText="Project(s)" ItemStyle-Width="15%" />
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblemp" runat="server" Text='<%# Bind("empId") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record Found...</EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
<%--            <asp:PostBackTrigger ControlID="btnword" />
--%>        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
