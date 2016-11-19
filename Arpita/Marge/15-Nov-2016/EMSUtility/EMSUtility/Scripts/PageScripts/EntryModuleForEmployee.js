var i = 1;
var FY = $('#FY').val();
var Head = $('#Head').val();


function SingleDocDetals(headName) {

    var prevTable = $('#tblEmpSingleDocDetails').DataTable();
    prevTable.destroy();

    $('#tblEmpSingleDocDetails').DataTable({

        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollCollapse": true,
        "paging": false,

        "columnDefs": [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],
        "order": [[1, 'asc']],

        "ajax": {
            "type": 'POST',
            "url": 'GetPreviousEmployeeSingleDocDetails',
            "data": { 'FYid': FY, 'Head': headName },
            "dataSrc": function (json) {

                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["Invoice_Date"] != null) {
                        json.data[i]["Invoice_Date"] = moment(json.data[i]["Invoice_Date"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["Invoice_Amt"] != null) {
                        json.data[i]["Invoice_Amt"] = addCommas(json.data[i]["Invoice_Amt"]);
                    }
                }
                return json.data;
            }
        },
        "columns": [
            { "defaultContent": "" },
            { "data": "Invoice_Date" },
            { "data": "Invoice_Amt" },
            { "data": "ApprovedAmount" }
        ],
        "columnDefs": [
            { className: "text-right", "targets": [2] },
            { className: "text-right", "targets": [3] }
        ]
    });

}

$('#tblEmpSingleDocDetails').DataTable().on('order.dt search.dt', function () {
    $('#tblEmpSingleDocDetails').DataTable().column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        cell.innerHTML = i + 1;
    });
}).draw();


$(document).ready(function () {


    var table = $('#tblEmpDocDetails').DataTable({

        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        //"scrollY": "250px",
        "scrollCollapse": true,
        "paging": false,

        //"columnDefs": [{
        //    "searchable": false,
        //    "orderable": false,
        //    "targets": 0
        //}],
        //"order": [[1, 'asc']],


        "ajax": {
            "type": 'POST',
            "url": 'GetPreviousEmployeeDetails',
            "data":{ 'FYid' : FY },
            "dataSrc": function (json) {

                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["Invoice_Date"] != null) {
                        json.data[i]["Invoice_Date"] = moment(json.data[i]["Invoice_Date"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["Invoice_Amt"] != null) {
                        json.data[i]["Invoice_Amt"] = addCommas(json.data[i]["Invoice_Amt"]);
                    }
                }
                return json.data;
            }
        },

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "Head" },
            { "defaultContent": "" },
            { "defaultContent": "" },
            { "defaultContent": "" },
            { "defaultContent": "" },
            { "defaultContent": "" },
            { "defaultContent": "" }
            //{ "data": "LastAction" },
            //{
            //    "defaultContent": "<button class='btn btn-primary btn-xs'  id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
            //    "orderable": false
            //},
            //{
            //    "orderable": false,
            //    "render": function (data, type, row) {
            //        if (row.LastAction == "Delete") {
            //            return "<button  class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' disabled><span class='glyphicon glyphicon-trash'></span></button>";
            //            //  $('#btnDelete').attr('disabled', true);
            //        }
            //        else {
            //            return "<button class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>";
            //        }
            //    }
            //}

            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Edit'><button class='btn btn-primary btn-xs' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button></p>" },
            //{ "defaultContent": "<p data-placement='top' data-toggle='tooltip' title='Delete'><button class='btn btn-danger btn-xs' data-title='Dekete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button></p>" },
            //{ "defaultContent": "<button>Edit</button>" },
            //{ "defaultContent": "<button>Delete</button>" }
        ]
        //"columnDefs": [
        //    { className: "text-right", "targets": [4], "width": "20px" },
        //    { "width": "20px", "targets": 0 },
        //    { "width": "1px", "targets": 1 },
        //    { "width": "50px", "targets": 2 },
        //    { "width": "10px", "targets": 3 },
        //    { "width": "40px", "targets": 5 },
        //    { "width": "10px", "targets": 6 },
        //    { "width": "10px", "targets": 7 },
        //    { "width": "10px", "targets": 8 }

        //]
       
    });
        

    function format(d) {
        // `d` is the original data object for the row
        return '<div class="table-responsive">' +
                        '<table id="tblDocDetails" class="table table-striped table-bordered" data-page-length="8">' +
                                '<tr>' +
                                    '<td>Invoice Date:</td>' +
                                    '<td>'+ d.Invoice_Date +'</td>' +
                                '</tr>' +
                                '<tr>' +
                                    '<td>Invoice Amount:</td>' +
                                    '<td>' + d.Invoice_Amt + '</td>' +
                                '</tr>' +
                                //'<tr>' +
                                //    '<td>Extra info:</td>' +
                                //    '<td>And any further details here (images etc)...</td>' +
                                //'</tr>' +
                        '</table>' +
                '</div>';
    }

    // Add event listener for opening and closing details
    $('#tblEmpDocDetails tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        
        if ($(tr).hasClass("shown")) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
    //End of DataTable 

    $("#FY").select2();
    $("#Basis").select2();
    $("#Head").select2();

    //$(":file").filestyle({ buttonName: "btn-primary" });


    //Start Get Today's Date
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

    document.getElementById('CurrentDate').innerHTML = today;
    //      End Get Today's Date

    /*----------------------------------------------------*/
    /*	 add row and delete row
    /*----------------------------------------------------*/


    $("#add_row").click(function () {
        $('#addr' + i).html("<td>" + (i + 1) + "</td><td><div class='input-group date' id='InvoiceDate" + i + "'><input type='text' id='txtCurrentDate' class='form-control' name = 'InvoiceDetails[" + i + "].Invoice_Date'/><span class='input-group-addon'><i class='fa fa-calendar'></i></span></div> </td><td><input  type='text' id='txtInvoiceAmt' placeholder='Invoice Amount'  class='validate[required, maxSize[10]] form-control text-right' name = 'InvoiceDetails[" + i + "].Invoice_Amt' onkeypress = 'return isNumber(event)' onkeyup = 'NewTotalAmountCount(&#39;divTotalAmount&#39; , this); this.value=addCommas(this.value.replace(/,/g,&#39;&#39;));'></td><td><input  type='text' id='txtRemarks' placeholder='Remarks'  class='form-control input-md' name = 'InvoiceDetails[" + i + "].Invoice_Remark'></td>");

        $('#tab_logic').append('<tr id="addr' + (i + 1) + '"></tr>');

        $('#InvoiceDate' + i).datetimepicker({
            defaultDate: new Date(),
            format: 'DD/MM/YYYY'
        });

        i++;
    });
    $("#delete_row").click(function () {
        if (i > 1) {
            $("#addr" + (i - 1)).html('');
            i--;
        }
    });

});

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//  To delete row
//function deleteRow(row) {
//    var iRow = row.parentNode.parentNode.rowIndex;
//    document.getElementById('tab_logic').deleteRow(iRow);
//}


