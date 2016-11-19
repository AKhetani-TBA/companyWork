var SelectedItem = 0;
var SelectedName = "";

$(document).ready(function () {

    //$('#createDate').datetimepicker({
    //    defaultDate: new Date(),
    //    format: 'MM/DD/YYYY'
    //});

    //$('#ceaseDate').datetimepicker({
    //    defaultDate: new Date(),
    //    format: 'MM/DD/YYYY'
    //});

    //$('#wef').datetimepicker({
    //    defaultDate: new Date(),
    //    format: 'MM/DD/YYYY'
    //});
    //$('#efd').datetimepicker({
    //    defaultDate: new Date(),
    //    format: 'MM/DD/YYYY'
    //});

    //$('#toDate').datetimepicker({
    //    defaultDate: new Date(),
    //    format: 'MM/DD/YYYY'
    //});

    //$("#AddDepartmentAllocationForm").validationEngine('attach', { promptPosition: "topLeft:70", scroll: false, maxErrorsPerField: 1 });

    //function DateFormat(field, rules, i, options) {
    //    var regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
    //    if (!regex.test(field.val())) {
    //        return "Please enter date in MM/DD/YYYY format."
    //    }
    //}

    $('#txtEffectFromDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#txtToDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#btnAddDepartmentAllocation').click(function () {
        $('#AddDepartmentAllocationForm').validationEngine();
        $('#AddDepartmentAllocationForm').submit();
    });

    $('#btnUpdateDepartmentAllocation').click(function () {
        $('#UpdateDepartmentAllocationForm').submit();
    });

    $('#Dept').click(function () {
        $('#AddDepartmentForm').submit();
    });

    $('#techmst').click(function () {
        $('#TechnologyForm').submit();
    });

    $('#btnDeleteYes').click(function () {
        $('#UpdateDepartmentAllocationForm').submit();
    });

    selectChange(SelectedItem);

});

$(document).on('change', '#htmddl', function () {
    var SelectedItem = $('#htmddl').val();
    var SelectedName = $("#htmddl").find('option:selected').text();
    var table = $('#tblDepartmentAllocationDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblDepartmentAllocationDetails').DataTable({

        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetDepartmentAllocation',
            "dataSrc": function (json) {
                debugger;
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    //if (json.data[i]["CreatedDate"] != null) {
                    //    json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD-MM-YYYY');
                    //}
                    if (json.data[i]["CeaseDate"] != null) {
                        json.data[i]["CeaseDate"] = moment(json.data[i]["CeaseDate"]).format('DD-MM-YYYY');
                    }
                    if (json.data[i]["ModifyDate"] != null) {
                        json.data[i]["ModifyDate"] = moment(json.data[i]["ModifyDate"]).format('DD-MM-YYYY ');
                    }
                    if (json.data[i]["EffectFromDate"] != null) {
                        json.data[i]["EffectFromDate"] = moment(json.data[i]["EffectFromDate"]).format('DD-MM-YYYY');
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
            { "data": "EmployeeName" },
            { "data": "DepartmentName" },
            { "data": "EffectFromDate" },
            { "data": "ToDate" },
            { "data": "CreatedBy" },
            //{ "data": "CreatedDate" },
            { "data": "ModifyBy" },
            { "data": "ModifyDate" },
            //{ "data": "LastAction" },
            { "data": "CeaseDate" },
            {
                //"defaultContent": "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
                "orderable": false, "render": function (data, type, row) {
                    if (row.LastAction == "Delete") {
                        return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' disabled><span class='glyphicon glyphicon-pencil'></span></button>";

                    }
                    else {
                        return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>";
                    }
                }
            },
            {
                //"defaultContent": "<button class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>",
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
        ]
    });

    //var htmlDD = ' 	<select id="htmddl"class="pull-left" style=" width: 80px; font-size:14px"> 		<option >Select</option> 	 <option value="0">All</option> 	<option value="1">Active</option> 		<option value="2">Delete</option>   	</select> ';
    //$(htmlDD).insertBefore("#tblDepartmentAllocationDetails_filter ");

    $('#tblDepartmentAllocationDetails tbody').on('click', 'button', function () {
        debugger;
        var data = table.row($(this).parents('tr')).data();
        $('#hdnDeptAllocationId').val(data["DepartmentAllocationId"]);
        $('#hdnEmpId').val(data["EmployeeId"]);
        $('#hdnDeptName').val(data["DepartmentName"]);
        $('#hdnDeptId').val(data["DeptId"]);
        if ($(this).attr("id") == "btnEdit") {

            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
            //else if ($(this).attr("id") == "btnDelete") {
            //    $('#txtWEFDate').val(data["EffectFromDate"]);
            //    $('#delete').modal('show');
            //}
        else {
            $('#delete').modal('show');
        }
        return false;
    });
};