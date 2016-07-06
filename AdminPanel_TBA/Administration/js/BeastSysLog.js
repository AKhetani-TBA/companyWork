$(document).ready(function () {
    $('html, body').attr('style', 'overflow-x: initial !important;');
    $('#txtUIUserId').blur(function (e) {
        if (/\D/g.test(this.value)) {
            this.value = this.value.replace(/\D/g, '');
        }
    });
    $("#beastLogs").tabs();
    BindValues();


    //BindValuesForFrameworkApp();
    //BindValuesForAllLogs();
    //BindValuesForUserImages();
    //BindValuesForImages();
    $('#selectedTab').val("1");
    setDateRange(7);
    //$('#hdnUCStartDate').val($("input[name='daterangepicker_start']").val());
    //$('#hdnUCEndDate').val($("input[name='daterangepicker_end']").val());
    MostUsedUsers($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), "0");

    $('#btnFilter').click(function () {
        var id = $('#selectedTab').val();
        switch (id) {
            case "1":
                MostUsedUsers($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), "0");
                break;
                //case 2:
                //    text = "Today is Sunday";
                //    break;
            case "3":
                SearchImages();
                break;
            case "4":
                SearchUserImages();
                break;
                //case 5:
                //    text = "Today is Sunday";
                //    break;
            case "6":
                SearchAllLogs();
                break;
        }
        //if ($('#selectedTab').val() == "1") {
        //    MostUsedUsers($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), "0");
        //}
        //else if ($('#selectedTab').val() == "6") {
        //    SearchAllLogs();
        //}
        //else if ($('#selectedTab').val() == "4") {
        //    SearchUserImages();
        //}
        //else if ($('#selectedTab').val() == "3") {
        //    SearchImages();
        //}
    });

    $('#btnFilterFramework').click(function () {
        SearchAdminAppLog();
    });
});

$(document).ajaxStart(function () {
    $("#loading").css("display", "block");
});

$(document).ajaxComplete(function () {
    $("#loading").css("display", "none");
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
    //var c = $("input[name='daterangepicker_start']").val();
    var startDateTime = $("input[name='daterangepicker_start']").val().split(' ')[0] + " 00:00:00";
    $("input[name='daterangepicker_start']").val(startDateTime);

    //$("input[name='daterangepicker_start']").text(startDateTime);
    //var c = $("input[name='daterangepicker_start']").val();

}
function clickMenuTab(id) {
    $("#loading").css("display", "none");
    $('#selectedTab').val(id);
    $(".calendarSearch").css("display", "block");


    switch (id) {
        case "1":
            //$("input[name='daterangepicker_start']").val($('#hdnUCStartDate').val());
            //$("input[name='daterangepicker_end']").val($('#hdnUCEndDate').val());
            setDateRange(7);
            $("#searchAllLogTbl").css("display", "none");
            $("#searchUserImagesTbl").css("display", "none");
            $("#searchImagesTbl").css("display", "none");
            break;
            //case 2:
            //    text = "Today is Sunday";
            //    break;
        case "3":
            setDateRange(0);
            $("#searchAllLogTbl").css("display", "none");
            $("#searchUserImagesTbl").css("display", "none");
            $("#searchImagesTbl").css("display", "block");
            break;
        case "4":
            setDateRange(0);
            $("#searchAllLogTbl").css("display", "none");
            $("#searchImagesTbl").css("display", "none");
            $("#searchUserImagesTbl").css("display", "block");
            break;
            //case 5:
            //    text = "Today is Sunday";
            //    break;
        case "6":
            setDateRange(0);
            $("#searchUserImagesTbl").css("display", "none");
            $("#searchImagesTbl").css("display", "none");
            $("#searchAllLogTbl").css("display", "block");
            break;
        default:
            $(".calendarSearch").css("display", "none");
    }



    //if (id == 1 || id == 6 || id == 4 || id == 3) {
    //    $(".calendarSearch").css("display", "block");
    //    if (id == 1) {
    //        $("#searchAllLogTbl").css("display", "none");
    //        $("#searchUserImagesTbl").css("display", "none");
    //        $("#searchImagesTbl").css("display", "none");
    //    }
    //    else if (id == 6) {
    //        setDateRange(0);
    //        $("#searchUserImagesTbl").css("display", "none");
    //        $("#searchImagesTbl").css("display", "none");
    //        $("#searchAllLogTbl").css("display", "block");
    //        //SearchAllLogs();
    //    }
    //    else if (id == 4) {
    //        setDateRange(0);
    //        $("#searchAllLogTbl").css("display", "none");
    //        $("#searchImagesTbl").css("display", "none");
    //        $("#searchUserImagesTbl").css("display", "block");
    //        //SearchUserImages();
    //    }
    //    else if (id == 3) {
    //        setDateRange(0);
    //        $("#searchAllLogTbl").css("display", "none");
    //        $("#searchUserImagesTbl").css("display", "none");
    //        $("#searchImagesTbl").css("display", "block");
    //    }
    //}
    //else { $(".calendarSearch").css("display", "none"); }
}
function BindValues() {
    $.ajax({
        url: "Service.asmx/BeastBindServers",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlServer, #ddlImageServer, #ddlUIServer").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlServer, #ddlImageServer, #ddlUIServer").multipleSelect({
                filter: true,
                placeholder: "Servers"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindApplicationTypes",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlOnApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            //BindApplicationOnChange();
            $("#ddlEvent").multipleSelect({
                filter: true,
                placeholder: "Events"
            });
            $("#ddlApplication").multipleSelect({
                filter: true,
                placeholder: "Applications"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindImageEvents",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlImageEvent").append($("<option></option>").text(jsondata.EventName).val(jsondata.Id));
            });
            $("#ddlImageEvent").multipleSelect({
                filter: true,
                placeholder: "Events"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindSID",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlImageSID").append($("<option></option>").text(jsondata.Name).val(jsondata.SID));
            });
            $("#ddlImageSID").multipleSelect({
                filter: true,
                placeholder: "SIDs"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindSeverity",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlUISeverity").append($("<option></option>").text(jsondata.SeverityType).val(jsondata.Id));
            });
            $("#ddlUISeverity").multipleSelect({
                filter: true,
                placeholder: "Severity"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastUIApplication",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlUIApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            $("#ddlUIApplication").multipleSelect({
                filter: true,
                placeholder: "Application"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastAllLogAppList",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlApplicationAllLogs").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            $("#ddlApplicationAllLogs").multipleSelect({
                filter: true,
                placeholder: "Application"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastAllLogHostList",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlHostAllLogs").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlHostAllLogs").multipleSelect({
                filter: true,
                placeholder: "Source Host Name"
            });
        },
        error: function (request, status, error) {
        }
    })

    var i = 50;
    while (i <= 250) {        
        $('#ddlImagePageSize, #ddlUIPageSize, #ddlAllLogPageSize, #ddlAdminAppPageSize').append($('<option>', {
            value: i,
            text: i
        }));
        i=i+50;
    }
    $('#ddlOnApplication').append($('<option value="%">-- On Application --</option>'));
    

    $('#ddlImagePageSize').val(250);
    $('#ddlUIPageSize').val(250);
    $('#ddlAllLogPageSize').val(250);
    $('#ddlAdminAppPageSize').val(250);
}

