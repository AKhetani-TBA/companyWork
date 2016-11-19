$(document).ready(function () {

    /*----------------------------------------------------*/
    /*	Left Accordion Icon
	/*----------------------------------------------------*/
    function toggleChevron(e) {
        $(e.target)
            .prev('.heading')
            .find("i.indicator")
            .toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
    }
    $('#leftAccordion').on('hidden.bs.collapse', toggleChevron);
    $('#leftAccordion').on('shown.bs.collapse', toggleChevron);

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

});


//      End Get Today's Date


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

    /*----------------------------------------------------*/
    /*	 add row and delete row
	/*----------------------------------------------------*/

    var i = 1;
    $("#add_row").click(function () {
        $('#addr' + i).html("<td>" + (i + 1) + "</td><td><div class='input-group date' id='birthDate" + i + "'><input type='text' class='form-control' /><span class='input-group-addon'><i class='fa fa-calendar'></i></span></div> </td><td><input  name='mail" + i + "' type='text' placeholder='Mail'  class='form-control input-md'></td><td><input  name='mobile" + i + "' type='text' placeholder='Mobile'  class='form-control input-md'></td>");

        $('#tab_logic').append('<tr id="addr' + (i + 1) + '"></tr>');
        i++;
    });
    $("#delete_row").click(function () {
        if (i > 1) {
            $("#addr" + (i - 1)).html('');
            i--;
        }
    });

    /*----------------------------------------------------*/
    /*	 Entry Module User Dropdown Search Start
	/*----------------------------------------------------*/
    //$("#FY").select2({
    //    data: FY
    //});

    //$("#Basis").select2({
    //    data: Basis
    //});

    //$("#Head").select2({
    //    data: Head
    //});

    /*----------------------------------------------------*/
    /*	 Employees Personal Details Date Picker
    /*----------------------------------------------------*/







    /*----------------------------------------------------*/
    /*	 Entry Module User Dropdown Search End
	/*----------------------------------------------------*/






/* ----------------- End JS Document ----------------- */



