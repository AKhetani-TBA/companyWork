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

    //$('#hireDate').datetimepicker();

    $('#txtBirthDate').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });
	
    $('#birthDate').datetimepicker({        
        format: 'DD/MM/YYYY'    
    });
	
    $('#ceaseDate').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });

    $('#efd').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });
	
    $('#onlyTime').datetimepicker({
        format: 'DD/MM/YYYY'
    });
	
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
	
    $('#joiningDate').datetimepicker({        
        format: 'DD/MM/YYYY'
    });
    
    $('.datepicker').datetimepicker({
        format: 'DD/MM/YYYY',
        startDate: '-3d'
    });

    $('#wef').datetimepicker({
        format: 'DD/MM/YYYY',        
    });

    $('#txtEffectFromDate').datetimepicker({
        format: 'DD/MM/YYYY',        
    });

    $('#txtWithEffectFrom').datetimepicker({
        format: 'DD/MM/YYYY'
    });
	
    $('#toDate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
	
    $('#txtToDate').datetimepicker({
        format: 'DD/MM/YYYY',        
    });
	
    $('#createDate').datetimepicker({        
        format: 'DD/MM/YYYY',
    });
    $('#txtCreateDate').datetimepicker({        
        format: 'DD/MM/YYYY',
    });
	
    $('#txtCeaseDate').datetimepicker({        
        format: 'DD/MM/YYYY',
    });

    $('#doJ').datetimepicker({        
        format: 'DD/MM/YYYY',
    });

    $('#txtDoj').datetimepicker({  
        format: 'DD/MM/YYYY',
    });

//    $('#hireDate').datetimepicker();

////    $('#birthDate').datetimepicker();

//    $('#DateToWEF').datetimepicker({
//        format: 'DD/MM/YYYY'
//    });

//    $('#DateToEdit').datetimepicker({
//        format: 'DD/MM/YYYY'
//    });

//    $('#DateToDelete').datetimepicker({
//        format: 'DD/MM/YYYY'
//    });

//    $('#onlyTime').datetimepicker({
//        format: 'LT'
//    });

//    $('#birthDate').datetimepicker({
//        //format: 'L',
//        format: 'DD/MM/YYYY'    
//    });

//    $('#txtBirthDate').datetimepicker({
//        defaultDate: new Date(),
//        format: 'DD/MM/YYYY'
//    });

//    $('#ceaseDate').datetimepicker({
//        defaultDate: new Date(),
//        format: 'DD/MM/YYYY'
//    });

//    $('#efd').datetimepicker({
//        defaultDate: new Date(),
//        format: 'DD/MM/YYYY'
//    });

//    //$('#toDate').datetimepicker({
//    //    defaultDate: new Date(),
//    //    format: 'MM/DD/YYYY',
//    //    defaultOption:null
//    //});

//    $('#onlyTime').datetimepicker({
//        //format: 'LT'
//        format: 'DD/MM/YYYY'
//    });

//    $('#joiningDate').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY'
//    });

//    $('.datepicker').datetimepicker({
//        format: 'DD/MM/YYYY',
//        startDate: '-3d'
//    });
//    $('#wef').datetimepicker({
//        format: 'DD/MM/YYYY',
//        //format: 'L'
//    });

//    $('#txtEffectFromDate').datetimepicker({
//        format: 'DD/MM/YYYY',
//        //format: 'L'
//    });

//    $('#toDate').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY'
//    });
//    $('#txtToDate').datetimepicker({
//        format: 'DD/MM/YYYY',
//        //format: 'L'
//    });

//    $('#createDate').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY',
//    });
//    $('#txtCreateDate').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY',
//    });

//    $('#txtCeaseDate').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY',
//    });

//    $('#doJ').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY',
//    });

//    $('#txtDoj').datetimepicker({
//        //format: 'L'
//        format: 'DD/MM/YYYY',
//    });

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