function MostUsedUsers(FromDate, ToDate, flag) {
    pageno = 1;
    var ctrr = 1;
    var param = '{"FromDate":"' + FromDate + '","ToDate":"' + ToDate.trim() + '"}';
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: param,
        url: "BeastSysLog.aspx/BeastMostUsedUsers",
        //dataType: "jsonp",
        success: function (data) {
            //var tmpJsonData = data.d;
            var tableData = $.parseJSON(data.d);
            var summaryCtr = 1;

            $('#sessionSummaryTbl').empty();
            $('table.sessionSummaryTbl').append("<thead><tr> <th style='display:none' width='5%' align='center' valign='middle'></th> <th>Date Time</th><th>User Name</th><th>Operation</th><th style='text-align:right;'>Seen</th><th>To Server</th><th>From IP</th>    </tr></thead>  <tbody>   <tr id='1000000' class='sessionSummaryRow' style='display:none'><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            $.each(tableData, function () {
                var trClassSmry = "tr3";
                if (summaryCtr % 2 == 0)
                    trClassSmry = "tr4";
                else
                    trClassSmry = "tr3";

                var ctrrRow = summaryCtr + 'UserRow';


                var rol = 1;
                var over = 2;
                var lastseen = $.trim(this['DateTime']).replace(/ /gi, '|');

                var onclick = "onclick =getUserSessions('" + ctrrRow + "','" + $.trim(this['UserID']) + "','" + lastseen + "') ";
                //$('<tr onmouseover="mouseEvent(\'' + ctrrRow + '\', ' + rol + ', ' + summaryCtr + ')" onmouseout="mouseEvent(\'' + ctrrRow + '\', ' + over + ', ' + summaryCtr + ')" id="' + ctrrRow + '"  style="cursor:pointer"  ><td style="display:none"><  input type="checkbox" /> </td><td ' + onclick + '>' + $.trim(jsondata.UserName) + '</td><td ' + onclick + '>' + $.trim(jsondata.Seen) + '</td><td ' + onclick + '>' + $.trim(jsondata.LastSeen) + '</td></tr>').insertBefore('table.sessionSummaryTbl tr.sessionSummaryRow');
                $('<tr onmouseover="mouseEvent(\'' + ctrrRow + '\', ' + rol + ', ' + summaryCtr + ')" onmouseout="mouseEvent(\'' + ctrrRow + '\', ' + over + ', ' + summaryCtr + ')" id="' + ctrrRow + '"  style="cursor:pointer"  ><td style="display:none"><  input type="checkbox" /> </td><td ' + onclick + '>' + $.trim(this['DateTime']) + '</td><td ' + onclick + '>' + $.trim(this['UserName']) + '</td><td ' + onclick + '>' + $.trim(this['Operation']) + '</td><td ' + onclick + ' style="text-align:right;">' + $.trim(this['Seen']) + '</td><td ' + onclick + '>' + $.trim(this['ToServer']) + '</td><td ' + onclick + '>' + $.trim(this['FromIP']) + '</td></tr>').insertBefore('table.sessionSummaryTbl tr.sessionSummaryRow');

                summaryCtr++;

            });

            //chartCreateAndUpdate("divChart", data, 'column');

            $('#sessionSummaryTbl').dataTable({
                //"bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": true,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ]
            });

            if ($('#sessionSummaryTbl tbody tr').length == 1) {
                $('.dataTables_empty')[0].innerText = 'Your search did not match any activities.Please Refine your Search.';
                $('.dataTables_empty')[0].innerHTML = 'Your search did not match any activities.Please Refine your Search.';

            }

            //if (flag == "0" && $('#sessionSummaryTbl tbody tr').length == 1) {

            //}
            //else {
            //    var html = '<div class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#" >Next</a></li>             </ul>         </div>     </div></div>';
            //    $('#sessionSummaryTbl_wrapper').append(html);

            //    if ($('.wrapper #toppageing').length == 0) {
            //        html = '<div id="toppageing"  class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#">Next</a></li>             </ul>         </div>     </div></div>';

            //        $('#sessionSummaryTbl').before(html);

            //    }
            //}

            //$('.dataTables_paginate ul li a').on('click', function () {
            //    if ($(this)[0].innerHTML == 'Next') {
            //        pageno = parseInt(pageno) + 1;
            //    }
            //    if ($(this)[0].innerHTML == 'Previous') {
            //        pageno = parseInt(pageno) - 1;
            //        if (pageno == 0 || pageno < 0) {
            //            pageno = 1;

            //        }
            //    }

            //    MostUsedUsers($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), "1");
            //});
            //if ($('#sessionSummaryTbl tbody tr').length == 1) {
            //    $('.next').addClass('disabled');
            //}


        },

        error: function (request, status, error) {

            $('#btnSysShowMore').css({ visibility: "hidden" });
            //alert("No More SysLog Data Available");

        }
    });
}
function getUserSessions(RowID, UserID, LastSeen) {

    var divid = 'divDetailsnew' + UserID;
    var flag = LastSeen;
    var id = 'tmpTR' + RowID;


    if ($("#tmpTR" + RowID).length > 0 && flag != "0") {

        if ($("#tmpTR" + RowID).css('display') == 'none') {
            $('#tmpTR' + RowID).show('slow');
        }
        else {
            $('#tmpTR' + RowID).hide();
        }
    }
    else {

        var loading = 'loading' + UserID;
        var tblid = 'tblUserWiseSummary' + UserID;

        if (flag == "0") {
            LastSeen = $("#getsessionid" + UserID + ' :selected').val();
            $('#' + divid).remove();
            $('#loading' + UserID).show();
            var div = '<div  id="' + divid + '" style="width: 95%; float:right;"><div id="' + tblid + '_wrapper" class="dataTables_wrapper form-inline" role="grid"><table id="' + tblid + '" class="' + tblid + ' table table-bordered table-hover" width="100%" align="center" cellpadding="0" cellspacing="0"></table></div>';

            $(div).insertAfter('#' + loading + '');
        }
        else {

            var div = '<tr  id=' + id + ' style="background-color: #f5f5f5;" ><td border="0px" colspan="6" id="tmpTD">  <div align="center" id="' + loading + '" style="z-index: 100; position: relative;display:none"><img src="images/ajax-loader.gif" /></div><div id="' + divid + '" style="width: 100%;"><div id="' + tblid + '_wrapper" class="dataTables_wrapper form-inline" role="grid"><table id="' + tblid + '" class="' + tblid + ' table table-bordered table-hover" width="100%" align="center" cellpadding="0" cellspacing="0"></table></div></div></td></tr>';

            $(div).insertAfter('#' + RowID + '');
            $('#' + divid).show('slow');
            $('#' + divid).css({ 'width': '95%', 'float': 'right' });
        }

        populateSessionAverageForUser(RowID, UserID, LastSeen, flag);
    }
}
function populateSessionAverageForUser(RowID, UserID, LastSeen, ddl) {
    $('#tblUserWiseSummary' + UserID).empty();
    var param = '{"Userid":"' + UserID + '","LastSeen":"' + LastSeen.trim() + '"}';
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: param,
        url: "BeastSysLog.aspx/getBeastUserAllActivity",
        success: function (data) {
            debugger;
            var tableData = $.parseJSON(data.d);
            var flag = false;
            var summaryCtr = 1;
            var summaryCtr = 1;
            addSessionSummaryRowUser(UserID);

            $.each(tableData, function () {
                var trClassSmry = "";
                var Operation = $.trim(this['Operation']);
                if ($.trim(this['Operation']) == "User Login") {
                    trClassSmry = "success";
                }
                else if ($.trim(this['Operation']) == "Image Created") {
                    trClassSmry = "info";
                }
                else if ($.trim(this['Operation']) == "Open Image") {
                    trClassSmry = "active";
                }
                else if ($.trim(this['Operation']) == "Close Image") {
                    trClassSmry = "warning";
                }
                else if ($.trim(this['Operation']) == "Delete Image") {
                    trClassSmry = "danger";
                }
                else {
                    trClassSmry = "danger";
                }

                var tbleid = 'sessionSummaryRowUser' + UserID;
                $('<tr style="font-size:11px;" class=' + trClassSmry + '><td style="display:none"></td><td>' + $.trim(this['DateTime']) + ' <td>' + $.trim(this['SIF']) + '</td><td>' + $.trim(this['Operation']) + '</td></tr>').insertBefore('.' + tbleid);

                summaryCtr++;


            });
            $('#tblUserWiseSummary' + UserID).dataTable({
                //    "bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                "bSort": true

            });
            $('#loading' + UserID).hide();

            getSessionList(RowID, UserID, LastSeen, ddl, $("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val());


        },
        error: function (request, status, error) {
            $("#tblUserWiseSummary" + UserID).empty();
        }
    });
    //getSessionList(RowID, UserID, LastSeen, ddl, FromDate, ToDate);
}
function addSessionSummaryRowUser(UserID) {
    var tbleid = 'sessionSummaryRowUser' + UserID;
    $('#tblUserWiseSummary' + UserID).append('<thead><tr style="font-size:12px;"><th style="display:none"></th><th> Date Time (GMT)</th><th>SIF</th><th>Operation</th></tr></thead><tbody><tr id="99000" class="' + tbleid + '" style="display:none"><td></td><td></td><td></td><td></td></tr></tbody>');
}
function getSessionList(RowID, UserID, LastSeen, flag, FromDate, ToDate) {
    var param = '{"UserID":"' + UserID + '","FromDate":"' + FromDate.trim() + '","ToDate":"' + ToDate.trim() + '"}';
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: param,
        url: "BeastSysLog.aspx/GetBeastSessionList",
        success: function (data) {
            debugger;
            var tableData = $.parseJSON(data.d);
            var id = "getsessionid" + UserID;

            var onChange = "onChange =getUserSessions('" + RowID + "','" + UserID + "','" + '0' + "') ";
            $('#tblUserWiseSummary' + UserID + '_wrapper').prepend('<div class="row"> <div class="col-xs-6"></div><div>');
            $('#tblUserWiseSummary' + UserID + '_wrapper .row .col-xs-6')[0].outerHTML = '<div class="col-xs-6"><label> Activities By: <select ' + onChange + ' class="getsessionid" id=' + id + ' ></select></label></div>';

            $.each(tableData, function () {

                $("#" + id).append($("<option></option>").text(this['DateTime']).val($.trim(this['DateTime'])));
                $("#" + id).val($.trim(this['DateTime']));
            })
            if (LastSeen != '' && flag == "0") {
                $("#" + id).val($.trim(LastSeen));
            }
        }
    });
}

