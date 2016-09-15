<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLeaveDeduction.aspx.cs"
    Inherits="HR_AutoLeaveDeduction" EnableViewState="true" EnableEventValidation="false"
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
            width: 108px;
        }
        .style3
        {
            width: 97px;
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
                            <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;">Leaves Deduction
                                Options:</legend>
                            <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                <table width="100%">
                                    <tr>
                                        <td align="right" nowrap="nowrap" class="style2">
                                            <b>Select Year:</b>
                                        </td>
                                        <td align="left" class="style1">
                                            <asp:DropDownList ID="ddlYears" runat="server" CssClass="ddl" Width="90px" />
                                        </td>
                                        <td align="right" nowrap="nowrap" class="style3">
                                            <b>Select Year:</b>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlMonths" runat="server" CssClass="ddl" Width="130px">
                                                <asp:ListItem Text="Jan" Value="1" />
                                                <asp:ListItem Text="Feb" Value="2" />
                                                <asp:ListItem Text="Mar" Value="3" />
                                                <asp:ListItem Text="Apr" Value="4" />
                                                <asp:ListItem Text="May" Value="5" />
                                                <asp:ListItem Text="Jun" Value="6" />
                                                <asp:ListItem Text="Jul" Value="7" />
                                                <asp:ListItem Text="Aug" Value="8" />
                                                <asp:ListItem Text="Sep" Value="9" />
                                                <asp:ListItem Text="Oct" Value="10" />
                                                <asp:ListItem Text="Nov" Value="11" />
                                                <asp:ListItem Text="Dec" Value="12" />
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnDeduction" runat="server" Text="Send For Approval" ToolTip="Click here to run leaves deduction &amp; Send to admin for approval..!"
                                                OnClick="btnDeduction_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div>
                        <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto">
                            <fieldset style="margin-top: 1px; width: 95%; direction: inherit; height:350px">
                                <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;">Leaves Deduction's
                                    Summary:</legend>
                                <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" Height="100%" Width="100%" HeaderStyle-CssClass="gridheader"
                                                    BorderColor="Black" HeaderStyle-Wrap="false" RowStyle-HorizontalAlign="Center"
                                                    EmptyDataText="No Record(s) found it..!" ShowFooter="false" FooterStyle-BorderWidth="0px"
                                                    FooterStyle-Height="8px" FooterStyle-BackColor="#808080" OnRowDataBound="gv_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                            <ItemTemplate>
                                                                <%--<asp:Button ID="gvbuttonStatus" runat="server" Text="" Width="75px" Height="19px" Enabled="false" />--%>
                                                                <asp:Label ID="gvLabelStatus" runat="server" Text="" Width="40px" Height="15px" Enabled="false"
                                                                    BorderWidth="1px" />
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IsLock") %>' Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="YearMonth" HeaderText="Year-Month" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="Counter" HeaderText="Ded. Counter" HeaderStyle-Wrap="false" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="Deducted By" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CreatedDateTime" HeaderText="Deducted Datetime" />
                                                        <asp:BoundField DataField="Is_Lock" HeaderText="IsLock ?" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ApprovedDateTime" HeaderText="Approved DateTime" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
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
                                            <%-- <td align="right">
                                                <asp:Label ID="Label3" runat="server" Width="15px" Height="15px" BackColor="WhiteSmoke" BorderColor="Black"
                                                    BorderWidth="1px"></asp:Label>
                                            </td>
                                            <td align="left">
                                                There are not status available for this record.
                                            </td>--%>
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
