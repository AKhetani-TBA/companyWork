<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EmployeePersonal.aspx.cs"
    Inherits="HR_EmployeePersonal" %>

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
            width: 120px;
            height: 26px;
        }
        .style2
        {
            width: 402px;
            height: 26px;
        }
        .style4
        {
            width: 335px;
            height: 26px;
        }
    </style>
</head>
<body style="margin: 0; padding: 0;" onload="window.parent.scroll(0,0);window.parent.document.getElementById('searchbar').style.display = 'none';">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="EMS_font">
                <br />
                <table cellpadding="0" style="text-align: left; vertical-align: top">
                    <tr>
                        <td style="width: 120px; height: 26px;">
                            Employee&nbsp; Name <span style="color: #FF0000">* </span>
                        </td>
                        <td style="width: 402px; height: 26px;">
                            <asp:TextBox ID="fn" runat="server" Width="200px"></asp:TextBox>
                            <asp:DropDownList ID="showEmployees" runat="server" AutoPostBack="True" OnSelectedIndexChanged="showEmployees_SelectedIndexChanged"
                                Visible="False" Width="180px" CssClass="ddl">
                            </asp:DropDownList>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="fn"
                                ErrorMessage="Enter proper name" ValidationExpression="^[a-zA-Z. ]+$" ValidationGroup="Validation"></asp:RegularExpressionValidator>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="fn"
                                ErrorMessage="Please enter employee name" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Shift
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:DropDownList ID="ddlShift" runat="server" CssClass="ddl" Width="220px">
                                <asp:ListItem Text="General" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Night" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px;">
                            <asp:Label ID="lblempid" runat="server" Text="Employee Id"></asp:Label>
                        </td>
                        <td style="width: 402px; height: 26px;">
                            <asp:TextBox ID="empID" Style="float: left" runat="server" Width="200px" ClientIDMode='Static'
                                onkeyup="checkExistenceofId(this.value,'statusOfId');" Visible="False"></asp:TextBox>
                            <asp:Label ID="LabelEmpid" runat="server"></asp:Label>
                            <div style="float: left; color: red; padding-left: 5px;" id="statusOfId">
                            </div>
                        </td>
                        <td style="height: 26px;">
                            Department <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px;">
                            <asp:DropDownList ID="deptList" runat="server" CssClass="ddl" OnSelectedIndexChanged="deptList_SelectedIndexChanged"
                                Width="220px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="deptList"
                                CssClass="hideselect" ErrorMessage="Please select department" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px;" height="26">
                            Gender
                        </td>
                        <td style="height: 26px; width: 402px;" height="26">
                            <asp:RadioButton ID="m" runat="server" GroupName="sex" OnCheckedChanged="m_CheckedChanged"
                                Text="Male" Checked="True" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="f" runat="server" GroupName="sex" OnCheckedChanged="f_CheckedChanged"
                                Text="Female" />
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Designation <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:DropDownList ID="empDesignations" runat="server" Width="220px" CssClass="ddl">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="empDesignations"
                                CssClass="hideselect" ErrorMessage="Please select designation" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px;">
                            Date of Birth <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px;">
                            <asp:TextBox ID="dob" runat="server" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="dob_CalendarExtender" runat="server" TargetControlID="dob"
                                Format="dd MMMM yyyy">
                            </asp:CalendarExtender>
                            &nbsp;&nbsp;
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                                ControlToValidate="dob" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="dob"
                                ErrorMessage="Please enter date of birth" CssClass="hideselect" ValidationGroup="Validation" ></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Marital Status <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:DropDownList ID="status" runat="server" OnSelectedIndexChanged="status_SelectedIndexChanged"
                                Width="220px" CssClass="ddl">
                                <asp:ListItem>Select..</asp:ListItem>
                                <asp:ListItem Value="1">Married</asp:ListItem>
                                <asp:ListItem Value="0">Unmarried</asp:ListItem>
                                <asp:ListItem Value="2">Seperated</asp:ListItem>
                                <asp:ListItem Value="3">Divorced</asp:ListItem>
                                <asp:ListItem Value="4">Widowed</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="status"
                                CssClass="hideselect" ErrorMessage="Please select marital status" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px;">
                            Hire Date <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px;">
                            <asp:TextBox ID="hireDate" runat="server" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="hireDate_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                TargetControlID="hireDate">
                            </asp:CalendarExtender>
                            &nbsp; &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server" ValidationGroup="Validation"
                                ControlToValidate="hireDate" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidat" runat="server" ControlToValidate="hiredate"
                                ErrorMessage="Please enter hire date" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Blood Group <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:DropDownList ID="bg" runat="server" AutoPostBack="True" Width="220px" CssClass="ddl">
                                <asp:ListItem>Select..</asp:ListItem>
                                <asp:ListItem>Unknown</asp:ListItem>
                                <asp:ListItem>A+</asp:ListItem>
                                <asp:ListItem>B+</asp:ListItem>
                                <asp:ListItem>AB+</asp:ListItem>
                                <asp:ListItem>A-</asp:ListItem>
                                <asp:ListItem>B-</asp:ListItem>
                                <asp:ListItem>AB-</asp:ListItem>
                                <asp:ListItem>O+</asp:ListItem>
                                <asp:ListItem>O-</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="bg"
                                CssClass="hideselect" ErrorMessage="Please select blood group" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px;">
                            Contact No. <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px;">
                            <asp:TextBox ID="cno" runat="server" MaxLength="14" Width="150px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="cno"
                                ErrorMessage="Enter proper contact no." ValidationExpression="^[0-9+-]+$" ValidationGroup="Validation"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator9" runat="server" ControlToValidate="cno" CssClass="hideselect"
                                    ErrorMessage="Please enter contact number" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Mother Tongue <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:TextBox ID="mt" runat="server" Width="150px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server"
                                ErrorMessage="Enter proper mother tongue" ValidationExpression="^[a-zA-Z ]+$"
                                ControlToValidate="mt" ValidationGroup="Validation"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="mt"
                                ErrorMessage="Please enter mother tongue" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px">
                            Nationality <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px;">
                            <asp:TextBox ID="nationality" runat="server" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="nationality"
                                ErrorMessage="Please enter nationality" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                            &nbsp;&nbsp;
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Pan No <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:TextBox ID="panno" runat="server" MaxLength="13" Width="150px"></asp:TextBox>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ControlToValidate="panno" ErrorMessage="Enter proper pan no." ValidationGroup="Validation" ValidationExpression="^[0-9a-zA-Z ]+$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="panno"
                                ErrorMessage="Please enter pan no" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Personal E-Mail <span style="color: #FF0000">*</span>
                        </td>
                        <td class="style2">
                            <asp:TextBox ID="pmail" runat="server" Width="200px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                ControlToValidate="pmail" ErrorMessage="Invalid E-mail" ValidationGroup="Validation" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="nationality"
                                ErrorMessage="Please enter personal mail" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td class="style3">
                            Passport No <span style="color: #FF0000">*</span>
                        </td>
                        <td class="style4">
                            <asp:TextBox ID="passno" runat="server" OnTextChanged="dm1_TextChanged" Width="150px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="panno"
                                ErrorMessage="Please enter passport no" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 26px">
                            Work E-mail <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px;">
                            <asp:TextBox ID="mail" runat="server" Width="200px"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="mail" ErrorMessage="Invalid E-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="mail"
                                ErrorMessage="Please enter work E-mail" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            Passport Expiry
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:TextBox ID="passportExp" runat="server" Width="150px"></asp:TextBox>
                            <asp:CalendarExtender ID="passportExp_CalendarExtender" runat="server" Format="dd MMMM yyyy"
                                TargetControlID="passportExp">
                            </asp:CalendarExtender>
                            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ValidationGroup="Validation"
                                ControlToValidate="passportExp" ErrorMessage="Enter proper date" ValidationExpression="^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?!\ Feb(ruary)?))|(29(?=\ Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ (Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ ((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 12px; vertical-align: top">
                            Permanent Address <span style="color: #FF0000">*</span>
                        </td>
                        <td style="width: 402px;" height="80">
                            <div style="float: left; width: 200px">
                                <asp:TextBox ID="paddress" runat="server" Height="66px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="paddress"
                                    ErrorMessage="Please enter permanent address" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                            </div>
                            <div style="width: 119px; margin-top: 20px; float: right">
                                <asp:ImageButton ID="btncopyadd" runat="server" Width="40px" ImageUrl="~/images/rightarrow.png"
                                    CausesValidation="False" OnClick="btncopyadd_Click"/>
                            </div>
                        </td>
                        <td style="width: 101px; height: 12px; vertical-align: top">
                            Temporary <span style="color: #FF0000">*</span> Address
                        </td>
                        <td style="height: 12px; width: 335px;">
                            <asp:TextBox ID="taddress" runat="server" Height="66px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="taddress"
                                ErrorMessage="Please enter temporary address" CssClass="hideselect" ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 101px; height: 26px;">
                            Domicile <span style="color: #FF0000">*</span>
                        </td>
                        <td style="height: 26px; width: 402px; margin-left: 40px;">
                            <asp:DropDownList ID="ddlDomicile" runat="server" CssClass="ddl" Width="220px">
                                <asp:ListItem>Select..</asp:ListItem>
                                <asp:ListItem Value="1">Andhra Pradesh</asp:ListItem>
                                <asp:ListItem Value="2">Andaman and Nicobar Islands</asp:ListItem>
                                <asp:ListItem Value="3">Arunachal Pradesh</asp:ListItem>
                                <asp:ListItem Value="4">Assam</asp:ListItem>
                                <asp:ListItem Value="5">Bihar</asp:ListItem>
                                <asp:ListItem Value="6">Chandigarh</asp:ListItem>
                                <asp:ListItem Value="7">Chhattisgarh</asp:ListItem>
                                <asp:ListItem Value="8">Dadra and Nagar Haveli</asp:ListItem>
                                <asp:ListItem Value="9">Daman and Diu</asp:ListItem>
                                <asp:ListItem Value="10">Goa</asp:ListItem>
                                <asp:ListItem Value="11">Delhi</asp:ListItem>
                                <asp:ListItem Value="12">Gujarat</asp:ListItem>
                                <asp:ListItem Value="13">Haryana</asp:ListItem>
                                <asp:ListItem Value="14">Himachal Pradesh</asp:ListItem>
                                <asp:ListItem Value="15">Jammu and Kashmir</asp:ListItem>
                                <asp:ListItem Value="16">Jharkhand</asp:ListItem>
                                <asp:ListItem Value="17">Karnataka</asp:ListItem>
                                <asp:ListItem Value="18">Kerala</asp:ListItem>
                                <asp:ListItem Value="19">Lakshadweep</asp:ListItem>
                                <asp:ListItem Value="20">Madhya Pradesh</asp:ListItem>
                                <asp:ListItem Value="21">Maharashtra</asp:ListItem>
                                <asp:ListItem Value="22">Manipur</asp:ListItem>
                                <asp:ListItem Value="23">Meghalaya</asp:ListItem>
                                <asp:ListItem Value="24">Mizoram</asp:ListItem>
                                <asp:ListItem Value="25">Nagaland</asp:ListItem>
                                <asp:ListItem Value="26">Orissa</asp:ListItem>
                                <asp:ListItem Value="27">Puducherry</asp:ListItem>
                                <asp:ListItem Value="28">Punjab</asp:ListItem>
                                <asp:ListItem Value="29">Rajasthan</asp:ListItem>
                                <asp:ListItem Value="30">Sikkim</asp:ListItem>
                                <asp:ListItem Value="31">Tamil Nadu</asp:ListItem>
                                <asp:ListItem Value="32">Tripura</asp:ListItem>
                                <asp:ListItem Value="33">Uttar Pradesh</asp:ListItem>
                                <asp:ListItem Value="34">Uttarakhand</asp:ListItem>
                                <asp:ListItem Value="35">West Bengal</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlDomicile"
                                CssClass="hideselect" ErrorMessage="Please select domicile" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="deptList"
                                CssClass="hideselect" ErrorMessage="Please select department" InitialValue="Select.." ValidationGroup="Validation"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 101px; height: 26px;">
                            VCM Experience
                        </td>
                        <td style="height: 26px; width: 335px;">
                            <asp:Label ID="vcmexp" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; height: 12px; vertical-align: top">
                            &nbsp;
                        </td>
                        <td style="height: 12px; width: 402px; margin-left: 40px;">
                            &nbsp;
                        </td>
                        <td style="width: 101px; height: 12px; vertical-align: top">
                            &nbsp;
                        </td>
                        <td style="height: 12px; width: 335px;">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 26px; text-align: right;">
                            <asp:Button ID="updt" runat="server" Font-Bold="False" ValidationGroup="Validation"  
                                OnClick="updt_Click" Text="Save" CssClass="button" /> <%--OnClientClick="javascript:return validation()"--%>
                            &nbsp;&nbsp;<asp:Button ID="reset" runat="server" CausesValidation="false" OnClick="reset_Click"
                                Text="Reset" CssClass="button" />
                            &nbsp;
                        </td>
                        <td colspan="2" style="height: 26px;">
                            &nbsp;
                            <asp:Button ID="Button1" runat="server" Text="Back" OnClick="Button1_Click" CausesValidation="False"
                                CssClass="button" />
                            &nbsp;
                            <asp:Label ID="lblack" runat="server" ForeColor="#CC3300" Text="Details Saved."></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="Validation"
                    ShowMessageBox="True" ShowSummary="False" />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
