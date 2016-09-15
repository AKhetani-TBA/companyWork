<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopUp.aspx.cs" Inherits="PopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .style2
        {
            font: 8pt;
            font-family: Verdana;
        }
        #imgload
        {
            width: 170px;
            height: 12px;
        }
        #imgload2
        {
            height: 12px;
            width: 166px;
        }
        .linkNoUnderline a
        {
            text-decoration: none;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function showlogdiv() {
            var dv = document.getElementById("in_out_logs");
            dv.style.pixelLeft = event.x - 380;
            dv.style.pixelTop = event.y + 15;
        }             
      
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblEmpName" runat="server" Font-Bold="true" ForeColor="#993300" Font-Names="Calibri"/><br /><br />
                </td>
            </tr>
                        <tr>
                        <td colspan="2">
               <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" 
                                AllowSorting="true"  Width="100%" >
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                    Font-Size="17px" ForeColor="White" Height="19px" />
                                <RowStyle Wrap="true" />
                                <Columns>
                                            <%--    <asp:BoundField DataField="empName" HeaderText="Employee" SortExpression="empName"
                                        ItemStyle-Width="10%" />--%>
                                    <asp:BoundField DataField="deptName" HeaderText="Department" SortExpression="deptName"
                                        ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="intime" HeaderText="In Time" SortExpression="intime" ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="outtime" HeaderText="Out Time" SortExpression="outtime"
                                        ItemStyle-Width="7%" />
                                    <asp:BoundField DataField="GrossTime" HeaderText="Gross Hrs" SortExpression="GrossTime"
                                        ItemStyle-Width="6%" />
                                    <asp:BoundField DataField="DurationOUT" HeaderText="Net Out" SortExpression="DurationOUT"
                                        ItemStyle-Width="6%" />
                                    <asp:BoundField DataField="DurationIn" HeaderText="Net In" SortExpression="DurationIn"
                                        ItemStyle-Width="6%" />
                                        </Columns>
                                        </asp:GridView><br /><br />          
</td>                              
            </tr>

            <tr >
                <td colspan="2">
                    <asp:GridView ID="gvLogDetails" runat="server" AutoGenerateColumns="False" GridLines="Both"
                        CellPadding="0" CellSpacing="0" Width="100%" ShowFooter="true">
                        <HeaderStyle CssClass="gridheader" />
                        <Columns>
                            <asp:BoundField DataField="Card No" HeaderText="Card" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="25%" ItemStyle-Font-Names="Verdana" ItemStyle-Font-Size="Small"
                                ItemStyle-Width="25%" />
                            <asp:BoundField DataField="Machine Code" HeaderText="Machine Code" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Font-Names="Verdana" ItemStyle-Font-Size="Small" ItemStyle-Width="25%"
                                HeaderStyle-Width="25%" />
                            <asp:BoundField DataField="DATE" HeaderText="DATE" ItemStyle-Font-Names="Verdana"
                                ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25%"
                                ItemStyle-Width="25%"></asp:BoundField>
                            <asp:BoundField DataField="TIME" ItemStyle-Font-Names="Verdana" ItemStyle-Font-Size="Small"
                                HeaderText="TIME" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25%"
                                DataFormatString="{0:hh:mm}" ItemStyle-Width="25%"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No records to display
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <%-- <tr>
                <td>
                    <asp:GridView ID="gvdetails" runat="server" AutoGenerateColumns="False" GridLines="Both"
                        CellPadding="0" CellSpacing="0" Width="100%" ShowFooter="true">
                        <HeaderStyle CssClass="gridheader" />
                        <Columns>
                            <asp:BoundField DataField="CheckIn" HeaderText="CheckIn-Log" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="10%" ItemStyle-CssClass="style2" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="CheckOut" HeaderText="CheckOut-Log" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-CssClass="style2" ItemStyle-Width="10%" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="Duration" HeaderText="Duration In Time" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="10%" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:BoundField DataField="DurationOutTime" HeaderText="Duration Out Time" ItemStyle-CssClass="style2" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="10%" ItemStyle-Width="10%"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            No records to display
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>--%>
            <%--   <tr>
                <td style="width: 12%" class="gridheader">
                    In-Log :
                </td>
                <td style="width: 88%">
                    <asp:Label ID="lblChkIn" Font-Size="Small" Font-Names="Verdana" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="gridheader">
                    In-Time :
                </td>
                <td>
                    <asp:Label ID="lblDurIn" Font-Size="Small" Font-Names="Verdana" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="gridheader">
                    Out-Log :
                </td>
                <td>
                    <asp:Label ID="lblChkOut" Font-Size="Small" Font-Names="Verdana" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="gridheader">
                    Out-Time :
                </td>
                <td>
                    <asp:Label ID="lblDurOut" Font-Size="Small" Font-Names="Verdana" runat="server"></asp:Label>
                </td>
            </tr>
            --%>
            <tr>
                <td style="width: 50%">
                    <fieldset id="in_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                        padding-bottom: 5px; padding-left: 5px; width: 250px; padding-right: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Inside Details
                        </legend>
                        <asp:Label ID="lblDetailLogs" runat="server" Style="text-align: right" CssClass="EMS_font_small"></asp:Label>
                    </fieldset>
                </td>
                <td style="width: 50%">
                    <fieldset id="out_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                        padding-bottom: 5px; padding-left: 5px; width: 250px; padding-right: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Outside Details
                        </legend>
                        <asp:Label ID="lblOutsideDetails" runat="server" Style="text-align: right" CssClass="EMS_font_small"></asp:Label>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
