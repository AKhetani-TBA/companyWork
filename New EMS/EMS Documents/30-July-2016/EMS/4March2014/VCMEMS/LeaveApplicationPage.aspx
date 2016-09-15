<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveApplicationPage.aspx.cs"
    Inherits="HR_LeaveApplicationPage" EnableEventValidation="false" Async="true" %>

<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/ajax.js" type="text/javascript"></script>
    <link href="css/ems.css" rel="stylesheet" type="text/css" />
 
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
    <script type="text/javascript">
        function checkDate(sender, args) {
            var StartDate = document.getElementById('fromDate').value;
            var EndDate = document.getElementById('toDate').value;
            var eDate = new Date(EndDate);
            var sDate = new Date(StartDate);
            var chk_arr = document.getElementById('chkProbable');
            if (StartDate != '' && EndDate != '' && sDate > eDate) 
            {
                alert("Please ensure that the End Date is greater than or equal to the Start Date.");
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
                return false;
            }
            if (sender._selectedDate.getDay() == 0) 
            {
                sender._selectedDate = new Date();
                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
                alert("Sunday is already Holiday !");
            }
           if(sender._selectedDate < new Date())
               {
                    var conf = confirm("Are You sure you want to add leave for Previous date?");
                    if (conf == true) {
                        sender._selectedDate = sender._selectedDate.format(sender._format);                        
                         chk_arr.disabled = true;
                    }
                    else {
                        sender._textbox.set_Value(sender._selectedDate.format(sender._format));
                        chk_arr.disabled = false;
                    }
                }
            }
     
    </script>
   <script type="text/javascript">
       function checkDt(sender, args) {
           var StartDate = document.getElementById('txtFormDate').value;
           var EndDate = document.getElementById('txtTodate').value;
           var eDate = new Date(EndDate);
           var sDate = new Date(StartDate);
           //            if (StartDate != '' && EndDate != '' && sDate > eDate)

           if (EndDate < StartDate) {
               alert("Please ensure that the To Date is greater than or equal to the From Date.");
               sender._selectedDate = new Date();
               sender._textbox.set_Value(sender._selectedDate.format(sender._format));
               return false;
           }
       }
   
   </script>
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
            <div class="EMS_font">
                <div id="SearchMode" runat="server">
                    <table width="100%">
                        <tr>

                       <td width="90%">
                    <fieldset>
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Department / Employee / Product (Project)  wise Search
                        </legend>
                                    Department 
         <%--                       </td>
                                <td >--%>
                                    <asp:DropDownList ID="showDepartments" runat="server" AutoPostBack="True"
                                        Width="175px" OnSelectedIndexChanged="showDepartments_SelectedIndexChanged">
                                    </asp:DropDownList>
                 <%--               </td>
                                <td>--%>
                                    Employee 
       <%--                         </td>
                                <td >--%>
                                    <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="false" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                        Width="180px">
                                    </asp:DropDownList>
                 <%--               </td>--%>
  <%--                            <td  width="10%">--%>
                                   <b>OR </b> Product :
<%--                                </td>
                            <td  width="20%">                                
