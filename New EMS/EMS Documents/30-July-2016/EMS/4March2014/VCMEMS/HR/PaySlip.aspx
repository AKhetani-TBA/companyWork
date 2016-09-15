<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaySlip.aspx.cs" Inherits="HR_PaySlip" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            Deparment :
                        </td>
                        <td>
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" Width="168px">
                            </asp:DropDownList>
                           
                        </td>
                        <td>
                            &nbsp; &nbsp; &nbsp; &nbsp;Employee :
                        </td>
                        <td>
                            <asp:DropDownList ID="showEmployees" runat="server" Width="190px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp; &nbsp; &nbsp; &nbsp;Year :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYears" runat="server" Width="60px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;Month :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonths" runat="server" Width="80px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="Show" runat="server" CssClass="button" Text="Show" OnClick="Show_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="payslip" runat="server">
                <div id="payslipforreport" runat="server">
                <fieldset style="width:530px; margin-left:200px; padding-left:30px">
                    <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;"></legend>
                    <table id="payslipupper" runat="server" width="508px" >
                        <tr>
                            <td style="text-align: center">
                                Payslip for the month of
                                <asp:Label ID="lblmonthyear" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align:left">
                                Employee Name :
                                <asp:Label ID="lblempname" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:left">
                                Designation :
                                <asp:Label ID="lblempdesign" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table id="paysliplower" ClientIDMode="Static" runat="server" border="1" width="500px">
                        <tr style="text-align: center">
                            <td style="width:25%">
                                Total Days
                            </td>
                            <td style="width:25%">
                                Present Days
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td>
                                <asp:Label ID="lbltotaldays" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbltotpresentdays" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr style="text-align: center; width:500px">
                            <td colspan="2">
                             <asp:GridView ID="gridearnings" runat="server" AutoGenerateColumns="False" 
                                    OnPageIndexChanging="gridpayslip_PageIndexChanging" 
                                    OnRowDataBound="gridpayslip_RowDataBound" 
                                    Width="500px" >
                                    <RowStyle BorderColor="#333333" />
                                    <HeaderStyle CssClass="gridheader"  HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Earnings">
                                        
                                            <ItemTemplate>
                                                
                                                <asp:Label ID="lblearnings" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="245px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblearningamount" runat="server" 
                                                    Text='<%# Bind("contribution") %>'></asp:Label>
                                                   
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Package Details Added...
                                    </EmptyDataTemplate>
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td>
                                Gross Salary</td>
                            <td style="text-align:right">
                                <asp:Label ID="lblGross" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr style="text-align: center; width:500px">
                            <td colspan="2">
                              <asp:GridView ID="griddeductions" runat="server" AutoGenerateColumns="False" 
                                  
                                     OnRowDataBound="griddeductions_RowDataBound" 
                                   Width="500px">
                                    <RowStyle BorderColor="#333333" />
                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"  />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Deductions">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldeductions" runat="server" Text='<%# Bind("slabName") %>'></asp:Label>
                                            </ItemTemplate>
                                               <ItemStyle Width="245px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldeductionamount" runat="server" Text='<%# Bind("contribution") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Package Details Added...
                                    </EmptyDataTemplate>
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                </asp:GridView>
                               <div style="border-left:solid 1px bisque; border-bottom: solid 1px bisque; border-right:solid 1px bisque; height:18px">
                               <div style="float:left; width:247px; border-right:solid 1px bisque; text-align:left">
                               TDS</div>
                               <div style="float:right; width:100px; text-align:right">
                                   <asp:Label ID="lblTDS" runat="server" Text=""></asp:Label>
                               </div>
                               </div>
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td>
                                Total Deduction</td>
                            <td style="text-align:right">
                                <asp:Label ID="lbltotdeduction" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td>
                                Net Payable</td>
                            <td style="text-align:right">
                                <asp:Label ID="lblNet" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td colspan="2">
                                <asp:Label ID="lblNetInWords" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <%-- <table  border="1"  style="text-align:center" >
                        <tr>
                        <td>
                            &nbsp;</td>
                            <td colspan="2">
                                Opening Balance</td>
                            <td colspan="2">
                                Availed Leave</td>
                            <td colspan="2">
                                Closing Balance</td>
                        </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    PL</td>
                                <td>
                                    CL/SL</td>
                                <td>
                                    PL</td>
                                <td>
                                    CL/SL</td>
                                <td>
                                    PL</td>
                                <td>
                                    CL/SL</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="lblopeningpl" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblopeningclsl" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblavailedpl" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblavailedclsl" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblclosingpl" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblclosingclsl" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                    &nbsp;&nbsp;&nbsp;
                    </fieldset>
                 </div>
                    <br />
                    <fieldset style="width:555px; margin-left:200px; padding:3px ">
                        <div style="text-align:center">
                                
                            <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" 
                                onclick="btnexcel_Click" Width="22px" />
                            &nbsp;
                            <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" 
                                onclick="btnword_Click" Width="22px" />
                                
                            </div>
                    </fieldset>
                </div>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
                 <asp:PostBackTrigger ControlID="btnword" />
             </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
