var SelectedItem = 1;
var SelectedName = "";

$(document).ready(function () {
    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#DeptAlloc').click(function () {
        $('#AddDepartmentAllocationForm').submit();
    });

    $('#Desig').click(function () {
        $('#AddDesignationAllocationForm').submit();
    });

    $('#btnAddRoleAllocation').click(function () {
        $('#RoleAllocationForm').validationEngine();
        $('#RoleAllocationForm').submit();
    });

    $('#btnUpdateRoleAllocation').click(function () {
        $('#UpdateRoleAllocationForm').validationEngine();
        $('#UpdateRoleAllocationForm').submit();
    });

    $('#txtCeaseDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#txtWithEffectFrom').bind('keypress', function (evt) {
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

    $('#Role').click(function () {
        $('#AddRoleForm').submit();
    });
    $('#btnDeleteYes').click(function () {
        $('#UpdateRoleAllocationForm').submit();
    });

    selectChange(SelectedItem);

    //$('#btnDesigCancel').click(function () {
    //    //$("#DesigName").val("");
    //});

    //$('#DesigName').bind('keypress', function (event) {
    //    if (window.event) {
    //        var charCode = window.event.keyCode;
    //    }
    //    else if (e) {
    //        var charCode = e.which;
    //    }
    //    else { return true; }
    //    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
    //        return true;
    //    else
    //        return false;
    //});

});

$(document).on('change', '#htmddl', function () {    
    var SelectedItem = $('#htmddl').val();
    var SelectedName = $("#htmddl").find('option:selected').text();
    var table = $('#tblRoleAllocationDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblRoleAllocationDetails').DataTable({
        "searching": true,
        "ordering": true,
        //"pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetRoleAllocationDetails',
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
                    if (json.data[i]["EffectFromDate"] != null) {
                        json.data[i]["EffectFromDate"] = moment(json.data[i]["EffectFromDate"]).format('DD/MM/YYYY');
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
            { "data": "RoleName" },
            { "data": "EffectFromDate" },
            { "data": "ToDate" },
            { "data": "CreatedBy" },
            //{ "data": "CreatedDate" },
            { "data": "ModifyBy" },
            { "data": "ModifyDate" },
            //{ "data": "LastAction" },
            { "data": "CeaseDate" },
            //{
            //    //"defaultContent": "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
            //    "orderable": false, "render": function (data, type, row) {
            //        if (row.LastAction == "Delete") {
            //            return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' disabled><span class='glyphicon glyphicon-pencil'></span></button>";

            //        }
            //        else {
            //            return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>";
            //        }
            //    }
            //},
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
    
    $("#tblRoleAllocationDetails_wrapper").find(">:first-child").hide();

    $('#tblRoleAllocationDetails tbody').on('click', 'button', function () {
        var data = table.row($(this).parents('tr')).data();
        $('#hdnRoleAllocationId').val(data["RoleAllocationId"]);
        $('#hdndate').val(data["EffectFromDate"]);
        $('#hdnEmpId').val(data["EmployeeId"]);
        $('#hdnRoleName').val(data["RoleName"]);
        $('#hdnRoleId').val(data["RoleId"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblRoleAllocationDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
};