var SelectedItem = 1;
var SelectedName = "";

$(document).ready(function () {
    
    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#txtCreateDate').bind('keypress', function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31
         && (charCode < 48 || charCode > 57))
            return false;
        return true;
    });

    $('#TechName').bind('keydown', function (e) {
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

    $('#btnAddTechnology').click(function () {
        $('#AddTechnologyForm').validationEngine();
        $('#AddTechnologyForm').submit();
    });

    $('#btnUpdateTechnology').click(function () {
        $('#UpdateTechnologyForm').validationEngine();
        $('#UpdateTechnologyForm').submit();
    });

    $('#DeptAlloc').click(function () {
        $('#DepartmentAllocationForm').submit();
    });

    $('#Dept').click(function () {
        $('#AddDepartmentForm').submit();
    });

    $('#role').click(function () {
        $('#AddRoleForm').submit();
    });

    $('#desig').click(function () {
        $('#AddDesignationForm').submit();
    });

    $('#lblTechNameToEdit').bind('keypress', function (e) {
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

    $('#btnUpdateTechnology').click(function () {
        $('#UpdateTechnologyForm').submit();
    });

    $('#btnDeleteYes').click(function () {
        $('#UpdateTechnologyForm').submit();
    });

    $('#btnTechCancel').click(function () {
        $("#TechName").val("");
    });

    selectChange(SelectedItem);

    //$('#DeptName').bind('keypress', function (e) {

    //    var charCode = (e.which) ? e.which : e.keyCode;
    //    if (charCode != 43 && charCode != 46 && charCode != 45 && charCode > 31
    //     && (charCode < 48 || charCode > 57))
    //        return false;
    //    return true;

    //    //if ((charCode >= 48 && charCode <= 57 && charCode == 43) ||
    //    //   (charCode >= 65 && charCode <= 90) ||
    //    //    (charCode >= 97 && charCode <= 122))
    //    //    return true;
    //    //return false;
    //});

    //$('#txtCreateDate').bind('keypress', function (evt) {        
    //    evt = (evt) ? evt : window.event;
    //    var charCode = (evt.which) ? evt.which : evt.keyCode;        
    //    if (charCode != 46 && charCode != 45 && charCode != 47 && charCode > 31 
    //     && (charCode < 48 || charCode > 57))
    //        return false;
    //    return true; 
    //});

    //$('#tblDepartmentDetails tbody').on('click', 'button', function () {
    //    debugger;
    //    var data = table.row($(this).parents('tr')).data();
    //    $('#hdnDeptId').val(data["DeptId"]);
    //    $('#lblDeptNameToEdit').val(data["DeptName"]);
    //    if ($(this).attr("id") == "btnEdit") {
    //        $('#txtCeaseDate').val(data["CeaseDate"]);
    //        $('#edit').modal('show');
    //    }
    //    else {
    //        $('#delete').modal('show');
    //    }
    //    return false;
    //});

});

$(document).on('change', '#htmddl', function () {
    var SelectedItem = $(this).val();
    var SelectedName = $(this).find('option:selected').text();
    var table = $('#tblTechnology').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});

function selectChange(SelectedItem) {
    var table = $('#tblTechnology').DataTable({
        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetTechnology',
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
            { "data": "TechName" },
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
            {
                // "defaultContent": "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
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

                    }
                    else {
                        return "<button class='btn btn-danger btn-xs' id='btnDelete' data-title='Delete' data-toggle='modal' data-target='#delete' ><span class='glyphicon glyphicon-trash'></span></button>";
                    }
                }
            }
        ],
    });
    $("#tblTechnology_wrapper").find(">:first-child").hide();

    $('#tblTechnology tbody').on('click', 'button', function () {
        var data = table.row($(this).parents('tr')).data();
        $('#hdnTechId').val(data["TechId"]);
        $('#lblTechNameToEdit').val(data["TechName"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblTechnology').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
}