//// Total Amount Count 
//function totalAmountCount(divTotalAmount) {
//    amountValues = new Array();
//    var iCount = 0;

//    for (iCount = 0; iCount < i; iCount++) {
//        amountValues[iCount] = document.getElementById("txtInvoiceAmt" + iCount).value;
//        amountValues[iCount] = amountValues[iCount].replace(/,/g, '');
//    }

//    var total = 0;
//    $.each(amountValues, function () {
//        //total += this;

//        total = total + (+this);
//    });

//    total = addCommas(total);
//    document.getElementById(divTotalAmount).innerHTML = total + "/-";
//}

//new Total Amount Count
function NewTotalAmountCount(divTotalAmount, rowIndex) {
    var iCount = 0;
    newAmountValues = new Array();

    $("#tab_logic tr").each(function () {
        if ($(this).find('input[id="' + rowIndex.id + '"]').length > 0) {
            newAmountValues[iCount] = Number($(this).find('input[id="' + rowIndex.id + '"]').val().replace(/[^0-9\.]+/g, ""));
            iCount++;
        }
    });

    var total = 0;
    $.each(newAmountValues, function () {
        //total += this;

        total = total + (+this);
    });

    total = addCommas(total);
    document.getElementById(divTotalAmount).innerHTML = total + "/-";
}


//Add Commas in amount 
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

    $('#InvoiceDate').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });

    $(function () {
        $("#form1").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
        $("#UpdateDocumentUploadForm").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
        $("#UpdateSingleDocumentUploadForm").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });
    });

    $(function () {

        // We can attach the `fileselect` event to all file inputs on the page
        $(document).on('change', ':file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselect', [numFiles, label]);
        });

        // We can watch for our custom `fileselect` event like this
        $(document).ready(function () {
            $(':file').on('fileselect', function (event, numFiles, label) {

                var input = $(this).parents('.input-group').find(':text'),
                    log = numFiles > 1 ? numFiles + ' files selected' : label;

                if (input.length) {
                    input.val(log);
                } else {
                    if (log) alert(log);
                }

            });
        });

    });