$(document).ready(function () {

    $("#FY").select2();
    $("#Basis").select2();
    $("#Head").select2();

    //$(":file").filestyle({ buttonName: "btn-primary" });

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

    document.getElementById('CurrentDate').innerHTML = today;
    //      End Get Today's Date
    
/*----------------------------------------------------*/
/*	 add row and delete row
/*----------------------------------------------------*/

var i = 1;
$("#add_row").click(function () {
    $('#addr' + i).html("<td>" + (i + 1) + "</td><td><div class='input-group date' id='InvoiceDate" + i + "'><input type='text' id='txtCurrentDate' class='form-control' name = 'InvoiceDetails[" + i + "].Invoice_Date'/><span class='input-group-addon'><i class='fa fa-calendar'></i></span></div> </td><td><input  type='text' id='txtInvoiceAmt' placeholder='Invoice Amount'  class='form-control input-md' name = 'InvoiceDetails[" + i + "].Invoice_Amt'></td><td><input  type='text' id='txtRemarks' placeholder='Remarks'  class='form-control input-md' name = 'InvoiceDetails[" + i + "].Invoice_Remark'></td>");

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