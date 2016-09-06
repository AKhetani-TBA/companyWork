<%@ Page Language="C#" CodeFile="PackageDetails.aspx.cs" Inherits="HR_PackageDetails" AutoEventWireup="true"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>

    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .hideselect
        {
            display: none;
        }
        .style4
        {
            height: 26px;
            width: 192px;
        }
        .style8
        {
            height: 26px;
            width: 21px;
        }
        .style11
        {
            height: 26px;
            width: 68px;
        }
        .style13
        {
            height: 26px;
            width: 112px;
        }
        .style23
        {
            width: 102px;
        }
        #editPage
        {
            height: 420px;
        }
        .style24
        {
            width: 270px;
        }
        .style28
        {
            width: 296px;
        }
        .style30
        {
            height: 26px;
            width: 73px;
            text-align: right;
        }
        .style31
        {
            width: 82px;
            height: 26px;
        }
        .style32
        {
            height: 26px;
            width: 296px;
        }
        .style33
        {
            width: 82px;
        }
        .style34
        {
            color: #CC3300;
        }
        .style35
        {
            height: 26px;
            width: 177px;
        }
    </style>
    <script language = "javascript">
     $(document).ready(function() {
            $("#savePackage").click(function() {
                $("#bonusviewdiv").slideDown();
            });

    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font">
        <br />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr>
                        <td nowrap="nowrap" class="style11">
                            Department&nbsp;
                        </td>
                        <td nowrap="nowrap" class="style13">
                            <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged"
                                Width="168px" CssClass="ddl">
                            </asp:DropDownList>
                        </td>
                        <td class="style30" nowrap="nowrap">
                            Employee
                        </td>
                        <td class="style4">
                            <asp:DropDownList ID="showEmployees" runat="server" Width="180px" CssClass="ddl"
                                OnSelectedIndexChanged="showEmployees_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style8">
                            <asp:Button ID="btnSearch" runat="server" CssClass="button" 
                               OnClick="btnSearch_Click" Text="Search"
                                CausesValidation="False" />
                        </td>
                        <td class="style35" style="text-align:right">
                           
                            <asp:Button ID="btnAssignPackage" runat="server" CausesValidation="False" 
                                CssClass="button" OnClick="btnAssignPackage_Click1" Style="float: right" 
                                Text="Assign Package" />
                        </td>
                        <td align="right">
                          
                         
                            <asp:Button ID="earningBtn" runat="server" CssClass="button" 
                                onclick="earningBtn_Click" Text="Salary Break Up" Visible="False" />
                        </td>
                    </tr>
                </table>
                
                
                
                <div id="gridviewdiv" runat="server">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Current 
                            Package
                        </legend>
                        
                         <asp:GridView ID="GVcurrentPackage" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            OnPageIndexChanging="GVcurrentPackage_PageIndexChanging" OnRowCommand="GVcurrentPackage_RowCommand"
                            OnRowDataBound="GVcurrentPackage_RowDataBound" OnSelectedIndexChanged="GVcurrentPackage_SelectedIndexChanged"
                            Width="100%" PageSize="40">
                            <RowStyle BorderColor="#333333" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" />
                            <Columns>
                                <asp:BoundField DataField="empId" HeaderText="empid">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:BoundField DataField="packageId" HeaderText="packageid">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnEmpName" runat="server" CausesValidation="False" CommandArgument="empName"
                                            CommandName="sort" CssClass="gridlink">Employee Name</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("empName") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Employee Name">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnEmpHireDate" runat="server" CausesValidation="False" CommandArgument="empHireDate"
                                            CommandName="sort" CssClass="gridlink">Joining Date</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblHireDate" runat="server" Text='<%# Bind("empHireDate") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnEmpDesig" runat="server" CausesValidation="False" CommandArgument="empDomicile"
                                            CommandName="sort" CssClass="gridlink">Designation</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnDept" runat="server" CausesValidation="False" CommandArgument="deptName"
                                            CommandName="sort" CssClass="gridlink">Department</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Salary">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnSalary" runat="server" CausesValidation="False" CommandArgument="salaryAmount"
                                            CommandName="sort" CssClass="gridlink">Salary</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblsalary" runat="server" Text='<%# Bind("salaryAmount") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="With Effect From">
                                <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnwef" runat="server" CausesValidation="False" CommandArgument="wef"
                                            CommandName="sort" CssClass="gridlink">With Effect From</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblwef" runat="server" Text='<%# Bind("wef") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="packageDeleteImage" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                            Width="16px" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the designation?');" />
                                    </ItemTemplate>
                                    <ItemStyle Width="16px" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Package Details Added...</EmptyDataTemplate>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        </asp:GridView>
                         </fieldset>
                        <br />
                         <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                         <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Preavious 
                             Packages
                        </legend>
                       
                             <asp:GridView ID="displayAll" runat="server" AllowPaging="True" 
                                 AutoGenerateColumns="False" OnPageIndexChanging="displayAll_PageIndexChanging" 
                                 OnRowCommand="displayAll_RowCommand" OnRowDataBound="displayAll_RowDataBound" 
                                 OnSelectedIndexChanged="displayAll_SelectedIndexChanged" Width="100%">
                                 <RowStyle BorderColor="#333333" />
                                 <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" />
                                 <Columns>
                                     <asp:BoundField DataField="empId" HeaderText="empid">
                                         <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="packageId" HeaderText="packageid">
                                         <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                     </asp:BoundField>
                                     <asp:TemplateField HeaderText="Employee Name">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnEmpName" runat="server" CausesValidation="False" 
                                                 CommandArgument="empName" CommandName="sort" CssClass="gridlink">Employee Name</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Employee Name">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnEmpHireDate" runat="server" CausesValidation="False" 
                                                 CommandArgument="empHireDate" CommandName="sort" CssClass="gridlink">Joining Date</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="lblHireDate" runat="server" Text='<%# Bind("empHireDate") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Designation">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnEmpDesig" runat="server" CausesValidation="False" 
                                                 CommandArgument="empDomicile" CommandName="sort" CssClass="gridlink">Designation</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label4" runat="server" Text='<%# Bind("empDomicile") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Department">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnDept" runat="server" CausesValidation="False" 
                                                 CommandArgument="deptName" CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label2" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Salary">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnSalary" runat="server" CausesValidation="False" 
                                                 CommandArgument="salaryAmount" CommandName="sort" CssClass="gridlink">Salary</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="lblsalary" runat="server" Text='<%# Bind("salaryAmount") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="With Effect From">
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="lbtnwef" runat="server" CausesValidation="False" 
                                                 CommandArgument="wef" CommandName="sort" CssClass="gridlink">With Effect From</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="lblwef" runat="server" Text='<%# Bind("wef") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ShowHeader="False">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="packageDeleteImage" runat="server" 
                                                 CausesValidation="False" Height="16px" ImageUrl="~/images/delete.ico" 
                                                 OnClientClick="return confirm('Are you sure you want to delete the designation?');" 
                                                 Width="16px" />
                                         </ItemTemplate>
                                         <ItemStyle Width="16px" />
                                     </asp:TemplateField>
                                 </Columns>
                                 <EmptyDataTemplate>
                                     No Package Details Added...
                                 </EmptyDataTemplate>
                                 <FooterStyle BackColor="White" ForeColor="#333333" />
                                 <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                             </asp:GridView>
                    </fieldset>
                </div>
                <div id="editPage" runat="server">
                    <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; color: #808080;">Assign Package </legend>
                        <div>
                        <div style="height:172px" >
                        <div style="float:left; width:400px">
                       
                            <table>
                                <tr>
                                    <td class="style23">
                                        Employee</td>
                                    <td class="style24">
                                        <asp:Label ID="lblEmp" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        Date of Joining</td>
                                    <td class="style24">
                                        <asp:Label ID="lblDOJ" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        Designation</td>
                                    <td class="style24">
                                        <asp:Label ID="lblDesignation" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        Salary Per Month</td>
                                    <td class="style24">
                                        <asp:TextBox ID="tbSalary" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style23">
                                        With Effect From</td>
                                    <td class="style24">
                                        <asp:TextBox ID="tbWEF" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="tbWEF_CalendarExtender" runat="server" TargetControlID="tbWEF"
                                            Format="dd MMMM yyyy">
                                        </asp:CalendarExtender>
                                        &nbsp;&nbsp;
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                            ControlToValidate="tbWEF" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                            </table>
                            </div>
                            <div id="packagesofempdiv" visible="false" runat="server">
                             <fieldset style="padding-left: 5px; padding-right:5px; padding-bottom:5px; width:310px">
                        <legend style="margin-bottom: 10px; color: #808080;">All Packages </legend>
                        
                             <asp:GridView ID="GridAllPackages" runat="server" AllowPaging="True" 
                                     AutoGenerateColumns="False" PageSize="5" 
                                     onrowdatabound="GridAllPackages_RowDataBound" 
                                     onselectedindexchanged="GridAllPackages_SelectedIndexChanged" 
                                     AutoPostback = "true"  >
                                    
                            <RowStyle BorderColor="#333333" />
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" />
                            <Columns>
                                <asp:BoundField DataField="empId" HeaderText="empid">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                                <asp:BoundField DataField="packageId" HeaderText="packageid">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect" />
                                </asp:BoundField>
                               
                                
                               
                               <asp:TemplateField HeaderText="With Effect From">
                                <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnwef" runat="server" CausesValidation="False" CommandArgument="wef"
                                            CommandName="sort" CssClass="gridlink">With Effect From</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblwef" runat="server" Text='<%# Bind("wef") %>'></asp:Label></ItemTemplate>
                                         <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Salary">
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lbtnSalary" runat="server" CausesValidation="False" CommandArgument="salaryAmount"
                                            CommandName="sort" CssClass="gridlink">Salary</asp:LinkButton></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblsalary" runat="server" Text='<%# Bind("salaryAmount") %>'></asp:Label></ItemTemplate>
                                        <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                
                            </Columns>
                            <EmptyDataTemplate>
                                No Package Details Added...</EmptyDataTemplate>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        </asp:GridView>
                        </fieldset>
                            </div>
                            </div>
                            <br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="savePackage" runat="server" CssClass="button" Text="Save Package"
                                OnClick="savePackage_Click" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click"
                                Text="Back" CausesValidation="False" />
                                <div id="bonusviewdiv" runat="server" style="overflow: hidden;">
                                    <fieldset style="padding: 5px; margin-bottom:5px">
                                        <legend style="color: #808080; margin-bottom:5px">Assign Bonus </legend>
                                           
                                        <div style="float: left; width: 500px; margin-right: 20px">
                                            <asp:GridView ID="bonusgrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                OnPageIndexChanging="bonusgrid_PageIndexChanging" OnRowCommand="bonusgrid_RowCommand"
                                                OnRowDataBound="bonusgrid_RowDataBound" OnSelectedIndexChanged="bonusgrid_SelectedIndexChanged"
                                                Width="100%">
                                                <RowStyle BorderColor="#333333" />
                                                <HeaderStyle CssClass="gridheader" HorizontalAlign="Left" />
                                                <Columns>
                                                    <asp:BoundField DataField="packageId" HeaderText="packageId">
                                                        <ControlStyle CssClass="hideselect" />
                                                        <FooterStyle CssClass="hideselect" />
                                                        <HeaderStyle CssClass="hideselect" />
                                                        <ItemStyle CssClass="hideselect" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="empBonusId" HeaderText="empbonusid">
                                                        <ControlStyle CssClass="hideselect" />
                                                        <FooterStyle CssClass="hideselect" />
                                                        <HeaderStyle CssClass="hideselect" />
                                                        <ItemStyle CssClass="hideselect" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Bonus Name">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lbtnBonusName" runat="server" CausesValidation="False" CommandArgument="slabName"
                                                                CommandName="sort" CssClass="gridlink">Bonus Name</asp:LinkButton></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBonusName" runat="server" Text='<%# Bind("slabName") %>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Criteria">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lbtnCriteria" runat="server" CausesValidation="False" CommandArgument="criteria"
                                                                CommandName="sort" CssClass="gridlink">Criteria</asp:LinkButton></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCriteria" runat="server" Text='<%# Bind("criteria") %>'></asp:Label></ItemTemplate>
                                                        <ItemStyle Width="70px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lbtnBonusAmount" runat="server" CausesValidation="False" CommandArgument="bonusAmount"
                                                                CommandName="sort" CssClass="gridlink">Amount</asp:LinkButton></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBonusAmount" runat="server" Text='<%# Bind("bonusAmount") %>'></asp:Label></ItemTemplate>
                                                        <ItemStyle Width="90px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payable On">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lbtnPayableOn" runat="server" CausesValidation="False" CommandArgument="payableOn"
                                                                CommandName="sort" CssClass="gridlink">Payable On</asp:LinkButton></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPayableDate" runat="server" Text='<%# Bind("payableOn") %>'></asp:Label></ItemTemplate>
                                                        <ItemStyle Width="110px" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="bonusDeleteImage" runat="server" Height="16px" ImageUrl="~/images/delete.ico"
                                                                Width="16px" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete the designation?');" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="16px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No Package Details Added...</EmptyDataTemplate>
                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                            </asp:GridView>
                                        </div>
                                        <div style="width: 388px; float:right">
                                            <table style="margin-top: 20px;">
                                                <tr>
                                                    <td class="style33">
                                                        Bonus Name
                                                    </td>
                                                    <td class="style28">
                                                        <asp:DropDownList ID="showBonus" runat="server" CssClass="ddl" Width="164px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        &nbsp;Criteria
                                                    </td>
                                                    <td class="style28">
                                                        <asp:RadioButtonList ID="rbtnCriteria" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="0">Prorata</asp:ListItem>
                                                            <asp:ListItem Value="1">Fixed</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style31">
                                                        Amount
                                                    <span class="style34">*</span></td>
                                                    <td class="style32">
                                                        <asp:TextBox ID="tbBonusAmount" runat="server" Width="164px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style33">
                                                        Payable On
                                                    <span class="style34">*</span></td>
                                                    <td class="style28">
                                                        <asp:TextBox ID="tbBonusPaidDate" runat="server" Width="164px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbBonusPaidDate"
                                                            Format="dd MMMM yyyy">
                                                        </asp:CalendarExtender>
                                                        &nbsp;&nbsp;
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbBonusPaidDate"
                                                            ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            
                                                <tr>
                                                    <td class="style33">
                                                        &nbsp;
                                                    </td>
                                                    <td class="style28">
                                                        &nbsp;
                                                        <asp:Button ID="btnAddBonus" CssClass="button" runat="server" Text="Save Bonus" OnClick="btnAddBonus_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnCancelBonus" runat="server" CssClass="button" Text="Cancel" 
                                                            OnClick="btnCancelBonus_Click" CausesValidation="False" />
                                                    </td>
                                                </tr>
                                                </table>
                                            </div>
                                    </fieldset>
                                </div>
                          
                           
                        </div>
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
