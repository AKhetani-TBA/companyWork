<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="True" CodeBehind="Users.aspx.cs" Inherits="Administration.Users" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">






    <script type="text/javascript">

        function BindFunctions() {
            $('#loading').show();
            $('#tblUsers').dataTable({
                "pagingType": "simple",

                "aaSorting": [],
              
                "initComplete": function (settings, json) {
                    $('#loading').hide();
                    $("#tblUsers").show();
                }
            });
            $(".tabs-menu a").click(function (event) {

                event.preventDefault();
                $(this).parent().addClass("current");
                $(this).parent().siblings().removeClass("current");
                var tab = $(this).attr("href");
                $(".tab-content").not(tab).css("display", "none");
                $(tab).fadeIn();
            });
            $('#<%=txtChangePassword.ClientID%>').val("");
            $('#<%=companySearch.ClientID %>').keyup(function () {
                var valThis = $(this).val().toLowerCase();
                $('#<%=drpCompany.ClientID %> option').each(function () {
                    var text = $(this).text().toLowerCase();
                    (text.indexOf(valThis) == 0) ? $(this).show() : $(this).hide();
                });
            });

        }

    </script>
    <script type="text/javascript">
        function btnSaveCompanyDetails() {
            var selectedval = $('#<%=drpCompany.ClientID %>').val();
            var userid = $('#<%=hdnStoreUserID.ClientID%>').val();
            var action;
            if ($('#<%=btnCompany.ClientID%>').val() == "Add")
                action = 'N';
            else
                action = 'D';
            var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '" ,"companyid":"' + selectedval + '" ,"action":"' + action + '" ,"currentUserID":"' + $('#<%=hdnUserID.ClientID%>').val() + '"}';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/AddRemoveCompany",
                success: function (data1) {
                    if (data1.d == "0") {
                        $('#<%=lblCmpny.ClientID%>').text("Company assigned to user");
                        $('#<%=btnCompany.ClientID%>').val("Remove");
                        var src = document.getElementById('<%= drpCompany.ClientID%>');
                        //iterate through each option of the listbox
                        for (var count = src.options.length - 1; count >= 0; count--) {
                            if (src.options[count].value == selectedval) {
                                document.getElementById('<%= drpCompany.ClientID%>').selectedValue = count;
                                //src.options[count].selected = "true";
        }
                            else {
                                src.options[count].disabled = true;
                            }

                            //    $('#<%= drpCompany.ClientID%> option[value="' + data1.d + '"]').attr("selected", true);
                        }
        }
                    if (data1.d == "1") {
                        $('#<%=lblCmpny.ClientID%>').text("Company removed from user");
                        var src = document.getElementById('<%= drpCompany.ClientID%>');
                        for (var count = src.options.length - 1; count >= 0; count--) {
                            src.options[count].disabled = false;
                        }
                        document.getElementById('<%= drpCompany.ClientID%>').selectedIndex = -1;
                        //$('#<%= drpCompany.ClientID%> option[value="-1"]').prop("selected", true);
                        $('#<%=btnCompany.ClientID%>').val("Add");
                    }
                    if (data1.d == "2") {
                        $('#<%=lblCmpny.ClientID%>').text("can't assign company to user");
                        document.getElementById('<%= drpCompany.ClientID%>').selectedIndex = -1;
                    }
                }
            });
            return false;
        }
        function GeneratePswd() {

            var randomstring = Math.floor((Math.random() * 100000) + 103);
            $('#<%=txtPassword.ClientID%>').val(randomstring);
        }

        function getCompanyVendorDetails() {
            $('#<%=lblCmpny.ClientID%>').text('');
            $(":button").val("Submit");
            //for (var i = 0; i < btns.length; i++)
            //    btns[i].val("Submit");
            var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '"}';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/CompanyDetails",
                success: function (data1) {


           
            $('input[type=checkbox]').each(function () {
                if (this.checked) {
                    $(this).prop('checked', false);
                }
            });
            $("#ContentPlaceHolder1_rptrVendors_radio2_0").attr('checked', true);
                    var txtSplit = data1.d[0].split("-");
                    var txtType = data1.d[1].split("-");
            for (var i = 0; i < txtSplit.length - 1; i++) {
                var new1 = "rd" + txtSplit[i];
                $('input[class="' + new1 + '"][value="' + txtType[i] + '"]').attr('checked', true);
                        $(".btn" + txtSplit[i]).val("Remove");

                        $(".cid" + txtSplit[i]).prop('checked', true);
            }

            var disabledItems = document.getElementsByClassName("vndorPanel");
            for (var j = 0; j < disabledItems.length; j++) {
                        if ((disabledItems[j]).id == "") {
                            var displayId = disabledItems[j].id;

                    $(disabledItems[j]).find('input[type=checkbox]').attr("disabled", 'disabled');
                            $(disabledItems[j]).find('input[type=button]').attr("disabled", 'disabled');
                    $(disabledItems[j]).find('input[type=radio]').attr("disabled", 'disabled');
                            $(disabledItems[j]).find('input[type=button]').css("cursor", "auto");
                            $(disabledItems[j]).find('input[type=button]').css("background-color", "#e6e6e6");
                            $(disabledItems[j]).find('input[type=button]').css("border", "#e6e6e6");

                        }
                    }
                }
            });
            var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '"}';
            var src = document.getElementById('<%= drpCompany.ClientID%>');
            //iterate through each option of the listbox
            for (var count = src.options.length - 1; count >= 0; count--) {
                src.options[count].disabled = false;
            }
            document.getElementById('<%= drpCompany.ClientID%>').selectedIndex = -1;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/GetCustomerDetails",
                success: function (data1) {
                    if (data1.d != "") {
                        $('#<%=btnCompany.ClientID%>').val("Remove");
                        var src = document.getElementById('<%= drpCompany.ClientID%>');
                        //iterate through each option of the listbox
                        for (var count = src.options.length - 1; count >= 0; count--) {
                            if (src.options[count].value == data1.d) {
                                src.options[count].selected = "true";
                            }
                            else {
                                src.options[count].disabled = true;
                            }
                            //    $('#<%= drpCompany.ClientID%> option[value="' + data1.d + '"]').attr("selected", true);
                        }
                    }
                    else
                        $('#<%=btnCompany.ClientID%>').val("Add");
                    //     $("#mySelect option[value='" + data1.d + "']").attr("selected", true);
                }
            });
            return false;
        }

        function SubmitVendorDetails(companyId, groupId, val) {
            var status;
            var asd = document.getElementsByClassName("btn" + companyId);
            if (asd[0].value == "Submit")
                status = "N";
            else
                status = "D";
            var adminvalue = document.getElementsByClassName("rd" + companyId);
            var adminStatus;
            for (var i = 0; i < adminvalue.length; i++) {
                if (adminvalue[i].checked) {
                    adminStatus = adminvalue[i].value;
                }
            }
            var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '" ,"vendorId":"' + companyId + '" ,"currentUserID":"' + $('#<%=hdnUserID.ClientID%>').val() + '" ,"GroupId":"' + groupId + '" ,"status":"' + status + '" ,"adminStatus":"' + adminStatus + '"}';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/SubmitVendor",
                success: function (data1) {
                    if (data1.d == "0" && status == "N") {
                        $('#<%=lblGroup.ClientID%>').text("Vendor assigned to user");
                        $(".cid" + companyId).attr('checked', 'true');
                        asd[0].value = "Remove";
                    }
                    else if (data1.d == 0 && status == "D") {
                        $('#<%=lblGroup.ClientID%>').text("Vendor removed from user");
                            asd[0].value = "Submit";
                            $(".cid" + companyId).prop('checked', false);
                        }
                }
            });
        }
        function UpdateConfigrations() {
            $('#<%=lblRole.ClientID%>').text('');
            var role, password, xmlperm;
            role = $('#<%=DpUserRole.ClientID%>').val();
            password = $('#<%=txtChangePassword.ClientID%>').val();
            xmlperm = $('#<%=rXmlPrmsn.ClientID%>').find(":checked").val();
            if ($('#<%=DpUserRole.ClientID%>').val() == "") {
                document.getElementById('<%=lblRole.ClientID %>').style.display = "inline-block";
                $('#<%=lblRole.ClientID%>').text("Select role");
            }
            else {
                if ($("#lblNewPswd").text() == "") {
                    var valueData = $('#<%=hdnData.ClientID%>').val();
                    var details = valueData.split(",");
                    if (role == details[0])
                        role = "";
                    if (xmlperm == details[1])
                        xmlperm = "";
                    var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '","role":"' + role + '" ,"password":"' + password + '" ,"xmlperminssion":"' + xmlperm + '"}';
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        data: param,
                        url: "Users.aspx/SubmitConfigration_Click",
                        success: function (data1) {
                            if (data1.d == "1")
                                document.getElementById('<%=lblSubmit.ClientID %>').style.display = "inline-block";
                            $('#<%=txtChangePassword.ClientID%>').val('');
                            $('#<%=lblSubmit.ClientID%>').text("Data updated successfully");
                        }
                    });
                }
            }
            return false;
        }
        function FuncGetDetails(userid, fname, lname, emailId) {
            document.getElementById("details").style.display = "block";
            $('#<%=hdnStoreUserID.ClientID%>').val(userid);
            if (fname != "")
                $('#<%=lblUsrName.ClientID%>').text(fname + " " + lname);
            else
                $('#<%=lblUsrName.ClientID%>').text(emailId);
            $(".tabs-menu a[href=#tab-1]").parent().addClass("current");
            $(".tabs-menu a[href=#tab-1]").parent().siblings().removeClass("current");
            var tab = $(".tabs-menu a[href=#tab-1]").attr("href");
            $(".tab-content").not(tab).css("display", "none");
            $(tab).fadeIn();
            getUserdetails();
            return false;
        }
        function getUserdetails() {
            debugger;
            var param = '{"UserID":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '"}';
            $('#<%=lblSubmit.ClientID%>').text('');
            $('#<%=lblRole.ClientID%>').text('');
            $('#<%=rXmlPrmsn.ClientID %> input[value="' + details[1] + '"]').prop('checked', true);
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/GetDetails",
                success: function (data1) {
                    var details = data1.d.split(",");
                    $('#<%= hdnData.ClientID%>').val(details);
                    $('#<%=txtChangePassword.ClientID %>').val("");
                    $('#<%= DpUserRole.ClientID%>').val(details[0]);
                    if (details[1] != "")
                        $('#<%=rXmlPrmsn.ClientID %> input[value="' + details[1] + '"]').prop('checked', true);
                    else
                        $('#<%=rXmlPrmsn.ClientID %> input[value="N"]').prop('checked', true);
                }
            });
        }
        function getBasicDetails() {
            var userdtls = $('.bscDetails');
            for (var i = 0; i < userdtls.length; i++) {
                $(userdtls[i]).text('');
            }
            var UserID = $('#<%=hdnStoreUserID.ClientID%>').val();
            var param = '{"UserID":"' + UserID + '"}';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/GetUserDetails",
                success: function (data) {
                    var tableData = $.parseJSON(data.d);
                    $.each(tableData, function () {
                        $('#<%=txtUserFName_reg.ClientID%>').val(this['FirstName']);
                        $('#<%=txtUserMName_reg.ClientID%>').val(this['MiddleName']);
                        $('#<%=txtUserLName_reg.ClientID%>').val(this['LastName']);
                        $('#<%=txtUserSecondaryEmail_reg.ClientID%>').val(this['Email']);
                        $('#<%=txtUserContact1_reg.ClientID%>').val(this['Phone1']);
                        $('#<%=txtUserContact2_reg.ClientID%>').val(this['Phone2']);
                        $('#<%=txtUserFax_reg.ClientID%>').val(this['Fax']);
                        $('#<%=txtUserAddress1_reg.ClientID%>').val(this['AddressString1']);
                        $('#<%=txtUserAddress2_reg.ClientID%>').val(this['AddressString2']);
                        $('#<%=txtUserCity_reg.ClientID%>').val(this['City']);
                        $('#<%=txtUserState_reg.ClientID%>').val(this['State']);
                        $('#<%=txtUserCountry_reg.ClientID%>').val(this['Country']);
                        $('#<%=txtUserZipCode_reg.ClientID%>').val(this['zipCode']);
                        $('#<%=txtUserCompany_reg.ClientID%>').val(this['Company']);
                        $('#<%=txtUserDept_reg.ClientID%>').val(this['Department']);
                        $('#<%=txtUserMxSmltnsLgns_reg.ClientID%>').val(this['MaxSimultaneousLogins']);
                        if (this['ExpirationDate'] == null)
                            $('#<%=txtUserExpDate_reg.ClientID%>').val("");
                        else
                            $('#<%=txtUserExpDate_reg.ClientID%>').val(ChangeDateFormat(Date.parse(this['ExpirationDate'])));
                        $('#<%=txtUserCmnt_reg.ClientID%>').val(this['Comment']);
                    });
                },
                failure: function (data) {
                }
            });
            return false;
        }
        function ChangeDateFormat(jsondate) {
            if (jsondate != undefined) {
                jsondate = jsondate.replace("/Date(", "").replace(")/", "");
                if (jsondate.indexOf("+") > 0) {
                    jsondate = jsondate.substring(0, jsondate.indexOf("+"));
                }
                else if (jsondate.indexOf("-") > 0) {
                    jsondate = jsondate.substring(0, jsondate.indexOf("-"));
                }
                var date = new Date(parseInt(jsondate, 10));
                //   return date.getFullYear() + "-" + month + "-" + currentDate;
                return ("0" + (date.getMonth() + 1)).slice(-2) + "-" +
     ("0" + date.getDate()).slice(-2) + "-" + date.getFullYear() + "  " + date.getHours() + ":" +
     date.getMinutes() + ":" + date.getSeconds();
            } else
                return "";
        }
        function SaveUserBasicInfo() {
            var param = '{"userid":"' + $('#<%=hdnStoreUserID.ClientID%>').val() + '","firstName":"' + $('#<%=txtUserFName_reg.ClientID%>').val() + '" ,"lastName":"' + $('#<%=txtUserLName_reg.ClientID%>').val() + '" ,"MiddleName":"' + $('#<%=txtUserMName_reg.ClientID%>').val() + '","SecEmail":"' + $('#<%=txtUserSecondaryEmail_reg.ClientID%>').val() + '" ,"CntNo1":"' + $('#<%=txtUserCity_reg.ClientID%>').val() + '" ,"CntNo2":"' + $('#<%=txtUserCity_reg.ClientID%>').val() + '","faxNo":"' + $('#<%=txtUserFax_reg.ClientID%>').val() + '" ,"AdrsStrg1":"' + $('#<%=txtUserAddress1_reg.ClientID%>').val() + '" ,"AdrsString2":"' + $('#<%=txtUserAddress1_reg.ClientID%>').val() + '","city":"' + $('#<%=txtUserAddress1_reg.ClientID%>').val() + '" ,"state":"' + $('#<%=txtUserSecondaryEmail_reg.ClientID%>').val() + '" ,"country":"' + $('#<%=txtUserCity_reg.ClientID%>').val() + '","zipcode":"' + $('#<%=txtUserZipCode_reg.ClientID%>').val() + '" ,"cmpnyName":"' + $('#<%=txtUserCompany_reg.ClientID%>').val() + '" ,"DeptName":"' + $('#<%=txtUserDept_reg.ClientID%>').val() + '","MaxSmltLogin":"' + $('#<%=txtUserMxSmltnsLgns_reg.ClientID%>').val() + '" ,"ExpDate":"' + $('#<%=txtUserExpDate_reg.ClientID%>').val() + '" ,"Cmnt":"' + $('#<%=txtUserCmnt_reg.ClientID%>').val() + '"}';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                data: param,
                url: "Users.aspx/SaveUserDetails",
                success: function (data) {
                    $('#<%=lblSbtDtlStatus.ClientID%>').text(data.d);
                }
            });
                return false;
            }
            function onBlurFunction() {
                var psd = document.getElementById('<%=txtChangePassword.ClientID%>').value;
             if (psd != "") {
                 re = /^[0-9a-zA-Z.,@&(){}\[\]:;#$*!^%|+-='`~"<>?_\\\\]+$/;
                 if (!re.test(psd)) {
                     document.getElementById("lblNewPswd").innerHTML = "8 to 32 chars. Atleast one uppercase and one lowercase letter, and one digit required";
                     document.getElementById("lblNewPswd").style.color = "red";
                     return false;
                 }
                 if (psd.length < 8 || psd.length > 32) {
                     document.getElementById("lblNewPswd").innerHTML = "8 to 32 chars. Atleast one uppercase and one lowercase letter, and one digit required";
                     document.getElementById("lblNewPswd").style.color = "red";
                     return false;
                 }
                 re = /[0-9]/;
                 if (!re.test(psd)) {
                     document.getElementById("lblNewPswd").innerHTML = "8 to 32 chars. Atleast one uppercase and one lowercase letter, and one digit required";
                     document.getElementById("lblNewPswd").style.color = "red";
                     return false;
                 }
                 re = /[a-z]/;
                 if (!re.test(psd)) {
                     document.getElementById("lblNewPswd").innerHTML = "8 to 32 chars. Atleast one uppercase and one lowercase letter, and one digit required";
                     document.getElementById("lblNewPswd").style.color = "red";
                     return false;
                 }
                 re = /[A-Z]/;
                 if (!re.test(psd)) {
                     document.getElementById("lblNewPswd").innerHTML = "8 to 32 chars. Atleast one uppercase and one lowercase letter, and one digit required";
                     document.getElementById("lblNewPswd").style.color = "red";
                     return false;
                 }
                 else {
                     document.getElementById("lblNewPswd").innerHTML = "";
                     return true;
                 }
             }
             else {
                 document.getElementById("lblNewPswd").innerHTML = "";
             }
         }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .tabs-menu li
        {
            list-style: none;
            height: 30px;
            line-height: 30px;
            float: left;
            margin-right: 10px;
            background-color: #428bca;
            border-top: 2px solid Navy;
            border-right: 2px solid Navy;
            border-left: 2px solid Navy;
        }
            .tabs-menu li.current
            {
                position: relative;
                background-color: #fff;
                border-bottom: 1px solid #fff;
                z-index: 5;
            }

            .tabs-menu li a
            {
                padding: 10px;
                text-transform: uppercase;
                color: #fff;
                text-decoration: none;
            }

        .tabs-menu .current a
        {
            color: #2e7da3;
        }

        .tab
        {
            /*border: 1px solid #d4d4d1;*/
            background-color: #fff;
            float: left;
            margin-bottom: 20px;
            width: 100%;
        }

        .tab-content
        {
            /*width: 593px;*/
            padding: 20px;
            display: none;
        }

        #tab-1
        {
            display: block;
        }

            #tab-1 div
            {
                margin-bottom: 10px;
                flex-align: baseline;
            }
    </style>

    <asp:UpdatePanel ID="upCreateUser" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
            </script>
            <div class="span12" style="margin-left: 0px !important">
                <div class="span6" style="margin-left: 0px !important">
                    <div class="span12" style="margin-left: 0px;">
                        <div class="control-group createUsr">
                        <div class="controls">

                                <div class="heading">
                                    Create User
                                </div>
                                <div class="span12" style="margin-left: 0px; border:2px solid navy">
                                    <div>
                                        <div class="span6">
                                            <div class="span3">
                                                First Name :  
                            </div>
                                            <div class="span9">


                                           
                                                <input type="text" id="txtFname" runat="server" enableviewstate="false" validationgroup="valgroupNewsLetterCreateUser" placeholder="Enter First Name" autocomplete="off" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="valgroupNewsLetterCreateUser" ControlToValidate="txtFname" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[a-zA-Z1-9_']{1,30}$" ControlToValidate="txtFname" ValidationGroup="valgroupNewsLetterCreateUser" ForeColor="#FF3300">InValid First Name</asp:RegularExpressionValidator>
                                            </div>

                                        </div>
                                        <div class="span6">
                                            <div class="span3">
                                                Last Name :
                                            </div>
                                            <div class="span9">
                                                <input type="text" id="txtLastName" runat="server" validationgroup="valgroupNewsLetterCreateUser" placeholder="Enter Last Name" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="valgroupNewsLetterCreateUser" ControlToValidate="txtLastName" ForeColor="#FF3300">*</asp:RequiredFieldValidator>

                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[a-zA-z']{1,30}$" ControlToValidate="txtLastName" ValidationGroup="valgroupNewsLetterCreateUser" ForeColor="#FF3300">InValid Last Name</asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="span6">
                                            <div class="span3">
                                                Email ID : 
                                            </div>
                                            <div class="span9">
                                                <input type="text" id="txtUserName" runat="server" enableviewstate="false" validationgroup="valgroupNewsLetterCreateUser" placeholder="Enter Email-ID" autocomplete="off" />
                                            <asp:RequiredFieldValidator ID="vfieldUserName" runat="server" ValidationGroup="valgroupNewsLetterCreateUser" ControlToValidate="txtUserName" ErrorMessage="Please enter email id" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="span3">
                                                Password :    
                                            </div>
                                            <div class="span9">
                                                <input type="text" id="txtPassword" runat="server" style="width: 85%" validationgroup="valgroupNewsLetterCreateUser" placeholder="Enter Password" autocomplete="off" width="76%" />
                                                <linkbutton id="gnrtPswd" font-bold="false" c font-size="Large" onclick="GeneratePswd();" />
                                                A</linkbutton>
                                            <asp:RequiredFieldValidator ID="vfiledtxtPwd" runat="server" ValidationGroup="valgroupNewsLetterCreateUser" ControlToValidate="txtPassword" ForeColor="#FF3300" ErrorMessage="Please Enter Password" SetFocusOnError="True">*</asp:RequiredFieldValidator>



                                            <asp:RegularExpressionValidator ID="vfieldEmail" ValidationGroup="valgroupNewsLetterCreateUser"
                                                runat="server" ControlToValidate="txtUserName" ToolTip="Enter Valid Email Id"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="#FF3300">Please enter valid email id</asp:RegularExpressionValidator>
                                            </div>
                                        </div>

                                    </div>


                                    <div>
                                        <div class="span6">

                                            <div class="span3">
                                                User Type :
                                            </div>
                                            <div class="span9">
                                                <asp:DropDownList ID="drpType" runat="server" ValidationGroup="valgroupNewsLetterCreateUser" Width="93%">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Text="Test User" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Normal User" Value="0"></asp:ListItem>
                                            </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="span6">
                                            <div class="span4">
                                          <asp:Label ID="lblStaff" Text="Free Staff : " runat="server" ></asp:Label>
                                            <input type="checkbox" id="freestaff" value ="1" runat="server" />
                                            </div>

                                           
                                            <div class="span8">
                                                <asp:Button ID="btnCreateUsr" runat="server" Text="Create User" ToolTip="Create User" CssClass="btnClass" ValidationGroup="valgroupNewsLetterCreateUser" OnClick="btnCreateUsr_Click" />
                                            <asp:ValidationSummary ID="vsSign" runat="server" ValidationGroup="valgroupNewsLetterCreateUser"
                                                ShowMessageBox="false" ShowSummary="false" />
                                          
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="span6">
                                             <asp:RequiredFieldValidator ID="rfield" ControlToValidate="drpType" ForeColor="Red" InitialValue="-1" runat="server" ErrorMessage="select user type" ValidationGroup="valgroupNewsLetterCreateUser"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="span6">
                                            <asp:Label ID="lblMsgCreateUser" runat="server" Text=""></asp:Label></td>
                                        </div>
                                    </div>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="span12  allUsers" style="margin-left: 0px !important">
                        <div class="heading">
                            All Users
                        </div>
                        <div class="pnl">
                        <table>

                            <tr>
                                <td>
                                    <div id="loading" style="vertical-align:central">
                                        <img src="images/loadingImage.png" />
                                    </div>
                                    <asp:Repeater ID="rptrUsers" runat="server">
                                        <HeaderTemplate>
                                                <table id="tblUsers" class="hor-scroll" style="border-spacing: 0; display: none">
                                                <thead>
                                                    <tr class="tblHdr">
                                                        <th>User Id
                                                        </th>
                                                        <th>User Name
                                                        </th>
                                                        <%--<th>Last Name
                                                                </th>--%>
                                                        <th>Email Id
                                                        </th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="padding: 0px 10px 0px 2px">
                                                    <%#Eval("UserID")%>
                                                </td>
                                                <td style="width: 170px">
                                                    <%#Eval("FirstName")%>   <%#Eval("LastName")%>
                                                </td>
                                                <%--<td>
                                                            <%#Eval("LastName")%>
                                                        </td>--%>
                                                <td>
                                                    <%#Eval("Name")%>
                                                </td>

                                                <td>

                                                    <%-- <asp:ImageButton ID="btnsendURL" CommandArgument='<%#Eval("FirstName") +","+ Eval("LastName") +","+ Eval("Name")%>' value='<%#Eval("UserID")%>' OnClick="btnSendAutoURL_Click" ToolTip="Send Auto URL" src="images/send_autUrl.png" runat="server" />--%>
                                                        <input type="image" id="getDetails" src="images/getDetails.png" onclick='<%# string.Format("return FuncGetDetails(\"{0}\",\"{1}\",\"{2}\",\"{3}\")" ,Eval("UserID"), Eval("FirstName"), Eval("LastName"), Eval("Name")) %>' />
                                                    <%-- <asp:ImageButton ID="btnImgPrmsn" value='<%#Eval("UserID")%>' OnClick="btnImagePermission_Click" ToolTip="Image Permissions" src="images/imagePermissions.png" runat="server" />--%>
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody> </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
                <div class="span6" id="details" style="display: none; margin-left: 20px">
                    <div class="span12">
                <div id="userName mainDiv" class="span12">
                            <div class="" style="border: 2px solid navy; margin: 2px">
                        <img class="user" src="images/userImage.jpg" />
                        <asp:Label ID="lblUsrName" CssClass="user userName" runat="server" Text=""></asp:Label>
                        <%--  <asp:Label ID="Label1" CssClass="user lstlogin"  runat="server" Text="Last Login time"></asp:Label>--%>
                    </div>
                        </div>
                </div>

                    <div class="span12 pnl" style="margin: 2px !important;">
                        <%-- <div class="panel panel-primary">
                            <div class="panel-heading">--%>
                        <div id="tabs-container">
                            <ul class="tabs-menu" style="margin: 0px">
                                <li class="current"><a href="#tab-1" onclick="getUserdetails()">Configration</a></li>
                                <li><a href="#tab-2" onclick="getCompanyVendorDetails()">Company Details</a></li>
                                <li><a href="#tab-3" onclick="getBasicDetails()">Basic Details</a></li>

                            </ul>
                            </div>
                        <div class="tab" style="border-top: 2px solid navy">
                            <div id="tab-1" class="tab-content">
                                <div class="span12">
                                    <div class="span4" style="margin-left: 15px">
                                        Role :
                        </div>
                                    <div class="span6">
                                        <asp:DropDownList ID="DpUserRole" runat="server" Width="188px">
                                        </asp:DropDownList>


                        </div>
                                    <div class="span4"></div>
                                    <div class="span6">
                                        <asp:Label runat="server" ID="lblRole" ForeColor="Red"></asp:Label>
                    </div>

                                    <div class="span4">
                                Change Password
                            </div>
                                    <div class="span6">
                                        <input type="password" id="txtChangePassword" runat="server" style="width: 60%" validationgroup="valgroupNewsLetterResetPswd" onblur="onBlurFunction()" class="chgpwd" placeholder="New Password" />
                                                     

                        </div>
                                    <div class="span10">
                                        <div id="lblNewPswd" class="lblNewPwd"></div>
                </div>



                                    <div class="span4">
                                        <label id="lblXmlPrmsn" runat="server">XML View Permission </label>

                                  
                                        <%-- </td>
                                            <td style="width: 40%">--%>
                        </div>
                                    <div class="span4">
                                        <asp:RadioButtonList ID="rXmlPrmsn" runat="server" RepeatDirection="Horizontal" CssClass="radio control-label" Height="33px" Style="margin-left: 10px">
                                            <asp:ListItem Value="Y">yes</asp:ListItem>
                                            <asp:ListItem Selected="true" Value="N">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <%--  </td>
                                            <td>--%>
                    </div>
                                    <div class="span4">
                                        <input type="Submit" id="btnSubmit" class="btnClass" style="margin: 3px" onclick="return UpdateConfigrations()" value="Save" />

                                        <asp:ValidationSummary ID="valgroupNewsLetterResetPswd" runat="server" ValidationGroup="valgroupNewsSubmit"
                                            ShowMessageBox="false" ShowSummary="false" />

                        </div>
                                    <div class="span4">

                                        <asp:Label ID="lblSubmit" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                            Font-Bold="true" Style="text-align: left; display: none" ForeColor="red"></asp:Label>
                        </div>

                            </div>

                        </div>
                            <div id="tab-2" class="tab-content">
                          
                                <div class="span12">
                                    <div class="span7  allUsers">
                                        <div class="">
                                            <div class="">
                            Vendors
                        </div>
                    </div>
                                        <div class="">
                        <table>

                            <tr>
                                <td>
                                    <asp:Repeater ID="rptrVendors" runat="server">
                                        <HeaderTemplate>
                                                                <table id="Table1">
                                                <thead>
                                                    <tr class="tblHdr">

                                                        <th></th>

                                                        <%--<th>Last Name
                                                                </th>--%>
                                                        <th>Vendor Name
                                                        </th>
                                                        <th>Is Admin
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                                <tr class="vndorPanel" id='<%#Eval("GroupId")%>'>
                                                <td style="padding: 0px 10px 0px 2px">
                                                                        <input type="checkbox" id="CheckAll" name="chkGroup" class='<%#"cid" + Eval("CompanyId")%>' value='<%#Eval("CompanyId")%>' runat="server" disabled="disabled" />

                                                    <td>
                                                        <%#Eval("CompanyName")%>
                                                    </td>
                                                    <td>
                                                        <input type="radio" name="radio" class='<%# "rd" + Eval("CompanyId")%>' id="radio1" value="1" runat="server" />
                                                        Yes
                                                    <input type="radio" name="radio" class='<%# "rd" + Eval("CompanyId")%>' id="radio2" value="0" runat="server" checked="true" />No
                                                    
                                                    </td>
                                                    <td>
                                                                            <input type="button" id="btnAdmin" class='<%# "btn" + Eval("CompanyId")%>' value="Submit" onclick='<%# string.Format("return SubmitVendorDetails(\"{0}\",\"{1}\")",Eval("CompanyId"),Eval("GroupId")) %>' style="margin: 2px" />
                                                    </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody> </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblGroup" runat="server" ForeColor="#FF3300"></asp:Label>
                    </div>
                </div>


                                    <div class="span5">
                                        <div class="">
                                            <div class="">
                            Customer
                        </div>
                    </div>
                                        <div class="">
                        <table>
                            <tr>
                                <td>

                                    <asp:TextBox ID="companySearch" runat="server" Style="width: 165px; margin-bottom: 5px" /><br />
                                    <asp:ListBox ID="drpCompany" runat="server" Style="height: 250px; width: 178px;"></asp:ListBox>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                                        <asp:Button ID="btnCompany" runat="server" Text="Add" CssClass="btnClass" Style="margin: 3px" OnClientClick=" return btnSaveCompanyDetails()" />
                                    <br />
                                    <asp:Label ID="lblCmpny" runat="server" ForeColor="#FF3300"></asp:Label>
                                </td>
                            </tr>
                        </table>


                    </div>
                </div>
                                </div>
                            </div>
                            <div id="tab-3" class="tab-content">
                                <div class="span11 bscInf ">
                                    <div class="">
                                        <div class="">
                                            Basic Information 
                                           <%--  <asp:ImageButton ID="shwDetail" src="images/colapseBtn.png" OnClientClick="ShowHide()" Style="margin-left: 65%" runat="server" />--%>
                        </div>
                    </div>
                                    <div class="pnl1 ">
                                        <table id="tblBasic">
                                            <tr>
                                                <td style="width: 30%;">First Name
                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserFName_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldFName" runat="server" ValidationExpression="^[a-zA-Z1-9_ ']{1,30}$" ControlToValidate="txtUserFName_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300">InValid First Name</asp:RegularExpressionValidator>
                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Middle Name
                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserMName_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldMName" runat="server" ValidationExpression="^[a-zA-z1-9_ ']{1,30}$" ControlToValidate="txtUserMName_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300">InValid Middle Name</asp:RegularExpressionValidator>


                                </td>
                                            </tr>
                            <tr>
                                                <td style="width: 30%;">Last Name
                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserLName_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldLName" runat="server" ValidationExpression="^[a-zA-Z1-9_ ']{1,30}$" ControlToValidate="txtUserLName_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300">InValid Last Name</asp:RegularExpressionValidator>

                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Secondary Email
                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserSecondaryEmail_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldSEmail" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtUserSecondaryEmail_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Email Address</asp:RegularExpressionValidator>
                                </td>
                                
                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Contact No1

                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserContact1_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserCNo1" runat="server" ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$" ControlToValidate="txtUserContact1_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Contact No</asp:RegularExpressionValidator>
                                </td>
                                            </tr>
                                            <tr>

                                                <td style="width: 30%;">Contact No2
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserContact2_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserCNo2" runat="server" ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$" ControlToValidate="txtUserContact2_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid Contact No</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Fax No
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserFax_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldUserFaxNo" runat="server" ValidationExpression="^[123456789]\d{5}$" ControlToValidate="txtUserFax_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid Fax No</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Address String1
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserAddress1_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldAddString1" runat="server" Type="String" ValidationExpression="^[a-zA-z0-9-'/\s]{1,50}$" ControlToValidate="txtUserAddress1_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid Address</asp:RegularExpressionValidator>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Address String-2
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserAddress2_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldAddString2" runat="server" Type="String" ValidationExpression="^[a-zA-z0-9-'/\s]{1,50}$" ControlToValidate="txtUserAddress2_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid Address</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">City
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserCity_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldUserCity" runat="server" ValidationExpression="^[a-zA-Z]{1,30}$" ControlToValidate="txtUserCity_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid City</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">State
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserState_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserState" runat="server" ValidationExpression="^[a-zA-Z ]{1,30}$" ControlToValidate="txtUserState_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300"> InValid State</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Country
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserCountry_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="vfieldUserCntry" runat="server" ValidationExpression="^[a-zA-z ]{1,30}$" ControlToValidate="txtUserCountry_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300"> InValid Country</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Zip Code
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserZipCode_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldUserZipCode" runat="server" ValidationExpression="^(\d{5}-\d{4}|\d{5}|\d{9})$|^([a-zA-Z]\d[a-zA-Z] \d[a-zA-Z]\d)$" ControlToValidate="txtUserZipCode_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid ZipCode</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Company Name
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserCompany_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfiledCompany" runat="server" ValidationExpression="^[a-zA-z1-9_ ]{1,30}$" ControlToValidate="txtUserCompany_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Company Name</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Department Name
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserDept_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldUserDeptName" runat="server" ValidationExpression="^[a-zA-z1-9_ ]*$" ControlToValidate="txtUserDept_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Department Name</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Max Simultaneous Logins 
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserMxSmltnsLgns_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldMxLgns" runat="server" ValidationExpression="[0-9]{1,30}$" ControlToValidate="txtUserMxSmltnsLgns_reg" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300">InValid Value</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Expiration Date
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserExpDate_reg" runat="server" placeHolder="MM/DD/YYYY" CssClass="bscDetails" ValidationGroup="valbasicUserInformation" onfocus="showCalendarControl(this);"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldExpDate" runat="server" ControlToValidate="txtUserExpDate_reg" ValidationExpression="^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$" ValidationGroup="valbasicUserInformation" ForeColor="#FF3300">Invalid Date</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 30%;">Comment 
                                                </td>
                                                <td style="width: 70%;">
                                                    <asp:TextBox ID="txtUserCmnt_reg" runat="server" CssClass="bscDetails"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="vfieldCmnt" runat="server" ValidationExpression="^[a-zA-Z0-9''-'\s]{1,100}$" ControlToValidate="txtUserCmnt_reg" ValidationGroup="valbasicUserInformation" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300">InValid Comment</asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <input type="submit" id="btnSaveUserInfo" validationgroup="valbasicUserInformation" value="Save" class="btnClass" onclick="return SaveUserBasicInfo()" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblSbtDtlStatus" runat="server" ForeColor="#FF3300" CssClass="bscDetails" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:ValidationSummary ID="vfieldSmry" runat="server" ValidationGroup="valbasicUserInformation"
                            ShowMessageBox="false" ShowSummary="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnStoreUserID" runat="server" Value="" />
            <asp:HiddenField ID="hdnVendorID" runat="server" Value="" />
            <asp:HiddenField ID="hdnType" runat="server" Value="" />
            <asp:HiddenField ID="hdnCompanyId" runat="server" />
            <asp:HiddenField ID="hdnData" runat="server" />
            <asp:HiddenField ID="hdnUserID" runat="server" Value="" />

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

