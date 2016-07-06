<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ExcelAutoUpdateConfig.aspx.cs" Inherits="Administration.ExcelAutoUpdateConfig" %>

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
        }

        function BindVersionFunctions() {
            $('#tblversioninfo').dataTable({
                "sEcho": 1,
                "bprocessing": true,
                "bserverSide": true,

                "iDisplayLength": 25,

                "pagingType": "simple",
                "order": [[3, "desc"]],
                "initComplete": function (settings, json) {
                    $("#tblversioninfo").show();
                }

            });
        }

        function BindClickOnceFileFunctions() {
            $('#tblClickOnceFile').dataTable({
                "sEcho": 1,
                "bprocessing": true,
                "bserverSide": true,

                "iDisplayLength": 25,

                "pagingType": "simple",
                "order": [[3, "desc"]],
                "initComplete": function (settings, json) {
                    $("#tblClickOnceFile").show();
                }

            });
        }

        function BindExcelProductFunctions() {
            $('#tblExcelProductFile').dataTable({
                "sEcho": 1,
                "bprocessing": true,
                "bserverSide": true,

                "iDisplayLength": 25,

                "pagingType": "simple",
                "order": [[3, "desc"]],
                "initComplete": function (settings, json) {
                    $("#tblExcelProductFile").show();
                }

            });
        }


        function getVersionDetails(vId, version) {
            document.getElementById('<%= hdnObjectId.ClientID %>').value = vId;
            document.getElementById('<%= hdnVersion.ClientID %>').value = version;
            $('#<%= btnEditExcelVersion.ClientID %>').click();
        }
        function deleteVersionDetails(vId, version) {
            document.getElementById('<%= hdnObjectId.ClientID %>').value = vId;
            document.getElementById('<%= hdnVersion.ClientID %>').value = version;
            $('#<%= btnDeleteExcelVersion.ClientID %>').click();
        }

    </script>

    <asp:UpdatePanel ID="UplPnl" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
                Sys.Application.add_load(BindVersionFunctions);
                Sys.Application.add_load(BindClickOnceFileFunctions);
                Sys.Application.add_load(BindExcelProductFunctions);

            </script>

            <body onload="initiateFuntion()">
                <asp:HiddenField ID="hidTAB" runat="server" Value="update" />
                <asp:HiddenField ID="hdnObjectId" runat="server" Value="" />
                <asp:HiddenField ID="hdnVersion" runat="server" Value="" />
                <asp:HiddenField ID="hdnTabVal" runat="server" Value="" />

                <div style="display: none">
                    <asp:Button ID="btnEditExcelVersion" runat="server" OnClick="btnEditExcelVersion_Click" />
                    <asp:Button ID="btnDeleteExcelVersion" runat="server" OnClick="btnDeleteExcelVersion_Click" />
                </div>
                <table id="tblSelect" runat="server" border="0" style="height: 10px; vertical-align: middle; width: 90%; margin-top: 15px; border: 2px solid Navy;"
                    align="center" class="allUsers">
                    <tr>
                        <td>

                            <ul class="nav nav-tabs" id="excelTab" data-tabs="excelTab">
                                <li id="liupdate" class="active"><a href="#update" data-toggle="tab">Auto Update</a></li>
                                <li id="liversion"><a href="#version" data-toggle="tab">Auto URL</a></li>
                                <li id="liproducts"><a href="#products" data-toggle="tab">Excel New Products</a></li>
                                <li id="liexcel"><a href="#excel" data-toggle="tab">Excel Click once Versions</a></li>
                            </ul>



                            <div class="tab-content">
                                <div class="tab-pane active" id="update">
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">

                                        <div class="panel panel-primary" style="margin-bottom: 0; margin-top: 20px;">
                                            <div class="panel-heading">
                                                Excel Force Update Configuration
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblObject" runat="server" Text="Select Object Name"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlExcelObjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExcelObjects_SelectedIndexChanged">
                                                    </asp:DropDownList>

                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblmsgddlobj" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                                                        ControlToValidate="ddlExcelObjects" InitialValue="0"
                                                        ErrorMessage="Required"
                                                        ValidationGroup="SaveExcelMap"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblVersion" runat="server" Text="Select Version"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlExcelVersions" runat="server" AutoPostBack="true">
                                                    </asp:DropDownList>

                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblMessageVersion" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic"
                                                        ControlToValidate="ddlExcelVersions" InitialValue="0"
                                                        ErrorMessage="Required"
                                                        ValidationGroup="SaveExcelMap"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label1" runat="server" Text="Force Update"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:CheckBox ID="chkForceUpdate" runat="server" AutoPostBack="true" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="btnSubmitExcelVersion" runat="server" CssClass="btn btn-info" Text="Save" ValidationGroup="SaveExcelMap" OnClick="btnSubmitExcelVersion_Click" />
                                                    <asp:Button ID="btnExcelVersionClear" runat="server" CssClass="btn btn-info" Text="Clear"
                                                        CausesValidation="false" OnClick="btnExcelVersionClear_Click" />

                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>

                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div align="center" class="panel-success pnl allGrid">

                                            <asp:Repeater ID="rptExcelVersions" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblexcelversions" style="width: 90%; display: none" class="innerPnl">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th>ObjectId</th>
                                                                <th>Object Name</th>
                                                                <th>Version</th>
                                                                <th>Force Update</th>
                                                                <th>Edit</th>
                                                                <th>Delete</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="margin-left: 1px;">
                                                            <%#Eval("ObjectId")%>
                                                        </td>
                                                        <td style="width: 170px">
                                                            <%#Eval("ObjectName")%>   
                                                        </td>
                                                        <td>
                                                            <%#Eval("ObjectVersion")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("ForceUpdate")%>
                                                        </td>
                                                        <td>
                                                            <img src='images/getDetails.png' alt='' onclick='<%# string.Format("return getVersionDetails(\"{0}\", \"{1}\");", Eval("ObjectId"), Eval("ObjectVersion")) %>' />
                                                        </td>
                                                        <td>
                                                            <img src='images/delete-icon.png' alt='' onclick='<%# string.Format("return deleteVersionDetails(\"{0}\", \"{1}\");", Eval("ObjectId"), Eval("ObjectVersion")) %>' />

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
                                            <asp:Label ID="lblNoRecordsMappings" runat="server" ForeColor="Red" Text="No Records Found.."></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="version">
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div class="panel panel-primary" style="margin-bottom: 0; margin-top: 20px;">
                                            <div class="panel-heading">
                                                Auto URL
                                            </div>
                                        </div>
                                        <table>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblVersionName" runat="server" Text="Version Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtVersionName" runat="server" onfocus="clearErrorVersionName()" onblur="checkVersionName()"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorVersionName" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblFileName" runat="server" Text="File Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtFileName" runat="server" onfocus="clearErrorFileName()" onblur="checkFileName()"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblZip" Text=".zip"></asp:Label>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorFileName" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblFilePath" runat="server" Text="File Path:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:Label ID="lblPrimaryPath" runat="server" Text="/WWSApp/ExcelPkg/"></asp:Label>
                                                    + 
                                                    <asp:TextBox ID="txtFilePath" runat="server" onfocus="clearErrorFilePath()" onblur="checkFilePath()"></asp:TextBox>
                                                    <br>
                                                    </br>
                                                    <asp:Label runat="server" ID="lblEgFilePathTwd" Text="e.g. 80517/TWD" Style="padding-left: 8em"></asp:Label>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorFilePath" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblFileUpload" runat="server" Text="File Upload:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:FileUpload ID="fileUploadExe" runat="server" onchange="checkFile(this)" />
                                                </td>
                                                <td width="15%" align="left">

                                                    <asp:Label ID="lblErrorFileUpload" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                    <%--<asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ErrorMessage="*Required"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnUpdateVersion" runat="server" CssClass="btn btn-info"
                                                        Text="Submit" OnClientClick="if (!validateData()) {return false;}"
                                                        OnClick="btnUpdateVersion_Click" ValidationGroup="SubmitVersionInfo" />
                                                    <%--<asp:Button ID="btnClearVersion" CssClass="btn btn-info" runat="server"
                                                    Text="Clear" OnClientClick="btnClearVersionInfo()"/>--%>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div align="center" class="panel-success pnl allGrid">
                                            <asp:Repeater ID="rptVersionInfo" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblversioninfo" style="width: 95%; display: none" class="innerPnl">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th>Version Name</th>
                                                                <%--<th>Ref Key</th>--%>
                                                                <th>File Name</th>
                                                                <th>File Path</th>
                                                                <th>Date Of Creation</th>
                                                                <th>Is Active</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Eval("VersionName")%>
                                                        </td>
                                                        <%--<td>
                                                        <%#Eval("RefKey")%>
                                                        </td>--%>
                                                        <td>
                                                            <%#Eval("FileName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("FilePath")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Record_Create_DtTime")%>
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:ImageButton ImageUrl='images/right-icon.png' Visible='<%# (byte) Eval("IsActive")==(byte)1? true:false%>' ID='imgBtnRight' OnCommand="imgBtnRight_Command" CommandName="ToDeactivate" CommandArgument='<%#Eval("RefKey")+","+ Eval("VersionName")+","+ Eval("FileName")+","+ Eval("FilePath")+","+ Eval("Record_Create_By")+","+ Eval("Record_Create_DtTime")%>' runat='server' />
                                                            <asp:ImageButton ImageUrl='images/cancel.jpg' Height="19" Width="19" Visible='<%# (byte) Eval("IsActive")!=(byte)1? true:false%>' ID='imgBtnWrong' OnCommand="imgBtnRight_Command" CommandName="ToActivate" CommandArgument='<%#Eval("RefKey")+","+ Eval("VersionName")+","+ Eval("FileName")+","+ Eval("FilePath")+","+ Eval("Record_Create_By")+","+ Eval("Record_Create_DtTime")%>' runat='server' />
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
                                            <asp:Label ID="lblVersionMsg" runat="server" ForeColor="Red" Text="No Records Found.."></asp:Label>
                                        </div>
                                    </div>

                                </div>

                               <!-- ******************************************************* Abhishek Code ******************************************************* -->

                                    <div class="tab-pane" id="products">
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div class="panel panel-primary" style="margin-bottom: 0; margin-top: 20px;">
                                            <div class="panel-heading">
                                                Excel New Products
                                            </div>
                                        </div>
                                        <table>

                                           <!-- Textbox For Excel Product Name -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label2" runat="server" Text="Product Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtExcelProductName" ReadOnly="false" runat="server" width="300"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorExcelProductName" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>

                                            </tr>
                                          
                                            <!-- Drop Down List For Group Name -->
                                              <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label5" runat="server" Text="Group Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddExcelGroupName" runat="server" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorExcelGroupName" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>
                                           
                                            <!-- Drop Down List For Priority -->
                                              <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label6" runat="server" Text="Priority:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddExcelPriority" runat="server" AutoPostBack="false">
                                                        <asp:ListItem Text="--Select--" Value="0" />
                                                        <asp:ListItem Text="1" Value="1" />
                                                        <asp:ListItem Text="2" Value="2" />
                                                        <asp:ListItem Text="3" Value="3" />
                                                        <asp:ListItem Text="4" Value="4" />
                                                        <asp:ListItem Text="5" Value="5" />
                                                        <asp:ListItem Text="6" Value="6" />
                                                        <asp:ListItem Text="7" Value="7" />
                                                        <asp:ListItem Text="8" Value="8" />
                                                        <asp:ListItem Text="9" Value="9" />
                                                        <asp:ListItem Text="10" Value="10" />
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorExcelPriority" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>

                                            <!-- Textbox For Excel Installed Name -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label7" runat="server" Text="Installed Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtExcelInstalledName" ReadOnly="false" runat="server" width="300"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorExcelInstalledName" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>

                                            </tr>

                                            <!-- Submit button For Excel New Product -->
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label14" runat="server" ForeColor="Green"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExcelProduct" runat="server" CssClass="btn btn-info"
                                                        Text="Submit" OnClientClick="if (!validateExcelProduce()) {return false;}"
                                                        OnClick="btnExcel_Products_Click" ValidationGroup="SubmitExcelProduct" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div align="center" class="panel-success pnl allGrid">
                                            <asp:Repeater ID="rptExcelProductFile" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblExcelProductFile" style="width: 95%; display: none" class="innerPnl">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                
                                                                <th>Product Name</th>
                                                                <th>Group Name</th>
                                                                <th>Priority</th>
                                                                <th>Installed ProductName</th>
                                                                <th>[Record Created By</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        
                                                        <td>
                                                            <%#Eval("ProductName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("GroupName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Priority")%>
                                                        </td>
                                                         <td>
                                                            <%#Eval("Installed_ProductName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Record_Created_By")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody></table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblExcelProductGridMsg" runat="server" ForeColor="Red" Text="No Records Found.."></asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <!-- ****************************************************************************************************************************************** -->



                                <div class="tab-pane" id="excel">
                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div class="panel panel-primary" style="margin-bottom: 0; margin-top: 20px;">
                                            <div class="panel-heading">
                                                Click once Version
                                            </div>
                                        </div>
                                        <table>

                                            <!--  Product Name -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProduct" runat="server" Text="Select Product Name:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorProduct" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>

                                           
<%--                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProductGuid" runat="server" Text="Product GUID:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtProductGuid" ReadOnly="true" runat="server"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left"></td>
                                            </tr>
                                            <tr>--%>

                                             <!-- Product Version  -->

                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProductVersion" runat="server" Text="Product Version:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtProductVersion" runat="server"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorProductVersion" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>

                                            <!-- IsFocusUpdate Type  -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProductType" runat="server" Text="IsFocusUpdate Type:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:RadioButtonList ID="rbtProductType" runat="server"
                                                        RepeatDirection="Horizontal" RepeatLayout="Table">
                                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td width="15%" align="left"></td>
                                            </tr>

                                            <!-- Server IP  -->
                                             <tr>
                                               <td width="20%">
                                                    <asp:Label ID="lblServerIP" runat="server" Text="Server IP:"></asp:Label>
                                                </td>
                                                <td width="20%"> 
                                                    <asp:TextBox ID="txtServerIP" runat="server"></asp:TextBox>  
                                                </td>
                                                 <td width="15%" align="left">
                                                     <asp:Label ID="lblErrorServerIP" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                 </td>
                                            </tr>

                                            <!-- Destination (Path) of Product  -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProductPath" runat="server" Text="Destination Folder:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:TextBox ID="txtProductPath" runat="server" onfocus="clearErrorFilePath()" onblur="checkFilePath()"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorProductPath" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>

                                            <!-- Source (Path) of Product Version  -->
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblProductFileUpload" runat="server" Text="Source File:"></asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:FileUpload ID="prodFileUploadExe" runat="server" onchange="checkFileExcel(this)" />
                                                </td>
                                                <td width="15%" align="left">
                                                    <asp:Label ID="lblErrorProductFileUpload" runat="server" ForeColor="Red" Text="*"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblClickOnceFileMsg" runat="server" ForeColor="Green"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClickOnceFile" runat="server" CssClass="btn btn-info"
                                                        Text="Submit" OnClientClick="if (!validateClickOnceFileData()) {return false;}"
                                                        OnClick="btnClickOnceFile_Click" ValidationGroup="SubmitClickOnceFile" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div style="width: 90%; margin: 0 auto 20px;" class="allGrid">
                                        <div align="center" class="panel-success pnl allGrid">
                                            <asp:Repeater ID="rptClickOnceFile" runat="server">
                                                <HeaderTemplate>
                                                    <table id="tblClickOnceFile" style="width: 95%; display: none" class="innerPnl">
                                                        <thead>
                                                            <tr class="tblHdr">
                                                                <th>Product Name</th>
                                                                <th>Version</th>
                                                                <th>IsForceUpdate</th>
                                                                <th>ServerIP</th>
                                                                <th>FilePath</th>
                                                                <th>Record_Created_By</th>

                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Eval("ProductName")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Version")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("IsForceUpdate")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("ServerIP")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("FilePath")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Record_Created_By")%>
                                                        </td>
                                                        
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody></table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div>
                                            <asp:Label ID="lblClickOnceGridMsg" runat="server" ForeColor="Red" Text="No Records Found.."></asp:Label>
                                        </div>
                                    </div>

                                </div>

                            </div>

                        </td>
                    </tr>
                </table>

            </body>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEditExcelVersion" />
            <asp:PostBackTrigger ControlID="btnDeleteExcelVersion" />
            <asp:PostBackTrigger ControlID="btnUpdateVersion" />
             <asp:PostBackTrigger ControlID="btnExcelProduct" />
            <asp:PostBackTrigger ControlID="btnClickOnceFile" />

        </Triggers>
    </asp:UpdatePanel>


    <script type="text/javascript">


        $(document).ready(function () {
            initiateFuntion();
        });

        function validateData() {

            var isInValidFile = 0;
            var validFile = false;
            document.getElementById("<%=lblErrorVersionName.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorFileName.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorFilePath.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorFileUpload.ClientID%>").innerHTML = "";

            var txtVersionValue = document.getElementById("<%=txtVersionName.ClientID%>").value;
            var txtFileNameValue = document.getElementById("<%=txtFileName.ClientID%>").value;
            var txtFilePathValue = document.getElementById("<%=txtFilePath.ClientID%>").value;

            if (txtVersionValue == "" || txtVersionValue == "undefined") {
                document.getElementById("<%=lblErrorVersionName.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }
            if (txtFileNameValue == "" || txtFileNameValue == "undefined") {
                document.getElementById("<%=lblErrorFileName.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }
            if (txtFilePathValue == "" || txtFilePathValue == "undefined") {
                document.getElementById("<%=lblErrorFilePath.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }


            var file = document.getElementById("<%=fileUploadExe.ClientID%>");

            var path = file.value;
            var validFilesTypes = ["exe"];
            if (path == '' || path == 'undefined') {
                document.getElementById("<%=lblErrorFileUpload.ClientID%>").innerHTML = "* Please select valid file type";
                isInValidFile = isInValidFile + 1;
            }
            else {
                var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                for (var i = 0; i < validFilesTypes.length; i++) {
                    if (ext == validFilesTypes[i]) {
                        validFile = true;
                        break;
                    }
                }
            }
            document.getElementById("version").setAttribute("class", "active");


            if (isInValidFile == 0 && validFile) {
                return true;
            }
            else {
                if (!validFile)
                    document.getElementById("<%=lblErrorFileUpload.ClientID%>").innerHTML = "* Please select valid file type.";
                return false;
            }
        }


        // Abhishek code

        function validateExcelProduce() {

            var isInValidFile = 0;
            var validFile = false;

            document.getElementById("<%=lblErrorExcelProductName.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorExcelGroupName.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorExcelPriority.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorExcelInstalledName.ClientID%>").innerHTML = "";
            
            

            var txtExcelProductNameValue = document.getElementById("<%=txtExcelProductName.ClientID%>").value;
            var txtExcelGroupNameValue = document.getElementById("<%=ddExcelGroupName.ClientID%>").value;
            var txtExcelPriorityValue = document.getElementById("<%=ddExcelPriority.ClientID%>").value;
            var txtExcelInstalledNameValue = document.getElementById("<%=txtExcelInstalledName.ClientID%>").value;

            if (txtExcelProductNameValue == "" || txtExcelProductNameValue == "undefined") {

                document.getElementById("<%=lblErrorExcelProductName.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;

            }

            if (txtExcelProductNameValue.indexOf(' ') >= 0) {

                document.getElementById("<%=lblErrorExcelProductName.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
                // string is not empty and not just whitespace
            }

            if (txtExcelGroupNameValue == "--Select--" || txtExcelGroupNameValue == "0") {
            document.getElementById("<%=lblErrorExcelGroupName.ClientID%>").innerHTML = "* Required";
            isInValidFile = isInValidFile + 1;
            }
            if (txtExcelPriorityValue == "--Select--" || txtExcelPriorityValue == "0") {
                document.getElementById("<%=lblErrorExcelPriority.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }
            if (txtExcelInstalledNameValue == "" || txtExcelInstalledNameValue == "undefined") {
                document.getElementById("<%=lblErrorExcelInstalledName.ClientID%>").innerHTML = "* Required";
                 isInValidFile = isInValidFile + 1;
             }

            document.getElementById("products").setAttribute("class", "active");

            if (isInValidFile == 0) {
                return true;
            }

            return false;
        }

        //**************************************


        function validateClickOnceFileData() {
            var isInValidFile = 0;
            var validFile = false;
            document.getElementById("<%=lblErrorProduct.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorProductVersion.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorProductPath.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorProductFileUpload.ClientID%>").innerHTML = "";
            document.getElementById("<%=lblErrorServerIP.ClientID%>").innerHTML = "";

            var ddlProductValue = document.getElementById("<%=ddlProduct.ClientID%>").value;
            var txtProductVersionValue = document.getElementById("<%=txtProductVersion.ClientID%>").value;
            var txtProductPathValue = document.getElementById("<%=txtProductPath.ClientID%>").value;
            var txtServerIPValue = document.getElementById("<%=txtServerIP.ClientID%>").value;

            if (ddlProductValue == "" || ddlProductValue == "undefined" || ddlProductValue == "0") {
                document.getElementById("<%=lblErrorProduct.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }
            if (txtProductVersionValue == "" || txtProductVersionValue == "undefined" || txtProductVersionValue.length != 8) {
                document.getElementById("<%=lblErrorProductVersion.ClientID%>").innerHTML = "* Required";
                
                isInValidFile = isInValidFile + 1;
            }

            var numbers = /^[0-9]+$/;

            if (!txtProductVersionValue.match(numbers)) {
                document.getElementById("<%=lblErrorExcelProductName.ClientID%>").innerHTML = "* Invalid Product Version";
                isInValidFile = isInValidFile + 1;
            }

            if (txtServerIPValue == "" || txtServerIPValue == "undefined") {
                document.getElementById("<%=lblErrorServerIP.ClientID%>").innerHTML = "* Required";

                  isInValidFile = isInValidFile + 1;
              }

            if (txtProductPathValue == "" || txtProductPathValue == "undefined") {
                document.getElementById("<%=lblErrorProductPath.ClientID%>").innerHTML = "* Required";
                isInValidFile = isInValidFile + 1;
            }

            //if (!txtProductVersionValue.match(/bd{1,3}.d{1,3}.d{1,3}.d{1,3}b/)) {
            //  document.getElementById("<%=lblErrorProductVersion.ClientID%>").innerHTML = "* Invalid Version";
            //isInValidFile = isInValidFile + 1;
            //}

            var file = document.getElementById("<%=prodFileUploadExe.ClientID%>");

            var path = file.value;
            var validFilesTypes = ["exe"];
            if (path == "" || path == 'undefined') {
                document.getElementById("<%=lblErrorProductFileUpload.ClientID%>").innerHTML = "* Please select valid file type";
                isInValidFile = isInValidFile + 1;
            }
            else {
                var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                for (var i = 0; i < validFilesTypes.length; i++) {
                    if (ext == validFilesTypes[i]) {
                        validFile = true;
                        break;
                    }
                }
            }
            document.getElementById("excel").setAttribute("class", "active");


            if (isInValidFile == 0 && validFile) {
                return true;
            }
            else {
                if (!validFile)
                    document.getElementById("<%=lblErrorProductFileUpload.ClientID%>").innerHTML = "* Please select valid file type.";
                return false;
            }
        }




        function clearErrorVersionName() {
            document.getElementById("<%=lblErrorVersionName.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
        }

        function checkVersionName() {
            var txtVersionValue = document.getElementById("<%=txtVersionName.ClientID%>").value;
            if (txtVersionValue == "" || txtVersionValue == "undefined") {
                document.getElementById("<%=lblErrorVersionName.ClientID%>").innerHTML = "* Required";
            }
        }

        function clearErrorFileName() {
            document.getElementById("<%=lblErrorFileName.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
        }

        function checkFileName() {
            var txtFileValue = document.getElementById("<%=txtFileName.ClientID%>").value;
            if (txtFileValue == "" || txtFileValue == "undefined") {
                document.getElementById("<%=lblErrorFileName.ClientID%>").innerHTML = "* Required";
            }
        }


        function clearErrorFilePath() {
            document.getElementById("<%=lblErrorFilePath.ClientID%>").innerHTML = "*";
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
        }

        function checkFilePath() {
            var txtFilePath = document.getElementById("<%=txtFilePath.ClientID%>").value;
            if (txtFilePath == "" || txtFilePath == "undefined") {
                document.getElementById("<%=lblErrorFilePath.ClientID%>").innerHTML = "* Required";
            }
        }

        function checkFile() {
            document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
            var file = document.getElementById("<%=fileUploadExe.ClientID%>");

            var path = file.value;
            var validFilesTypes = ["exe"];
            if (path == '' || path == 'undefined') {
                document.getElementById("<%=lblErrorFileUpload.ClientID%>").innerHTML = "* Please select valid file type";
                    isInValidFile = isInValidFile + 1;
                }
                else {
                    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                    for (var i = 0; i < validFilesTypes.length; i++) {
                        if (ext == validFilesTypes[i]) {
                            document.getElementById("<%=lblErrorFileUpload.ClientID%>").innerHTML = "*";
                            break;
                        }
                    }
                }

            }

            function checkFileExcel() {
                document.getElementById("<%=lblMsg.ClientID%>").innerHTML = "";
            var file = document.getElementById("<%=prodFileUploadExe.ClientID%>");

            var path = file.value;
            var validFilesTypes = ["exe"];
            if (path == '' || path == 'undefined') {
                document.getElementById("<%=lblErrorProductFileUpload.ClientID%>").innerHTML = "* Please select valid file type";
                  isInValidFile = isInValidFile + 1;
              }
              else {
                  var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();

                  for (var i = 0; i < validFilesTypes.length; i++) {
                      if (ext == validFilesTypes[i]) {
                          document.getElementById("<%=lblErrorProductFileUpload.ClientID%>").innerHTML = "*";
                            break;
                        }
                    }
                }

            }

            function initiateFuntion() {

            }

            $('#excelTab a[href="#update"]').click(function (e) {
                e.preventDefault();
                $("#excelTab").removeClass("active");
                $(this).addClass('active');
                document.getElementById('<%=hidTAB.ClientID %>').value = "update";
                $(this).tab('show');
            });

            $('#excelTab a[href="#version"]').click(function (e) {
                $("#excelTab").removeClass("active");
                $(this).addClass('active');
                document.getElementById('<%=hidTAB.ClientID %>').value = "version";
                $(this).tab('show');
            });

            function SelectExcelPackageTab() {
                $("#update").removeClass("active");
                $("#liupdate").removeClass("active");
                $("#excel").removeClass('active');
                $("#liexcel").removeClass('active');
                $("#products").removeClass('active');
                $("#liproducts").removeClass('active');

                $("#version").addClass('active');
                $("#liversion").addClass('active');
                var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
                $('#excelTab a[href="' + tab + '"]').tab('show');
            }

        // Abhishek Code 

        function SelectExcelProductTab() {
            $("#update").removeClass("active");
            $("#liupdate").removeClass("active");
            $("#version").removeClass('active');
            $("#liversion").removeClass('active');
            $("#excel").removeClass('active');
            $("#liexcel").removeClass('active');

            $("#products").addClass('active');
            $("#liproducts").addClass('active');
            var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
            $('#excelTab a[href="' + tab + '"]').tab('show');

        }

        //**********************

        function SelectClickOnceTab() {
            $("#update").removeClass("active");
            $("#liupdate").removeClass("active");
            $("#version").removeClass('active');
            $("#liversion").removeClass('active');
            $("#products").removeClass('active');
            $("#liproducts").removeClass('active');

            $("#excel").addClass('active');
            $("#liexcel").addClass('active');

            var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
                $('#excelTab a[href="' + tab + '"]').tab('show');

            }

    </script>

</asp:Content>
