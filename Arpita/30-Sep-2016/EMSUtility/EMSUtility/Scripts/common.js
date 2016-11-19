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


    /*----------------------------------------------------*/
    /*	 Employees Personal Details Date Picker
	/*----------------------------------------------------*/
    $('#hireDate').datetimepicker();

//    $('#birthDate').datetimepicker();

    $('#DateToWEF').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#DateToEdit').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#DateToDelete').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#onlyTime').datetimepicker({
        format: 'LT'
    });


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
    /*	 Entry Module User Dropdown Search End
	/*----------------------------------------------------*/


});

/* ----------------- End JS Document ----------------- */



