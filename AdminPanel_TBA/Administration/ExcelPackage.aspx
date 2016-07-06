<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ExcelPackage.aspx.cs" Inherits="Administration.ExcelPackage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .allGrid table tr {
            border: 1px solid #428bca;
            border-top: none;
        }



            .allGrid table tr td {
                padding: 5px;
            }

        .borderMappings tbody tr {
            border: none;
        }

            .borderMappings tbody tr td {
                padding: 5px;
            }
    </style>
    <script type="text/javascript">
        /* ========================================================================
  * Bootstrap: tab.js v3.3.2
  * http://getbootstrap.com/javascript/#tabs
  * ========================================================================
  * Copyright 2011-2015 Twitter, Inc.
  * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
  * ======================================================================== */
        +function ($) {
            'use strict';
            // TAB CLASS DEFINITION
            // ====================
            var Tab = function (element) {
                this.element = $(element)
            }

            Tab.VERSION = '3.3.2'
            Tab.TRANSITION_DURATION = 150
            Tab.prototype.show = function () {
                var $this = this.element
                var $ul = $this.closest('ul:not(.dropdown-menu)')
                var selector = $this.data('target')
                if (!selector) {
                    selector = $this.attr('href')
                    selector = selector && selector.replace(/.*(?=#[^\s]*$)/, '') // strip for ie7
                }
                if ($this.parent('li').hasClass('active')) return
                var $previous = $ul.find('.active:last a')
                var hideEvent = $.Event('hide.bs.tab', {
                    relatedTarget: $this[0]
                })
                var showEvent = $.Event('show.bs.tab', {
                    relatedTarget: $previous[0]
                })
                $previous.trigger(hideEvent)
                $this.trigger(showEvent)
                if (showEvent.isDefaultPrevented() || hideEvent.isDefaultPrevented()) return
                var $target = $(selector)
                this.activate($this.closest('li'), $ul)
                this.activate($target, $target.parent(), function () {
                    $previous.trigger({
                        type: 'hidden.bs.tab',
                        relatedTarget: $this[0]
                    })
                    $this.trigger({
                        type: 'shown.bs.tab',
                        relatedTarget: $previous[0]
                    })
                })
            }
            Tab.prototype.activate = function (element, container, callback) {
                var $active = container.find('> .active')
                var transition = callback
                && $.support.transition
                && (($active.length && $active.hasClass('fade')) || !!container.find('> .fade').length)
                function next() {
                    $active
                    .removeClass('active')
                    .find('> .dropdown-menu > .active')
                    .removeClass('active')
                    .end()
                    .find('[data-toggle="tab"]')
                    .attr('aria-expanded', false)
                    element
                    .addClass('active')
                    .find('[data-toggle="tab"]')
                    .attr('aria-expanded', true)
                    if (transition) {
                        element[0].offsetWidth // reflow for transition
                        element.addClass('in')
                    } else {
                        element.removeClass('fade')
                    }
                    if (element.parent('.dropdown-menu').length) {
                        element
                        .closest('li.dropdown')
                        .addClass('active')
                        .end()
                        .find('[data-toggle="tab"]')
                        .attr('aria-expanded', true)
                    }
                    callback && callback()
                }
                $active.length && transition ?
                $active
                .one('bsTransitionEnd', next)
                .emulateTransitionEnd(Tab.TRANSITION_DURATION) :
                next()
                $active.removeClass('in')
            }
            // TAB PLUGIN DEFINITION
            // =====================
            function Plugin(option) {
                return this.each(function () {
                    var $this = $(this)
                    var data = $this.data('bs.tab')
                    if (!data) $this.data('bs.tab', (data = new Tab(this)))
                    if (typeof option == 'string') data[option]()
                })
            }
            var old = $.fn.tab
            $.fn.tab = Plugin
            $.fn.tab.Constructor = Tab
            // TAB NO CONFLICT
            // ===============
            $.fn.tab.noConflict = function () {
                $.fn.tab = old
                return this
            }
            // TAB DATA-API
            // ============
            var clickHandler = function (e) {
                e.preventDefault()
                Plugin.call($(this), 'show')
            }
            $(document)
            .on('click.bs.tab.data-api', '[data-toggle="tab"]', clickHandler)
            .on('click.bs.tab.data-api', '[data-toggle="pill"]', clickHandler)
        }(jQuery);
    </script>
    <script type="text/javascript">
        function jsDecimals(e) {

            var evt = (e) ? e : window.event;
            var key = (evt.keyCode) ? evt.keyCode : evt.which;
            if (key != null) {
                key = parseInt(key, 10);
                if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
                    if (!jsIsUserFriendlyChar(key, "Decimals")) {
                        return false;
                    }
                }
                else {
                    if (evt.shiftKey) {
                        return false;
                    }
                }
            }
            return true;
        }

        function jsIsUserFriendlyChar(val, step) {
            // Backspace, Tab, Enter, Insert, and Delete  
            if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
                return true;
            }
            // Ctrl, Alt, CapsLock, Home, End, and Arrows  
            if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
                return true;
            }
            //if (step == "Decimals") {
            //    if (val == 190 || val == 110) {  //Check dot key code should be allowed
            //        return true;
            //    }
            //}
            // The rest  
            return false;
        }
        var validFilesTypes = ["exe"];

        function ValidateFile() {
            debugger;
            document.getElementById("<%=lblmsgddl.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblmsgExcelVersion.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblmsgResolved.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblmsgfeature.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblflMessage.ClientID%>").innerHTML = "";

            document.getElementById("<%=lblMsgV2.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblMsgV3.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblMsgV4.ClientID%>").innerHTML = "";


            var isValidFile = false;
            var selection = document.getElementById("<%=ddlExcelPackage.ClientID%>");
            var selectedvalue = selection.options[selection.selectedIndex].value;
            if (selectedvalue == "0") {
                document.getElementById("<%=lblmsgddl.ClientID%>").innerHTML = "* Required";

            }


            var txtval = document.getElementById("<%=txtExcelVersionNumber.ClientID%>").value;
            var txtval2 = document.getElementById("<%=txtExcelVersionNumber2.ClientID%>").value;
            var txtval3 = document.getElementById("<%=txtExcelVersionNumber3.ClientID%>").value;
            var txtval4 = document.getElementById("<%=txtExcelVersionNumber4.ClientID%>").value;
            if (txtval == "" || txtval == "undefined" || txtval2 == "" || txtval2 == 'undefined'
                || txtval3 == "" || txtval3 == 'undefined' || txtval4 == "" || txtval4 == 'undefined') {
                document.getElementById("<%=lblmsgExcelVersion.ClientID%>").innerHTML = "* Required All";

            }

            var isChildSelected = true;
            var trvisible = $("#ctl00_ContentPlaceHolder1_trchildpackage").is(':visible');
            if (trvisible) {
                var childselection = document.getElementById("<%=ddlChildPackages.ClientID%>");
                var childselectedvalue = childselection.options[childselection.selectedIndex].value;
                if (childselectedvalue == "0") {
                    document.getElementById("<%=lblmsgddlchild.ClientID%>").innerHTML = "* Required";
                    isChildSelected = false;
                }
            }





            var featureDetails = document.getElementById("<%=txtFeatureDetails.ClientID%>").value;
            if (featureDetails == "" || featureDetails == "undefined") {
                document.getElementById("<%=lblmsgfeature.ClientID%>").innerHTML = "* Required";
            }

            var resolvedIssueDetails = document.getElementById("<%=txtResolvedIssueDetails.ClientID%>").value;
            if (resolvedIssueDetails == "" || resolvedIssueDetails == "undefined") {
                document.getElementById("<%=lblmsgResolved .ClientID%>").innerHTML = "* Required";
            }



            var file = document.getElementById("<%=flSetupMSI.ClientID%>");
            var label = document.getElementById("<%=lblflMessage.ClientID%>");

            var visible = $("#ctl00_ContentPlaceHolder1_lblSetupFileName").is(':visible');
            if (!visible) {
                var path = file.value;
                if (path == '' || path == 'undefined' || selectedvalue == "0" || txtval == "" || txtval == "undefined"
                    || txtval2 == "" || txtval2 == 'undefined'
                || txtval3 == "" || txtval3 == 'undefined' || txtval4 == "" || txtval4 == 'undefined' ||
                    featureDetails == "" || featureDetails == 'undefined' || resolvedIssueDetails == "" || resolvedIssueDetails == 'undefined') {
                    if (path == '' || path == 'undefined') {
                        label.style.color = "red";
                        label.innerHTML = "* Select Exe File";
                    }
                    if (!isChildSelected) {
                        isValidFile = false;
                    }
                    return isValidFile;
                }
                var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                for (var i = 0; i < validFilesTypes.length; i++) {
                    if (ext == validFilesTypes[i]) {
                        isValidFile = true;
                        break;
                    }
                }
                if (!isValidFile) {
                    label.style.color = "red";
                    label.innerHTML = "Invalid File. Please upload a File with" +
                     " extension:\n\n" + validFilesTypes.join(", ");
                }

            }
            else {
                if (txtval == "" || txtval == "undefined" || txtval2 == "" || txtval2 == 'undefined'
                || txtval3 == "" || txtval3 == 'undefined' || txtval4 == "" || txtval4 == 'undefined'
                    || featureDetails == "" || featureDetails == 'undefined' || resolvedIssueDetails == "" || resolvedIssueDetails == 'undefined') {
                    isValidFile = false;
                }
                else {
                    isValidFile = true;
                }
            }
            if (txtval2.length < 2) {
                document.getElementById("<%=lblMsgV2 .ClientID%>").innerHTML = "Required Two Digits.";
            }
            if (txtval3.length < 2) {
                document.getElementById("<%=lblMsgV3 .ClientID%>").innerHTML = "Required Two Digits.";
            }
            if (txtval4.length < 2) {
                document.getElementById("<%=lblMsgV4 .ClientID%>").innerHTML = "Required Two Digits.";
            }
            if (txtval2.length < 2 || txtval3.length < 2 || txtval4.length < 2) {

                isValidFile = false;
            }

            return isValidFile;
        }



        function getMapDetails(excelMapGuid) {
            debugger;
            document.getElementById('<%= hdnExcelProductVersionMapId.ClientID %>').value = excelMapGuid;
            $('#<%= btnEditMapping.ClientID %>').click();
        }
        function deleteMapDetails(excelId) {
            debugger;
            document.getElementById('<%= hdnDeleteExcelMap.ClientID %>').value = excelId;
            $('#<%= btnDeleteMapping.ClientID %>').click();
        }

        function ValidateChkList(source, arguments) {
            arguments.IsValid = IsCheckBoxChecked() ? true : false;

        }

        function IsCheckBoxChecked() {
            var isChecked = false;
            var list = document.getElementById('<%= chkAddinVersions.ClientID %>');
            if (list != null) {
                for (var i = 0; i < list.rows.length; i++) {
                    for (var j = 0; j < list.rows[i].cells.length; j++) {
                        var listControl = list.rows[i].cells[j].childNodes[0];
                        if (listControl.checked) {
                            isChecked = true;
                        }
                    }
                }
            }
            return isChecked;

        }

        function downLoadSetup(id) {
            document.getElementById('<%= hdnExcelVersionDownload.ClientID %>').value = id;
            $('#<%= btnDownLoadExcelVersion.ClientID %>').click();
        }

        function gerVersionDetails(vId) {
            document.getElementById('<%= hdnEditExcelVersion.ClientID %>').value = vId;
            $('#<%= btnEditExcelVersion.ClientID %>').click();
        }


        function ClearPack() {
            document.getElementById("<%=ddlExcelBasePlugins.ClientID%>").selectedIndex = "0";
            document.getElementById("<%=ddlExcelVersions.ClientID%>").selectedIndex = "0";
            var tblSelects = $("#tblAddins").find('select');
            var tblradios = $("#tblAddins").find('input[type=radio]');
            if (tblSelects != null) {
                for (var i = 0; i < tblSelects.length; i++) {
                    tblSelects[i].selectedIndex = "0";
                }
            }

            if (tblradios != null) {
                for (var j = 0; j < tblradios.length; j++) {
                    tblradios[j].checked = false;
                }
            }
            document.getElementById("<%=lblpackagemsg.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblmsgddlExcelBasePlugins.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblmsgddlExcelVersions.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblflPackage.ClientID%>").innerHTML = "*";
            document.getElementById("<%=flPackage.ClientID%>").value = "";

        }

        function getval(sel) {

            if (sel.value == "0") {
                debugger;
                var selectObj = document.getElementById("<%=ddlExcelVersions.ClientID%>");
                selectObj.options.length = 0;
                var option = document.createElement("option");
                option.text = "--Select--";
                option.value = "0";
                selectObj.add(option);
            }

        }

        function GeneratePackage() {
            document.getElementById("<%=lblpackagemsg.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblmsgddlExcelBasePlugins.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblmsgddlExcelVersions.ClientID%>").innerHTML = "*";

            var isValidate = false;
            var baseselection = document.getElementById("<%=ddlExcelBasePlugins.ClientID%>");
            var baseselectedvalue = baseselection.options[baseselection.selectedIndex].value;
            var baseselectedtext = baseselection.options[baseselection.selectedIndex].text;

            var baseversionselection = document.getElementById("<%=ddlExcelVersions.ClientID%>");
            var baseversionselectedvalue = baseversionselection.options[baseversionselection.selectedIndex].value;
            var baseversionselectedtext = baseversionselection.options[baseversionselection.selectedIndex].text;


            if (baseselectedvalue == "0") {
                document.getElementById("<%=lblmsgddlExcelBasePlugins.ClientID%>").innerHTML = "* Required";
            }

            if (baseversionselectedvalue == "0") {
                document.getElementById("<%=lblmsgddlExcelVersions.ClientID%>").innerHTML = "* Required";
            }

            if (baseselectedvalue == "0" || baseversionselectedvalue == "0") {
                return isValidate;
            }

            var strbuild = "";
            strbuild += baseversionselectedvalue.toString() + "#" + baseselectedtext.toString() + "#" + baseversionselectedtext.toString();


            var tblAddinSource = $("#tblAddins");
            var tblSelects = $("#tblAddins").find('select');
            var tblradios = $("#tblAddins").find('input[type=radio]');
            if (tblSelects != null) {
                for (var i = 0; i < tblSelects.length; i++) {
                    strbuild += ">AddIns=";
                    var op = tblSelects[i].options[tblSelects[i].selectedIndex].value;
                    var optext = tblSelects[i].options[tblSelects[i].selectedIndex].text;
                    var id = $(tblSelects[i]).attr("id");
                    var name = $(tblSelects[i]).attr("name");
                    strbuild += name.toString() + "#" + op.toString();
                    if (tblradios != null) {
                        for (var j = 0; j < tblradios.length; j++) {
                            if (tblradios[j].checked) {
                                var radioval = tblradios[j].value.toString();
                                var radiovalsplit = radioval.split("#");
                                if (tblSelects[i].id.toString() == radiovalsplit[2].toString()) {
                                    strbuild += "#ChildAddIns=";
                                    strbuild += tblradios[j].value.toString();
                                }
                            }
                        }
                    }



                }
            }


            var file = document.getElementById("<%=flPackage.ClientID%>");
            var label = document.getElementById("<%=lblflPackage.ClientID%>");
            var isValidFile = false;
            var path = file.value;

            if (path == '' || path == 'undefined') {
                label.style.color = "red";
                label.innerHTML = "* Required Exe File";
                return;
            }
            else {
                var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                for (var i = 0; i < validFilesTypes.length; i++) {
                    if (ext == validFilesTypes[i]) {
                        isValidFile = true;
                        break;
                    }
                }
            }

            if (!isValidFile) {
                label.style.color = "red";
                label.innerHTML = "Invalid File. Please upload a File with" +
                 " extension:\n\n" + validFilesTypes.join(", ");
            }
            else {
                label.style.color = "red";
                label.innerHTML = "*";
                document.getElementById('<%= hdnBuild.ClientID %>').value = strbuild;
                $('#<%= btnGeneratePack.ClientID %>').click();
            }

        }
        //tblProductVersionMap
        function BindFunctions() {
            $('#tblexcelversions').dataTable({
                "sEcho": 1,
                "bprocessing": true,
                "bserverSide": true,

                "iDisplayLength": 25,

                "pagingType": "simple",

                "initComplete": function (settings, json) {
                    $("#tblexcelversions").show();
                }

            });

            $('#tblProductVersionMap').dataTable({
                "sEcho": 1,
                "bprocessing": true,
                "bserverSide": true,
                "bSort": false,
                "iDisplayLength": 25,
                "pagingType": "simple",
                "initComplete": function (settings, json) {
                    $("#tblProductVersionMap").show();
                }

            });
        }
        function DeleleVersionDetails(exid) {
            document.getElementById('<%= hdnDeleteVersionId.ClientID %>').value = exid;
            $('#<%= btnDeleteVersion.ClientID %>').click();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
            </script>
            <asp:HiddenField ID="hdnExcelProductVersionId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnExcelProductVersionMapId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnExcelVersionDownload" runat="server" Value="" />
            <asp:HiddenField ID="hdnExcelExeDownload" runat="server" Value="" />
            <asp:HiddenField ID="hdnEditExcelVersion" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDeleteExcelMap" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDeleteVersionId" runat="server" Value="" />
            <table style="height: 10px; vertical-align: middle; width: 90%; margin-top: 15px;">
                <tr>
                    <td style="width: 40px">&nbsp;
                    </td>
                    <td>
                        <div style="float: left; font-family: Arial; font-size: medium; font-weight: bold;">
                            For Developer Use
                        </div>

                        <div style="float: right;">
                            <a href="ExcelURLDownLoad.aspx">Back To Excel Package</a>
                        </div>

                    </td>
                </tr>
            </table>
            <table id="tblSelect" runat="server" border="0" style="height: 10px; vertical-align: middle; width: 90%; margin-top: 15px; border: 2px solid Navy;"
                align="center" class="allUsers">
                <tr>
                    <td>
                        <div role="tabpanel" id="tabclient">

                            <!-- Nav tabs -->

                            <ul class="nav nav-tabs" id="excelTab" data-tabs="excelTab">
                                <li class="active" id="liversion"><a href="#version" data-toggle="tab">Excel Versions</a></li>
                                <li id="lipackage"><a href="#package" data-toggle="tab">Excel Packages</a></li>
                                <li id="limaster"><a href="#master" data-toggle="tab">Excel Master</a></li>

                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content">
                                <div class="tab-pane active" id="version">
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div align="left" valign="top">

                                            <div class="panel panel-primary" style="margin-bottom: 0;">
                                                <div class="panel-heading">
                                                    Excel Packages Version
                                                </div>
                                            </div>

                                            <table>
                                                <%--<tr>
                                                                <td colspan="4">
                                                                    <asp:Label ID="lblExcelPackageMsg1" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>--%>

                                                <tr>
                                                    <td width="20%">
                                                        <asp:Label ID="lblSelectExcelPackage" runat="server" Text="Select Excel Package Name"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:DropDownList ID="ddlExcelPackage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExcelPackage_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td width="15%" align="left">
                                                        <asp:Label ID="lblmsgddl" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblFeatureDetails" runat="server" Text="Feature Details"></asp:Label>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox ID="txtFeatureDetails" runat="server" TextMode="MultiLine" Rows="2" Width="100%" Height="100px"></asp:TextBox>
                                                    </td>
                                                    <td width="5%" style="text-align:right;">
                                                        <asp:Label ID="lblmsgfeature" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="trchildpackage" runat="server" visible="false">
                                                    <td width="20%">
                                                        <asp:Label ID="lblChildPackage" runat="server" Text="Select Child Package Name"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:DropDownList ID="ddlChildPackages" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>

                                                    </td>
                                                    <td width="15%" align="left">
                                                        <asp:Label ID="lblmsgddlchild" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    </td>
                                                    <td width="10%"></td>
                                                    <td width="30%"></td>
                                                    <td width="5%"></td>
                                                </tr>
                                                <tr>
                                                    <td width="20%">
                                                        <asp:Label ID="lblExcelVersionNumber" runat="server" Text="Enter Version Number"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <table>
                                                            <tr style="border: none;">
                                                                <td>
                                                                    <asp:TextBox ID="txtExcelVersionNumber" runat="server" onkeydown="return jsDecimals(event);" Width="30px" MaxLength="2"></asp:TextBox>.                                                        
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtExcelVersionNumber2" runat="server" onkeydown="return jsDecimals(event);" Width="30px" MaxLength="2"></asp:TextBox>.
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtExcelVersionNumber3" runat="server" onkeydown="return jsDecimals(event);" Width="30px" MaxLength="2"></asp:TextBox>.
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtExcelVersionNumber4" runat="server" onkeydown="return jsDecimals(event);" Width="30px" MaxLength="2"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="border: none;">
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Style="display: none;" Text="Required"></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblMsgV2" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMsgV3" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMsgV4" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                    <td width="15%">X - XX - XX- XX
                                                        <asp:Label ID="lblmsgExcelVersion" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    </td>
                                                    <td width="10%">
                                                        <asp:Label ID="lblResolvedIssueDetails" runat="server" Text="Resolved Issue Details"></asp:Label>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox ID="txtResolvedIssueDetails" runat="server" TextMode="MultiLine" Rows="2" Width="100%" Height="100px"></asp:TextBox>
                                                    </td>
                                                    <td width="5%" style="text-align:right;">
                                                        <asp:Label ID="lblmsgResolved" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%">
                                                        <asp:Label ID="lblSelectMSI" runat="server" Text="Select EXE File"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:FileUpload ID="flSetupMSI" runat="server" />

                                                        <asp:ImageButton ID="imgDownloadExcelSetup" runat="server" ImageUrl="images/download.png" OnClick="imgDownloadExcelSetup_Click" Visible="false" />
                                                        <asp:Label ID="lblSetupFileName" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblflMessage" runat="server" Text="*" ForeColor="Red"></asp:Label>

                                                    </td>
                                                    <td width="10%"></td>
                                                    <td width="20%"></td>
                                                    <td width="15%"></td>
                                                </tr>
                                                <%--  <tr id="trExe" runat="server" visible="false">
                                                    <td width="20%">
                                                        <asp:Label ID="Label2" runat="server" Text="Select SetUp File"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:FileUpload ID="flExe" runat="server" />
                                                        <asp:ImageButton ID="imgDownloadExcelEXE" runat="server" ImageUrl="images/download.png" Visible="false" OnClick="imgDownloadExcelEXE_Click" />
                                                        <asp:Label ID="lblEXEName" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                    <td width="15%">
                                                        <asp:Label ID="lblfl2Message" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td width="10%"></td>
                                                    <td width="20%"></td>
                                                    <td width="15%"></td>
                                                </tr>--%>


                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Button ID="btnSubmitExcelVersion" runat="server" CssClass="btn btn-info" Text="Save" OnClientClick="return ValidateFile()"
                                                            OnClick="btnSubmitExcelVersion_Click" />
                                                        <asp:Button ID="btnExcelVersionClear" runat="server" CssClass="btn btn-info" Text="Clear"
                                                            CausesValidation="false" OnClick="btnExcelVersionClear_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div>
                                                            <asp:Label ID="lblExcelPackageMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>

                                        <div class="panel-success pnl">

                                            <asp:Repeater ID="rptExcelVersions" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblexcelversions" style="width: 100%; display: none" class="innerPnl">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th>Product 
                                                                </th>
                                                                <th>VersionNumber
                                                                </th>
                                                                <th>LastUpdatedBy
                                                                </th>
                                                                <th>LastUpdatedDate</th>
                                                                <th>Edit</th>
                                                                <th>Delete</th>
                                                                <th>Download</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="margin-left: 1px;">
                                                            <%#Eval("ExcelProductMasterName")%>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <%#Eval("VersionNumber")%>   
                                                        </td>
                                                        <td>
                                                            <%#Eval("LastUpdatedByName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("LastUpdatedDate")%>
                                                        </td>
                                                        <td>
                                                            <img src='images/getDetails.png' alt='' onclick='gerVersionDetails(<%# Eval("ExcelProductVersionMasterId").ToString() %>);' />
                                                        </td>
                                                        <td>
                                                            <img src='images/delete-icon.png' alt='' onclick='DeleleVersionDetails(<%# Eval("ExcelProductVersionMasterId").ToString() %>);' />

                                                        </td>
                                                        <td>
                                                            <%# Eval("IsSetUpAvailable").ToString() != "" ? writeAutoURLwithImage(Eval("ExcelProductVersionMasterId").ToString()) : writeAutoURLwithoutImage(Eval("ExcelProductVersionMasterId").ToString()) %>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody> 
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>

                                        </div>

                                        <div>
                                            <asp:Label ID="lblNoRecordsVersion" runat="server" ForeColor="Red" Text="No Records found."></asp:Label>
                                            <div style="display: none;">
                                                <asp:Button ID="btnDeleteVersion" runat="server" OnClick="btnDeleteVersion_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div style="width: 90%; margin: 0 auto 15px;" class="allGrid">

                                        <div align="left" valign="top">

                                            <div class="panel panel-primary" style="margin-bottom: 0;">
                                                <div class="panel-heading">
                                                    Excel Packages Version Mappings
                                                </div>
                                            </div>
                                            <table>

                                                <tr>
                                                    <td style="width: 30%;">
                                                        <asp:Label ID="lblMainPlugin" runat="server" Text="Select Main Plugin Version"></asp:Label>
                                                    </td>
                                                    <td style="width: 35%;">
                                                        <asp:DropDownList ID="ddlMainExcelPlugin" runat="server"></asp:DropDownList>

                                                    </td>
                                                    <td style="width: 35%;">
                                                        <span style="color: red">*</span>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlMainExcelPlugin" InitialValue="0"
                                                            ErrorMessage="Required"
                                                            ValidationGroup="SaveExcelMap"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSelectAddIns" runat="server" Text="Select AddIns"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBoxList ID="chkAddinVersions" runat="server" CssClass="borderMappings"></asp:CheckBoxList>


                                                    </td>
                                                    <td valign="top"><span style="color: red">*</span>
                                                        <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateChkList"
                                                            runat="server" ValidationGroup="SaveExcelMap">Required.</asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Button ID="btnSaveExcelMappings" runat="server" CssClass="btn btn-info" Text="Save" OnClick="btnSaveExcelMappings_Click"
                                                            ValidationGroup="SaveExcelMap" />
                                                        <asp:Button ID="btnExcelMappingClear" runat="server" CssClass="btn btn-info" Text="Clear"
                                                            CausesValidation="false" OnClick="btnExcelMappingClear_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div>
                                                            <asp:Label ID="lblmsgMapping" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>

                                        <div align="left" valign="top">

                                            <div class="panel-success pnl">
                                                <asp:Repeater ID="rptExcelProductVersionMap" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tblProductVersionMap" class="innerPnl">
                                                            <thead>
                                                                <tr class="tblHdr">
                                                                    <th>Product 
                                                                    </th>
                                                                    <th>VersionNumber
                                                                    </th>
                                                                    <th>AddInName
                                                                    </th>
                                                                    <th>AddInVersionNumber
                                                                    </th>
                                                                    <th>Edit
                                                                    </th>
                                                                    <th>Delete
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>


                                                            <td>
                                                                <%# Eval("MainExcelProductMasterName").ToString() %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("MainVersionNumber").ToString() != "0" ? Eval("MainVersionNumber").ToString() : "" %>
                                                            </td>
                                                            <td>
                                                                <%#Eval("CompatibleExcelProductMasterName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("CompatibleVersionNumber")%>
                                                            </td>
                                                            <td>
                                                                <%# Eval("MainVersionNumber").ToString() != "0" ? writeTdimgwithColspan(Eval("UniID").ToString()) : "" %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("MainVersionNumber").ToString() != "0" ? writeTdimgDelete(Eval("UniID").ToString()) : "" %>
                                                                
                                                            </td>
                                                        </tr>


                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>

                                        <div>
                                            <asp:Label ID="lblNoRecordsMappings" runat="server" ForeColor="Red" Text="No Records Found.."></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane" id="package">
                                    <div style="width: 90%; margin: 0 auto 15px;" class="allGrid pnl">
                                        <div align="left" valign="top">
                                            <div class="panel panel-primary">
                                                <div class="panel-heading">
                                                    Excel Package Upload
                                                </div>
                                            </div>
                                            <div>


                                                <table class="innerPnl">
                                                    <tr>
                                                        <td valign="middle" style="text-align: right; width: 20%;">Select Base Plugin :
                                                        </td>
                                                        <td style="width: 40%;">

                                                            <asp:DropDownList ID="ddlExcelBasePlugins" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExcelBasePlugins_SelectedIndexChanged"></asp:DropDownList>

                                                        </td>
                                                        <td style="width: 40%;">
                                                            <asp:Label ID="lblmsgddlExcelBasePlugins" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; width: 20%;">Select Version :
                                                   

                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlExcelVersions" runat="server" OnSelectedIndexChanged="ddlExcelVersions_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Selected="True" Text="-- Select --" Value="0"></asp:ListItem>

                                                            </asp:DropDownList>


                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblmsgddlExcelVersions" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <div id="trAddins" runat="server" visible="false">
                                                        </div>
                                                    </tr>
                                                    <tr id="trflPackage" runat="server" visible="false">
                                                        <td style="text-align: right; width: 18%;">Select Package File :
                                                        </td>
                                                        <td>
                                                            <asp:FileUpload ID="flPackage" runat="server" />
                                                            <asp:Label ID="lblflPackage" runat="server" ForeColor="Red">*</asp:Label>
                                                        </td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td></td>
                                                        <td>

                                                            <input type="button" id="genera" onclick="GeneratePackage()" value="Generate" class="btn btn-info" />
                                                            <div style="display: none">
                                                                <asp:Button ID="btnGeneratePack" runat="server" Text="Generate Package" class="btn btn-info" OnClick="btnGeneratePack_Click" />
                                                            </div>
                                                            <asp:HiddenField ID="hdnBuild" runat="server" Value="blank" />
                                                            <input type="button" id="genclear" onclick="ClearPack()" value="Clear" class="btn btn-info" />
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label ID="lblpackagemsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>



                                                </table>



                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="tab-pane" id="master">
                                    <div style="width: 90%; margin: 0 auto 15px;" class="allGrid">

                                        <div class="panel panel-primary" style="margin-bottom: 0;">
                                            <div class="panel-heading">
                                                Excel Packages
                                            </div>
                                        </div>

                                        <div>
                                            <table>

                                                <tr>
                                                    <td style="width: 20%">
                                                        <asp:Label ID="lblExcelPackageName" runat="server" Text="Enter Excel Plugin Name"></asp:Label>
                                                    </td>
                                                    <td style="width: 40%">
                                                        <asp:TextBox ID="txtExcelPackageName" runat="server"></asp:TextBox>

                                                    </td>
                                                    <td style="width: 40%">
                                                        <span style="color: red">*</span>
                                                        <asp:RequiredFieldValidator ID="rqtxtExcelPackageName" runat="server" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                                            ForeColor="Red" ControlToValidate="txtExcelPackageName" ValidationGroup="ExcelPackage"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAppGUID" runat="server" Text="Enter GUID"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAppGuid" runat="server"></asp:TextBox>

                                                    </td>
                                                    <td>
                                                        <span style="color: red">*</span>
                                                        <asp:RequiredFieldValidator ID="rqtxtAppGuid" runat="server" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                                            ForeColor="Red" ControlToValidate="txtAppGuid" ValidationGroup="ExcelPackage"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblIsMain" runat="server" Text="Is Base Plugin"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsBasePlugin" runat="server" OnCheckedChanged="chkIsBasePlugin_Clicked" AutoPostBack="true" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr id="trParentPackage" runat="server">
                                                    <td>
                                                        <asp:Label ID="lblParentPackage" runat="server" Text="Parent Excel Plugin"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlParentExcelPackage" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td></td>
                                                </tr>


                                                <tr>
                                                    <td colspan="3">

                                                        <asp:Button ID="btnSubmitExcelPackage" runat="server" CssClass="btn btn-info" Text="Save" OnClick="btnSubmitExcelPackage_Click" ValidationGroup="ExcelPackage" />
                                                        <asp:Button ID="btnExcelPackageClear" runat="server" CssClass="btn btn-info" Text="Clear"
                                                            CausesValidation="false" OnClick="btnExcelPackageClear_Click" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div>
                                                            <asp:Label ID="lblExcelPackageMessage" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>

                                        <div align="left" valign="top">

                                            <div class="panel-success">

                                                <asp:Repeater ID="rptExcelProductMaster" runat="server">
                                                    <HeaderTemplate>
                                                        <table style="width: 100%">
                                                            <thead>
                                                                <tr class="tblHdr">

                                                                    <th>Product
                                                                    </th>
                                                                    <th>AppGUID
                                                                    </th>
                                                                    <th>ParentName
                                                                    </th>
                                                                    <th>LastUpdatedBy
                                                                    </th>
                                                                    <th>LastUpdatedDate</th>
                                                                    <th>Edit</th>
                                                                    <th>Delete</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="margin-left: 1px;">

                                                                <%#Eval("ExcelProductMasterName")%>
                                                            </td>
                                                            <td style="width: 170px">
                                                                <%#Eval("AppGUID")%>   
                                                            </td>
                                                            <td>
                                                                <%#Eval("ParentName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("LastUpdatedByName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("LastUpdatedDate")%>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgEditExcelMaster" CommandArgument='<%#Eval("ExcelProductMasterId")%>' OnClick="imgEditExcelMaster_Click" ToolTip="Edit Excel Master Details" src="images/getDetails.png" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgDeleteExcelMaster" runat="server" ImageUrl="~/images/delete-icon.png" CommandName="Delete" CommandArgument='<%# Eval("ExcelProductMasterId").ToString() %>' OnClick="imgDeleteExcelMaster_Click" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>

                                            </div>
                                            <div style="display: none">
                                                <asp:Button ID="btnEditMapping" runat="server" OnClick="btnEditMapping_Click" />
                                                <asp:Button ID="btnDownLoadExcelVersion" runat="server" OnClick="btnDownLoadExcelVersion_Click" />
                                                <asp:Button ID="btnEditExcelVersion" runat="server" OnClick="btnEditExcelVersion_Click" />
                                                <asp:Button ID="btnDeleteMapping" runat="server" OnClick="btnDeleteMapping_Click" />
                                            </div>
                                        </div>

                                        <div>
                                            <asp:Label ID="lblNoRecordsForBase" runat="server" Text="No Records Found.." ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                    </td>

                </tr>

            </table>


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmitExcelVersion" />
            <asp:PostBackTrigger ControlID="btnDownLoadExcelVersion" />
            <asp:PostBackTrigger ControlID="imgDownloadExcelSetup" />
            <asp:PostBackTrigger ControlID="btnEditExcelVersion" />
            <%--<asp:PostBackTrigger ControlID="imgDownloadExcelEXE" />--%>
            <asp:PostBackTrigger ControlID="btnGeneratePack" />
            <asp:PostBackTrigger ControlID="ddlExcelBasePlugins" />
            <asp:PostBackTrigger ControlID="ddlExcelVersions" />
            <asp:PostBackTrigger ControlID="btnDeleteVersion" />
            <asp:PostBackTrigger ControlID="btnDeleteMapping" />
            <asp:PostBackTrigger ControlID="btnSaveExcelMappings" />
            <asp:PostBackTrigger ControlID="ddlExcelPackage" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hidTAB" runat="server" Value="version" />



    <script type="text/javascript">
        $('#excelTab a[href="#version"]').click(function (e) {
            e.preventDefault();
            $("#excelTab").removeClass("active");
            $(this).addClass('active');
            document.getElementById('<%=hidTAB.ClientID %>').value = "version";
            $(this).tab('show');

        });

        $('#excelTab a[href="#master"]').click(function (e) {
            e.preventDefault();
            $("#excelTab").removeClass("active");
            $(this).addClass('active');
            document.getElementById('<%=hidTAB.ClientID %>').value = "master";
            $(this).tab('show');

        });

        $('#excelTab a[href="#package"]').click(function (e) {
            debugger;
            e.preventDefault();
            $("#excelTab").removeClass("active");
            $(this).addClass('active');
            document.getElementById('<%=hidTAB.ClientID %>').value = "package";
            $(this).tab('show');

        });

        function SelectExcelTab() {
            $("#version").removeClass("active");
            $("#liversion").removeClass("active");
            $("#package").removeClass("active");
            $("#lipackage").removeClass("active");
            $("#master").addClass('active');
            $("#limaster").addClass('active');
            var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
            $('#excelTab a[href="' + tab + '"]').tab('show');

        }

        function SelectExcelPackageTab() {
       
            $("#version").removeClass("active");
            $("#liversion").removeClass("active");
            $("#master").removeClass("active");
            $("#limaster").removeClass("active");
            $("#package").addClass('active');
            $("#lipackage").addClass('active');
            var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
            $('#excelTab a[href="' + tab + '"]').tab('show');

        }


    </script>
    <style>
        div#tblexcelversions_filter.dataTables_filter {
            /*border: 1px solid #428bca;*/
            border-top: none;
        }

        div#tblexcelversions_length.dataTables_length {
            margin-left: 5px;
            margin-top: 5px;
        }

        #tblexcelversions_length select {
            text-align: left;
            font-size: 88%;
            width: 71px !important;
        }

        #tblexcelversions_filter input {
            margin-right: 10px;
        }



        /*tblProductVersionMap*/

        div#tblProductVersionMap_filter.dataTables_filter {
            /*border: 1px solid #428bca;*/
            border-top: none;
        }

        div#tblProductVersionMap_length.dataTables_length {
            margin-left: 5px;
            margin-top: 5px;
        }

        #tblProductVersionMap_length select {
            text-align: left;
            font-size: 88%;
            width: 71px !important;
        }

        #tblProductVersionMap_filter input {
            margin-right: 10px;
        }
    </style>
</asp:Content>
