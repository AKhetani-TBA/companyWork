<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" Inherits="HR_Notifications" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>
    
    <script type="text/javascript">

        function OpenPopup(strURL) {
            window.open(strURL, "List", "scrollbars=yes,resizable=no,width=600,height=450,top=175,left=475");

            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
           <%-- <tr>
                <td style="width: 12%; text-align: left" class="gridheader">
                    Employee
                </td>
                <td style="width: 9%; text-align: center" class="gridheader">
                    Date
                </td>
                <td style="width: 9%; text-align: center" class="gridheader">
                    Original
                </td>
                <td style="width: 9%; text-align: center" class="gridheader">
                    Modified
                </td>
                <td style="width: 12%; text-align: left" class="gridheader">
                    Modified by
                </td>
                <td style="text-align: left; width: 20%;" class="gridheader">
                    Comments
                </td>
                <td style="width: 10%; text-align: center;" class="gridheader">
                    Approve / Reject
                </td>
                <td style="width: 10%; text-align: center;" class="gridheader">
                    Log Detail
                </td>
            </tr>--%>
            <tr>
                <td colspan="8">
                    <div style="height: 650px; width: 100%; overflow-y: auto; overflow-x: auto;">
                        <asp:GridView ID="gvnotification" runat="server" AutoGenerateColumns="False" HorizontalAlign="Justify"
                            Width="100%" AllowSorting="true" OnRowCommand="gvnotification_RowCommand" OnRowDataBound="gvnotification_RowDataBound"
                            OnSorting="gvnotification_Sorting" ShowHeader="true">
                            <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                ForeColor="White" Height="19px" HorizontalAlign="Left" />
                            <RowStyle Wrap="true" Font-Names="Verdana" Font-Size="10px" />
                            <Columns>
                                <asp:BoundField DataField="empName" HeaderText="Employee Name" SortExpression="empName"
                                    ItemStyle-Width="12%" />
                                <asp:BoundField DataField="dateOfRecord" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Date" 
                                SortExpression="dateOfRecord" ItemStyle-Width="9%"  />
                                <asp:BoundField DataField="workDayCategory" HeaderText="Origional" SortExpression="workDayCategory"
                                    ItemStyle-Width="9%" />
                                <asp:BoundField DataField="newCategory" HeaderText="Modified" SortExpression="newCategory"
                                    ItemStyle-Width="9%" />
                                <asp:BoundField DataField="modifyBy" HeaderText="Modified By" SortExpression="modifyBy"
                                    ItemStyle-Width="12%" />
                                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="comments"
                                    ItemStyle-Width="20%" />
                                <asp:TemplateField HeaderText="Approve / Reject">
                                    <ItemStyle Width="10%" />
                                    <ItemTemplate>
                                        <div id="mydiv" style="text-align: center;">
                                            <asp:ImageButton ID="confirmBtn1" CommandName="confirmBtn" Style="border: 1px solid white;
                                                margin-top: 2px" onmouseover="this.style.border = '1px solid gray';" onmouseout="this.style.border = '1px solid white';"
                                                Text="Confirm" runat="server" ImageUrl="../images/confirm.png" CommandArgument='<%# Eval("empId") %>' />
                                            <asp:ImageButton ID="cancelBtn1" CommandName="cancelBtn" Style="border: 1px solid white;
                                                margin-top: 2px" onmouseover="this.style.border = '1px solid gray';" onmouseout="this.style.border = '1px solid white';"
                                                Text="Cancel" runat="server" ImageUrl="../images/delete1.png" CommandArgument='<%# Eval("empId") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Log Detail" >
                                    <ItemStyle Width="10%"  />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="logsImage" runat="server" CommandName="popup" CommandArgument='<%# Eval("dateOfRecord") %>'
                                            ImageUrl="~/images/doc.png" Width="30px" Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <%--ControlStyle-CssClass="hideselect"--%>
                                    <ItemStyle Width="10%" />
                                    <ItemTemplate>
                                        <asp:Label ID="empId" runat="server" Text='<%# Eval("empId") %>'></asp:Label>
                                        <%--CssClass="hideselect"--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Records Found..
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
