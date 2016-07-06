<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="True" CodeBehind="BeastSysLog.aspx.cs" Inherits="Administration.BeastSysLog" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>


    <link href="css/AdminLTE.css" rel="stylesheet" type="text/css" />

    <link href="css/StyleSheet.css" rel="stylesheet" />


    <%--<script src="js/app.js" type="text/javascript"></script>--%>
    <script src="js/jquery-ui.js"></script>
    <script src="js/moment.js"></script>
    <%--<script src="plugins/datatable/jquery.dataTables.js"></script>--%>
    <script src="js/dataTables.bootstrap.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js"></script>
    <link href="css/daterangepicker-bs2.css" rel="stylesheet" />
    <script src="js/jquery.multiple.select.js"></script>
    <link href="css/multiple-select.css" rel="stylesheet" />
    <%--<script src="js/chart/highcharts.js" type="text/javascript"></script>--%>
    <%--<script src="http://code.highcharts.com/stock/highstock.js"></script>
    <script src="http://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="js/chart/chartCreateAndUpdate.js" type="text/javascript"></script>--%>
    <script src="js/BeastSysLog.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="loading" style="display: none; width: 69px; height: 80px; position: absolute; top: 30%; left: 50%; padding: 2px; z-index: 2;">
        <img src='images/loader-1.gif' width="64" height="64" />
    </div>   
    <style>
        .dataTables_info {
            display: none;
        }

        .no-print {
            display: none;
        }

        .pace .pace-progress {
            background-color: yellow;
        }
    </style>
    <div>

        <div class="wrapper row-offcanvas row-offcanvas-left">
            <aside class="right-side strech">
                <!-- Content Header (Page header) -->
                <!-- Main content -->
                <section class="content" style="padding-top: 0px;">
                    <div class="row">
                        <div id="beastLogs" class="beastLogs">
                            <ul>
                                <li onclick="clickMenuTab('1')"><a href="#beastLogs-1">User Connection</a></li>
                                <li onclick="clickMenuTab('2')"><a href="#beastLogs-2">Framework Admin Apps Log</a></li>
                                <li onclick="clickMenuTab('3')"><a href="#beastLogs-3">Images</a></li>
                                <li onclick="clickMenuTab('4')"><a href="#beastLogs-4">User Created Images</a></li>
                                <%--<li onclick="clickMenuTab('5')"><a href="#beastLogs-5">SIF</a></li>--%>
                                <li onclick="clickMenuTab('6')"><a href="#beastLogs-6">All Logs</a></li>
                                <span style="float: right; padding: 5px;"><b>Beast System Log</b></span>
                            </ul>
                            <div class="calendarSearch">
                                <input type="hidden" id="selectedTab" />
                                <div id="searchAllLogTbl" class="allLogTbl" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="width: 20%;">
                                                <select id="ddlHostAllLogs" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 30%;">
                                                <select id="ddlApplicationAllLogs" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 30%;">
                                                <input placeholder="Application Description Like" class="txtBox" type="text" id="txtApplicationAllLogs" style="height: 18px !important; font-size: 12px;" /></td>
                                            <td style="width: 20%;">
                                                <span>Page Size: </span>
                                                <select id="ddlAllLogPageSize" style="width: 50%;">
                                                    <%--<option value="50">50</option>
                                                    <option value="100">100</option>
                                                    <option value="150">150</option>
                                                    <option value="200">200</option>
                                                    <option value="250">250</option>--%>
                                                </select></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="searchUserImagesTbl" class="allLogTbl" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="width: 20%;">
                                                <select id="ddlUIServer" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 18%;">
                                                <select id="ddlUISeverity" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 25%;">
                                                <select id="ddlUIApplication" style="width: 100%;">
                                                </select></td>
                                            <%--<td style="width:20%;">
                                                <select id="ddlUISID" style="width: 100%;">
                                                </select></td>--%>
                                            <td style="width: 7%;">
                                                <input placeholder="User Id" type="text" onkeyup="if (/\D/g.test(this.value)) this.value = this.value.replace(/\D/g,'')" class="txtBox" id="txtUIUserId" style="height: 18px !important; font-size: 12px; width: 60px;" /></td>
                                            <td style="width: 10%;">
                                                <input placeholder="Description" class="txtBox" type="text" id="txtUIDesc" style="height: 18px !important; font-size: 12px; width: 100px;" /></td>
                                            <td style="width: 20%;">
                                                <span>Page Size: </span>
                                                <select id="ddlUIPageSize" style="width: 50%;">
                                                    <%--<option value="50">50</option>
                                                    <option value="100">100</option>
                                                    <option value="150">150</option>
                                                    <option value="200">200</option>
                                                    <option value="250">250</option>--%>
                                                </select></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="searchImagesTbl" class="allLogTbl" style="display: none;">
                                    <table>
                                        <tr>
                                            <td style="width: 20%;">
                                                <select id="ddlImageServer" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 20%;">
                                                <select id="ddlImageEvent" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 35%;">
                                                <select id="ddlImageSID" style="width: 100%;">
                                                </select></td>
                                            <td style="width: 5%;">
                                                <input placeholder="User Id" type="text" onkeyup="if (/\D/g.test(this.value)) this.value = this.value.replace(/\D/g,'')" class="txtBox" id="txtImageUserId" style="height: 18px !important; font-size: 12px; width: 60px;" /></td>
                                            <td style="width: 20%;">
                                                <span>Page Size: </span>
                                                <select id="ddlImagePageSize" style="width: 50%;">
                                                    <%--<option value="50">50</option>
                                                    <option value="100">100</option>
                                                    <option value="150">150</option>
                                                    <option value="200">200</option>
                                                    <option value="250">250</option>--%>
                                                </select></td>
                                        </tr>
                                    </table>
                                </div>
                                <input id="btnFilter" type="button" value="Search" class="btn btn-primary pull-right" style="height: 28px! important; padding: 3px 12px; float: right; margin-top: 5px; margin-left: 8px;" />
                                <div id="reportrange" class="pull-right" style="margin-top: 5px; background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; font-size: 12px;">
                                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>
                                    <span></span><b class="caret"></b>
                                </div>

                            </div>

                            <div id="beastLogs-1">
                                <%--<input type="hidden" id="hdnUCStartDate" />
                                <input type="hidden" id="hdnUCEndDate" />--%>
                                <div class="box">
                                    <%--<div class="box-header">
                                            <h4 class="box-title">All Users</h4>
                                        </div>--%>
                                    <!-- /.box-header -->
                                    <div class="box-body table-responsive" id="divRecentUser">
                                        <%--<div id="userConnDateRange" class="searchHldr">
                                            <input id="btnFilterUserConn" type="button" value="Search" class="btn btn-primary pull-right" style="height: 28px! important; padding: 3px 12px; float: right;" />
                                            <div id="reportrange" class="pull-right" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                                                <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>
                                                <span></span><b class="caret"></b>
                                            </div>
                                        </div>--%>
                                        <table id="sessionSummaryTbl" class="sessionSummaryTbl table table-bordered table-hover beastSessionSummary">
                                        </table>
                                    </div>
                                </div>
                                <%--<div id="divChart">
                                </div>--%>
                            </div>
                            <div id="beastLogs-2">
                                <div class="box">
                                    <%--<div class="box-header">
                                            <h4 class="box-title">Most Used Apps</h4>
                                        </div>--%>
                                    <!-- /.box-header -->
                                    <div class="box-body table-responsive">

                                        <div class="searchHldr">
                                            <table>
                                                <%--  <tr>
                                                    <td class="bigSelectBox"><span>Servers:</span></td>
                                                    <td class="bigSelectBox"><span>On Applications:</span></td>
                                                    <td class="bigSelectBox"><span>Events:</span></td>
                                                    <td class="bigSelectBox"><span>Applications:</span></td>
                                                    <td class="btnBox">&nbsp;</td>
                                                </tr>--%>
                                                <tr>
                                                    <td class="mediumSelectBox">
                                                        <select name="tech" id="ddlServer"></select></td>
                                                    <td class="mediumSelectBox">
                                                        <select onchange="BindApplicationOnChange()" name="tech" id="ddlOnApplication">
                                                            <%--<option value="%">-- On Application --</option>--%>
                                                        </select></td>
                                                    <td class="mediumSelectBox">
                                                        <select name="tech" id="ddlEvent"></select></td>
                                                    <td class="mediumSelectBox">
                                                        <select name="tech" id="ddlApplication"></select></td>
                                                    <td class="smallSelectBox">
                                                        <span>Page Size: </span>
                                                        <select id="ddlAdminAppPageSize" style="width: 50%;">
                                                            <%--<option value="50">50</option>
                                                            <option value="100">100</option>
                                                            <option value="150">150</option>
                                                            <option value="200">200</option>
                                                            <option value="250">250</option>--%>
                                                        </select></td>
                                                    <td class="btnBox">
                                                        <input id="btnFilterFramework" type="button" value="Search" class="btn btn-primary pull-right" style="height: 28px! important; padding: 3px 12px; float: right;" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table id="adminAppTbl" class="adminAppTbl table table-bordered table-hover beastSessionSummary">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="beastLogs-3">
                                <div class="box">
                                    <div class="box-body table-responsive" id="divImages">
                                        <div class="boxImagesTbl">
                                            <table id="imagesTbl" class="imagesTbl table table-bordered table-hover beastSessionSummary"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="beastLogs-4">
                                <div class="box">
                                    <div class="box-body table-responsive" id="divUserImages">
                                        <div class="boxUserImagesTbl">
                                            <table id="userImagesTbl" class="userImagesTbl table table-bordered table-hover beastSessionSummary"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%-- <div id="beastLogs-5">
                                <p>
                                    <h2>Tab 5</h2>
                                </p>
                            </div>--%>
                            <div id="beastLogs-6">
                                <div class="box">
                                    <!-- /.box-header -->
                                    <div class="box-body table-responsive" id="divAllLogs">
                                        <div class="boxAllLogSummaryTbl">
                                            <table id="allLogSummaryTbl" class="allLogSummaryTbl table table-bordered table-hover beastSessionSummary"></table>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </section>
                <!-- /.content -->
            </aside>
            <!-- /.right-side -->
        </div>
    </div>
</asp:Content>
