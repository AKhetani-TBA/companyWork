$(document).ready(function () {
    debugger;
    var table = $('#tblDocDetails').DataTable({
        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "250px",
        "scrollCollapse": true,
        "paging": false,

        //"columnDefs": [{
        //    "searchable": false,
        //    "orderable": false,
        //    "targets": 0
        //}],
        //"order": [[1, 'asc']],


        "ajax": {
            "url": 'GetPreviousRecordOfSectionsExemptions',
            "dataSrc": function (json) {
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["WEF"] != null) {
                        json.data[i]["WEF"] = moment(json.data[i]["WEF"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["Maximum"] != null) {
                        json.data[i]["Maximum"] = addCommas(json.data[i]["Maximum"]);
                    }
                    if (json.data[i]["LastAction"] == "N" && json.data[i]["ModifyDate"] == null) {
                        json.data[i]["LastAction"] = "Add";
                    }
                    else if (json.data[i]["LastAction"] == "N" && json.data[i]["ModifyDate"] != null) {
                        json.data[i]["LastAction"] = "Update";
                    }
                    else {
                        json.data[i]["LastAction"] = "Delete";
                    }
                }
                return json.data;
            }
        },

        "columns": [
            //{ "defaultContent": "" },
            { "data": "Basis"},
            { "data": "Section"},
            { "data": "Head"},
            { "data": "Abbreviation"},
            { "data": "Maximum"},
            { "data": "WEF" },
            { "data": "LastAction" },
            {
                "defaultContent": "<button class='btn btn-primary btn-xs'  id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
                "orderable": false
            },
            {
                "orderable": false,
                "render": function (data, type, row) {
                    if (row.LastAction == "Delete") {
                        return "<button  class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' disabled><span class='glyphicon glyphicon-trash'></span></button>";
                        //  $('#btnDelete').attr('disabled', true);
                    }
                    else {
                        return "<button class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>";
                    }
                }
            }
          
            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Edit'><button class='btn btn-primary btn-xs' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button></p>" },
            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Delete'><button class='btn btn-danger btn-xs' data-title='Dekete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button></p>" },
            //{ "defaultContent": "<button>Edit</button>" },
            //{ "defaultContent": "<button>Delete</button>" }
        ],
        "columnDefs": [
            { className: "text-right", "targets": [4], "width": "20px" },
            { "width": "20px", "targets": 0 },
            { "width": "1px", "targets": 1 },
            { "width": "50px", "targets": 2 },
            { "width": "10px", "targets": 3 },
            { "width": "40px", "targets": 5 },
            { "width": "10px", "targets": 6 },
            { "width": "10px", "targets": 7 },
            { "width": "10px", "targets": 8 }

        ]

    });

    $('#tblDocDetails tbody').on('click', 'button', function () {
        debugger;
        var data = table.row($(this).parents('tr')).data();
        $('#hdnSecExemID').val(data["SecExemID"]);
        $('#lblFYToEdit').val(data["FYId"]);
        $('#lblWEFToEdit').val(data["WEF"]);
        $('#lblSectionToEdit').val(data["SectionId"]);
        $('#lblHeadToEdit').val(data["Head"]);
        $('#lblAbbreviationToEdit').val(data["Abbreviation"]);
        $('#lblBaseToEdit').val(data["BasisId"]);
        $('#lblMaximumToEdit').val(data["Maximum"]);
        
        if ($(this).attr("id") == "btnEdit") {
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });
    //table.on('order.dt search.dt', function () {
    //    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();


    //$('#tblRoleDetails tbody').on('click', 'button', function () {
    //    var data = table.row($(this).parents('tr')).data();
    //    alert("RoleId is: " + data["RoleId"]);
    //});

    //      Start Get Today's Date

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = dd + '/' + mm + '/' + yyyy;
  
    $('#txtCurrentDate').val(today);
    document.getElementById('CurrentDate').innerHTML = today;
    //      End Get Today's Date
});


//          Diff


$(function () {
    $("#form1").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
    $("#UpdateDocumentUploadForm").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
});

function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function DateFormat(field, rules, i, options) {
    var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    if (!regex.test(field.val())) {
        return "Please enter date in MM/DD/YYYY format."
    }
}

function AmountFormat(field, rules, i, options) {
    var regex = /^[0-9]{1,3}(,[0-9]{3})*(([\\.,]{1}[0-9]*)|())$/;
    if (!regex.test(field.val())) {
        return "Please enter right amount format."
    }
}

function SetPointingFormat(value) {
    //return parseFloat(Math.round(value * 100) / 100).toFixed(2);
    //return parseFloat(value.toFixed(2));

    var num = value;
    return num.toFixed(2);
}

//      Diff


function DeleteUsers(bID) {
    var result = confirm("Are you Sure you Want to Delete Selected Record? : " + bID);
    if (result) {
        var data = bID;
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: 'DeleteDetails',
            data: { "json": JSON.stringify(data) },
            success: function (json) {
                if (json) {
                    alert('ok');
                } else {
                    alert('failed');
                }
            },
        })
    }
}

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    // add a zero in front of numbers<10
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('time').innerHTML = h + ":" + m + ":" + s;
    t = setTimeout(function () {
        startTime()
    }, 500);
}
startTime();

$('#txtCurrentDate').datetimepicker({
    defaultDate: new Date(),
    format: 'DD/MM/YYYY'
});


/* For Edit Mode */

$('#lblWEFToEdit').datetimepicker({
    defaultDate: new Date(),
    format: 'DD/MM/YYYY'
});

/* End Edit Mode */

/* For Delete Mode */

$('#lblDateToDelete').datetimepicker({
    defaultDate: new Date(),
    format: 'DD/MM/YYYY'
});


/* End Delete Mode */