--%>                                      <asp:DropDownList ID="showProduct" runat="server" AutoPostBack="True"  Width="190px"
                                          OnSelectedIndexChanged="showProduct_SelectedIndexChanged">
                                      </asp:DropDownList>
                                    
                            <%--   </td>
                            <td  width="5%">             --%>                  
                                   &nbsp;&nbsp;&nbsp;   <asp:ImageButton ID="btnSearch" runat="server" 
                                          ImageUrl="~/images/searchbtn.png" OnClick="btnSearch_Click" /> 
                                           </fieldset>
                            </td>     
                          <td width="3%">
                                <asp:Button ID="team" runat="server" CommandArgument="1" Text="TLs/Mgr"  onclick="team_Click"/> 
                      </td>
                      <td width="2%">
                                  <asp:ImageButton ID="btnexcel" runat="server" ImageUrl="~/images/excelicon.png" Width="23px"
                                        OnClick="btnexcel_Click" /><br />
                        </td>  
                                  
                         </tr>
                         </table>
                 <table>
                            <tr> 
                         <td >
                         <fieldset>
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Apply Leave
                        </legend>
                                     <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click"  
                                         Text="Click to Apply For Leave" />
                                       <br /><br />
                                         </fieldset>  
                                 </td>

                               <td > 
                        <fieldset style="width:450px;" >
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
                           <td >
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
                            </tr>
                   </table>                
                   </div>           
                <div id="EditMode" runat="server" >
                    <fieldset style="padding: 1px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Apply Leave
                        </legend>
                        <table style="width: 95%">
                            <tr>
                                <%--      Username : -
                                    <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                </td>--%>
                                <td align="right">
                                <b>    Leave Applying Date : -
                                    <asp:Label ID="lblDate" runat="server" ></asp:Label></b>
                                </td>
                                <%--     <td>
                                    Date Of Join : -
                                    <asp:Label ID="lblDateOfJoin" runat="server"></asp:Label>
                                </td>--%>
                            </tr>
                        </table>
                        <table style="width:95%;">
                            <tr style="width:95%">
                                <td style="width:25%">
                                    Employee
                                </td>
                                <td style="width:70%">
                                    <asp:DropDownList ID="ddlEmp" runat="server" CssClass="ddl" 
                                        AutoPostBack="false" Width="20%" 
                                           >
                                    </asp:DropDownList>
                                </td>
                         
                            </tr>
                            <tr>
                                <td >
                                    Leave Date
                                </td>
                                <td>
                                    <asp:TextBox ID="fromDate" runat="server"  AutoPostBack="true"  Width="20%" 
                                        ontextchanged="fromDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="fromDate_CalendarExtender" runat="server" Format="dd MMM yyyy"
                                        OnClientDateSelectionChanged="checkDate" TargetControlID="fromDate">
                                    </asp:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="rgProperDateFrom" runat="server" ControlToValidate="fromDate"
                                        ErrorMessage="Improper date"  Display="None" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="reqDateFrom" runat="server" ControlToValidate="fromDate"
                                        ErrorMessage="Please enter date"  Display="None" InitialValue="">*</asp:RequiredFieldValidator>
                             &nbsp;&nbsp;&nbsp;
                       
                                    To &nbsp;
                                    <asp:TextBox ID="toDate"  runat="server" width="20%" AutoPostBack="true"
                                        ontextchanged="toDate_TextChanged"  ></asp:TextBox>
                                    <asp:CalendarExtender ID="toDate_CalendarExtender1" runat="server" Format="dd MMM yyyy"  
                                        OnClientDateSelectionChanged="checkDate" TargetControlID="toDate">
                                    </asp:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="rgProperDateTo" runat="server" ControlToValidate="toDate"
                                        ErrorMessage="Improper date"   Display="None" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="toDate"
                                        ErrorMessage="Please enter date"  Display="None" InitialValue="">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            
                            </td>
                            <td>
                                     <b><asp:Label ID="lblFromDay" runat="server" Width="26%" ForeColor="#a31515" Font-Size="13px"></asp:Label></b>
                                     
                                    <b><asp:Label ID="lblToDay" runat="server" Width="25%" ForeColor="#a31515" Font-Size="13px"></asp:Label></b>

                            </td>
                            </tr>
                            <tr >
                                <td >
                                   Full / Half  Day
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDayType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDayType_SelectedIndexChanged" Width="20%"
                                        OnLoad="ddlDayType_SelectedIndexChanged" >
                                        <asp:ListItem >-- Select day type</asp:ListItem>
                                        <asp:ListItem Selected="True">Full Day</asp:ListItem>
                                        <asp:ListItem>Half Day</asp:ListItem>
                                    </asp:DropDownList>                             
                                    <asp:RadioButton ID="radFirstHalf" runat="server" AutoPostBack="true" GroupName="radHalfDay"
                                        Text="1st Half"  Visible="False" OnCheckedChanged="radFirstHalf_CheckedChanged" Checked="true"/>
                                    <asp:RadioButton ID="radSecondHalf" runat="server" AutoPostBack="true" GroupName="radHalfDay"
                                        Text="2nd Half" Visible="False" OnCheckedChanged="radSecondHalf_CheckedChanged" />
                             
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Leave Probable ?                             
                                    <asp:CheckBox  ID="chkProbable"  runat="server"   />
                                </td>
                              <%--  <td >
                                    <asp:ValidationSummary ID="vsDateValidation" runat="server" Height="16px" ShowMessageBox="True"
                                        ShowSummary="False"  Width="16px" />
                                </td>--%>
                            </tr>  
                            <tr>
                                <td style="width:"40%">
                                    Reason 
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine"   Width="60%"></asp:TextBox>
                                     <asp:ValidationSummary ID="vsDateValidation" runat="server" Height="16px" ShowMessageBox="True"
                                        ShowSummary="False"  Width="16px" />
                                </td>
                            </tr>
                        </table>
                        <fieldset style=" width: 90%;">
                            <legend style="margin-right: 5px; font-weight: normal; color: #808080;">Extra Information
                            </legend>
                            <table style="width:80%; ">
                                <tr>
                                    <td style="width: 50%;text-align:left;">
                                        1. Out Of Town?
                                    </td>
                                    <td style="width: 30%;text-align:left;">
                                        <asp:RadioButton ID="radTownYes" GroupName="radInTown" runat="server" Text="Yes"
                                            AutoPostBack="True" OnCheckedChanged="radTownYes_CheckedChanged" Checked="True" />
                                        <asp:RadioButton ID="radTownNo" GroupName="radInTown" runat="server" Text="No" AutoPostBack="True"
                                            OnCheckedChanged="radTownNo_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2. Available on call ?
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radAvailYes" GroupName="radAvailCall" runat="server" Text="Yes"
                                            AutoPostBack="True" Checked="True" OnCheckedChanged="radAvailYes_CheckedChanged" />
                                        <asp:RadioButton ID="radAvailNo" GroupName="radAvailCall" runat="server" Text="No"
                                            AutoPostBack="True" OnCheckedChanged="radAvailNo_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3. Availability Of System/Internet ?
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radSysAvailYes" GroupName="radAvailSys" runat="server" Text="Yes"
                                            AutoPostBack="True" Checked="True" OnCheckedChanged="radSysAvailYes_CheckedChanged" />
                                        <asp:RadioButton ID="radSysAvailNo" GroupName="radAvailSys" runat="server" Text="No"
                                            AutoPostBack="True" OnCheckedChanged="radSysAvailNo_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        4. Available For Office Emergency From Location?
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radEmergencyLocYes" GroupName="radEmergencyLoc" runat="server"
                                            Text="Yes" AutoPostBack="True" Checked="True" />
                                        <asp:RadioButton ID="radEmergencyLocNo" GroupName="radEmergencyLoc" runat="server"
                                            Text="No" AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5. Available For Office Emergency At Office?
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radEmergencyOfcYes" GroupName="radEmergencyOfc" runat="server"
                                            Text="Yes" AutoPostBack="True" />
                                        <asp:RadioButton ID="radEmergencyOfcNo" GroupName="radEmergencyOfc" AutoPostBack="True"
                                            runat="server" Text="No" Checked="True" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <table style="text-align: center; width:90%;">
                            <tr>
                                <td >
                                    <asp:Button ID="btnSave" runat="server" Text="Apply Leave" OnClick="btnSave_Click"
                                        Width="93px" />                       
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        CausesValidation="False" Width="58px" />
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>           
                <div id="DisplayMode" runat="server" style=" width: 100%;">
                    <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Leave Summary
                        </legend>
                        <div style="height: 500px;width: 100%; overflow-y: auto; overflow-x: auto;" >                         
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
                                  <asp:BoundField HeaderText="Product" DataField="Projectname" SortExpression="Projectname" ItemStyle-Wrap="true" ItemStyle-Width="15%">
                              </asp:BoundField>
                             <asp:BoundField DataField="DayType" HeaderText="Type" SortExpression="DayType"  ItemStyle-Width="3%" >
                                </asp:BoundField>
                                <asp:BoundField DataField="IsProbable" HeaderText="Probable" SortExpression="IsProbable"  ItemStyle-Width="3%"  >
                                </asp:BoundField>
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
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <div style="text-align: center;">
                                            <asp:ImageButton ID="statusImageButton" runat="server" CausesValidation="False" Height="20px"  Width="20px" 
                                            ImageUrl="~/images/Extrainfo.jpg" CommandArgument="<%# Container.DataItemIndex %>"
                                            OnClick="statusImageButton_Click " onmouseover="setstatusdivExtra();" />
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle />
                                </asp:TemplateField>
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
                         
                            </Columns>
                            <PagerStyle />
                            <EmptyDataTemplate>
                                Sorry... No Data Found...!
                            </EmptyDataTemplate>
                             <FooterStyle BackColor="White" ForeColor="#333333" />
                             <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                        </asp:GridView>
                           </fieldset>
                           </div>
                <div id="statusDivExtra" class="status_div" style="border-left: solid 3px #999999;
                            border-top: solid 1px #C0C0C0; border-right: solid 1px #C0C0C0; border-bottom: solid 3px #999999;
                            position: relative; width: 267px; height: 250px;">
                            <fieldset id="Fieldset2" style="margin-right: 5px; margin-left: 5px; margin-bottom: 5px;
                                margin-top: 5px; padding-bottom: 2px; padding-left: 2px; padding-top: 2px; padding-right: 2px;
                                width: 250px;">
                                <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Extra Information
                                </legend>
                                <div>
                                    1. Out Of Town?
                                    <br />
                                    <asp:RadioButton ID="radOutTown" GroupName="radInTown" runat="server" Text="Yes"
                                        AutoPostBack="false"  Checked="True" />
                                    <asp:RadioButton ID="radInTown" GroupName="radInTown" runat="server" Text="No" AutoPostBack="false"
                                        />
                                    <br />
                                    2. Available on call ?
                                    <br />
                                    <asp:RadioButton ID="radCallYes" GroupName="radAvailCall" runat="server" Text="Yes"
                                        AutoPostBack="false" Checked="True" />
                                    <asp:RadioButton ID="radCallNo" GroupName="radAvailCall" runat="server" Text="No"
                                        AutoPostBack="false"  />
                                    <br />
                                    3. Availability Of System/Internet ?
                                    <br />
                                    <asp:RadioButton ID="radSysYes" GroupName="radAvailSys" runat="server" Text="Yes"
                                        AutoPostBack="false" Checked="True" />
                                    <asp:RadioButton ID="radSysNo" GroupName="radAvailSys" runat="server" Text="No" AutoPostBack="false"
                                        />
                                    <br />
                                    4. Available For Office Emergency From Location?
                                    <br />
                                    <asp:RadioButton ID="radEmergeLocationYes" GroupName="radEmergencyLoc" runat="server"
                                        Text="Yes" AutoPostBack="false" Checked="True" />
                                    <asp:RadioButton ID="radEmergeLocationNo" GroupName="radEmergencyLoc" runat="server"
                                        Text="No" AutoPostBack="false" />
                                    <br />
                                    5. Available For Office Emergency At Office?
                                    <br />
                                    <asp:RadioButton ID="radEmergeOfficeYes" GroupName="radEmergencyOfc" runat="server"
                                        Text="Yes" AutoPostBack="false" />
                                    <asp:RadioButton ID="radEmergeOfficeNo" GroupName="radEmergencyOfc" AutoPostBack="false"
                                        runat="server" Text="No" Checked="false" />
                                    <br />
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="btnStatusSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnStatusSubmit_Click"  />
                                    <asp:Button ID="btnClosed" runat="server" Text="Cancel" CssClass="button" OnClientClick="self.close();" />
                                </div>
                         
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
                     
                        <asp:HiddenField ID="hidproject" runat="server" />
                        <%--  <div id="statusDiv" class="status_div"></div>--%>
                   
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
