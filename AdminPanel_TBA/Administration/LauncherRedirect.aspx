<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LauncherRedirect.aspx.cs" Inherits="Administration.LauncherRedirect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Beast Framework-AutoURL</title>
    <link href="css/FinalStyleSheet.css" rel="stylesheet" />
    <link href="css/main.css" rel="stylesheet" />
    <link href="css/jquery-ui-1.7.1.custom.css" rel="stylesheet" />
    <link href="css/normalize.css" rel="stylesheet" />
    <link href="css/VCMComman.css" rel="stylesheet" />
    <link href="css/bootstrap.min.2.1.1.css" rel="stylesheet" />
    <link href="css/bootstrap-responsive.css" rel="stylesheet" />
    <link href="css/StyleSheet.css" rel="stylesheet" />
    <script src="js/jquery-1.9.1.js"></script>
    <script src="js/jquery-1.10.2.js"></script>
    <script src="js/jquery-ui-1.10.3.custom.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.5.js" type="text/javascript"></script>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="spripManger" runat="server"></asp:ScriptManager>
        <!---
            <p class="chromeframe">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> or <a href="http://www.google.com/chromeframe/?redirect=true">activate Google Chrome Frame</a> to improve your experience.</p>
        <![endif]-->
        <div class="row-fluid">
            <div class="span12">
                <!--Header-->
                <div class="row-fluid" style="background: url(images/body_bg.jpg) repeat-x top left;">
                    <div class="span10">
                        <div class="row-fluid">
                            <div class="span4" style="margin-left: 20px;">
                                <img src="images/beastlogo-1.png" class="pull-left" style="padding: 10px 0 3px 1px;"
                                    alt="" />
                            </div>

                        </div>
                    </div>
                    <div class="span2 visible-desktop">
                    </div>
                </div>

                <!--Body-->
                <div class="row-fluid">
                    <div class="span12">

                        <asp:UpdatePanel ID="uppNl" runat='server'>
                            <ContentTemplate>
                                <div class="row-fluid" style="padding: 10px 10px; line-height: 40px;">
                                    <div class="span12">
                                        <div class="row-fluid">
                                            <div class="span12">
                                                <span id="spnValidationMessage" style="color: red;"></span>

                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="" OnClick="lnkDownload_Click"></asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="row-fluid">
                                            <div class="span12">
                                                Please contact us for further assistance.<br />
                                                Phone: +1-646-688-7500<br />
                                                Email: info@thebeastapps.com<br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkDownload" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdfConversionID" runat="server" />
                        <asp:HiddenField ID="hdnUserID" runat="server" />
                        <input type="hidden" id="hdnPageFocusChange" value="true" />
                        <asp:HiddenField ID="hdn_VendorID" runat="server" />
                    </div>
                    <div class="span2 visible-desktop">
                    </div>
                </div>
                <!--Footer-->
                <div class="row-fluid footer">
                    <div class="span8 offset2">
                        <div class="row-fluid">
                        </div>
                        <div class="row-fluid">
                            <div class="span12 footer-copyright">
                                Copyright © 2005-2014. THE BEAST APPS. ALL RIGHTS RESERVED.
                            </div>
                        </div>
                    </div>
                    <div class="span2 visible-desktop">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
