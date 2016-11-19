<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveManagement.aspx.cs" Inherits="HR_LeaveManagement" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <script type= "text/javascript" ="javascript" >
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
    <style type="text/css">
         .style1
        {
            width: 728px;
            height: 100px;
        }
        .style2
        {
            width: 92px;
        }
        .style3
        {
            color: #FF0066;
        }
        .hi
    .td_style
    {
    width:50%;
    border-left-style: solid ;
     border-left-width: 1px;
      border-left-color: #E8E8E8 ;
    
    }
    .headerSeperator
    {
    border-left-style:inset ;
    border-left-width: thin ;
    border-left-color: #FFFFEC;
    }
        
  
        .style4
        {
            height: 26px;
            width: 168px;
        }
        .style17
        {
            height: 26px;
            width: 104px;
        }
        .style19
        {
            color: #FF0000;
        }
        .style20
        {
            width: 92px;
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
        .newStyle1
        {
            border-right-style: solid;
            border-right-width: thin;
            border-right-color: #C0C0C0;
        }
        .style29
        {
            width: 72px;
        }
        </style>
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
                     <table>
                         <tr>
                             <td >
                                 Department&nbsp; :</td>
                             <td class="style17" nowrap="nowrap">
                                 <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="100px">
                                    
                                 </asp:DropDownList>
                             </td>
                             <td class="style29" >
                                 &nbsp;&nbsp;&nbsp;Employee :</td>
                             <td class="style4">
                                 <asp:DropDownList ID="showEmployees" runat="server" CssClass="ddl"
                                     Width="160px" onselectedindexchanged="showEmployees_SelectedIndexChanged">
                                    
                                 </asp:DropDownList>
                             </td>
                             <td >
                                 &nbsp;Leave Type :</td>
                             <td >
                                 <asp:DropDownList ID="showLeaveTypes" runat="server" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="110px">
                                 </asp:DropDownList>
                             </td>
                             <td >
                                 Year:</td>
                             <td >
                                 <asp:DropDownList ID="showYear" runat="server" CssClass="ddl"
                                     OnSelectedIndexChanged="showDepartments_SelectedIndexChanged" 
                                     Width="100px">
                                 </asp:DropDownList>
                             </td>
                             <td >
                                
                                 <asp:ImageButton ID="btnSearch" runat="server" 
                                     ImageUrl="~/images/searchbtn.png" onclick="btnSearch_Click" />
                             </td>
                             <td style ="width:130px; text-align: right;">
                                 <asp:Button ID="AddLeaveDetail" runat="server" CssClass="button" 
                                     Font-Bold="False" ForeColor="#333333" onclick="AddLeaveDetail_Click" 
                                     Style="float: right" Text="Assign Balance" Width="110px" />
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
                                 Width="100%" PageSize="15">
                                 <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                 <HeaderStyle  CssClass="gridheader" />
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
                                    <%-- <asp:TemplateField HeaderText="Department">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("empId") %>'></asp:TextBox>
                                         </EditItemTemplate>
                                         <HeaderTemplate>
                                             <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                                                 CommandArgument="deptName" CommandName="sort" CssClass="gridlink">Department</asp:LinkButton>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label1" runat="server" Text='<%# Bind("deptName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>--%>
                                     <asp:TemplateField HeaderText="LeaveTypeId">
                                         <EditItemTemplate>
                                             <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("empName") %>'></asp:TextBox>
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
                                     <asp:TemplateField HeaderText="Jan">
                                         <HeaderTemplate>
                                             Jan
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                             <asp:Label ID="Label7" runat="server" Text='<%# Bind("January") %>'></asp:Label>
                                             
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Feb">
                                             <HeaderTemplate>
                                                 Feb
                                         </HeaderTemplate>
                                             <ItemTemplate>
                                                 <asp:Label ID="Label8" runat="server" Text='<%# Bind("February") %>'></asp:Label>
                                           
                                             </ItemTemplate></asp:TemplateField>
                                             <asp:TemplateField HeaderText="Mar">
                                            
                                             <HeaderTemplate >
                                                 Mar
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                                 <asp:Label ID="Label5" runat="server" Text='<%# Bind("March") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Apr">
                                          <HeaderTemplate >
                                              Apr
                                             </HeaderTemplate>
                                            
                                             <ItemTemplate>
                                              
                                                 <asp:Label ID="Label9" runat="server" Text='<%# Bind("April") %>'></asp:Label>
                                             
                                             
                                             
                                             </ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="May">
                                             <ItemTemplate>
                                                 <asp:Label ID="Label10" runat="server" Text='<%# Bind("May") %>'></asp:Label>
                                              </ItemTemplate>
                                             <HeaderTemplate >
                                                 May
                                             </HeaderTemplate>
                                           
                                             
                                             </asp:TemplateField><asp:TemplateField HeaderText="Jun">
                                             
                                             <HeaderTemplate >
                                                 Jun
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                                 <asp:Label ID="Label11" runat="server" Text='<%# Bind("June") %>'></asp:Label>
                                         </ItemTemplate>
                                            
                                            
                                             </asp:TemplateField><asp:TemplateField HeaderText="Jul">
                                                     <HeaderTemplate >
                                                         Jul
                                             </HeaderTemplate>
                                                 <ItemTemplate>
                                                     <asp:Label ID="Label12" runat="server" Text='<%# Bind("July") %>'></asp:Label>
                                                   
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Aug">
                                         <ItemTemplate>
                                             <asp:Label ID="Label13" runat="server" Text='<%# Bind("August") %>'></asp:Label>
                                            
                                          </ItemTemplate>
                                          <HeaderTemplate >
                                              Aug
                                             </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Sep">
                                         <ItemTemplate>
                                             <asp:Label ID="Label14" runat="server" Text='<%# Bind("September") %>'></asp:Label>
                                             
                                             </ItemTemplate>
                                             <HeaderTemplate >
                                                 Sep
                                             </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Oct">
                                         <ItemTemplate>
                                             <asp:Label ID="Label15" runat="server" Text='<%# Bind("October") %>'></asp:Label>
                                             
                                             </ItemTemplate>
                                             <HeaderTemplate >
                                                 Oct
                                             </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Nov">
                                         <ItemTemplate>
                                             <asp:Label ID="Label16" runat="server" Text='<%# Bind("November") %>'></asp:Label>
                                            
                                             </ItemTemplate>
                                             <HeaderTemplate >
                                                 Nov
                                             </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Dec">
                                         <ItemTemplate>
                                             <asp:Label ID="Label17" runat="server" Text='<%# Bind("December") %>'></asp:Label>
                                            
                                             </ItemTemplate>
                                             <HeaderTemplate >
                                                 Dec
                                             </HeaderTemplate>
                                     </asp:TemplateField>
                                     <asp:CommandField ShowSelectButton="True">
                                         <ControlStyle CssClass="hideselect" />
                                         <FooterStyle CssClass="hideselect" />
                                         <HeaderStyle CssClass="hideselect" />
                                         <ItemStyle CssClass="hideselect" />
                                     </asp:CommandField>
                                     <asp:TemplateField ShowHeader="False">
                                         <ItemTemplate>
                                             <asp:ImageButton ID="delLeaveBalance" runat="server" ImageUrl="~/images/delete.ico"
                                                 OnClientClick="return confirm('Are you sure you want to delete?');" Width="16px" />
                                         </ItemTemplate>
                                         <ItemStyle Width="20px" />
                                     </asp:TemplateField>
                                 </Columns>
                                 <EmptyDataTemplate>
                                     No Leave Record Found...</EmptyDataTemplate></asp:GridView></fieldset>
                     </div>
                 </div>
                 <div id="insertTypeDetails" runat="server" visible="false" >
                 <fieldset style="padding: 5px;">
                         <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Update
                             Leave Type </legend>
                         <table class="style1">
                         <tr>
                                 <td class="style2">
                                     Employee</td><td>
                                     <asp:Label ID="empName" runat="server" Text="Label"></asp:Label></td></tr><tr>
                                 <td class="style2">
                                     Leave Type <span class="style3">*</span>
                                 </td>
                                 <td>
                                     <asp:RadioButton ID="typePl" runat="server" Text="PL" Checked="True" 
                                         GroupName="leaveTypesBtn" oncheckedchanged="typePl_CheckedChanged" AutoPostBack="true"/>
                                     <asp:RadioButton style="padding-left:5px" ID="typeCl" runat="server" Text="CL/SL" 
                                         GroupName="leaveTypesBtn" oncheckedchanged="typeCl_CheckedChanged" AutoPostBack="true"/>
                                     
                                     <asp:ImageButton ID="editLeavesBtn" style="padding-left:10px" runat="server" Height="20px" 
                                         ImageUrl="~/images/pen(2).ico" ToolTip="Edit Leaves" Width="20px" 
                                         onclick="editLeavesBtn_Click" />
                                     <asp:DropDownList ID="leaveTypes" runat="server" Width="120px" Visible="False" 
                                         CssClass="ddl" onselectedindexchanged="leaveTypes_SelectedIndexChanged" 
                                         AutoPostBack="True">
                                     </asp:DropDownList>
                                     
                                     
                                     &nbsp;
                                     
                                 </td>
                             </tr>
                             
                            
                             <tr>
                                 <td class="style2">
                                     For the year
                                 <span class="style19">*</span></td><td>
                                     <asp:DropDownList ID="forTheYear" runat="server" AutoPostBack="True" 
                                         CssClass="ddl"  Width="120px" 
                                         onselectedindexchanged="forTheYear_SelectedIndexChanged">
                                     </asp:DropDownList>
                                 </td>
                             </tr>
                             <tr>
                                 <td class="style2">
                                     January <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="janBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     February <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="febBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     March <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="marBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style20">
                                     April <span class="style19">*</span></td><td class="style21">
                                     <asp:TextBox ID="aprBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     May <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="mayBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     June <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="juneBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     July <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="julyBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     August <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="augBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     September <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="septBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     October <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="octBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     November <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="novBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     December <span class="style19">*</span></td><td>
                                     <asp:TextBox ID="decBalance" runat="server" Width="116px"></asp:TextBox></td></tr><tr>
                                 <td class="style2">
                                     &nbsp;</td><td>
                                     <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                         OnClick="saveBtn_Click" Text="Save" />
                                     &nbsp;&nbsp;<asp:Button ID="resetBtn" runat="server" CausesValidation="False" 
                                         CssClass="button" OnClick="resetBtn_Click" Text="Back" 
                                         onclientclick="i=6;hidefeed();" />
                                         &nbsp;<div id="feed" 
                                         style="display:none ;border: thin inset #ACA899; padding: 10px; background-color :#FFFFCC" 
                                         class="style22">
                                             Record Added Succesfully...</div></td></tr></table><asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                             ShowMessageBox="True" ShowSummary="False" />
                     </fieldset>
                  &nbsp;</div>
         </div>
         <br />
         </div>
    </ContentTemplate>
     </asp:UpdatePanel>
    </div>
            
    </form>
    </body>
</html>
