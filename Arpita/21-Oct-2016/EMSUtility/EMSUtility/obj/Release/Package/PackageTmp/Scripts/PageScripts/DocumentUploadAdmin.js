$(document).ready(function () {
    debugger;
    var table = $('#tblDocDetails').DataTable({
        //"searching": true,
        "ordering": true,
        //"pagingType": "full_numbers",
        //"scrollY": "250px",
        "scrollCollapse": true,
        "paging": false,

        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']],

        "ajax": {
            "url": 'GetPreviousRecordOfSectionsExemptions'},
        "columns": [
            { "defaultContent": "" },
            { "data": "Section" },
            { "data": "Head" },
            { "data": "Basis" },
            { "data": "Minimum" },
            { "data": "Maximum" },
            { "data": "WEF" },
            { "defaultContent": "<button class='btn btn-primary btn-xs' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>" },
            { "defaultContent": "<button class='btn btn-danger btn-xs' data-title='Dekete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>" },
            
            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Edit'><button class='btn btn-primary btn-xs' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button></p>" },
            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Delete'><button class='btn btn-danger btn-xs' data-title='Dekete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button></p>" },
            //{ "defaultContent": "<button>Edit</button>" },
            //{ "defaultContent": "<button>Delete</button>" }
        ]
    });

    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    //$('#tblRoleDetails tbody').on('click', 'button', function () {
    //    var data = table.row($(this).parents('tr')).data();
    //    alert("RoleId is: " + data["RoleId"]);
    //});
});


//          Diff


$(function () {
    $("#form1").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
});

function addCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

function DateFormat(field, rules, i, options) {
    var regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
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