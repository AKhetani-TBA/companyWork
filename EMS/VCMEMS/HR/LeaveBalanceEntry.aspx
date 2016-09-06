<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveBalanceEntry.aspx.cs" Inherits="HR_LeaveBalanceEntry" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language ="javascript" >
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
               
                setTimeout("hidefeed();", "1000");
            }
        
        }
    
    
    </script>
    <style type="text/css">
        .style1
        {
            width: 728px;
            height: 100px;
        }
        .style2
        {
            width: 115px;
        }
        .style3
        {
            color: #FF0066;
        }

        .style11
        {
            height: 26px;
            width: 68px;
        }
   
        .style12
        {
            height: 26px;
            width: 45px;
        }
        .style4
        {
            height: 26px;
            width: 164px;
        }
        .style9
        {
            height: 26px;
            width: 139px;
        }
        .style6
        {
            height: 26px;
            width: 28px;
        }
        .style17
        {
            height: 26px;
            width: 104px;
        }
        .style18
        {
            height: 26px;
            width: 105px;
        }
        .style19
        {
            color: #FF0000;
        }
        .style20
        {
            width: 115px;
            height: 24px;
        }
        .style21
        {
            height: 24px;
        }
        .style22
        {
            color: #993399;
        }
        </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0; padding: 0;">
    <form id="form2" runat="server">
    <div class="EMS_font" id="innerDiv">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
         <div>
           <br />
                 
                 <div id="search_grid" runat="server">
                     <table style="width: 100%">
                         <tr>
                             <td class="style11" nowrap="nowrap">
                                 Department&nbsp;
                             </td>
                             <td class="style17" nowrap="nowrap">
                                 <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="100px">
                                    
                                 </asp:DropDownList>
                             </td>
                             <td class="style12" nowrap="nowrap">
                                 Employee
                             </td>
                             <td class="style4">
                                 <asp:DropDownList ID="showEmployees" runat="server" CssClass="ddl"
                                     Width="160px" onselectedindexchanged="showEmployees_SelectedIndexChanged">
                                    
                                 </asp:DropDownList>
                             </td>
                             <td class="style18">
                                 &nbsp;Leave Type</td>
                             <td class="style9">
                                 <asp:DropDownList ID="showLeaveTypes" runat="server" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="110px">
                                 </asp:DropDownList>
                             </td>
                             <td class="style6">
                                 Year
                             </td>
                             <td class="style17">
                                 <asp:DropDownList ID="showYear" runat="server" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="100px">
                                 </asp:DropDownList>
                             </td>
                             <td style="height: 26px; width: 224px;">
                                 <asp:Button ID="AddLeaveDetail" runat="server" CssClass="button" 
                                     Font-Bold="False" ForeColor="#333333" Style="float: right" 
                                     Text="Assign Balance" Width="110px" onclick="AddLeaveDetail_Click" />
                                 <asp:ImageButton ID="btnSearch0" runat="server" 
                                     ImageUrl="~/images/searchbtn.png" onclick="btnSearch_Click" />
                             </td>
                         </tr>
                     </table>
                     <div id="CardDetail">
                         <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                             <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                             </legend>
                             <asp:GridView ID="LeaveBalance" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                 HorizontalAlign="Justify" 
                                 OnPageIndexChanging="LeaveBalance_PageIndexChanging" OnRowCommand="LeaveBalance_RowCommand"
                                 OnRowDataBound="LeaveBalance_RowDataBound" OnSelectedIndexChanged="LeaveBalance_SelectedIndexChanged"
                                 Width="100%" PageSize="20">
                                 <RowStyle BorderColor="#333333" />
                                 <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                 <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                     ForeColor="White" Height="19px" />
                                 <Columns>
                                     <asp:BoundField DataField="LeaveTypeDetailsId" HeaderText="LeaveTypeDetailsId" 
                                         ItemStyle-CssClass="hideselect" ControlStyle-CssClass="hideselect" 
                                         HeaderStyle-CssClass="hideselect" FooterStyle-CssClass="hideselect" >
                                         <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                     </asp:BoundField>
                                     <asp:BoundField DataField="empId" HeaderText="emp">
                                      <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                         </asp:BoundField>
                                    
                                     <asp:TemplateField HeaderText="Employee">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" 
                                                 CommandArgument="empName" CommandName="sort" CssClass="gridlink">Employee</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label2" runat="server" Text='<%# Bind("empName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Department">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                 CommandArgument="deptName" CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label1" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Type">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" 
                                                 CommandArgument="LeaveTypeName" CommandName="sort" CssClass="gridlink">Type</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label3" runat="server" Text='<%# Bind("LeaveTypeName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Year">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("forTheYear") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" 
                                                 CommandArgument="forTheYear" CommandName="sort" CssClass="gridlink">Year</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label4" runat="server" Text='<%# Bind("forTheYear") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:BoundField DataField="January" HeaderText="Jan"/>
                                     <asp:BoundField DataField="February" HeaderText="Feb"/>
                                     <asp:BoundField DataField="March" HeaderText="Mar"/>
                                     <asp:BoundField DataField="April" HeaderText="Apr"/>
                                     <asp:BoundField DataField="May" HeaderText="May"/>
                                     <asp:BoundField DataField="June" HeaderText="Jun"/>
                                     <asp:BoundField DataField="July" HeaderText="Jul"/>
                                     <asp:BoundField DataField="August" HeaderText="Aug"/>
                                     <asp:BoundField DataField="September" HeaderText="Sep"/>
                                     <asp:BoundField DataField="October" HeaderText="Oct"/>
                                     <asp:BoundField DataField="November" HeaderText="Nov"/>
                                     <asp:BoundField DataField="December" HeaderText="Dec"/>
                                     <asp:CommandField ShowSelectButton="True">
                                         <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                     </asp:CommandField>
                                     <asp:TemplateField ShowHeader="False">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="delLeaveBalance" runat="server" ImageUrl="~/images/delete.ico" OnClientClick="return confirm('Are you sure you want to delete?');"
                                                 Width="16px" />
                                         </ItemTemplate>
                                         <ItemStyle Width="20px" />
                                     </asp:TemplateField>
                                 </Columns>
                                 <EmptyDataTemplate>
                                     No Leave Record Found...
                                 </EmptyDataTemplate>
                             </asp:GridView>
                         </fieldset>
                     </div>
                 </div>
                 <div id="insertTypeDetails" runat="server" visible="false" >
                 <fieldset style="padding: 5px;">
                         <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update
                             Leave Type </legend>
                         <table class="style1">
                         <tr>
                                 <td class="style2">
                                     Employee</td>
                                 <td>
                                     <asp:Label ID="empName" runat="server" Text="Label"></asp:Label>
                                     
                                 </td>
                             </tr>
                             
                            
                             <tr>
                                 <td class="style2">
                                     Leave Type <span class="style3">*</span>
                                 </td>
                                 <td>
                                     <asp:RadioButton ID="typePl" runat="server" Text="PL" Checked="True" 
                                         GroupName="leaveTypesBtn"  oncheckedchanged="typePl_CheckedChanged" AutoPostBack="true" />
                                     <asp:RadioButton style="padding-left:5px" ID="typeCl" runat="server" Text="CL/SL" AutoPostBack="true" 
                                         GroupName="leaveTypesBtn" oncheckedchanged="typeCl_CheckedChanged" />
                                     
                                     <asp:ImageButton ID="editLeavesBtn" style="padding-left:10px" runat="server" Height="20px" 
                                         ImageUrl="~/images/pen(2).ico" ToolTip="Edit Leaves" Width="20px" 
                                         onclick="editLeavesBtn_Click" />
                                     <asp:DropDownList 
                                         ID="leaveTypes" runat="server"  Width="120px" 
                                         AutoPostBack="True" 
                                         onselectedindexchanged="leaveTypes_SelectedIndexChanged">
                                     </asp:DropDownList>
                                     
                                     
                                     &nbsp;
                                     
                                 </td>
                             </tr>
                             
                            
                             <tr>
                                 <td class="style2">
                                     For the year
                                 <span class="style19">*</span></td>
                                 <td>
                                     <asp:DropDownList ID="forTheYear" runat="server" AutoPostBack="True" 
                                         CssClass="ddl"  
                                         Width="120px" onselectedindexchanged="forTheYear_SelectedIndexChanged">
                                     </asp:DropDownList>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     January <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="janBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                            <tr>
                                 <td class="style2">
                                     February <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="febBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     March <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="marBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style20">
                                     April <span class="style19">*</span></td>
                                 <td class="style21">
                                     <asp:TextBox ID="aprBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     May <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="mayBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     June <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="juneBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     July <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="julyBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     August <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="augBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     September <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="septBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     October <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="octBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     November <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="novBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     December <span class="style19">*</span></td>
                                 <td>
                                     <asp:TextBox ID="decBalance" runat="server" Width="116px"></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     &nbsp;</td>
                                 <td>
                                     <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                         OnClick="saveBtn_Click" Text="Save" />
                                     <asp:Button ID="resetBtn" runat="server" CausesValidation="False" 
                                         CssClass="button" OnClick="resetBtn_Click" Text="Back" 
                                         onclientclick="i=6;hidefeed();" />
                                         <div id="feed" 
                                         style="display:none ;border: thin inset #ACA899; padding: 10px; background-color :#FFFFCC" 
                                         class="style22">
                                             Record Added Succesfully...</div>
                                 </td>
                             </tr>
                         </table>
                         <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                             ShowMessageBox="True" ShowSummary="False" />
                     </fieldset>
                  </div> 
             
         </div>
         <br />
         </div>
    </ContentTemplate>
     </asp:UpdatePanel>
    </div>
            
    </form>
    </body>
</html>
