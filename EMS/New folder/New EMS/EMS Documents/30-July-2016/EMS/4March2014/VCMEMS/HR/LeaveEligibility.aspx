<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveEligibility.aspx.cs"
    Inherits="HR_LeaveEligibility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 510px;
            height: 100px;
            float: left;
        }
        .itemstrSty
        {
            text-align:left;
        }
        .itemdigitSty
        {
            text-align:right;
        }
        #tdResult td
        {
            border: solid 1px red;
        }
    </style>

    <script type="text/javascript">

        function sum() {
            var txtCL = document.getElementById('txtCL').value;
            var txtSL = document.getElementById('txtSL').value;
            var txtPL = document.getElementById('txtPL').value;
            var txtVPL = document.getElementById('txtVPL').value;
            var txtVOL = document.getElementById('txtVOL').value;
            var result = parseFloat(txtCL) + parseFloat(txtSL) + parseFloat(txtPL) + parseFloat(txtVPL) + parseFloat(txtVOL);
            if (!isNaN(result)) {
                document.getElementById('txtTotal').value = result;
            }
            else {
                document.getElementById('txtTotal').value = 0.0;
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
            <div class="EMS_font" id="mainDiv">
                <br />
                <div id="searchPane" runat="server">
                    <table width="100%">
                        <tr>
                            <%--<td style="width: 6%" align="left">
                                Form Date:&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtFormDate" runat="server" Font-Bold="True" Width="95px"></asp:TextBox>
                                <asp:CalendarExtender ID="attendaceDate" runat="server" TargetControlID="txtFormDate"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td style="width: 7%" align="right">
                                To Date :&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 7%">
                                <asp:TextBox ID="txtTodate" runat="server" Font-Bold="True" Width="95px"></asp:TextBox>
                                <asp:CalendarExtender ID="attendancetodate" runat="server" TargetControlID="txtTodate"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>--%>
                            <td style="width: 8%" align="right">
                                Employee Name:&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 12%">
                                <asp:DropDownList ID="ddlEmpName" runat="server" Width="170px" 
                                  />
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
                            <td style="width: 14%" align="left">
                                &nbsp;DOJ :<asp:Label ID="lblDoj" runat="server" Width="80%" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <%--<div id="searchResults" runat="server">
                    <table width="100%" style="border: solid 1x black;">
                        <tr>
                            <td>
                                <table width="50%" style="border: solid 1px black;">
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            Type
                                        </td>
                                        <td align="center" style="width: 5%">
                                            &nbsp;&nbsp;CL
                                        </td>
                                        <td align="center" style="width: 5%">
                                            &nbsp;&nbsp;SL
                                        </td>
                                        <td align="center" style="width: 5%">
                                            &nbsp;&nbsp;PL
                                        </td>
                                        <td align="center" style="width: 5%">
                                            &nbsp;VPL
                                        </td>
                                        <td align="center" style="width: 5%">
                                            &nbsp;VOL
                                        </td>
                                        <td align="center" style="width: 5%">
                                            Total
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="50%" style="border: solid 1px black;">
                                    <tr>
                                        <td align="left" style="width: 5%">
                                            Eligibility
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrcl" runat="server" align="center"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrsl" runat="server" align="center"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrpl" runat="server" align="center"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrvpl" runat="server" align="center"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrvol" runat="server" align="center"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%">
                                            <asp:Label ID="lblhrtotal" runat="server" align="center"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <div id="divGrid" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="width: 33%;font-weight:bold;" align="center">
                                ENTITLEMENT LEAVE
                            </td>
                            <td style="width: 33%;font-weight:bold;" align="center">
                                LEAVE TAKEN
                            </td>
                            <td style="width: 34%;font-weight:bold;" align="center">
                                LEAVE BALANCE IN DETAIL
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" valign="top">
                                <asp:GridView ID="gvleavehr" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                    HorizontalAlign="Justify" OnRowCommand="gvleavehr_RowCommand" OnRowDataBound="gvleavehr_RowDataBound"
                                    Width="100%" ShowFooter="true">
                                    <RowStyle BorderColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                    <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                        ForeColor="White" Height="19px" />
                                    <FooterStyle CssClass="" Height="21px" BorderStyle="Solid"   Font-Bold="true" />
                                 
                                    <Columns>
                                        <%--<asp:BoundField DataField="EmpName" HeaderText="Employee Name" ItemStyle-Width="10%" />--%>
                                        <%--<asp:BoundField DataField="Entitlement_Yr" HeaderText="Year" ItemStyle-Width="10%" /> --%>
                                        <%--<asp:BoundField DataField="LMonthName" HeaderText="Month" ItemStyle-Width="6%" />--%>
                                        <asp:TemplateField HeaderText="Month" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"
                                            ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="5%" 
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("LMonthName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemstrSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfMonth" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemstrSty" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="CL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCL" runat="server" Text='<%# Eval("Ent_CL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSL" runat="server" Text='<%# Eval("Ent_SL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfSL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPL" runat="server" Text='<%# Eval("Ent_PL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfPL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VPL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVPL" runat="server" Text='<%# Eval("Ent_VPL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfVPL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VOL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVOL" runat="server" Text='<%# Eval("Ent_VOL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfVOL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCO" runat="server" Text='<%# Eval("Ent_CO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCO" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Ent_Tot") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfTotal" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <%--LEAVE TAKEEN--%>
                                        <asp:BoundField ItemStyle-BackColor="#959595" FooterStyle-BackColor="#959595"  HeaderText="" ItemStyle-Width="1%" />
                                        <asp:TemplateField HeaderText="OL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAOL" runat="server" Text='<%# Eval("Allot_OL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfAOL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leaves" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblALeave" runat="server" Text='<%# Eval("Allot_Tot_Except_OL_LWP") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfALeave" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="LWP" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblALWP" runat="server" Text='<%# Eval("Allot_LeaveWithoutPay") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfALWP" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MonthDaysOff" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="4%" ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center"
                                            FooterStyle-Width="4%" HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblATDO" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfATDO" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <%--LEAVE BALANCE in DETAIL--%>
                                        <asp:BoundField HeaderText="" ItemStyle-BackColor="#959595" FooterStyle-BackColor="#959595"  ItemStyle-Width="1%" />
                                        <asp:TemplateField HeaderText="CL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCL" runat="server" Text='<%# Eval("Calc_Bal_CL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />                                            
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCCL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCSL" runat="server" Text='<%# Eval("Calc_Bal_SL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCSL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCPL" runat="server" Text='<%# Eval("Calc_Bal_PL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCPL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VPL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCVPL" runat="server" Text='<%# Eval("Calc_Bal_VPL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCVPL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VOL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCVOL" runat="server" Text='<%# Eval("Calc_Bal_VOL") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCVOL" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCCO" runat="server" Text='<%# Eval("Calc_Bal_CO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="itemdigitSty" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCCO" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="4%"
                                            ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" FooterStyle-Width="4%"
                                            HeaderStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCBal" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                            </ItemTemplate>       
                                            <ItemStyle CssClass="itemdigitSty" />                                                                                 
                                            <FooterTemplate>
                                                <asp:Label ID="lblfCBal" runat="server" ForeColor="Navy" />
                                            </FooterTemplate>
                                            <FooterStyle CssClass="itemdigitSty" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Leave Record Found
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <input id="hidhrleaveID" runat="server" type="hidden" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- <div id="assignLeave" runat="server" visible="false">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update
                            Leave&nbsp; </legend>
                        <table class="style1">
                            <tr>
                                <td>
                                    CL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCL" runat="server" onkeyup="sum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    SL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSL" runat="server" onkeyup="sum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    PL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPL" runat="server" onkeyup="sum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    VPL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVPL" runat="server" onkeyup="sum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    VOL
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVOL" runat="server" onkeyup="sum();"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotal" runat="server" Font-Bold="True" Width="145px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                            ShowMessageBox="True" ShowSummary="False" />
                    </fieldset>
                </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
