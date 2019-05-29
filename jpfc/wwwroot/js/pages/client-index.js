var Jpfc = Jpfc || {};

Jpfc.ClientIndex = function () {
    var loadingPriceSpinner = null;
    var table;

    var initDatePickers = function () {
        $('#FilterStartDate, #FilterEndDate').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy",
            clearBtn: true,
            todayBtn: 'linked'
        }).on('change', function () {
            table.ajax.reload();
        });
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#client-table').on('preXhr.dt', function () {
            loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('client-table'));
        }).DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2],
                "orderable": false
            }],
            "ajax": {
                "url": "/Client/GetClientList",
                "method": "get",
                "data": function (d) {
                    return {
                        startDate: $('#FilterStartDate').val(),
                        endDate: $('#FilterEndDate').val()
                    };
                },
                "dataSrc": function (d) {
                    if (loadingPriceSpinner !== null) {
                        loadingPriceSpinner.stop();
                    }
                    return d;
                }
            },
            "columns": [
                {
                    data: "dateStr"
                },
                {
                    data: "name"
                },
                {
                    data: "referenceNumber"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary" title="Edit Client" href="/Client/EditClient/' + row.clientId + '"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger" title = "Delete Client"  onclick="Jpfc.ClientIndex.deleteClient(' + row.clientId + ')"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var deleteClient = function (id) {
        swal({
            title: 'Are you sure?',
            text: "Once deleted, you will not be able to recover this data.",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#337ab7',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    "url": "/Client/DeleteClient",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        table.ajax.reload(null, false);
                        toastr.success('Client deleted successfully', '', Jpfc.Toastr.config);
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function (jqXhr, textStatus) {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var init = function () {
        initDataTable();
        initDatePickers();
    };

    return {
        init: init,
        deleteClient: deleteClient
    };
}();