function SearchAllLogs() {
    var hostList = '';
    var appList = '';
    var appDesc = '';
    var allLogPageno = 1;
    var allLogPagesize = $("#ddlAllLogPageSize" + ' :selected').val();
    if ($('#ddlHostAllLogs').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        hostList = '';
    }
    else {
        $('#ddlHostAllLogs').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlHostAllLogs').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (hostList == '') {
                hostList = $(this).find('label').find('input').attr('value');
            }
            else {
                hostList = hostList + ',' + $(this).find('label').find('input').attr('value');
            }
        });
    }

    if ($('#ddlApplicationAllLogs').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        appList = '';
    }
    else {
        $('#ddlApplicationAllLogs').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlApplicationAllLogs').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (appList == '') {
                appList = $(this).find('label').find('input').attr('value');
            }
            else {
                appList = appList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    if ($("#txtApplicationAllLogs").val() != '') {
        appDesc = $("#txtApplicationAllLogs").val();
    }
    DisplayAllLogs($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), hostList, appList, appDesc, allLogPageno, allLogPagesize);
}
function DisplayAllLogs(FromDate, ToDate, hostList, appList, appDesc, allLogPageno, allLogPagesize) {
    var ctrr = 1;
    $.ajax({
        //url: "Service.asmx/BeastDisplayAllLogs?dateList=" + dateList + "&hostList=" + hostList + "&appList=" + appList + "&appDesc=" + appDesc + "&fromTime=" + fromTime + "&toTime=" + toTime + "&pageNo=" + allLogPageno + "&pageSize=" + allLogPagesize,
        url: "Service.asmx/BeastDisplayAllLogs?FromDate=" + FromDate + "&ToDate=" + ToDate + "&hostList=" + hostList + "&appList=" + appList + "&appDesc=" + appDesc + "&pageNo=" + allLogPageno + "&pageSize=" + allLogPagesize,
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            var tmpJsonData = data.d;
            var summaryCtr = 1;

            $('#allLogSummaryTbl').empty();
            $('table.allLogSummaryTbl').append("<thead><tr> <th style='width:12%;'>Syslog Timestamp</th><th style='width:10%;'>Server Name</th><th style='width:15%;'>Process</th><th style='width:10%;'>Message Type</th><th style='width:53%;'>Description</th>    </tr></thead>  <tbody>   <tr id='1000000' class='allLogSummaryRow' style='display:none'><td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            $.each(data, function (i, jsondata) {
                var trClassSmry = "tr3";
                if (summaryCtr % 2 == 0)
                    trClassSmry = "tr4";
                else
                    trClassSmry = "tr3";

                var ctrrRow = summaryCtr + 'UserRow';

                var rol = 1;
                var over = 2;


                //var onclick = "onclick =getUserSessions('" + ctrrRow + "','" + $.trim(jsondata.UserID) + "','" + lastseen + "') ";
                var onclick = "onclick =''";
                $('<tr onmouseover="mouseEvent(\'' + ctrrRow + '\', ' + rol + ', ' + summaryCtr + ')" onmouseout="mouseEvent(\'' + ctrrRow + '\', ' + over + ', ' + summaryCtr + ')" id="' + ctrrRow + '"  style="cursor:pointer"  ><td ' + onclick + '>' + $.trim(jsondata.DateTime) + '</td><td ' + onclick + '>' + $.trim(jsondata.ServerName) + '</td><td ' + onclick + '>' + $.trim(jsondata.Application) + '</td><td ' + onclick + '>' + $.trim(jsondata.MessageType) + '</td><td ' + onclick + '>' + $.trim(jsondata.Description) + '</td></tr>').insertBefore('table.allLogSummaryTbl tr.allLogSummaryRow');

                summaryCtr++;

            });

            $('#allLogSummaryTbl').dataTable({
                "bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": true,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                //"iDisplayLength": 60,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ]
            });

            if ($('#allLogSummaryTbl tbody tr').length == 1) {
                //$('.allLogSummaryTbl')[0].innerText = 'Your search did not match any activities.Please Refine your Search.';
                $('.allLogSummaryTbl')[0].innerHTML = 'Your search did not match any activities.Please Refine your Search.';
            }

            if ($('#allLogSummaryTbl tbody tr').length == 1) {

            }
            else {
                //var html = '<div class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#" >Next</a></li>             </ul>         </div>     </div></div>';
                var html = '<div class="row"><div class="dataTables_paginate paging_two_button" id="allLogSummaryTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="allLogSummaryTbl_previous" aria-controls="allLogSummaryTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="allLogSummaryTbl_next" aria-controls="allLogSummaryTbl">Next</a></div></div>';
                $('#allLogSummaryTbl_wrapper').append(html);

                if ($('.wrapper #beastLogs-6 #toppageing').length == 0) {
                    html = '<div id="toppageing"  class="row"><a id="btnExportAllLog" href="#" style="float:left; padding-left: 30px; padding-bottom: 10px;">Export to Excel &nbsp;<img src="images/download.png" /></a><div class="dataTables_paginate paging_two_button" id="allLogSummaryTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="allLogSummaryTbl_previous" aria-controls="allLogSummaryTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="allLogSummaryTbl_next" aria-controls="allLogSummaryTbl">Next</a></div></div>';
                    //html = '<div id="toppageing"  class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#">Next</a></li>             </ul>         </div>     </div></div>';

                    $('#allLogSummaryTbl').before(html);

                }
            }

            $('.dataTables_paginate a').on('click', function () {
                if ($(this)[0].innerHTML == 'Next') {
                    allLogPageno = parseInt(allLogPageno) + 1;
                }
                if ($(this)[0].innerHTML == 'Previous') {
                    allLogPageno = parseInt(allLogPageno) - 1;
                    if (allLogPageno == 0 || allLogPageno < 0) {
                        allLogPageno = 1;

                    }
                }
                //DisplayAllLogs(dateList, hostList, appList, appDesc, fromTime, toTime, allLogPageno, allLogPagesize);
                DisplayAllLogs(FromDate, ToDate, hostList, appList, appDesc, allLogPageno, allLogPagesize);
            });

            $("#btnExportAllLog").click(function (e) {
                window.open('data:application/vnd.ms-excel,' + '<table>' + encodeURIComponent($('#allLogSummaryTbl').html()) + '</table>');
                e.preventDefault();
            });

            if (allLogPageno == '1') {
                $('.prev').css("display", "none");
            }
            if ($('#allLogSummaryTbl tbody tr').length == 1 || $('#allLogSummaryTbl tbody tr').length < allLogPagesize) {
                //$('.next').addClass('disabled');
                $('.next').css("display", "none");
            }
        },
        error: function (request, status, error) {
            //$('.allLogSummaryTbl')[0].innerHTML = error;
            //alert('aa');
            alert(error);
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })
}
function BindValuesForAllLogs() {
    //$.ajax({
    //    url: "Service.asmx/BeastAllLogDateList",
    //    data: "{}",
    //    contentType: "application/jsonp; charset=utf-8",
    //    dataType: "jsonp",
    //    success: function (data) {
    //        $.each(data, function (i, jsondata) {
    //            $("#ddlDateAllLogs").append($("<option></option>").text($.trim(jsondata.LogDate)).val($.trim(jsondata.LogDate)));
    //        });
    //        $("#ddlDateAllLogs").multipleSelect({
    //            filter: true
    //        });
    //    },
    //    error: function (request, status, error) {
    //        //$('#btnSysShowMore').css({ visibility: "hidden" });
    //    }
    //})

    $.ajax({
        url: "Service.asmx/BeastAllLogHostList",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlHostAllLogs").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlHostAllLogs").multipleSelect({
                filter: true,
                placeholder: "Source Host Name"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })

    $.ajax({
        url: "Service.asmx/BeastAllLogAppList",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlApplicationAllLogs, #ddlUIApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
                //$("#ddlUIApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            $("#ddlApplicationAllLogs, #ddlUIApplication").multipleSelect({
                filter: true,
                placeholder: "Application"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })


    $('#ddlAllLogPageSize').val(250);

    //var hourOption = '';
    //var minuteOption = '';
    //for (i = 0; i < 24; i++) {
    //    hourOption += '<option value="' + i + '">' + i + '</option>';
    //}
    //for (i = 0; i < 60; i++) {
    //    minuteOption += '<option value="' + i + '">' + i + '</option>';
    //}
    //$('#ddlFromHr').append(hourOption);
    //$('#ddlToHr').append(hourOption);
    //$('#ddlFromMin').append(minuteOption);
    //$('#ddlToMin').append(minuteOption);
    //setTimeRange()


}
function setTimeRange() {
    var fromTime = $("#ddlFromHr").val() + ":" + $("#ddlFromMin").val();
    var ToTime = $("#ddlToHr").val() + ":" + $("#ddlToMin").val();

    $("#fromTime").val(fromTime);
    $("#ToTime").val(ToTime);
}

function BindValuesForFrameworkApp() {
    $.ajax({
        url: "Service.asmx/BeastBindServers",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlServer").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlServer").multipleSelect({
                filter: true,
                placeholder: "Servers"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindApplicationTypes",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlOnApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            //BindApplicationOnChange();
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })
}
function BindApplicationOnChange() {
    $.ajax({
        url: "Service.asmx/BeastBindEvents?application=" + $("#ddlOnApplication" + ' :selected').val(),
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $('#ddlEvent option').remove();
            $.each(data, function (i, jsondata) {
                $("#ddlEvent").append($("<option></option>").text(jsondata.MessageType).val(jsondata.OperationCode));
            });
            $("#ddlEvent").multipleSelect({
                filter: true,
                placeholder: "Events"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })
    $.ajax({
        url: "Service.asmx/BeastBindApplications?application=" + $("#ddlOnApplication" + ' :selected').val(),
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $('#ddlApplication option').remove();
            $.each(data, function (i, jsondata) {
                $("#ddlApplication").append($("<option></option>").text(jsondata.Application).val(jsondata.Application));
            });
            $("#ddlApplication").multipleSelect({
                filter: true,
                placeholder: "Applications"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })
}

function SearchUserImages() {
    var UIServerList = '';
    var UISeverityList = '';
    var UIAppList = '';
    //var UISidList = '';
    var UIDesc = '';
    var UIPageno = 1;
    var UIPagesize = $("#ddlUIPageSize" + ' :selected').val();
    var UIUserId = '';

    if ($('#ddlUIServer').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        UIServerList = '';
    }
    else {
        $('#ddlUIServer').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlUIServer').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (UIServerList == '') {
                UIServerList = $(this).find('label').find('input').attr('value');
            }
            else {
                UIServerList = UIServerList + ',' + $(this).find('label').find('input').attr('value');
            }
        });
    }

    if ($('#ddlUISeverity').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        UISeverityList = '';
    }
    else {
        $('#ddlUISeverity').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlUISeverity').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (UISeverityList == '') {
                UISeverityList = $(this).find('label').find('input').attr('value');
            }
            else {
                UISeverityList = UISeverityList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    if ($('#ddlUIApplication').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        UIAppList = '';
    }
    else {
        $('#ddlUIApplication').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlUIApplication').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (UIAppList == '') {
                UIAppList = $(this).find('label').find('input').attr('value');
            }
            else {
                UIAppList = UIAppList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    //if ($('#ddlUISID').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
    //    UISidList = '';
    //}
    //else {
    //    $('#ddlUISID').next('div').find('.ms-drop').find('ul .selected').each(function () {
    //        $('#ddlUISID').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
    //        if (UISidList == '') {
    //            UISidList = $(this).find('label').find('input').attr('value');
    //        }
    //        else {
    //            UISidList = UISidList + ',' + $(this).find('label').find('input').attr('value');
    //        }

    //    });
    //}

    if ($("#txtUIDesc").val() != '') {
        UIDesc = $("#txtUIDesc").val();
    }

    if ($("#txtUIUserId").val() != '') {
        UIUserId = $("#txtUIUserId").val();
    }

    DisplayUserImages($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), UIServerList, UISeverityList, UIAppList, UIDesc, UIUserId, UIPageno, UIPagesize);
}
function DisplayUserImages(FromDate, ToDate, UIServerList, UISeverityList, UIAppList, UIDesc, UIUserId, UIPageno, UIPageSize) {
    var ctrr = 1;
    $.ajax({
        url: "Service.asmx/BeastDisplayUserImages?FromDate=" + FromDate + "&ToDate=" + ToDate + "&serverList=" + UIServerList + "&severityList=" + UISeverityList + "&appList=" + UIAppList + "&description=" + UIDesc + "&userId=" + UIUserId + "&pageNo=" + UIPageno + "&pageSize=" + UIPageSize,
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            var tmpJsonData = data.d;
            var summaryCtr = 1;
            $('#userImagesTbl').empty();
            $('table.userImagesTbl').append("<thead><tr> <th style='width:12%;'>Date Time</th><th style='width:10%;'>Server Name</th><th style='width:15%;'>Application</th><th style='width:10%;'>Message Type</th><th style='width:53%;'>Description</th>    </tr></thead>  <tbody>   <tr id='1000000' class='userImages' style='display:none'><td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            $.each(data, function (i, jsondata) {

                var trClassSmry = "";
                var Operation = $.trim(jsondata.MessageType);
                if ($.trim(jsondata.MessageType) == "Information") {
                    trClassSmry = "info";
                }
                else if ($.trim(jsondata.Operation) == "Warning") {
                    trClassSmry = "warning";
                }
                else {
                    trClassSmry = "danger";
                }

                var ctrrRow = summaryCtr + 'UserRow';

                var rol = 1;
                var over = 2;


                //var onclick = "onclick =getUserSessions('" + ctrrRow + "','" + $.trim(jsondata.UserID) + "','" + lastseen + "') ";
                var onclick = "onclick =''";
                $('<tr class=' + trClassSmry + ' id="' + ctrrRow + '"  style="cursor:pointer"  ><td ' + onclick + '>' + $.trim(jsondata.DateTime) + '</td><td ' + onclick + '>' + $.trim(jsondata.ServerName) + '</td><td ' + onclick + '>' + $.trim(jsondata.Application) + '</td><td ' + onclick + '>' + $.trim(jsondata.MessageType) + '</td><td ' + onclick + '>' + $.trim(jsondata.Description) + '</td></tr>').insertBefore('table.userImagesTbl tr.userImages');

                summaryCtr++;

            });

            $('#userImagesTbl').dataTable({
                "bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": true,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                //"iDisplayLength": 60,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ]
            });

            if ($('#userImagesTbl tbody tr').length == 1) {
                //$('.allLogSummaryTbl')[0].innerText = 'Your search did not match any activities.Please Refine your Search.';
                $('.userImagesTbl')[0].innerHTML = 'Your search did not match any activities.Please Refine your Search.';
            }

            if ($('#userImagesTbl tbody tr').length == 1) {

            }
            else {
                //var html = '<div class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#" >Next</a></li>             </ul>         </div>     </div></div>';
                var html = '<div class="row"><div class="dataTables_paginate paging_two_button" id="userImagesTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="userImagesTbl_previous" aria-controls="userImagesTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="userImagesTbl_next" aria-controls="userImagesTbl">Next</a></div></div>';
                $('#userImagesTbl_wrapper').append(html);

                if ($('.wrapper #beastLogs-4 #toppageing').length == 0) {
                    html = '<div id="toppageing"  class="row"><a id="btnExportUserImages" href="#" style="float:left; padding-left: 30px; padding-bottom: 10px;">Export to Excel &nbsp;<img src="images/download.png" /></a><div class="dataTables_paginate paging_two_button" id="userImagesTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="userImagesTbl_previous" aria-controls="userImagesTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="userImagesTbl_next" aria-controls="userImagesTbl">Next</a></div></div>';
                    //html = '<div id="toppageing"  class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#">Next</a></li>             </ul>         </div>     </div></div>';

                    $('#userImagesTbl').before(html);

                }
            }

            $('.dataTables_paginate a').on('click', function () {
                if ($(this)[0].innerHTML == 'Next') {
                    UIPageno = parseInt(UIPageno) + 1;
                }
                if ($(this)[0].innerHTML == 'Previous') {
                    UIPageno = parseInt(UIPageno) - 1;
                    if (UIPageno == 0 || UIPageno < 0) {
                        UIPageno = 1;

                    }
                }
                //DisplayUserImages($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), "", "", "", "", "", UIPageno, UIPageSize);
                DisplayUserImages(FromDate, ToDate, UIServerList, UISeverityList, UIAppList, UIDesc, UIUserId, UIPageno, UIPageSize);

            });
            $("#btnExportUserImages").click(function (e) {
                window.open('data:application/vnd.ms-excel,' + '<table>' + encodeURIComponent($('#userImagesTbl').html()) + '</table>');
                e.preventDefault();
            });

            if (UIPageno == '1') {
                $('.prev').css("display", "none");
            }
            if ($('#userImagesTbl tbody tr').length == 1 || $('#userImagesTbl tbody tr').length < UIPageSize) {
                //$('.next').addClass('disabled');
                $('.next').css("display", "none");
            }
        },
        error: function (request, status, error) {
            //$('.allLogSummaryTbl')[0].innerHTML = error;
            //alert('aa');
            alert(error);
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })
}
function BindValuesForUserImages() {
    $.ajax({
        url: "Service.asmx/BeastBindServers",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlUIServer").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlUIServer").multipleSelect({
                filter: true,
                placeholder: "Servers"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindSeverity",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlUISeverity").append($("<option></option>").text(jsondata.SeverityType).val(jsondata.Id));
            });
            $("#ddlUISeverity").multipleSelect({
                filter: true,
                placeholder: "Severity"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })

    //$.ajax({
    //    url: "Service.asmx/BeastBindSID",
    //    data: "{}",
    //    contentType: "application/jsonp; charset=utf-8",
    //    dataType: "jsonp",
    //    success: function (data) {
    //        $.each(data, function (i, jsondata) {
    //            $("#ddlUISID").append($("<option></option>").text(jsondata.SID).val(jsondata.SID));
    //        });
    //        $("#ddlUISID").multipleSelect({
    //            filter: true,
    //            placeholder: "SIDs"
    //        });
    //    },
    //    error: function (request, status, error) {
    //        //$('#btnSysShowMore').css({ visibility: "hidden" });
    //    }
    //})
    $('#ddlUIPageSize').val(250);
}


function SearchImages() {
    var ImageServerList = '';
    var ImageEventList = '';
    var ImageSidList = '';
    var ImagePageno = 1;
    var ImagePagesize = $("#ddlImagePageSize" + ' :selected').val();
    var ImageUserId = '';

    if ($('#ddlImageServer').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        ImageServerList = '';
    }
    else {
        $('#ddlImageServer').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlImageServer').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (ImageServerList == '') {
                ImageServerList = $(this).find('label').find('input').attr('value');
            }
            else {
                ImageServerList = ImageServerList + ',' + $(this).find('label').find('input').attr('value');
            }
        });
    }

    if ($('#ddlImageEvent').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        ImageEventList = '';
    }
    else {
        $('#ddlImageEvent').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlImageEvent').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (ImageEventList == '') {
                ImageEventList = $(this).find('label').find('input').attr('value');
            }
            else {
                ImageEventList = ImageEventList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }
    if ($('#ddlImageSID').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        ImageSidList = '';
    }
    else {
        $('#ddlImageSID').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlImageSID').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (ImageSidList == '') {
                ImageSidList = $(this).find('label').find('input').attr('value');
            }
            else {
                ImageSidList = ImageSidList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    if ($("#txtImageUserId").val() != '') {
        ImageUserId = $("#txtImageUserId").val();
    }

    DisplayImages($("input[name='daterangepicker_start']").val(), $("input[name='daterangepicker_end']").val(), ImageServerList, ImageEventList, ImageSidList, ImageUserId, ImagePageno, ImagePagesize);
}
function DisplayImages(FromDate, ToDate, ImageServerList, ImageEventList, ImageSidList, ImageUserId, ImagePageno, ImagePagesize) {
    var ctrr = 1;
    $.ajax({
        url: "Service.asmx/BeastDisplayImages?FromDate=" + FromDate + "&ToDate=" + ToDate + "&serverList=" + ImageServerList + "&eventList=" + ImageEventList + "&sidList=" + ImageSidList + "&userId=" + ImageUserId + "&pageNo=" + ImagePageno + "&pageSize=" + ImagePagesize,
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            var tmpJsonData = data.d;
            var summaryCtr = 1;
            $('#imagesTbl').empty();
            $('table.imagesTbl').append("<thead><tr> <th style='width:15%;'>Date Time</th><th style='width:15%;'>Server Name</th><th style='width:10%;'>SID</th><th style='width:10%;'>Event</th><th style='width:50%;'>Other Details</th>    </tr></thead>  <tbody>   <tr id='1000000' class='images' style='display:none'><td></td><td></td><td></td><td></td><td></td></tr></tbody>");

            $.each(data, function (i, jsondata) {

                //var trClassSmry = "";
                //var Operation = $.trim(jsondata.MessageType);
                //if ($.trim(jsondata.MessageType) == "Information") {
                //    trClassSmry = "info";
                //}
                //else if ($.trim(jsondata.Operation) == "Warning") {
                //    trClassSmry = "warning";
                //}
                //else {
                //    trClassSmry = "danger";
                //}

                var ctrrRow = summaryCtr + 'UserRow';

                var rol = 1;
                var over = 2;


                //var onclick = "onclick =getUserSessions('" + ctrrRow + "','" + $.trim(jsondata.UserID) + "','" + lastseen + "') ";
                var onclick = "onclick =''";
                $('<tr onmouseover="mouseEvent(\'' + ctrrRow + '\', ' + rol + ', ' + summaryCtr + ')" onmouseout="mouseEvent(\'' + ctrrRow + '\', ' + over + ', ' + summaryCtr + ')"  id="' + ctrrRow + '"  style="cursor:pointer"  ><td ' + onclick + '>' + $.trim(jsondata.DateTime) + '</td><td ' + onclick + '>' + $.trim(jsondata.ServerName) + '</td><td ' + onclick + '>' + $.trim(jsondata.SID) + '</td><td ' + onclick + '>' + $.trim(jsondata.Event) + '</td><td ' + onclick + '>' + $.trim(jsondata.OtherDetails) + '</td></tr>').insertBefore('table.imagesTbl tr.images');

                summaryCtr++;

            });

            $('#imagesTbl').dataTable({
                "bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": true,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                //"iDisplayLength": 60,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ]
            });

            if ($('#imagesTbl tbody tr').length == 1) {
                //$('.allLogSummaryTbl')[0].innerText = 'Your search did not match any activities.Please Refine your Search.';
                $('.imagesTbl')[0].innerHTML = 'Your search did not match any activities.Please Refine your Search.';
            }

            if ($('#imagesTbl tbody tr').length == 1) {

            }
            else {
                //var html = '<div class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#" >Next</a></li>             </ul>         </div>     </div></div>';
                var html = '<div class="row"><div class="dataTables_paginate paging_two_button" id="imagesTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="imagesTbl_previous" aria-controls="imagesTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="imagesTbl_next" aria-controls="imagesTbl">Next</a></div></div>';
                $('#imagesTbl_wrapper').append(html);

                if ($('.wrapper #beastLogs-3 #toppageing').length == 0) {
                    html = '<div id="toppageing"  class="row"><a id="btnExportImages" href="#" style="float:left; padding-left: 30px; padding-bottom: 10px;">Export to Excel &nbsp;<img src="images/download.png" /></a><div class="dataTables_paginate paging_two_button" id="userImagesTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="imagesTbl_previous" aria-controls="imagesTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="imagesTbl_next" aria-controls="imagesTbl">Next</a></div></div>';
                    //html = '<div id="toppageing"  class="row"> <div class="col-xs-6">            </div>     <div class="col-xs-6">         <div class="dataTables_paginate paging_bootstrap" style="float:right" >             <ul class="pagination">                 <li class="prev"><a href="#">Previous</a></li>                               <li class="next"><a href="#">Next</a></li>             </ul>         </div>     </div></div>';

                    $('#imagesTbl').before(html);

                }
            }

            $('.dataTables_paginate a').on('click', function () {
                if ($(this)[0].innerHTML == 'Next') {
                    ImagePageno = parseInt(ImagePageno) + 1;
                }
                if ($(this)[0].innerHTML == 'Previous') {
                    ImagePageno = parseInt(ImagePageno) - 1;
                    if (ImagePageno == 0 || ImagePageno < 0) {
                        ImagePageno = 1;

                    }
                }
                DisplayImages(FromDate, ToDate, ImageServerList, ImageEventList, ImageSidList, ImageUserId, ImagePageno, ImagePagesize);

            });
            $("#btnExportImages").click(function (e) {
                window.open('data:application/vnd.ms-excel,' + '<table>' + encodeURIComponent($('#imagesTbl').html()) + '</table>');
                e.preventDefault();
            });

            if (ImagePageno == '1') {
                $('.prev').css("display", "none");
            }
            if ($('#imagesTbl tbody tr').length == 1 || $('#imagesTbl tbody tr').length < ImagePagesize) {
                $('.next').css("display", "none");
            }
        },
        error: function (request, status, error) {
            alert(error);
        }
    })
}
function BindValuesForImages() {
    $.ajax({
        url: "Service.asmx/BeastBindServers",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlImageServer").append($("<option></option>").text(jsondata.SourceHostName).val(jsondata.SourceHostName));
            });
            $("#ddlImageServer").multipleSelect({
                filter: true,
                placeholder: "Servers"
            });
        },
        error: function (request, status, error) {
            //$('#btnSysShowMore').css({ visibility: "hidden" });
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindImageEvents",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlImageEvent").append($("<option></option>").text(jsondata.EventName).val(jsondata.Id));
            });
            $("#ddlImageEvent").multipleSelect({
                filter: true,
                placeholder: "Events"
            });
        },
        error: function (request, status, error) {
        }
    })

    $.ajax({
        url: "Service.asmx/BeastBindSID",
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            $.each(data, function (i, jsondata) {
                $("#ddlImageSID").append($("<option></option>").text(jsondata.SID).val(jsondata.SID));
            });
            $("#ddlImageSID").multipleSelect({
                filter: true,
                placeholder: "SIDs"
            });
        },
        error: function (request, status, error) {
        }
    })

    $('#ddlImagePageSize').val(250);
}

