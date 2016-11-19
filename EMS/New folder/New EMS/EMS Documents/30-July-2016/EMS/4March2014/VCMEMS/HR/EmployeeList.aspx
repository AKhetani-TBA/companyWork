<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EmployeeList.aspx.cs"
    Inherits="HR_EmployeeList" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ajax.js" type="text/javascript"></script>
<%--<script type="text/javascript" >
    function doClear() {
        var name = document.getElementById('txtsrchname').value;
        name.value = "";
    }
</script>--%>
</head>
<body style="margin: 0; padding: 0;" onload="window.parent.document.getElementById('searchbar').style.display = 'none';">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <div>
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
                                Employee : &nbsp;&nbsp;
                            </td>
                            <td style="width: 220px; overflow: hidden">
                                <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" Width="171px"
                                    OnSelectedIndexChanged="showEmployees_SelectedIndexChanged" CssClass="ddl">
                                </asp:DropDownList>
                                &nbsp;OR
                            </td>
                            <td style="width: 180px;" align="left">
                                &nbsp;
                                <asp:TextBox ID="txtsrchname" runat="server" Text="Enter name to search" 
                                ForeColor="GrayText"  Font-Italic="true" Font-Bold="true"  
                                Width="150px" onfocus="if(this.value=='Enter name to search') this.value='';" ></asp:TextBox>
                            </td>
                  <%--          <td style="width: 200px;">
                                &nbsp; &nbsp;
                                <div>
                                </div>
                            </td>--%>
                               <td >
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="ImageButton1_Click" />
                            </td>
                            <td style="width: 190px; text-align: right;">
                                <asp:Button ID="AddEmp" runat="server" OnClick="AddEmp_Click" ForeColor="#333333"
                                    Text="Add Employee" Width="110px" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td nowrap="nowrap" style="width: 60px">
                                Employee
                            </td>
                            <td nowrap="nowrap" style="width: 55px">
                                <asp:DropDownList ID="ddlDurationFlag" runat="server" OnSelectedIndexChanged="ddlDurationFlag_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Till</asp:ListItem>
                                    <asp:ListItem Value="2">In</asp:ListItem>
                                    <asp:ListItem Value="3">After</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td nowrap="nowrap" style="width: 50px">
                                &nbsp; Month
                            </td>
                            <td nowrap="nowrap" style="width: 90px;">
                                <asp:DropDownList ID="ddlMonths" runat="server" CssClass="ddl" OnSelectedIndexChanged="ddlMonths_SelectedIndexChanged"
                                    Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td nowrap="nowrap" style="width: 45px;">
                                &nbsp;&nbsp; Year&nbsp;&nbsp;
                            </td>
                            <td style="width: 80px; overflow: hidden">
                                <asp:DropDownList ID="ddlYears" runat="server" CssClass="ddl" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged"
                                    Width="60px">
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="width: 80px; overflow: hidden;">
                                Having Status
                            </td>
                            <td style="width: 100px;">
                                <asp:DropDownList ID="ddlEmpStatus" runat="server" OnSelectedIndexChanged="ddlEmpStatus_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Hired</asp:ListItem>
                                    <asp:ListItem Value="2">Resigned</asp:ListItem>
                                    <%--<asp:ListItem Value="3">Deleted</asp:ListItem>--%>
                                    <asp:ListItem Value="3">ALL</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                         
                            <td >
                                <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="22px"
                                OnClick="btnexcel_Click" ToolTip="Excel Report" Height="20px" />
                            &nbsp;   &nbsp;   &nbsp;
                            <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" Width="22px"
                                OnClick="btnword_Click" ToolTip="Word Report" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <fieldset style="padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 1050px;width: 100%; overflow-y: auto; overflow-x: auto;" > 
                        <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowDataBound="srchView_RowDataBound"
                            OnSelectedIndexChanged="srchView_SelectedIndexChanged" HorizontalAlign="Justify"
                            Width="100%" OnPageIndexChanging="srchView_PageIndexChanging" OnRowCommand="srchView_RowCommand"
                            OnSorting="srchView_OnSorting" AllowPaging="false" PageSize="40">
                            <RowStyle BorderColor="#333333" BorderWidth="0px" Height="20px" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            <HeaderStyle CssClass="gridheader" />
                            <Columns>
                                <asp:BoundField HeaderText="Code" DataField="empId" HeaderStyle-CssClass="hideselect"
                                    FooterStyle-CssClass="hideselect" ItemStyle-CssClass="hideselect" ControlStyle-CssClass="hideselect">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sr No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerial" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" SortExpression="empName">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="sortNameBtn" runat="server" CausesValidation="False" CommandArgument="empName"
                                            CommandName="sort" CssClass="gridlink">Name</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("empName") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="170px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department" SortExpression="deptName">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox></EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="sortDeptBtn" runat="server" CausesValidation="False" CommandArgument="deptName"
                                            CommandName="sort" CssClass="gridlink">Department</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("deptName") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation" SortExpression="empDomicile">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:TextBox></EditItemTemplate>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument="empDomicile"
                                            CommandName="sort" CssClass="gridlink">Designation</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Joining Date" SortExpression="empHireDate">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkBuon2" runat="server" CausesValidation="False" CommandArgument="empHireDate"
                                            CommandName="sort" CssClass="gridlink">Joining Date</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label55" runat="server" Text='<%# Bind("empHireDate") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="110px" />
                                </asp:TemplateField>                               
                                <asp:TemplateField HeaderText="Resigned Date" SortExpression="resignedDate">
                                     <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument="empDOB"
                                            CommandName="sort" CssClass="gridlink">Resigned Date</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("resignedDate") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="110px" />                                   
                                </asp:TemplateField>  
                               <asp:TemplateField HeaderText="Birth Date" SortExpression="empDOB">                                   
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandArgument="empDOB"
                                            CommandName="sort" CssClass="gridlink">Birth Date</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label51" runat="server" Text='<%# Bind("empDOB") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="110px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Duration" SortExpression="Duration"  HeaderText="Duration (Year)" ItemStyle-CssClass="" >
                               </asp:BoundField >                                      
                                <asp:TemplateField HeaderText="Contact No" SortExpression="empContactNo">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="LinkBu2" runat="server" CausesValidation="False" CommandArgument="empContactNo"
                                            CommandName="sort" CssClass="gridlink">Contact No</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label44" runat="server" Text='<%# Bind("empContactNo") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="125px" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <div align="center">
                                            <asp:ImageButton ID="delEmployeeImg" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                                Width="16px" OnClientClick="return confirm('Are you sure you want to delete the employee?');" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:CommandField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found...</EmptyDataTemplate>
                        </asp:GridView>
                       </div>    
                    </fieldset>
                    <div id="reportdiv" runat="server" style="text-align: center">
                        <fieldset style="margin-top: 5px; padding-top: 3px; padding-bottom: 3px">
                       <%--     <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="22px"
                                OnClick="btnexcel_Click" ToolTip="Excel Report" Height="20px" />--%>
                            &nbsp;
                          <%--  <asp:ImageButton ID="btnword" runat="server" ImageUrl="~/images/wordicon.png" Width="22px"
                                OnClick="btnword_Click" ToolTip="Word Report" />--%>
                        </fieldset>
                    </div>
                </div>
                <br />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
            <asp:PostBackTrigger ControlID="btnword" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
