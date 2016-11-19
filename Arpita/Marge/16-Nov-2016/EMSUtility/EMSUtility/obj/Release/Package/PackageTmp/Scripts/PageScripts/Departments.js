var SelectedItem = 1;
var SelectedName = "";
$(document).ready(function () {
    
    $('#txtSearch').keydown(function () {
        if (event.keyCode == 13) {
            event.preventDefault();
        }
    });

    $('#btnAddDepartment').click(function () {
        $('#AddDepartmentForm').validationEngine();
        $('#AddDepartmentForm').submit();
    });

    //$('#DeptAlloc').click(function () {        
    //    $('#DepartmentAllocationForm').submit();
    //});
    
    $('#Desig').click(function () {
        $('#AddDesignationForm').submit();
    });

    $('#Role').click(function () {
        $('#AddRoleForm').submit();
    });

    $('#techmst').click(function () {
        $('#TechnologyForm').submit();
    });
    
    $('#btnUpdateDepartment').click(function () {
        $('#UpdateDepartmentForm').validationEngine();
        $('#UpdateDepartmentForm').submit();
    });

    $('#btnDeleteYes').click(function () {
        $('#UpdateDepartmentForm').submit();
    });

    $('#btnDeptCancel').click(function () {
        $("#DeptName").val("");
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

    $('#DeptName').bind('keydown', function (e) {        
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

    $('#lblDeptNameToEdit').bind('keypress', function (e) {
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
});

$(document).on('change', '#htmddl', function () {    
    var SelectedItem = $(this).val();
    var SelectedName = $(this).find('option:selected').text();
    var table = $('#tblDepartmentDetails').DataTable();
    table.destroy();
    selectChange(SelectedItem);
});


function selectChange(SelectedItem) {    

    var table = $('#tblDepartmentDetails').DataTable({
        "searching": true,
        "ordering": true,
        "pagingType": "full_numbers",
        "scrollY": "500px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetDepartment',
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
            { "data": "DeptName" },    
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
      
    $("#tblDepartmentDetails_wrapper").find(">:first-child").hide();

    $('#tblDepartmentDetails tbody').on('click', 'button', function () {
        var data = table.row($(this).parents('tr')).data();
        $('#hdnDeptId').val(data["DeptId"]);
        $('#lblDeptNameToEdit').val(data["DeptName"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

    var table = $('#tblDepartmentDetails').DataTable();
    $('#txtSearch').on('keyup', function () {
        table.search(this.value).draw();
    });
}




