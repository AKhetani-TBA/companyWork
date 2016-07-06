<%@ Page Title="" Language="C#" MasterPageFile="~/Common.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Administration.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .quickAccess
        {
            list-style-type: none;
            width: 50%;
            padding-left: 0px;
        }

            .quickAccess li
            {
                /* text-align: center;*/
                cursor: pointer;
                padding: 1px 0px 1px 10px;
            }

            .quickAccess a
            {
                text-decoration: none;
                color: inherit;
            }

        .selectedPage
        {
            font-weight: bold;
            color: white;
            text-decoration: none;
            background-color: #353535;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $("ul.quickAccess li").each(function (e) {
                $(this).click(function (e) {
                    $("ul.quickAccess li").each(function () {
                        $(this).removeClass('selectedPage');
                    });
                    $(this).addClass('selectedPage');
                  
                    $('#<%=hdnRedirectTo.ClientID %>').val($(this).attr('name'));
                });
            });
        });

        function setDataType(ele) {
            document.getElementById('dataType').value = ele.innerHTML;
        }
    </script>

    <div class="row-fluid">
        <div class="span4" style="padding: 10px 0; line-height: 20px;">
            <div class="row-fluid">
                <div class="span12" id="dvUserLogin">
                    <asp:UpdatePanel ID="upnlUserLogin" runat="server">
                        <ContentTemplate>
                            <asp:MultiView ID="mvUserLogin" runat="server" ActiveViewIndex="0">
                                <asp:View ID="v_UserLogin" runat="server" OnActivate="v_UserLogin_Activate">
                                    <div class="control-group">
                                        <label class="control-label" for="txtSignInUserID">
                                            Email</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSignInUserID" runat="server" ValidationGroup="valgroupNewsLetterSignin"
                                                ToolTip="User ID (e.g. userid@thebeastapps.com)" placeholder="Email" CssClass="span12" />
                                            <asp:RequiredFieldValidator ID="rfielduid" runat="server" ValidationGroup="valgroupNewsLetterSignin"
                                                Display="None" ControlToValidate="txtSignInUserID" ErrorMessage="Please enter email id"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationGroup="valgroupNewsLetterSignin"
                                                runat="server" Display="None" ControlToValidate="txtSignInUserID" ToolTip="Enter Valid Email Id"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Please enter valid email id"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label" for="txtSignInUserPass">
                                            Password</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtSignInUserPass" runat="server" ValidationGroup="valgroupNewsLetterSignin"
                                                TextMode="Password" ToolTip="Password" placeholder="Password" CssClass="span12" />
                                            <asp:RequiredFieldValidator ID="rfieldpwd" runat="server" ValidationGroup="valgroupNewsLetterSignin"
                                                Display="None" ControlToValidate="txtSignInUserPass" ErrorMessage="Please enter password"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Button ID="btnSignIn" runat="server" Text="Sign in" CssClass="btn btn-inverse"
                                                OnClick="btnSignIn_Click" ValidationGroup="valgroupNewsLetterSignin" />
                                            <asp:ValidationSummary ID="vsSign" runat="server" ValidationGroup="valgroupNewsLetterSignin"
                                                ShowMessageBox="true" ShowSummary="false" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls" style="line-height: 15px;">
                                            <asp:Label ID="lblSigninMsg" ForeColor="Red" runat="server" Font-Names="verdana"
                                                Font-Size="Small"></asp:Label>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="v_UserForgotPwd_EmailInput" runat="server" OnActivate="v_UserForgotPwd_EmailInput_Activate">
                                    <%--Enter Your Registered Email Address to Receive Your Password--%>
                                    <div class="control-group">
                                        <div class="controls" style="line-height: 15px;">
                                            <strong>
                                                <asp:Label ID="lblInputEmailTitle" runat="server" Font-Names="verdana" Font-Size="Small"></asp:Label></strong>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label" for="txtForgetEmail">
                                            Email:</label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtForgetEmail" runat="server" CssClass="span12"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" ToolTip="Submit" CssClass="btn btn-inverse"
                                                ValidationGroup="valgroupNewsLetterSignin" OnClientClick="return validateEmail();"
                                                OnClick="btnSubmit_Click" />
                                            &nbsp;&nbsp;
                                                            <%--<a href="#" onclick="lnkBack_Click();" style="font-size: 85%;">< Back</a>--%>
                                            <asp:LinkButton ID="lbtnBack" runat="server" Style="font-size: 85%;" OnClick="lbtnBack_Click"> < Back </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls" style="line-height: 15px;">
                                            <asp:Label ID="lblEmailSubmitMsg" runat="server" Font-Names="verdana" Font-Size="Small"></asp:Label>
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="v_UserForgotPwd_SecQstn" runat="server">
                                    <div class="control-group">
                                        <div class="controls" style="line-height: 15px;">
                                            <strong style="font-family: Verdana; font-size: small;">Please answer below security
                                                                question</strong>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label" for="lblMail">
                                            Email:
                                        </label>
                                        <div class="controls">
                                            <asp:Label ID="lblMail" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label" for="lblSecQuestion">
                                            Question:
                                        </label>
                                        <div class="controls">
                                            <asp:Label ID="lblSecQuestion" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label" for="txtAnswer">
                                            Enter Answer
                                        </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtAnswer" runat="server" MaxLength="50" class="span12"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RfvtxtAns" runat="server" ControlToValidate="txtAnswer"
                                                ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <asp:HiddenField ID="hdfId" runat="server" />
                                        <asp:HiddenField ID="hdfAns" runat="server" />
                                        <div class="controls">
                                            <asp:Label ID="lblAttempt" runat="server">You have maximum 5 attempts.</asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Button ID="btnSubmitAnswer" runat="server" Text="Submit" CssClass="btn btn-inverse"
                                                OnClick="btnSubmitAnswer_Click" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Names="verdana" Font-Size="Small"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="controls">
                                            <asp:CheckBox ID="chkResetPass" runat="server" Text="Do you want to reset password for your registered Email address ?"
                                                AutoPostBack="true" TextAlign="Left" Visible="false" OnCheckedChanged="chkResetPass_CheckedChanged" />
                                        </div>
                                    </div>
                                </asp:View>
                                <asp:View ID="v_UserForgotPwd_ResetPwd" runat="server">
                                    RESET PASSWORD VIEW
                                </asp:View>
                            </asp:MultiView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="span6 offset2">
            <span class="devzone">MOVE TO...</span><br />
            <ul class="quickAccess">
                <li name="UM">Dashboard</li>
                <li name="UM">User Management</li>
                <li name="CM" id="CM">Company Management</li>
                <li name="AM" id="AM">Apps Management</li>
                <li name="AU">Auto URL</li>
            </ul>
            <asp:HiddenField ID="hdnRedirectTo" runat="server" Value="UM" />
        </div>
    </div>
</asp:Content>
