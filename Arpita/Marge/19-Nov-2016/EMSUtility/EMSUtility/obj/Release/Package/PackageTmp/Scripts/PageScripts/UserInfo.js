
var SelectedItem = 1;
var SelectedName = "";

$(document).ready(function () {
       
    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#doj').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });

    $('#txtdob').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });
    $('#dob').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });

    $('#txtdoj').datetimepicker({
        defaultDate: new Date(),
        format: 'DD/MM/YYYY'
    });

    
    $('#txtdob').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#txtdoj').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

   

    $('#btnAddUserDetails').click(function () {
        $('#PersonalDetailsForm').validationEngine();
        $('#PersonalDetailsForm').submit();
    });

    $('#btnDeptCancel').click(function (){
        $('#txtFname').val('');
        $('#txtMname').val('');
        $('#txtLname').val('');
        $('#txtNo').val('');
        $('#txtUser').val('');

});
    $('#btnUpdateUserDetails').click(function () {
        $('#UpdatePersonalDetailsForm').validationEngine();
        $('#UpdatePersonalDetailsForm').submit();
    });
    
    $('#btnDeleteYes').click(function () {        
        $('#UpdatePersonalDetailsForm').submit();
    });

    selectChange(SelectedItem);

});



$(document).on('change', '#htmddl', function () {
    var SelectedItem = $('#htmddl').val();
    var SelectedName = $("#htmddl").find('option:selected').text();
    var table = $('#tblEmployeeDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblEmployeeDetails').DataTable({
        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetPersonalDetails',
             data: { activeId: SelectedItem },
            "dataSrc": function (json) {
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["DateOfBirth"] != null) {
                        json.data[i]["DateOfBirth"] = moment(json.data[i]["DateOfBirth"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["DateOfJoining"] != null) {
                        json.data[i]["DateOfJoining"] = moment(json.data[i]["DateOfJoining"]).format('DD/MM/YYYY');
                    }
                    //if (json.data[i]["CreatedDate"] != null) {
                    //    json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD/MM/YYYY');
                    //}
                    if (json.data[i]["ModifyDate"] != null) {
                        json.data[i]["ModifyDate"] = moment(json.data[i]["ModifyDate"]).format('DD/MM/YYYY ');
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
        }
        ,
        "columns": [
            { "data": "EmployeeName" },
            { "data": "Gender" },
            { "data": "DateOfBirth" },
            { "data": "ContactNo" },
            { "data": "DateOfJoining" },
            //{ "data": "TechId" },
            { "data": "TechName" },
            { "data": "CreatedBy" },
            //{ "data": "CreatedDate" },
            { "data": "ModifyBy" },
            { "data": "ModifyDate" },
            //{ "data": "LastAction" },
            {
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
                "orderable": false,
                "render": function (data, type, row) {
                    if (row.LastAction == "Delete") {
                        return "<button  class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' disabled><span class='glyphicon glyphicon-trash'></span></button>";
                    }
                    else {
                        return "<button class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>";
                    }
                }
            }
        ]
    });

    $("#tblEmployeeDetails_wrapper").find(">:first-child").hide();

    $('#tblEmployeeDetails tbody').on('click', 'button', function () {
        debugger;
        var data = table.row($(this).parents('tr')).data();
        //$('#hdnDeptAllocationId').val(data["DepartmentAllocationId"]);
        $('#hdnEmpId').val(data["EmployeeId"]);
        //$('#hdnTechId').val(data["TechId"]);
        $('#lblTechToEdit').val(data["TechId"]);
        $('#Fname').val(data["FirstName"]);
        $('#Mname').val(data["MiddleName"]);
        $('#Lname').val(data["LastName"]);
        $('#txtdob').val(data["DateOfBirth"]);
        $('#txtdoj').val(data["DateOfJoining"]);
        $('#lblContactNoToEdit').val(data["ContactNo"]);

        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblEmployeeDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
};