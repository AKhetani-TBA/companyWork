
var SelectedItem = 1;
var SelectedName = "";

$(document).ready(function () {

    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#lblWEFToEdit').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    $('#DeptAlloc').click(function () {
        $('#AddDepartmentAllocationForm').submit();
    });
    $('#Role').click(function () {
        $('#AddRoleAllocationForm').submit();
    });
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

    $('#txtCeaseDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#btnAddDesignationAllocation').click(function () {
        $('#AddDesignationAllocationForm').validationEngine();
        $('#AddDesignationAllocationForm').submit();
    });

    $('#btnUpdateDesignationAllocation').click(function () {
        $('#UpdateDesignationAllocationForm').validationEngine();
        $('#UpdateDesignationAllocationForm').submit();
    });

    $('#Desig').click(function () {
        $('#AddDesignationForm').submit();
    });
    $('#btnDeleteYes').click(function () {
        $('#UpdateDesignationAllocationForm').submit();
    });

    selectChange(SelectedItem);


});

$(document).on('change', '#htmddl', function () {    
    var SelectedItem = $('#htmddl').val();
    var SelectedName = $("#htmddl").find('option:selected').text();
    var table = $('#tblDesignationAllocationDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblDesignationAllocationDetails').DataTable({
        "searching": true,
        "ordering": true,
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetDesignationAllocation',
            data: { activeId: SelectedItem },
            "dataSrc": function (json) {                
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    //if (json.data[i]["CreatedDate"] != null) {
                    //    json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD-MM-YYYY HH:mm:ss');
                    //}
                    if (json.data[i]["CeaseDate"] != null) {
                        json.data[i]["CeaseDate"] = moment(json.data[i]["CeaseDate"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["ModifyDate"] != null) {
                        json.data[i]["ModifyDate"] = moment(json.data[i]["ModifyDate"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["WithEffectFrom"] != null) {
                        json.data[i]["WithEffectFrom"] = moment(json.data[i]["WithEffectFrom"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["ToDate"] != null) {
                        json.data[i]["ToDate"] = moment(json.data[i]["ToDate"]).format('DD/MM/YYYY');
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
            { "data": "DesignationName" },
            { "data": "WithEffectFrom" },
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

    $("#tblDesignationAllocationDetails_wrapper").find(">:first-child").hide();

    $('#tblDesignationAllocationDetails tbody').on('click', 'button', function () {        
        var data = table.row($(this).parents('tr')).data();
        $('#hdnDesigAllocationId').val(data["DesignationAllocationId"]);
        $('#hdnEmpId').val(data["EmployeeId"]);
        //$('#hdnDesigName').val(data["DesignationName"]);
        //$('#hdnDesigId').val(data["DesignationId"]);
        $('#lblWEFToEdit').val(data["WithEffectFrom"]);
        $('#lblDesigToEdit').val(data["DesignationId"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblDesignationAllocationDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });

};
