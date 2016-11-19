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
	
    $('#birthDate').datetimepicker({
     format: 'L'
    });
	
	$('#onlyTime').datetimepicker({
     format: 'LT'
    });

	
});





/* ----------------- End JS Document ----------------- */



