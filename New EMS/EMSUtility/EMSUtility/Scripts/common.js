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
	
	//$('#birthDate').datetimepicker();
	
	
	$('#onlyTime').datetimepicker({
     format: 'LT'
    });

    /*----------------------------------------------------*/
    /*	 add row and delete row
	/*----------------------------------------------------*/

	var i = 1;
	$("#add_row").click(function () {
	    $('#addr' + i).html("<td>" + (i + 1) + "</td><td><div class='input-group date' id='Inv_Date" + i + "'><input type='text' class='form-control' /><span class='input-group-addon'><i class='fa fa-calendar'></i></span></div> </td><td><input type='text' name='Inv_Amt" + i + "' placeholder='Invoice Amount' class='form-control' /></td><td><input type='text' name='Remarks'" + i + "' placeholder='Remarks' class='form-control' /></td>");
	    $('#tab_logic').append('<tr id="addr' + (i + 1) + '"></tr>');
	    $('#Inv_Date' + i).datetimepicker({ format: 'L' });
	    i++;

	});
	$("#delete_row").click(function () {
	    if (i > 1) {
	        $("#addr" + (i - 1)).html('');
	        i--;
	    }
	});

});





/* ----------------- End JS Document ----------------- */



