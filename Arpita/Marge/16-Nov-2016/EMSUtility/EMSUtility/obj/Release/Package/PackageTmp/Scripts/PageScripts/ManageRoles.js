
var SelectedItem = 1;
var SelectedName = "";

$(document).ready(function () {

    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#btnAddRole').click(function () {
        $('#AddRoleForm').validationEngine();
        $('#AddRoleForm').submit();
    });

    $('#btnUpdateRole').click(function () {
        $('#UpdateAddRoleForm').validationEngine();
        $('#UpdateAddRoleForm').submit();
    });
    //$('#DeptAlloc').click(function () {        
    //    $('#DepartmentAllocationForm').submit();
    //});

    $('#Department').click(function () {
        $('#AddDepartmentForm').submit();
    });

    $('#desig').click(function () {
        $('#AddDesignationForm').submit();
    });

    $('#techmst').click(function () {
        $('#TechnologyForm').submit();
    });

    $('#btnUpdateRole').click(function () {
        $('#UpdateRoleForm').submit();
    });

    $('#btnDeleteYes').click(function () {
        $('#UpdateRoleForm').submit();
    });

    $('#btnRoleCancel').click(function () {
        $("#RoleName").val("");
    });

    $('#lblRoleNameToEdit').bind('keypress', function (e) {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
            return true;
        else
            return false;
    });

    $('#txtCeaseDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    selectChange(SelectedItem);

    $('#txtCreateDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#RoleName').bind('keydown', function (e) {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode == 8 || charCode == 9 || charCode == 37 || charCode == 38 || charCode == 39 || charCode == 40 || charCode == 46) ||
            (charCode > 64 && charCode < 91))
            return true;
        else
            return false;
    });
});

$(document).on('change', '#htmddl', function () {
    var SelectedItem = $('#htmddl').val();
    var SelectedName = $("#htmddl").find('option:selected').text();
    var table = $('#tblRoleDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblRoleDetails').DataTable({
        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetRoles',
            data: { activeId: SelectedItem },
            "dataSrc": function (json) {
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["CreatedDate"] != null) {
                        json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD/MM/YYYY');
                    }
                    if (json.data[i]["CeaseDate"] != null) {
                        json.data[i]["CeaseDate"] = moment(json.data[i]["CeaseDate"]).format('DD/MM/YYYY ');
                    }
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
        },
        "columns": [
            { "data": "RoleName" },
            { "data": "CreatedBy" },
            { "data": "CreatedDate" },
            { "data": "ModifyBy" },
            { "data": "ModifyDate" },
            { "data": "CeaseDate" },
            //{ "width": "16%", "data": "DeptName" },
            //{ "width": "11%", "data": "CreatedBy" },
            //{ "width": "11%", "data": "CreatedDate" },
            //{ "width": "11%", "data": "ModifyBy" },
            //{ "width": "12%", "data": "ModifyDate" },
            //{ "width": "12%", "data": "CeaseDate" },
            //{
            //    "orderable": false,
            //    "render": function (data, type, row) {
            //        if (row.LastAction == "Delete") {
            //            return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' disabled><span class='glyphicon glyphicon-pencil'></span></button>";

            //        }
            //        else {
            //            return "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>";
            //        }
            //    }
            //},
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
        ],
    });

    $("#tblRoleDetails_wrapper").find(">:first-child").hide();

    $('#tblRoleDetails tbody').on('click', 'button', function () {

        var data = table.row($(this).parents('tr')).data();
        $('#hdnRoleId').val(data["RoleId"]);
        $('#lblRoleNameToEdit').val(data["RoleName"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblRoleDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
}



