<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ExcelURLDownLoad.aspx.cs" Inherits="Administration.ExcelURLDownLoad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .backg {
            background-color: lightblue;
        }

        .bgcolor {
            /*background: linear-gradient(to bottom, rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%);*/
            background: #36648B !important;
            color: White;
        }

        .hover {
            /* background: linear-gradient(to bottom, rgba(225,255,255,1) 0%,rgba(225,255,255,1) 7%,rgba(225,255,255,1) 12%,rgba(253,255,255,1) 12%,rgba(230,248,253,1) 30%,rgba(200,238,251,1) 54%,rgba(190,228,248,1) 75%,rgba(177,216,245,1) 100%); /* W3C */
            background-color: #3299CC;
            color: #fff;
            cursor: pointer;
        }


        .login-mid {
            background: linear-gradient(to bottom, rgba(255,255,255,1) 0%,rgba(229,229,229,1) 100%);
        }

        input[type="radio"], input[type="checkbox"] {
            margin: 0;
        }

        .unselected {
            /* background-image: url('Images/AppSelection.png'); */
            background-repeat: no-repeat;
            background-position: bottom;
            margin: 0px;
            padding: 0px;
            height: 14px;
            width: 14px;
            overflow: hidden;
        }

        .selected {
            background-repeat: no-repeat;
            background-position: top;
            margin: 0px;
            padding: 0px;
            height: 18px;
            width: 18px;
            overflow: hidden;
        }

        ul li {
            list-style: none !important;
            margin-bottom: 4px;
        }

        ul {
            margin-left: 1px !important;
        }

        .allGrid table tr {
            border-top: none;
        }

            .allGrid table tr td {
                padding: 5px;
            }

        .allGrid table tbody tr td {
            padding: 5px;
        }
        /*#tblCategory tr
        {
            margin-bottom: 4px !important;
            padding-bottom: 4px !important;
        }*/
    </style>

    <script type="text/javascript">
        function BindFunctions() {

            var sampleData = new Array();
            $("input[type='checkbox']").change(function () {
                if (this.className == "CheckAll") {
                    if (this.checked) {
                        var vdata = $(this).val();
                        sampleData.push(vdata);
                        var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                        txtMailId.value = sampleData;
                        var data = vdata.split(',')[0].trim();
                        document.getElementById(data).className = "backg";
                    }
                    else {
                        var vdata = $(this).val();
                        var ipop = sampleData.indexOf(vdata);
                        var newData1 = sampleData.slice(0, ipop);
                        var ipopNew = ipop + 1;
                        var newData2 = sampleData.slice(ipopNew, sampleData.length);
                        sampleData = newData1.concat(newData2);
                        var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                        txtMailId.value = sampleData;
                        $(this).parent().parent().removeClass("backg");
                    }
                }


            });
            $('#ChkAllCheck').click(function () {
                if ($(this).is(":checked")) {
                    $('input[type=checkbox]').each(function () {
                        if (this.className == "CheckAll") {
                            if (!this.checked) {

                                $(this).prop('checked', true);
                                var vdata = $(this).val();
                                sampleData.push(vdata);
                            }
                        }
                    });
                    var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                    txtMailId.value = sampleData;
                }
                else {
                    $('input[type=checkbox]').each(function () {
                        if (this.className == "CheckAll") {
                            $(this).prop('checked', false);
                            var vdata = $(this).val();
                            var ipop = sampleData.indexOf(vdata);
                            var newData1 = sampleData.slice(0, ipop);
                            var ipopNew = ipop + 1;
                            var newData2 = sampleData.slice(ipopNew, sampleData.length);
                            sampleData = newData1.concat(newData2);

                        }
                    });
                    var txtMailId = document.getElementById('<%=hdnMailId.ClientID %>');
                    txtMailId.value = sampleData;
                }
            });



            function pageChanged() {
                $('input[type=checkbox]').each(function () {
                    if (this.className == "CheckAll") {
                        if (this.checked) {
                        }
                        else {
                            $('#ChkAllCheck').prop("checked", false);
                            return;
                        }
                        $('#ChkAllCheck').prop("checked", true);
                    }
                });

            }

            $('#tblAutoURLNew').dataTable({
                "sPaginationType": "simple",
                "iDisplayLength": 25,
                "aaSorting": [],
                "initComplete": function (settings, json) {
                    $("#tblAutoURLNew").show();
                },
                "fnDrawCallback": function () {
                    $(".paginate_enabled_previous").bind('click', pageChanged);
                    $(".paginate_enabled_next").bind('click', pageChanged);
                }

            });

            $('#tblAutoURLExcelVersion').dataTable({
                "pagingType": "simple",
                "aaSorting": [],
                "iDisplayLength": 25,
                "initComplete": function (settings, json) {
                    $("#tblAutoURLExcelVersion").show();
                },
                "fnDrawCallback": function () {
                    $(".paginate_enabled_previous").bind('click', pageChanged);
                    $(".paginate_enabled_next").bind('click', pageChanged);
                }

            });

            $('#tblAutoURLHistory').dataTable({
                "pagingType": "simple",
                "iDisplayLength": 25,
                "aaSorting": [],
                "initComplete": function (settings, json) {
                    $("#tblAutoURLHistory").show();
                },
                "fnDrawCallback": function () {
                    $(".paginate_enabled_previous").bind('click', pageChanged);
                    $(".paginate_enabled_next").bind('click', pageChanged);
                }

            });





        }



        function CheckboxSelection() {
            debugger;
            var txtPack = document.getElementById('<%= hdnSelectedPackage.ClientID %>');
            var txtMailId = document.getElementById('<%=hdnMailId.ClientID%>');


            if (txtMailId.value == "" || txtPack.value == "") {
                if (txtMailId.value == "" && txtPack.value == "") {
                    document.getElementById('<%=lblMessage.ClientID %>').innerHTML = "select package and aleast one user to send AutoURL.";
                }
                else if (txtPack.value == "") {
                    document.getElementById('<%=lblMessage.ClientID %>').innerHTML = "select package to send AutoURL.";
                }
                else {
                    document.getElementById('<%=lblMessage.ClientID %>').innerHTML = "select aleast one user to send AutoURL.";
                }
            document.getElementById('<%=lblMessage.ClientID %>').style.display = "block";
                return false;
            }
        }



        function downLoadZip(packageurl) {
            document.getElementById('<%= hdnPackageDownload.ClientID %>').value = packageurl;
            $('#<%= btnPackageDownLoad.ClientID %>').click();
        }

        function onCheckPack(source, id) {
            debugger;
            document.getElementById('<%= hdnSelectedPackage.ClientID %>').value = "";
            var isChecked = source.checked;

            $("#RepeaterTable input[id*='cbSelect']").each(function (index) {
                $(this).attr('checked', false);
                $(this).parent().parent().removeClass("backg");
            });

            if (isChecked) {
                document.getElementById('<%= hdnSelectedPackage.ClientID %>').value = id;

                source.checked = isChecked;
                var idx = 'trl' + id.toString();
                document.getElementById(idx).className = "backg";
            }
        }




        function setExipry(id) {
            var selectedExpiry = document.getElementById("<%=ddlExpiryGuid.ClientID%>");
            var selectedExpiryValue = selectedExpiry.options[selectedExpiry.selectedIndex].value;
            if (selectedExpiryValue == "0") {
                alert("Please select expiry");
                return false;
            }

            document.getElementById('<%= hdnSetExpiryId.ClientID %>').value = id;

            $('#<%= btnSetExpiry.ClientID %>').click();
        }
        function deleteExcelPack(id) {
            //alert(id);//hdnDeletePackage
            document.getElementById('<%= hdnDeletePackage.ClientID %>').value = id;
            $('#<%= btnDeleteExcelPackage.ClientID %>').click();

        }

        function deleteAutoURL(id) {
            //hdnDeleteAutoURL
            document.getElementById('<%= hdnDeleteAutoURL.ClientID %>').value = id;
            $('#<%= btnDeleteAutoURL.ClientID %>').click();
        }










    </script>

    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <%-- <Services>
            <asp:ServiceReference Path="~/openf2.asmx" />
        </Services>--%>
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpnlAutoURL" runat="server">

        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
            </script>

            <div class="row-fluid">
                <div class="span12">
                    <div class="row-fluid">
                        <div class="span6">
                            <div class="section-outer">
                                <div class="section-title">
                                    <table>
                                        <tr>
                                            <td style="width: 40px; ">&nbsp;
                                            </td>
                                            <td>
                                                <div style="float: left; font-family: Arial; font-size:large; font-weight: bold;margin-top:10px;margin-bottom:10px;">
                                                    For OPS Use
                                                </div>
                                            </td>

                                        </tr>
                                    </table>

                                </div>
                                <div class="section-detail">
                                </div>
                            </div>
                        </div>
                        <div class="span6"></div>
                    </div>
                </div>
            </div>


            <table id="tblSelect" runat="server" border="0" style="height: 10px; vertical-align: middle; width: 90%"
                align="center">
                <tr>

                    <td style="border-top: 5px solid Navy; border-bottom: 2px solid Navy;" colspan="2">
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 20%;">
                                    <img runat="server" id="imgCompanyLogo" alt="" src="" height="50" width="170" />
                                </td>
                                <td align="right" style="width: 80%" class="AutoUrl_CompanyTitle">
                                    <asp:Label ID="lblCompanyTitle" runat="server" Text="Excel Auto Url Panel" Font-Bold="True" Font-Size="18px" ForeColor="#000066"></asp:Label>
                                    &nbsp;&nbsp;
                                            <%--<a href="ExcelPackage.aspx">Excel Version Mappings</a>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 55%;" align="left" valign="top"></td>


                </tr>

            </table>

            <table style="vertical-align: middle; width: 90%;" align="center">
                <tr>
                    <td align="left" valign="top" style="width: 50%;">
                        <div style="border: 2px solid Navy;" align="left" valign="top">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Available Packages
                                </div>
                            </div>
                            <div class="panel-success allGrid">
                                <table style="width: 100%" align="center" class="allUsers" id="RepeaterTable">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoRecordsExcelVersionInfo" runat="server" ForeColor="Red" Text="No Records found.."></asp:Label>
                                            <asp:HiddenField ID="hdnPackageDownload" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnSelectedPackage" runat="server" Value="" />

                                            <div style="display: none">
                                                <asp:Button ID="btnPackageDownLoad" runat="server" OnClick="btnPackageDownLoad_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptExcelVersionInfo" runat="server" OnItemDataBound="rptExcelVersionInfo_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table id="tblAutoURLExcelVersion" style="border-spacing: 0; display: none; width: 100%">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th></th>
                                                                <th>Version Details
                                                                </th>

                                                                <th>Last UpdatedBy
                                                                </th>
                                                                <th>Click to Download
                                                                </th>
                                                                <th></th>
                                                            </tr>


                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr id='trl<%#Eval("PackageId")%>'>
                                                        <td style="width: 5%">

                                                            <asp:CheckBox ID="cbSelect" runat="server" />
                                                        </td>
                                                        <td style="margin-left: 1px; width: 57%;">

                                                            <%#Eval("CompatibleVersionInfo")%>
                                                        </td>

                                                        <td style="width: 25%">
                                                            <%#Eval("LastUpdatedByName")%>
                                                        </td>
                                                        <td style="width: 8%">
                                                            <img src='images/download.png' alt='' onclick='downLoadZip(<%#Eval("PackageId")%>);' style="cursor:pointer;" />

                                                            <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%#Eval("PackageId")%>' />

                                                        </td>
                                                        <td style="width: 8%">
                                                            <img src="images/delete-icon.png" alt="" onclick="deleteExcelPack(<%# Eval("PackageId").ToString() %>);" style="cursor:pointer;" />
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
                            <asp:HiddenField ID="hdnDeletePackage" runat="server" />
                            <asp:HiddenField ID="hdnDeleteAutoURL" runat="server" />
                        </div>
                    </td>
                    <td style="width: 50%;" valign="top">
                        <div style="border: 2px solid Navy;" align="left" valign="top">
                            <div class="panel panel-primary">
                                <div class="panel-heading" style="height: 30px;">
                                    Select User and Send AUTO URL
                                             <div style="float: right;">
                                                 Set Expiry -  
                                                <asp:DropDownList ID="ddlSendExpiry" runat="server" Width="170px" AutoPostBack="true">
                                                    <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                                    <asp:ListItem Value="30" Text="30 minutes"></asp:ListItem>
                                                    <asp:ListItem Value="60" Text="1 hour"></asp:ListItem>
                                                    <asp:ListItem Value="120" Text="2 hours"></asp:ListItem>
                                                    <asp:ListItem Value="180" Text="3 hours"></asp:ListItem>
                                                    <asp:ListItem Value="360" Text="6 hours"></asp:ListItem>
                                                    <asp:ListItem Value="540" Text="9 hours"></asp:ListItem>
                                                    <asp:ListItem Value="720" Text="12 hours"></asp:ListItem>
                                                    <asp:ListItem Value="1440" Text="24 hours (1 Day)" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="2880" Text="48 hours (2 days)"></asp:ListItem>
                                                    <asp:ListItem Value="4320" Text="72 hours (3 days)"></asp:ListItem>
                                                    <asp:ListItem Value="7200" Text="5 days"></asp:ListItem>
                                                    <asp:ListItem Value="10080" Text="1 week"></asp:ListItem>
                                                    <asp:ListItem Value="20160" Text="2 weeks"></asp:ListItem>
                                                    <asp:ListItem Value="43200" Text="1 month"></asp:ListItem>
                                                </asp:DropDownList>
                                             </div>
                                </div>
                            </div>
                            <div class="panel-success allGrid">
                                <table style="width: 100%" align="center" class="allUsers">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptrUsers" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblAutoURLNew" style="border-spacing: 0; display: none; width: 100%">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th>
                                                                    <input type="checkbox" id="ChkAllCheck" class="ChkAllCheck" />
                                                                </th>
                                                                <th>User Id
                                                                </th>
                                                                <th>UserName
                                                                </th>

                                                                <th>Email Id
                                                                </th>
                                                            </tr>


                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr id='<%#Eval("UserID")%>'>
                                                        <td>

                                                            <input type="checkbox" id="CheckAll" name="CheckAll" value='<%#Eval("UserID") + " ," + Eval("UserName") + "," + Eval("Login_ID") + "," + Eval("CustomerId") +"!"%>' class="CheckAll" runat="server" />
                                                            <%--  <asp:HiddenField ID="hdf_CustomerID" runat="server" Value='<% #Eval("CustomerId")%>' />--%>
                                                        </td>
                                                        <td style="margin-left: 1px;">

                                                            <%#Eval("UserID")%>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <%#Eval("UserName")%>   

                                                        </td>
                                                        <%--<td>
                                                            <%#Eval("LastName")%>
                                                        </td>--%>
                                                        <td>
                                                            <%#Eval("Login_Id")%>
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
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <table style="width: 100%">
                            <tr>

                                <td style="height: 23px; text-align: center; border: none; width: 20%">
                                    <asp:UpdateProgress AssociatedUpdatePanelID="UpnlAutoURL" ID="updProgress" runat="server"
                                        DynamicLayout="true">
                                        <ProgressTemplate>
                                            <img alt="Please wait..." src="images/Loding.gif" align="middle" id="imgLoad" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                                <td align="center" style="border: none; width: 60%">
                                    <%--<asp:Label ID="lblMessInfo" runat="server" Font-Names="Verdana" Font-Size="8pt" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>--%>
                                    <asp:Label ID="lblMessage" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                        Font-Bold="true" Style="text-align: left; display: block" ForeColor="Navy"></asp:Label>
                                </td>
                                <td align="right" style="width: 20%">
                                    <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClientClick="return(CheckboxSelection());"
                                        CssClass="btn btn-info CalcList" OnClick="btnSendMail_Click" CausesValidation="false"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: none; font-style: italic;" colspan="2">
                                    <asp:Label ID="lblNote" runat="server" Text="NOTE: UserId, LoginId, UserName and Mnemonic must be available for successful sending
                                    of emails."></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>


                <asp:HiddenField ID="hdnUserID" runat="server" />
                <asp:HiddenField ID="hdnStoreUserID" runat="server" />
                <asp:HiddenField ID="hdnGroup" runat="server" />
                <asp:HiddenField ID="hdnMailId" runat="server" />
                <asp:HiddenField ID="hdnCmeBackground" runat="server" Value="0" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <input type="hidden" id="hdnPageFocusChange" value="true" />

            </table>
            <asp:HiddenField ID="hdnSetExpiryId" runat="server" />

            <div style="display: none">
                <asp:Button ID="btnSetExpiry" runat="server" OnClick=" btnSetExpiry_Click" />
                <asp:Button ID="btnDeleteExcelPackage" runat="server" OnClick="btnDeleteExcelPackage_Click" />
                <asp:Button ID="btnDeleteAutoURL" runat="server" OnClick="btnDeleteAutoURL_Click" />
            </div>
            <table style="vertical-align: middle; width: 90%;" align="center">
                <tr>
                    <td>
                        <div style="border: 2px solid Navy;" align="left" valign="top">
                            <div class="panel panel-primary">
                                <div class="panel-heading" style="height: 30px;">
                                    Sent AutoURL History

                                            <div style="float: right;">
                                                Extend Expiry -  
                                                <asp:DropDownList ID="ddlExpiryGuid" runat="server" Width="170px" AutoPostBack="true">
                                                    <asp:ListItem Value="0" Text="-- Select --"></asp:ListItem>
                                                    <asp:ListItem Value="30" Text="30 minutes"></asp:ListItem>
                                                    <asp:ListItem Value="60" Text="1 hour"></asp:ListItem>
                                                    <asp:ListItem Value="120" Text="2 hours"></asp:ListItem>
                                                    <asp:ListItem Value="180" Text="3 hours"></asp:ListItem>
                                                    <asp:ListItem Value="360" Text="6 hours"></asp:ListItem>
                                                    <asp:ListItem Value="540" Text="9 hours"></asp:ListItem>
                                                    <asp:ListItem Value="720" Text="12 hours"></asp:ListItem>
                                                    <asp:ListItem Value="1440" Text="24 hours (1 Day)" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="2880" Text="48 hours (2 days)"></asp:ListItem>
                                                    <asp:ListItem Value="4320" Text="72 hours (3 days)"></asp:ListItem>
                                                    <asp:ListItem Value="7200" Text="5 days"></asp:ListItem>
                                                    <asp:ListItem Value="10080" Text="1 week"></asp:ListItem>
                                                    <asp:ListItem Value="20160" Text="2 weeks"></asp:ListItem>
                                                    <asp:ListItem Value="43200" Text="1 month"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                </div>
                            </div>
                            <div class="panel-success allGrid">
                                <table style="width: 100%" align="center" class="allUsers">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="rptAutoURLHistory" runat="server">
                                                <HeaderTemplate>
                                                    <table  id="tblAutoURLHistory" style="border-spacing: 0; display: none;width: 100%">
                                                        <thead>
                                                            <tr class="tblHdr">

                                                                <th>Username
                                                                </th>
                                                                <th>Package URL
                                                                </th>
                                                                <th>ExpiryDate
                                                                </th>
                                                                <th>DownLoaded
                                                                </th>
                                                                <th>Extend
                                                                </th>
                                                                <th>Delete
                                                                </th>
                                                            </tr>


                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="margin-left: 1px;">

                                                            <%#Eval("UserName")%>
                                                        </td>

                                                        <td>
                                                            <%#Eval("SentPackageURL")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("ExpiryDate")%>
                                                        </td>
                                                        <td>

                                                            <%# Eval("ClickCount").ToString() != "0" ? writeImagewithCount(Eval("ClickCount").ToString()) : "" %>
                                                        </td>
                                                        <td>
                                                            <%# writeImageForExpiry(Convert.ToString(Eval("id")),Convert.ToString(Eval("ExpiryDate"))) %>
                                                        </td>
                                                        <td>

                                                            <img src="images/delete-icon.png" alt="" onclick="deleteAutoURL(<%# Eval("id").ToString() %>)" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> 
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                </table>
                                <%--<table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoRecordsAutoURLHistory" runat="server" ForeColor="Red" Text="No Records Found."></asp:Label>
                                        </td>
                                    </tr>
                                </table>--%>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPackageDownLoad" />
            <%--<asp:PostBackTrigger ControlID="btnGeneratePack" />--%>
            <asp:PostBackTrigger ControlID="btnDeleteExcelPackage" />
            <asp:PostBackTrigger ControlID="btnSetExpiry" />
            <asp:PostBackTrigger ControlID="btnDeleteAutoURL" />
            <%--<asp:PostBackTrigger ControlID="ddlExcelBasePlugins" />
            <asp:PostBackTrigger ControlID="ddlExcelVersions" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
