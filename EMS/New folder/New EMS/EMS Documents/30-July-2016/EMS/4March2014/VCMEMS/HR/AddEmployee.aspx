<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEmployee.aspx.cs" Inherits="HR_AddEmployee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/ajax.js" type="text/javascript"></script>
    <link href="../css/ems.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 431px;
        }
        .style2
        {
            width: 130px;
        }
    </style>
</head>
<body  style="margin: 0; padding: 0;" onload="window.parent.scroll(0,0);window.parent.document.getElementById('searchbar').style.display = 'none';" >
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
            
              <fieldset style="padding: 5px; width:590px;">
                            <legend style="margin-bottom: 10px; font-weight: normal; color: #808080;">
                                Add Employee</legend>
                  <table cellpadding="0">
                    <tr style="height:30px">
                    <td class="style2">
                        Employee Name
                    <span style="color: #FF0000">*</span>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtEmpName" runat="server" Width="200px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                ControlToValidate="txtEmpName" ErrorMessage="Enter proper name" 
                                ValidationExpression="^[a-zA-Z. ]+$"></asp:RegularExpressionValidator>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtEmpName"
                                ErrorMessage="Please enter employee name" CssClass="hideselect"></asp:RequiredFieldValidator>
                    </td>
                    </tr>
                      <tr style="height:30px">
                          <td class="style2">
                              Department <span style="color: #FF0000">*</span>
                          </td>
                          <td class="style1">
                              <asp:DropDownList ID="deptList" runat="server" CssClass="ddl" 
                               Width="220px">
                              </asp:DropDownList>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2w" runat="server" 
                                        ControlToValidate="deptList" CssClass="hideselect" 
                                        ErrorMessage="Please select department" InitialValue="Select.."></asp:RequiredFieldValidator>
                              &nbsp;</td>
                      </tr>
                     <tr style="height:30px">
                        <td class="style2">
                            Work Email <span style="color: #FF0000">*</span></td>
                        <td class="style1">
                            <asp:TextBox ID="txtWorkEmail" runat="server" Width="200px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="txtWorkEmail" ErrorMessage="Invalid E-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtWorkEmail"
                                ErrorMessage="Please enter work E-mail" CssClass="hideselect"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                       <tr style="height:30px">
                          <td class="style2">
                              Hire Date <span style="color: #FF0000">*</span></td>
                          <td class="style1">
                                <asp:TextBox ID="hireDate" runat="server" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="hireDate_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                TargetControlID="hireDate">
                            </asp:CalendarExtender>
                            &nbsp; &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server"
                                ControlToValidate="hireDate" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidat" runat="server" ControlToValidate="hiredate"
                                ErrorMessage="Please enter hire date" CssClass="hideselect"></asp:RequiredFieldValidator>
                          </td>
                      </tr>
                      <tr style="height:30px">
                          <td class="style2">
                              &nbsp;</td>
                          <td class="style1">
                              <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                                  onclick="btnSave_Click" />
                              &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" 
                                  onclick="btnCancel_Click" CausesValidation="False" />
                          </td>
                      </tr>
                    </table>
                   </fieldset>
                   <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                    ShowMessageBox="True" ShowSummary="False" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