function SearchAdminAppLog() {
    var serverList = '';
    var eventList = '';
    var appList = '';
    var onApp = '';
    var frameworkAppPageno = 1;
    var frameworkAppPagesize = $("#ddlAdminAppPageSize" + ' :selected').val();
    if ($('#ddlServer').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        serverList = '';
    }
    else {
        $('#ddlServer').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlServer').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (serverList == '') {
                serverList = $(this).find('label').find('input').attr('value');
            }
            else {
                serverList = serverList + ',' + $(this).find('label').find('input').attr('value');
            }
        });
    }

    if ($('#ddlEvent').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        eventList = '';
    }
    else {
        $('#ddlEvent').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlEvent').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (eventList == '') {
                eventList = $(this).find('label').find('input').attr('value');
            }
            else {
                eventList = eventList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    if ($('#ddlApplication').next('div').find('.ms-choice').find('span')[0].innerHTML == 'All selected') {
        appList = '';
    }
    else {
        $('#ddlApplication').next('div').find('.ms-drop').find('ul .selected').each(function () {
            $('#ddlApplication').next('div').find('.ms-drop').find('ul .selected').parent().prepend(this);
            if (appList == '') {
                appList = $(this).find('label').find('input').attr('value');
            }
            else {
                appList = appList + ',' + $(this).find('label').find('input').attr('value');
            }

        });
    }

    if ($("#ddlOnApplication").val() != '') {
        onApp = $("#ddlOnApplication").val();
    }
    DisplayAdminAppLog(serverList, eventList, appList, onApp, frameworkAppPageno, frameworkAppPagesize);
}
function DisplayAdminAppLog(serverList, eventList, appList, onApp, frameworkAppPageno, frameworkAppPagesize) {
    var ctrr = 1;
    debugger;
    $.ajax({
        url: "Service.asmx/BeastDisplayAdminAppLog?serverList=" + serverList + "&eventList=" + eventList + "&appList=" + appList + "&onApp=" + onApp + "&pageNo=" + frameworkAppPageno + "&pageSize=" + frameworkAppPagesize,
        data: "{}",
        contentType: "application/jsonp; charset=utf-8",
        dataType: "jsonp",
        success: function (data) {
            var tmpJsonData = data.d;
            var summaryCtr = 1;
            $('#adminAppTbl').empty();
            $('table.adminAppTbl').append("<thead><tr> <th style='width:15%;'>Date Time</th><th style='width:15%;'>Server Name</th><th style='width:15%;'>On Application</th><th style='width:10%;'>Event</th><th style='width:15%;'>Application</th><th style='width:30%;'>Description</th>    </tr></thead>  <tbody>   <tr id='1000000' class='images' style='display:none'><td></td><td></td><td></td><td></td><td></td><td></td></tr></tbody>");
            debugger;
            $.each(data, function (i, jsondata) {

                //var trClassSmry = "";
                //var Operation = $.trim(jsondata.MessageType);
                //if ($.trim(jsondata.MessageType) == "Information") {
                //    trClassSmry = "info";
                //}
                //else if ($.trim(jsondata.Operation) == "Warning") {
                //    trClassSmry = "warning";
                //}
                //else {
                //    trClassSmry = "danger";
                //}

                var ctrrRow = summaryCtr + 'UserRow';

                var rol = 1;
                var over = 2;


                //var onclick = "onclick =getUserSessions('" + ctrrRow + "','" + $.trim(jsondata.UserID) + "','" + lastseen + "') ";
                //var onclick = "onclick =''";
                $('<tr onmouseover="mouseEvent(\'' + ctrrRow + '\', ' + rol + ', ' + summaryCtr + ')" onmouseout="mouseEvent(\'' + ctrrRow + '\', ' + over + ', ' + summaryCtr + ')"  id="' + ctrrRow + '"  style="cursor:pointer"  ><td>' + $.trim(jsondata.DateTime) + '</td><td>' + $.trim(jsondata.ServerName) + '</td><td>' + $.trim(jsondata.OnApplication) + '</td><td>' + $.trim(jsondata.Event) + '</td><td >' + $.trim(jsondata.Application) + '</td><td>' + $.trim(jsondata.Description) + '</td></tr>').insertBefore('table.adminAppTbl tr.images');

                summaryCtr++;

            });

            $('#adminAppTbl').dataTable({
                "bProcessing": true,
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bSort": true,
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                //"iDisplayLength": 60,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ]
            });

            if ($('#adminAppTbl tbody tr').length == 1) {
                //$('.allLogSummaryTbl')[0].innerText = 'Your search did not match any activities.Please Refine your Search.';
                $('.adminAppTbl')[0].innerHTML = 'Your search did not match any activities.Please Refine your Search.';
            }

            if ($('#adminAppTbl tbody tr').length == 1) {

            }
            else {
                var html = '<div class="row"><div class="dataTables_paginate paging_two_button" id="adminAppTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="adminAppTbl_previous" aria-controls="adminAppTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="adminAppTbl_next" aria-controls="adminAppTbl">Next</a></div></div>';
                $('#adminAppTbl_wrapper').append(html);

                if ($('.wrapper #beastLogs-3 #toppageing').length == 0) {
                    html = '<div id="toppageing"  class="row"><a id="btnExportAdminAppLog" href="#" style="float:left; padding-left: 30px; padding-bottom: 10px;">Export to Excel &nbsp;<img src="images/download.png" /></a><div class="dataTables_paginate paging_two_button" id="adminAppTbl_paginate"><a class="paginate_enabled_previous prev" tabindex="0" role="button" id="adminAppTbl_previous" aria-controls="adminAppTbl">Previous</a><a class="paginate_enabled_next next" tabindex="0" role="button" id="adminAppTbl_next" aria-controls="adminAppTbl">Next</a></div></div>';
                    $('#adminAppTbl').before(html);
                }
            }

            $('.dataTables_paginate a').on('click', function () {
                if ($(this)[0].innerHTML == 'Next') {
                    frameworkAppPageno = parseInt(frameworkAppPageno) + 1;
                }
                if ($(this)[0].innerHTML == 'Previous') {
                    frameworkAppPageno = parseInt(frameworkAppPageno) - 1;
                    if (frameworkAppPageno == 0 || frameworkAppPageno < 0) {
                        frameworkAppPageno = 1;

                    }
                }
                DisplayAdminAppLog(serverList, eventList, appList, onApp, frameworkAppPageno, frameworkAppPagesize);

            });


            $("#btnExportAdminAppLog").click(function (e) {
                window.open('data:application/vnd.ms-excel,' + '<table>' + encodeURIComponent($('#adminAppTbl').html()) + '</table>');
                e.preventDefault();
            });

            if (frameworkAppPageno == '1') {
                $('.prev').css("display", "none");
            }
            if ($('#adminAppTbl tbody tr').length == 1 || $('#adminAppTbl tbody tr').length < frameworkAppPagesize) {
                $('.next').css("display", "none");
            }
        },
        error: function (request, status, error) {
            alert(error);
        }
    })
}

function mouseEvent(RowID, evnt, RowCtr) {
    if (evnt == 1) {
        document.getElementById(RowID).className = "tdhover";
    }
    else {
        if (RowCtr % 2 == 0) {
            document.getElementById(RowID).className = "tr4";
        }
        else {
            document.getElementById(RowID).className = "tr3";
        }
    }
}

