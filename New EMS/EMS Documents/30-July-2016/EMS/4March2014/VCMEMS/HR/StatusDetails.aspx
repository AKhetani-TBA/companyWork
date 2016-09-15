<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatusDetails.aspx.cs" Inherits="HR_StatusDetails"
    EnableViewState="true" EnableEventValidation="false" Async="true" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />

    <script src="../js/ajax.js" type="text/javascript"></script>

    <script src="../js/jquery.js" type="text/javascript"></script>
  <script language="javascript" type="text/javascript">
        function showlogdiv() {
            var dv = document.getElementById("in_out_logs");
            dv.style.pixelLeft = event.x - 380;
            dv.style.pixelTop = event.y + 15;
        }
        function OpenPopup(strURL) {
            window.open(strURL, "List", "scrollbars=yes,resizable=no,width=600,height=450,top=175,left=475");
            return false;
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="EMS_font">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnexcel" />
            </Triggers>
            <ContentTemplate>
                <div id="search_grid" runat="server">
               
                        <table width="100%"> 
                            <tr>
                                                  <td width="25%">
                      <fieldset >
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Planned/Unplanned Leave
                        </legend>
                                    <asp:RadioButtonList ID="radPlanned" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="rdlApp_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Text="ALL" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Unplanned" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                           
                            </fieldset>
                                </td>

                                <td width="60%">
                                         <fieldset >
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Department and Employee wise Search
                        </legend>
                                    Department 
                                    <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True"
                                        Width="175px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    Employee 
                                    <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="false" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                        Width="180px">
                                    </asp:DropDownList>
                                      &nbsp;&nbsp;    <asp:ImageButton ID="btnSearch" runat="server" 
                                          ImageUrl="~/images/searchbtn.png" OnClick="btnSearch_Click"   />

                                        </fieldset>
                                </td>
                            <td width="3%">
                                <asp:Button ID="team" runat="server" CommandArgument="1" Text="TLs/Mgr"  onclick="team_Click"/> 
                      </td>
                         <td width="5%">       
                             <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" 
                                        OnClick="btnexcel_Click" Width="25px" />
                      </td>
                         
                        </tr>
                        </table>
                    
                              <%--  <td style="width: 10%;">
                                    From Date :
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="fdate" runat="server" Font-Bold="True" Width="125px"></asp:TextBox>
                                    <asp:CalendarExtender ID="fdatecx" runat="server" TargetControlID="fdate" Format="dd MMMM yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td style="width: 8%;">
                                    To Date :
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="tDate" runat="server" Font-Bold="True" Width="125px"></asp:TextBox>
                                    <asp:CalendarExtender ID="tDatece" runat="server" TargetControlID="tDate" Format="dd MMMM yyyy">
                                    </asp:CalendarExtender>
                                </td>--%>

                          <%--    <tr>
                              <td colspan="2">
                                    <asp:RadioButtonList ID="rdlApp" runat="server" RepeatDirection="Horizontal" 
                                      >
                                        <asp:ListItem Text="ALL" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>--%>
                            <%--    <td style="text-align: center" colspan="4">
                                    <asp:Button ID="btnShowDetails" runat="server" Text="Search" CssClass="button" OnClick="btnShowDetails_Click" />
                                </td>--%>
                              <%--  <td style="text-align: right" colspan="2">
                                    <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="22px"
                                        OnClick="btnexcel_Click" />
                                </td>
                            </tr>--%>
                 <table width="100%">
                            <tr>   
                              <td width="50%"> 
                        <fieldset  >
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search  Range
                        </legend> 
                                    From Date:
                                    <asp:TextBox ID="txtFormDate" runat="server" AutoPostBack="true" 
                                        Font-Bold="True" ontextchanged="txtFormDate_TextChanged" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendaceDate" runat="server" Format="dd-MMM-yyyy" 
                                        TargetControlID="txtFormDate">
                                    </asp:CalendarExtender>
                                    &nbsp;&nbsp; To Date :
                                    <asp:TextBox ID="txtTodate" runat="server" Font-Bold="True" ></asp:TextBox>
                                    <asp:CalendarExtender ID="attendancetodate" runat="server" Format="dd-MMM-yyyy" 
                                       TargetControlID="txtTodate">
                                    </asp:CalendarExtender>                                    
                                    <br />
                                    <asp:LinkButton ID="CY" runat="server" onclick="CY_Click"> Current Year </asp:LinkButton> 
                                      &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;  &nbsp;&nbsp;     &nbsp;&nbsp;    &nbsp;&nbsp; 
                                     &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp;    &nbsp;&nbsp; 
                                    <asp:LinkButton ID="CM" runat="server" onclick="CM_Click"> Current Month </asp:LinkButton>
                             </fieldset>
                    </td>  
                           <td width="30%">
                        <fieldset >
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Leave Status
                        </legend>
                                    <asp:RadioButtonList ID="rdlApp" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="rdlApp_SelectedIndexChanged" 
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Text="ALL" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                           <br />
                            </fieldset>
                         </td>
                     

                     <%-- <td width="10%">
                      </td>--%>
                            </tr>
                   </table>
                </div>
                <div>
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Search Result
                        </legend>
                        <div style="height: 700px; width: 100%; overflow-y: auto; overflow-x: auto;">
<%--                            <asp:GridView ID="srchView" runat="server" AutoGenerateColumns="False" OnRowDataBound="srchView_RowDataBound"
                                AllowSorting="true" OnSorting="srchView_Sorting" Width="100%" OnRowCommand="srchView_RowCommand"
                                AllowPaging="false">
                                <HeaderStyle BackColor="#959595" CssClass="gridheader" Font-Names="Tw Cen MT Condensed"
                                    Font-Size="17px" ForeColor="White" Height="19px" />
                                <RowStyle Wrap="true" />
                                <Columns>
                                    <asp:BoundField DataField="EmpName" HeaderText="Employee" SortExpression="EmpName"
                                        ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="DeptName" HeaderText="Department" SortExpression="DeptName"
                                        ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="SelectDate" HeaderText="Date" SortExpression="SelectDate"
                                        ItemStyle-Width="5%" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ApprovedStatus" HeaderText="Approved Status" SortExpression="ApprovedStatus"
                                        ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="ApprovedEmpName" HeaderText="Approved By" SortExpression="ApprovedEmpName"
                                        ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" SortExpression="ApprovedDate"
                                        ItemStyle-Width="5%" DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No Records Found..
                                </EmptyDataTemplate>
                            </asp:GridView>
--%>      
                        <asp:GridView ID="displayAll" runat="server" AllowPaging="false" AutoGenerateColumns="False" 
                            OnPageIndexChanging="displayAll_PageIndexChanging" OnRowCommand="displayAll_RowCommand"
                            OnRowDataBound="displayAll_RowDataBound" OnSelectedIndexChanged="displayAll_SelectedIndexChanged" OnSorting="displayAll_OnSorting"
                           Width="100%" Font-Bold="False" AllowSorting="True"  Font-Size="12px" Font-Names="Microsoft Sans Serif">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                                <HeaderStyle BackColor="#959595" Font-Names="Tw Cen MT Condensed" Font-Size="17px"
                                    ForeColor="White" Height="19px" HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeaveId" runat="server" Text='<%# Bind("LeaveId") %>' ></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect"  />
                                </asp:TemplateField>
                                 <asp:BoundField HeaderText="AppliedDate" DataField="AppliedDate"  DataFormatString="{0:dd-MMM-yyyy}"
                                SortExpression="AppliedDate"  ItemStyle-Width="10%">
                                </asp:BoundField>

                                        <asp:BoundField HeaderText="From" DataField="FromDate"  DataFormatString="{0:dd-MMM-yyyy}"
                                SortExpression="FromDate"  ItemStyle-Width="10%">
                                </asp:BoundField>
                                <asp:BoundField HeaderText="To" DataField="ToDate" DataFormatString="{0:dd-MMM-yyyy}"
                                  SortExpression="ToDate"  ItemStyle-Width="10%"> 
                                </asp:BoundField>
                                 <asp:BoundField HeaderText="Emp" DataField="empName" SortExpression="empName"  ItemStyle-Width="15%">
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Dep" DataField="deptName" SortExpression="deptName" ItemStyle-Width="8%" >
                              </asp:BoundField>
<%--                                  <asp:BoundField HeaderText="Product" DataField="Projectname" SortExpression="Projectname" ItemStyle-Wrap="true" ItemStyle-Width="15%">
                              </asp:BoundField>
--%>                             <asp:BoundField DataField="DayType" HeaderText="Type" SortExpression="DayType"  ItemStyle-Width="3%" >
                                </asp:BoundField>
<%--                                <asp:BoundField DataField="IsProbable" HeaderText="Probable" SortExpression="IsProbable"  ItemStyle-Width="3%"  >
                                </asp:BoundField>
--%>                  
                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveCategory" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="true" Width="5%" />
                                    </asp:TemplateField>                 
                                 <asp:BoundField DataField="LeaveReason" HeaderText="Reason" SortExpression="LeaveReason" >
                                </asp:BoundField>
                  <%--         <asp:BoundField HeaderText="ApplyDt" DataField="AppliedDate" DataFormatString="{0:dd-MMM-yyyy}"
                                    SortExpression="AppliedDate"  ItemStyle-Width="8%">
                                </asp:BoundField>
--%>                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:ImageButton ID="deltebtn" runat="server" CausesValidation="False" 
                                            ImageUrl="~/images/Deletings.PNG" CommandArgument="delete" 
                                            OnClientClick="javascript:if(!confirm('Are you sure ,you want to Delete Record?')){return false;}"  />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle  />
                                </asp:TemplateField>
                           <%--     <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:ImageButton ID="statusImageButton" runat="server" CausesValidation="False" Height="20px"  Width="20px" 
                                            ImageUrl="~/images/Extrainfo.jpg" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClick="statusImageButton_Click " onmouseover="setstatusdivExtra();" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle />
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnApproved" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                onmouseover="appsetstatusdiv();" Style="height: 20px" Text="" Width="40px"
                                                OnClick="btnApproved_Click" />
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                <asp:BoundField DataField="TLComments" HeaderText="Comments" SortExpression="TLComments"    >
                                </asp:BoundField>
                                
                                <asp:CommandField ShowSelectButton="True">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect" />
                                    <HeaderStyle CssClass="hideselect" />
                                    <ItemStyle CssClass="hideselect"  />
                                </asp:CommandField>
                                <asp:BoundField DataField="EmpId" >
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect"  />
                                    <HeaderStyle CssClass="hideselect"   />
                                    <ItemStyle CssClass="hideselect"  />
                                </asp:BoundField>
                                 <asp:BoundField DataField="DepartmentId">
                                    <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect"  />
                                    <HeaderStyle CssClass="hideselect"   />
                                    <ItemStyle CssClass="hideselect"   />
                                </asp:BoundField >
                                       <asp:BoundField DataField="TLApproval" HeaderText=" Approval Status"     >
                                      <ControlStyle CssClass="hideselect" />
                                    <FooterStyle CssClass="hideselect"  />
                                    <HeaderStyle CssClass="hideselect"   />
                                    <ItemStyle CssClass="hideselect"   />
                                </asp:BoundField>
                                                             <asp:TemplateField HeaderText="Logs">
                                        <ItemStyle Width="5%" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="logsImage" runat="server" ImageUrl="~/images/doc.png" Width="30px"
                                                Height="20px" />
                                            <%--OnClick="logsImage_Click" --%>
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                            </Columns>
                            <PagerStyle />
                            <EmptyDataTemplate>
                                Sorry... No Data Found...!
                            </EmptyDataTemplate>
                             <FooterStyle BackColor="White" ForeColor="#333333" />
                             <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        </asp:GridView>
             
     </div>
       <div id="in_out_logs" class="logs_div">
                        <div>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblname" runat="server" Text="" ForeColor="Black"></asp:Label>
                            <asp:Label ID="logdate" runat="server" Text=""></asp:Label>
                        </div>
                        <fieldset id="in_logs" style="margin-left: 5px; margin-right: 5px; margin-bottom: 5px;
                            padding-bottom: 5px; padding-left: 5px; float: left; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Inside Details
                            </legend>
                            <asp:Label ID="lblDetailLogs" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                        <fieldset id="out_logs" style="margin-right: 5px; margin-bottom: 5px; padding-bottom: 5px;
                            padding-left: 5px; float: right; width: 163px; padding-right: 5px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Outside Details
                            </legend>
                            <asp:Label ID="lblOutsideDetails" runat="server" Style="text-align: center" CssClass="EMS_font_small"></asp:Label>
                        </fieldset>
                    </div>
                     <div id="appstatusDiv" class="status_div">
                            <fieldset id="fld" style="margin-right: 5px; margin-left: 5px; margin-bottom: 10px;
                                margin-top: 10px; padding-bottom: 5px; padding-left: 5px; width: 165px; padding-right: 5px;"
                                onmouseover="appshowstatusdiv();">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Approved/Reject
                                </legend>
                                <asp:Label ID="lblemp" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblADate" runat="server"></asp:Label>
                                <div style="text-align: left" onmouseover="appshowstatusdiv();">
                                    <asp:RadioButtonList ID="rblapprove" runat="server" CellPadding="0" CellSpacing="0"
                                        onmouseover="appshowstatusdiv();" RepeatDirection="Vertical">
                                        <asp:ListItem ID="liApp" runat="server" Text="Approved " Value="0"></asp:ListItem>                                        
                                        <asp:ListItem ID="liRej" runat="server" Text="Reject" Value="1"></asp:ListItem>
                                        <asp:ListItem ID="liPen" runat="server" Text="Pending " Value="2"></asp:ListItem>                                        
                                        <%--<asp:ListItem ID="lipen" runat="server" Text="Pending" Value="2"></asp:ListItem>--%>
                                    </asp:RadioButtonList>
                                </div>
                                <asp:TextBox ID="txtAppCom" runat="server" Height="70px" Width="96%" TextMode="MultiLine"
                                    onmouseover="appshowstatusdiv();"> </asp:TextBox>
                                <div style="text-align: center; margin-top: 5px;" onmouseover="appshowstatusdiv();">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Submit" CssClass="button" OnClick="btnUpdate_Click"
                                        onmouseover="appshowstatusdiv();" />
                                    <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="button" OnClientClick="self.close();"
                                        onmouseover="appshowstatusdiv();" />
                                </div>
                            </fieldset>
                        </div>

                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
