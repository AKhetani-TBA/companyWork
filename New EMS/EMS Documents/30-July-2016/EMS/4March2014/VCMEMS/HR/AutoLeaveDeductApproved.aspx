<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLeaveDeductApproved.aspx.cs"
    Inherits="HR_AutoLeaveDeductApproved" EnableViewState="true" EnableEventValidation="false"
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
        .style1
        {
            width: 90px;
        }
        .style2
        {
            width: 88px;
        }
        .style3
        {
            width: 130px;
        }
        .style4
        {
            width: 102px;
        }
        .style6
        {
            width: 118px;
        }
        .style7
        {
            width: 152px;
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
                    <div id="Div6" runat="server" style="overflow-y: auto; overflow-x: auto">
                        <fieldset style="margin-top: 1px; width: 95%; direction: inherit; height: 100%">
                            <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;">Search:</legend>
                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                <table width="100%">
                                    <tr>
                                        <td align="right" nowrap="nowrap" class="style6">
                                            <b>Select Year:</b>
                                        </td>
                                        <td align="left" class="style1">
                                            <asp:DropDownList ID="ddlYears" runat="server" CssClass="ddl" Width="90px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" nowrap="nowrap" class="style2">
                                            <b>Select Month:</b>
                                        </td>
                                        <td align="left" class="style3">
                                            <asp:DropDownList ID="ddlMonths" runat="server" CssClass="ddl" Width="130px">
                                                <asp:ListItem Selected="True" Text="- - -ALL- - -" Value="0" />
                                                <asp:ListItem Text="JAN" Value="1" />
                                                <asp:ListItem Text="FEB" Value="2" />
                                                <asp:ListItem Text="MAR" Value="3" />
                                                <asp:ListItem Text="APR" Value="4" />
                                                <asp:ListItem Text="MAY" Value="5" />
                                                <asp:ListItem Text="JUN" Value="6" />
                                                <asp:ListItem Text="JUL" Value="7" />
                                                <asp:ListItem Text="AUG" Value="8" />
                                                <asp:ListItem Text="SEP" Value="9" />
                                                <asp:ListItem Text="OCT" Value="10" />
                                                <asp:ListItem Text="NOV" Value="11" />
                                                <asp:ListItem Text="DEC" Value="12" />
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" class="style4">
                                            <b>Apploved Mode:</b>
                                        </td>
                                        <td align="left" class="style7">
                                            <asp:DropDownList ID="ddlMode" runat="server" CssClass="ddl" Width="110px">
                                                <asp:ListItem Selected="True" Text="- - -ALL- - -" Value="-1" />
                                                <asp:ListItem Text="Pending" Value="0" />
                                                <asp:ListItem Text="Approved" Value="1" />
                                                <asp:ListItem Text="Not Approved" Value="2" />
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="../images/searchbtn.png"
                                                OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div>
                    <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto">
                        <fieldset style="margin-top: 1px; width: 95%; direction: inherit; height: 450px">
                            <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;"></legend>
                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="false" Width="100%"
                                    EmptyDataText="No record(s) found it." OnRowDataBound="gvMain_RowDataBound" HeaderStyle-CssClass="gridheader"
                                    BorderWidth="1px" BorderColor="Black" OnSelectedIndexChanging="gvMain_SelectedIndexChanging"
                                    OnRowCommand="gvMain_RowCommand">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Year Month" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvYearMonth" runat="server" Text='<%# Bind("YearMonth") %>'></asp:Label>
                                                <asp:Label ID="lblGvMonth" runat="server" Text='<%# Bind("Months") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblGvYear" runat="server" Text='<%# Bind("Years") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Deducted By" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDeductedBy" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Deducted Date"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvDeductedDateTime" runat="server" Text='<%# Bind("CreatedDateTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approved By" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvApprovedBy" runat="server" Text='<%# Bind("ApprovedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approved Date"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvApprovedDateTime" runat="server" Text='<%# Bind("ApprovedDateTime") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:DropDownList ID="ddlGVStatus" runat="server" CssClass="ddl">
                                                                <asp:ListItem Selected="True" Text="Pending" Value="0" />
                                                                <asp:ListItem Text="Approved" Value="1" />
                                                                <asp:ListItem Text="Not Approved" Value="2" />
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btngv" runat="server" Text="Save" Height="20px" CssClass="button"
                                                                OnClick="btngv_Click" CommandName="Select" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' />
                                                        </td>
                                                        <td runat="server" id="tdislock" visible="false">
                                                            <asp:Label ID="lblGvIsLock" runat="server" Text='<%# Bind("IsLock") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="gridheader" />
                                </asp:GridView>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div>
                    <div id="Div2" runat="server" style="overflow-y: auto; overflow-x: auto">
                        <fieldset style="margin-top: 1px; width: 95%; direction: inherit; height: 100%">
                            <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;"></legend>
                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label2" runat="server" Width="15px" Height="15px" BackColor="Yellow"
                                                BorderColor="Black" BorderWidth="1px"></asp:Label>
                                        </td>
                                        <td align="left">
                                            Deducted,But pending for admin action..!
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label1" runat="server" Width="15px" Height="15px" BackColor="Green"
                                                BorderColor="Black" BorderWidth="1px"></asp:Label>
                                        </td>
                                        <td align="left">
                                            Deducted & approved by admin...!
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl2" runat="server" Width="15px" Height="15px" BackColor="Red" BorderColor="Black"
                                                BorderWidth="1px"></asp:Label>
                                        </td>
                                        <td align="left">
                                            Deducted,But rejected by admin..!
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        &nbsp;&nbsp;&nbsp;</div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
