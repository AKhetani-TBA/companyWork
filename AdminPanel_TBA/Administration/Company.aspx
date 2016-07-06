<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/Admin.Master" CodeBehind="Company.aspx.cs" Inherits="Administration.Company" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #parentDiv
        {
            z-index: 1000;
            top: 0px;
            width: 100%;
            background: rgba(34, 19, 19, 0.11);
        }

        .lbjs-list
        {
            height: 259px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function SaveMessage() {
            alert("Company has been created successfully !!!");
        }
        function UpdateMessage() {
            alert("Details updated successfully !!!");
        }

        function ResetFunc() {
            $('#lblDtlStatus').text("");
        }
        function EnabledControls() {

            var txtUserID = document.getElementById('<%= txtCompanySmtpUserId_reg.ClientID %>');
            var txtPassword = document.getElementById('<%= txtCompanySmtpPassword_reg.ClientID %>');
            var txtPort = document.getElementById('<%= txtCompanySmtpPort_reg.ClientID %>');
            var txtSMTPServer = document.getElementById('<%= txtCompanySmtpServer_reg.ClientID %>');

            if (document.getElementById('<%= chkCompanyUseSmtp_reg.ClientID %>').checked === true) {
                txtUserID.disabled = false;
                txtPassword.disabled = false;
                txtPort.disabled = false;
                txtSMTPServer.disabled = false;
            }
            else {
                txtUserID.disabled = true;
                txtPassword.disabled = true;
                txtPort.disabled = true;
                txtSMTPServer.disabled = true;
            }
        }

        function pop(d, c) {
            var c1;
            var Value1 = d;

            $('#<%=hdnCompanyId.ClientID %>').val(c);
            $('#divGroup').css('top', 0);
            document.getElementById('<%=lstGroup.ClientID%>').selectedIndex = -1
            document.getElementById("<%=hdnSts.ClientID%>").value = "Submit";
            var div = document.getElementById('parentDiv');
            div.style.display = "block";
            if (Value1 != "") {
                var techGroups = document.getElementById("<%=lstGroup.ClientID%>");
                for (var j = 0; j < techGroups.options.length; j++) {
                    if (techGroups.options[j].value == Value1) {
                        techGroups.options[j].selected = true;
                        document.getElementById("<%=btnSubmit.ClientID%>").value = "Remove";
                        document.getElementById("<%=hdnSts.ClientID%>").value = "Remove";
                    }
                    else {
                        techGroups.options[j].disabled = true;
                    }
                }
            }
        }
        function ValidateCheckBox(sender, args) {
            var chkListModules = document.getElementById('<%= ddlproduct.ClientID %>');
            var chkListinputs = chkListModules.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

    </script>

    <script type="text/javascript">
        Sys.Application.add_load(BindFunctions);

        function BindFunctions() {

            $('#parentDiv').height($(document).height());
            var classType1 = document.getElementsByClassName("1");
            debugger;
            for (var i = 0; i < classType1.length; i++) {
                if (classType1[i].innerHTML == "")
                    classType1[i].innerHTML = "Assign";

            }
            var classType2 = document.getElementsByClassName("2");
            for (var i = 0; i < classType2.length; i++) {
                if (classType2[i].innerHTML == "")
                    classType2[i].innerHTML = "Assign";

            }
            $('#loading').show();
            $('#tblVendors').dataTable({
                "pagingType": "simple",
                "aaSorting": [],
                "initComplete": function (settings, json) {
                    $("#tblVendors").show();
                    $('#loading').hide();
                }
            });
            $('#<%=ddlproduct.ClientID %>').on('click', ':checkbox', function () {
                // if ($(this).is(':checked')) {
                //alert($(this).select());

                //       // handle checkbox check
                //       document.getElementById('<%=hdnApplication.ClientID%>').value += $(this).val() + ",";

                //     } else {
                //        document.getElementById('<%=hdnApplication.ClientID%>').value += "," + $(this).val() + ",";

                //    }
            });
        }
    </script>

    <div class="row-fluid">
        <div class="span12">
            <div class="row-fluid">
                <div class="span6">
                    <div class="section-outer">
                        <div class="section-title">
                        </div>
                        <div class="section-detail">
                        </div>
                    </div>
                </div>
                <div class="span6"></div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpnlVendor" runat="server">

        <ContentTemplate>
            <div class="span12">
                <div class="span10 offset1">
                    <%--  <div class="span12" style="margin-left: 10px;">--%>
                    <div class="control-group createUsr panel">
                        <div class="controls">
                            <div class="heading" style="padding-bottom: 0px!important">
                                <table>
                                    <tr>
                                        <td style="padding-left: 5px;" class="auto-style1">
                                            <asp:Label ID="lblUserCreationTitle" runat="server" Text="Company Creation"></asp:Label>
                                            <%--User Creation and Send AUTO URL--%>
                                        </td>

                                        <td width="50%" style="padding-left: 20%">

                                            <a id="link" href="#Existing" rel="up" style="color: black">Existing Company</a>

                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                        <div id="tblVend" class="pnl span12" style="margin: 0px">
                            <div class="span12">

                                <div class="span6" style="margin: 5px; background-color: aliceblue">
                                    <div class="span12" style="font-weight: bold; border-bottom: 2px solid navy; min-height: 1px;">Company Details</div>
                                    <div class="span12">
                                        <div class="span4">
                                            <span style="color: red">*</span> Company Name :
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanyName_reg" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfiledCmpnyName" runat="server" ControlToValidate="txtCompanyName_reg" ValidationGroup="valCompnyInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="vfiledCompany" runat="server" ValidationExpression="^[a-zA-z1-9_ ]{1,30}$" ControlToValidate="txtCompanyName_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            <span style="color: red">*</span> Company Legal Entity :
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtLglEntity_reg" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfiledLglEntity" runat="server" ControlToValidate="txtLglEntity_reg" ValidationGroup="valCompnyInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vtxtLegalEntity" runat="server" ValidationExpression="^[a-zA-z0-9 ]{1,30}$" ControlToValidate="txtLglEntity_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Title</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            <span style="color: red">*</span> Mnemonic :
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtMnemonic_reg" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfiledMnemomic" runat="server" ControlToValidate="txtMnemonic_reg" ValidationGroup="valCompnyInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vtxtMnemonic" runat="server" ValidationExpression="^[a-zA-z0-9 ]{1,30}$" ControlToValidate="txtMnemonic_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Website</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            <span style="color: red">*</span> Contact Email-Id :
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCntctEmail" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfiledEmail" runat="server" ControlToValidate="txtCntctEmail" ValidationGroup="valCompnyInformation" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vCntEmailID" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtCntctEmail" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Email Address
                                                    </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            &nbsp;  Company Type :
                                        </div>
                                        <div class="span8">

                                                    <asp:RadioButtonList ID="rcompnyType" runat="server" RepeatDirection="Horizontal" CssClass="radio control-label" Height="33px" Width="356px">
                                                        <asp:ListItem id="d0" Value="0" Selected="True">Customer</asp:ListItem>
                                                        <asp:ListItem id="d1" Value="1">Vendor</asp:ListItem>
                                                        <asp:ListItem id="d2" Value="2">Both</asp:ListItem>
                                                    </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            &nbsp;    Subscription Required : 
                                        </div>
                                        <div class="span8">
                                                    <asp:RadioButtonList ID="rsubscription" runat="server" RepeatDirection="Horizontal" CssClass="radio control-label" Height="33px" Width="356px" Style="margin-left: 10px">

                                                        <asp:ListItem Value="1">yes</asp:ListItem>
                                                        <asp:ListItem Selected="true" Value="0">No</asp:ListItem>

                                                    </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4" align="left" valign="middle">
                                            &nbsp;   Application  :
                                        </div>
                                        <div class="span8">

                                            <asp:CheckBoxList ID="ddlproduct" runat="server" ValidationGroup="valCompnyInformation" RepeatColumns="2" Repeadivirection="Horizontal" OnSelectedIndexChanged="ddlproduct_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Select atleast one application" ClientValidationFunction="ValidateCheckBox" ValidationGroup="valCompnyInformation" ForeColor="Red"></asp:CustomValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="span6" style="background-color: aliceblue; margin: 5px">

                                    <div class="span12" style="font-weight: bold; border-bottom: 2px solid navy; min-height: 1px">Contact Details : </div>

                                    <div class="span12">
                                        <div class="span4">
                                            Contact Person : 
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtContctPrsn" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="rtxtCntctPrsn" runat="server" ValidationExpression="^[a-zA-z1-9_ ]{1,30}$" ControlToValidate="txtCompanyName_reg" ValidationGroup="valCompnyInformation" ForeColor="#FF3300">Invalid Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                    <div class="span12">
                                        <div class="span4">
                                            From Email Id:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanyFromEmailId_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldSEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtCompanyFromEmailId_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Email Address</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            CC Email Id:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanyCCEmailId_reg" runat="server"></asp:TextBox>&nbsp;
                                            <asp:RegularExpressionValidator ID="vtxtCCEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtCompanyCCEmailId_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Email Address</asp:RegularExpressionValidator>

                                        </div>
                                    </div>

                                    <div class="span12">
                                        <div class="span4" valign="top">
                                            Contact Address:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtcompanyContactAddress_reg" TextMode="MultiLine" runat="server"
                                                        Rows="4"></asp:TextBox>
                                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ValidationExpression="^[a-zA-z0-9/ ]{1,100}$" ControlToValidate="txtompanyContactAddress_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Address</asp:RegularExpressionValidator>--%>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            City : 
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCity_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vTxtCity" runat="server" ControlToValidate="txtCity_reg" ValidationGroup="valCompnyInformation" ForeColor="#FF3300" ValidationExpression="^[a-zA-z _ ]{1,30}$">Invalid City Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            State : 
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtState_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vtxtState" runat="server" ControlToValidate="txtState_reg" ValidationGroup="valCompnyInformation" ForeColor="#FF3300" ValidationExpression="^[a-zA-z _ ]{1,30}$">Invalid State Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                    <div class="span12">
                                        <div class="span4">
                                            ZipCode : 
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtZipCode_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserZipCode" runat="server" ValidationExpression="^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$" ControlToValidate="txtZipCode_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid ZipCode</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            Country : 
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCountry_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserCntry" runat="server" ValidationExpression="^[a-zA-z.]{1,30}$" ControlToValidate="txtCountry_reg" ValidationGroup="valCompnyInformation" ForeColor="#FF3300"> InValid Country Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="span12" style="margin: 5px">

                                <div class="span6" style="background-color: aliceblue">

                                    <div class="span12" style="font-weight: bold; min-height: 1px; border-bottom: 2px solid navy">Extra Details</div>


                                    <div class="span12">
                                        <div class="span4">
                                            Company Logo:
                                        </div>
                                        <div class="span8">


                                                    <asp:FileUpload ID="fupCompanyLogo_reg" runat="server" />
                                        </div>
                                    </div>

                                    <div class="span12">
                                        <div class="span4">
                                            Website:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanyWebsite_reg" runat="server"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+[.com]+(/[/?%&=]*)?" ControlToValidate="txtCompanyWebsite_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Website</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4" valign="top">
                                            Signature:
                                        </div>

                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanyEmailSignature_reg" TextMode="MultiLine" runat="server"
                                                        Rows="4"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="span12">
                                        <div class="areaTitle">
                                                    <input type="checkbox" id="chkCompanyUseSmtp_reg" runat="server" name="chkExchangeServer" value="chkExchangeServer" onclick="EnabledControls()" />
                                                    <%--<asp:CheckBox ID="chkCompanyUseSmtp_reg" runat="server" AutoPostBack="True" OnCheckedChanged="chkCompanyUseSmtp_reg_CheckedChanged" />--%>
                                                            &nbsp;Use Company's exchange server
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            SMTP Server Name:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanySmtpServer_reg" runat="server" Enabled="false"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtCompanySmtpServer_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Server Name</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            SMTP UserId:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanySmtpUserId_reg" runat="server" Enabled="false"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtCompanySmtpUserId_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid SMTP UserId</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            SMTP Password:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanySmtpPassword_reg" runat="server" Enabled="false"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtCompanySmtpPassword_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Password</asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="span12">
                                        <div class="span4">
                                            SMTP Port:
                                        </div>
                                        <div class="span8">
                                                    <asp:TextBox ID="txtCompanySmtpPort_reg" runat="server" Enabled="false"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtCompanySmtpPort_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid SMTP Port</asp:RegularExpressionValidator>
                                        </div>
                                    </div>

                                </div>
                                <div class="span6" style="margin-left: 10px;">
                                    <div class="span12" style="background-color: aliceblue">
                                        <div class="span12" style="font-weight: bold; min-height: 1px; border-bottom: 2px solid navy">Clearing Details</div>
                                        <div class="span12">
                                            <div class="span4">
                                                MPID : 
                                            </div>
                                            <div class="span8">
                                                <asp:TextBox ID="txtMPID_reg" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vtxtMPID" runat="server" ValidationExpression="^[a-zA-z0-9/ ]{1,100}$" ControlToValidate="txtMPID_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid MPID</asp:RegularExpressionValidator>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="span4">
                                                DTC : 
                                            </div>
                                            <div class="span8">
                                                <asp:TextBox ID="txtDTC_reg" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ValidationExpression="^[a-zA-z0-9/ ]{1,100}$" ControlToValidate="txtDTC_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid DTC</asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="span4">
                                                Pershing Account No  : 
                                            </div>
                                            <div class="span8">
                                                <asp:TextBox ID="txtPershingAcctNo_reg" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vPershingAccntNo" runat="server" ValidationExpression="^[a-zA-z0-9/ ]{1,100}$" ControlToValidate="txtPershingAcctNo_reg" ValidationGroup="valCompnyInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Pershing Account No</asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="span12" style="margin: 5px; margin-left: 0px; background-color: aliceblue">

                                        <div class="span12" style="font-weight: bold; min-height: 1px; border-bottom: 2px solid navy">Fix Details</div>

                                        <div class="span12">
                                            <div class="span4">
                                                    <label id="lblCompID" runat="server">Client Comp ID </label>
                                            </div>
                                            <div class="span8">
                                                    <asp:TextBox ID="txtcompId" runat="server" Style="margin-bottom: 5px" />
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="valgroupNewsSubmit" ControlToValidate="txtcompId" ForeColor="#FF3300">*</asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>
                                        <div class="span12">
                                            <div class="span4">
                                                Password : 
                                            </div>
                                            <div class="span8">
                                                    <asp:TextBox ID="txtPswd" runat="server"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="valgroupNewsSubmit" ControlToValidate="txtPswd" ForeColor="#FF3300">*</asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>

                                        <div class="span12">
                                            <div class="span4">
                                                    <label id="lblMsgSprt" runat="server">Ack Msg Support </label>
                                            </div>
                                            <div class="span8">
                                                    <asp:RadioButtonList ID="rMsgSupport" runat="server" RepeatDirection="Horizontal" CssClass="radio control-label" Height="33px" Style="margin-left: 10px">

                                                        <asp:ListItem Value="Y">yes</asp:ListItem>
                                                        <asp:ListItem Selected="true" Value="N">No</asp:ListItem>

                                                    </asp:RadioButtonList>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="span12" style="margin: 5px">

                                        <asp:Button ID="btnSaveCompanyInfo" Style="margin-left: 10px; float: left" runat="server" Text="SUBMIT" CssClass="btnClass" OnClick="btnSaveCompanyInfo_Click" OnClientClick="ResetFunc()" ValidationGroup="valCompnyInformation" />
                                                    <asp:ValidationSummary ID="vfieldSmry" runat="server" ValidationGroup="valCompnyInformation"
                                                        ShowMessageBox="false" ShowSummary="false" />
                                        <asp:Label ID="lblDtlStatus" runat="server" ForeColor="#FF3300" Style="margin-left: 5px"></asp:Label>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="span12" style="margin-left:0px; margin-top:20px">

                <div class="span10 offset1 panel ImagesInGrp">
                    <%--      <div class="span12" style="margin-left: 10px;">--%>
                    <div class="control-group">
                        <div class="controls">
                            <div class="heading" id="Existing">
                                Existing Company
                            </div>
                        </div>
                        <div class="pnl span12" style="margin-left:0px">
                            <div id="loading" style="vertical-align:central">
                                        <img src="images/loadingImage.png" />
                                    </div>
                                        <asp:Repeater ID="rptrCompanies" runat="server">
                                            <HeaderTemplate>
                                    <table id="tblVendors" class="hor-scroll" style="border-spacing: 0; display: none">
                                                    <thead>
                                                        <tr>
                                                            <%--  <th></th>--%>
                                                            <th style="text-align: left;">Company ID
                                                            </th>
                                                            <th style="text-align: left;">Company Name
                                                            </th>
                                                            <th style="text-align: left;">Legal Entity
                                                            </th>
                                                            <th style="text-align: left;">Mnemonic
                                                            </th>
                                                            <th style="text-align: left;">Group</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>

                                                    <td>
                                                        <%#Eval("CompanyId")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("CompanyName")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("CompanyLegalEntity")%> 
                                                    </td>
                                                    <td width="250px !important">
                                                        <%#Eval("Mnemonic")%>
                                                    </td>
                                                    <td style="width: 150px !important">
                                                        <%-- <input type="image" id="editVendor"  src="images/group.png" onclick="javascript:btnVendorClick();"  />       --%>
                                                        <asp:Label ID="getVendor" class='<%#Eval("CompanyType")%>' onclick='<%# String.Format("return pop(\"{0}\", \"{1}\")", Eval("GroupId"),Eval("CompanyId")) %>' runat="server" Text='<%#Eval("GroupName")%>' ForeColor="Blue" Style="cursor: pointer">  </asp:Label>

                                                    </td>
                                                    <td id='<%#Eval("CompanyType") %>'>
                                                        <asp:ImageButton ID="getDetails" CommandArgument='<%#Eval("CompanyId")%>' OnClick="btngetDetails_Click" ValidationGroup="valgroupNewsLetterUsrDetails" ToolTip="Edit Vendors Details" src="images/getDetails.png" runat="server" />

                                                        <%--<asp:ImageButton ID="btnImgPrmsn" CommandArgument='<%#Eval("CompanyId")%>' OnClick="btnImagePermission_Click" ToolTip="Image Permissions" src="images/imagePermissions.png" runat="server" />--%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody> </table>
                                            </FooterTemplate>
                                        </asp:Repeater>

                        </div>
                        <asp:Label ID="lblGroup" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
            <div id="parentDiv" style="position: fixed; display: none;">
                <div class="span12">
                    <div class="span4"></div>
                    <div class="span4 panel">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                Select Group
                            </div>
                        </div>
                        <div class="panel-success span12 offset2">

                            <%-- <div id="divGroup" class="span4" style="border: 1px solid black">
                            --%>
                            <asp:ListBox ID="lstGroup" runat="server" Height="245px" SelectionMode="Multiple"></asp:ListBox>



                            <%-- </div>--%>
                        </div>
                        <div class="span12" style="margin-top: 10px; margin-left: 0px !important">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info" OnClick="btnVendorGroup_Click" Width="48%"></asp:Button>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-info" Width="48%"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
            <div>
                <asp:HiddenField ID="hdnLogo" Value="" runat="server" />
                <asp:HiddenField ID="hdn_vndrId" Value="" runat="server" />
                <asp:HiddenField ID="hdnEditCompanyId" runat="server" Value="" />
                <asp:HiddenField ID="hdn_Userid" runat="server" Value="" />
                <asp:HiddenField ID="hdnCompanyId" runat="server" />
                <asp:HiddenField ID="hdnGrpId" runat="server" />
                <asp:HiddenField ID="hdnSts" runat="server" />
                <asp:HiddenField ID="hdnApplication" runat="server" />
            </div>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveCompanyInfo" />

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>


