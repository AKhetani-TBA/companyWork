
$(document).ready(function () {

    $('#ceaseDate').datetimepicker({
        defaultDate: new Date(),
        format: 'MM/DD/YYYY'
    });

    $('#wef').datetimepicker({
        defaultDate: new Date(),
        format: 'MM/DD/YYYY'
    });

    $('#toDate').datetimepicker({
        defaultDate: new Date(),
        format: 'MM/DD/YYYY'
    });


    $('#btnAddDesignationAllocation').click(function () {
        $('#AddDesignationAllocationForm').submit();
    });

    $('#btnUpdateDesignationAllocation').click(function () {
        $('#UpdateDesignationAllocationForm').submit();
    });


    $('#Desig').click(function () {
        $('#AddDesignationForm').submit();
    });
    $('#btnDeleteYes').click(function () {
        $('#UpdateDesignationAllocationForm').submit();
    });

    var table = $('#tblDesignationAllocationDetails').DataTable({
        "searching": true,
        "ordering": true,
        //"pagingType": "full_numbers",
        "scrollY": "250px",
        "scrollCollapse": true,
        "paging": false,
        "ajax": {
            "url": 'GetDesignationAllocation',
            "dataSrc": function (json) {
                debugger;
                for (var i = 0, len = json.data.length ; i < len ; i++) {
                    if (json.data[i]["CreatedDate"] != null) {
                        json.data[i]["CreatedDate"] = moment(json.data[i]["CreatedDate"]).format('DD-MM-YYYY HH:mm:ss');
                    }
                    if (json.data[i]["CeaseDate"] != null) {
                        json.data[i]["CeaseDate"] = moment(json.data[i]["CeaseDate"]).format('DD-MM-YYYY HH:mm:ss');
                    }
                    if (json.data[i]["ModifyDate"] != null) {
                        json.data[i]["ModifyDate"] = moment(json.data[i]["ModifyDate"]).format('DD-MM-YYYY HH:mm:ss');
                    }
                    if (json.data[i]["WithEffectFrom"] != null) {
                        json.data[i]["WithEffectFrom"] = moment(json.data[i]["WithEffectFrom"]).format('DD-MM-YYYY HH:mm:ss');
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
            { "data": "CreatedDate" },
            { "data": "ModifyBy" },
            { "data": "ModifyDate" },
            { "data": "LastAction" },
            { "data": "CeaseDate" },
            {
                "defaultContent": "<button class='btn btn-primary btn-xs' id='btnEdit' data-title='Edit' data-toggle='modal' data-target='#edit' ><span class='glyphicon glyphicon-pencil'></span></button>",
                "orderable": false
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


    $('#tblDesignationDetails tbody').on('click', 'button', function () {
        var data = table.row($(this).parents('tr')).data();
        $('#hdnDesigAllocationId').val(data["DesigAllocationId"]);
        if ($(this).attr("id") == "btnEdit") {
            $('#txtCeaseDate').val(data["CeaseDate"]);
            $('#edit').modal('show');
        }
        else {
            $('#delete').modal('show');
        }
        return false;
    });

});


