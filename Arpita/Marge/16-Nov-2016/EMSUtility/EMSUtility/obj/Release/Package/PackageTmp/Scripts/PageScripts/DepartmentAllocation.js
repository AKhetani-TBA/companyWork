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

    $('#Desig').click(function () {
        $('#AddDesignationAllocationForm').submit();
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

    $('#btnAddDepartmentAllocation').click(function () {
        $('#AddDepartmentAllocationForm').validationEngine();
        $('#AddDepartmentAllocationForm').submit();
    });

    $('#btnUpdateDepartmentAllocation').click(function () {
        $('#UpdateDepartmentAllocationForm').validationEngine();
        $('#UpdateDepartmentAllocationForm').submit();
    });

    $('#txtCeaseDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });
    //$('#Dept').click(function () {
    //    $('#AddDepartmentForm').submit();
    //});

    //$('#techmst').click(function () {
    //    $('#TechnologyForm').submit();
    //});

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
            data: { activeId: SelectedItem },
            "dataSrc": function (json) {
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    //if (json.data[i]["CreatedDate"] != null) {
                    //    json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD-MM-YYYY');
                    //}
                    if (json.data[i]["CeaseDate"] != null) {
                        json.data[i]["CeaseDate"] = moment(json.data[i]["CeaseDate"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["ModifyDate"] != null) {
                        json.data[i]["ModifyDate"] = moment(json.data[i]["ModifyDate"]).format('DD/MM/YYYY ');
                    }
                    if (json.data[i]["EffectFromDate"] != null) {
                        json.data[i]["EffectFromDate"] = moment(json.data[i]["EffectFromDate"]).format('DD/MM/YYYY');
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
               "orderable": false,
               "render": function (data, type, row) {
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

    $("#tblDepartmentAllocationDetails_wrapper").find(">:first-child").hide();

    $('#tblDepartmentAllocationDetails tbody').on('click', 'button', function () {        
        var data = table.row($(this).parents('tr')).data();
        $('#hdnDeptAllocationId').val(data["DepartmentAllocationId"]);
        $('#hdnEmpId').val(data["EmployeeId"]);
        //$('#hdndate').val(data["EffectFromDate"]);
        //$('#hdnDeptName').val(data["DepartmentName"]);
        //$('#hdnDeptId').val(data["DepartmentId"]);
        //$('#hdnWEF').val(data["EffectFromDate"]);
        $('#lblWEFToEdit').val(data["EffectFromDate"]);
        $('#lblDeptToEdit').val(data["DepartmentId"]);
        
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblDepartmentAllocationDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
};

