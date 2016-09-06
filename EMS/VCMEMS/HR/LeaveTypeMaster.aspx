<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveTypeMaster.aspx.cs" Inherits="HR_LeaveTypeMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<script type="text/javascript">
//        function checkDate(sender, args) 
//        {
//            var StartDate = document.getElementById('startDate').value;
//            var EndDate = document.getElementById('endDate').value;
//            var eDate = new Date(EndDate);
//            var sDate = new Date(StartDate);
//            if (StartDate != '' && StartDate != '' && sDate > eDate) 
//            {
//                alert("Please ensure that the End Date is greater than or equal to the Start Date.");
//                sender._selectedDate = new Date();
//                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
//                return false;
//            }
//        }
//        function checkDates(sender, args) {
//            var StartDate = document.getElementById('start').value;
//            var EndDate = document.getElementById('end').value;
//            var eDate = new Date(EndDate);
//            var sDate = new Date(StartDate);
//            if (StartDate != '' && StartDate != '' && sDate > eDate) {
//                alert("Please ensure that the End Date is greater than or equal to the Start Date.");
//                sender._selectedDate = new Date();
//                sender._textbox.set_Value(sender._selectedDate.format(sender._format));
//                return false;
//            }
//        }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
   
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div class="EMS_font">
    
         <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
              <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
             <div  id="SearchMode"  runat="server">
                                <div  style=" text-align:right">
                                                    <br />
                                                   <asp:Button ID="btnAdd" runat="server" Text="Add LeaveType" OnClick="btnAdd_Click"   />
                                </div>
                    
                      <%--<table >
                                    <tr >
                                 <td><b>W.E.F :</b>
                                        <asp:TextBox ID="wef"   runat="server" AutoPostBack="True"
                                                ></asp:TextBox>
                                         <asp:CalendarExtender ID="startDate_CalendarExtender" runat="server" 
                                           OnClientDateSelectionChanged="checkDate" 
                                            Enabled="True" Format="dd MMMM yyyy" TargetControlID="wef">
                                        </asp:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                            ControlToValidate="wef" ErrorMessage="Improper date" 
                                            ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>
                                       </td>
                                 <td>&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/searchbtn.png"
                                    OnClick="btnSearch_Click" />
                                       </td>
                                     
                                    </tr>
                      </table>--%>
                                     
                  
             </div>
             <div id="EditMode" runat="server" class="EMS_font" >
         
             <fieldset style="padding: 5px;">
                        <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Add/Edit Holidays  </legend>
                <table>
            <tr>
                                        <td>
                                           W.E.F :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="wefDate" runat="server" AutoPostBack="True"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                                Format="dd MMMM yyyy" OnClientDateSelectionChanged="" 
                                                TargetControlID="wefDate">
                                            </asp:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                ControlToValidate="wefDate" ErrorMessage="Improper date" 
                                                ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$">*</asp:RegularExpressionValidator>
                                        </td>
                    <tr>
                        <td>
                            Leave Name</td>
                        <td width="75%">
                            <asp:TextBox ID="leaveName" runat="server" CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqLeaveName" runat="server" 
                                ControlToValidate="leaveName" Display="None" 
                                ErrorMessage="Please mention leave name" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Leave Abbrevation</td>
                        <td width="75%">
                            <asp:TextBox ID="leaveAbbrevation" runat="server" CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqAbbrevation" runat="server" 
                                ControlToValidate="leaveAbbrevation" Display="None" 
                                ErrorMessage="Please mention leave abbrevation" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Eligibility Criteria Start</td>
                        <td width="75%">
                            <asp:TextBox ID="eligibilityCriteriaStart" runat="server" 
                                CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ControlToValidate="eligibilityCriteriaStart" Display="None" 
                                ErrorMessage="Please mention eligibility criteria" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Eligibility Criteria End</td>
                        <td width="75%">
                            <asp:TextBox ID="eligibilityCriteriaEnd" runat="server" CausesValidation="True"></asp:TextBox>
                           <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ControlToValidate="eligibilityCriteriaEnd" Display="None" 
                                ErrorMessage="Please mention eligibility criteria" InitialValue="">*</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No. Of Days</td>
                        <td width="75%">
                            <asp:TextBox ID="days" runat="server" CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="days" Display="None" 
                                ErrorMessage="Please mention no. of days" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Max. Carry Forward Days</td>
                        <td width="75%">
                            <asp:TextBox ID="maxDays" runat="server" CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="maxDays" Display="None" 
                                ErrorMessage="Please mention max days" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                                      <tr>
                        <td>
                            Serial To Deduction</td>
                        <td width="75%">
                            <asp:TextBox ID="serialTodeduction" runat="server" CausesValidation="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ControlToValidate="serialTodeduction" Display="None" 
                                ErrorMessage="Please mention Serial To deduction" InitialValue="">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <table style="text-align: center; width:50%;">
                        <tr>
                            <td>
                                <asp:Button ID="saveBtn" runat="server" CssClass="button" 
                                    onclick="saveBtn_Click" Text="Save" />
                                <asp:Button ID="cancelBtn" runat="server" CausesValidation="False" 
                                    CssClass="button" onclick="cancelBtn_Click" Text="Cancel" Width="62px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                </div>
              <div id="DisplayMode" runat="server" style=" width: 95%; padding-left: 5px;">                        
                       <fieldset style="padding-top: 5px; padding-bottom: 5px; padding-left: 0px; padding-right: 0px;">
                                     <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">Leave Types</legend>
                                    <asp:GridView ID="displayAll" runat="server" AllowPaging="True" Width="100%"
                                         AllowSorting="True" AutoGenerateColumns="False"   Font-Bold="False" 
                                         OnPageIndexChanging="displayAll_PageIndexChanging" 
                                         OnRowCommand="displayAll_RowCommand" OnRowDataBound="displayAll_RowDataBound" 
                                         OnSelectedIndexChanged="displayAll_SelectedIndexChanged" 
                                         OnSorting="displayAll_OnSorting" PageSize="10" Font-Size="12px" Font-Names="Microsoft Sans Serif">
                            <RowStyle BorderColor="#333333" />
                            <SelectedRowStyle BackColor="Silver" ForeColor="Black" />
                            <HeaderStyle BackColor="#959595" Font-Names="Calibri" Font-Size="15px"
                                    ForeColor="White" HorizontalAlign="Left" Font-Bold="false"/>
                                         <Columns>
                                                 <asp:TemplateField  HeaderText="LeaveType Id">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblLeaveTypeId" runat="server" Text='<%# Bind("Leave_TypeId") %>'></asp:Label>
                                                 </ItemTemplate>
                                                 <ControlStyle CssClass="hideselect" />
                                                 <FooterStyle CssClass="hideselect" />
                                                 <HeaderStyle CssClass="hideselect" />
                                                 <ItemStyle CssClass="hideselect" />
                                             </asp:TemplateField>
                                                 <asp:BoundField DataField="Wef" DataFormatString="{0:dd-MM-yyyy}"  HtmlEncode="false"
                                               HeaderText="W.E.F" SortExpression="Wef">
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Serial_To_Deduction" HeaderStyle-Width="12%"
                                                  HeaderText="Serial Of Deduction" SortExpression="Serial_To_Deduction">
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Leave_Abbrv" HeaderStyle-Width="12%"
                                                HeaderText="Leave Abbrevation" SortExpression="Leave_Abbrv">
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Leave_Name"   HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="18%"
                                                  HeaderText="Leave Name" SortExpression="Leave_Name">
                                                     <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Eligibility_Criteria_Start"    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="18%"
                                                  HeaderText="Criteria Start" SortExpression="Eligibility_Criteria_Start">
                                                     <HeaderStyle HorizontalAlign="Left" Width="18%" />
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Eligibility_Criteria_End" HeaderStyle-Width="11%"
                                               HeaderText="Criteria End" SortExpression="Eligibility_Criteria_End">
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="No_Of_Days"  HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="7%"
                                                 HeaderText="Days" SortExpression="No_Of_Days">
                                             </asp:BoundField>
                                                 <asp:BoundField DataField="Max_CarryFwd_Days"    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"
                                                 HeaderText="Carry Fwd(Y/N)" SortExpression="Max_CarryFwd_Days">
                                             </asp:BoundField>
                                             <asp:TemplateField HeaderText="" ShowHeader="False">
                                                 <ItemTemplate>
                                                     <div style="text-align: center">
                                                         <asp:ImageButton ID="deltebtn" runat="server" CausesValidation="False" 
                                                             CommandArgument="delete" ImageUrl="~/images/Deletings.PNG" 
                                                             OnClientClick="javascript:if(!confirm('Are you sure ,you want to Delete Record?')){return false;}"  />
                                                     </div>
                                                 </ItemTemplate>
                                                 <ItemStyle Width="16px" />
                                             </asp:TemplateField>
                                             <asp:CommandField ShowSelectButton="True">
                                             <ControlStyle CssClass="hideselect" />
                                             <FooterStyle CssClass="hideselect" />
                                             <HeaderStyle CssClass="hideselect" />
                                             <ItemStyle CssClass="hideselect" />
                                             </asp:CommandField>
                                         </Columns>
                                         <EmptyDataTemplate>
                                             Sorry... No Data Found...!
                                         </EmptyDataTemplate>
                               <FooterStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="Silver" ForeColor="Black" />                                       
                                     </asp:GridView>
                                   <asp:HiddenField ID="hidproject" runat="server" />
                          </fieldset>
              </div>               
    </ContentTemplate>
     </asp:UpdatePanel>
     </div>
   
            
    </form>
</body>
</html>
