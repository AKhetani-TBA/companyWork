<%@ Page Title="" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="UserLoginDetails.aspx.cs" Inherits="Administration.UserLoginDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
  
    <script src="js/jquery.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/moment.js"></script>
    <script src="plugins/datatable/jquery.dataTables.js"></script>
    <script src="js/dataTables.bootstrap.js" type="text/javascript"></script>
    <script src="js/daterangepicker.js"></script>
    <link href="css/daterangepicker-bs2.css" rel="stylesheet" />
    <script src="js/jquery.multiple.select.js"></script>
    <link href="css/multiple-select.css" rel="stylesheet" />
    <link href="css/AdminLTE.css" rel="stylesheet" type="text/css" />
    <link href="css/StyleSheet.css" rel="stylesheet" />

 
    <script>
        var parsed_data;
        var status;
        $(document).ready(function () {
            setDateRange(7);
            status = 1;
            $('#btnSearch').click(function () {
                debugger;
                $('#divRecentUser').show();
                var startDate = $("input[name='daterangepicker_start']").val();
                var endDate = $("input[name='daterangepicker_end']").val();
                UserDetails($('#ddlServerName').val(), startDate, endDate);
            });


        });

        function setDateRange(duration) {
            var cb = function (start, end, label) {
                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            }
            var optionSet2 = {
                timePicker: true,
                timePicker12Hour: false,
                timePickerIncrement: 1,
                format: 'MMM/DD/YYYY HH:mm:ss ',
                startDate: moment().subtract('days', duration),
                endDate: moment(),
                opens: 'left',
                ranges: {
                    'last 24 hours': [moment().subtract('days', 1), moment()],
                    'Last 7 Days': [moment().subtract('days', 6), moment()],
                    'Last 15 Days': [moment().subtract('days', 14), moment()],
                    'Last 30 Days': [moment().subtract('days', 29), moment()]
                }
            }
            $('#reportrange span').html(moment().subtract('days', duration).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
            $('#reportrange').daterangepicker(optionSet2, cb);

            var startDateTime = $("input[name='daterangepicker_start']").val().split(' ')[0] + " 00:00:00";
            $("input[name='daterangepicker_start']").val(startDateTime);
        }

        function UserDetails(ServerId, FromDate, ToDate) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "UserLoginDetails.aspx/GetUserLoginDetails",
                data: "{'ServerId':'" + ServerId + "','FromDate':'" + FromDate + "','ToDate':'" + ToDate + "'}",
                dataType: "json",
                success: function (data) {

                    $('#sessionSummaryTbl').empty();

                    parsed_data = $.parseJSON(data.d);

                    //$.each(parsed_data, function (key, value) {
                    //    debugger;
                    //    parsed_data[key].LogInTime = moment(parsed_data[key].LogInTime).format('DD-MM-YYYY HH:MM:SS');
                    //});

                    debugger;
                    if (status == 1) {

                        $('#sessionSummaryTbl').dataTable({
                            data: parsed_data,
                            columns: [
                                { data: 'LogInTime', title: 'Log In Time' },
                                { data: 'Client', title: 'Client' },
                                { data: 'UserName', title: 'UserName' },
                                { data: 'FirstName', title: 'First Name' },
                                { data: 'LastName', title: 'Last Name' },
                                { data: 'CustomerId', title: 'Customer Id' },
                                { data: 'CustomerName', title: 'Customer Name' },
                                { data: 'AuthToken', title: 'AuthToken' },
                                { data: 'LauncherToken', title: 'Launcher Token' },
                                { data: 'LoginID', title: 'LoginID' },
                                { data: 'Isvalid', title: 'A' },
                                { data: 'Validationflag', title: 'B' },
                                { data: 'LoginTypeId', title: 'C' },
                                { data: 'IsTrumidAdmin', title: 'D' },
                                { data: 'IsStateStreetAdmin', title: 'E' },
                                { data: 'IsbeastAdmin', title: 'F' },
                                { data: 'IsFirmBlotterAccess', title: 'G' },
                                { data: 'MustChangePassword', title: 'H' },
                                { data: 'RetryCount', title: 'I' },
                                { data: 'LauncherVersion', title: 'J' },
                             // { data: 'LastActivityDate', title: 'Last Activity Date' },
                             // { data: 'OtherDetail' }
                            ],
                            "bRetrieve": true,
                            "bProcessing": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bAutoWidth": false,
                            "bFilter": true,
                            "bInfo": false,
                            "aaSorting": [[0, 'desc']],
                            "iDisplayLength": 100,
                            "aLengthMenu": [[50, 100, 200, 300, 400, 500], ["50", "100", "200", "300", "400", "500"]]
                            // "bJQueryUI": false
                        });
                        status = 0;
                    }
                    else {
                        $('#sessionSummaryTbl').dataTable().fnDestroy();

                        $('#sessionSummaryTbl').dataTable({
                            data: parsed_data,
                            columns: [
                                { data: 'LogInTime', title: 'Log In Time' },
                                { data: 'Client', title: 'Client' },
                                { data: 'UserName', title: 'UserName' },
                                { data: 'FirstName', title: 'First Name' },
                                { data: 'LastName', title: 'Last Name' },
                                { data: 'CustomerId', title: 'Customer Id' },
                                { data: 'CustomerName', title: 'Customer Name' },
                                { data: 'AuthToken', title: 'AuthToken' },
                                { data: 'LauncherToken', title: 'Launcher Token' },
                                { data: 'LoginID', title: 'LoginID' },
                                { data: 'Isvalid', title: 'A' },
                                { data: 'Validationflag', title: 'B' },
                                { data: 'LoginTypeId', title: 'C' },
                                { data: 'IsTrumidAdmin', title: 'D' },
                                { data: 'IsStateStreetAdmin', title: 'E' },
                                { data: 'IsbeastAdmin', title: 'F' },
                                { data: 'IsFirmBlotterAccess', title: 'G' },
                                { data: 'MustChangePassword', title: 'H' },
                                { data: 'RetryCount', title: 'I' },
                                { data: 'LauncherVersion', title: 'J' },
                             // { data: 'LastActivityDate', title: 'Last Activity Date' },
                             // { data: 'OtherDetail' }
                            ],
                            "bRetrieve": true,
                            "bProcessing": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bAutoWidth": false,
                            "bFilter": true,
                            "bInfo": false,
                            "aaSorting": [[0, 'desc']],
                            "iDisplayLength": 100,
                            "aLengthMenu": [[50, 100, 200, 300, 400, 500], ["50", "100", "200", "300", "400", "500"]]
                            //"bJQueryUI": false
                        });
                    }
                },
                error: function (result) {
                    alert("Error");
                }
            });
        }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <p></p>
 <section class="content">
        <div class="cf">
            <div class="col-md-12">
                <table style="width: 100%;text-align:center;">
                    <tbody>
                        <tr>
                            <td style="color: #0073ea; font-weight: bold">Server :</td>
                            <td>
                                <select style="width: 170px" id="ddlServerName">
                                    <option value="1">UAT</option>
                                    <option value="2">Demo</option>
                                    <option value="3">Production</option>
                                </select>
                            </td>
                            <td style="color: #0073ea; font-weight: bold; text-align: right;">Date Range:</td>
                            <td>
                                <div id="reportrange" class="pull-right" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc">
                                    <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>
                                    <span></span><b class="caret"></b>
                                </div>
                            </td>

                            <td>&nbsp<input id="btnSearch" type="button" value="Search" class="btn btn-primary" style="height: 28px! important; padding: 3px 12px" />
                            </td>
                            <td>
                                <table border="1">
                                    <thead>
                                        <tr>
                                            <th>Title </th>
                                            <th>Column Name </th>
                                            <th>Title </th>
                                            <th>Column Name </th>
                                            <th>Title </th>
                                            <th>Column Name </th>
                                            <th>Title </th>
                                            <th>Column Name </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>A </td>
                                            <td>Isvalid </td>
                                            <td>B </td>
                                            <td>Validationflag </td>
                                            <td>C </td>
                                            <td>LoginTypeId </td>
                                            <td>D </td>
                                            <td>IsTrumidAdmin </td>
                                        </tr>
                                        <tr>

                                            <td>E </td>
                                            <td>IsStateStreetAdmin </td>
                                            <td>F </td>
                                            <td>IsbeastAdmin </td>

                                            <td>G </td>
                                            <td>IsFirmBlotterAccess </td>
                                            <td>H </td>
                                            <td>MustChangePassword </td>
                                        </tr>
                                        <tr>
                                            <td>I </td>
                                            <td>RetryCount </td>
                                            <td>J </td>
                                            <td>LauncherVersion </td>

                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box-body table-responsive" id="divRecentUser" style="overflow-x: scroll; display: none;">
                    <table id="sessionSummaryTbl" class="table table-bordered table-hover" style="width: 2200px!important">
                    </table>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
