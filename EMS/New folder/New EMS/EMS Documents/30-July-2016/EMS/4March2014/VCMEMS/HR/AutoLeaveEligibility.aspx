<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoLeaveEligibility.aspx.cs"
    Inherits="HR_AutoLeaveEligibility" EnableEventValidation="false" Async="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script type="text/javascript" ="javascript">
        var i = 0;
        function showCustomMsg() {
            document.getElementById("feed").style.display = "block";
            hidefeed();
        }
        function hidefeed() {
            
            i = i + 1;
            if (i >= 5) {
                i = 0;
                document.getElementById("feed").style.display = "none";
            }
            else {
                setTimeout("hidefeed();", "250");
            }
        }
    </script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .gvItemStyle
        {
            vertical-align: middle;
            text-align: center;
            border-color: Black;
            border-width: 1px;
        }
    </style>
    <style type="text/css">
        .itemstrSty
        {
            text-align: left;
        }
        .itemdigitSty
        {
            text-align: right;
        }
        #tdResult td
        {
            border: solid 1px red;
        }
    </style>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form2" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
        <ContentTemplate>
            <div class="EMS_font" id="mainDiv" runat="server">
                <div id="Div6" runat="server" style="overflow-y: auto; overflow-x: auto">
                    <fieldset style="margin-top: 1px; width: 98%; direction: inherit; height: 50px">
                        <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;">Search:</legend>
                        <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                            <table width="100%">
                                <tr>
                                    <td style="width: 8%" align="right">
                                        Employee Name:&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 12%">
                                        <asp:DropDownList ID="ddlEmpName" runat="server" Width="170px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged" />
                                    </td>
                                    <td style="width: 8%" align="right">
                                        Year :
                                    </td>
                                    <td style="width: 12%">
                                        <asp:DropDownList ID="ddlYear" runat="server" Width="170px" />
                                    </td>
                                    <td style="width: 4%" align="center">
                                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                            OnClick="btnSearch_Click" />
                                    </td>
                                    <td style="width: 14%" align="right">
                                        <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"
                                            OnClick="btnexcel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <div id="Div1" runat="server" style="overflow-y: auto; overflow-x: auto;">
                    <fieldset style="margin-top: 1px; width: 98%; height: 455px; direction: inherit;">
                        <legend style="margin-bottom: 1px; font-weight: normal; color: #808080;"></legend>
                        <div id="Div1_1" runat="server" style="width: 100%; overflow-y: auto; overflow-x: auto;">
                            <br />
                            <asp:GridView ID="GvLeaves" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                                ShowFooter="false" DataKeyNames="empId" AllowPaging="false" AllowSorting="false"
                                Width="100%" Height="100%" EmptyDataText="No Record(s) Found it." OnRowDataBound="GvLeaves_RowDataBound">
                                <EmptyDataTemplate>
                                    No Leave Record Found
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="100%">
                                        <ItemTemplate>
                                            <table width="100%" style="border: 1px solid #000000;">
                                                <tr>
                                                    <td colspan="3" align="center" style="border: 1px solid #000000; background-color: #808080;
                                                        color: White">
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" nowrap="nowrap" style="width: 55%; font-family: Arial; font-size: 14px">
                                                                    <b>Employee Name:</b>
                                                                    <asp:Label ID="gvlblempName" runat="server" Text='<%# Bind("empName") %>' />
                                                                    (<b>Dept.:</b>
                                                                    <asp:Label ID="gvlblDeptName" runat="server" Text='<%# Bind("deptName") %>' />
                                                                    )
                                                                </td>
                                                                <td align="left" nowrap="nowrap" style="width: 23%; font-family: Arial; font-size: 14px">
                                                                    <b>Date Of Joining:</b>
                                                                    <asp:Label ID="gvlblDOJ" runat="server" Text='<%# Bind("empHireDate") %>' />
                                                                </td>
                                                                <td align="left" nowrap="nowrap" style="width: 22%; font-family: Arial; font-size: 14px">
                                                                    <b>Duration With Vypar:</b>
                                                                    <asp:Label ID="gvlblJourney" runat="server" Text='<%# Bind("JourneyTime") %>' />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="background-color: White">
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="border: 1px solid #000000; background-color: #808080;">
                                                        <asp:Label ID="title1" runat="server" Font-Bold="true" Text="ENTITLEMENT LEAVE" ForeColor="White" />
                                                    </td>
                                                    <td align="center" style="border: 1px solid #000000; background-color: #808080;">
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="TAKEN LEAVES" ForeColor="White" />
                                                    </td>
                                                    <td align="center" style="border: 1px solid #000000; background-color: #808080;">
                                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="BALANCE LEAVES" ForeColor="White" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center" style="border: 1px solid #000000; background-color: White;">
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdentiliment" runat="server" style="border: 1px solid #000000;">
                                                        <asp:GridView ID="gvEntilments" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="gridheader"
                                                            Width="100%" ShowFooter="true" AlternatingRowStyle-BackColor="LightGray">
                                                            <RowStyle BorderColor="#333333" />
                                                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                            <HeaderStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" BorderColor="Black" BorderWidth="1px" />
                                                            <FooterStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" HorizontalAlign="Center" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Year-Month" HeaderStyle-Width="100px" HeaderStyle-Wrap="false"
                                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                                                    FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementYearMonth" runat="server" Text='<%# Bind("YearMonth") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        Total:
                                                                        <br />
                                                                        Grand Total:
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementCL" runat="server" Text='<%# Bind("CL_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalCL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalCL" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementSL" runat="server" Text='<%# Bind("SL_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalSL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalSL" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementPL" runat="server" Text='<%# Bind("PL_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalPL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalPL" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VOL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementVOL" runat="server" Text='<%# Bind("VOL_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalVOL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalVOL" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VPL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementVPL" runat="server" Text='<%# Bind("VPL_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalVPL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalVPL" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementCO" runat="server" Text='<%# Bind("CO_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalCO" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalCO" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total" ItemStyle-Font-Bold="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementTotal" runat="server" Text='<%# Bind("Total_Entilement") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvEntilementTotalTotal" runat="server" Text="0.00" />
                                                                        <br />
                                                                        <asp:Label ID="gvEntilementGTotalTotal" runat="server" Text="0.00" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td id="tdtaken" runat="server" style="border: 1px solid #000000;">
                                                        <asp:GridView ID="gvTaken" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="gridheader"
                                                            Width="100%" Height="100%" ShowFooter="true" AlternatingRowStyle-BackColor="LightGray"
                                                            OnRowDataBound="gvTaken_RowDataBound">
                                                            <RowStyle BorderColor="#333333" Width="100%" />
                                                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                            <HeaderStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" BorderColor="Black" BorderWidth="1px" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" HorizontalAlign="Center" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="CL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveCL" runat="server" Text='<%# Bind("CL_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalCL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveSL" runat="server" Text='<%# Bind("SL_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalSL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeavePL" runat="server" Text='<%# Bind("PL_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalPL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VOL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvEntilementVOL" runat="server" Text='<%# Bind("VOL_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalVOL" runat="server" Text="0.00" /><br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VPL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveVPL" runat="server" Text='<%# Bind("VPL_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalVPL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CO">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveCO" runat="server" Text='<%# Bind("CO_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalCO" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="UPL">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveUPL" runat="server" Text='<%# Bind("UnPaidLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalUPL" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="LPP">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveLPP" runat="server" Text='<%# Bind("Oth_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalLPP" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total" ItemStyle-Font-Bold="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotal" runat="server" Text='<%# Bind("Total_TakenLeave") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="gvTakenLeaveTotalTotal" runat="server" Text="0.00" />
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                    <td id="tdbalance" runat="server" style="border: 1px solid #000000;">
                                                        <asp:GridView ID="gvBalance" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="gridheader"
                                                            Width="100%" Height="100%" ShowFooter="true" AlternatingRowStyle-BackColor="LightGray"
                                                            OnRowDataBound="gvBalance_RowDataBound">
                                                            <RowStyle BorderColor="#333333" Width="100%" />
                                                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                                            <HeaderStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" BorderColor="Black" BorderWidth="1px" />
                                                            <FooterStyle BackColor="#808080" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                                                ForeColor="White" Height="19px" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="CL" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceCL" runat="server" Text='<%# Bind("CL_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceSL" runat="server" Text='<%# Bind("SL_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PL" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalancePL" runat="server" Text='<%# Bind("PL_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VOL" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceVOL" runat="server" Text='<%# Bind("VOL_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VPL" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceVPL" runat="server" Text='<%# Bind("VPL_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="CO" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceCO" runat="server" Text='<%# Bind("CO_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Font-Bold="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="gvBalanceTotal" runat="server" Text='<%# Bind("Total_Balance") %>' />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        --
                                                                        <br />
                                                                        --
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <ItemStyle Width="100%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                </div>
                <div id="Div2" runat="server" style="overflow-y: auto; overflow-x: auto">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <div style="width: 100%; overflow-y: auto; overflow-x: auto;">
                                    <fieldset style="margin-top: 1px; width: 90%; direction: inherit;">
                                        <legend style="margin-bottom: 1px; height: 90%; font-weight: normal; color: #808080;">
                                            Descriptions</legend>
                                        <div style="overflow-y: auto; overflow-x: auto;">
                                            <table id="tbldescription" runat="server" style="width: 100%;">
                                                <tr>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000; width: 20%">
                                                        <b>CL:</b> Casual Leave <b>
                                                    </td>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000; width: 20%">
                                                        </b> <b>SL:</b> Sick Leave&nbsp; <b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000; width: 20%">
                                                        <b>PL</b> Privilege Leave
                                                    </td>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000; width: 20%">
                                                        <b>VOL:</b> Vypar Optional Leave
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000; width: 20%">
                                                        <b>VPL:</b> Vypar Privilege Leave
                                                    </td>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000;">
                                                        <b>CO:</b> Compansatory Off (Comp Off)
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000;">
                                                        <b>UPL</b> UnPaid Leave
                                                    </td>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000;">
                                                        <b>LPP:</b> Leave(s) Posting Not Processed
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000;">
                                                    </td>
                                                    <td align="left" nowrap="nowrap" style="border: 1px solid #000000;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </fieldset>
                                </div>
                                <div style="overflow-y: auto; overflow-x: auto;">
                                    <fieldset style="margin-top: 1px; width: 90%; direction: inherit;">
                                        <legend style="margin-bottom: 1px; height: 100%; font-weight: normal; color: #808080;">
                                            Previous Year Attendance Summary:</legend>
                                        <div style="overflow-y: auto; overflow-x: auto;">
                                            <asp:GridView ID="gvprivousAttendance" runat="server" Width="100%" AutoGenerateColumns="true"
                                                HeaderStyle-CssClass="gridheader" BorderColor="Black" BorderWidth="1px" RowStyle-BorderWidth="1px"
                                                RowStyle-BorderColor="Black" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                                EmptyDataText="No Record(s) Found it." ShowFooter="false">
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </div>

                            </td>
                            <td align="right">
                            <div style="overflow-y: auto; overflow-x: auto;">
                                    <fieldset style="margin-top: 1px; width: 90%; direction: inherit;">
                                        <legend style="margin-bottom: 1px; height: 100%; font-weight: normal; color: #808080;">
                                            Holiday List :</legend>
                                        <div style="overflow-y: auto; overflow-x: auto;">
                                            <asp:GridView ID="gvHoliday" runat="server" Width="100%" AutoGenerateColumns="false"
                                                HeaderStyle-CssClass="gridheader" BorderColor="Black" BorderWidth="1px" RowStyle-BorderWidth="1px"
                                                RowStyle-BorderColor="Black" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                                EmptyDataText="No Record(s) Found it." EnableModelValidation="True">
                                                <Columns>
<%--                                                    <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
--%>                                                    <asp:BoundField DataField="LeaveTypeName" HeaderText="Leave Type" SortExpression="Location" />
                                                    <asp:BoundField DataField="StartDate" HeaderText="Date" SortExpression="Location" DataFormatString="{0:dd-MMM-yyyy}"/>
                                                    <asp:BoundField DataField="Purpose" HeaderText="Purpose" SortExpression="Location" />
                                                </Columns>
                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Center" />
                                                <RowStyle BorderColor="Black" BorderWidth="1px" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </div>
                            </td>
                            </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
